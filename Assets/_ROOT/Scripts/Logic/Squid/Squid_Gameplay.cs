using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Squid_Gameplay : MonoBehaviour
    {
        private Squid_Master _master;

        [Title("Reference")]
        [SerializeField] private float _prepareTime;
        [SerializeField] RuntimeAnimatorController _combatController;


        private void Awake()
        {
            StaticBus<Event_Squid_Constructed>.Subscribe(Constructed);
            StaticBus<Event_Squid_ChooseWeapon>.Subscribe(InitWeapon);
        }

        private void OnDestroy()
        {
            StaticBus<Event_Squid_Constructed>.Unsubscribe(Constructed);
            StaticBus<Event_Squid_ChooseWeapon>.Unsubscribe(InitWeapon);
        }

        private void Start()
        {
            _master = GetComponent<Squid_Master>();
        }

        public void Constructed(Event_Squid_Constructed e)
        {

        }
        public void InitWeapon(Event_Squid_ChooseWeapon e)
        {
            _master.player.character.animator.SetAnimator(_combatController);
            StartCoroutine(PrepareStart());
        }

        IEnumerator PrepareStart()
        {
            float currentTime = _prepareTime;

            while (currentTime > 0)
            {
                _master.gui.announcement.PushMesseage($"Eliminate all other players.").Forget();

                currentTime -= Time.deltaTime;

                yield return null;
            }
            _master.gui.announcement.PushMesseage($"Start !!!").Forget();

            Squid_AI_Manager aim = GetComponent<Squid_AI_Manager>();

            foreach(var a in aim._ai)
            {
                a.gameObject.AddComponent<AIFollowWaypoint>();
            }
        }
    }
}
