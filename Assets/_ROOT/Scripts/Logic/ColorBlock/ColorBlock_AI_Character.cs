using Cysharp.Threading.Tasks;
using LFramework;
using UnityEngine;

namespace Game
{
    public class ColorBlock_AI_Character : MonoBehaviour
    {
        private int _surviveCount;
        private bool _isSurvive;
        private AI _ai;

        private void Awake()
        {
            _ai = GetComponent<AI>();

            StaticBus<Event_ColorBlock_RoundStart>.Subscribe(StaticBus_ColorBlock_RoundStart);
        }

        private void OnDestroy()
        {
            StaticBus<Event_ColorBlock_RoundStart>.Unsubscribe(StaticBus_ColorBlock_RoundStart);
        }

        private async void Start()
        {
            _ai.eventChaseComplete += AI_EventChaseComplete;
            _ai.eventIdleComplete += AI_EventIdleComplete;

            await UniTask.WaitForSeconds(Random.Range(0.5f, 1.5f), cancellationToken: this.GetCancellationTokenOnDestroy());

            _ai.Chase(ColorBlock_PlatformManager.Instance.platforms.GetRandom().transformCached.position);

        }

        private void AI_EventIdleComplete()
        {
            ColorBlock_Platform platform;

            if (!_isSurvive)
            {
                platform = ColorBlock_PlatformManager.Instance.GetRandomPlatformWrong();
                _ai.Chase(platform.transformCached.position);
            }
        }

        private void AI_EventChaseComplete()
        {
            _ai.Idle();
        }

        private void StaticBus_ColorBlock_RoundStart(Event_ColorBlock_RoundStart e)
        {
            ColorBlock_Platform platform;

            _isSurvive = _surviveCount > 0;

            if (_isSurvive)
                platform = ColorBlock_PlatformManager.Instance.GetRandomPlatformRight();
            else
                platform = ColorBlock_PlatformManager.Instance.GetRandomPlatformWrong();

            _ai.Chase(platform.transformCached.position);

            _surviveCount--;
        }

        public void Construct(int surviveCount)
        {
            _surviveCount = surviveCount;
        }
    }
}
