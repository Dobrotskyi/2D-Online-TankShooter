using TMPro;
using UnityEngine;

public class LobbyListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameField;
    [SerializeField] private TextMeshProUGUI _statusField;
    private PlayerReadyStatus _status = PlayerReadyStatus.Not_Ready;
    private Color _readyColor = Color.green;
    private Color _notReadyColor = Color.red;

    public PlayerReadyStatus Status => _status;

    public void InitializeItem(Photon.Realtime.Player player)
    {
        _nameField.text = player.NickName;
        _status = PlayerReadyStatus.Not_Ready;
        _statusField.text = _status.ToString().Replace("_", " ");
        _statusField.color = _notReadyColor;
    }

    public void ChangeStatus()
    {
        if (_status == PlayerReadyStatus.Ready)
        {
            _status = PlayerReadyStatus.Not_Ready;
            _statusField.color = _notReadyColor;
        }
        else if (_status == PlayerReadyStatus.Not_Ready)
        {
            _status = PlayerReadyStatus.Ready;
            _statusField.color = _readyColor;
        }

        _statusField.text = _status.ToString().Replace("_", " ");
    }
}
