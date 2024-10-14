using Cinemachine;
using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class CameraManager : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineFreeLook _cineFreeLook;
        [SerializeField] private CameraTutorial _cameraTutorial;

        private Transform _cameraTransform;

        public Transform cameraTransform
        {
            get
            {
                if (_cameraTransform == null)
                    _cameraTransform = _camera.transform;

                return _cameraTransform;
            }
        }

        public CameraTutorial cameraTutorial { get { return _cameraTutorial; } }

        private void Awake()
        {
            StaticBus<Event_Player_Revive>.Subscribe(StaticBus_Player_Revive);
        }

        private void OnDestroy()
        {
            StaticBus<Event_Player_Revive>.Unsubscribe(StaticBus_Player_Revive);
        }

        private void StaticBus_Player_Revive(Event_Player_Revive e)
        {
            if (e.character == null)
                return;

            _cineFreeLook.ForceCameraPosition(e.character.transformCached.TransformPoint(new Vector3(0f, 7f, -10f)), e.character.transformCached.rotation);
        }

        public void UpdateInput(Vector3 input)
        {
            _cineFreeLook.m_XAxis.m_InputAxisValue = input.x;
            _cineFreeLook.m_YAxis.m_InputAxisValue = input.y;
        }
    }
}
