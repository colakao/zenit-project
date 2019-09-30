using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Difficulty
{
    public enum GameMode
    {
        EASY = 0,
        MEDIUM = 1,
        HARD = 2
    };

    public static GameMode gameMode = GameMode.EASY;
   

    public static void SetDifficulty(float n)
    {
        gameMode = (GameMode)n;
        gameMode.ToString();
        Debug.Log("Difficulty set to " + gameMode);
    }

    public static void GetDifficulty()
    {
        Debug.Log(gameMode);
    }
}
