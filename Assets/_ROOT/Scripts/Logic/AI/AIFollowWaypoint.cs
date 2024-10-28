using JetBrains.Annotations;
using LFramework;
using UnityEngine;

namespace Game
{
    public class AIFollowWaypoint : MonoBehaviour
    {
        private AI _ai;

        private AIWaypoint _waypointPrevious;
        private AIWaypoint _waypoint;
        private AIWaypoint _waypointNext;

        public bool isCombat = false;
        

        private void Awake()
        {
            _ai = GetComponent<AI>();
        }

        private void Start()
        {
            _ai.eventIdleComplete += AI_EventIdleComplete;
            _ai.eventChaseComplete += AI_EventChaseComplete;

            _ai.character.eventRevive += Character_EventRevive;

            FollowNearestWaypoint();
        }

        private void AI_EventChaseComplete()
        {
            if (isCombat) return;
            if (_waypoint.next.IsNullOrEmpty())
            {
                _ai.Idle();
                return;
            }

            // Get next waypoint
            _waypointNext = _waypoint.next.GetRandom();

            if (_waypoint.type == AIWaypointType.WaitForDistance)
                _ai.IdleWaitForDistance(_waypointNext.transformCached, _waypoint.radius);
            else
                _ai.Idle();
        }

        private void AI_EventIdleComplete()
        {
            if (isCombat) return;
            // If AI not reached position
            if (!_waypoint.IsReached(_ai.character.transformCached.position))
            {
                FollowWaypoint();
                return;
            }

            /*
            // Waypoint to high
            if (_waypoint.transformCached.position.y - _ai.character.transformCached.position.y > 3f)
            {
                FollowNearestWaypoint();
                return;
            }
            */

            if (!_waypoint.next.IsNullOrEmpty())
            {
                FollowNextWaypoint();
            }
            else
            {
                FollowWaypoint();
            }
        }

        private void Character_EventRevive()
        {
            FollowNearestWaypoint();
        }

        private void FollowNearestWaypoint()
        {
            _waypointPrevious = null;
            _waypoint = AIWaypointManager.Instance.GetNearestWaypoint(_ai.character.transformCached.position);
            _waypointNext = null;

            _ai.Chase(_waypoint.GetRandomPosition());
        }

        private void FollowNextWaypoint()
        {
            _waypointPrevious = _waypoint;
            _waypoint = _waypointNext;
            _waypointNext = null;

            FollowWaypoint();
        }

        private void FollowWaypoint()
        {
            if (_waypointPrevious != null && _waypointPrevious.type == AIWaypointType.WaitForDistance)
                _ai.Chase(_waypoint.transformCached);
            else
                _ai.Chase(_waypoint.GetRandomPosition());
        }
    }
}
