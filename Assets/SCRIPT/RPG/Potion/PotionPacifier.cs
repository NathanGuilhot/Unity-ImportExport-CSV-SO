using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Dependencies:
/// - GameManager (GAMESTATE)
/// - GameEvent (Notification)

public class PotionPacifier : MonoBehaviour, IPotionEffect
{
    int _turnRemaining;
    GameObject _FX;
    IPotionTarget _target;

    public void Init(ItemSO pPotion, IPotionTarget pTarget)
    {
        _turnRemaining = pPotion.PotionValue;
        
        _target = pTarget;
        _target.CanAttack += CannotAttack;
        
        _FX = Instantiate(pPotion.effectParticle, transform);
        _FX.transform.position += new Vector3(0f, 0f, -1f);
    }

    
    void Awake()
    {
        GameEvent.NotificationEvent("The enemy is relaxed!");
        GameEvent.OnTurnChanged += ProcessTurns;
    }
    private void OnDestroy()
    {
        GameEvent.OnTurnChanged -= ProcessTurns;
        _target.CanAttack -= CannotAttack;
        Destroy(_FX);
    }

    bool CannotAttack()
    {
        GameEvent.NotificationEvent("Enemy is too chill to attack!");
        return false;
    }
    
    void ProcessTurns(GAMESTATE pState)
    {
        if (pState == GAMESTATE.ENEMY_TURN)
        {
            _turnRemaining -= 1;
            if (_turnRemaining < 0)
                Destroy(this);
        }
    }
}
