using UnityEngine;
using UnityEngine.UI;

public class NotificationFabric : MonoBehaviourSingleton<NotificationFabric>
{
    [SerializeField] private Notification _notificationPrefab;

    public void DisplayNotification(string text, Vector3 pos, NotificationType type, Transform parent)
    {
        Notification notification = Instantiate(_notificationPrefab, parent);
        notification.transform.position = pos;
        notification.transform.rotation = Quaternion.identity;
        if (type == NotificationType.Success)
            notification.GetComponentInChildren<Image>().color = Color.white;

        notification.SetText(text);
        notification.transform.SetParent(parent);
        notification.transform.localScale = Vector3.one;
    }
}
