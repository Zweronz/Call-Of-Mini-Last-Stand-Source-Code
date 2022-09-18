using UnityEngine;

public class iZombieSniperCartoonUI : MonoBehaviour, TUIHandler
{
	public TUI m_TUI;

	private iZombieSniperGameState m_GameState;

	private bool m_bVideo;

	private bool m_bEnter;

	private float m_fTimeCount;

	private void Start()
	{
		switch (Input.deviceOrientation)
		{
		case DeviceOrientation.Portrait:
		case DeviceOrientation.PortraitUpsideDown:
		case DeviceOrientation.FaceUp:
		case DeviceOrientation.FaceDown:
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			break;
		}
		m_TUI = TUI.Instance("TUI");
		m_TUI.SetHandler(this);
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_bVideo = false;
		m_bEnter = false;
		m_fTimeCount = 0f;
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
		if (m_bEnter)
		{
			return;
		}
		m_fTimeCount += Time.deltaTime;
		if (!m_bVideo)
		{
			if (m_fTimeCount >= 0.1f)
			{
				XAdManagerWrapper.SetVideoFile("XAdVideo.mp4");
				XAdManagerWrapper.SetVideoAdUrl("http://itunes.apple.com/us/app/isniper-3d-arctic-warfare/id533741523?ls=1&mt=8");
				XAdManagerWrapper.ShowVideoAdLocal();
				m_bVideo = true;
			}
		}
		else if (m_fTimeCount >= 0.5f)
		{
			iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kMenu);
			m_bEnter = true;
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
	}
}
