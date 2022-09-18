using UnityEngine;

public abstract class CAnim
{
	public iZombieSniperNpc m_Npc;

	public GameObject m_Model;

	public iZombieSniperGameState m_GameState;

	public abstract void Destroy();

	public abstract void Initialize(GameObject model, iZombieSniperNpc npc);

	public abstract void PlayAnim(ACTION_ENUM actionType, int nIndex = -1, float fSpeed = 0f, bool bLoop = false);

	public abstract void PlayAnimRandom(ACTION_ENUM actionType, int nIndex = -1, float fSpeed = 0f, bool bLoop = false);

	public abstract void CrossAnim(ACTION_ENUM actionType, int nIndex = -1, float fSpeed = 0f, bool bLoop = false);

	public virtual float GetAnimTime(ACTION_ENUM actionType, int nIndex)
	{
		return 0f;
	}

	public virtual Vector3 GetCenter()
	{
		return Vector3.zero;
	}
}
