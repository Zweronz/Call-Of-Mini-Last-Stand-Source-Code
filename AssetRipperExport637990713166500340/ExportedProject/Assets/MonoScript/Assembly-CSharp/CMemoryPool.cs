using UnityEngine;

public class CMemoryPool
{
	public class UnitInfo
	{
		public bool isused;

		public float m_fTime;

		public GameObject gameobject;

		public ParticleEmitter[] emitter;
	}

	private UnitInfo[] m_Pool;

	private int m_nSize;

	public void Initialize(GameObject o, int size)
	{
		m_Pool = new UnitInfo[size];
		m_nSize = size;
		UnitInfo unitInfo = null;
		for (int i = 0; i < size; i++)
		{
			unitInfo = new UnitInfo();
			unitInfo.isused = false;
			unitInfo.gameobject = (GameObject)Object.Instantiate(o, new Vector3(-10000f, -10000f, -10000f), Quaternion.identity);
			unitInfo.emitter = unitInfo.gameobject.GetComponentsInChildren<ParticleEmitter>();
			for (int j = 0; j < unitInfo.emitter.Length; j++)
			{
				unitInfo.emitter[j].emit = false;
			}
			m_Pool[i] = unitInfo;
		}
	}

	public void Destroy()
	{
		for (int i = 0; i < m_Pool.Length; i++)
		{
			if (m_Pool[i].gameobject != null)
			{
				Object.Destroy(m_Pool[i].gameobject);
			}
		}
		m_Pool = null;
		m_nSize = 0;
	}

	public void Update(float deltaTime)
	{
		for (int i = 0; i < m_nSize; i++)
		{
			if (m_Pool[i].gameobject == null || !m_Pool[i].isused)
			{
				continue;
			}
			m_Pool[i].m_fTime -= deltaTime;
			if (m_Pool[i].m_fTime <= 0f)
			{
				m_Pool[i].m_fTime = 0f;
				m_Pool[i].isused = false;
				m_Pool[i].gameobject.transform.parent = null;
				m_Pool[i].gameobject.transform.position = new Vector3(-10000f, -10000f, -10000f);
				m_Pool[i].gameobject.transform.eulerAngles = Vector3.zero;
				for (int j = 0; j < m_Pool[i].emitter.Length; j++)
				{
					m_Pool[i].emitter[j].emit = false;
				}
			}
		}
	}

	public GameObject GetFree(float fTime)
	{
		for (int i = 0; i < m_nSize; i++)
		{
			if (!m_Pool[i].isused)
			{
				m_Pool[i].m_fTime = fTime;
				m_Pool[i].isused = true;
				for (int j = 0; j < m_Pool[i].emitter.Length; j++)
				{
					m_Pool[i].emitter[j].emit = true;
				}
				return m_Pool[i].gameobject;
			}
		}
		return null;
	}
}
