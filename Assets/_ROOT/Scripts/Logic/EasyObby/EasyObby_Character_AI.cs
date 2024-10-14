using LFramework;
using UnityEngine;

namespace Game
{
    public class EasyObby_Character_AI : MonoBehaviour
    {
        private AI _ai;
        private EasyObby_Character _character;

        private AIWaypoint _waypoint;
        private bool _waypointChasing = false;

        private void Awake()
        {
            _ai = GetComponentInParent<AI>();
            _character = GetComponent<EasyObby_Character>();
        }

        private void Start()
        {
            _ai.eventChaseComplete += AI_EventChaseComplete;
            _ai.eventIdleComplete += AI_EventIdleComplete;

            _character.character.eventDie += Character_EventDie;
            _character.character.eventRevive += Character_EventRevive;

            ReviveAtCheckpointNearPlayer();
        }

        private void Character_EventRevive()
        {
            ChaseNextCheckpoint();
        }

        private void Character_EventDie()
        {
            _waypoint = null;

            UpdateCheckpointNearPlayer();
        }

        private void AI_EventIdleComplete()
        {
            ChaseNextWaypoint();
        }

        private void AI_EventChaseComplete()
        {
            IdleToNextWaypoint();
        }

        private void ChaseNextWaypoint()
        {
            if (_waypointChasing)
            {
                // If chase to last waypoint of current path, go to next checkpoint position
                if (_waypoint == null)
                {
                    _waypointChasing = false;

                    _ai.Chase(_character.checkpointNext.GetRandomPosition());
                }
                else
                {
                    if (_waypoint.type == AIWaypointType.WaitForDistance)
                        _ai.Chase(_waypoint.transformCached);
                    else
                        _ai.Chase(_waypoint.GetRandomPosition());
                }
            }
            else
            {
                ChaseNextCheckpoint();
            }
        }

        private void IdleToNextWaypoint()
        {
            Transform nextTransform = (_waypoint == null || _waypoint.next.IsNullOrEmpty()) ? _character.checkpointNext.transformCached : _waypoint.next.GetRandom().transformCached;

            if (_waypoint != null && _waypoint.type == AIWaypointType.WaitForDistance)
                _ai.IdleWaitForDistance(nextTransform, _waypoint.radius);
            else
                _ai.Idle();

            // Assign next waypoint into current waypoint
            _waypoint = (_waypoint == null || _waypoint.next.IsNullOrEmpty()) ? null : _waypoint.next.GetRandom();
        }

        private void ChaseNextCheckpoint()
        {
            _waypoint = null;
            _waypointChasing = false;

            EasyObby_Stage stage = EasyObby_StageManager.stages.GetClamp(_character.checkpointCurrent.index);

            if (stage.aiWaypoints.IsNullOrEmpty())
            {
                _ai.Chase(_character.checkpointNext.GetRandomPosition());
            }
            else
            {
                _waypoint = stage.aiWaypoints.First();
                _waypointChasing = true;

                ChaseNextWaypoint();
            }
        }

        private void UpdateCheckpointNearPlayer()
        {
            int index = _character.checkpointCurrent.index;

            // Follow up player current stage
            if (index < DataEasyObby.checkpointIndex.value + FactoryEasyObby.aiCheckpointRange.x || index > DataEasyObby.checkpointIndex.value + FactoryEasyObby.aiCheckpointRange.y)
                _character.checkpointCurrent = EasyObby_StageManager.checkpoints.GetClamp(DataEasyObby.checkpointIndex.value + FactoryEasyObby.aiCheckpointRange.RandomWithin());
        }

        private void ReviveAtCheckpointNearPlayer()
        {
            _character.checkpointCurrent = EasyObby_StageManager.checkpoints.GetClamp(DataEasyObby.checkpointIndex.value + FactoryEasyObby.aiCheckpointRange.RandomWithin());
            _character.Revive();
        }
    }
}