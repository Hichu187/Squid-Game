using DG.Tweening;
using LFramework;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class TheFloorIsLava_LevelSelection : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Transform _root;

        [Title("Config")]
        [SerializeField] private GameObject _itemPrefab;

        [Space]

        [SerializeField] private float _itemWidth;
        [SerializeField] private float _itemSpacing;

        [Space]

        [SerializeField] private Vector2 _itemScaleRange;
        [SerializeField] private AnimationCurve _itemAnimationCurve;

        [Space]

        [SerializeField] private int _shuffleCount = 30;
        [SerializeField] private float _shuffleDuration = 3.0f;
        [SerializeField] private Ease _shuffleEase = Ease.OutSine;

        [Space]

        [SerializeField] private UnityEvent _eventComplete;

        private List<TheFloorIsLava_LevelConfig> _configs;
        private int _configIndex = 0;
        private int _configShuffleCount = 0;
        private TheFloorIsLava_LevelConfig[] _configFixed;

        private float _leftX;

        private List<TheFloorIsLava_LevelSelection_Item> _items;
        private int _itemCenterIndex;

        private Tween _tween;

        public event Action<TheFloorIsLava_LevelConfig> eventStart;

        private void OnDestroy()
        {
            _tween?.Kill();
        }

        private void Move(float delta)
        {
            if (_items == null)
                return;

            float animEvaluate = 0f;

            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].transformCached.TranslateLocalX(delta);

                animEvaluate = _itemAnimationCurve.Evaluate(Mathf.InverseLerp(_leftX, -_leftX, _items[i].transformCached.localPosition.x));

                _items[i].transformCached.SetScale(Mathf.Lerp(_itemScaleRange.x, _itemScaleRange.y, animEvaluate));

                // Find item center index
                if (Mathf.Abs(_items[i].transformCached.localPosition.x) < Mathf.Abs(_items[_itemCenterIndex].transformCached.localPosition.x))
                    _itemCenterIndex = i;

                if (_items[i].transformCached.localPosition.x < _leftX)
                {
                    _items[i].transformCached.TranslateLocalX(_items.Count * (_itemWidth + _itemSpacing));
                    _items[i].Construct(GetLevelConfig());
                }
            }
        }

        private void ConstructItems()
        {
            _items = new List<TheFloorIsLava_LevelSelection_Item>();

            _leftX = GetComponent<RectTransform>().rect.width * -0.5f - _itemWidth * 0.5f;

            int itemCount = 0;
            float x = _leftX;

            while (x < -_leftX)
            {
                itemCount++;

                TheFloorIsLava_LevelSelection_Item item = _itemPrefab.Create(_root, false).GetComponent<TheFloorIsLava_LevelSelection_Item>();
                item.Construct(GetLevelConfig());

                _items.Add(item);

                if (itemCount == 1)
                {
                    item.transformCached.SetLocalX(x + _itemWidth * 0.5f);

                    x += _itemWidth;
                }
                else
                {
                    item.transformCached.SetLocalX(x + _itemSpacing + _itemWidth * 0.5f);

                    x += _itemWidth + _itemSpacing;
                }
            }
        }

        private TheFloorIsLava_LevelConfig GetLevelConfig()
        {
            TheFloorIsLava_LevelConfig config = _configs.GetClamp(_configIndex);

            _configIndex++;

            if (_configIndex >= _configs.Count)
            {
                _configIndex = 0;
                _configShuffleCount++;

                _configs.Shuffle();

                CheckConfigFixed();
            }

            return config;
        }

        private void CheckConfigFixed()
        {
            if ((_configShuffleCount + 1) * _configs.Count >= _shuffleCount)
            {
                int indexCenter = _shuffleCount % _configs.Count;

                for (int i = 0; i < _configFixed.Length; i++)
                {
                    int indexRight = _configs.IndexOf(_configFixed[i]);

                    if (indexCenter - 1 + i != indexRight)
                        _configs.Swap(indexRight, indexCenter - 1 + i);
                }
            }
        }

        private void PlayMoveAnimation()
        {
            float time = 0f;

            _tween?.Kill();
            _tween = DOVirtual.Float(0f, _leftX + (_itemSpacing + _itemWidth) * _shuffleCount + _itemWidth * 0.5f, _shuffleDuration, (x) =>
            {
                Move(time - x);
                time = x;
            }).SetEase(_shuffleEase);
        }

        private void Item_OnStart(TheFloorIsLava_LevelConfig config)
        {
            GetComponent<View>().Close();

            eventStart?.Invoke(config);
        }

        public void Construct(TheFloorIsLava_LevelConfig[] configs, TheFloorIsLava_LevelConfig[] configFixed = null)
        {
            _configFixed = configFixed;

            _configs = new List<TheFloorIsLava_LevelConfig>(configs);
            _configs.Shuffle();

            CheckConfigFixed();

            ConstructItems();

            PlayMoveAnimation();

            _tween.onComplete += () =>
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (i == _items.GetLoopIndex(_itemCenterIndex - 1))
                        _items[i].ConstructStart(Item_OnStart, false);
                    else if (i == _items.GetLoopIndex(_itemCenterIndex))
                        _items[i].ConstructStart(Item_OnStart, false);
                    else if (i == _items.GetLoopIndex(_itemCenterIndex + 1))
                        _items[i].ConstructStart(Item_OnStart, false);
                    else
                        _items[i].gameObjectCached.SetActive(false);
                }

                _eventComplete?.Invoke();
            };
        }
    }
}
