using UnityEngine;

public class TUIInputManager
{
	private static int m_lastFrameCount = -1;

	public static TUIInput[] GetInput()
	{
		if (Time.frameCount != m_lastFrameCount)
		{
			if (Application.isMobilePlatform)
			{
			TUIInputManageriOS.UpdateInput();
			}
			else
			{
				TUIInputManagerWindows.UpdateInput();
			}
		}
		m_lastFrameCount = Time.frameCount;
			if (Application.isMobilePlatform)
			{
			return TUIInputManageriOS.GetInput();
			}
			else
			{
				return TUIInputManagerWindows.GetInput();
			}
	}
}
