using System.Collections;

public class CImageMsg
{
	private class ImageWarnInfo
	{
		public int nLevel;

		public string sImageName;
	}

	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameUI m_GameSceneUI;

	private Hashtable m_ImageList;

	private int m_nCurLevel;

	private string m_sCurName;

	public void Initialize()
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameSceneUI = m_GameScene.m_GameSceneUI;
		m_ImageList = new Hashtable();
		m_nCurLevel = -1;
		m_sCurName = string.Empty;
	}

	public void Destroy()
	{
		if (m_ImageList != null)
		{
			m_ImageList.Clear();
			m_ImageList = null;
		}
	}

	public void Update(float deltaTime)
	{
	}

	public void Play(int nID)
	{
		if (!m_ImageList.ContainsKey(nID))
		{
			return;
		}
		ImageWarnInfo imageWarnInfo = (ImageWarnInfo)m_ImageList[nID];
		if (imageWarnInfo.nLevel < m_nCurLevel)
		{
			return;
		}
		UIImage uIImage = null;
		uIImage = m_GameSceneUI.GetUIImage(m_sCurName);
		if (uIImage != null)
		{
			uIImage.Visible = false;
		}
		uIImage = m_GameSceneUI.GetUIImage(imageWarnInfo.sImageName);
		if (uIImage != null)
		{
			uIImage.Visible = true;
			uIImage.SetAlpha(1f);
			string text = imageWarnInfo.sImageName + "_fadeout";
			if (m_GameSceneUI.m_animations.ContainsKey(text))
			{
				m_GameSceneUI.StartAnimation(text);
			}
		}
		m_nCurLevel = imageWarnInfo.nLevel;
		m_sCurName = imageWarnInfo.sImageName;
	}

	public void Stop()
	{
		UIImage uIImage = m_GameSceneUI.GetUIImage(m_sCurName);
		if (uIImage != null)
		{
			uIImage.Visible = false;
		}
		m_nCurLevel = -1;
		m_sCurName = string.Empty;
	}

	public void Add(int nID, int nLevel, string sImageName)
	{
		if (!m_ImageList.ContainsKey(nID))
		{
			ImageWarnInfo imageWarnInfo = new ImageWarnInfo();
			imageWarnInfo.nLevel = nLevel;
			imageWarnInfo.sImageName = sImageName;
			m_ImageList.Add(nID, imageWarnInfo);
		}
	}
}
