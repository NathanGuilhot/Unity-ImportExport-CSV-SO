using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    Dictionary<GameManager.GAMESTATE, string> TurnMessage = new Dictionary<GameManager.GAMESTATE, string>()
    {
        { GameManager.GAMESTATE.ENEMY_TURN, "ENEMY'S TURN!" },
        { GameManager.GAMESTATE.PLAYER_TURN, "YOUR TURN!"},
    };

    [SerializeField]
    private TextMeshProUGUI TurnTextObj;

    private void OnEnable()
    {
        GameEvent.OnTurnChanged += TurnChanged;
    }
    private void OnDisable()
    {
        GameEvent.OnTurnChanged -= TurnChanged;
    }

    public void TurnChanged(GameManager.GAMESTATE pState)
    {
        TurnTextObj.text = TurnMessage[pState];
    }
}
