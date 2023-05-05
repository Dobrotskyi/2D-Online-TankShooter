using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPartBehav : MonoBehaviour
{
    private TurretPart _data;
    private Transform _barrel;
    private Transform _mainPart;
    private float _fireRateMultiplier = 1f;
    private float _lastShotTime = 0f;

    public void SetData(TurretPart data)
    {
        _data = data;
    }

    public void Shoot(ProjectileSO projectileSO)
    {
        if (IsLoaded())
        {
            Vector2 direction = (Vector2)_barrel.up +
                new Vector2(Random.Range(-_data.Spread.x, _data.Spread.x), Random.Range(-_data.Spread.y, _data.Spread.y));
            GameObject projectile = projectileSO.SpawnAt(_barrel);
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * _data.ShotForce, ForceMode2D.Impulse);
            projectile.GetComponent<Projectile>().IgnoreCollisionWith(_mainPart.gameObject);
            _fireRateMultiplier = projectileSO.FireRateMultiplier;
            _lastShotTime = Time.time;
        }
    }

    public void AimAtTarget(Vector2 target)
    {
        Vector2 substraction = target - (Vector2)_barrel.transform.position;
        if (substraction.magnitude < _data.MinAimingDistance)
            return;

        Vector2 lookDirection = (target - (Vector2)_barrel.transform.position);
        float angleZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angleZ, _barrel.transform.forward);
        gameObject.transform.rotation = Quaternion.Slerp(_barrel.transform.rotation, rotation, _data.RotationSpeed * Time.deltaTime);
    }

    private bool IsLoaded()
    {
        if (Time.time >= _lastShotTime + _data.FireRate * _fireRateMultiplier)
            return true;
        else
            return false;
    }

    private void OnEnable()
    {
        _mainPart = transform.parent.Find("MainPart");
        _barrel = transform.Find("Barrel");
    }
}
