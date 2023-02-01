using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NightenUtils;
using static NightenUtils.ChainDelegate;

public class EnemyStat : MonoBehaviour, IPotionTarget
{
    [SerializeField] new string name;
    [field: SerializeField] public int PV { get; set; }
    int PV_Max;
    [field: SerializeField] public int Attack { get; set; }
    [SerializeField] ItemSO loot;

    [SerializeField] LifeBar _lifeBar;

    public Action<GameObject> OnDestroyed;

    private Animator anim;
    private bool isAlive = true;
    private bool inAction = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(EnemySO pData)
    {
        name = pData.name;
        PV_Max = pData.PV;
        PV = pData.PV;
        Attack = pData.Attack;
        loot = pData.loot;
    }

    public void GetDamage(uint pAmount)
    {
        pAmount = (uint)ChainIntDelegate((int)pAmount, CheckDamage);

        GameManager.FX.DisplayDamage((int)pAmount, transform.position + new Vector3(4f, 0f, -0.5f));

        PV = Mathf.Max(PV - (int)pAmount, 0);
        _lifeBar.SetValue((float)PV / (float)PV_Max);

        if (PV <= 0)
        {
            StartCoroutine(OnDead());
        }
        else
        {
            if (!inAction)
                anim.Play("EnemyDamage 0");
        }
    }

    public void PerformAttack(uint pAmount)
    {
        inAction = true;
        Debug.Log("Enemy's turn!");
        StartCoroutine(OnAttack());
    }

    private void Update()
    {
        if (isAlive && !inAction)
        {
            if (GameManager.GameState == GameManager.GAMESTATE.ENEMY_TURN)
            {
                PerformAttack((uint)this.Attack);
            }

        }
    }

    private IEnumerator OnDead()
    {
        isAlive = false;
        anim.SetBool("alive", false);
        if (loot != null)
            ItemDrop(loot);
        yield return new WaitForSeconds(0.5f);
        OnDestroyed?.Invoke(this.gameObject);
    }
    private IEnumerator OnAttack()
    {
        if ((bool)!CanAttack?.Invoke())
        {
            Debug.Log("Can't attack");
            OnAttackEnded();
            yield break;
        }
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(0.5f);
        //NOTE(Nighten) We switch this bool quickly so we can be sure the animation is played at exit time of the spawn
        //              Playing the animation directly doesn't allow that
        anim.SetBool("attack", false);

        yield return new WaitForSeconds(0.3f);
        PerformOnAttack?.Invoke();
        GameManager.Player.GetDamage((uint)ChainIntDelegate(Attack, CheckAttack));

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
    
    public void Heal(int pAmount)
    {
        PV = Mathf.Min(PV + pAmount, PV_Max);
        _lifeBar.SetValue((float)PV / (float)PV_Max);

        GameEvent.NotificationEvent("The enemy is healing!");
    }

    //---
    //Events
    public List<_ProcessInt> CheckDamage { get; set; } = new List<_ProcessInt>();
    public List<_ProcessInt> CheckAttack { get; set; } = new List<_ProcessInt>();
    public _CheckCondition CanAttack { get; set; }  = () => true;
    public Action PerformOnAttack { get; set; }
}




