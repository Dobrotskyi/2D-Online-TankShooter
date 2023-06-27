using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher : MonoBehaviourSingleton<GameLauncher>, IUseLoading
{
    public event Action StartLoading;
    public event Action EndLoading;

    private void Start()
    {
        StartLoading?.Invoke();
        PlayerPreparationChecker.Instance.GameIsReadyToLaunch += LaunchGame;
    }

    private void OnDisable()
    {
        PlayerPreparationChecker.Instance.GameIsReadyToLaunch -= LaunchGame;
    }

    private void LaunchGame()
    {
        EndLoading?.Invoke();
    }
}
