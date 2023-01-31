using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    EnemySO[] Enemies;
    public static EnemyStat ActiveEnemy;

    private void OnEnable()
    {
        GameEvent.OnEncounter += SpawnMonster;
    }
    private void OnDisable()
    {
        GameEvent.OnEncounter -= SpawnMonster;
    }

    private void Update()
    {
        
    }

    void SpawnMonster()
    {
        if (Enemies.Length > 0)
        {
            EnemySO RandomEnemy = Enemies[Random.Range(0, Enemies.Length)];
            ActiveEnemy = Instantiate(RandomEnemy.prefab, transform).GetComponent<EnemyStat>();
            ActiveEnemy.Init(RandomEnemy);
            ActiveEnemy.OnDestroyed = OnMonsterDestroy;
        }
    }

    void OnMonsterDestroy(GameObject pDestroyedMonster)
    {
        pDestroyedMonster.AddComponent<AnimationAutoDestroy>();
        Debug.Log("Monster defeated!");
        SpawnMonster();
    }
}
