using System;
using UnityEngine;

public class SaveGame
{

    private const string HIGHEST_LEVEL_UNLOCKED_KEY = "max_level";

    public int GetScore(int level)
    {
        return PlayerPrefs.GetInt(GetLevelSaveName(level));
    }

    public void SaveScore(int level, int score)
    {
        PlayerPrefs.SetInt(GetLevelSaveName(level), score);
    }

    public bool IsHighscore(int level, int score)
    {
        return GetScore(level) < score;
    }

    public void SetHighestUnlockedLevel(int level)
    {
        if (level < GetHighestUnlockedLevel())
        {
            return;
        }
        PlayerPrefs.SetInt(HIGHEST_LEVEL_UNLOCKED_KEY, level);
    }

    public int GetHighestUnlockedLevel()
    {
        return PlayerPrefs.GetInt(HIGHEST_LEVEL_UNLOCKED_KEY);
    }

    private string GetLevelSaveName(int level)
    {
        return "level_" + level;
    }
}
