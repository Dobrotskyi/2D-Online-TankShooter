using TMPro;
using UnityEngine;

public class NameTag : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nicknameField;
    private Transform _followedObj;
    private float _yOfsset;

    public NameTag SetupNameTag(string nickname, Transform followObj)
    {
        _nicknameField.text = nickname;
        _followedObj = followObj;
        return this;
    }

    public void SetYOffset(float offset)
    {
        _yOfsset = offset;
    }

    private void LateUpdate()
    {
        if (_followedObj == null)
            return;
        transform.rotation = Quaternion.identity;
        Vector2 newPos = _followedObj.position;
        newPos.y -= _yOfsset;
        transform.position = newPos;
    }
}
