using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    EnemySO[] Enemies;

    private EnemyStat _activeEnemy;

    private void OnEnable() => GameEvent.OnEncounter += SpawnMonster;
    private void OnDisable() => GameEvent.OnEncounter -= SpawnMonster;

    void SpawnMonster()
    {
        if (Enemies.Length > 0)
        {
            EnemySO RandomEnemy = Enemies[Random.Range(0, Enemies.Length)];
            _activeEnemy = Instantiate(RandomEnemy.prefab, transform).GetComponent<EnemyStat>();
            _activeEnemy.Init(RandomEnemy, OnMonsterDestroy);
        }
    }

    void OnMonsterDestroy(GameObject pDestroyedMonster)
    {
        pDestroyedMonster.AddComponent<AnimationAutoDestroy>();
        Debug.Log("Monster defeated!");
        SpawnMonster();
    }
}
