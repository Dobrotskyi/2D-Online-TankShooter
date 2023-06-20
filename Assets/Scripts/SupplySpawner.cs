using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplySpawner : MonoBehaviour
{
    private const float SUPP_TIME_OF_LIFE = 4.9f;

    [SerializeField] List<GameObject> _suppliesPrefs = new();
    [SerializeField] List<Transform> _spawnPoints = new();
    [SerializeField] Vector2 _minMaxTime = new(5, 10);
    private PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {
        while (InGameTimer.GameTime)
        {
            int prefInd = Random.Range(0, _suppliesPrefs.Count);
            int spInd = Random.Range(0, _spawnPoints.Count);
            Debug.Log(prefInd);
            Debug.Log(spInd);
            Debug.Log(_view);
            _view.RPC("SpawnSupply", RpcTarget.All, prefInd, spInd);
            yield return new WaitForSeconds(Random.Range(_minMaxTime.x, _minMaxTime.y));
        }
    }

    [PunRPC]
    private void SpawnSupply(int prefInd, int spInd)
    {
        GameObject supp = Instantiate(_suppliesPrefs[prefInd], _spawnPoints[spInd].position, Quaternion.identity);
        Destroy(supp, SUPP_TIME_OF_LIFE);
    }
}
