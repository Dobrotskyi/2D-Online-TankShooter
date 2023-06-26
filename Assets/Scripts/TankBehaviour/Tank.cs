using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class Tank : MonoBehaviourPun, ITakeDamageFromPlayer, IPunObservable
{
    public event Action TankWasDestroyed;

    [SerializeField] private PropertyBar[] _bars;
    [SerializeField] private GameObject _explosionAnim;
    [SerializeField] private Player _player;
    private AmmoStorage _ammoStorage;
    private Health _health;
    private bool _setupInProgress = true;
    private PhotonView _view;
    private string _lastDamagerName;

    private Vector3 _netPos;
    private Quaternion _netRot;

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (_setupInProgress)
            return;

        if (stream.IsWriting)
        {
            stream.SendNext(_mainPart.SpawnedObj.transform.position);
            stream.SendNext(_mainPart.SpawnedObj.transform.rotation);
            stream.SendNext(_mainPart.Behav.Velocity);
            stream.SendNext(_mainPart.Behav.AngularVelocity);
        }
        else if (stream.IsReading)
        {
            _netPos = (Vector3)stream.ReceiveNext();
            _netRot = (Quaternion)stream.ReceiveNext();
            _mainPart.Behav.ChangeVelocity((Vector3)stream.ReceiveNext());
            _mainPart.Behav.ChangeAngularVelocity((float)stream.ReceiveNext());
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            _netPos += (_mainPart.Behav.Velocity * lag);

            Quaternion lagRotation = Quaternion.Euler(0, 0, _mainPart.Behav.AngularVelocity * lag);
            _netRot = Quaternion.Euler(_netRot.eulerAngles + lagRotation.eulerAngles);
        }
    }

    private void FixedUpdate()
    {
        if (_view.IsMine)
            return;

        _mainPart.Behav.ChangeTowards(_netPos, _netRot);
    }

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
        {
            _view.RPC("RPC_TakeDamage", RpcTarget.All, amt, damagerName);
        }
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
        _view.RPC("RPC_Shoot", RpcTarget.All, _turret.GenereteDirection());
    }

    [PunRPC]
    private void RPC_Shoot(Vector2 direction)
    {
        _turret.Shoot(direction);
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

        yield return StartCoroutine(ObjectFromDBBuilder.GetSelectedByUser(mainBuilder, DBManager.SELECTED_MAIN_URL, photonView.Owner.NickName));
        yield return StartCoroutine(ObjectFromDBBuilder.GetSelectedByUser(turretBuilder, DBManager.SELECTED_TURRET_URL, photonView.Owner.NickName));

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
        GameObject explosion = Instantiate(_explosionAnim, _mainPart.SpawnedObj.transform.position, Quaternion.identity);
        _view.RPC("RPC_Respawn", RpcTarget.All, FindObjectOfType<PlayerSpawner>().GetRandomSpawnPoint());
        GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreBoard>().UpdateScoreboard(_lastDamagerName);
    }

    [PunRPC]
    private void RPC_Respawn(Vector2 newPos)
    {
        TankWasDestroyed?.Invoke();
        _mainPart.SpawnedObj.transform.position = newPos;
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
