using System;
using UnityEngine;

public class iZombieSniperDataCollect : MonoBehaviour
{
	public CDataCollectManager m_dcManager;

	public iZombieSniperGameState m_GameState;

	private float m_fSendTime;

	private void Start()
	{
		m_dcManager = iZombieSniperGameApp.GetInstance().m_DataCollect;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_fSendTime = 120f;
		SendDailyDataBefore();
	}

	private void Update()
	{
		m_GameState.m_fTimeInApp += Time.deltaTime;
		if (iZombieSniperGameApp.GetInstance().m_GameScene != null)
		{
			m_GameState.m_fTimeInGameScene += Time.deltaTime;
		}
		if (m_fSendTime > 0f)
		{
			m_fSendTime -= Time.deltaTime;
			if (m_fSendTime <= 0f)
			{
				m_fSendTime = 120f;
				SendDailyDataBefore();
			}
		}
	}

	private void OnApplicationPause(bool bPause)
	{
		if (bPause)
		{
			UpdateDataCollect();
			return;
		}
		string text = DateTime.Now.ToString("yyyy_MM_dd");
		if (text != m_dcManager.m_sTodayDate)
		{
			m_dcManager.CreateToday(text);
			SendDailyDataBefore();
		}
		m_dcManager.AddGameLogin();
		if (m_GameState.m_nEnterGameCount != -1)
		{
			m_GameState.m_nEnterGameCount++;
		}
	}

	private void UpdateDataCollect()
	{
		if (m_dcManager != null)
		{
			m_dcManager.AddGameTime(m_GameState.m_fTimeInApp);
			m_GameState.m_fTimeInApp = 0f;
			if (m_GameState.m_fTimeInGameScene > 0f)
			{
				m_dcManager.AddGameBreak(m_GameState.m_fTimeInGameScene);
				m_GameState.m_fTimeInGameScene = 0f;
			}
			m_dcManager.UpdateTotalDataToToday(m_dcManager.m_sTodayDate);
			m_dcManager.SaveDailyData(m_dcManager.m_sTodayDate);
			m_dcManager.SaveData();
		}
	}

	private void SendData()
	{
		if (Application.internetReachability != 0)
		{
			m_dcManager.SendData();
		}
	}

	private void SendDailyData()
	{
		if (Application.internetReachability != 0)
		{
			m_dcManager.SendDailyData(m_dcManager.m_sTodayDate);
		}
	}

	private void SendDailyDataBefore()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			return;
		}
		foreach (string key in m_dcManager.m_dictCollect.Keys)
		{
			if (!(key == m_dcManager.m_sTodayDate))
			{
				m_dcManager.SendDailyData(key);
			}
		}
	}
}
