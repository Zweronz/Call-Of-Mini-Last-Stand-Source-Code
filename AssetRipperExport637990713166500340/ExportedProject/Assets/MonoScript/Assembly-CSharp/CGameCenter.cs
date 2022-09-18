public class CGameCenter
{
	private static bool m_bActive;

	private static bool m_bLogin;

	public static void Initialize()
	{
		m_bActive = false;
		m_bLogin = false;
		GameCenterPlugin.Initialize();
		Login();
	}

	public static void Update(float deltaTime)
	{
		if (m_bActive)
		{
			GameCenterPlugin.LOGIN_STATUS lOGIN_STATUS = GameCenterPlugin.LoginStatus();
			if (lOGIN_STATUS == GameCenterPlugin.LOGIN_STATUS.LOGIN_STATUS_SUCCESS)
			{
				m_bLogin = true;
			}
			else
			{
				m_bLogin = false;
			}
		}
	}

	public static void Login()
	{
		m_bActive = true;
		GameCenterPlugin.Login();
	}

	public static bool IsLogin()
	{
		return m_bLogin;
	}

	public static void SubmitAchievement(string category, int percent = 100)
	{
		GameCenterPlugin.SubmitAchievement(category, percent);
	}

	public static void SubmitScore(string category, int score)
	{
		GameCenterPlugin.SubmitScore(category, score);
	}

	public static void OpenAchievement()
	{
		GameCenterPlugin.OpenAchievement();
	}

	public static void OpenLeaderboard()
	{
		GameCenterPlugin.OpenLeaderboard();
	}

	public static void OpenLeaderboard(string category)
	{
		GameCenterPlugin.OpenLeaderboard(category);
	}

	public static GameCenterPlugin.SUBMIT_STATUS SubmitAchievementStatus(string category, int percent = 100)
	{
		return GameCenterPlugin.SubmitAchievementStatus(category, percent);
	}

	public static GameCenterPlugin.SUBMIT_STATUS SubmitScoreStatus(string category, int score)
	{
		return GameCenterPlugin.SubmitScoreStatus(category, score);
	}
}
