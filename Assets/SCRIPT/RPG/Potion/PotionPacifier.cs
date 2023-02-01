using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPacifier : MonoBehaviour, IPotionEffect
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
        GameEvent.NotificationEvent("The enemy is relaxed!");
        GameEvent.OnTurnChanged += ProcessTurns;
        _target = GetComponent<IPotionTarget>();
        _target.CanAttack += CannotAttack;
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
