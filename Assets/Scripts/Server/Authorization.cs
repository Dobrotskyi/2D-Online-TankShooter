using UnityEngine;
using UnityEngine.UI;

public class Authorization : MonoBehaviour
{
    [SerializeField] private GameObject _notification;
    protected enum MessageType
    {
        Success,
        Fail
    }

    protected void DisplayMessage(string text, Vector3 pos, MessageType type)
    {
        GameObject notification = Instantiate(_notification);
        if (type == MessageType.Success)
            notification.GetComponentInChildren<Image>().color = Color.white;

        notification.GetComponent<Notification>().DisplayNotification(text);
        notification.transform.SetParent(transform);
        notification.transform.position = pos;
        notification.transform.localScale = Vector3.one;
    }
}
