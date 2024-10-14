using Cysharp.Threading.Tasks;
using LFramework;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game
{
    public class ParkourRace_GUI_Progress : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private RectTransform _root;

        [Title("Config")]
        [SerializeField] private GameObject _itemPrefab;

        private float _width;

        private List<ParkourRace_GUI_Progress_Item> _items = new List<ParkourRace_GUI_Progress_Item>();

        private void Awake()
        {
            StaticBus<Event_ParkourRace_Gameplay_Start>.Subscribe(StaticBus_ParkourRace_Gameplay_Start);
        }

        private void OnDestroy()
        {
            StaticBus<Event_ParkourRace_Gameplay_Start>.Unsubscribe(StaticBus_ParkourRace_Gameplay_Start);
        }

        private void Start()
        {
            _width = _root.rect.width;
        }

        private void StaticBus_ParkourRace_Gameplay_Start(Event_ParkourRace_Gameplay_Start e)
        {
            UpdateProgressTask().AttachExternalCancellation(this.GetCancellationTokenOnDestroy()).SuppressCancellationThrow().Forget();
        }

        public void Add(ParkourRace_Character character)
        {
            ParkourRace_GUI_Progress_Item item = _itemPrefab.Create(_root, false).GetComponent<ParkourRace_GUI_Progress_Item>();

            item.Construct(character);
            item.rectTransform.anchoredPosition = Vector2.zero;

            _items.Add(item);
        }

        private async UniTask UpdateProgressTask()
        {
            ParkourRace_Checkpoint checkpointFirst = ParkourRace_Static.level.checkpoints.First();
            ParkourRace_Checkpoint checkpointLast = ParkourRace_Static.level.checkpoints.Last();

            while (true)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].character == null)
                        continue;

                    if (_items[i].character.checkpoint.index == checkpointLast.index)
                    {
                        _items[i].rectTransform.SetAnchoredPositionX(_width);
                    }
                    else
                    {
                        float d1 = Vector3.Distance(_items[i].character.character.transformCached.position, checkpointFirst.transformCached.position);
                        float d2 = Vector3.Distance(_items[i].character.character.transformCached.position, checkpointLast.transformCached.position);

                        _items[i].rectTransform.SetAnchoredPositionX(Mathf.Clamp01(d1 / (d1 + d2)) * _width);
                    }
                }

                await UniTask.Yield();
            }
        }
    }
}
