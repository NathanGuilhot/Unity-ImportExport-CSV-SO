using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEnrage : MonoBehaviour, IPotionEffect
{
    [SerializeField] int _turnRemaining;
    int _damage;
    Animator _FX;
    IPotionTarget _target;

    public void Init(ItemSO pPotion, IPotionTarget pTarget)
    {
        _turnRemaining = pPotion.PotionValue;
        _damage = pPotion.damage;

        _target = pTarget;
        if (GameManager.GameState == _target.ActiveState)
            _turnRemaining -= 1;
        _target.PerformOnAttack += HurtWhenAttack;
        
        _FX = Instantiate(pPotion.effectParticle, transform).GetComponent<Animator>();
        _FX.transform.position += new Vector3(0f, 0f, -1f);
        
        GameEvent.NotificationEvent($"The {_target.name} is enraged!");
        GameEvent.OnTurnChanged += ProcessTurns;
    }
    
    private void OnDestroy()
    {
        GameEvent.OnTurnChanged -= ProcessTurns;
        _target.PerformOnAttack -= HurtWhenAttack;
        _FX.Play("FadeOut");
        Destroy(_FX.gameObject, 1f);
    }

    void HurtWhenAttack()
    {
        _target.GetDamage(_damage);
        
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
