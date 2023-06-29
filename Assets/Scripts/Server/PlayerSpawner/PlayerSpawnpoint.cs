using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawnpoint : MonoBehaviour
{
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
