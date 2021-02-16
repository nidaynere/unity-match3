using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private Text scoreText;
    [SerializeField] private GamePlayEvents gamePlayEvents;
#pragma warning restore CS0649

    private void Start()
    {
        gamePlayEvents.OnScoreUpdate += ScoreUpdate;
    }

    public void StartGame()
    {
        gamePlayEvents.ClearGame?.Invoke();
        gamePlayEvents.StartGame?.Invoke();   
    }

    private void ScoreUpdate(int score)
    {
        scoreText.text = score.ToString();
    }
}