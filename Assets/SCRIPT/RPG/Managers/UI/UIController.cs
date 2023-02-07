using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    Dictionary<GAMESTATE, string> TurnMessage = new Dictionary<GAMESTATE, string>()
    {
        { GAMESTATE.ENEMY_TURN, "ENEMY'S TURN!" },
        { GAMESTATE.PLAYER_TURN, "YOUR TURN!"},
    };

    public static GameObject MainCanvas;

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

    public void Start()
    {
        MainCanvas = GameObject.Find("Canvas");
    }

    public void TurnChanged(GAMESTATE pState)
    {
        TurnTextObj.text = TurnMessage[pState];
    }
}
