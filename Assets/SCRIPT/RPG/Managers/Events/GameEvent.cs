using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using NightenUtils;

/// Dependencies:
/// - GameManager.GAMESTATE

public class GameEvent: MonoBehaviour
{
    public static event Action OnGameStart;
    public static event Action OnEncounter;
    public static event Action OnVictory;
    public static event Action OnGameOver;
    public static event Action<GAMESTATE> OnTurnChanged;
    public static event Action<KeyValuePair<ItemSO, int>[]> OnInventoryChanged;
    public static event Action<string> OnNotification;

    public static event Action<ItemSO> OnEnemyReceivePotion; //NOTE(Nighten) In the future, we would need to also pass an id for the target enemy
    public static event Action<ItemSO> OnPlayerReceivePotion;

    public static event Action<int> OnPlayerAttacked;
    public static event Action<int> OnEnemyAttacked;

    private void Start()
    {
        OnGameStart?.Invoke();
        OnEncounter?.Invoke();
    }

    //NOTE(Nighten) This method should only be invoked by the GameManager
    public static void EndTurnEvent(GAMESTATE pState)
        => OnTurnChanged?.Invoke(pState);

    //NOTE(Nighten) It would be cool to also send the modified slots, so that we can make an small animation in the UI
    public static void InventoryChangeEvent(KeyValuePair<ItemSO, int>[] pInventory)
        => OnInventoryChanged?.Invoke(pInventory);

    public static void NotificationEvent(string pText)
        => OnNotification?.Invoke(pText);

    public static void ThrowPotionEvent(ItemSO pPotion, bool pTargetIsThePlayer)
    {
        if (pTargetIsThePlayer)
            OnPlayerReceivePotion?.Invoke(pPotion);
        else
            OnEnemyReceivePotion?.Invoke(pPotion);
    }

    public static void AttackPlayer(int pDamage)
    {
        OnPlayerAttacked?.Invoke(pDamage);
    }
    public static void AttackEnemy(int pDamage)
    {
        OnEnemyAttacked?.Invoke(pDamage);
    }
}
