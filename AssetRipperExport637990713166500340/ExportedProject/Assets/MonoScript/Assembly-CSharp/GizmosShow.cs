using UnityEngine;

public class GizmosShow : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
		foreach (Transform item in base.transform)
		{
			Gizmos.DrawSphere(item.position, 1f);
		}
	}
}
