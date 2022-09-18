using UnityEngine;

public class UVAnim : MonoBehaviour
{
	protected Material material;

	protected float m_fUVPos;

	protected bool m_bActive;

	private void Start()
	{
		material = GetComponent<Renderer>().material;
		m_bActive = false;
	}

	private void Update()
	{
		if (m_bActive)
		{
			m_fUVPos += Time.deltaTime;
			if (m_fUVPos >= 1f)
			{
				m_fUVPos = 0f;
			}
			material.mainTextureOffset = new Vector2(m_fUVPos, 0f);
		}
	}

	public void Play()
	{
		m_bActive = true;
	}

	public void Stop()
	{
		m_bActive = false;
	}

	public bool IsPlay()
	{
		return m_bActive;
	}
}
