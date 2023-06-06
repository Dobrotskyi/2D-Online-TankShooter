using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    private int speed = 30;
    private PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (!_view.IsMine)
            return;
        float horiontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.position += new Vector3(horiontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime, 0);
    }
}
