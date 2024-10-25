using LFramework;
using UnityEngine;

namespace Game
{
    public class BladeBall_AI_Character : MonoBehaviour
    {
        public AIType aiType;
        public RuntimeAnimatorController _animator;
        private BladeBall_Ball _ball;

        [SerializeField] private int hitBase;
        [SerializeField] private int hit;
        private Character _character;
        [Header("SFX")]
        [SerializeField] BladeBall_SwordManager _swordManager;
        public AudioConfig _slash;
        public AudioConfig _atkSfx;
        public GameObject _vfxSlash;

        private void Awake()
        {
            StaticBus<Event_BladeBall_LevelStart>.Subscribe(StaticBus_BladeBall_LevelStart);
            StaticBus<Event_BladeBall_AiDie>.Subscribe(Respawn);

            _ball = FindObjectOfType<BladeBall_Ball>();

        }
        private void Start()
        {
            _swordManager = GetComponentInChildren<BladeBall_SwordManager>();
            _character = GetComponent<Character>();
        }

        private void OnDestroy()
        {
            StaticBus<Event_BladeBall_LevelStart>.Unsubscribe(StaticBus_BladeBall_LevelStart);
            StaticBus<Event_BladeBall_AiDie>.Unsubscribe(Respawn);
        }

        private void StaticBus_BladeBall_LevelStart(Event_BladeBall_LevelStart e)
        {
            transform.parent.GetComponent<AIFollowWaypoint>().enabled = true;
            _swordManager = GetComponentInChildren<BladeBall_SwordManager>();
            int i = Random.Range(0, _swordManager.swordModels.Count); ;
            _swordManager.SelectSword();
            _vfxSlash = _swordManager.SetSlashById();
            _character.animator.SetAnimator(_animator);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BladeBall_Ball") && _ball != null && _ball._target == this.transform)
            {
                if (hit == 0 || _ball.critHit == true)
                {
                    Die();
                }
                else
                {
                    PostEventByType();
                }

            }
        }

        public void SetHit()
        {
            switch (aiType)
            {
                case AIType.Normal_1:
                    hitBase = Random.Range(1, 2);
                    break;
                case AIType.Normal_2:
                    hitBase = Random.Range(2, 5);
                    break;
                case AIType.Normal_3:
                    hitBase = Random.Range(3, 6);
                    break;
                case AIType.Target_1:
                    hitBase = Random.Range(4, 7);
                    break;
                case AIType.Target_2:
                    hitBase = Random.Range(5, 8);
                    break;

            }
            hit = hitBase;
        }

        private void PostEventByType()
        {
            _character.animator.PlayBlock();

            AudioManager.Play(_atkSfx, false);
            GameObject slash = Instantiate(_vfxSlash, transform);
            slash.transform.localPosition = Vector3.up;

            switch (aiType)
            {
                case AIType.Normal_1:
                case AIType.Normal_2:
                case AIType.Normal_3:
                    StaticBus<Event_BladeBall_TargetRandom>.Post(new Event_BladeBall_TargetRandom(this.transform,_swordManager.SetExplosionById()));
                    break;
                case AIType.Target_1:
                case AIType.Target_2:
                    StaticBus<Event_BladeBall_TargetPlayer>.Post(new Event_BladeBall_TargetPlayer(this.transform, _swordManager.SetExplosionById()));
                    break;
            }
            hit--;
        }

        private void Die()
        {
            Character character = GetComponent<Character>();
            character.Kill();
            StaticBus<Event_BladeBall_AiDie>.Post(new Event_BladeBall_AiDie(character));
        }

        private void Respawn(Event_BladeBall_AiDie e)
        {
            if(e.character.transform != this.transform)
            {
                hit = hitBase;
            }
        }
    }

    public enum AIType { Normal_1, Normal_2, Normal_3, Target_1, Target_2 }
}
