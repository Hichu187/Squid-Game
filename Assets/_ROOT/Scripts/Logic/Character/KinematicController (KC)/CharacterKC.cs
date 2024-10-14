using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using LFramework;
using System.Linq;

namespace Game
{
    public class CharacterKC : MonoCached, ICharacterController
    {
        [System.Serializable]
        public enum OrientationBonusMethod
        {
            None,
            TowardsGravity,
            TowardsGroundSlopeAndGravity,
        }

        [System.Serializable]
        public enum OrientationMethod
        {
            TowardsCamera,
            TowardsMovement,
        }

        public enum State
        {
            Default,
            Climbing,
        }

        public enum StateClimb
        {
            Enter,
            Middle,
            Exit,
        }

        [Title("Reference")]
        [SerializeField] private KinematicCharacterMotor _motor;

        [Title("Stable Movement")]
        [SerializeField] private float _stableMoveSpeedMax = 4.5f;
        [SerializeField] private float _stableMoveSharpness = 1000f;

        [Title("Orientation")]
        [SerializeField] private OrientationMethod _orientationMethod = OrientationMethod.TowardsMovement;
        [SerializeField] private OrientationBonusMethod _orientationBonusMethod = OrientationBonusMethod.TowardsGravity;
        [SerializeField] private float _orientationSharpness = 10f;
        [SerializeField] private float _orientationBonusSharpness = 10f;

        [Title("Air Movement")]
        [SerializeField] private float _airMoveSpeedMax = 4.5f;
        [SerializeField] private float _airAccelerationSpeed = 1000f;
        [SerializeField] private float _airDrag = 0.1f;

        [Title("Jumping")]
        [SerializeField] private bool _jumpAllowedWhenSliding = false;
        [SerializeField] private float _jumpUpSpeed = 11f;
        [SerializeField] private float _jumpScalableForwardSpeed = 0f;
        [SerializeField] private float _jumpPreGroundingGraceTime = 0.5f;
        [SerializeField] private float _jumpPostGroundingGraceTime = 0.5f;

        [Title("Ladder")]
        [SerializeField] private float _ladderClimbSpeed = 5f;
        [SerializeField] private float _ladderClimbPadding = 0.2f;
        [SerializeField] private float _ladderEnterSpeed = 15f;
        [SerializeField] private float _ladderExitSpeed = 15f;

        [Title("Jetpack")]
        [SerializeField] private float _jetpackPower = 30f;
        [SerializeField] private float _jetpackDuration = 3f;
        [SerializeField] private float _jetpackRefuelDuration = 1f;
        [SerializeField] private float _jetpackJumpMagnitude = 0.3f;

        [Title("Misc")]
        [SerializeField] private Collider[] _ignoredColliders;
        [SerializeField] private Vector3 _gravity = new Vector3(0, -30f, 0);
        [SerializeField] private Transform _meshRoot;

        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;

        private bool _jumpRequested = false;
        private bool _jumpConsumed = false;
        private bool _jumpedThisFrame = false;
        private float _jumpTimeSinceRequested = Mathf.Infinity;
        private float _jumpUpSpeedMultiple = 1f;

        private float _timeSinceLastAbleToJump = 0f;
        private Vector3 _internalVelocityAdd = Vector3.zero;

        private CharacterSfx _sfx;
        private CharacterAnimator _animator;

        private Ladder _ladder;
        private Vector3 _ladderClimbDirection;
        private Vector3 _ladderClimbLocalPoint;

        private float _jetPackFuel;
        private bool _jetPackRefueling = false;
        private bool _jetPackEnable = false;

        private StateClimb _stateClimb;

        private StateMachine<State> _stateMachine;

        public KinematicCharacterMotor motor { get { return _motor; } }

        public State stateCurrent { get { return _stateMachine.CurrentState; } }

        public Vector3 gravity { get { return _gravity; } set { _gravity = value; } }

        public float jetPackFuel { get { return _jetPackFuel; } }

