using Cysharp.Threading.Tasks;
using LFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

namespace Game
{
    public class LightOff_AI : MonoBehaviour
    {
        private AI _ai;

        [SerializeField] LightOff_AIType _aiType;
        [SerializeField] private bool _isStart;

        [SerializeField] private GameObject _aim;

        int hp = 5;
        [SerializeField] int dmg = 1;
        [SerializeField] private int curHp;
        [SerializeField] UIHealthbar healthbar;
        [SerializeField] LightOff_Player _player;

        [SerializeField] private FieldOfView fov;
        [SerializeField] private LightOff_HandWeapon weapon;
        public int weaponId = -1;
        private Weapon curType;
        private void Awake()
        {
            _ai = transform.parent.GetComponent<AI>();
            
            healthbar = transform.GetComponentInChildren<UIHealthbar>();
            healthbar.gameObject.SetActive(false);
            healthbar.InitHealthBar(hp);
            curHp = hp;

            _player = FindObjectOfType<LightOff_Player>();
        
        }

        private void OnDestroy()
        {

        }

        [Button]
        public void TakeWeapon(RuntimeAnimatorController _animator, int id, Weapon type)
        {
            SetUpVision(_animator,id,type);

            ChangeBehavior();
        }

        public void SetUpVision(RuntimeAnimatorController _animator, int id, Weapon type)
        {
            if (weapon == null)
            {
                weapon = transform.GetComponentInChildren<LightOff_HandWeapon>();
            }
            GetComponent<Character>().animator.SetAnimator(_animator);
            weaponId = id;

            weapon.SetWeapon(id);

            curType = type;

            fov =  gameObject.AddComponent<FieldOfView>();

            fov.playerRef = this.gameObject;
            fov.targetMask = (1 << 3);

            switch (curType)
            {
                case Weapon.ConeRange:
                    fov.radius = 1.5f;
                    fov.angle = 120f;
                    break;
                case Weapon.CircleRange:
                    fov.radius = 1.5f;
                    fov.angle = 360;
                    break;
                case Weapon.ForwardRange:
                    fov.radius = 3;
                    fov.angle = 120f;
                    break;
            }
        }

        void ChangeBehavior()
        {
            switch( _aiType)
            {
                case LightOff_AIType.Normal:
                    break;

                case LightOff_AIType.Random:
                    break;
                case LightOff_AIType.Target:
                    _ai.gameObject.RemoveComponent<AIFollowWaypoint>();
                    _ai.StopAllCoroutines();
                    _ai.Chase(_player.transform);
                    break;
            }
        }

        public void DealDamage()
        {
            Debug.Log(gameObject.name + " slash");

            foreach (var ai in fov.visibleTargets)
            {
                if (ai.GetComponent<LightOff_AI>()) ai.GetComponent<LightOff_AI>().TakeDamage(1);
                else if (ai.GetComponent<LightOff_Player>()) ai.GetComponent<LightOff_Player>().TakeDamage(1);
            }
        }
        public void TakeDamage(int damage)
        {
            curHp -= damage;

            if (!healthbar.gameObject.activeSelf)
            {
                healthbar.gameObject.SetActive(true);
            }
            healthbar.UpdateHealthBar(curHp);

            if (curHp <= 0)
            {     
                _ai.character.Kill();
                _ai.character.gameObject.layer = 6;

                healthbar.gameObject.SetActive(false);
            }
        }
        public void Construct(int i)
        {
            _aim = transform.GetChild(0).gameObject;

            if(i ==2 || i == 4 || i == 5 || i == 6)
            {
                _aiType = LightOff_AIType.Target;
            }
        }
    }
}
