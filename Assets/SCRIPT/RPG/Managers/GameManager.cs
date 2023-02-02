using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static IFXController FX { get; private set; }
    public static IInventory Inventory { get; private set; }
    public static IPotionTarget Player { get; private set; }

    public static GAMESTATE GameState { get; private set; } = GAMESTATE.PLAYER_TURN;

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
        Inventory = GetComponent<IInventory>();
        Player = GetComponent<IPotionTarget>();

        GameEvent.EndTurnEvent(GameState);
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
