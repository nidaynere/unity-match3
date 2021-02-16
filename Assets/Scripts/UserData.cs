using UnityEngine;

public struct UserData
{
    const string userDataNameOnPlayerPrefs = "userData";
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(userDataNameOnPlayerPrefs);
    }

    public static void SetHighScore(int score)
    {
        PlayerPrefs.SetInt(userDataNameOnPlayerPrefs, score);
    }
}