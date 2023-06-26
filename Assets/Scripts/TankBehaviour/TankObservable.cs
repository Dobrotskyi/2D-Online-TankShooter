using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TankObservable : MonoBehaviour, IPunObservable
{
    private PhotonView _view;

    private Rigidbody2D _rb;
    private Transform _turret;

    private Vector3 _netPos;
    private Quaternion _netRot;

    private Quaternion _turretRotationDirection;
    private Quaternion _netTurretRotation;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PreparationFinished() == false)
            return;

        if (stream.IsWriting)
        {
            stream.SendNext(_rb.position);
            stream.SendNext(_rb.transform.rotation);
            stream.SendNext(_rb.velocity);
            stream.SendNext(_rb.angularVelocity);

            stream.SendNext(_turret.transform.rotation);
        }
        else if (stream.IsReading)
        {
            _netPos = (Vector2)stream.ReceiveNext();
            _netRot = (Quaternion)stream.ReceiveNext();
            _rb.velocity = (Vector2)stream.ReceiveNext();
            _rb.angularVelocity = (float)stream.ReceiveNext();
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            _netPos += ((Vector3)_rb.velocity * lag);

            Quaternion lagRotation = Quaternion.Euler(0, 0, _rb.angularVelocity * lag);
            _netRot = Quaternion.Euler(_netRot.eulerAngles + lagRotation.eulerAngles);


            _netTurretRotation = (Quaternion)stream.ReceiveNext();
            _turretRotationDirection = Quaternion.Euler(_netTurretRotation.eulerAngles - _turret.transform.rotation.eulerAngles);
        }
    }

    private void FixedUpdate()
    {
        if (_view.IsMine || PreparationFinished() == false) return;

        _rb.position = Vector3.MoveTowards(_rb.position, _netPos, Time.fixedDeltaTime);
        _rb.transform.rotation = Quaternion.RotateTowards(_rb.transform.rotation, _netRot, Time.fixedDeltaTime * 100f);

    }

    private void Update()
    {
        if (_view.IsMine || PreparationFinished() == false) return;
        _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, _netTurretRotation, Time.deltaTime * _turretRotationDirection.eulerAngles.magnitude * 100f);
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
        _turret = transform.GetComponentInChildren<TurretPartBehav>().transform;
    }
}
