using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour, IEntityStat
{
    [SerializeField] new string name;
    [field: SerializeField] public int PV { get; set; }
    [field:SerializeField] public int Attack { get; set; }
    [SerializeField] ItemSO loot;

    public Action<GameObject> OnDestroyed;

    private Animator anim;
    private bool alive = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(EnemySO pData)
    {
        name = pData.name;
        PV = pData.PV;
        Attack = pData.Attack;
        loot = pData.loot;
    }

    private void Update()
    {
        if (alive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameManager.FX.DisplayDamage(10, transform.position + new Vector3(4f, 0f, -0.5f));
                PV = Mathf.Max(PV-10, 0);
            }
            if (PV <= 0)
            {
                StartCoroutine(OnDead());
            }

        }
    }

    private IEnumerator OnDead()
    {
        alive = false;
        anim.SetBool("alive", false);
        yield return new WaitForSeconds(0.5f);
        OnDestroyed?.Invoke(this.gameObject);
    }
}


