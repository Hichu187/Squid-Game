using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class CameraTutorial : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _objDeactiveWhenPlay;

        [Title("Config")]
        [SerializeField] private float _inDuration = 2.0f;
        [SerializeField] private Ease _inEase = Ease.OutSine;

        [SerializeField] private float _stopDuration = 1.0f;

        [SerializeField] private float _outDuration = 1.0f;
        [SerializeField] private Ease _outEase = Ease.OutSine;

        private Sequence _sequence;

        private void OnDestroy()
        {
            _sequence?.Kill();
        }

        public Sequence Play(Transform target)
        {
            return Play(target.position, target.eulerAngles);
        }

        public Sequence Play(Vector3 position, Vector3 rotation)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            Transform camTransform = _camera.transform;

            Vector3 startPos = camTransform.position;
            Vector3 startRot = camTransform.eulerAngles;

            Vector3 endPos = position;
            Vector3 endRot = rotation;

            _sequence.AppendCallback(() => { _objDeactiveWhenPlay.SetActive(false); });

            _sequence.Append(camTransform.DOMove(endPos, _inDuration)
                                         .ChangeStartValue(startPos)
                                         .SetEase(_inEase));

            _sequence.Join(camTransform.DORotate(endRot, _inDuration)
                                       .ChangeStartValue(startRot)
                                       .SetEase(_inEase));

            _sequence.AppendInterval(_stopDuration);

            _sequence.Append(camTransform.DOMove(startPos, _outDuration)
                                         .ChangeStartValue(endPos)
                                         .SetEase(_outEase));

            _sequence.Join(camTransform.DORotate(startRot, _outDuration)
                                       .ChangeStartValue(endRot)
                                       .SetEase(_outEase));

            _sequence.AppendCallback(() => { _objDeactiveWhenPlay.SetActive(true); });

            return _sequence;
        }
    }
}
