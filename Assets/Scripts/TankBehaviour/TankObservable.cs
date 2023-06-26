using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TankSerialize : MonoBehaviour, IPunObservable
{
    private Rigidbody2D _rb;
    private MainPartBehav _mainPartBehav;
    private TurretPartBehav _turretPartBehav;

    private Vector3 _netPos;
    private Quaternion _netRot;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (ReadyToSerialize() == false)
            return;

        if (stream.IsWriting)
        {
            stream.SendNext(_rb.transform.position);
            stream.SendNext(_rb.transform.rotation);
            stream.SendNext(_mainPartBehav.Velocity);
            stream.SendNext(_mainPartBehav.AngularVelocity);
        }
        else if (stream.IsReading)
        {
            _netPos = (Vector3)stream.ReceiveNext();
            _netRot = (Quaternion)stream.ReceiveNext();
            _rb.velocity = (Vector3)stream.ReceiveNext();
            _rb.angularVelocity = (float)stream.ReceiveNext();
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            _netPos += ((Vector3)_rb.velocity * lag);

            Quaternion lagRotation = Quaternion.Euler(0, 0, _rb.angularVelocity * lag);
            _netRot = Quaternion.Euler(_netRot.eulerAngles + lagRotation.eulerAngles);
        }
    }

    private void FixedUpdate()
    {
        _rb.position = Vector3.MoveTowards(_rb.position, _netPos, Time.fixedDeltaTime);
        _rb.transform.rotation = Quaternion.RotateTowards(transform.rotation, _netRot, Time.fixedDeltaTime * 100.0f);
    }

    private bool ReadyToSerialize()
    {
        if (_mainPartBehav == null || _turretPartBehav == null || _rb == null)
            return false;
        else return true;
    }

    private void OnEnable()
    {
        StartCoroutine(GetComponents());
    }

    private IEnumerator GetComponents()
    {
        while (transform.GetComponentInChildren<MainPartBehav>() || transform.GetComponentInChildren<TurretPartBehav>())
            yield return new WaitForEndOfFrame();

        _rb = transform.GetComponentInChildren<Rigidbody2D>();
        _mainPartBehav = transform.GetComponentInChildren<MainPartBehav>();
        _turretPartBehav = transform.GetComponentInChildren<TurretPartBehav>();
    }
}
