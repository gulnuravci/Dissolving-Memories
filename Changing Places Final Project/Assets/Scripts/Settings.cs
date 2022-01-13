using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{ 
    public Toggle easy;
    public Toggle medium;
    public Toggle hard;
    public Toggle insane;

    private void Start()
    {
        easy.isOn = true;
        medium.isOn = false;
        hard.isOn = false;
        insane.isOn = false;

        easy.onValueChanged.AddListener(delegate { OnToggleEasyChanged(); });
        medium.onValueChanged.AddListener(delegate { OnToggleMediumChanged(); });
        hard.onValueChanged.AddListener(delegate { OnToggleHardChanged(); });
        insane.onValueChanged.AddListener(delegate { OnToggleInsaneChanged(); });
    }

    public void OnToggleEasyChanged()
    {
        if (easy.isOn)
        {
            DifficultyVariables.easyPipesSpawned = 10;
            DifficultyVariables.mediumPipesSpawned = 20;
            DifficultyVariables.hardPipesSpawned = 30;
            DifficultyVariables.impossiblePipesSpawned = 40;
            DifficultyVariables.moreImpossiblePipesSpawned = 50;
            Debug.Log($"{DifficultyVariables.easyPipesSpawned},{DifficultyVariables.mediumPipesSpawned}, {DifficultyVariables.hardPipesSpawned}, {DifficultyVariables.impossiblePipesSpawned}, {DifficultyVariables.moreImpossiblePipesSpawned}");
        }
    }

    public void OnToggleMediumChanged()
    {
        if (medium.isOn)
        {
            DifficultyVariables.easyPipesSpawned = 20;
            DifficultyVariables.mediumPipesSpawned = 40;
            DifficultyVariables.hardPipesSpawned = 60;
            DifficultyVariables.impossiblePipesSpawned = 80;
            DifficultyVariables.moreImpossiblePipesSpawned = 100;
            Debug.Log($"{DifficultyVariables.easyPipesSpawned},{DifficultyVariables.mediumPipesSpawned}, {DifficultyVariables.hardPipesSpawned}, {DifficultyVariables.impossiblePipesSpawned}, {DifficultyVariables.moreImpossiblePipesSpawned}");
        }
    }

    public void OnToggleHardChanged()
    {
        if (hard.isOn)
        {
            DifficultyVariables.easyPipesSpawned = 30;
            DifficultyVariables.mediumPipesSpawned = 60;
            DifficultyVariables.hardPipesSpawned = 90;
            DifficultyVariables.impossiblePipesSpawned = 120;
            DifficultyVariables.moreImpossiblePipesSpawned = 150;
            Debug.Log($"{DifficultyVariables.easyPipesSpawned},{DifficultyVariables.mediumPipesSpawned}, {DifficultyVariables.hardPipesSpawned}, {DifficultyVariables.impossiblePipesSpawned}, {DifficultyVariables.moreImpossiblePipesSpawned}");
        }
    }

    public void OnToggleInsaneChanged()
    {
        if (insane.isOn)
        {
            DifficultyVariables.easyPipesSpawned = 40;
            DifficultyVariables.mediumPipesSpawned = 80;
            DifficultyVariables.hardPipesSpawned = 120;
            DifficultyVariables.impossiblePipesSpawned = 160;
            DifficultyVariables.moreImpossiblePipesSpawned = 200;
            Debug.Log($"{DifficultyVariables.easyPipesSpawned},{DifficultyVariables.mediumPipesSpawned}, {DifficultyVariables.hardPipesSpawned}, {DifficultyVariables.impossiblePipesSpawned}, {DifficultyVariables.moreImpossiblePipesSpawned}");
        }
    }
}
