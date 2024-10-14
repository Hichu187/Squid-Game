using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    public class TowerOfHell_Floor_Manager : MonoSingleton<TowerOfHell_Floor_Manager>
    {
        [System.Serializable]
        public struct Turn
        {
            public int index;
            public float angle;
        }

        [Title("Reference")]
        [ReadOnly]
        [SerializeField] private TowerOfHell_Checkpoint[] _checkpoints;

        [Title("Config")]
        [AssetList(Path = "_ROOT/Prefabs/TowerOfHell/Floor", AutoPopulate = true)]
        [SerializeField] private GameObject[] _floorPrefabs;
        [SerializeField] private Turn[] _turn;
        [SerializeField] private Material[] _materials;

        public TowerOfHell_Checkpoint[] checkpoints { get { return _checkpoints; } }

        protected override bool _dontDestroyOnLoad { get { return false; } }

        private void Start()
        {
            // Construct checkpoint index
            for (int i = 0; i < _checkpoints.Length; i++)
            {
                _checkpoints[i].SetIndex(i);
            }
        }

#if UNITY_EDITOR

        [Button]
        private void Generate()
        {
            transform.DestroyChildrenImmediate();

            float height = 0f;
            Quaternion rotation = Quaternion.identity;

            for (int i = 0; i < _floorPrefabs.Length; i++)
            {
                for (int j = 0; j < _turn.Length; j++)
                {
                    if (i == _turn[j].index)
                    {
                        rotation *= Quaternion.Euler(0f, _turn[j].angle, 0f);
                    }
                }

                TowerOfHell_Floor floor = (PrefabUtility.InstantiatePrefab(_floorPrefabs[i], transform) as GameObject).GetComponent<TowerOfHell_Floor>();

                floor.transform.position = Vector3.up * height;
                floor.transform.rotation = rotation;

                floor.wall.transform.rotation = Quaternion.identity;
                floor.wall.GetComponentInChildren<MeshRenderer>().material = _materials.GetLoop(i);

                height += floor.height;
            }

            _checkpoints = GetComponentsInChildren<TowerOfHell_Checkpoint>();

            FactoryTowerOfHell.checkpointCount = _checkpoints.Length;

            EditorUtility.SetDirty(FactoryTowerOfHell.instance);
        }

#endif
    }
}
