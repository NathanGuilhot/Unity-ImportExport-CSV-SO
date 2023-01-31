using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _description;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _attack;
    RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        transform.position = Input.mousePosition;
        SetPivot();
    }
    public void SetItem(ItemSO pItem)
    {
        _description.text = pItem.description;
        _name.text = pItem.name;
        if (pItem.canEquip)
        {
            _attack.gameObject.SetActive(true);
            _attack.text = pItem.damage.ToString();
        }
        else
            _attack.gameObject.SetActive(false);
    }

    void SetPivot()
    {
        float _pivot_x = (transform.position.x > Screen.width  / 2) ? 1.1f : -0.1f;
        float _pivot_y = (transform.position.y > Screen.height / 2) ? 1f : 0f;
        
        _rectTransform.pivot = new Vector2(_pivot_x, _pivot_y);
    }
    private void Update()
    {
        transform.position = Input.mousePosition;
        SetPivot();
    }
}
