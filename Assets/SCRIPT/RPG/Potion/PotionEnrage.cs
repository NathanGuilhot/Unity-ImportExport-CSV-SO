using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEnrage : MonoBehaviour, IPotionEffect
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
        GameEvent.NotificationEvent("The enemy is enraged!");
        GameEvent.OnTurnChanged += ProcessTurns;
        _target = GetComponent<IPotionTarget>();
        _target.PerformOnAttack += HurtWhenAttack;
    }
    private void OnDestroy()
    {
        GameEvent.OnTurnChanged -= ProcessTurns;
        _target.PerformOnAttack -= HurtWhenAttack;
        Destroy(_FX);
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
