using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsGuide : MonoBehaviour
{
    private static bool s_showGuide = true;
    [SerializeField] private GameObject _body;

    public void CloseGuide(bool showAgain)
    {
        s_showGuide = showAgain;
        _body.SetActive(false);

        if (s_showGuide == false)
            Destroy(gameObject);
    }

    private void Start()
    {
        if (s_showGuide)
            _body.SetActive(true);
        else _body.SetActive(false);
    }
}
