using LFramework;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ColorBlock_PlatformManager : MonoSingleton<ColorBlock_PlatformManager>
    {
        [Title("Config")]
        [SerializeField] private GameObject _prefab;

        private ColorBlock_Platform[] _platforms;

        private ColorBlock_ColorConfig _colorConfig;

        protected override bool _dontDestroyOnLoad { get { return false; } }

        public ColorBlock_Platform[] platforms { get { return _platforms; } }

        private void Start()
        {
            _platforms = GetComponentsInChildren<ColorBlock_Platform>();

            ShufflePlatformColor();
        }

        public void ShufflePlatformColor()
        {
            _platforms.Shuffle();

            int colorEach = _platforms.Length / FactoryColorBlock.colorConfigs.Length;

            for (int i = 0; i < _platforms.Length; i++)
                _platforms[i].Construct(FactoryColorBlock.colorConfigs.GetClamp(i / colorEach));
        }

        public void ShufflePlatformColor(ColorBlock_ColorConfig config, int count)
        {
            _colorConfig = config;

            _platforms.Shuffle();

            List<ColorBlock_ColorConfig> configRemain = new List<ColorBlock_ColorConfig>(FactoryColorBlock.colorConfigs);
            configRemain.Remove(config);

            for (int i = 0; i < _platforms.Length; i++)
            {
                if (i < count)
                    _platforms[i].Construct(config);
                else
                    _platforms[i].Construct(configRemain.GetRandom());
            }
        }

        public ColorBlock_Platform GetRandomPlatformRight()
        {
            _platforms.Shuffle();

            for (int i = 0; i < _platforms.Length; i++)
            {
                if (_platforms[i].colorConfig == _colorConfig)
                    return _platforms[i];
            }

            return _platforms.GetRandom();
        }

        public ColorBlock_Platform GetRandomPlatformWrong()
        {
            _platforms.Shuffle();

            for (int i = 0; i < _platforms.Length; i++)
            {
                if (_platforms[i].colorConfig != _colorConfig)
                    return _platforms[i];
            }

            return _platforms.GetRandom();
        }
    }
}
