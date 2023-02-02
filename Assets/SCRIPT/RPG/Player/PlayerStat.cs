using static NightenUtils.ChainDel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStat : MonoBehaviour, IPotionTarget
{
    [SerializeField] int PV;
    [SerializeField] int PV_Max;

    [SerializeField] PlayerEquip _equipment;
    public new string name { get; private set; } = "Player";
    public GAMESTATE ActiveState { get; }  = GAMESTATE.PLAYER_TURN;
    bool _inAction = false;


    public List<Func<int, int>> CheckDamage { get; set; } = new List<Func<int, int>>();
    public List<Func<int, int>> CheckAttack { get; set; } = new List<Func<int, int>>();
    public Func<bool> CanAttack { get; set; } = () => true;
    public Action PerformOnAttack { get; set; }



    public void GetDamage(int pAmount)
    {
        pAmount = ChainDelegate<int>(pAmount, CheckDamage);

        GameManager.FX.DisplayDamage(pAmount, Camera.main.transform.position + new Vector3(10f, 0f, 4f));

        PV = Mathf.Max(PV - pAmount, 0);
        PlayerEvent.UpdateLife(PV, PV_Max);

        if (PV > 0)
        {
            return;
        }
        OnDead();
    }

    void OnDead()
    {
        Debug.Log("You're dead :(");
    }

    public void Heal(int pAmount)
    {
        PV = Mathf.Min(PV + pAmount, PV_Max);
        PlayerEvent.UpdateLife(PV, PV_Max);
        GameEvent.NotificationEvent("You're healing!");
    }

    private void Update()
    {
        if (GameManager.GameState == ActiveState) { 
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !_inAction)
            {
                StartCoroutine(PerformAttack());
            }
        }
    }

    IEnumerator PerformAttack()
    {
        _inAction = true;
        if ((bool)CanAttack?.Invoke())
        {
            GameManager.FX.AddSlash(new Vector3(0f,0f,0f));

            yield return new WaitForSeconds(0.15f);

            PerformOnAttack?.Invoke();
            int AttackAmount = ChainDelegate<int>(_equipment.getBaseAttack(), CheckAttack);
            EnemySpawner.ActiveEnemy.GetDamage(AttackAmount);
        }
        yield return new WaitForSeconds(0.5f);
        _inAction = false;
        GameManager.TurnEnded();
    }
}
