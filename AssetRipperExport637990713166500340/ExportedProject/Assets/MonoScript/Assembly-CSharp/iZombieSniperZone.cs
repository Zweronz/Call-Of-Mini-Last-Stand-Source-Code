using UnityEngine;

public class iZombieSniperZone : MonoBehaviour
{
	public Color ZoneColor = Color.red;

	public Vector3 v3Size = new Vector3(1f, 1f, 1f);

	private void OnDrawGizmos()
	{
		Gizmos.color = ZoneColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
