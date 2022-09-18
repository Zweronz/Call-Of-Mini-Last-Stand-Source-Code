using UnityEngine;

public class iZombieSniperOptionUI : UIHelper
{
	public iZombieSniperGameState m_GameState;

	private new void Start()
	{
		m_font_path = "ZombieSniper/Fonts/Materials/";
		m_ui_material_path = "ZombieSniper/UI/Materials/";
		m_ui_cfgxml = "ZombieSniper/UI/OptionUI";
		base.Start();
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		SetMusic(m_GameState.m_bMusicOn);
		SetSound(m_GameState.m_bSoundOn);
		SetTurtorial(m_GameState.m_bTutorial);
		SetCutScenes(m_GameState.m_bCutScenes);
	}

	private new void Update()
	{
		base.Update();
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (m_UIManagerRef.HandleInput(touch))
			{
			}
		}
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (command != 0)
		{
			return;
		}
		if (control.Id == GetControlId("btnPauseBack"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			m_GameState.SaveData();
			iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kMap);
		}
		else if (control.Id == GetControlId("btnOptionHelp"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kHelp);
		}
		else if (control.Id == GetControlId("btnOptionReview"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			if (iZombieSniperGameApp.GetInstance().m_IAPMode == iZombieSniperGameApp.kIAPMode.Amazon)
			{
				Application.OpenURL("http://www.amazon.com/gp/mas/dl/android?p=com.trinitigame.callofminisniper");
			}
			else
			{
				Application.OpenURL("market://details?id=com.trinitigame.callofminisniper");
			}
		}
		else if (control.Id == GetControlId("btnOptionSupport"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			Application.OpenURL("http://www.trinitigame.com/support?game=comls&version=1.2.2");
		}
		else if (control.Id == GetControlId("btnOptionCredit"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kCredits);
		}
		else if (control.Id == GetControlId("btnPauseSoundON"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			m_GameState.m_bSoundOn = true;
			m_GameState.SaveData();
			SetSound(m_GameState.m_bSoundOn);
			TAudioManager.instance.isSoundOn = true;
		}
		else if (control.Id == GetControlId("btnPauseSoundOFF"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			m_GameState.m_bSoundOn = false;
			m_GameState.SaveData();
			SetSound(m_GameState.m_bSoundOn);
			TAudioManager.instance.isSoundOn = false;
		}
		else if (control.Id == GetControlId("btnPauseMusicON"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			m_GameState.m_bMusicOn = true;
			m_GameState.SaveData();
			SetMusic(m_GameState.m_bMusicOn);
			TAudioManager.instance.isMusicOn = true;
		}
		else if (control.Id == GetControlId("btnPauseMusicOFF"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			m_GameState.m_bMusicOn = false;
			m_GameState.SaveData();
			SetMusic(m_GameState.m_bMusicOn);
			TAudioManager.instance.isMusicOn = false;
		}
		else if (control.Id == GetControlId("btnPauseTutorialON"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			m_GameState.m_bTutorial = true;
			m_GameState.ResetCGFlag();
			m_GameState.SaveData();
			SetTurtorial(m_GameState.m_bTutorial);
		}
		else if (control.Id == GetControlId("btnPauseTutorialOFF"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			m_GameState.m_bTutorial = false;
			m_GameState.SaveData();
			SetTurtorial(m_GameState.m_bTutorial);
		}
	}

	public void SetMusic(bool bOn)
	{
		((UIClickButton)m_control_table["btnPauseMusicON"]).Enable = !bOn;
		((UIClickButton)m_control_table["btnPauseMusicOFF"]).Enable = bOn;
	}

	public void SetSound(bool bOn)
	{
		((UIClickButton)m_control_table["btnPauseSoundON"]).Enable = !bOn;
		((UIClickButton)m_control_table["btnPauseSoundOFF"]).Enable = bOn;
	}

	public void SetTurtorial(bool bOn)
	{
		((UIClickButton)m_control_table["btnPauseTutorialON"]).Enable = !bOn;
		((UIClickButton)m_control_table["btnPauseTutorialOFF"]).Enable = bOn;
	}

	public void SetCutScenes(bool bOn)
	{
		((UIClickButton)m_control_table["btnPauseCutscenesON"]).Enable = !bOn;
		((UIClickButton)m_control_table["btnPauseCutscenesOFF"]).Enable = bOn;
	}
}
