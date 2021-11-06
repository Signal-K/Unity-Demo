/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

public class Player : MonoBehaviour {

    [SerializeField] private Transform pfEffect;
    [SerializeField] private Texture2D baseTexture;
    [SerializeField] private Texture2D headTexture;
    [SerializeField] private Texture2D helmet1Texture;
    [SerializeField] private Texture2D helmet2Texture;

    private Player_Base playerBase;
    private Material material;
    private Color materialTintColor;
    private LevelSystemAnimated levelSystemAnimated;

    public enum Equip {
        None,
        Helmet_1,
        Helmet_2,
    }

    private void Awake() {
        playerBase = gameObject.GetComponent<Player_Base>();
        material = transform.Find("Body").GetComponent<MeshRenderer>().material;
        materialTintColor = new Color(1, 0, 0, 0);
        SetEquip(Equip.None);
    }

    private void Update() {
        if (materialTintColor.a > 0) {
            float tintFadeSpeed = 6f;
            materialTintColor.a -= tintFadeSpeed * Time.deltaTime;
            material.SetColor("_Tint", materialTintColor);
        }
    }

    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated) {
        this.levelSystemAnimated = levelSystemAnimated;

        levelSystemAnimated.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e) {
        PlayVictoryAnim();
        SpawnParticleEffect();
        Flash(new Color(1, 1, 1, 1));

        SetHealthBarSize(1f + levelSystemAnimated.GetLevelNumber() * .1f);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    private void PlayVictoryAnim() {
        playerBase.PlayVictoryAnim();
    }

    private void SpawnParticleEffect() {
        Transform effect = Instantiate(pfEffect, transform);
        FunctionTimer.Create(() => Destroy(effect.gameObject), 3f);
    }

    private void Flash(Color flashColor) {
        materialTintColor = flashColor;
        material.SetColor("_Tint", materialTintColor);
    }

    private void SetHealthBarSize(float healthBarSize) {
        transform.Find("healthBar").localScale = new Vector3(.7f * healthBarSize, 1, 1);
    }

    public void SetEquip(Equip equip) {
        Texture2D texture = new Texture2D(512, 512, TextureFormat.RGBA32, true);

        Color[] spritesheetBasePixels = baseTexture.GetPixels(0, 0, 512, 512);
        texture.SetPixels(0, 0, 512, 512, spritesheetBasePixels);

        Color[] headPixels;
        switch (equip) {
        default:
        case Equip.None:
            headPixels = headTexture.GetPixels(0, 0, 128, 128);
            break;
        case Equip.Helmet_1:
            headPixels = helmet1Texture.GetPixels(0, 0, 128, 128);
            break;
        case Equip.Helmet_2:
            headPixels = helmet2Texture.GetPixels(0, 0, 128, 128);
            break;
        }
        texture.SetPixels(4, 384, 128, 128, headPixels);

        texture.Apply();

        material.mainTexture = texture;
    }

}
