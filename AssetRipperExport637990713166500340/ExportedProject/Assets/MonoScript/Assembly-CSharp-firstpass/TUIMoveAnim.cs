using UnityEngine;

public class TUIMoveAnim : MonoBehaviour
{
	public Vector2 m_v2Scr = Vector2.zero;

	public Vector2 m_v2Dst = Vector2.zero;

	public float m_fTime = 1f;

	public float m_fSpeed = 1f;

	private bool m_bAnim;

	private Transform m_Transform;

	private float m_fCurSpeed;

	private float m_fSpeedAcc = 1f;

	private void Awake()
	{
		m_Transform = base.transform;
	}

	private void Update()
	{
		if (!m_bAnim)
		{
			return;
		}
		if (m_v2Scr == m_v2Dst)
		{
			m_bAnim = false;
			return;
		}
		m_fCurSpeed += m_fSpeedAcc * Time.deltaTime;
		if (m_fCurSpeed < 0.1f)
		{
			m_fCurSpeed = 0.1f;
		}
		Vector2 vector = m_v2Dst - m_v2Scr;
		float magnitude = vector.magnitude;
		Vector2 vector2 = vector / magnitude;
		float num = m_fCurSpeed * Time.deltaTime;
		if (num > magnitude)
		{
			m_v2Scr = m_v2Dst;
			if (m_Transform.parent == null)
			{
				m_Transform.position = new Vector3(m_v2Scr.x, m_v2Scr.y, m_Transform.position.z);
			}
			else
			{
				m_Transform.localPosition = new Vector3(m_v2Scr.x, m_v2Scr.y, m_Transform.position.z);
			}
			m_bAnim = false;
		}
		else
		{
			m_v2Scr += num * vector2;
			if (m_Transform.parent == null)
			{
				m_Transform.position = new Vector3(m_v2Scr.x, m_v2Scr.y, m_Transform.position.z);
			}
			else
			{
				m_Transform.localPosition = new Vector3(m_v2Scr.x, m_v2Scr.y, m_Transform.position.z);
			}
		}
	}

	public void Begin(Vector2 v2Scr, Vector2 v3Dst)
	{
		if (!(v2Scr == v3Dst))
		{
			m_bAnim = true;
			m_v2Scr = v2Scr;
			m_v2Dst = v3Dst;
			if (m_Transform.parent == null)
			{
				m_Transform.position = new Vector3(v2Scr.x, v2Scr.y, m_Transform.position.z);
			}
			else
			{
				m_Transform.localPosition = new Vector3(v2Scr.x, v2Scr.y, m_Transform.position.z);
			}
			float magnitude = (m_v2Dst - m_v2Scr).magnitude;
			m_fSpeedAcc = (magnitude - m_fSpeed * m_fTime) * 2f / (m_fTime * m_fTime);
			m_fCurSpeed = m_fSpeed;
		}
	}
}
