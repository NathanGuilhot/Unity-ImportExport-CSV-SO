using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEnrage : MonoBehaviour, IPotionEffect
{
    int _turnRemaining;
    EnemyStat _target;
    public void Init(int pPotionValue)
    {
        _turnRemaining = pPotionValue;
    }
    void Awake()
    {
        GameEvent.NotificationEvent("The enemy is enraged!");
        GameEvent.OnTurnChanged += ProcessTurns;
        _target = GetComponent<EnemyStat>();
        _target.PerformOnAttack += HurtWhenAttack;
    }
    private void OnDestroy()
    {
        GameEvent.OnTurnChanged -= ProcessTurns;
        _target.PerformOnAttack -= HurtWhenAttack;
    }

    void HurtWhenAttack()
    {
        _target.GetDamage(20);
        
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
