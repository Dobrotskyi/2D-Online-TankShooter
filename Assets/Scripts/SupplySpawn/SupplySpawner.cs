using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplySpawner : MonoBehaviour
{
    [SerializeField] List<ResourceCrate> _suppliesPrefs = new();
    [SerializeField] List<SupplySpawnPoint> _spawnPoints = new();
    [SerializeField] Vector2 _minMaxTime = new(2, 5);
    private PhotonView _view;

    private GameTimeTimer _timer;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _timer = FindObjectOfType<GameTimeTimer>();
        if (PhotonNetwork.IsMasterClient)
        {
            _timer.GameTimeStarted += StartSpawning;
        }
    }

    private void OnDisable()
    {
        _timer.GameTimeStarted -= StartSpawning;
    }

    private void StartSpawning()
    {
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        while (GameTimeTimer.GameTime)
        {
            int prefInd = UnityEngine.Random.Range(0, _suppliesPrefs.Count);
            int spInd = GetRandomEmptySP();
            if (spInd != -1)
                _view.RPC("SpawnSupply", RpcTarget.All, prefInd, spInd);
            yield return new WaitForSeconds(UnityEngine.Random.Range(_minMaxTime.x, _minMaxTime.y));
        }
    }

    private int GetRandomEmptySP()
    {
        List<int> emptySPInd = new();

        int i = 0;
        foreach (var sp in _spawnPoints)
        {
            if (sp.IsEmpty)
                emptySPInd.Add(i);
            i++;
        }

        if (emptySPInd.Count > 0)
            return emptySPInd[UnityEngine.Random.Range(0, emptySPInd.Count)];
        else
            return -1;
    }

    [PunRPC]
    private void SpawnSupply(int prefInd, int spInd)
    {
        _spawnPoints[spInd].Spawn(_suppliesPrefs[prefInd]);
    }
}
