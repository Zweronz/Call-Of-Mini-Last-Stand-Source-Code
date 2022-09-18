using UnityEngine;

public class iZombieSniperMapUI : MonoBehaviour, TUIHandler
{
	public TUI m_TUI;

	public iZombieSniperGameState m_GameState;

	private Vector2[] m_MapInfoPosStart;

	private Vector2[] m_MapInfoPos;

	private TUIButtonClick[] m_btnUnlockStage;

	private TUIButtonClick[] m_btnLockedStage;

	private TUIButtonClick[] m_btnMapStage;

	private TUIFlex[] m_flexUnlockStage;

	private int m_nSelectStage;

	private GameObject m_dlgMapInfoLeft;

	private GameObject m_dlgMapInfoRight;

	private GameObject m_dlgMapBuy;

	private GameObject m_dlgMoneyNotEnough;

	private GameObject m_dlgStar;

	private TUIButtonClick m_Mask;

	private TUIButtonClick m_Shop;

	private GameObject m_ShopBack;

	private TUIButtonClick m_Start;

	private TUIMeshText m_txtInno;

	private TUIMeshText m_txtGold;

	private TUIMeshText m_txtCrystal;

	private int m_nBuyStage;

	private GameObject m_dlgBigMap;

	private void Start()
	{
		m_TUI = TUI.Instance("TUI");
		m_TUI.SetHandler(this);
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_btnUnlockStage = new TUIButtonClick[3];
		m_btnLockedStage = new TUIButtonClick[3];
		m_btnMapStage = new TUIButtonClick[3];
		m_flexUnlockStage = new TUIFlex[3];
		m_MapInfoPos = new Vector2[3];
		m_MapInfoPos[0] = new Vector2(-55.2f, 54.3f);
		m_MapInfoPos[1] = new Vector2(7.5f, 41f);
		m_MapInfoPos[2] = new Vector2(-9.4f, -51.6f);
		m_MapInfoPosStart = new Vector2[3];
		m_MapInfoPosStart[0] = new Vector2(-120f, 20f);
		m_MapInfoPosStart[1] = new Vector2(90f, 40f);
		m_MapInfoPosStart[2] = new Vector2(80f, -51f);
		GameObject gameObject = null;
		Transform transform = null;
		for (int i = 0; i < 3; i++)
		{
			gameObject = GameObject.Find("stage" + i + "_button");
			if (gameObject != null)
			{
				transform = gameObject.transform.Find("stage" + i + "_locked");
				if (transform != null)
				{
					m_btnLockedStage[i] = transform.GetComponent<TUIButtonClick>();
				}
				transform = gameObject.transform.Find("stage" + i + "_unlock");
				if (transform != null)
				{
					m_btnUnlockStage[i] = transform.GetComponent<TUIButtonClick>();
					m_flexUnlockStage[i] = transform.GetComponent<TUIFlex>();
				}
				transform = gameObject.transform.Find("stage" + i + "_map");
				if (transform != null)
				{
					m_btnMapStage[i] = transform.GetComponent<TUIButtonClick>();
				}
			}
		}
		m_dlgMapInfoLeft = GameObject.Find("stage_dlg_info_left");
		if (m_dlgMapInfoLeft == null)
		{
			return;
		}
		m_dlgMapInfoLeft.SetActiveRecursively(false);
		m_dlgMapInfoRight = GameObject.Find("stage_dlg_info_right");
		if (m_dlgMapInfoRight == null)
		{
			return;
		}
		m_dlgMapInfoRight.SetActiveRecursively(false);
		m_dlgMapBuy = GameObject.Find("dlgMapBuy");
		if (m_dlgMapBuy == null)
		{
			return;
		}
		m_dlgMapBuy.SetActiveRecursively(false);
		m_dlgMoneyNotEnough = GameObject.Find("dlgMapBuyNotEnough");
		if (m_dlgMoneyNotEnough == null)
		{
			return;
		}
		m_dlgMoneyNotEnough.SetActiveRecursively(false);
		m_dlgStar = GameObject.Find("dlgStar");
		if (m_dlgStar == null)
		{
			return;
		}
		m_dlgStar.SetActiveRecursively(false);
		gameObject = GameObject.Find("btnMask");
		if (gameObject == null)
		{
			return;
		}
		gameObject.SetActiveRecursively(false);
		m_Mask = gameObject.GetComponent<TUIButtonClick>();
		if (m_Mask == null)
		{
			return;
		}
		gameObject = GameObject.Find("btnShop");
		if (gameObject == null)
		{
			return;
		}
		m_Shop = gameObject.GetComponent<TUIButtonClick>();
		if (m_Shop == null)
		{
			return;
		}
		gameObject = GameObject.Find("shopbutton");
		if (gameObject == null)
		{
			return;
		}
		transform = gameObject.transform.Find("imgBack");
		if (transform == null)
		{
			return;
		}
		m_ShopBack = transform.gameObject;
		gameObject = GameObject.Find("btnStart");
		if (gameObject == null)
		{
			return;
		}
		m_Start = gameObject.GetComponent<TUIButtonClick>();
		if (m_Start == null)
		{
			return;
		}
		gameObject = GameObject.Find("txtInno");
		if (gameObject != null)
		{
			m_txtInno = gameObject.GetComponent<TUIMeshText>();
		}
		gameObject = GameObject.Find("txtGold");
		if (gameObject != null)
		{
			m_txtGold = gameObject.GetComponent<TUIMeshText>();
		}
		gameObject = GameObject.Find("txtCrystal");
		if (gameObject != null)
		{
			m_txtCrystal = gameObject.GetComponent<TUIMeshText>();
		}
		SelectMap(-1);
		RefreshStageButton();
		RefreshDataInfo();
		if (iZombieSniperGameApp.GetInstance().m_GameState.m_CurrScene == SceneEnum.kNone || iZombieSniperGameApp.GetInstance().m_GameState.m_CurrScene == SceneEnum.kGame)
		{
			iZombieSniperGameApp.GetInstance().ClearAudio("MusicMenu");
			iZombieSniperGameApp.GetInstance().PlayAudio("MusicMenu");
		}
		if (m_GameState.m_nEnterGameCount != -1)
		{
			m_GameState.m_nEnterGameCount++;
			if (m_GameState.m_nEnterGameCount >= 3)
			{
				m_GameState.m_nEnterGameCount = -1;
				ShowStarDialog();
			}
		}
		if (iZombieSniperGameApp.GetInstance().m_GameState.m_bReadyTutorial)
		{
			m_Shop.OnDisable();
			m_ShopBack.active = false;
		}
		else
		{
			m_Shop.OnEnable();
			m_ShopBack.active = true;
		}
		OpenClikPlugin.Hide();
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		TUIInput[] array = input;
		foreach (TUIInput input2 in array)
		{
			m_TUI.HandleInput(input2);
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "btnMapBuyYes")
		{
			if (eventType == 3)
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				HideStageBuy();
				BuyStage(m_nBuyStage);
				m_nBuyStage = -1;
				RefreshStageButton();
				RefreshDataInfo();
			}
		}
		else if (control.name == "btnMapBuyNo")
		{
			if (eventType == 3)
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				HideStageBuy();
				m_nBuyStage = -1;
			}
		}
		else if (control.name == "btnNotEnoughYes")
		{
			if (eventType == 3)
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kIAP);
			}
		}
		else if (control.name == "btnNotEnoughNo")
		{
			if (eventType == 3)
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				HideMoneyNotEnough();
			}
		}
		else if (control.name == "btnStarYes")
		{
			if (eventType == 3)
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
				HideStarDialog();
			}
		}
		else if (control.name == "btnStarNo")
		{
			if (eventType == 3)
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				HideStarDialog();
			}
		}
		else if (control.name == "btnStart")
		{
			if (eventType == 3)
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				m_GameState.m_nCurStage = m_nSelectStage;
				m_GameState.SaveData();
				iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kReady);
			}
		}
		else if (control.name == "btnOption")
		{
			if (eventType == 3)
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kOptions);
			}
		}
		else if (control.name == "btnShop")
		{
			if (eventType == 3)
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kShop);
			}
		}
		else if (control.name == "btnGameCenterAch")
		{
			if (eventType == 3)
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				CGameCenter.OpenAchievement();
			}
		}
		else if (control.name == "btnGameCenterLadder")
		{
			if (eventType == 3)
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				CGameCenter.OpenLeaderboard("com.trinitigame.callofminisniper.l1");
			}
		}
		else
		{
			if (eventType != 3)
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				if (control.name == "stage" + i + "_locked")
				{
					iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
					ShowStageBuy(i);
				}
				else if (control.name == "stage" + i + "_unlock")
				{
					iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
					SelectMap(i);
				}
				else if (control.name == "stage" + i + "_map_click")
				{
					iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
					if (m_GameState.GetStageState(i) == 1)
					{
						SelectMap(i);
					}
					else
					{
						ShowStageBuy(i);
					}
				}
			}
		}
	}

	private void RefreshStageButton()
	{
		for (int i = 0; i < 3; i++)
		{
			if (m_GameState.m_nStageState[i] == 0)
			{
				m_btnLockedStage[i].gameObject.active = true;
				m_btnUnlockStage[i].gameObject.active = false;
			}
			else if (m_GameState.m_nStageState[i] == 1)
			{
				m_btnLockedStage[i].gameObject.active = false;
				m_btnUnlockStage[i].gameObject.active = true;
			}
		}
	}

	private void ShowStageInfo(int nStage)
	{
		if (nStage < 0 || nStage >= 3 || m_GameState.m_nStageState[nStage] == 0)
		{
			return;
		}
		string empty = string.Empty;
		GameObject gameObject = null;
		switch (nStage)
		{
		case 0:
			gameObject = m_dlgMapInfoRight;
			empty = "popscene0";
			break;
		case 1:
			gameObject = m_dlgMapInfoLeft;
			empty = "popscene1";
			break;
		case 2:
			gameObject = m_dlgMapInfoLeft;
			empty = "popscene2";
			break;
		default:
			gameObject = m_dlgMapInfoRight;
			empty = "popscene0";
			break;
		}
		if (gameObject == null)
		{
			return;
		}
		HideStageInfo();
		if (gameObject.transform.parent == null)
		{
			gameObject.transform.position = new Vector3(m_MapInfoPos[nStage].x, m_MapInfoPos[nStage].y, gameObject.transform.position.z);
		}
		else
		{
			gameObject.transform.localPosition = new Vector3(m_MapInfoPos[nStage].x, m_MapInfoPos[nStage].y, gameObject.transform.position.z);
		}
		gameObject.SetActiveRecursively(true);
		Transform transform = null;
		TUIMeshText tUIMeshText = null;
		transform = gameObject.transform.Find("txtBestKillValue");
		if (transform != null)
		{
			tUIMeshText = transform.GetComponent<TUIMeshText>();
			if (tUIMeshText != null)
			{
				int bestKill = m_GameState.GetBestKill(nStage);
				if (bestKill == 0)
				{
					tUIMeshText.text = "-";
				}
				else
				{
					tUIMeshText.text = bestKill.ToString();
				}
			}
		}
		transform = gameObject.transform.Find("txtBestTimeValue");
		if (transform != null)
		{
			tUIMeshText = transform.GetComponent<TUIMeshText>();
			if (tUIMeshText != null)
			{
				tUIMeshText.text = Utils.TimeToString(m_GameState.GetBestTime(nStage));
			}
		}
		transform = gameObject.transform.Find("imgMapIcon");
		if (transform != null)
		{
			TUIMeshSprite component = transform.GetComponent<TUIMeshSprite>();
			if (component != null)
			{
				component.frameName = empty;
				component.UpdateMesh();
			}
		}
		TUIScaleAnim component2 = gameObject.transform.GetComponent<TUIScaleAnim>();
		if (component2 != null)
		{
			component2.Begin();
		}
		TUIMoveAnim component3 = gameObject.transform.GetComponent<TUIMoveAnim>();
		if (component3 != null)
		{
			component3.Begin(m_MapInfoPosStart[nStage], m_MapInfoPos[nStage]);
		}
	}

	private void HideStageInfo()
	{
		m_dlgMapInfoLeft.SetActiveRecursively(false);
		m_dlgMapInfoRight.SetActiveRecursively(false);
	}

	private void ShowStageBuy(int nStage)
	{
		if (nStage < 0 || nStage >= 3 || m_GameState.m_nStageState[nStage] == 1)
		{
			return;
		}
		bool bPriceGod = false;
		int nStagePrice = 0;
		if (!m_GameState.GetStagePrice(nStage, ref bPriceGod, ref nStagePrice))
		{
			return;
		}
		Transform transform = m_dlgMapBuy.transform.Find("txtBuy");
		if (transform == null)
		{
			return;
		}
		TUIMeshText component = transform.GetComponent<TUIMeshText>();
		if (component == null)
		{
			return;
		}
		if (nStage > 0)
		{
			int num = nStage - 1;
			if (num >= 0 && num < 3)
			{
				component.text = "Play " + m_GameState.m_StageName[num] + " to unlock this area!";
			}
		}
		else
		{
			component.text = "Play " + m_GameState.m_StageName[0] + " to unlock this area!";
		}
		m_dlgMapBuy.transform.position = new Vector3(0f, 0f, m_dlgMapBuy.transform.position.z);
		m_dlgMapBuy.SetActiveRecursively(true);
		ShowMask(true);
		m_nBuyStage = nStage;
	}

	private void HideStageBuy()
	{
		m_dlgMapBuy.SetActiveRecursively(false);
		ShowMask(false);
	}

	private void ShowMoneyNotEnough()
	{
		Transform transform = m_dlgMoneyNotEnough.transform.Find("txtBuy");
		if (!(transform == null))
		{
			TUIMeshText component = transform.GetComponent<TUIMeshText>();
			if (!(component == null))
			{
				component.text = "Not enough cash, do you want to\n buy more?";
				m_dlgMoneyNotEnough.transform.position = new Vector3(0f, 0f, m_dlgMapBuy.transform.position.z);
				m_dlgMoneyNotEnough.SetActiveRecursively(true);
				ShowMask(true);
			}
		}
	}

	private void HideMoneyNotEnough()
	{
		m_dlgMoneyNotEnough.SetActiveRecursively(false);
		ShowMask(false);
	}

	private void ShowMask(bool bShow)
	{
		if (!(m_Mask == null))
		{
			if (bShow)
			{
				m_Mask.transform.position = new Vector3(0f, 0f, m_Mask.transform.position.z);
				m_Mask.gameObject.SetActiveRecursively(true);
				m_Mask.disabled = false;
			}
			else
			{
				m_Mask.gameObject.SetActiveRecursively(false);
				m_Mask.disabled = true;
			}
		}
	}

	private void SelectMap(int nStage)
	{
		switch (nStage)
		{
		default:
			return;
		case 0:
		case 1:
		case 2:
			if (m_nSelectStage == nStage)
			{
				return;
			}
			m_nSelectStage = nStage;
			ShowStageInfo(nStage);
			m_Start.OnEnable();
			break;
		case -1:
			m_nSelectStage = -1;
			m_Start.OnDisable();
			break;
		}
		ShowStageMap(nStage);
		for (int i = 0; i < 3; i++)
		{
			if (i == nStage)
			{
				m_flexUnlockStage[i].Stop();
			}
			else
			{
				m_flexUnlockStage[i].Begin();
			}
		}
	}

	private void ShowStageMap(int nStage)
	{
		for (int i = 0; i < 3; i++)
		{
			if (i == nStage)
			{
				m_btnMapStage[i].gameObject.SetActiveRecursively(true);
			}
			else
			{
				m_btnMapStage[i].gameObject.SetActiveRecursively(false);
			}
		}
	}

	private void RefreshDataInfo()
	{
		m_txtInno.text = m_GameState.m_nSaveInnoTotalNum.ToString();
		m_txtGold.text = m_GameState.m_nPlayerTotalCash.ToString();
		m_txtCrystal.text = m_GameState.m_nPlayerTotalGod.ToString();
	}

	private void ShowStarDialog()
	{
		Transform transform = m_dlgStar.transform.Find("txtBuy");
		if (!(transform == null))
		{
			TUIMeshText component = transform.GetComponent<TUIMeshText>();
			if (!(component == null))
			{
				component.text = "Having fun? Rate this app!";
				m_dlgStar.transform.position = new Vector3(0f, 0f, m_dlgMapBuy.transform.position.z);
				m_dlgStar.SetActiveRecursively(true);
				ShowMask(true);
			}
		}
	}

	public void HideStarDialog()
	{
		m_dlgStar.SetActiveRecursively(false);
		ShowMask(false);
	}

	private void BuyStage(int nStage)
	{
		if (nStage < 0 && nStage >= m_GameState.m_nStageState.Length)
		{
			return;
		}
		bool bPriceGod = false;
		int nStagePrice = 0;
		if (!m_GameState.GetStagePrice(nStage, ref bPriceGod, ref nStagePrice))
		{
			return;
		}
		if (bPriceGod)
		{
			if (m_GameState.m_nPlayerTotalGod < nStagePrice)
			{
				ShowMoneyNotEnough();
				return;
			}
			m_GameState.m_nPlayerTotalGod -= nStagePrice;
		}
		else
		{
			if (m_GameState.m_nPlayerTotalCash < nStagePrice)
			{
				ShowMoneyNotEnough();
				return;
			}
			m_GameState.m_nPlayerTotalCash -= nStagePrice;
		}
		m_GameState.m_nStageState[nStage] = 1;
		m_GameState.SaveData();
	}
}
