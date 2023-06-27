using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private void FixedUpdate()
    {
        Debug.Log(PhotonNetwork.GetPing());
    }
}
