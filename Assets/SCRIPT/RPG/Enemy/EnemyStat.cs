using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour, IEntityStat
{
    [SerializeField] new string name;
    [field: SerializeField] public int PV { get; set; }
    int PV_Max;
    [field:SerializeField] public int Attack { get; set; }
    [SerializeField] ItemSO loot;

    [SerializeField] LifeBar _lifeBar;

    public Action<GameObject> OnDestroyed;

    private Animator anim;
    private bool isAlive = true;
    private bool inAction = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(EnemySO pData)
    {
        name = pData.name;
        PV_Max = pData.PV;
        PV = pData.PV;
        Attack = pData.Attack;
        loot = pData.loot;
    }

    private void Update()
    {
        if (isAlive && !inAction)
        {
            switch (GameManager.GameState)
            {
                case GameManager.GAMESTATE.PLAYER_TURN:
                    if (Input.GetMouseButtonDown(0))
                    {
                        GameManager.FX.DisplayDamage(10, transform.position + new Vector3(4f, 0f, -0.5f));

                        PV = Mathf.Max(PV - 10, 0);
                        _lifeBar.SetValue((float)PV / (float)PV_Max);

                        if (PV <= 0)
                        {
                            StartCoroutine(OnDead());
                        }
                        else
                            anim.Play("EnemyDamage 0");
                        GameManager.TurnEnded();
                    }
                    break;
                case GameManager.GAMESTATE.ENEMY_TURN:
                    inAction = true;
                    Debug.Log("Enemy's turn!");
                    StartCoroutine(OnAttack());
                    break;
                default:
                    break;
            }

        }
    }

    private IEnumerator OnDead()
    {
        isAlive = false;
        anim.SetBool("alive", false);
        if (loot != null)
            ItemDrop();
        yield return new WaitForSeconds(0.5f);
        OnDestroyed?.Invoke(this.gameObject);
    }
    private IEnumerator OnAttack()
    {
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("attack", false);
        //NOTE(Nighten) We switch this bool quickly so we can be sure the animation is played at exit time of the spawn
        //              Playing the animation directly doesn't allow that
        //anim.Play("EnemyAttack 0");
    }

    public void OnAttackEnded()
    {
        inAction = false;
        GameManager.TurnEnded();
    }

    void ItemDrop()
    {
        bool success = GameManager.Inventory.AddItem(loot);
            GameObject LootDropped = Instantiate(loot.prefab) as GameObject;
            LootDropped.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            LootDropped.AddComponent<AnimationAutoDestroy>();
        if (!success)
        {
            Debug.Log("No longer space in inventory");
            return;
        }
    }
}


