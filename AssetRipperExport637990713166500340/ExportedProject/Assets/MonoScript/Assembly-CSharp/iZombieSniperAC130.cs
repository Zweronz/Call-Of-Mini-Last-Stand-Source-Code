using UnityEngine;

public class iZombieSniperAC130 : MonoBehaviour
{
	private float m_fWidth;

	private float m_fWidthSpace;

	private float m_fHeight;

	private float m_fHeightSpace;

	private float m_fInterval;

	private Vector3 m_v3Start;

	private float m_fTimeCount;

	public iZombieSniperPerfabManager m_PerfabManager;

	private float m_fH;

	private float m_fW;

	private int m_nACCount;

	private void Start()
	{
	}

	private void Update()
	{
		m_fTimeCount += Time.deltaTime;
		if (m_fTimeCount < m_fInterval)
		{
			return;
		}
		m_fTimeCount = 0f;
		while (m_fH > 0f)
		{
			Vector3 vector = new Vector3(m_v3Start.x + m_fW, m_v3Start.y, m_v3Start.z + m_fH) + new Vector3(50f * UnityEngine.Random.Range(-1f, 1f), 0f, 50f * UnityEngine.Random.Range(-1f, 1f));
			Vector3 v3Dir = new Vector3(m_v3Start.x + m_fW, 0f, m_v3Start.z + m_fH) - vector;
			GameObject gameObject = (GameObject)Object.Instantiate(m_PerfabManager.Rocket01, vector, Quaternion.identity);
			if (gameObject == null)
			{
				return;
			}
			iZombieSniperRocket component = gameObject.GetComponent<iZombieSniperRocket>();
			if (component == null)
			{
				return;
			}
			component.Initialize(v3Dir, 20f, 20f, 200f, false);
			gameObject.tag = "Rocket";
			m_fH -= m_fHeightSpace;
			m_nACCount++;
		}
		m_fH = m_fHeight;
		m_fW -= m_fWidthSpace;
		if (m_fW < 0f)
		{
			Debug.Log(m_nACCount);
			Object.Destroy(base.gameObject);
		}
	}

	public void Initialize(Vector3 v3Start, float interval, float width, float height, float widthspace, float heightspace)
	{
		m_v3Start = v3Start;
		m_fInterval = interval;
		m_fWidth = width;
		m_fHeight = height;
		m_fWidthSpace = widthspace;
		m_fHeightSpace = heightspace;
		m_fW = m_fWidth;
		m_fH = m_fHeight;
		m_nACCount = 0;
		m_PerfabManager = iZombieSniperGameApp.GetInstance().m_GameScene.m_PerfabManager;
	}
}
