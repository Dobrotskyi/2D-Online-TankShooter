using System.Collections;
using UnityEngine;

public class Tank : MonoBehaviour, ITakeDamage
{
    [SerializeField] private PropertyBar[] _bars;
    private ProjectileData _projectileData;
    private AmmoStorage _ammoStorage;
    private Health _health;
    private bool _setupInProgress = true;

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
        if (_setupInProgress)
            return;

        Debug.Log(amt);
        _health.TakeDamage(amt);
    }

    public void Move(float direction)
    {
        if (_setupInProgress)
            return;
        _mainPart.Script.Move(direction);
    }

    public void Rotate(float side)
    {
        if (_setupInProgress)
            return;
        _mainPart.Script.Rotate(side);
    }

    public void Shoot()
    {
        if (_setupInProgress)
            return;
        _turret.Shoot();
        _ammoStorage.LoadTurret(_turret);
    }

    public void Aim(Vector2 target)
    {
        if (_setupInProgress)
            return;
        _turret.AimAtTarget(target);
    }

    public void RestoreAmmo(int amt)
    {
        Debug.Log($"ammo was added - + {amt}");
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
        StartCoroutine(SpawnTank());
    }

    private IEnumerator SpawnTank()
    {
        TurretDataBuilder turretBuilder = new();
        MainPartDataBuilder mainBuilder = new();

        yield return StartCoroutine(ObjectFromDBBuilder.GetSelectedByUser(mainBuilder));
        yield return StartCoroutine(ObjectFromDBBuilder.GetSelectedByUser(turretBuilder));

        MainPartData mainPartData = (MainPartData)mainBuilder.Build();
        _mainPart = new MainPart(mainPartData.SpawnInstance(transform));

        TurretData turretData = (TurretData)turretBuilder.Build();
        GameObject obj = turretData.SpawnInstance(transform);
        _turret = obj.GetComponent<TurretPartBehav>();
        _turret.SetData(turretData);
        _turret.AttachToBase(_mainPart.SpawnedObj.transform);
        _projectileData = turretData.ProjData;

        _ammoStorage = new AmmoStorage(mainPartData.AmmoStorage);
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
        }
    }

    private void OnDisable()
    {
        _health.ZeroHealth -= DestroyThisTank;
    }

    private void DestroyThisTank()
    {
        Debug.Log("Destroying this tank");
    }
}
