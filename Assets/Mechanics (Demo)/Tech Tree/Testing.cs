using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    [SerializeField]private LevelWindow levelWindow;
    private void Awake() {
        LevelSystem levelSystem = new LevelSystem();
        Debug.Log(levelSystem.GetLevelNumber()); // Print the current level
        levelSystem.AddExperience(50);
        Debug.Log(levelSystem.GetLevelNumber());
        levelSystem.AddExperience(60);
        Debug.Log(levelSystem.GetLevelNumber());

        levelWindow.SetLevelSystem(levelSystem);
    }
}
