using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GamePlayEvents", menuName = "GamePlayEvents", order = 1)]
public class GamePlayEvents : ScriptableObject
{
    #region outputs
    public Action<bool> OnGameplayStatusChange;
    public Action<int> OnScoreUpdate;
    public Action<int> OnGameOver;
    public Action OnGameStarted;
    #endregion

    #region inputs
    public Action StartGame;
    public Action ClearGame;
    #endregion
}

