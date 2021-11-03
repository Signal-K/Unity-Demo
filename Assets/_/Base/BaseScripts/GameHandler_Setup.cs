using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;
using GridPathfindingSystem;

public class GameHandler_Setup : MonoBehaviour {

    public static GridPathfinding gridPathfinding;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private CharacterAimHandler characterAimHandler;

    private void Start() {
        //Sound_Manager.Init();
        cameraFollow.Setup(GetCameraPosition, () => 60f, true, true);

        FunctionPeriodic.Create(SpawnEnemy, 1.5f);
        
        gridPathfinding = new GridPathfinding(new Vector3(-400, -400), new Vector3(400, 400), 5f);
        gridPathfinding.RaycastWalkable();

        EnemyHandler.Create(new Vector3(0, 0));

        characterAimHandler.OnShoot += CharacterAimHandler_OnShoot;
    }

    private void CharacterAimHandler_OnShoot(object sender, CharacterAimHandler.OnShootEventArgs e) {
        Shoot_Flash.AddFlash(e.gunEndPointPosition);
        WeaponTracer.Create(e.gunEndPointPosition, e.shootPosition);
        UtilsClass.ShakeCamera(.6f, .05f);

        // Any enemy hit?
        RaycastHit2D raycastHit = Physics2D.Raycast(e.gunEndPointPosition, (e.shootPosition - e.gunEndPointPosition).normalized, Vector3.Distance(e.gunEndPointPosition, e.shootPosition));
        if (raycastHit.collider != null) {
            EnemyHandler enemyHandler = raycastHit.collider.gameObject.GetComponent<EnemyHandler>();
            if (enemyHandler != null) {
                enemyHandler.Damage(characterAimHandler);
            }
        }
    }

    private Vector3 GetCameraPosition() {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        Vector3 playerToMouseDirection = mousePosition - characterAimHandler.GetPosition();
        return characterAimHandler.GetPosition() + playerToMouseDirection * .3f;
    }

    private void SpawnEnemy() {
        Vector3 spawnPosition = characterAimHandler.GetPosition() + UtilsClass.GetRandomDir() * 100f;
        EnemyHandler.Create(spawnPosition);
    }
}
