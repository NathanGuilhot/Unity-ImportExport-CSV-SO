using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionLove : MonoBehaviour, IPotionEffect
{
    int _turnRemaining;
    EnemyStat _target;

    public void Init(int pPotionValue)
    {
        _turnRemaining = pPotionValue;
    }

    void Awake()
    {
        GameEvent.NotificationEvent("The enemy don't want to hurt you!");
        GameEvent.OnTurnChanged += ProcessTurns;
        _target = GetComponent<EnemyStat>();
        _target.CheckAttack.Add(ReduceAttack);
    }
    private void OnDestroy()
    {
        GameEvent.OnTurnChanged -= ProcessTurns;
        _target.CheckAttack.Remove(ReduceAttack);
    }

    int ReduceAttack(int pAttack)
    {
        return pAttack / 4;
    }

    void ProcessTurns(GameManager.GAMESTATE pState)
    {
        if (pState == GameManager.GAMESTATE.ENEMY_TURN)
        {
            _turnRemaining -= 1;
            if (_turnRemaining < 0)
                Destroy(this);
        }
    }
}
