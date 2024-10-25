using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class BladeBall_Player : MonoBehaviour
    {
        private bool gameStart = false;
        [SerializeField] private float clickTime = 0f;
        [SerializeField] private float skillTime = 0f;
        [SerializeField] RuntimeAnimatorController _bladeBallAc;
        [SerializeField] Player player;
        [SerializeField] private float blockCooldown = 0.25f;
        [SerializeField] private float skillCooldown = 7f;

        public float atkRange = 2.5f;
        [SerializeField] private float atkAngle = 75f;

        [SerializeField] bool ballInRange = false;
        private CharacterAnimator _charAnim;
        private Animator _anim;
        private BladeBall_Ball _ball;
        [SerializeField] private BladeBall_SwordManager _swordManager;

        public Vector3 velocity;

        private Vector3 lastPos;
        [Title("SFX")]
        [SerializeField] AudioConfig _slash;
        [SerializeField] AudioConfig _atkSfx;
        [SerializeField] GameObject _vfxSlash;
        #region Unity
        private void Awake()
        {
            _charAnim = GetComponent<CharacterAnimator>();
            _ball = FindFirstObjectByType<BladeBall_Ball>();

            StaticBus<Event_BladeBall_Revive>.Subscribe(CharacterRevive);
            StaticBus<Event_BladeBall_LevelStart>.Subscribe(Init);
            StaticBus<Event_BladeBall_SelectWeapon>.Subscribe(SelectWeapon);
        }

        private void OnDestroy()
        {
            StaticBus<Event_BladeBall_Revive>.Unsubscribe(CharacterRevive);
            StaticBus<Event_BladeBall_LevelStart>.Unsubscribe(Init);
            StaticBus<Event_BladeBall_SelectWeapon>.Unsubscribe(SelectWeapon);
        }
        private void Start()
        {
            UIPointerClick block = player.gui.objBlock.GetComponent<UIPointerClick>();
            block.eventDown += Block;

            UIPointerClick skill = player.gui.objSkill.GetComponent<UIPointerClick>();
            skill.eventDown += Skill;
        }

        public void Init(Event_BladeBall_LevelStart e)
        {
            gameStart = true;
            _charAnim.SetAnimator(_bladeBallAc);
            _charAnim.TurnOnBladeBallLayer();
            _swordManager = GetComponentInChildren<BladeBall_SwordManager>();
        }
        private void Update()
        {
            CharacterInput();
        }
        #endregion

        void SelectWeapon(Event_BladeBall_SelectWeapon e)
        {
            _swordManager = GetComponentInChildren<BladeBall_SwordManager>();
            _swordManager.currentSwordId = e.id;
            _swordManager.PlayerSword();
        }

        private void CharacterInput()
        {
            ballInRange = IsBallInCone();

            if (!player.control.useMobileControl)
            {
                if (clickTime > 0)
                {
                    clickTime -= Time.unscaledDeltaTime;
                }
                if (clickTime <= 0 && gameStart)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        clickTime = blockCooldown;

                        _charAnim.PlayBlock();
                        AudioManager.Play(_slash, false);
                        _vfxSlash = _swordManager.SetSlashById();
                        GameObject slash = Instantiate(_vfxSlash, transform);
                        slash.transform.localPosition = Vector3.up;
                        
                        if (IsBallInCone())
                        {
                            AudioManager.Play(_atkSfx, false);
                            if (_ball.targetPlayer)
                            {
                                StaticBus<Event_BladeBall_Target>.Post(new Event_BladeBall_Target(_ball.preTarget, _swordManager.SetExplosionById()));
                                _ball.targetPlayer = false;
                            }
                            else
                            {
                                StaticBus<Event_BladeBall_TargetRandom>.Post(new Event_BladeBall_TargetRandom(this.transform, _swordManager.SetExplosionById()));
                            }
                        }
                    }
                }

                if (skillTime > 0)
                {
                    skillTime -= Time.unscaledDeltaTime;
                }
                if (skillTime <= 0 && gameStart)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        skillTime = skillCooldown;

                        _charAnim.PlayBlock();
                        _vfxSlash = _swordManager.SetSlashById();
                        GameObject slash = Instantiate(_vfxSlash, transform);
                        slash.transform.localPosition = Vector3.up;
                        AudioManager.Play(_slash, false);
                        StartCoroutine(MoveForwardOverTime(4f, 0.35f));
                        StaticBus<Event_BladeBall_Skill>.Post(new Event_BladeBall_Skill(skillCooldown));
                        if (Vector3.Distance(transform.position + transform.forward * 4, _ball.transform.position) <= atkRange)
                        {
                            AudioManager.Play(_atkSfx, false);
                            StaticBus<Event_BladeBall_SkillTarget>.Post(new Event_BladeBall_SkillTarget(_swordManager.SetExplosionById()));
                        }
                    }
                }
            }
            else
            {
                if (clickTime > 0)
                {
                    clickTime -= Time.unscaledDeltaTime;
                }

                if (skillTime > 0)
                {
                    skillTime -= Time.unscaledDeltaTime;
                }

            }
        }

        public void Block()
        {
            if(clickTime <= 0 && gameStart)
            {
                clickTime = blockCooldown;
                _charAnim.PlayBlock();
                _vfxSlash = _swordManager.SetSlashById();
                AudioManager.Play(_slash, false);
                GameObject slash = Instantiate(_vfxSlash, transform);
                slash.transform.localPosition = Vector3.up;

                if (IsBallInCone())
                {
                    AudioManager.Play(_atkSfx, false);
                    if (_ball.targetPlayer)
                    {
                        StaticBus<Event_BladeBall_Target>.Post(new Event_BladeBall_Target(_ball.preTarget, _swordManager.SetExplosionById()));
                        _ball.targetPlayer = false;
                    }
                    else
                    {
                        StaticBus<Event_BladeBall_TargetRandom>.Post(new Event_BladeBall_TargetRandom(this.transform, _swordManager.SetExplosionById()));
                    }
                }
            }
        }
        private void Skill()
        {
            if(skillTime <= 0 && gameStart)
            {
                skillTime = skillCooldown;
                _charAnim.PlayBlock();
                _vfxSlash = _swordManager.SetSlashById();

                GameObject slash = Instantiate(_vfxSlash, transform);
                slash.transform.localPosition = Vector3.up;

                AudioManager.Play(_slash, false);
                StartCoroutine(MoveForwardOverTime(4f, 0.35f));
                StaticBus<Event_BladeBall_Skill>.Post(new Event_BladeBall_Skill(skillCooldown));
                if (Vector3.Distance(transform.position + transform.forward * 4, _ball.transform.position) <= atkRange)
                {
                    AudioManager.Play(_atkSfx, false);
                    StaticBus<Event_BladeBall_SkillTarget>.Post(new Event_BladeBall_SkillTarget(_swordManager.SetExplosionById()));
                }
            }

        }
        IEnumerator MoveForwardOverTime(float distance, float duration)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + transform.forward * distance;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                player.character.motor.SetPosition(Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration));
                elapsedTime += Time.fixedDeltaTime;
                yield return null;
            }

            player.character.motor.SetPosition(targetPosition);

        }

        private bool IsBallInCone()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position + Vector3.up, atkRange);

            foreach (var hit in hits)
            {
                if (hit.transform == _ball.transform && _ball._target == this.transform)
                {
                    Vector3 directionToBall = (hit.transform.position - transform.position).normalized;

                    float angleToBall = Vector3.Angle(transform.forward, directionToBall);
                    if (angleToBall < atkAngle / 2 && Vector3.Distance(transform.position + Vector3.up, hit.transform.position) <= atkRange)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == _ball.transform && _ball._target == this.transform)
            {
                player.character.Kill();
                _ball.Respawn();
                lastPos = this.transform.position;
            }
        }

        private void CharacterRevive(Event_BladeBall_Revive e)
        {
            player.character.Revive(lastPos, Quaternion.LookRotation(-this.transform.position.normalized, Vector3.up));
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + Vector3.up, atkRange);
        }
    }

}
