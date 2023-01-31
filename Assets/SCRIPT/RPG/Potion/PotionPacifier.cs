using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPacifier : MonoBehaviour, IPotionEffect
{
    int _turnRemaining;
    EnemyStat _target;

    public void Init(int pPotionValue)
    {
        _turnRemaining = pPotionValue;
    }

    
    void Awake()
    {
        GameEvent.NotificationEvent("The enemy is relaxed!");
        GameEvent.OnTurnChanged += ProcessTurns;
        _target = GetComponent<EnemyStat>();
        _target.CanAttack += CannotAttack;
    }
    private void OnDestroy()
    {
        GameEvent.OnTurnChanged -= ProcessTurns;
        _target.CanAttack -= CannotAttack;
    }

    bool CannotAttack()
    {
        GameEvent.NotificationEvent("Enemy is too chill to attack!");
        return false;
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
