using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private Text[] scoreTexts;
    [SerializeField] private Text[] highScoreTexts;
    [SerializeField] private GamePlayEvents gamePlayEvents;
    [SerializeField] private UIAnimatedPanel panelMenu, panelGame, panelResult;
#pragma warning restore CS0649

    private void Start()
    {
        Application.targetFrameRate = 60;

        gamePlayEvents.OnGameStarted += GameStarted;
        gamePlayEvents.OnGameOver += GameOver;
        gamePlayEvents.OnScoreUpdate += ScoreUpdate;

        HighScoreUpdate();

        panelMenu.Open();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        gamePlayEvents.ClearGame?.Invoke();
        gamePlayEvents.StartGame?.Invoke();   
    }

    public void Close()
    {
        gamePlayEvents.ClearGame?.Invoke();
        panelGame.Close();
        panelMenu.Open();
    }

    private void ScoreUpdate(int score)
    {
        foreach (var text in scoreTexts)
            text.text = score.ToString();
    }

    private void GameOver(int score)
    {
        if (score > UserData.GetHighScore())
        {
            UserData.SetHighScore(score);
            HighScoreUpdate();
        }

        panelResult.Open();
    }

    private void GameStarted()
    {
        panelMenu.Close();
        panelGame.Open();
    }

    private void HighScoreUpdate()
    {
        int score = UserData.GetHighScore();
        foreach (var text in highScoreTexts)
            text.text = score.ToString();
    }

}