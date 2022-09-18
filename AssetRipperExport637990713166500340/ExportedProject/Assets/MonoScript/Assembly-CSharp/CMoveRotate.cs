using UnityEngine;

public class CMoveRotate : CMoveBase
{
	public Vector3 m_v3Dir;

	public float m_fSpeed;

	private void OnDrawGizmos()
	{
		m_v3Dir = base.transform.forward;
	}
}
