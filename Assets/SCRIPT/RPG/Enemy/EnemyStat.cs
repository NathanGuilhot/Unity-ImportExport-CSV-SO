using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NightenUtils;
using static NightenUtils.ChainDel;

/// Dependencies:
/// - GameManager (FX, GAMESTATE, Inventory)
/// - GameEvent (Notification)

public class EnemyStat : MonoBehaviour, IPotionTarget
{
    public new string name { get; private set; }
    [field: SerializeField] int PV;
    int PV_Max;
    [field: SerializeField] int Attack;
    [SerializeField] ItemSO[] _loot;

    EnemyInventory _inventory;
    public GAMESTATE ActiveState { get; } = GAMESTATE.ENEMY_TURN;

    [SerializeField] LifeBar _lifeBar;
    [SerializeField] Animator _anim;

    private bool isAlive = true;
    private bool inAction = false;
    Action<GameObject> _OnDestroyed;

    [SerializeField] Renderer _ImageRenderer;

    //Events
    public List<Func<int, int>> CheckDamage { get; set; } = new List<Func<int, int>>();
    public List<Func<int, int>> CheckAttack { get; set; } = new List<Func<int, int>>();
    public Func<bool> CanAttack { get; set; } = () => true;
    public Action PerformOnAttack { get; set; }

    private void OnEnable()
    {
        GameEvent.OnEnemyReceivePotion += ReceivePotion;
        GameEvent.OnEnemyAttacked += GetDamage;
    }

    private void OnDisable()
    {
        GameEvent.OnEnemyReceivePotion -= ReceivePotion;
        GameEvent.OnEnemyAttacked -= GetDamage;
    }
    private void ReceivePotion(ItemSO pPotion)
    {
        _PotionEffect.Perform(this, pPotion);
    }

    public void Init(EnemySO pData, Action<GameObject> pOnDestroyed)
    {
        name = pData.name;
        PV_Max = pData.PV;
        PV = pData.PV;
        Attack = pData.Attack;
        _loot = pData.loot;

        if (pData.inventory.Length > 0)
        {
            _inventory = gameObject.AddComponent<EnemyInventory>();
            _inventory.Init(pData.inventory);
        }

        _OnDestroyed = pOnDestroyed;
    }

    public void GetDamage(int pAmount)
    {
        pAmount = ChainDelegate<int>(pAmount, CheckDamage);

        StartCoroutine(DamageEffect(pAmount));
        GameManager.FX.DisplayDamage(pAmount, transform.position + new Vector3(4f, 0f, -0.5f));

        PV = Mathf.Max(PV - pAmount, 0);
        _lifeBar.SetValue((float)PV / (float)PV_Max);

        if (PV > 0)
        {
            if (!inAction)
                _anim.Play("EnemyDamage 0");
        }
        else
        {
            StartCoroutine(OnDead());
        }
    }

    IEnumerator DamageEffect(int pAmount)
    {
        SetShaderHit(true);
        yield return new WaitForSeconds((float)pAmount / 100f);
        SetShaderHit(false);
    }


    private void Update()
    {
        if (isAlive && !inAction)
        {
            if (GameManager.GameState == ActiveState)
            {
                PerformAttack();
            }

        }
    }
    public void PerformAttack()
    {
        inAction = true;
        Debug.Log("Enemy's turn!");

        bool useInventoryItem = false;
        if (_inventory != null)
        {
            if (_inventory.GetInventoryCount() >= 0)
            {
                NightenUtils.KeyValuePair<ItemSO, int> RandomItem;
                (useInventoryItem, RandomItem) = _inventory.GetRandomElement();

                if (useInventoryItem)
                    StartCoroutine(UseItem(RandomItem.Key));
            }
        }

        if (!useInventoryItem)
            StartCoroutine(OnAttack());
        
    }

    private IEnumerator UseItem(ItemSO pItem)
    {
        yield return new WaitForSeconds(0.5f);
        GameEvent.NotificationEvent($"{name} use {pItem.name}");
        _anim.Play("EnemyThrow 0");
        yield return new WaitForSeconds(0.5f);
        GameEvent.ThrowPotionEvent(pItem, true);
        PerformOnAttack?.Invoke();

        yield return new WaitForSeconds(0.5f);
        _inventory.RemoveItem(pItem);

        if (isAlive)
        {
            yield return new WaitForSeconds(0.3f);
            OnAttackEnded();
        }
    }

    private IEnumerator OnAttack()
    {
        if ((bool)!CanAttack?.Invoke())
        {
            Debug.Log("Can't attack");
            OnAttackEnded();
            yield break;
        }
        _anim.SetBool("attack", true);
        yield return new WaitForSeconds(0.4f);
        //NOTE(Nighten) We switch this bool quickly so we can be sure the animation is played at exit time of the spawn
        //              Playing the animation directly doesn't allow that
        _anim.SetBool("attack", false);

        yield return new WaitForSeconds(0.1f);
        PerformOnAttack?.Invoke();
        //GameManager.Player.GetDamage(ChainDelegate<int>(Attack, CheckAttack));
        GameEvent.AttackPlayer(ChainDelegate<int>(Attack, CheckAttack));

        if (isAlive) { 
            yield return new WaitForSeconds(0.3f);
            OnAttackEnded();
        }
        
    }

    public void OnAttackEnded()
    {
        inAction = false;
        GameManager.TurnEnded();
    }

    void ItemDrop(ItemSO[] pLoot)
    {
        bool success = true;
        foreach (ItemSO loot in pLoot)
        {
            success = success ? GameManager.Inventory.AddItem(loot) : false;
            GameObject LootDropped = Instantiate(loot.prefab);
            LootDropped.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            LootDropped.AddComponent<AnimationAutoDestroy>();
        }

        if (!success)
        {
            GameEvent.NotificationEvent("Inventory is full!");
        }
    }
    
    private IEnumerator OnDead()
    {
        isAlive = false;
        _anim.SetBool("alive", false);
        if (_loot != null)
            ItemDrop(_loot);
        yield return new WaitForSeconds(0.5f);
        _OnDestroyed?.Invoke(this.gameObject);
    }
    public void Heal(int pAmount)
    {
        PV = Mathf.Min(PV + pAmount, PV_Max);
        _lifeBar.SetValue((float)PV / (float)PV_Max);

        GameEvent.NotificationEvent($"The {name} is healing!");
    }

    private void SetShaderHit(bool pHit)
    {
        _ImageRenderer.material.SetFloat("_Hit", pHit ? 1 : 0);
    }
}