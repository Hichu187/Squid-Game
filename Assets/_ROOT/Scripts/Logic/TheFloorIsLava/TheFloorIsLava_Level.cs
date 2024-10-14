using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using Vertx.Debugging;

namespace Game
{
    public class TheFloorIsLava_Level : MonoCached
    {
        [Title("Reference")]
        [SerializeField] private TheFloorIsLava_LevelPoints _points;

        [Title("Config")]
        [SerializeField] private int _lavaDelay = 10;
        [SerializeField] private float _lavaDuration = 20f;
        [SerializeField] private float _lavaHeight = 10f;

        [Space]

        [ReadOnly]
        [SerializeField] private Bounds _bounds;

        public int lavaDelay { get { return _lavaDelay; } }
        public float lavaDuration { get { return _lavaDuration; } }
        public float lavaHeight { get { return _lavaHeight; } }

        public Bounds bounds { get { return _bounds; } }

        public TheFloorIsLava_LevelPoints points { get { return _points; } }

#if UNITY_EDITOR
        [Button]
        private void UpdateBounds()
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            _bounds = new Bounds();

            for (int i = 0; i < renderers.Length; i++)
            {
                _bounds.Encapsulate(renderers[i].bounds);

                renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            _points = GetComponentInChildren<TheFloorIsLava_LevelPoints>();

            UnityEditor.EditorUtility.SetDirty(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            D.raw(_bounds, Color.yellow);

            Bounds boundsLava = _bounds;

            Vector3 max = boundsLava.max;
            max.y = boundsLava.min.y + _lavaHeight;

            boundsLava.max = max;

            D.raw(boundsLava, Color.red);
        }
#endif
    }
}