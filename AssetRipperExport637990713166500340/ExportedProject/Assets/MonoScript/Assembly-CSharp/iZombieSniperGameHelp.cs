using UnityEngine;

public class iZombieSniperGameHelp
{
	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameState m_GameState;

	public iZombieSniperGameUI m_GameSceneUI;

	public GameHelpState m_HelpState;

	public bool m_bWaitConfirm;

	public iZombieSniperNpc m_Target;

	public bool m_bTrace;

	private float m_fTimeCount;

	public void Initialize()
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_GameSceneUI = m_GameScene.m_GameSceneUI;
		m_HelpState = GameHelpState.None;
		m_bWaitConfirm = false;
		m_bTrace = false;
	}

	public void Destroy()
	{
		m_Target = null;
		m_HelpState = GameHelpState.None;
		m_bWaitConfirm = false;
		m_bTrace = false;
		m_GameScene.StopAudio("MusicIntro01");
		m_GameScene.StopAudio("MusicIntro02");
		m_GameScene.StopAudio("UICue");
	}

	public bool IsInTutorial()
	{
		return m_HelpState != GameHelpState.None;
	}

	public bool IsWaitConfirm()
	{
		return m_bWaitConfirm;
	}

	public void Update(float deltaTime)
	{
		if (m_bTrace && m_Target != null && !m_Target.IsDead())
		{
			Vector3 vector = m_GameScene.m_CameraScript.GetComponent<Camera>().WorldToScreenPoint(m_Target.m_ModelTransForm.position);
			if (Utils.IsPad())
			{
				m_GameSceneUI.SetGameHelpIconPos(new Vector2(vector.x, vector.y) + new Vector2(5f, -25f) * m_GameState.m_nHDFactor);
			}
			else
			{
				m_GameSceneUI.SetGameHelpIconPos(new Vector2(vector.x, vector.y) + new Vector2(10f, -10f) * m_GameState.m_nHDFactor);
			}
		}
		if (m_HelpState == GameHelpState.Step3)
		{
			if (m_Target != null && m_Target.IsDead())
			{
				FinishHelpState(GameHelpState.Step3);
				int nUID = m_GameScene.AddNPC(103, new Vector3(-48f, 0f, -11f), -1);
				m_Target = m_GameScene.GetNPC(nUID);
				m_Target.SetStateDirectly(m_Target.stWaitState);
				m_Target.stMoveState.Initialize(m_Target, new Vector3(-49f, 0f, -21f), null, true);
				m_Target.SetState(m_Target.stMoveState);
			}
		}
		else if (m_HelpState == GameHelpState.Step7)
		{
			if (m_Target != null && m_Target.IsDead())
			{
				FinishHelpState(GameHelpState.Step7);
			}
		}
		else if (m_HelpState == GameHelpState.Step8)
		{
			m_fTimeCount += deltaTime;
			if (m_fTimeCount > 2f)
			{
				FinishHelpState(GameHelpState.Step8);
			}
			else if (m_fTimeCount > 1f)
			{
				m_GameSceneUI.StartAnimation("helpDescFadeOut");
			}
		}
		else if (m_HelpState == GameHelpState.Step9)
		{
			m_fTimeCount += deltaTime;
			if (m_fTimeCount > 3f)
			{
				FinishHelpState(GameHelpState.Step9);
			}
			else if (m_fTimeCount > 2f)
			{
				m_GameSceneUI.StartAnimation("helpDescFadeOut");
			}
		}
		else if (m_HelpState == GameHelpState.Step10)
		{
			m_fTimeCount += deltaTime;
			if (m_fTimeCount > 3f)
			{
				FinishHelpState(GameHelpState.Step10);
			}
			else if (m_fTimeCount > 2f)
			{
				m_GameSceneUI.StartAnimation("helpDescFadeOut");
			}
		}
	}

	public void NextHelpState()
	{
		m_bWaitConfirm = false;
		m_HelpState++;
		if (m_HelpState == GameHelpState.End)
		{
			m_HelpState = GameHelpState.None;
			m_GameSceneUI.RemoveGameHelpUI();
			m_GameScene.m_bTutorial = false;
			m_GameScene.Destroy();
			m_GameScene.Initialize();
			m_GameScene.ResetData();
			m_GameScene.StartGame();
		}
		else
		{
			EnterHelpState(m_HelpState);
		}
	}

	public void EnterHelpState(GameHelpState helpState)
	{
		m_HelpState = helpState;
		m_fTimeCount = 0f;
		m_bTrace = false;
		m_GameSceneUI.RemoveGameHelpUI();
		m_GameScene.StopAudio("UICue");
		switch (helpState)
		{
		case GameHelpState.Step1:
			m_Target = m_GameScene.GetNPC(m_GameScene.GetTurtorialNPC());
			m_bTrace = true;
			m_GameSceneUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, "Tap on a target to zoom.", new Vector4(240f, 240f, 240f, 40f) * m_GameState.m_nHDFactor);
			m_GameScene.PlayAudio("UICue");
			break;
		case GameHelpState.Step2:
			m_GameSceneUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, "Slide your finger to adjust the crosshairs.", new Vector4(240f, 240f, 240f, 80f) * m_GameState.m_nHDFactor);
			break;
		case GameHelpState.Step3:
			m_GameSceneUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, "Fire!", new Vector4(240f, 260f, 200f, 40f) * m_GameState.m_nHDFactor);
			m_GameSceneUI.SetGameHelpIconPos(new Vector2(440f, 30f) * m_GameState.m_nHDFactor);
			m_GameScene.PlayAudio("UICue");
			break;
		case GameHelpState.Step4:
			m_GameSceneUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, "Tap anywhere to exit zoom.", new Vector4(240f, 240f, 240f, 80f) * m_GameState.m_nHDFactor);
			m_GameSceneUI.SetGameHelpIconPos(new Vector2(400f, 150f) * m_GameState.m_nHDFactor);
			m_GameScene.PlayAudio("UICue");
			break;
		case GameHelpState.Step5:
			m_GameSceneUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, "Tap your weapon in the upper right to switch weapons.", new Vector4(240f, 240f, 240f, 80f) * m_GameState.m_nHDFactor);
			m_GameSceneUI.SetGameHelpIconPos(new Vector2(430f, 290f) * m_GameState.m_nHDFactor);
			m_GameScene.PlayAudio("UICue");
			break;
		case GameHelpState.Step6:
			m_GameSceneUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, "When not zoomed, slide your finger to look around. Practice aiming without your scope.", new Vector4(240f, 240f, 240f, 100f) * m_GameState.m_nHDFactor);
			break;
		case GameHelpState.Step7:
			m_fTimeCount = 0f;
			m_GameSceneUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, "You can hold down the fire button when using rifles. Mow 'em down!", new Vector4(240f, 220f, 240f, 80f) * m_GameState.m_nHDFactor);
			m_GameSceneUI.SetGameHelpIconPos(new Vector2(440f, 30f) * m_GameState.m_nHDFactor);
			m_GameScene.PlayAudio("UICue");
			break;
		case GameHelpState.Step8:
			m_fTimeCount = 0f;
			m_GameSceneUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, "Congrats!", new Vector4(240f, 240f, 200f, 40f) * m_GameState.m_nHDFactor);
			m_bWaitConfirm = true;
			break;
		case GameHelpState.Step9:
			m_fTimeCount = 0f;
			m_GameSceneUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, "See a fuel truck? Shoot it to bail yourself out of a tough spot!", new Vector4(240f, 240f, 260f, 100f) * m_GameState.m_nHDFactor);
			m_bWaitConfirm = true;
			break;
		case GameHelpState.Step10:
			m_fTimeCount = 0f;
			m_GameSceneUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, "If a single zombie gets into the safehouse, it's game over!", new Vector4(240f, 240f, 260f, 100f) * m_GameState.m_nHDFactor);
			m_bWaitConfirm = true;
			break;
		}
	}

	public void FinishHelpState(GameHelpState helpState)
	{
		if (m_HelpState == helpState)
		{
			NextHelpState();
		}
	}

	public bool IsCanFire()
	{
		if (m_HelpState == GameHelpState.Step3 || m_HelpState == GameHelpState.Step7)
		{
			return true;
		}
		return false;
	}

	public bool IsCanSwitchWeapon()
	{
		if (m_HelpState == GameHelpState.Step5)
		{
			return true;
		}
		return false;
	}

	public bool IsCanAim()
	{
		if (m_HelpState == GameHelpState.Step1)
		{
			return true;
		}
		return false;
	}

	public bool IsCanCloseAim()
	{
		if (m_HelpState == GameHelpState.Step4)
		{
			return true;
		}
		return false;
	}

	public bool IsCanSlipScreen()
	{
		if (m_HelpState == GameHelpState.Step2 || m_HelpState == GameHelpState.Step3 || m_HelpState == GameHelpState.Step6 || m_HelpState == GameHelpState.Step7)
		{
			return true;
		}
		return false;
	}
}
