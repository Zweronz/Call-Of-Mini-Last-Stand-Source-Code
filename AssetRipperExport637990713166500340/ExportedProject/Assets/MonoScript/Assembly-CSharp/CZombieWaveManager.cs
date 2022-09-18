using System.Collections;

public class CZombieWaveManager
{
	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameState m_GameState;

	private ArrayList m_ZombieWaveList;

	private ArrayList m_ZombieDestroyList;

	public void Initialize()
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		if (m_ZombieWaveList == null)
		{
			m_ZombieWaveList = new ArrayList();
		}
		if (m_ZombieDestroyList == null)
		{
			m_ZombieDestroyList = new ArrayList();
		}
		m_ZombieWaveList.Clear();
		m_ZombieDestroyList.Clear();
	}

	public void Destroy()
	{
		if (m_ZombieWaveList != null)
		{
			m_ZombieWaveList.Clear();
			m_ZombieWaveList = null;
		}
		if (m_ZombieDestroyList != null)
		{
			m_ZombieDestroyList.Clear();
			m_ZombieDestroyList = null;
		}
	}

	public void Update(float deltaTime)
	{
		foreach (ZombieWaveInfo zombieWaveInfo in m_GameScene.m_ZombieWaveCenter.m_ZombieWaveInfoList)
		{
			if (zombieWaveInfo.m_fTimePoint <= m_GameScene.m_fGameTimeTotal)
			{
				CZombieWave cZombieWave = new CZombieWave();
				if (cZombieWave.Initialize(zombieWaveInfo.m_nGroupID, zombieWaveInfo.m_nStartZone, zombieWaveInfo.m_fLoopTime))
				{
					m_ZombieWaveList.Add(cZombieWave);
				}
				m_ZombieDestroyList.Add(zombieWaveInfo);
			}
		}
		foreach (ZombieWaveInfo zombieDestroy in m_ZombieDestroyList)
		{
			m_GameScene.m_ZombieWaveCenter.m_ZombieWaveInfoList.Remove(zombieDestroy);
		}
		m_ZombieDestroyList.Clear();
		foreach (CZombieWave zombieWave in m_ZombieWaveList)
		{
			zombieWave.Update(deltaTime);
			if (zombieWave.m_bCanDestroy)
			{
				m_ZombieDestroyList.Add(zombieWave);
			}
		}
		foreach (CZombieWave zombieDestroy2 in m_ZombieDestroyList)
		{
			m_ZombieWaveList.Remove(zombieDestroy2);
		}
		m_ZombieDestroyList.Clear();
	}
}
