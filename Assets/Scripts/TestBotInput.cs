using System;
using System.Collections;
using UnityEngine;

public class TestBotInput : MonoBehaviour
{
    [SerializeField] private Tank _tank;
    public float MoveInDirectionInSec = 1f;
    private bool IsMoving = false;
    public bool StartMoving = false;

    private IEnumerator Move(bool forward)
    {
        if (StartMoving == false)
            yield break;

        float direction;
        if (forward)
            direction = 1;
        else
            direction = -1;

        DateTime beginning = DateTime.Now;
        IsMoving = true;

        while ((DateTime.Now - beginning).TotalSeconds < MoveInDirectionInSec)
        {
            _tank.Move(direction);
            yield return new WaitForFixedUpdate();
        }

        yield return Move(!forward);
    }

    private void FixedUpdate()
    {
        if (StartMoving && IsMoving == false)
            StartCoroutine(Move(true));
        if (StartMoving == false)
            IsMoving = false;
    }
}
