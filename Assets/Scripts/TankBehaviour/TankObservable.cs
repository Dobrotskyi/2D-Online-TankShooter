using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TankObservable : MonoBehaviour, IPunObservable
{
    private const float TELEPORT_IF_DISTANCE_GRATER_THAN = 1.5f;
    private const float ROTATION_MULTIPLIER = 100f;

    private float _tankRotationSpeed = -1;
    private float _tankAcceleration = -1;

    private PhotonView _view;

    private Rigidbody2D _rb;
    private Transform _turret;

    private Vector2 _netPos;
    private float _netRot;
    private Vector2 _netVelocity;
    private float _netAngularVelocity;

    private Quaternion _turretRotationDirection;
    private Quaternion _netTurretRotation;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PreparationFinished() == false)
            return;

        if (stream.IsWriting)
        {
            stream.SendNext(_rb.position);
            stream.SendNext(_rb.rotation);
            stream.SendNext(_rb.velocity);
            stream.SendNext(_rb.angularVelocity);

            stream.SendNext(_turret.transform.rotation);
        }
        else if (stream.IsReading)
        {
            _netPos = (Vector2)stream.ReceiveNext();
            _netRot = (float)stream.ReceiveNext();
            _netVelocity = (Vector2)stream.ReceiveNext();
            _netAngularVelocity = (float)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            _netPos += _netVelocity * lag;
            _netRot += _netAngularVelocity * lag;

            _netTurretRotation = (Quaternion)stream.ReceiveNext();
            _turretRotationDirection = Quaternion.Euler(_netTurretRotation.eulerAngles - _turret.transform.rotation.eulerAngles);
        }
    }

    private void FixedUpdate()
    {
        if (_view.IsMine || PreparationFinished() == false) return;

        if (Vector3.Distance(_rb.position, _netPos) > TELEPORT_IF_DISTANCE_GRATER_THAN)
            _rb.position = _netPos;

        _rb.velocity = Vector3.Lerp(_rb.velocity, _netVelocity, Time.fixedDeltaTime * _tankAcceleration);
        _rb.angularVelocity = Mathf.Lerp(_rb.angularVelocity, _netAngularVelocity, Time.fixedDeltaTime * _tankRotationSpeed);

        _rb.position = Vector3.MoveTowards(_rb.position, _netPos, Time.fixedDeltaTime);
        _rb.rotation = Quaternion.RotateTowards(Quaternion.Euler(0, 0, _rb.rotation),
                       Quaternion.Euler(0, 0, _netRot), Time.fixedDeltaTime * _tankRotationSpeed * 0.8f)
                       .eulerAngles.z;

    }

    private void Update()
    {
        if (_view.IsMine || PreparationFinished() == false) return;
        float step = Time.deltaTime * _turretRotationDirection.eulerAngles.magnitude * ROTATION_MULTIPLIER;
        _turret.transform.rotation = Quaternion.Lerp(_turret.transform.rotation, _netTurretRotation, step);
    }

    private bool PreparationFinished()
    {
        if (_turret == null || _rb == null)
            return false;
        else return true;
    }

    private void OnEnable()
    {
        _view = GetComponent<PhotonView>();
        StartCoroutine(GetComponents());
    }

    private IEnumerator GetComponents()
    {
        while (transform.GetComponentInChildren<MainPartBehav>() == null || transform.GetComponentInChildren<TurretPartBehav>() == null)
            yield return new WaitForEndOfFrame();


        _rb = transform.GetComponentInChildren<Rigidbody2D>();
        MainPartBehav mainPartBehav = transform.GetComponentInChildren<MainPartBehav>();
        _turret = transform.GetComponentInChildren<TurretPartBehav>().transform;

        _tankAcceleration = mainPartBehav.Acceleration;
        _tankRotationSpeed = mainPartBehav.RotationSpeed;
    }
}
