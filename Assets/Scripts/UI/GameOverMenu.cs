using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _killedEnemiesTxt;
    [SerializeField] private TextMeshProUGUI _rewardTxt;
    [SerializeField] private TextMeshProUGUI _nickNameTxt;
    private GameTimeTimer _timer;

    private void Awake()
    {
        _timer = FindObjectOfType<GameTimeTimer>();
        _timer.TimeIsUp += ShowMenu;
    }

    private void OnDisable()
    {
        _timer.TimeIsUp -= ShowMenu;
    }

    private void ShowMenu()
    {
        int kills = FindObjectOfType<Scoreboard>().GetPlayerKills(PhotonNetwork.NickName);
        _killedEnemiesTxt.text = kills.ToString();
        _rewardTxt.text = (kills * DBManager.REWARD_FOR_KILL).ToString();
        _nickNameTxt.text = PhotonNetwork.NickName;
        transform.GetChild(0).gameObject.SetActive(true);

        AddMoney();
    }

    private void AddMoney() => StartCoroutine(DBManager.AddMoney(int.Parse(_rewardTxt.text)));
}
