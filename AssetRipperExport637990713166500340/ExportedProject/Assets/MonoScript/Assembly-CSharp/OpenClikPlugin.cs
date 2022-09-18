public class OpenClikPlugin
{
	private enum Status
	{
		kShowFull = 0,
		kShowTip = 1,
		kHide = 2
	}

	private static Status s_Status;

	public static bool m_bActive;

	public static void Initialize(string key)
	{
		s_Status = Status.kHide;
	}

	public static void Show(bool show_full)
	{
		if (m_bActive && show_full)
		{
			ChartBoostAndroid.showInterstitial(null);
		}
	}

	public static void Hide()
	{
		if (m_bActive)
		{
		}
	}

	public static void AudriodInit(string id, string signature)
	{
		if (m_bActive)
		{
			ChartBoostAndroid.init(id, signature);
			ChartBoostAndroid.onStart();
		}
	}

	public static void setTestDevices(string[] testDevices)
	{
		if (m_bActive)
		{
		}
	}
}
