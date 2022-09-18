using UnityEngine;

public class CZombieWave
{
	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameState m_GameState;

	public bool m_bCanDestroy;

	private float m_fTimeCount;

	private int m_nStartZone;

	private bool m_bLoop;

	private float m_fLoopTime;

	private ZombieGroupInfo m_ZombieGroupInfo;

	private ZombieGroupUnit m_ZombieGroupUnit;

	private int m_nGroupIndex;

	private float m_fTimeDelay;

	public bool Initialize(int nGroupID, int nStartZone, float loopTime)
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_ZombieGroupInfo = m_GameScene.m_ZombieWaveCenter.GetZombieGroupInfo(nGroupID);
		if (m_ZombieGroupInfo == null)
		{
			return false;
		}
		if (m_ZombieGroupInfo.m_GroupUnitList.Count <= 0)
		{
			return false;
		}
		m_nStartZone = nStartZone;
		m_nGroupIndex = 0;
		m_ZombieGroupUnit = (ZombieGroupUnit)m_ZombieGroupInfo.m_GroupUnitList[m_nGroupIndex];
		m_fTimeDelay = m_ZombieGroupUnit.m_fTimeDelay;
		if (loopTime > 0f)
		{
			m_bLoop = true;
			m_fLoopTime = loopTime;
		}
		return true;
	}

	public void Update(float deltaTime)
	{
		if (m_bCanDestroy)
		{
			return;
		}
		m_fTimeCount += deltaTime;
		if (!(m_fTimeCount >= m_fTimeDelay) || m_GameScene.IsMaxNPC())
		{
			return;
		}
		m_fTimeCount = 0f;
		int num = m_nStartZone;
		if (num == 0)
		{
			int num2 = 0;
			int num3 = 0;
			switch (m_GameState.m_nCurStage)
			{
			case 0:
				num2 = 1;
				num3 = ((!m_GameScene.IsLidAnim()) ? 5 : 6);
				num = UnityEngine.Random.Range(num2, num3);
				break;
			case 1:
				num2 = 1;
				num3 = 10;
				if (m_GameScene.IsUnderWayAnim())
				{
					num3 = 11;
				}
				if (m_GameScene.IsSuperMarketAnim())
				{
					num3 = 12;
				}
				num = UnityEngine.Random.Range(num2, num3);
				if (num == 10 && !m_GameScene.IsUnderWayAnim())
				{
					num = 9;
				}
				break;
			case 2:
				num2 = 1;
				num3 = 9;
				if (m_GameScene.IsWaWaBegun())
				{
					num3 = 10;
				}
				num = UnityEngine.Random.Range(num2, num3);
				break;
			}
		}
		Vector3 v3Pos = Vector3.zero;
		if (m_GameScene.m_WayPointCenter.GetStartZoneRandomPoint(num, ref v3Pos))
		{
			m_GameScene.AddNPC(m_ZombieGroupUnit.m_nZombieID, v3Pos, num);
		}
		m_nGroupIndex++;
		if (m_nGroupIndex < m_ZombieGroupInfo.m_GroupUnitList.Count)
		{
			m_ZombieGroupUnit = (ZombieGroupUnit)m_ZombieGroupInfo.m_GroupUnitList[m_nGroupIndex];
			m_fTimeDelay = m_ZombieGroupUnit.m_fTimeDelay;
		}
		else if (m_bLoop)
		{
			m_nGroupIndex = 0;
			m_ZombieGroupUnit = (ZombieGroupUnit)m_ZombieGroupInfo.m_GroupUnitList[m_nGroupIndex];
			m_fTimeDelay = m_fLoopTime;
		}
		else
		{
			m_bCanDestroy = true;
		}
	}
}
