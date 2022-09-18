using UnityEngine;

public class TUIFlex : MonoBehaviour
{
	public bool m_bFlexLoop = true;

	public float m_fFlexScaleInit = 1f;

	public float m_fFlexPower = 0.8f;

	public float m_fFlexSpeed = 1f;

	public float m_fFlexSpeedAt = 1f;

	private Transform m_Transform;

	private float m_fScrScale = 1f;

	private float m_fFlexMin;

	private float m_fFlexMax;

	private bool m_bAnim;

	private int m_nDir;

	private float m_fCurSpeed;

	private float m_fCurSpeedAt;

	private float m_fCurScale;

	private float m_fCurFlex;

	private float m_fCurFlexMin;

	private float m_fCurFlexMax;

	private void Awake()
	{
		m_Transform = base.transform;
	}

	private void Start()
	{
		Begin();
	}

	private void Update()
	{
		if (!m_bAnim)
		{
			return;
		}
		m_fCurScale += m_fCurSpeed * Time.deltaTime * (float)m_nDir;
		m_fCurSpeed -= m_fCurSpeedAt * Time.deltaTime;
		m_fCurSpeedAt += Time.deltaTime;
		if (m_fCurScale > m_fCurFlexMax)
		{
			m_fCurScale = m_fCurFlexMax;
			m_nDir = -1;
		}
		else if (m_fCurScale < m_fCurFlexMin)
		{
			m_fCurScale = m_fCurFlexMin;
			m_nDir = 1;
		}
		if (m_fCurFlexMax > m_fCurFlexMin)
		{
			m_fCurFlexMax -= m_fCurFlex * Time.deltaTime;
		}
		else
		{
			m_fCurFlexMax = m_fCurFlexMin;
		}
		m_Transform.localScale = new Vector3(m_fCurScale, m_fCurScale, m_fCurScale);
		if (m_fCurSpeed <= 0f)
		{
			m_Transform.localScale = new Vector3(m_fScrScale, m_fScrScale, m_fScrScale);
			if (m_bFlexLoop)
			{
				Begin();
			}
			else
			{
				m_bAnim = false;
			}
		}
	}

	public void Begin()
	{
		if (!m_bAnim)
		{
			m_fScrScale = base.transform.localScale.x;
		}
		m_bAnim = true;
		m_nDir = 1;
		m_fCurSpeed = m_fFlexSpeed;
		m_fCurSpeedAt = m_fFlexSpeedAt;
		m_fCurScale = m_fFlexScaleInit;
		m_fFlexMin = m_fCurScale;
		m_fFlexMax = m_fCurScale * m_fFlexPower;
		if (m_fFlexMin > m_fFlexMax)
		{
			float fFlexMin = m_fFlexMin;
			m_fFlexMin = m_fFlexMax;
			m_fFlexMax = fFlexMin;
		}
		m_fCurFlexMin = m_fFlexMin;
		m_fCurFlexMax = m_fFlexMax;
		m_fCurFlex = (m_fFlexMax - m_fFlexMin) * 0.1f;
		m_Transform.localScale = new Vector3(m_fFlexScaleInit, m_fFlexScaleInit, m_fFlexScaleInit);
	}

	public void Stop()
	{
		if (m_bAnim)
		{
			m_bAnim = false;
			m_Transform.localScale = new Vector3(m_fScrScale, m_fScrScale, m_fScrScale);
		}
	}

	public bool IsAnim()
	{
		return m_bAnim;
	}
}
