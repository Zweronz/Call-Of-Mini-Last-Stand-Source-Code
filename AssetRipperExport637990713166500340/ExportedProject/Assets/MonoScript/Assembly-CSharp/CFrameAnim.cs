using UnityEngine;

public class CFrameAnim : CAnim
{
	public TrinitiModelAnimation m_ModelAnimation;

	public override void Destroy()
	{
		m_Npc = null;
		m_Model = null;
		m_GameState = null;
		m_ModelAnimation = null;
	}

	public override void Initialize(GameObject model, iZombieSniperNpc npc)
	{
		m_Npc = npc;
		m_Model = model;
		m_ModelAnimation = m_Model.GetComponentInChildren<TrinitiModelAnimation>();
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
	}

	public override void PlayAnim(ACTION_ENUM actionType, int nIndex = -1, float fSpeed = 0f, bool bLoop = false)
	{
		AnimInfo animInfo = m_GameState.GetAnimInfo(m_Npc.m_ZombieBaseInfo.m_nType, (int)actionType, nIndex);
		if (animInfo != null)
		{
			if (fSpeed > 0f)
			{
				m_ModelAnimation.Play(animInfo.m_sAnimName, (!bLoop) ? WrapMode.Once : WrapMode.Loop, fSpeed / animInfo.m_fNormalSpeed);
			}
			else
			{
				m_ModelAnimation.Play(animInfo.m_sAnimName, (!bLoop) ? WrapMode.Once : WrapMode.Loop);
			}
		}
	}

	public override void PlayAnimRandom(ACTION_ENUM actionType, int nIndex = -1, float fSpeed = 0f, bool bLoop = false)
	{
		AnimInfo animInfo = m_GameState.GetAnimInfo(m_Npc.m_ZombieBaseInfo.m_nType, (int)actionType, nIndex);
		if (animInfo != null)
		{
			if (fSpeed > 0f)
			{
				m_ModelAnimation.Play(animInfo.m_sAnimName, (!bLoop) ? WrapMode.Once : WrapMode.Loop, fSpeed / animInfo.m_fNormalSpeed, true);
			}
			else
			{
				m_ModelAnimation.Play(animInfo.m_sAnimName, (!bLoop) ? WrapMode.Once : WrapMode.Loop, 1f, true);
			}
		}
	}

	public override void CrossAnim(ACTION_ENUM actionType, int nIndex = -1, float fSpeed = 0f, bool bLoop = false)
	{
	}

	public override Vector3 GetCenter()
	{
		if (m_ModelAnimation != null)
		{
			return m_ModelAnimation.m_v3Center;
		}
		return base.GetCenter();
	}
}
