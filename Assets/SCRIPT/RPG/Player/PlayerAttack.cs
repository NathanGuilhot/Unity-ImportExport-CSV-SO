using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    ItemSO _equipedItem;
    [SerializeField] int DEFAULTATTACK = 10;

    private void OnEnable()
    {
        //Subscribe to event : equipement slot filled
        EquipmentSlot.OnUpdateEquipment += UpdateEquipment;
    }
    private void OnDisable()
    {
        EquipmentSlot.OnUpdateEquipment -= UpdateEquipment;
    }

    void UpdateEquipment(ItemSO pItem)
    {
        if (pItem != null)
            Debug.Log($"The equiped item is now {pItem?.name} !");
        else
            Debug.Log($"No more equiped items !");

        _equipedItem = pItem;
    }

    private void Update()
    {
        //_TargetEnemy = EnemySpawner.ActiveEnemy.GetComponent<EnemyStat>();

        if (GameManager.GameState == GameManager.GAMESTATE.PLAYER_TURN) { 
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                EnemySpawner.ActiveEnemy.GetDamage((uint)((_equipedItem!=null) ? _equipedItem.damage : DEFAULTATTACK));
                GameManager.TurnEnded();
            }
                
        }
    }
}
