public class CAchievement
{
	public enum AchiEnum
	{
		Achi1 = 0,
		Achi2 = 1,
		Achi3 = 2,
		Achi4 = 3,
		Achi5 = 4,
		Achi6 = 5,
		Achi7 = 6,
		Achi8 = 7,
		Achi9 = 8,
		Achi10 = 9,
		Achi11 = 10,
		Achi12 = 11,
		Achi13 = 12,
		Achi14 = 13,
		Achi15 = 14,
		Achi16 = 15,
		Achi17 = 16,
		Achi18 = 17,
		Count = 18
	}

	public enum AchiState
	{
		IDLE = 0,
		READY = 1,
		WAIT = 2
	}

	public class AchiInfo
	{
		public AchiEnum type;

		public AchiState state;
	}

	public string[] m_AchievementName = new string[36]
	{
		"Manslaughterer", "Kill 10 survivors", "Whose Side Are You On?", "Kill 100 survivors", "Terminator", "Kill 500 survivors", "Undertaker", "Kill 100 zombies", "Hand of God", "Kill 10000 zombies",
		"Evacuator", "Rescue 100 survivors", "Let My People Go!", "Rescue 1000 survivors", "I am become Death", "Use Armageddon once", "Hard Worker", "Max out 3 weapons with tCoins", "High Roller", "Max out 3 weapons with tCrystals",
		"Survivalist", "Last for 15 minutes", "Demolition Man", "Blow up all fuel tanks and trucks in one game", "Gravedigger            ", "Kill 500 zombies    ", "Vengeance is Mine      ", "Kill 1000 zombies   ", "King of Bullet Mountain", "Kill 5000 zombies   ",
		"Crossing Guard         ", "Rescue 10 survivors ", "First Responder        ", "Rescue 50 survivors ", "Folk Hero              ", "Rescue 500 survivors"
	};

	public AchiInfo[] m_arrAchiInfo;

	public void Initialize()
	{
		m_arrAchiInfo = new AchiInfo[18];
		for (int i = 0; i < m_arrAchiInfo.Length; i++)
		{
			m_arrAchiInfo[i] = new AchiInfo();
			m_arrAchiInfo[i].state = AchiState.IDLE;
		}
	}

	public void Update(float deltaTime)
	{
		if (!CGameCenter.IsLogin())
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < m_arrAchiInfo.Length; i++)
		{
			AchiInfo achiInfo = m_arrAchiInfo[i];
			if (achiInfo.state == AchiState.IDLE)
			{
				continue;
			}
			if (achiInfo.state == AchiState.READY)
			{
				Submit(achiInfo.type);
				achiInfo.state = AchiState.WAIT;
			}
			if (achiInfo.state != AchiState.WAIT)
			{
				continue;
			}
			switch (CheckState(achiInfo.type))
			{
			case GameCenterPlugin.SUBMIT_STATUS.SUBMIT_STATUS_SUCCESS:
				achiInfo.state = AchiState.IDLE;
				if (iZombieSniperGameApp.GetInstance().m_GameState != null)
				{
					iZombieSniperGameApp.GetInstance().m_GameState.SetAchievementFlag((int)achiInfo.type, 2);
					flag = true;
				}
				break;
			case GameCenterPlugin.SUBMIT_STATUS.SUBMIT_STATUS_ERROR:
				achiInfo.state = AchiState.IDLE;
				break;
			}
		}
		if (flag)
		{
			iZombieSniperGameApp.GetInstance().m_GameState.SaveData();
		}
	}

	public void CompleteAchievement(AchiEnum type)
	{
		if (type < AchiEnum.Achi1 || (int)type >= m_arrAchiInfo.Length)
		{
			return;
		}
		int achievementFlag = iZombieSniperGameApp.GetInstance().m_GameState.GetAchievementFlag((int)type);
		if (achievementFlag >= 0)
		{
			switch (achievementFlag)
			{
			case 2:
				return;
			case 0:
				iZombieSniperGameApp.GetInstance().m_GameState.SetAchievementFlag((int)type, 1);
				break;
			}
			AchiInfo achiInfo = m_arrAchiInfo[(int)type];
			achiInfo.type = type;
			if (CGameCenter.IsLogin())
			{
				Submit(type);
				achiInfo.state = AchiState.WAIT;
			}
			else
			{
				CGameCenter.Login();
				achiInfo.state = AchiState.READY;
			}
		}
	}

	public void Submit(AchiEnum type)
	{
		string iD = GetID(type);
		if (iD.Length != 0)
		{
			CGameCenter.SubmitAchievement(iD);
		}
	}

	public GameCenterPlugin.SUBMIT_STATUS CheckState(AchiEnum type)
	{
		string iD = GetID(type);
		if (iD.Length == 0)
		{
			return GameCenterPlugin.SUBMIT_STATUS.SUBMIT_STATUS_ERROR;
		}
		return CGameCenter.SubmitAchievementStatus(iD);
	}

	private string GetID(AchiEnum type)
	{
		string result = string.Empty;
		switch (type)
		{
		case AchiEnum.Achi1:
			result = "com.trinitigame.callofminisniper.a1";
			break;
		case AchiEnum.Achi2:
			result = "com.trinitigame.callofminisniper.a2";
			break;
		case AchiEnum.Achi3:
			result = "com.trinitigame.callofminisniper.a3";
			break;
		case AchiEnum.Achi4:
			result = "com.trinitigame.callofminisniper.a4";
			break;
		case AchiEnum.Achi5:
			result = "com.trinitigame.callofminisniper.a5";
			break;
		case AchiEnum.Achi6:
			result = "com.trinitigame.callofminisniper.a6";
			break;
		case AchiEnum.Achi7:
			result = "com.trinitigame.callofminisniper.a7";
			break;
		case AchiEnum.Achi8:
			result = "com.trinitigame.callofminisniper.a8";
			break;
		case AchiEnum.Achi9:
			result = "com.trinitigame.callofminisniper.a9";
			break;
		case AchiEnum.Achi10:
			result = "com.trinitigame.callofminisniper.a10";
			break;
		case AchiEnum.Achi11:
			result = "com.trinitigame.callofminisniper.a11";
			break;
		case AchiEnum.Achi12:
			result = "com.trinitigame.callofminisniper.a12";
			break;
		case AchiEnum.Achi13:
			result = "com.trinitigame.callofminisniper.a13";
			break;
		case AchiEnum.Achi14:
			result = "com.trinitigame.callofminisniper.a14";
			break;
		case AchiEnum.Achi15:
			result = "com.trinitigame.callofminisniper.a15";
			break;
		case AchiEnum.Achi16:
			result = "com.trinitigame.callofminisniper.a16";
			break;
		case AchiEnum.Achi17:
			result = "com.trinitigame.callofminisniper.a17";
			break;
		case AchiEnum.Achi18:
			result = "com.trinitigame.callofminisniper.a18";
			break;
		}
		return result;
	}
}
