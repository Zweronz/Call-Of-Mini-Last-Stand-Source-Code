using UnityEngine;

public class iZombieSniperOilCan
{
	public iZombieSniperGameSceneBase m_GameScene;

	public GameObject m_Model;

	public Transform m_ModelTransForm;

	public ModelInfo m_ModelInfo;

	public int m_nUID;

	public int m_nPointID;

	public bool m_bDestroy;

	public bool m_bBoom;

	public float m_fBoomTime;

	public bool m_bWaitForDestroy;

	public float m_fWaitTime;

	public float m_fDamage;

	public float m_fRange;

	public void Initialize(int nUID, int nPointID, Vector3 v3Pos, float fDamage, float fRange)
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_Model = (GameObject)Object.Instantiate(m_GameScene.m_PerfabManager.OilCan, v3Pos, Quaternion.identity);
		m_ModelTransForm = m_Model.transform;
		m_Model.tag = "OIL";
		m_Model.layer = 31;
		m_ModelTransForm.localEulerAngles = new Vector3(270f, 0f, 0f);
		m_ModelInfo = m_Model.AddComponent<ModelInfo>();
		m_ModelInfo.m_nUID = nUID;
		m_nUID = nUID;
		m_nPointID = nPointID;
		m_bDestroy = false;
		m_bWaitForDestroy = false;
		m_fWaitTime = 0f;
		m_bBoom = false;
		m_fBoomTime = 0f;
		m_fDamage = fDamage;
		m_fRange = fRange;
	}

	public void Destroy()
	{
		if (m_Model != null)
		{
			Object.Destroy(m_Model);
			m_Model = null;
		}
	}

	public void Update(float deltaTime)
	{
		if (m_bWaitForDestroy)
		{
			m_fWaitTime -= deltaTime;
			if (m_fWaitTime <= 0f)
			{
				m_bDestroy = true;
			}
		}
		else
		{
			if (!m_bBoom)
			{
				return;
			}
			m_fBoomTime += deltaTime;
			if (!(m_fBoomTime < 0.1f))
			{
				m_fBoomTime = 0f;
				m_GameScene.m_CameraScript.Shake(1.5f, 1f);
				m_GameScene.Boom(m_ModelTransForm.position, m_fRange, m_fDamage);
				m_GameScene.PlayAudio("FxExploMed01");
				GameObject obj = (GameObject)Object.Instantiate(m_GameScene.m_PerfabManager.m_Boom02, m_ModelTransForm.position + new Vector3(0f, 1.3f, 0f), Quaternion.identity);
				Object.Destroy(obj, 5f);
				m_GameScene.AddCinderEffect(m_ModelTransForm.position);
				m_GameScene.m_fOilCanRefreshCount -= 10f;
				if (m_GameScene.m_fOilCanRefreshCount < 0f)
				{
					m_GameScene.m_fOilCanRefreshCount = 0f;
				}
				m_GameScene.SoundStrike(m_ModelTransForm.position, 10f);
				if (m_Model.GetComponent<Animation>() != null && m_Model.GetComponent<Animation>()["arean"] != null)
				{
					m_Model.GetComponent<Animation>()["arean"].wrapMode = WrapMode.Once;
					m_Model.GetComponent<Animation>().Play("arean");
					m_fWaitTime = m_Model.GetComponent<Animation>()["arean"].length;
				}
				m_bWaitForDestroy = true;
			}
		}
	}
}
