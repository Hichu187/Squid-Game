using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private Character _char;

    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeeTarget;

    public List<Transform> visibleTargets = new List<Transform>();
    private void Start()
    {
        _char = GetComponent<Character>();
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        visibleTargets.Clear();

        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length > 1)
        {
            foreach (Collider col in rangeChecks)
            {
                if (col.gameObject != gameObject)
                {
                    Transform target = col.transform;
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);

                        if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        {
                            visibleTargets.Add(target.transform);
                        }
                    }
                }
            }

            canSeeTarget = visibleTargets.Count > 0;

            _char.animator.PlayBlock();
        }
        else
        {
            canSeeTarget = false;
        }

    }
}
