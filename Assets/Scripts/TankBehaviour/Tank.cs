using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class Tank : MonoBehaviourPun, ITakeDamageFromPlayer
{
    public event Action TankWasDestroyed;

    [SerializeField] private PropertyBar[] _bars;
    [SerializeField] private GameObject _explosionAnim;
    [SerializeField] private Player _player;
    [SerializeField] private ParticleSystem _engineSmoke;
    private AmmoStorage _ammoStorage;
    private Health _health;
    private bool _setupInProgress = true;
    private PhotonView _view;
    private string _lastDamagerName;

    private struct MainPart
    {
        public GameObject SpawnedObj { get; private set; }
        public MainPartBehav Behav { get; private set; }

        public MainPart(GameObject spawnedPart)
        {
            SpawnedObj = spawnedPart;
            Behav = spawnedPart.GetComponent<MainPartBehav>();
        }
    }

    private TurretPartBehav _turret;

    private MainPart _mainPart;

    public void TakeDamage(int amt)
    {
        if (_setupInProgress || _player.Frozen)
            return;
        if (_view.IsMine)
            _view.RPC("RPC_TakeDamage", RpcTarget.All, amt);
    }

    public void TakeDamage(int amt, string damagerName)
    {
        if (_setupInProgress || _player.Frozen)
            return;
        if (_view.IsMine)
            _view.RPC("RPC_TakeDamage", RpcTarget.All, amt, damagerName);
    }

    [PunRPC]
    private void RPC_TakeDamage(int amt, string damagerName)
    {
        _lastDamagerName = damagerName;
        _health.TakeDamage(amt);
    }

    public void Move(float direction)
    {
        if (_view.IsMine == false)
            return;
        if (_setupInProgress)
            return;
        _mainPart.Behav.Move(direction);
    }

    public void Rotate(float side)
    {
        if (_view.IsMine == false)
            return;
        if (_setupInProgress)
            return;
        _mainPart.Behav.Rotate(side);
    }

    public void Shoot()
    {
        if (_view.IsMine == false)
            return;
        if (_setupInProgress)
            return;
        _view.RPC("RPC_Shoot", RpcTarget.All, _turret.GetDirection, _turret.GetBarrelPos);
    }

    [PunRPC]
    private void RPC_Shoot(Vector2 direction, Vector2 barrelPos)
    {
        _turret.Shoot(direction, barrelPos);
    }

    public void Aim(Vector2 target)
    {
        if (_view.IsMine == false)
            return;
        if (_setupInProgress)
            return;
        _turret.AimAtTarget(target);
    }

    public void RestoreAmmo(int amt)
    {
        if (_view.IsMine == false)
            return;
        _view.RPC("RPC_RestoreAmmo", RpcTarget.All, amt);
    }

    [PunRPC]
    private void RPC_RestoreAmmo(int amt)
    {
        _ammoStorage.RessuplyAmmo(amt);
    }

    public void RestoreHealth(int amt)
    {
        if (_view.IsMine == false)
            return;
        _view.RPC("RPC_RestoreHealth", RpcTarget.All, amt);
    }

    [PunRPC]
    private void RPC_RestoreHealth(int amt)
    {
        _health.RestoreHealth(amt);
    }

    public Transform GetCameraTarget()
    {
        if (_setupInProgress || !_view.IsMine)
            return null;
        else
            return _mainPart.SpawnedObj.transform;
    }

    private void OnEnable()
    {
        TryGetComponent(out _view);
        StartCoroutine(SpawnTank());
    }

    private IEnumerator SpawnTank()
    {
        TurretDataBuilder turretBuilder = new();
        MainPartDataBuilder mainBuilder = new();

        yield return StartCoroutine(ObjectFromDBBuilder.GetSelectedByUser(mainBuilder, DBManager.ServerURLS.SELECTED_MAIN_URL, photonView.Owner.NickName));
        yield return StartCoroutine(ObjectFromDBBuilder.GetSelectedByUser(turretBuilder, DBManager.ServerURLS.SELECTED_TURRET_URL, photonView.Owner.NickName));

        MainPartData mainPartData = (MainPartData)mainBuilder.Build();
        _mainPart = new MainPart(mainPartData.SpawnInstance(transform));

        TurretData turretData = (TurretData)turretBuilder.Build();
        GameObject obj = turretData.SpawnInstance(transform);
        _turret = obj.GetComponent<TurretPartBehav>();
        _turret.SetData(turretData);
        _turret.AttachToBase(_mainPart.SpawnedObj.transform);

        _ammoStorage = new AmmoStorage(mainPartData.AmmoStorage);
        _turret.SetAmmoSource(_ammoStorage);
        int maxHealth = Mathf.FloorToInt(mainPartData.Durability * turretData.DurabilityMultiplier);
        _health = new Health(maxHealth);
        _health.ZeroHealth += DestroyThisTank;

        while (_mainPart.SpawnedObj == null && _turret.gameObject == null)
            yield return null;

        SetPropertyBars();
        SetupNameTagCanv();
        _setupInProgress = false;

        _health.TakeDamage(maxHealth / 2);
    }

    private void SetPropertyBars()
    {
        if (_bars.Length != 0)
        {
            float halfSize = _mainPart.SpawnedObj.GetComponent<Collider2D>().bounds.size.y / 2;
            float yOffset = halfSize + 0.3f;
            foreach (var bar in _bars)
            {
                bar.SetFollowObject(_mainPart.SpawnedObj.transform).SetYOffset(yOffset);
                yOffset += 0.3f;
            }
            _bars[0].SetProperty(_health);
            _bars[1].SetProperty(_ammoStorage);
        }
    }

    private void SetupNameTagCanv()
    {
        Transform nameTagCanv = transform.Find("NameTagCanvas");
        float halfSize = _mainPart.SpawnedObj.GetComponent<Collider2D>().bounds.size.y / 2;
        float yOffset = halfSize + 0.3f;
        nameTagCanv.GetComponent<NameTag>().SetupNameTag(_view.Owner.NickName, _mainPart.SpawnedObj.transform)
                                           .SetYOffset(yOffset);
    }

    private void DestroyThisTank()
    {
        Instantiate(_explosionAnim, _mainPart.SpawnedObj.transform.position, Quaternion.identity);
        if (_view.IsMine)
            _view.RPC("RPC_Respawn", RpcTarget.All, FindObjectOfType<PlayerSpawner>().GetRandomSpawnPoint());
        GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreBoard>().UpdateScoreboard(_lastDamagerName);
    }

    [PunRPC]
    private void RPC_Respawn(Vector2 newPos)
    {
        TankWasDestroyed?.Invoke();
        _mainPart.SpawnedObj.SetActive(false);
        _mainPart.SpawnedObj.transform.position = newPos;
        _mainPart.SpawnedObj.SetActive(true);
        FullRepair();
    }

    private void FullRepair()
    {
        _health.RestoreHealth(1000);
        _ammoStorage.RessuplyAmmo(1000);
    }

    private void OnDisable()
    {
        if (_view != null && _view.IsMine)
        {
            _health.ZeroHealth -= DestroyThisTank;
        }
    }
}
