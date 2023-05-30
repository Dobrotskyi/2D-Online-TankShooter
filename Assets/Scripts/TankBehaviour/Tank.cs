using System;
using System.Collections;
using UnityEngine;

public class Tank : MonoBehaviour, ITakeDamage
{
    [SerializeField] private MainPartData _mainPartData;
    [SerializeField] private ProjectileSO _projectile;
    [SerializeField] private PropertyBar[] _bars;
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
        _turret.Shoot(_projectile);
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
        StartCoroutine(Setup());
    }

    private IEnumerator Setup()
    {
        yield return StartCoroutine(SpawnTank());
        yield return StartCoroutine(SetPropertyBars());
        yield break;
    }

    private IEnumerator SpawnTank()
    {
        TurretDataBuilder builder = new();
        yield return StartCoroutine(TurretDataBuilder.GetSelectedByUserTurret(builder));

        TurretData data = builder.Build();
        GameObject obj = data.SpawnInstance(transform);
        _turret = obj.GetComponent<TurretPartBehav>();
        _turret.SetData(data);

        _mainPart = new MainPart(_mainPartData.SpawnPart(transform));
        _mainPart.Script.SetData(_mainPartData);
        _turret.AttachToBase(_mainPart.SpawnedObj.transform);

        _ammoStorage = new AmmoStorage(_mainPartData.AmmoStorage);
        int maxHealth = Mathf.FloorToInt(_mainPartData.Durability * data.DurabilityMultiplier);
        _health = new Health(maxHealth);
        _health.ZeroHealth += DestroyThisTank;

        _setupInProgress = false;
        yield break;
    }

    private IEnumerator SetPropertyBars()
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
        yield break;
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
