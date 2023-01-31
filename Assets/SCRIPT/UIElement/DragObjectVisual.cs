using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragObjectVisual : MonoBehaviour
{
    [SerializeField] Image _image;
    
    public void SetSprite(Sprite pSprite)
    {
        _image.sprite = pSprite;
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
