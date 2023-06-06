using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Tank : MonoBehaviour, ITakeDamage
{
    [SerializeField] private PropertyBar[] _bars;
    [SerializeField] private bool _isBot = false;
    private AmmoStorage _ammoStorage;
    private Health _health;
    private bool _setupInProgress = true;
    private PhotonView _view;

    private struct MainPart
    {
        public GameObject SpawnedObj { get; private set; }
        public MainPartBehav Script { get; private set; }

        public MainPart(GameObject spawnedPart)
        {
            SpawnedObj = spawnedPart;
            Script = spawnedPart.GetComponent<MainPartBehav>();
        }
    }

    private TurretPartBehav _turret;

    private MainPart _mainPart;

    public void TakeDamage(int amt)
    {
        if (_isBot == false && _view.IsMine == false)
            return;
        if (_setupInProgress)
            return;

        Debug.Log(amt);
        _health.TakeDamage(amt);
    }

    public void Move(float direction)
    {
        if (_isBot == false && _view.IsMine == false)
            return;
        if (_setupInProgress)
            return;
        _mainPart.Script.Move(direction);
    }

    public void Rotate(float side)
    {
        if (_isBot == false && _view.IsMine == false)
            return;
        if (_setupInProgress)
            return;
        _mainPart.Script.Rotate(side);
    }

    public void Shoot()
    {
        if (_isBot == false && _view.IsMine == false)
            return;
        if (_setupInProgress)
            return;
        _turret.Shoot();
    }

    public void Aim(Vector2 target)
    {
        if (_isBot == false && _view.IsMine == false)
            return;
        if (_setupInProgress)
            return;
        _turret.AimAtTarget(target);
    }

    public void RestoreAmmo(int amt)
    {
        if (_isBot == false && _view.IsMine == false)
            return;
        _ammoStorage.RessuplyAmmo(amt);
    }

    public Transform GetCameraTarget()
    {
        if (_setupInProgress)
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
        if (_isBot == false && _view.IsMine == false)
            yield break;

        TurretDataBuilder turretBuilder = new();
        MainPartDataBuilder mainBuilder = new();

        yield return StartCoroutine(ObjectFromDBBuilder.GetSelectedByUser(mainBuilder, DBManager.SELECTED_MAIN_URL));
        yield return StartCoroutine(ObjectFromDBBuilder.GetSelectedByUser(turretBuilder, DBManager.SELECTED_TURRET_URL));

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
        {
            Debug.Log("InProgress");
            yield return null;
        }
        SetPropertyBars();
        _setupInProgress = false;
    }

    private void SetPropertyBars()
    {
        if (_isBot == false && _view.IsMine == false)
            return;
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

    private void OnDisable()
    {
        if (_view.IsMine)
        {
            _health.ZeroHealth -= DestroyThisTank;
        }
    }

    private void DestroyThisTank()
    {
        if (_isBot == false && _view.IsMine == false)
            return;
        Debug.Log("Destroying this tank");
        Animator animator = GetComponent<Animator>();
        animator.enabled = true;
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
