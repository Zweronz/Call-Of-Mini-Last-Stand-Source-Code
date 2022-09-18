using UnityEngine;

public class cgTimer : MonoBehaviour
{
	protected float m_TimeCount;

	protected bool m_isPlay;

	protected float m_fTimeScale;

	public float TimeTotal
	{
		get
		{
			return m_TimeCount;
		}
	}

	public bool isPlay
	{
		get
		{
			return m_isPlay;
		}
		set
		{
			m_isPlay = value;
		}
	}

	public float TimeScale
	{
		get
		{
			return m_fTimeScale;
		}
		set
		{
			m_fTimeScale = value;
		}
	}

	private void Awake()
	{
		m_TimeCount = 0f;
		m_isPlay = false;
	}

	private void Update()
	{
		if (m_isPlay)
		{
			m_TimeCount += Time.deltaTime * m_fTimeScale;
		}
	}
}
