using UnityEngine;

public class iZombieSniperMenuUI : MonoBehaviour, TUIHandler
{
	public TUI m_TUI;

	private void Start()
	{
		Application.targetFrameRate = 120;
		m_TUI = TUI.Instance("TUI");
		m_TUI.SetHandler(this);
		iZombieSniperGameApp.GetInstance().PlayAudio("MusicMenu");
		OpenClikPlugin.Hide();
		OpenClikPlugin.Show(true);
		XAdManagerWrapper.ShowImageAd();
		Screen.orientation = ScreenOrientation.AutoRotation;
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
		if (Input.GetKeyDown("s") && !Application.isMobilePlatform)
		{
			iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kMap);
		}
		TUIInput[] input = TUIInputManager.GetInput();
		TUIInput[] array = input;
		for (int i = 0; i < array.Length; i++)
		{
			TUIInput tUIInput = array[i];
			if (tUIInput.inputType != TUIInputType.Ended)
			{
				continue;
			}
			iZombieSniperGameState gameState = iZombieSniperGameApp.GetInstance().m_GameState;
			if (gameState != null)
			{
				if (gameState.m_bReadyTutorial)
				{
					gameState.m_nCurStage = 0;
					gameState.m_bPlayedScene2 = true;
					gameState.SaveData();
					iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kReady);
				}
				else if (!gameState.m_bPlayedScene2)
				{
					gameState.m_nCurStage = 2;
					gameState.m_bPlayedScene2 = true;
					gameState.SaveData();
					iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kGame);
				}
				else
				{
					iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kMap);
				}
			}
			else
			{
				iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kMap);
			}
			break;
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
	}
}
