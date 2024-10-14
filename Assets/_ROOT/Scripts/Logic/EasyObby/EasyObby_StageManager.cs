using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    public class EasyObby_StageManager : MonoSingleton<EasyObby_StageManager>
    {
        [System.Serializable]
        public class Turn
        {
            [SerializeField] private int _index;
            [SerializeField] private Direction _direction;

            public int index { get { return _index; } }
            public Direction direction { get { return _direction; } }
        }

        [Title("Reference")]
        [ReadOnly]
        [SerializeField] private EasyObby_Checkpoint[] _checkpoints;

        [ReadOnly]
        [SerializeField] private EasyObby_Stage[] _stages;

        [Title("Config")]
        [SerializeField] private GameObject _checkpointPrefab;
        [SerializeField] private GameObject[] _stagePrefabs;
        [SerializeField] private Turn[] _turns;

        protected override bool _dontDestroyOnLoad { get { return false; } }

        public static EasyObby_Checkpoint[] checkpoints { get { return Instance._checkpoints; } }

        public static EasyObby_Stage[] stages { get { return Instance._stages; } }

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < _checkpoints.Length; i++)
            {
                _checkpoints[i].Construct(i);
            }
        }

#if UNITY_EDITOR

        [Button]
        private void Generate()
        {
            transform.DestroyChildrenImmediate();

            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;

            _stages = new EasyObby_Stage[_stagePrefabs.Length];

            for (int i = 0; i < _stagePrefabs.Length; i++)
            {
                for (int j = 0; j < _turns.Length; j++)
                {
                    if (_turns[j].index == i)
                    {
                        switch (_turns[j].direction)
                        {
                            case Direction.Left:
                                rotation *= Quaternion.Euler(0f, -90f, 0f);
                                break;
                            case Direction.Right:
                                rotation *= Quaternion.Euler(0f, 90f, 0f);
                                break;
                        }
                    }
                }

                Transform checkpointTransform = SpawnCheckpoint();

                EasyObby_Stage stage = (UnityEditor.PrefabUtility.InstantiatePrefab(_stagePrefabs[i], transform) as GameObject).GetComponent<EasyObby_Stage>();

                _stages[i] = stage;

                position = checkpointTransform.TransformPoint(-stage.startPointOffset + Vector3.forward * (FactoryEasyObby.checkpointSize.z + stage.bounds.size.z) * 0.5f);

                stage.transform.position = position;
                stage.transformCached.rotation = rotation;

                position = stage.transformCached.TransformPoint(stage.endPointOffset + Vector3.forward * (FactoryEasyObby.checkpointSize.z + stage.bounds.size.z) * 0.5f);
            }

            // Spawn last checkpoint
            SpawnCheckpoint();

            _checkpoints = GetComponentsInChildren<EasyObby_Checkpoint>();

            FactoryEasyObby.checkpointCount = _checkpoints.Length;

            EditorUtility.SetDirty(FactoryEasyObby.instance);
            EditorUtility.SetDirty(this);

            Transform SpawnCheckpoint()
            {
                Transform checkpointTransform = (UnityEditor.PrefabUtility.InstantiatePrefab(_checkpointPrefab, transform) as GameObject).transform;

                checkpointTransform.position = position;
                checkpointTransform.rotation = rotation;

                return checkpointTransform;
            }
        }

#endif
    }
}
