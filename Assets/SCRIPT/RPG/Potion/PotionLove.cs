using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Dependencies:
/// - GameManager (GAMESTATE)
/// - GameEvent (Notification)

public class PotionLove : MonoBehaviour, IPotionEffect
{
    int _turnRemaining;
    GameObject _FX;
    IPotionTarget _target;

    public void Init(ItemSO pPotion, IPotionTarget pTarget)
    {
        _turnRemaining = pPotion.PotionValue;

        _target = pTarget;
        _target.CheckAttack.Add(ReduceAttack);

        SpawnFX(pPotion);
    }

    private void SpawnFX(ItemSO pPotion)
    {
        _FX = Instantiate(pPotion.effectParticle, transform);
        _FX.transform.position += new Vector3(0f, 0f, -1f);
    }

    void Awake()
    {
        GameEvent.NotificationEvent("The enemy has a crush on you!");
        GameEvent.OnTurnChanged += ProcessTurns;
    }
    private void OnDestroy()
    {
        GameEvent.OnTurnChanged -= ProcessTurns;
        Destroy(_FX);
        _target.CheckAttack.Remove(ReduceAttack);
    }

    int ReduceAttack(int pAttack)
    {
        return pAttack / 4;
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
