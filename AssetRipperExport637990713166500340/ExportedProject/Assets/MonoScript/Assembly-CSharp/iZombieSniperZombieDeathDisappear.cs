using UnityEngine;

public class iZombieSniperZombieDeathDisappear : MonoBehaviour
{
	private enum Status
	{
		kFalling = 0,
		kWaiting = 1,
		kDowning = 2
	}

	private float m_fStartDownTime;

	private float m_fAlpha;

	private Renderer[] m_Renderer;

	private Status m_Status;

	private Transform m_ModelTransForm;

	private void Start()
	{
		m_ModelTransForm = base.transform;
		m_fStartDownTime = 0f;
		m_Status = Status.kWaiting;
		if (m_Renderer == null)
		{
			m_Renderer = GetComponentsInChildren<SkinnedMeshRenderer>();
		}
	}

	private void Update()
	{
		if (m_Renderer == null)
		{
			return;
		}
		switch (m_Status)
		{
		case Status.kWaiting:
			m_fStartDownTime += Time.deltaTime;
			if (m_fStartDownTime > 5f)
			{
				m_Status = Status.kDowning;
				m_fAlpha = 1f;
			}
			break;
		case Status.kDowning:
		{
			m_fAlpha -= Time.deltaTime * 0.5f;
			if (m_fAlpha <= 0f)
			{
				m_fAlpha = 0f;
				Object.Destroy(base.gameObject);
			}
			for (int i = 0; i < m_Renderer.Length; i++)
			{
				Color color = m_Renderer[i].material.color;
				m_Renderer[i].material.color = new Color(color.r, color.b, color.g, m_fAlpha);
			}
			m_ModelTransForm.position -= new Vector3(0f, (0.8f - 0.8f * m_fAlpha) * 4f * Time.deltaTime, 0f);
			break;
		}
		}
	}
}
