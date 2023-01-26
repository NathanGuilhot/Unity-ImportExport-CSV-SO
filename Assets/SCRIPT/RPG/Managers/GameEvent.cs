using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent: MonoBehaviour
{
    public delegate void GameStart();
    public delegate void Encounter();
    public delegate void Victory();
    public delegate void GameOver();
    public delegate void TurnChanged(GameManager.GAMESTATE pState);
    public delegate void InventoryChanged(KeyValuePair<ItemSO, int>[] pInventory);

    public static event GameStart OnGameStart;
    public static event Encounter OnEncounter;
    public static event Victory OnVictory;
    public static event GameOver OnGameOver;
    public static event TurnChanged OnTurnChanged;
    public static event InventoryChanged OnInventoryChanged;

    private void Start()
    {
        OnGameStart?.Invoke();
        OnEncounter?.Invoke();
        OnTurnChanged?.Invoke(GameManager.GameState);
    }

    //NOTE(Nighten) This method should only be invoked by the GameManager
    public static void EndTurnEvent(GameManager.GAMESTATE pState)
    {
        OnTurnChanged?.Invoke(pState);
    }
    
    //NOTE(Nighten) It would be cool to also send the modified slot, so that we can make an small animation in the UI
    public static void InventoryChangeEvent(KeyValuePair<ItemSO, int>[] pInventory)
    {
        OnInventoryChanged?.Invoke(pInventory);
    }
}
