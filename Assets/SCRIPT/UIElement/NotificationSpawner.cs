using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationSpawner : MonoBehaviour
{
    [SerializeField] Notification _notificatioPrefab;

    private void OnEnable()
    {
        GameEvent.OnNotification += FireNotification;
    }
    private void OnDisable()
    {
        GameEvent.OnNotification -= FireNotification;
    }

    // Update is called once per frame
    void FireNotification(string pText)
    {
        Notification notifInstance = Instantiate<Notification>(_notificatioPrefab, transform);
        notifInstance.SetText(pText);
    }
}
