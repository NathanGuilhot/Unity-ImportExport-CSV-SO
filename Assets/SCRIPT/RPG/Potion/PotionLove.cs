using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Dependencies:
/// - GameManager (GAMESTATE)
/// - GameEvent (Notification)

public class PotionLove : MonoBehaviour, IPotionEffect
{
    int _turnRemaining;
    Animator _FX;
    IPotionTarget _target;

    public void Init(ItemSO pPotion, IPotionTarget pTarget)
    {
        _turnRemaining = pPotion.PotionValue;

        _target = pTarget;
        _target.CheckAttack.Add(ReduceAttack);
        if (GameManager.GameState == _target.ActiveState)
            _turnRemaining -= 1;

        SpawnFX(pPotion);

        GameEvent.NotificationEvent($"The {_target.name} is in love!");
        GameEvent.OnTurnChanged += ProcessTurns;
    }

    private void SpawnFX(ItemSO pPotion)
    {
        _FX = Instantiate(pPotion.effectParticle, transform).GetComponent<Animator>();
        _FX.transform.position += new Vector3(0f, 0f, -1f);
    }
    
    private void OnDestroy()
    {
        GameEvent.OnTurnChanged -= ProcessTurns;
        _target.CheckAttack.Remove(ReduceAttack);
        
        _FX.Play("FadeOut");
        Destroy(_FX.gameObject, 1f);
    }

    int ReduceAttack(int pAttack)
    {
        return pAttack / 4;
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
