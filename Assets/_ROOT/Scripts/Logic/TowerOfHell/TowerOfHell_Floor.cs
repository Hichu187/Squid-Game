using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class TowerOfHell_Floor : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private TowerOfHell_Wall _wall;

        [Title("Config")]
        [SerializeField] private float _height;

        public float height { get { return _height; } }

        public TowerOfHell_Wall wall { get { return _wall; } }

#if UNITY_EDITOR

        [Button]
        private void UpdateFloor()
        {
            MeshRenderer[] meshRenderer = GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }

        private void OnValidate()
        {
            if (Application.isPlaying || UnityEditor.Selection.objects == null || !UnityEditor.Selection.objects.Contains(gameObject))
                return;

            _wall.SetHeight(_height);
        }
#endif
    }
}
