using UnityEngine;

public class CBoneAnim : CAnim
{
	public override void Destroy()
	{
		m_Npc = null;
		m_Model = null;
		m_GameState = null;
	}

	public override void Initialize(GameObject model, iZombieSniperNpc npc)
	{
		m_Npc = npc;
		m_Model = model;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
	}

	public override void PlayAnim(ACTION_ENUM actionType, int nIndex = -1, float fSpeed = 0f, bool bLoop = false)
	{
		AnimInfo animInfo = m_GameState.GetAnimInfo(m_Npc.m_ZombieBaseInfo.m_nType, (int)actionType, nIndex);
		if (animInfo != null && !(m_Model.GetComponent<Animation>()[animInfo.m_sAnimName] == null))
		{
			if (fSpeed > 0f)
			{
				m_Model.GetComponent<Animation>()[animInfo.m_sAnimName].speed = fSpeed / animInfo.m_fNormalSpeed;
			}
			else
			{
				m_Model.GetComponent<Animation>()[animInfo.m_sAnimName].speed = 1f;
			}
			m_Model.GetComponent<Animation>()[animInfo.m_sAnimName].wrapMode = ((!bLoop) ? WrapMode.Once : WrapMode.Loop);
			m_Model.GetComponent<Animation>()[animInfo.m_sAnimName].time = 0f;
			m_Model.GetComponent<Animation>().CrossFade(animInfo.m_sAnimName);
		}
	}

	public override void PlayAnimRandom(ACTION_ENUM actionType, int nIndex = -1, float fSpeed = 0f, bool bLoop = false)
	{
		AnimInfo animInfo = m_GameState.GetAnimInfo(m_Npc.m_ZombieBaseInfo.m_nType, (int)actionType, nIndex);
		if (animInfo != null && !(m_Model.GetComponent<Animation>()[animInfo.m_sAnimName] == null))
		{
			if (fSpeed > 0f)
			{
				m_Model.GetComponent<Animation>()[animInfo.m_sAnimName].speed = fSpeed / animInfo.m_fNormalSpeed;
			}
			else
			{
				m_Model.GetComponent<Animation>()[animInfo.m_sAnimName].speed = 1f;
			}
			m_Model.GetComponent<Animation>()[animInfo.m_sAnimName].wrapMode = ((!bLoop) ? WrapMode.Once : WrapMode.Loop);
			m_Model.GetComponent<Animation>()[animInfo.m_sAnimName].time = UnityEngine.Random.Range(0f, m_Model.GetComponent<Animation>()[animInfo.m_sAnimName].length);
			m_Model.GetComponent<Animation>().CrossFade(animInfo.m_sAnimName);
		}
	}

	public override void CrossAnim(ACTION_ENUM actionType, int nIndex = -1, float fSpeed = 0f, bool bLoop = false)
	{
	}

	public override float GetAnimTime(ACTION_ENUM actionType, int nIndex)
	{
		AnimInfo animInfo = m_GameState.GetAnimInfo(m_Npc.m_ZombieBaseInfo.m_nType, (int)actionType, nIndex);
		if (animInfo == null)
		{
			return 0f;
		}
		if (m_Model.GetComponent<Animation>()[animInfo.m_sAnimName] == null)
		{
			return 0f;
		}
		return m_Model.GetComponent<Animation>()[animInfo.m_sAnimName].length;
	}
}
