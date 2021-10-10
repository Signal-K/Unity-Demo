using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Target : MonoBehaviour {

    private static List<Target> targetList;
    
    public static Target GetClosest(Vector3 position, float maxRange) {
        Target closest = null;

        Vector3 targetSpriteOffset = new Vector3(0, 7.35f);

        foreach (Target target in targetList) {
            if (Vector3.Distance(position, target.GetPosition() + targetSpriteOffset) <= maxRange) {
                if (closest == null) {
                    closest = target;
                } else {
                    if (Vector3.Distance(position, target.GetPosition() + targetSpriteOffset) < Vector3.Distance(position, closest.GetPosition() + targetSpriteOffset)) {
                        closest = target;
                    }
                }
            }
        }
        return closest;
    }



    private Animation animation;

    private void Awake() {
        if (targetList == null) targetList = new List<Target>();
        targetList.Add(this);
        animation = transform.Find("Sprite").GetComponent<Animation>();
    }

    public void Damage() {
        animation.Play();
        Instantiate(GameAssets.i.pfSmoke, transform.position + new Vector3(0, 7.35f) + UtilsClass.GetRandomDir() * Random.Range(0f, 2.2f), Quaternion.identity);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

}
