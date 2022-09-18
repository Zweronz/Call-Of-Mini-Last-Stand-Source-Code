public class CRanking
{
	public enum RankState
	{
		IDLE = 0,
		READY = 1,
		WAIT = 2
	}

	public enum RankEnum
	{
		Scene0 = 0,
		Scene1 = 1,
		Scene2 = 2,
		Count = 3
	}

	public class RankInfo
	{
		public RankEnum type;

		public RankState state;

		public int score;
	}

	public RankInfo[] m_arrRankInfo;

	public void Initialize()
	{
		m_arrRankInfo = new RankInfo[3];
		for (int i = 0; i < m_arrRankInfo.Length; i++)
		{
			m_arrRankInfo[i] = new RankInfo();
			m_arrRankInfo[i].state = RankState.IDLE;
		}
	}

	public void Update(float deltaTime)
	{
		if (!CGameCenter.IsLogin())
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < m_arrRankInfo.Length; i++)
		{
			RankInfo rankInfo = m_arrRankInfo[i];
			if (rankInfo.state == RankState.IDLE)
			{
				continue;
			}
			if (rankInfo.state == RankState.READY)
			{
				Submit(rankInfo.type, rankInfo.score);
				rankInfo.state = RankState.WAIT;
			}
			if (rankInfo.state != RankState.WAIT)
			{
				continue;
			}
			switch (CheckState(rankInfo.type, rankInfo.score))
			{
			case GameCenterPlugin.SUBMIT_STATUS.SUBMIT_STATUS_SUCCESS:
				rankInfo.state = RankState.IDLE;
				if (iZombieSniperGameApp.GetInstance().m_GameState != null)
				{
					iZombieSniperGameApp.GetInstance().m_GameState.SetBestKillSubmitFlag((int)rankInfo.type, false);
					flag = true;
				}
				break;
			case GameCenterPlugin.SUBMIT_STATUS.SUBMIT_STATUS_ERROR:
				rankInfo.state = RankState.IDLE;
				break;
			}
		}
		if (flag)
		{
			iZombieSniperGameApp.GetInstance().m_GameState.SaveData();
		}
	}

	public void CompleteRanking(RankEnum type, int nScore)
	{
		if (type < RankEnum.Scene0 || (int)type >= m_arrRankInfo.Length)
		{
			return;
		}
		RankInfo rankInfo = m_arrRankInfo[(int)type];
		if (nScore > rankInfo.score)
		{
			rankInfo.type = type;
			rankInfo.score = nScore;
			if (CGameCenter.IsLogin())
			{
				Submit(type, nScore);
				rankInfo.state = RankState.WAIT;
			}
			else
			{
				CGameCenter.Login();
				rankInfo.state = RankState.READY;
			}
		}
	}

	public void Submit(RankEnum type, int nScore)
	{
		switch (type)
		{
		case RankEnum.Scene0:
			CGameCenter.SubmitScore("com.trinitigame.callofminisniper.l1", nScore);
			break;
		case RankEnum.Scene1:
			CGameCenter.SubmitScore("com.trinitigame.callofminisniper.l2", nScore);
			break;
		case RankEnum.Scene2:
			CGameCenter.SubmitScore("com.trinitigame.callofminisniper.l3", nScore);
			break;
		}
	}

	public GameCenterPlugin.SUBMIT_STATUS CheckState(RankEnum type, int nScore)
	{
		switch (type)
		{
		case RankEnum.Scene0:
			return CGameCenter.SubmitScoreStatus("com.trinitigame.callofminisniper.l1", nScore);
		case RankEnum.Scene1:
			return CGameCenter.SubmitScoreStatus("com.trinitigame.callofminisniper.l2", nScore);
		case RankEnum.Scene2:
			return CGameCenter.SubmitScoreStatus("com.trinitigame.callofminisniper.l3", nScore);
		default:
			return GameCenterPlugin.SUBMIT_STATUS.SUBMIT_STATUS_ERROR;
		}
	}
}
