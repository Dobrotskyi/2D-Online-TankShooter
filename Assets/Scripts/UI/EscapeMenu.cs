using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _body;

    private void Start()
    {
        if (_body.activeSelf)
            _body.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            _body.SetActive(true);
    }

    public void Submit()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    public void Decline()
    {
        _body.SetActive(false);
    }
}
