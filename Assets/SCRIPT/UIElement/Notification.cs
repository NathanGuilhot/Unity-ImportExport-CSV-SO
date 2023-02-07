using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    public void SetText(string pText)
    {
        _text.text = pText;
    }
}
