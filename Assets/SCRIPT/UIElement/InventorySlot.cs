using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventorySlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _AmountDisplay;
    [SerializeField] Image _ImageRenderer;

    public void SetSprite(Sprite pSprite)
    {
        _ImageRenderer.sprite = pSprite;
    }
    public void SetAmount(int pAmount)
    {
        _AmountDisplay.text = pAmount.ToString();
        _AmountDisplay.gameObject.SetActive(pAmount > 1);
    }
}
