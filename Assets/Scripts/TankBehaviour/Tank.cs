using System;
using UnityEngine;

public class Tank : MonoBehaviour, ITakeDamage
{
    [SerializeField] private MainPartData _mainPartData;
    [SerializeField] private ProjectileSO _projectile;
    [SerializeField] private PropertyBar[] _bars;
    private AmmoStorage _ammoStorage;
    private Health _health;

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

    public void TakeDamage(int amt) => _health.TakeDamage(amt);

    public void Move(float direction) => _mainPart.Script.Move(direction);

    public void Rotate(float side) => _mainPart.Script.Rotate(side);

    public void Shoot()
    {
        _turret.Shoot(_projectile);
        _ammoStorage.LoadTurret(_turret);
    }

    public void Aim(Vector2 target) => _turret.AimAtTarget(target);

    public Transform GetCameraTarget() => _mainPart.SpawnedObj.transform;

    public void RestoreAmmo(int amt)
    {
        Debug.Log($"ammo was added - + {amt}");
        _ammoStorage.RessuplyAmmo(amt);
    }

    private void OnEnable()
    {
        SpawnTank();
        SetPropertyBars();
    }

    private void SpawnTank()
    {
        TurretData turretPartData = TurretDataBuilder.GetSelectedByUser(this);

        _mainPart = new MainPart(_mainPartData.SpawnPart(transform));
        _turret = turretPartData.SpawnInstance(transform).GetComponent<TurretPartBehav>();

        _mainPart.Script.SetData(_mainPartData);
        _turret.AttachToBase(_mainPart.SpawnedObj.transform);
        _ammoStorage = new AmmoStorage(_mainPartData.AmmoStorage);
        int maxHealth = Mathf.FloorToInt(_mainPartData.Durability * turretPartData.DurabilityMultiplier);
        _health = new Health(maxHealth);
        _health.ZeroHealth += DestroyThisTank;
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
