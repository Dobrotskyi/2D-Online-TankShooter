using System.Collections;
using System;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawnpoint : MonoBehaviour
{
    public static event Action PlayerSpawned;

    [SerializeField] private float _spawnHereCoolDownTime = 3.5f;
    public bool IsBlocked { get; private set; } = false;

    public bool Spawn(GameObject tankPrefab)
    {
        if (IsBlocked)
            return false;

        IsBlocked = true;
        PhotonNetwork.Instantiate(tankPrefab.name, transform.position, Quaternion.identity);
        StartCoroutine(UnblockSpawning());
        return true;
    }

    private IEnumerator UnblockSpawning()
    {
        yield return new WaitForSeconds(_spawnHereCoolDownTime);
        IsBlocked = false;
    }

}
