using NightenUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStat : MonoBehaviour, IPotionTarget
{
    [SerializeField] int PV;
    [SerializeField] int PV_Max;

    public List<Func<int, int>> CheckDamage { get; set; } = new List<Func<int, int>>();
    public List<Func<int, int>> CheckAttack { get; set; } = new List<Func<int, int>>();
    public _CheckCondition CanAttack { get; set; } = () => true;
    public Action PerformOnAttack { get; set; }

    [SerializeField] PlayerEquip _equipment;

    public void GetDamage(int pAmount)
    {
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
        if (GameManager.GameState == GAMESTATE.PLAYER_TURN) { 
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                PerformAttack();
            }
        }
    }

    private void PerformAttack()
    {
        EnemySpawner.ActiveEnemy.GetDamage(_equipment.getBaseAttack());
        GameManager.TurnEnded();
    }
}
