using LFramework;
using UnityEngine;

namespace Game
{
    public class ColorBlock_Platform : MonoCached
    {
        private MeshRenderer _meshRenderer;

        private ColorBlock_ColorConfig _colorConfig;

        public ColorBlock_ColorConfig colorConfig { get { return _colorConfig; } }

        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();

            StaticBus<Event_ColorBlock_PlatformHide>.Subscribe(StaticBus_PlatformHide);
            StaticBus<Event_ColorBlock_PlatformRestore>.Subscribe(StaticBus_PlatformRestore);
        }

        private void OnDestroy()
        {
            StaticBus<Event_ColorBlock_PlatformHide>.Unsubscribe(StaticBus_PlatformHide);
            StaticBus<Event_ColorBlock_PlatformRestore>.Unsubscribe(StaticBus_PlatformRestore);
        }

        private void StaticBus_PlatformHide(Event_ColorBlock_PlatformHide e)
        {
            if (_colorConfig != e.colorConfig)
                gameObjectCached.SetActive(false);
        }

        private void StaticBus_PlatformRestore(Event_ColorBlock_PlatformRestore e)
        {
            gameObjectCached.SetActive(true);
        }

        public void Construct(ColorBlock_ColorConfig colorConfig)
        {
            _colorConfig = colorConfig;

            _meshRenderer.material = colorConfig.material;
        }
    }
}