        public float jumpUpSpeedMultiple { get { return _jumpUpSpeedMultiple; } set { _jumpUpSpeedMultiple = value; } }

        private void Awake()
        {
            _animator = GetComponent<CharacterAnimator>();
            _sfx = GetComponent<CharacterSfx>();

            // Init state machine
            InitStateMachine();

            // Assign the characterController to the motor
            _motor.CharacterController = this;
        }

        private void Update()
        {
            if (_motor.GroundingStatus.IsStableOnGround)
            {
                Vector3 local = transformCached.InverseTransformPoint(transformCached.position + motor.BaseVelocity);
                _animator.SetVelocityZ(Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-_stableMoveSpeedMax, _stableMoveSpeedMax, local.z)));
            }
        }

        private void InitStateMachine()
        {
            _stateMachine = new StateMachine<State>();

            _stateMachine.AddState(State.Default);
            _stateMachine.AddState(State.Climbing);

            _stateMachine.OnStateChanged += StateMachine_OnStateChanged;

            // Set default state
            _stateMachine.CurrentState = State.Default;
        }

        private void StateMachine_OnStateChanged()
        {
            switch (_stateMachine.CurrentState)
            {
                case State.Default:
                    _animator.SetClimbing(false);

                    _motor.SetMovementCollisionsSolvingActivation(true);
                    _motor.SetGroundSolvingActivation(true);
                    break;
                case State.Climbing:
                    _animator.SetClimbing(true);

                    _motor.SetMovementCollisionsSolvingActivation(false);
                    _motor.SetGroundSolvingActivation(false);
                    break;
            }
        }

        /// <summary>
        /// This is called every frame by Player in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref CharacterKCInputPlayer inputs)
        {
            // Clamp input
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.moveAxisRight, 0f, inputs.moveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.cameraRotation * Vector3.forward, _motor.CharacterUp).normalized;

            if (cameraPlanarDirection.sqrMagnitude == 0f)
                cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.cameraRotation * Vector3.up, _motor.CharacterUp).normalized;

            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, _motor.CharacterUp);

            switch (_stateMachine.CurrentState)
            {
                case State.Climbing:
                case State.Default:
                    {
                        // Move and look inputs
                        _moveInputVector = cameraPlanarRotation * moveInputVector;

                        _jetPackEnable = inputs.jetpackDown;

                        switch (_orientationMethod)
                        {
                            case OrientationMethod.TowardsCamera:
                                _lookInputVector = cameraPlanarDirection;
                                break;
                            case OrientationMethod.TowardsMovement:
                                _lookInputVector = _moveInputVector.normalized;
                                break;
                        }

                        // Jumping input
                        if (inputs.jumpDown)
                        {
                            _jumpTimeSinceRequested = 0f;
                            _jumpRequested = true;
                        }

                        break;
                    }
            }
        }

        /// <summary>
        /// This is called every frame by the AI script in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref CharacterKCInputAI inputs)
        {
            _moveInputVector = inputs.moveVector;
            _lookInputVector = inputs.lookVector;

            if (inputs.jump)
            {
                _jumpTimeSinceRequested = 0f;
                _jumpRequested = true;
            }
            else
            {
                _jumpRequested = false;
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime)
        {
            switch (_stateMachine.CurrentState)
            {
                case State.Default:
                    RaycastHit hitInfo;

                    if (Physics.Raycast(transformCached.position, transformCached.forward, out hitInfo, _motor.Capsule.radius + 0.1f, FactoryConfig.layerMaskLadder))
                    {
                        _ladder = hitInfo.collider.GetComponent<Ladder>();

                        if (_ladder == null)
                            break;

                        _ladderClimbDirection = -hitInfo.normal;
                        _ladderClimbDirection.y = 0f;

                        _ladderClimbLocalPoint = _ladder.transformCached.InverseTransformPoint(hitInfo.point + hitInfo.normal * _ladderClimbPadding);

                        // If input is heading toward ladder
                        if (_moveInputVector.magnitude > Mathf.Epsilon && Vector3.Angle(_moveInputVector, _ladderClimbDirection) < 90.0f)
                        {
                            _stateMachine.CurrentState = State.Climbing;

                            _stateClimb = StateClimb.Enter;
                        }
                    }
                    break;

                case State.Climbing:

                    Vector3 relativePos = _ladder.transformCached.InverseTransformPoint(_motor.TransientPosition);

                    switch (_stateClimb)
                    {
                        case StateClimb.Middle:
                            _animator.SetClimbY(relativePos.y);

                            if (relativePos.y <= 0f)
                            {
                                _stateMachine.CurrentState = State.Default;
                            }
                            else if (relativePos.y > _ladder.height)
                            {
                                _stateClimb = StateClimb.Exit;

                                _animator.SetJumping(false);
                                _animator.SetClimbing(false);
                            }
                            break;
                    }

                    break;
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            switch (_stateMachine.CurrentState)
            {
                case State.Default:
                    {
                        if (_lookInputVector.sqrMagnitude > 0f && _orientationSharpness > 0f)
                        {
                            // Smoothly interpolate from current to target look direction
                            Vector3 smoothedLookInputDirection = Vector3.Slerp(_motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-_orientationSharpness * deltaTime)).normalized;

                            // Set the current rotation (which will be used by the KinematicCharacterMotor)
                            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _motor.CharacterUp);
                        }

                        Vector3 currentUp = (currentRotation * Vector3.up);
                        if (_orientationBonusMethod == OrientationBonusMethod.TowardsGravity)
                        {
                            // Rotate from current up to invert gravity
                            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -_gravity.normalized, 1 - Mathf.Exp(-_orientationBonusSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                        else if (_orientationBonusMethod == OrientationBonusMethod.TowardsGroundSlopeAndGravity)
                        {
                            if (_motor.GroundingStatus.IsStableOnGround)
                            {
                                Vector3 initialCharacterBottomHemiCenter = _motor.TransientPosition + (currentUp * _motor.Capsule.radius);

                                Vector3 smoothedGroundNormal = Vector3.Slerp(_motor.CharacterUp, _motor.GroundingStatus.GroundNormal, 1 - Mathf.Exp(-_orientationBonusSharpness * deltaTime));
                                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGroundNormal) * currentRotation;

                                // Move the position to create a rotation around the bottom hemi center instead of around the pivot
                                _motor.SetTransientPosition(initialCharacterBottomHemiCenter + (currentRotation * Vector3.down * _motor.Capsule.radius));
                            }
                            else
                            {
                                Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -_gravity.normalized, 1 - Mathf.Exp(-_orientationBonusSharpness * deltaTime));
                                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                            }
                        }
                        else
                        {
                            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-_orientationBonusSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                        break;
                    }
                case State.Climbing:
                    Vector3 dir = Vector3.RotateTowards(transformCached.forward, _ladderClimbDirection, 10f * deltaTime, 0.0f);

                    dir.y = 0f;

                    currentRotation = Quaternion.LookRotation(dir);
                    break;
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            switch (_stateMachine.CurrentState)
            {
                case State.Default:
                    {
                        // Ground movement
                        if (_motor.GroundingStatus.IsStableOnGround)
                        {
                            float currentVelocityMagnitude = currentVelocity.magnitude;

                            Vector3 effectiveGroundNormal = _motor.GroundingStatus.GroundNormal;

                            // Reorient velocity on slope
                            currentVelocity = _motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                            // Calculate target velocity
                            Vector3 inputRight = Vector3.Cross(_moveInputVector, _motor.CharacterUp);
                            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                            Vector3 targetMovementVelocity = reorientedInput * _stableMoveSpeedMax;

                            // Smooth movement Velocity
                            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-_stableMoveSharpness * deltaTime));
                        }
                        // Air movement
                        else
                        {
                            // Add move input
                            if (_moveInputVector.sqrMagnitude > 0f)
                            {
                                Vector3 addedVelocity = _moveInputVector * _airAccelerationSpeed * deltaTime;

                                Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, _motor.CharacterUp);

                                // clamp addedVel to make total vel not exceed max vel on inputs plane
                                Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, _airMoveSpeedMax);
                                addedVelocity = newTotal - currentVelocityOnInputsPlane;

                                /*
                                // Limit air velocity from inputs
                                if (currentVelocityOnInputsPlane.magnitude < MaxAirMoveSpeed)
                                {
                                    // clamp addedVel to make total vel not exceed max vel on inputs plane
                                    Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, MaxAirMoveSpeed);
                                    addedVelocity = newTotal - currentVelocityOnInputsPlane;
                                }
                                else
                                {
                                    // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                                    if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                                    {
                                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                                    }
                                }
                                */

                                // Prevent air-climbing sloped walls
                                if (_motor.GroundingStatus.FoundAnyGround)
                                {
                                    if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                                    {
                                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(_motor.CharacterUp, _motor.GroundingStatus.GroundNormal), _motor.CharacterUp).normalized;
                                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                                    }
                                }

                                // Apply added velocity
                                currentVelocity += addedVelocity;
                            }

                            // Gravity
                            currentVelocity += _gravity * deltaTime;

                            // Drag
                            currentVelocity *= (1f / (1f + (_airDrag * deltaTime)));
                        }

                        _jumpedThisFrame = false;
                        _jumpTimeSinceRequested += deltaTime;
                        if (_jumpRequested)
                        {
                            // See if we actually are allowed to jump
                            if (!_jumpConsumed && ((_jumpAllowedWhenSliding ? _motor.GroundingStatus.FoundAnyGround : _motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= _jumpPostGroundingGraceTime))
                            {
                                // Calculate jump direction before ungrounding
                                Vector3 jumpDirection = _motor.CharacterUp;
                                if (_motor.GroundingStatus.FoundAnyGround && !_motor.GroundingStatus.IsStableOnGround)
                                {
                                    jumpDirection = _motor.GroundingStatus.GroundNormal;
                                }

                                // Makes the character skip ground probing/snapping on its next update. 
                                // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                                _motor.ForceUnground();

                                // Add to the return velocity and reset jump state
                                currentVelocity += (jumpDirection * _jumpUpSpeed * _jumpUpSpeedMultiple) - Vector3.Project(currentVelocity, _motor.CharacterUp);
                                currentVelocity += (_moveInputVector * _jumpScalableForwardSpeed);
                                _jumpRequested = false;
                                _jumpConsumed = true;
                                _jumpedThisFrame = true;

                                _sfx.PlayJump();
                            }
                        }

                        // Update jetpack logic
                        if (_jetPackEnable && _jetPackFuel > 0f)
                        {
                            if (_motor.GroundingStatus.IsStableOnGround)
                                currentVelocity += (_motor.CharacterUp * _jumpUpSpeed * _jetpackJumpMagnitude) - Vector3.Project(currentVelocity, _motor.CharacterUp);

                            _motor.ForceUnground();

                            _jetPackRefueling = false;

                            currentVelocity += ((_motor.CharacterUp * _jetpackPower) - Vector3.Project(currentVelocity, _motor.CharacterUp)) * deltaTime;

                            _jetPackFuel -= deltaTime * (1f / _jetpackDuration);
                        }
                        else if (_jetPackRefueling)
                        {
                            _jetPackFuel += deltaTime * (1f / _jetpackRefuelDuration);
                            _jetPackFuel = Mathf.Clamp01(_jetPackFuel);
                        }
                        else if (_motor.GroundingStatus.IsStableOnGround)
                        {
                            _jetPackRefueling = true;
                        }

                        // Take into account additive velocity
                        if (_internalVelocityAdd.sqrMagnitude > 0f)
                        {
                            currentVelocity += _internalVelocityAdd;
                            _internalVelocityAdd = Vector3.zero;
                        }
                        break;
                    }
                case State.Climbing:
                    {
                        currentVelocity = Vector3.zero;

                        if (_jumpRequested)
                        {
                            _jumpRequested = false;

                            currentVelocity = _ladder.transformCached.forward * -_stableMoveSpeedMax;
                            _animator.SetJumping(true);
                            _stateMachine.CurrentState = State.Default;

                            return;
                        }

                        switch (_stateClimb)
                        {
                            case StateClimb.Enter:
                                {
                                    Vector3 desirePosition = _ladder.transformCached.TransformPoint(_ladderClimbLocalPoint);
                                    desirePosition.y = Mathf.Clamp(transformCached.position.y, _ladder.transformCached.position.y, _ladder.transformCached.position.y + _ladder.height);

                                    _motor.MoveCharacter(Vector3.Lerp(transformCached.position, desirePosition, _ladderEnterSpeed * Time.deltaTime));

                                    if (Vector3.Distance(transformCached.position, desirePosition) < 0.01f)
                                        _stateClimb = StateClimb.Middle;
                                    break;
                                }
                            case StateClimb.Middle:
                                Vector3 desireVelocity = Vector3.zero;
                                desireVelocity.y = transformCached.InverseTransformPoint(transformCached.position + _moveInputVector).z * _ladderClimbSpeed;
                                currentVelocity = Vector3.Lerp(currentVelocity, desireVelocity, 1f - Mathf.Exp(-_stableMoveSharpness * deltaTime));
                                break;
                            case StateClimb.Exit:
                                {
                                    _ladderClimbLocalPoint.y = _ladder.height;

                                    Vector3 desirePosition = _ladder.transformCached.TransformPoint(_ladderClimbLocalPoint);

                                    desirePosition += _ladderClimbDirection * (_ladderClimbPadding + 0.1f);

                                    _motor.MoveCharacter(Vector3.Lerp(transformCached.position, desirePosition, _ladderExitSpeed * Time.deltaTime));

                                    if (Vector3.Distance(transformCached.position, desirePosition) < 0.1f)
                                        _stateMachine.CurrentState = State.Default;
                                    break;
                                }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            switch (_stateMachine.CurrentState)
            {
                case State.Default:
                    {
                        // Handle jump-related values
                        {
                            // Handle jumping pre-ground grace period
                            if (_jumpRequested && _jumpTimeSinceRequested > _jumpPreGroundingGraceTime)
                            {
                                _jumpRequested = false;
                            }

                            if (_jumpAllowedWhenSliding ? _motor.GroundingStatus.FoundAnyGround : _motor.GroundingStatus.IsStableOnGround)
                            {
                                // If we're on a ground surface, reset jumping values
                                if (!_jumpedThisFrame)
                                {
                                    _jumpConsumed = false;
                                }
                                _timeSinceLastAbleToJump = 0f;
                            }
                            else
                            {
                                // Keep track of time since we were last able to jump (for grace period)
                                _timeSinceLastAbleToJump += deltaTime;
                            }
                        }
                        break;
                    }
            }
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            // Handle landing and leaving ground
            if (_motor.GroundingStatus.IsStableOnGround && !_motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLanded();
            }
            else if (!_motor.GroundingStatus.IsStableOnGround && _motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLeaveStableGround();
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            if (_ignoredColliders.Length == 0)
                return true;

            if (_ignoredColliders.Contains(coll))
                return false;

            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void AddVelocity(Vector3 velocity)
        {
            switch (_stateMachine.CurrentState)
            {
                case State.Default:
                    {
                        _internalVelocityAdd += velocity;
                        break;
                    }
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        protected void OnLanded()
        {
            _animator.SetJumping(false);
        }

        protected void OnLeaveStableGround()
        {
            _animator.SetJumping(true);
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }
    }
}