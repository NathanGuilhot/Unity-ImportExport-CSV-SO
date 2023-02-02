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
    [SerializeField] new string name;
    [field: SerializeField] int PV;
    int PV_Max;
    [field: SerializeField] int Attack;
    [SerializeField] ItemSO _loot;

    [SerializeField] LifeBar _lifeBar;
    [SerializeField] Animator _anim;

    private bool isAlive = true;
    private bool inAction = false;
    Action<GameObject> _OnDestroyed;

    //Events
    public List<Func<int, int>> CheckDamage { get; set; } = new List<Func<int, int>>();
    public List<Func<int, int>> CheckAttack { get; set; } = new List<Func<int, int>>();
    public _CheckCondition CanAttack { get; set; } = () => true;
    public Action PerformOnAttack { get; set; }

    public void Init(EnemySO pData, Action<GameObject> pOnDestroyed)
    {
        name = pData.name;
        PV_Max = pData.PV;
        PV = pData.PV;
        Attack = pData.Attack;
        _loot = pData.loot;

        _OnDestroyed = pOnDestroyed;
    }

    public void GetDamage(int pAmount)
    {
        pAmount = ChainDelegate<int>(pAmount, CheckDamage);

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

    public void PerformAttack()
    {
        inAction = true;
        Debug.Log("Enemy's turn!");
        StartCoroutine(OnAttack());
    }

    private void Update()
    {
        if (isAlive && !inAction)
        {
            if (GameManager.GameState == GAMESTATE.ENEMY_TURN)
            {
                PerformAttack();
            }

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
        yield return new WaitForSeconds(0.5f);
        //NOTE(Nighten) We switch this bool quickly so we can be sure the animation is played at exit time of the spawn
        //              Playing the animation directly doesn't allow that
        _anim.SetBool("attack", false);

        yield return new WaitForSeconds(0.3f);
        PerformOnAttack?.Invoke();
        GameManager.Player.GetDamage(ChainDelegate<int>(Attack, CheckAttack));

        yield return new WaitForSeconds(0.3f);
        OnAttackEnded();
    }

    public void OnAttackEnded()
    {
        inAction = false;
        GameManager.TurnEnded();
    }

    void ItemDrop(ItemSO pLoot)
    {
        bool success = GameManager.Inventory.AddItem(pLoot);
        GameObject LootDropped = Instantiate(pLoot.prefab);
        LootDropped.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        LootDropped.AddComponent<AnimationAutoDestroy>();
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

        GameEvent.NotificationEvent("The enemy is healing!");
    }
}




