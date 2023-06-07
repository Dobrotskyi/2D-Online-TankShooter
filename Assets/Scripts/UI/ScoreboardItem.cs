using TMPro;
using UnityEngine;

public class ScoreboardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nicknameField;
    [SerializeField] private TextMeshProUGUI _scoreField;

    public void Initialize(Photon.Realtime.Player player)
    {
        _nicknameField.text = player.NickName;
        _scoreField.text = "0";
    }
}
