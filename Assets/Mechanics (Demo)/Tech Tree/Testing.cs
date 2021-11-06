using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    [SerializeField]private LevelWindow levelWindow;
    private void Awake() {
        LevelSystem levelSystem = new LevelSystem();
        levelWindow.SetLevelSystem(levelSystem);
    }
}
