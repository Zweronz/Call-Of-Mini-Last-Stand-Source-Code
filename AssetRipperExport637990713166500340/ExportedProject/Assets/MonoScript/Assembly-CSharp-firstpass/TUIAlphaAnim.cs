using UnityEngine;

[RequireComponent(typeof(TUIMesh))]
public class TUIAlphaAnim : MonoBehaviour
{
	public float m_fCurAlpha;

	public float m_fMinAlpha;

	public float m_fMaxAlpha = 1f;

	public float m_fSpeedInc = 1f;

	public float m_fSpeedDec = 1f;

	public bool m_bAutoPlay;

	private bool m_bPlay;

	private int m_nDir;

	private float m_fCurSpeed;

	private float m_fScrAlpha;

	private TUIMesh m_mesh;

	private void Start()
	{
		m_bPlay = false;
		m_mesh = base.transform.GetComponent<TUIMesh>();
		if (m_bAutoPlay)
		{
			Play();
		}
	}

	private void Update()
	{
		if (m_bPlay && !(m_mesh == null))
		{
			m_fCurAlpha += m_fCurSpeed * Time.deltaTime * (float)m_nDir;
			if (m_fCurAlpha <= m_fMinAlpha)
			{
				m_fCurAlpha = m_fMinAlpha;
				m_nDir = 1;
				m_fCurSpeed = m_fSpeedInc;
			}
			else if (m_fCurAlpha >= m_fMaxAlpha)
			{
				m_fCurAlpha = m_fMaxAlpha;
				m_nDir = -1;
				m_fCurSpeed = m_fSpeedDec;
			}
			m_mesh.color = new Color(m_mesh.color.r, m_mesh.color.g, m_mesh.color.b, m_fCurAlpha);
		}
	}

	public void Play()
	{
		if (!(m_mesh == null))
		{
			m_bPlay = true;
			if (m_fCurAlpha < m_fMaxAlpha)
			{
				m_nDir = 1;
				m_fCurSpeed = m_fSpeedInc;
			}
			else
			{
				m_nDir = -1;
				m_fCurSpeed = m_fSpeedDec;
			}
			m_fScrAlpha = m_mesh.color.a;
		}
	}

	public void Stop()
	{
		if (!(m_mesh == null))
		{
			m_bPlay = false;
			m_mesh.color = new Color(m_mesh.color.r, m_mesh.color.g, m_mesh.color.b, m_fScrAlpha);
		}
	}
}
