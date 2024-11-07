using Cinemachine;
using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Dalgona_Camera : MonoBehaviour
    {
        [SerializeField] Camera _camera;
        [SerializeField] CinemachineFreeLook _playerCam;
        [SerializeField] CinemachineVirtualCamera _gameCam;

        private void Awake()
        {
            StaticBus<Event_Dalgona_RoundStart>.Subscribe(RoundStart);
        }

        private void OnDestroy()
        {
            StaticBus<Event_Dalgona_RoundStart>.Subscribe(RoundStart);
        }

        private void Start()
        {
           
        }

        void RoundStart(Event_Dalgona_RoundStart e)
        {
            if(_camera != null)_camera.orthographic = true;

            _playerCam.Priority = 0;
        }

        public void ResetCamera()
        {
            _camera.orthographic = false;

            _playerCam.Priority = 10;

        }
    }
}
