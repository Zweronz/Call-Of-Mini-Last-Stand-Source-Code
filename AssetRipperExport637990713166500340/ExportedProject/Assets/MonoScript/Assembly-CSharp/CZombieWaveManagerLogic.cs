using System.Collections.Generic;
using UnityEngine;

public class CZombieWaveManagerLogic
{
	public class ZombieWave
	{
		public float m_fTime;

		public int m_nWaveInfoID;
	}

	public class ZombieCreate
	{
		public float m_fTime;

		public int m_nZombieID;

		public Vector3 m_v3Pos;

		public int m_nStartZone;
	}

	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameState m_GameState;

	public iZombieSniperZombieWaveCenter m_ZombieWaveCenter;

	public iZombieSniperWayPointCenter m_WayPointCenter;

	private List<ZombieWave> m_ZombieWaveList;

	private List<ZombieWave> m_ZombieDestroyList;

	private List<ZombieCreate> m_ZombieCreateList;

	private List<ZombieCreate> m_ZombieCreateDestroyList;

	public void Initialize()
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_ZombieWaveCenter = iZombieSniperGameApp.GetInstance().m_ZombieWaveCenter;
		m_WayPointCenter = iZombieSniperGameApp.GetInstance().m_WayPointCenter;
		if (m_ZombieWaveList == null)
		{
			m_ZombieWaveList = new List<ZombieWave>();
		}
		if (m_ZombieDestroyList == null)
		{
			m_ZombieDestroyList = new List<ZombieWave>();
		}
		m_ZombieWaveList.Clear();
		m_ZombieDestroyList.Clear();
		if (m_ZombieCreateList == null)
		{
			m_ZombieCreateList = new List<ZombieCreate>();
		}
		if (m_ZombieCreateDestroyList == null)
		{
			m_ZombieCreateDestroyList = new List<ZombieCreate>();
		}
		m_ZombieCreateList.Clear();
		m_ZombieCreateDestroyList.Clear();
		foreach (LogicWaveInfo value in m_ZombieWaveCenter.m_dictLogicWaveInfo.Values)
		{
			float startTime = value.GetStartTime();
			ZombieWave zombieWave = new ZombieWave();
			zombieWave.m_nWaveInfoID = value.m_nID;
			zombieWave.m_fTime = startTime;
			m_ZombieWaveList.Add(zombieWave);
		}
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
		if (m_ZombieCreateList != null)
		{
			m_ZombieCreateList.Clear();
			m_ZombieCreateList = null;
		}
		if (m_ZombieCreateDestroyList != null)
		{
			m_ZombieCreateDestroyList.Clear();
			m_ZombieCreateDestroyList = null;
		}
	}

	public void Update(float deltaTime)
	{
		foreach (ZombieWave zombieWave in m_ZombieWaveList)
		{
			if (zombieWave.m_fTime > m_GameScene.m_fGameTimeTotal)
			{
				continue;
			}
			LogicWaveInfo logicWaveInfo = m_ZombieWaveCenter.GetLogicWaveInfo(zombieWave.m_nWaveInfoID);
			if (logicWaveInfo == null)
			{
				m_ZombieDestroyList.Add(zombieWave);
				continue;
			}
			if (m_ZombieWaveCenter.GetZombieBaseInfo(logicWaveInfo.m_nZombieID) == null)
			{
				m_ZombieDestroyList.Add(zombieWave);
				continue;
			}
			int count = m_ZombieWaveCenter.CaculateZombieNum(logicWaveInfo.m_nFormulaID, m_GameScene.m_fGameTimeTotal);
			Create(logicWaveInfo.m_nZombieID, count, logicWaveInfo.m_nStartPoint, 0.2f);
			float loopTime = logicWaveInfo.GetLoopTime();
			if (loopTime > 0f)
			{
				zombieWave.m_fTime = m_GameScene.m_fGameTimeTotal + loopTime;
				continue;
			}
			m_ZombieDestroyList.Add(zombieWave);
			Debug.Log("refresh zombie id = " + logicWaveInfo.m_nID + " finished");
		}
		foreach (ZombieWave zombieDestroy in m_ZombieDestroyList)
		{
			m_ZombieWaveList.Remove(zombieDestroy);
		}
		m_ZombieDestroyList.Clear();
		foreach (ZombieCreate zombieCreate in m_ZombieCreateList)
		{
			zombieCreate.m_fTime -= deltaTime;
			if (zombieCreate.m_fTime <= 0f)
			{
				m_GameScene.AddNPC(zombieCreate.m_nZombieID, zombieCreate.m_v3Pos, zombieCreate.m_nStartZone);
				m_ZombieCreateDestroyList.Add(zombieCreate);
			}
		}
		foreach (ZombieCreate zombieCreateDestroy in m_ZombieCreateDestroyList)
		{
			m_ZombieCreateList.Remove(zombieCreateDestroy);
		}
		m_ZombieCreateDestroyList.Clear();
	}

	private void Create(int zombieid, int count, int startzone, float dt)
	{
		for (int i = 0; i < count; i++)
		{
			int num = startzone;
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
			if (m_WayPointCenter.GetStartZoneRandomPoint(num, ref v3Pos))
			{
				if (i == 0)
				{
					m_GameScene.AddNPC(zombieid, v3Pos, num);
					continue;
				}
				ZombieCreate zombieCreate = new ZombieCreate();
				zombieCreate.m_nZombieID = zombieid;
				zombieCreate.m_v3Pos = v3Pos;
				zombieCreate.m_nStartZone = num;
				zombieCreate.m_fTime = dt * (float)i;
				m_ZombieCreateList.Add(zombieCreate);
			}
		}
	}
}
