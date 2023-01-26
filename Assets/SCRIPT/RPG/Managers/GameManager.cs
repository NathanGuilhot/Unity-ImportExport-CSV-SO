using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static IFXController FX { get; private set; }
    public static PlayerInventory Inventory { get; private set; }

    public enum GAMESTATE
    {
        PLAYER_TURN,
        ENEMY_TURN
    }

    public static GAMESTATE GameState { get; private set; }

    private void Start()
    {
        FX = GetComponent<IFXController>();
        Inventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Inventory.LogInventory();
        }
    }

    public static void TurnEnded()
    {
        if (GameState == GAMESTATE.PLAYER_TURN)
            GameState = GAMESTATE.ENEMY_TURN;
        else if (GameState == GAMESTATE.ENEMY_TURN)
            GameState = GAMESTATE.PLAYER_TURN;

        GameEvent.EndTurnEvent(GameState);
    }

}
