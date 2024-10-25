using Cysharp.Threading.Tasks.Triggers;
using LFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BladeBall_Ball : MonoBehaviour
    {
        public bool canMove = false;
        bool gameStart = false;
        [Header("REFERENCES")]
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private GameObject _explosionPrefab;
        public List<Transform> _listTargets;
        [SerializeField] private Transform _player;
        public Transform _target;
        public bool targetPlayer = false;
        public Transform preTarget;
        public bool critHit = false;

        private int hitCount = 0;
        [Header("MOVEMENT")]
        [SerializeField] private float _baseSpeed = 15;
        private float _speed = 15;
        [SerializeField] private float _baseRotateSpeed = 95;
        private float _rotateSpeed = 95;
        [SerializeField] private float _speedScaleUp = 1.25f;
        [Header("PREDICTION")]
        [SerializeField] private float _maxDistancePredict = 100;
        [SerializeField] private float _minDistancePredict = 5;
        [SerializeField] private float _maxTimePrediction = 5;
        private Vector3 _standardPrediction, _deviatedPrediction;

        [Header("DEVIATION")]
        [SerializeField] private float _deviationAmount = 50;
        [SerializeField] private float _deviationSpeed = 2;
        [Header("Color")]
        [SerializeField] Material _defaultMat;
        [SerializeField] Material _targetMat;
        
        private void Awake()
        {
            StaticBus<Event_BladeBall_LevelStart>.Subscribe(Init);
            StaticBus<Event_BladeBall_SkillTarget>.Subscribe(TargetSkill);
            StaticBus<Event_BladeBall_Target>.Subscribe(Target);
            StaticBus<Event_BladeBall_TargetPlayer>.Subscribe(TargetPlayer);
            StaticBus<Event_BladeBall_TargetRandom>.Subscribe(TargetRandom);
            StaticBus<Event_Player_Die>.Subscribe(Player_Die);
            StaticBus<Event_BladeBall_AiDie>.Subscribe(AI_Character_Die);
            StaticBus<Event_Player_Revive>.Subscribe(Player_Revive);
        }
        private void OnDestroy()
        {
            StaticBus<Event_BladeBall_LevelStart>.Unsubscribe(Init);
            StaticBus<Event_BladeBall_SkillTarget>.Unsubscribe(TargetSkill);
            StaticBus<Event_BladeBall_Target>.Unsubscribe(Target);
            StaticBus<Event_BladeBall_TargetPlayer>.Unsubscribe(TargetPlayer);
            StaticBus<Event_BladeBall_TargetRandom>.Unsubscribe(TargetRandom);
            StaticBus<Event_Player_Die>.Unsubscribe(Player_Die);
            StaticBus<Event_BladeBall_AiDie>.Unsubscribe(AI_Character_Die);
            StaticBus<Event_Player_Revive>.Unsubscribe(Player_Revive);
        }

        private void Init(Event_BladeBall_LevelStart start)
        {
            transform.position = Vector3.up;
            canMove = true;
            _speed = _baseSpeed;
            _rotateSpeed = _baseRotateSpeed;
            _rb.isKinematic = false;
            critHit = false;
            targetPlayer = false;
            gameStart = true;
            if (!DataBladeBall.tutorialCompleted)
            {
                hitCount = 0;
                _target = _player;
                Block(_speedScaleUp - 0.1f);
                SetTargetMat();
            }
            else
            {
                RandomTarget();
            }

        }

        private void AI_Character_Die(Event_BladeBall_AiDie aDie)
        {
            _listTargets.Remove(aDie.character.transform);
            GameObject exlposion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Respawn();   
        }

        private void Player_Die(Event_Player_Die die)
        {
            GameObject exlposion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Respawn();
        }
        private void Player_Revive(Event_Player_Revive revive)
        {
            canMove = true;
            _rb.isKinematic = false;
            _target = null;
            if (gameStart) { Invoke("RandomTarget", 3f); }

        }

        public void Respawn()
        {
            transform.position = Vector3.up;
            canMove = false;
            _speed = _baseSpeed;
            _rotateSpeed = _baseRotateSpeed;
            _rb.isKinematic = true;
            critHit = false;
            //
            targetPlayer = false;
            SetDefautColor();

            Invoke("RandomTarget", 3f);
        }
        private void Target(Event_BladeBall_Target e)
        {
            _target = e.transform;
            _explosionPrefab = e.vfx;
            preTarget = _player;
            Block(1.15f);
            SetDefautColor();
        }
        private void TargetPlayer(Event_BladeBall_TargetPlayer e)
        {
            hitCount = 0;
            preTarget = e.transform;
            targetPlayer = true;
            _target = _player;
            _explosionPrefab = e.vfx;
            Block(_speedScaleUp - 0.1f);
            SetTargetMat();
        }
        private void TargetRandom(Event_BladeBall_TargetRandom e)
        {
            _explosionPrefab = e.explosion;
            preTarget = e.transform;
            hitCount++;
            if(hitCount < 5)
            {
                List<Transform> targets = new List<Transform>();
                targets.AddRange(_listTargets);
                targets.Remove(e.transform);
                if (e.transform.GetComponent<Character>().isPlayer)
                {
                }
                else
                {
                    targets.Add(_player);
                }
                _target = LFramework.ExtensionsList.GetRandom(targets);
                if (_target.gameObject.GetComponent<Character>().isPlayer)
                {
                    SetTargetMat(); 
                    hitCount = 0;
                }
                else
                {
                    SetDefautColor();
                }
            }
            else
            {
                hitCount = 0;
                _target = _player.transform;
                SetTargetMat();
            }


            Block(_speedScaleUp);
        }
        private void TargetSkill(Event_BladeBall_SkillTarget e)
        {
            preTarget = _player;
            List<Transform> targets = new List<Transform>();
            targets.AddRange(_listTargets);

            _target = LFramework.ExtensionsList.GetRandom(targets);
            _explosionPrefab = e.vfx;
            SetDefautColor();
            Block(_speedScaleUp * 2);

            critHit = true;
        }
        private void RandomTarget()
        {
            preTarget = null;
            List<Transform> targets = new List<Transform>();
            targets.AddRange(_listTargets);
            targets.Add(_player);

            _target = LFramework.ExtensionsList.GetRandom(targets);
            if (_target.gameObject.GetComponent<Character>().isPlayer)
            {
                SetTargetMat();
            }
            else
            {
                SetDefautColor();
            }
            Block(_speedScaleUp);
        }
        private void Block(float scale)
        {
            _speed = Mathf.Min(_speed * scale, 40);
            _rotateSpeed = Mathf.Min(_rotateSpeed * (scale + 0.25f), 15000);
        }

        #region Moving
        private void PredictMovement(float leadTimePercentage)
        {
            var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);
            _standardPrediction = _target.transform.position + Vector3.up + _target.GetComponent<Rigidbody>().velocity * predictionTime;
        }
        private void AddDeviation(float leadTimePercentage)
        {
            var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);
            var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;
            _deviatedPrediction = _standardPrediction + predictionOffset;
        }


        private void FixedUpdate()
        {
            if(_target != null && canMove)
            {
                _rb.velocity = transform.forward * _speed;

                var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, _target.position + Vector3.up));

                PredictMovement(leadTimePercentage);

                AddDeviation(leadTimePercentage);

                RotateRocket();
            }
        }

        private void RotateRocket()
        {
            var heading = _deviatedPrediction - transform.position;

            var rotation = Quaternion.LookRotation(heading);
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
/*            if (other.GetComponent<BladeBall_Player>() && _target == other.transform)
            {
                Respawn();
                _player.GetComponent<Character>().Kill();
            }*/
        }

        public void SetDefautColor()
        {
            GetComponent<MeshRenderer>().material = _defaultMat;
        }

        public void SetTargetMat()
        {
            GetComponent<MeshRenderer>().material = _targetMat;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _standardPrediction);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_standardPrediction, _deviatedPrediction);
        }
        #endregion
    }
}
