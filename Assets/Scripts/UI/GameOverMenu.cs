using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _killedEnemiesTxt;
    [SerializeField] private TextMeshProUGUI _rewardTxt;
    [SerializeField] private TextMeshProUGUI _nickNameTxt;

    private void Awake()
    {
        InGameTimer.TimeIsUp += ShowMenu;
    }

    private void OnDisable()
    {
        InGameTimer.TimeIsUp -= ShowMenu;
    }

    private void ShowMenu()
    {
        int kills = FindObjectOfType<ScoreBoard>().GetPlayerKills(PhotonNetwork.NickName);
        _killedEnemiesTxt.text = kills.ToString();
        _rewardTxt.text = (kills * DBManager.REWARD_FOR_KILL).ToString();
        _nickNameTxt.text = PhotonNetwork.NickName;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
