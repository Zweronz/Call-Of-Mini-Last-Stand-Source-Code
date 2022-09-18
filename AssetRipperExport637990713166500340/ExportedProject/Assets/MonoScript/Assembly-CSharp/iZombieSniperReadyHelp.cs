using UnityEngine;

public class iZombieSniperReadyHelp
{
	public enum GameReadyState
	{
		None = 0,
		Step1 = 1,
		Step2 = 2,
		Step3 = 3,
		Step4 = 4,
		End = 5
	}

	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameState m_GameState;

	public iZombieSniperReadyUI m_GameReadyUI;

	public GameReadyState m_State;

	public void Initialize(iZombieSniperReadyUI helpui)
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_GameReadyUI = helpui;
		m_State = GameReadyState.None;
	}

	public void Destroy()
	{
	}

	public void Update(float deltaTime)
	{
		if (m_State == GameReadyState.Step1)
		{
			if (m_GameState.GetCarryWeapon(0) == 1)
			{
				FinishHelpState(GameReadyState.Step1);
			}
		}
		else if (m_State == GameReadyState.Step2)
		{
			if (m_GameReadyUI.m_Stage == GameShopStage.AUTOSHOOT)
			{
				FinishHelpState(GameReadyState.Step2);
			}
		}
		else if (m_State == GameReadyState.Step3 && m_GameState.GetCarryWeapon(1) == 11)
		{
			FinishHelpState(GameReadyState.Step3);
		}
	}

	public void NextHelpState()
	{
		m_State++;
		if (m_State == GameReadyState.End)
		{
			m_State = GameReadyState.None;
		}
		else
		{
			EnterHelpState(m_State);
		}
	}

	public void EnterHelpState(GameReadyState state)
	{
		m_State = state;
		m_GameReadyUI.RemoveGameHelpUI();
		iZombieSniperGameApp.GetInstance().StopAudio("UICue");
		switch (state)
		{
		case GameReadyState.Step1:
			m_GameReadyUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, string.Empty, new Vector4(240f, 260f, 200f, 40f) * m_GameState.m_nHDFactor);
			m_GameReadyUI.SetGameHelpIconMove(Utils.CalcScale(new Vector2(120f, 200f)), Utils.CalcScale(new Vector2(205f, 74f)));
			break;
		case GameReadyState.Step2:
			m_GameReadyUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, string.Empty, new Vector4(240f, 260f, 200f, 40f) * m_GameState.m_nHDFactor);
			m_GameReadyUI.SetGameHelpIconPos(new Vector2(130f, 262f) * m_GameState.m_nHDFactor);
			iZombieSniperGameApp.GetInstance().PlayAudio("UICue");
			break;
		case GameReadyState.Step3:
			m_GameReadyUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, string.Empty, new Vector4(240f, 260f, 200f, 40f) * m_GameState.m_nHDFactor);
			m_GameReadyUI.SetGameHelpIconMove(Utils.CalcScale(new Vector2(120f, 200f)), Utils.CalcScale(new Vector2(320f, 74f)));
			break;
		case GameReadyState.Step4:
			m_GameReadyUI.SetGameHelpUI("hand", new Vector4(0f, 0f, 48f, 43f) * m_GameState.m_nHDFactor, string.Empty, new Vector4(240f, 260f, 200f, 40f) * m_GameState.m_nHDFactor);
			m_GameReadyUI.SetGameHelpIconPos(new Vector2(370f, 22f) * m_GameState.m_nHDFactor);
			iZombieSniperGameApp.GetInstance().PlayAudio("UICue");
			break;
		}
	}

	public void FinishHelpState(GameReadyState state)
	{
		if (m_State == state)
		{
			NextHelpState();
		}
	}

	public bool IsCanDrag(int nWeaponID)
	{
		if (m_State == GameReadyState.Step1 && nWeaponID == 1)
		{
			return true;
		}
		if (m_State == GameReadyState.Step3 && nWeaponID == 11)
		{
			return true;
		}
		return false;
	}

	public bool IsCanDragEnd(int nWeaponID, int nIndex)
	{
		if (m_State == GameReadyState.Step1 && nWeaponID == 1 && nIndex == 0)
		{
			return true;
		}
		if (m_State == GameReadyState.Step3 && nWeaponID == 11 && nIndex == 1)
		{
			return true;
		}
		return false;
	}

	public bool IsCanSwitchLabel(GameShopStage stage)
	{
		if (m_State == GameReadyState.Step2 && stage == GameShopStage.AUTOSHOOT)
		{
			return true;
		}
		return false;
	}

	public bool IsCanSlip()
	{
		return false;
	}

	public bool IsCanStartGame()
	{
		if (m_State == GameReadyState.Step4)
		{
			return true;
		}
		return false;
	}
}
