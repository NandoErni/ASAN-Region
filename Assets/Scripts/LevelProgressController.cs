using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressController : MonoBehaviour
{
    public Button[] Buttons;

    private SaveGame _saveGame = new();

    // Start is called before the first frame update
    void Start()
    {
        int unlockedLevels = _saveGame.GetHighestUnlockedLevel();
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (i > unlockedLevels)
            {
                Buttons[i].interactable = false;
            }
        }
    }
}
