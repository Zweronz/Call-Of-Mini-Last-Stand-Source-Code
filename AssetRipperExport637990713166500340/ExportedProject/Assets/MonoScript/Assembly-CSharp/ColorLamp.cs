using UnityEngine;

public class ColorLamp : MonoBehaviour
{
	public enum State
	{
		None = 0,
		Wait = 1,
		Running = 2
	}

	public Vector2 randomtime = new Vector2(2f, 2f);

	public Vector2 alpharange = new Vector2(0.1f, 1f);

	public Vector2 alphaspeed = new Vector2(2.5f, 3.5f);

	public Vector2 timesrange = new Vector2(5f, 7f);

	public State m_State;

	private Renderer m_MeshRender;

	private float m_fCurTime;

	private float m_fLstAlpha;

	private float m_fCurAlpha;

	private float m_fCurSpeed;

	private int m_nDir;

	private int m_nTimes;

	private float m_fCount;

	private void Start()
	{
		m_MeshRender = GetComponent<MeshRenderer>();
		if (m_MeshRender == null)
		{
			m_MeshRender = GetComponent<SkinnedMeshRenderer>();
		}
		m_fCount = 0f;
		m_fCurTime = UnityEngine.Random.Range(randomtime.x, randomtime.y);
		m_State = State.Wait;
	}

	private void Update()
	{
		if (m_MeshRender == null)
		{
			return;
		}
		m_fCount += Time.deltaTime;
		switch (m_State)
		{
		case State.Wait:
			if (!(m_fCount < m_fCurTime))
			{
				m_fCount = 0f;
				m_nDir = 1;
				m_fLstAlpha = m_MeshRender.material.color.a;
				m_fCurAlpha = m_fLstAlpha;
				m_fCurSpeed = UnityEngine.Random.Range(alphaspeed.x, alphaspeed.y);
				m_nTimes = (int)UnityEngine.Random.Range(timesrange.x, timesrange.y);
				m_State = State.Running;
			}
			break;
		case State.Running:
		{
			if (m_nTimes <= 0)
			{
				m_fCount = 0f;
				m_fCurTime = UnityEngine.Random.Range(randomtime.x, randomtime.y);
				m_State = State.Wait;
				break;
			}
			m_fCurAlpha += m_fCurSpeed * (float)m_nDir * Time.deltaTime;
			if (m_fCurAlpha >= alpharange.y)
			{
				m_nDir *= -1;
			}
			else if (m_fCurAlpha < alpharange.x)
			{
				m_nTimes--;
				if (m_nTimes > 0)
				{
					m_nDir *= -1;
				}
				else
				{
					m_fCurAlpha = m_fLstAlpha;
				}
			}
			Color color = m_MeshRender.material.color;
			color.a = m_fCurAlpha;
			m_MeshRender.material.color = color;
			break;
		}
		}
	}
}
