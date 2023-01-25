using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent: MonoBehaviour
{
    public delegate void GameStart();
    public delegate void Encounter();
    public delegate void Victory();
    public delegate void GameOver();

    public static event GameStart OnGameStart;
    public static event Encounter OnEncounter;
    public static event Victory OnVictory;
    public static event GameOver OnGameOver;

    private void Start()
    {
        OnGameStart?.Invoke();
        OnEncounter?.Invoke();
    }

}
