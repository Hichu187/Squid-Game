using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class CharacterRenderer : MonoBehaviour
    {
        private Character _character;

        private CharacterSkin _skin;

        private CharacterSkinConfig _skinConfig;

        public CharacterSkinConfig skinConfig { get { return _skinConfig; } }

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void OnDestroy()
        {
            if (_skin != null)
                Addressables.ReleaseInstance(_skin.gameObjectCached);
        }

        private void Start()
        {
            _character.eventBoosterUpdate += Character_EventBoosterUpdate;
        }

        private void Character_EventBoosterUpdate()
        {
            _skin.objJetPack.SetActive(_character.boosterJetpackEnabled);

            for (int i = 0; i < _skin.objShoes.Length; i++)
                _skin.objShoes[i].SetActive(_character.boosterShoesEnabled);
        }

        public async UniTaskVoid LoadSkin(CharacterSkinConfig config)
        {
            if (_skin != null)
            {
                Addressables.ReleaseInstance(_skin.gameObjectCached);
                _skin = null;
            }

            _skinConfig = config;

            var handle = config.prefab.InstantiateAsync(transform);

            await handle;

            _skin = handle.Result.GetComponent<CharacterSkin>();
            _skin.transformCached.localPosition = Vector3.zero;
            _skin.transformCached.localRotation = Quaternion.identity;

            Character_EventBoosterUpdate();
        }
    }
}
