using UnityEngine;

[ExecuteInEditMode]
public class CMoveGo : CMoveBase
{
	public Vector3 m_v3Pos;

	public Vector3 m_v3Dir;

	public float m_fSpeed;

	private void OnDrawGizmos()
	{
		m_v3Pos = base.transform.position;
		m_v3Dir = base.transform.forward;
	}
}
