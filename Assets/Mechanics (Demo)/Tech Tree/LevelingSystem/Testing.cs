/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] private LevelWindow levelWindow;
    [SerializeField] private Player player;
    [SerializeField] private EquipWindow equipWindow;
    
    private void Awake() {
        LevelSystem levelSystem = new LevelSystem();
        levelWindow.SetLevelSystem(levelSystem);
        equipWindow.SetLevelSystem(levelSystem);

        LevelSystemAnimated levelSystemAnimated = new LevelSystemAnimated(levelSystem);
        levelWindow.SetLevelSystemAnimated(levelSystemAnimated);
        player.SetLevelSystemAnimated(levelSystemAnimated);
    }

}
