using UnityEngine;

public class iZombieSniperAlphaDisappear : MonoBehaviour
{
	private float m_fAlphaFade;

	private float m_fTimeCount;

	private void Start()
	{
		m_fAlphaFade = 0.1f;
		m_fTimeCount = 0f;
	}

	private void Update()
	{
		if (base.gameObject.GetComponent<Renderer>() == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		m_fTimeCount -= Time.deltaTime;
		if (!(m_fTimeCount > 0f))
		{
			m_fTimeCount = 0.2f;
			Color color = base.gameObject.GetComponent<Renderer>().material.color;
			color.a -= m_fAlphaFade * m_fTimeCount;
			base.gameObject.GetComponent<Renderer>().material.color = color;
			if (color.a <= 0f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void SetFadeSpeed(float fAlphaFade)
	{
		m_fAlphaFade = fAlphaFade;
	}
}
