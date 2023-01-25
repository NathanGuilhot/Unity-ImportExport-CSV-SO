using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static IFXController FX;

    public delegate void TurnChanged(GameManager.GAMESTATE pState);
    public static event TurnChanged OnTurnChanged;

    public enum GAMESTATE
    {
        PLAYER_TURN,
        ENEMY_TURN
    }

    public static GAMESTATE GameState { get; private set; }

    private void Start()
    {
        FX = GetComponent<IFXController>();
        OnTurnChanged?.Invoke(GameState);
    }

    public static void TurnEnded()
    {
        if (GameState == GAMESTATE.PLAYER_TURN)
            GameState = GAMESTATE.ENEMY_TURN;
        else if (GameState == GAMESTATE.ENEMY_TURN)
            GameState = GAMESTATE.PLAYER_TURN;

        OnTurnChanged?.Invoke(GameState);
    }

}
