using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using Vertx.Debugging;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    public class EasyObby_Stage : MonoCached
    {
        [Title("Reference")]
        [ReadOnly]
        [SerializeField] private AIWaypoint[] _aiWaypoints;

        [Title("Config")]
        [SerializeField] private Vector3 _boundsExpandMax;
        [SerializeField] private Vector3 _boundsExpandMin;

        [Space]

        [SerializeField] private Vector3 _startPointOffset;
        [SerializeField] private Vector3 _endPointOffset;

        [Space]

        [ReadOnly]
        [SerializeField] private Bounds _bounds;

        public Bounds bounds { get { return _bounds; } }

        public Vector3 startPointOffset { get { return _startPointOffset; } }
        public Vector3 endPointOffset { get { return _endPointOffset; } }

        public AIWaypoint[] aiWaypoints { get { return _aiWaypoints; } }

#if UNITY_EDITOR

        [Button("Validate")]
        private void Validate()
        {
            GetAIWaypoints();

            CenterObjects();

            EditorUtility.SetDirty(this);
        }

        private void GetAIWaypoints()
        {
            _aiWaypoints = GetComponentsInChildren<AIWaypoint>();

            for (int i = 0; i < _aiWaypoints.Length; i++)
            {
                _aiWaypoints[i].gameObject.name = $"AIWaypoint ({i})";
            }
        }

        private void CenterObjects()
        {
            UpdateBounds();

            for (int i = 0; i < transformCached.childCount; i++)
                transformCached.GetChild(i).localPosition -= _bounds.center;

            MeshRenderer[] meshRenderer = GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            UpdateBounds();
        }

        private void UpdateBounds()
        {
            Bounds bounds = new Bounds();

            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            bounds.max += _boundsExpandMax;
            bounds.min -= _boundsExpandMin;

            _bounds = bounds;
        }

        private void OnDrawGizmosSelected()
        {
            D.raw(new Shape.Box(transformCached.position, _bounds.extents, transformCached.rotation), Color.yellow);

            Vector3 checkpointCenter = transformCached.TransformPoint(Vector3.back * _bounds.size.z * 0.5f + _startPointOffset + Vector3.back * FactoryEasyObby.checkpointSize.z * 0.5f);

            D.raw(new Shape.Box(checkpointCenter, FactoryEasyObby.checkpointSize * 0.5f), Color.blue);

            checkpointCenter = transformCached.TransformPoint(Vector3.forward * _bounds.size.z * 0.5f + _endPointOffset + Vector3.forward * FactoryEasyObby.checkpointSize.z * 0.5f);

            D.raw(new Shape.Box(checkpointCenter, FactoryEasyObby.checkpointSize * 0.5f), Color.green);
        }

#endif
    }
}
