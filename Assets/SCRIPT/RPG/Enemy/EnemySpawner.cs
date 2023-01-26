using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    EnemySO[] Enemies;
    public GameObject ActiveEnemy;

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
            ActiveEnemy = Instantiate(RandomEnemy.prefab) as GameObject;
            ActiveEnemy.GetComponent<EnemyStat>().Init(RandomEnemy);
            ActiveEnemy.GetComponent<EnemyStat>().OnDestroyed = OnMonsterDestroy;
        }
    }

    void OnMonsterDestroy(GameObject pDestroyedMonster)
    {
        pDestroyedMonster.AddComponent<AnimationAutoDestroy>();
        Debug.Log("Monster defeated!");
        SpawnMonster();
    }
}
