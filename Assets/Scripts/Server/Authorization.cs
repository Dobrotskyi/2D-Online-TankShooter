using UnityEngine;
using UnityEngine.UI;

public class Authorization : MonoBehaviour
{
    public static int REWARD_FOR_KILL = 50;
    public static int MAX_NAME_LENGTH = 10;
    public static int MIN_NAME_LENGTH = 5;
    public static int MIN_PASSW_LENGTH = 5;

    protected void DisplayMessage(string text, Vector3 pos, NotificationType type) =>
                                                            NotificationFabric.Instance.DisplayNotification(text, pos, type, transform);

}
