using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Authorization : MonoBehaviour
{
    [SerializeField] private GameObject _notification;

    protected void DisplayMessage(string text, Vector3 pos)
    {
        GameObject notification = Instantiate(_notification);
        notification.GetComponent<Notification>().DisplayNotification(text);
        notification.transform.SetParent(transform);
        notification.transform.position = pos;
        notification.transform.localScale = Vector3.one;
    }
}
