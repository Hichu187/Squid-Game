using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class ColorBlock_PlatformGenerator : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField] private GameObject _prefab;

        [Space]

        [SerializeField] private int _row;
        [SerializeField] private int _col;
        [SerializeField] private float _size;

        private void Start()
        {
            ConstructPlatforms();
        }

        private void ConstructPlatforms()
        {
            Vector3 min = new Vector3(_row * _size * -0.5f, 0f, _col * _size * -0.5f);

            for (int r = 0; r < _row; r++)
            {
                for (int c = 0; c < _col; c++)
                {
                    ColorBlock_Platform platform = _prefab.Create(transform).GetComponent<ColorBlock_Platform>();

                    platform.transformCached.localPosition = min + Vector3.right * r * _size + Vector3.forward * c * _size;
                }
            }
        }
    }
}
