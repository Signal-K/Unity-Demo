namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GameCreator.Core;
	using GameCreator.Characters;
    using System.Globalization;

    public static class SkinmeshManager
	{
        private const string GARMENT_NAME = "Skinwear-{0}";

        private class Bones : Dictionary<string, Transform>
        {
            public Bones(Transform transform)
            {
                GatherBones(transform);
            }

            private void GatherBones(Transform transform)
            {
                if (this.ContainsKey(transform.name))
                {
                    Remove(transform.name);
                }

                Add(transform.name, transform);
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; ++i)
                {
                    this.GatherBones(transform.GetChild(i));
                }
            }

            public Transform Get(string name)
            {
                if (this.ContainsKey(name)) return this[name];
                return null;
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static GameObject Wear(GameObject prefab, Character character)
		{
            if (prefab == null || character == null) return null;

            CharacterAnimator animator = character.GetCharacterAnimator();
            if (animator == null || animator.animator == null) return null;

            Transform root = animator.animator.transform;
            Bones bones = new Bones(root);

            GameObject garment = GameObject.Instantiate<GameObject>(prefab);
            garment.name = string.Format(GARMENT_NAME, prefab.name);
            SkinnedMeshRenderer[] renderers = garment.GetComponentsInChildren<SkinnedMeshRenderer>();
            Transform target = AddAsChild(garment.transform, root);

            for (int i = 0; i < renderers.Length; ++i)
            {
                SkinnedMeshRenderer renderer = AddSkinnedMeshRenderer(renderers[i], target);
                renderer.bones = GetTransforms(renderers[i].bones, bones);
            }

            return target.gameObject;
        }

        public static GameObject TakeOff(GameObject prefab, Character character)
        {
            if (prefab == null || character == null) return null;

            CharacterAnimator animator = character.GetCharacterAnimator();
            if (animator == null || animator.animator == null) return null;

            string wearName = string.Format(GARMENT_NAME, prefab.name);
            Transform wear = animator.animator.transform.Find(wearName);
            if (wear != null) GameObject.Destroy(wear.gameObject);

            return null;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static Transform AddAsChild(Transform source, Transform parent)
        {
            source.SetParent(parent);
            for (int i = source.childCount - 1; i >= 0; --i)
            {
                GameObject.Destroy(source.GetChild(i).gameObject);
            }

            return source;
        }

        private static SkinnedMeshRenderer AddSkinnedMeshRenderer(SkinnedMeshRenderer source, Transform parent)
        {
            GameObject instance = new GameObject(source.name);
            instance.transform.SetParent(parent);

            SkinnedMeshRenderer instanceMesh = instance.AddComponent<SkinnedMeshRenderer>();
            instanceMesh.sharedMesh = source.sharedMesh;
            instanceMesh.materials = source.materials;
            return instanceMesh;
        }

        private static Transform[] GetTransforms(Transform[] sources, Bones bones)
        {
            Transform[] transforms = new Transform[sources.Length];
            for (int i = 0; i < sources.Length; ++i)
            {
                transforms[i] = bones.Get(sources[i].name);
            }
            
            return transforms;
        }
    }
}