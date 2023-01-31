using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static IFXController FX { get; private set; }
    public static PlayerInventory Inventory { get; private set; }
    public static PlayerStat Player { get; private set; }

    public enum GAMESTATE
    {
        PLAYER_TURN,
        ENEMY_TURN
    }

    public static GAMESTATE GameState { get; private set; }

    private void Awake()
    {
        //Singleton code
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        FX = GetComponent<IFXController>();
        Inventory = GetComponent<PlayerInventory>();
        Player = GetComponent<PlayerStat>();
    }
    public static void TurnEnded()
    {
        Debug.Log("CHANGING TURN");
        if (GameState == GAMESTATE.PLAYER_TURN)
            GameState = GAMESTATE.ENEMY_TURN;
        else if (GameState == GAMESTATE.ENEMY_TURN)
            GameState = GAMESTATE.PLAYER_TURN;

        GameEvent.EndTurnEvent(GameState);
    }

}
