using UnityEngine;

public class TUIScaleAnim : MonoBehaviour
{
	public float m_fScaleFrom = 1f;

	public float m_fScaleTo = 1f;

	public float m_fTime = 1f;

	private bool m_bAnim;

	private Transform m_Transform;

	private float m_fCurScale;

	private float m_fCurSpeed;

	private void Awake()
	{
		m_Transform = base.transform;
	}

	private void Update()
	{
		if (!m_bAnim || m_fScaleTo == m_fScaleFrom)
		{
			return;
		}
		m_fCurScale += m_fCurSpeed * Time.deltaTime;
		if (m_fScaleFrom > m_fScaleTo)
		{
			if (m_fCurScale < m_fScaleTo)
			{
				m_fCurScale = m_fScaleTo;
				m_bAnim = false;
			}
		}
		else if (m_fCurScale > m_fScaleTo)
		{
			m_fCurScale = m_fScaleTo;
			m_bAnim = false;
		}
		m_Transform.localScale = new Vector3(m_fCurScale, m_fCurScale, m_fCurScale);
	}

	public void Begin()
	{
		if (m_fScaleTo != m_fScaleFrom)
		{
			m_bAnim = true;
			m_fCurScale = m_fScaleFrom;
			m_fCurSpeed = (m_fScaleTo - m_fScaleFrom) / m_fTime;
			m_Transform.localScale = new Vector3(m_fCurScale, m_fCurScale, m_fCurScale);
		}
	}
}
