using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Dependencies:
/// - GameManager (GAMESTATE)
/// - GameEvent (Notification)

public class PotionPacifier : MonoBehaviour, IPotionEffect
{
    int _turnRemaining;
    Animator _FX;
    IPotionTarget _target;

    public void Init(ItemSO pPotion, IPotionTarget pTarget)
    {
        _turnRemaining = pPotion.PotionValue;

        _target = pTarget;
        if (GameManager.GameState == _target.ActiveState)
            _turnRemaining -= 1;
        _target.CanAttack += CannotAttack;
        
        _FX = Instantiate(pPotion.effectParticle, transform).GetComponent<Animator>();
        _FX.transform.position += new Vector3(0f, 0f, -1f);
        
        GameEvent.NotificationEvent($"The {_target.name} is relaxed!");
        GameEvent.OnTurnChanged += ProcessTurns;
    }
    
    private void OnDestroy()
    {
        GameEvent.OnTurnChanged -= ProcessTurns;
        _target.CanAttack -= CannotAttack;
        _FX.Play("FadeOut");
        Destroy(_FX.gameObject, 1f);
    }

    bool CannotAttack()
    {
        GameEvent.NotificationEvent($"{_target.name} is too chill to attack!");
        return false;
    }
    
    void ProcessTurns(GAMESTATE pState)
    {
        if (pState == _target.ActiveState)
        {
            _turnRemaining -= 1;
            if (_turnRemaining < 0)
                Destroy(this);
        }
    }
}
