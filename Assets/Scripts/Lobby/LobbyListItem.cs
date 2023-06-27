using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameField;
    [SerializeField] private TextMeshProUGUI _statusField;

    public void InitializeItem(Photon.Realtime.Player player, PlayStatus status)
    {
        _nameField.text = player.NickName;
        _statusField.text = status.ToString().Replace("_", " ");
    }
}
