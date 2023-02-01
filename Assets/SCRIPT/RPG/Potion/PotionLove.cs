using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionLove : MonoBehaviour, IPotionEffect
{
    int _turnRemaining;
    GameObject _FX;
    IPotionTarget _target;

    public void Init(ItemSO pPotion)
    {
        _turnRemaining = pPotion.PotionValue;
        _FX = Instantiate(pPotion.effectParticle, transform);
        _FX.transform.position += new Vector3(0f, 0f, -1f);
    }

    void Awake()
    {
        GameEvent.NotificationEvent("The enemy has a crush on you!");
        GameEvent.OnTurnChanged += ProcessTurns;
        _target = GetComponent<IPotionTarget>();
        _target.CheckAttack.Add(ReduceAttack);
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
