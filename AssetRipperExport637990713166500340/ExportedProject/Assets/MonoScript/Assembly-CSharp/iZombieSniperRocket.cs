using UnityEngine;

public class iZombieSniperRocket : MonoBehaviour
{
	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameState m_GameState;

	public iZombieSniperPerfabManager m_PerfabManager;

	private Vector3 m_v3Dir;

	private float m_fSpeed;

	private float m_fRange;

	private float m_fDamage;

	private bool m_bNeedClinder;

	private bool m_bBlue;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
		base.transform.position += m_v3Dir * m_fSpeed * Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 30)
		{
			return;
		}
		switch (m_GameState.m_nCurStage)
		{
		case 0:
			if (other.gameObject.name.IndexOf("sky_01") != -1)
			{
				return;
			}
			break;
		case 1:
			if (other.gameObject.name.IndexOf("Z_sky") != -1)
			{
				return;
			}
			break;
		case 2:
			if (other.gameObject.name.IndexOf("sky") != -1)
			{
				return;
			}
			break;
		}
		if (m_bNeedClinder)
		{
			switch (m_GameState.m_nCurStage)
			{
			case 0:
				if (other.transform.root.name == "truck")
				{
					iZombieSniperTruck truckScript2 = m_GameScene.GetTruckScript();
					if (truckScript2 != null && !truckScript2.IsDead())
					{
						truckScript2.AddHP(0f - m_fDamage);
					}
				}
				break;
			case 1:
				if (other.transform.root.name == "TruckScene1")
				{
					iZombieSniperTruck truckScript = m_GameScene.GetTruckScript();
					if (truckScript != null && !truckScript.IsDead())
					{
						truckScript.AddHP(0f - m_fDamage);
					}
				}
				break;
			}
		}
		m_GameScene.Boom(base.transform.position, m_fRange, m_fDamage, !m_bNeedClinder);
		Object.Destroy(base.gameObject);
		GameObject gameObject = null;
		Vector3 vector = Vector3.zero;
		bool flag = false;
		switch (m_GameState.m_nCurStage)
		{
		case 0:
			if (other.gameObject.name == "gound_01")
			{
				flag = true;
			}
			break;
		case 1:
			if (other.gameObject.name == "Z_airport_floor01" || other.gameObject.name == "Z_airport_floor03" || other.gameObject.name == "Z_airport_floor04")
			{
				flag = true;
			}
			break;
		case 2:
			if (other.gameObject.name == "ground")
			{
				flag = true;
			}
			break;
		}
		if (!flag)
		{
			gameObject = ((!m_bBlue) ? m_PerfabManager.m_Boom01 : m_PerfabManager.m_BoomBlue01);
		}
		else if (m_bBlue)
		{
			gameObject = m_PerfabManager.m_BoomBlue02;
			vector = new Vector3(0f, 0.2f, 0f);
		}
		else
		{
			gameObject = m_PerfabManager.m_Boom02;
			vector = new Vector3(0f, 1.3f, 0f);
		}
		if (gameObject != null)
		{
			GameObject obj = (GameObject)Object.Instantiate(gameObject, base.transform.position + vector, Quaternion.identity);
			Object.Destroy(obj, 5f);
		}
		m_GameScene.m_CameraScript.Shake(1.5f, 0.3f);
		if (m_bNeedClinder && flag)
		{
			m_GameScene.AddCinderEffect(base.transform.position);
		}
		if (m_bBlue)
		{
			m_GameScene.PlayAudio("FxExploEnergyLarge01");
		}
		else if (m_bNeedClinder)
		{
			m_GameScene.PlayAudio("FxExploMed01");
		}
		else
		{
			m_GameScene.PlayAudio("FxExploLarge01");
		}
	}

	public void Initialize(Vector3 v3Dir, float fSpeed, float fRange, float fDamage, bool bNeedClinder = true, bool bBlue = false)
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_PerfabManager = m_GameScene.m_PerfabManager;
		m_v3Dir = v3Dir.normalized;
		m_fSpeed = fSpeed;
		m_fRange = fRange;
		m_fDamage = fDamage;
		m_bNeedClinder = bNeedClinder;
		m_bBlue = bBlue;
		base.transform.forward = m_v3Dir;
		Object.Destroy(base.gameObject, 10f);
	}
}
