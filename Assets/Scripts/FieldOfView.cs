using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
	public float viewRadius;
	public float hearingRadius;
	[Range(0, 360)]
	public float viewAngle;
	bool hasSeenPlayer = false;
	public EnemyBehaviour enemyBehaviour;
	public LayerMask targetMask;
	public LayerMask obstacleMask;

    void Start()
	{
		StartCoroutine(FindTargetsWithDelay(5f));
	}

	IEnumerator FindTargetsWithDelay(float cooldown)
	{
		while (true)
		{
			Transform target = HearTarget();
			if (target)
            {
				enemyBehaviour.SetTarget(target);
				Debug.Log("Has a Target");
			}
			else
            {
				Debug.Log("Hasn't a Target");
				if (hasSeenPlayer)
				{
					yield return new WaitForSeconds(cooldown);
					FindVisibleTargets();
				}
				else
				{
					FindVisibleTargets();
				}
			}
			yield return new WaitForEndOfFrame();
		}
	}

	Transform HearTarget()
    {
		Collider[] collidedObjects = Physics.OverlapSphere(transform.position, hearingRadius, targetMask);
		Transform target = null;

		if(collidedObjects.Length > 0)
        {
			for (int i = 0; i < collidedObjects.Length; i++)
			{
				if (collidedObjects[i].gameObject.CompareTag("Player"))
				{
					target = collidedObjects[i].transform;
				}
			}
		}

		return target;
	}

	void FindVisibleTargets()
	{
		enemyBehaviour.SetTarget(null);

		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
		Transform finalTarget = null;

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
			{
				float dstToTarget = Vector3.Distance(transform.position, target.position);

				if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
				{
					hasSeenPlayer = true;
					StartCoroutine(ForgetPlayer());
					finalTarget = target;
					break;
				}
			}
		}

		enemyBehaviour.SetTarget(finalTarget);
	}

	IEnumerator ForgetPlayer()
    {
		yield return new WaitForSeconds(3f);
		hasSeenPlayer = false;
    }

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}