using UnityEngine;

public class iZombieSniperReadyUI : UIHelper
{
	public iZombieSniperGameState m_GameState;

	public iZombieSniperGunCenter m_GunCenter;

	public iZombieSniperIconCenter m_IconCenter;

	public CShopCell m_ShopCell;

	public CShopPower m_ShopPower;

	public GameShopStage m_Stage;

	public GameShopState m_State;

	public bool m_bDragMouse;

	public Vector2 m_v2TouchPos;

	public iZombieSniperReadyHelp m_GameReadyHelp;

	public CImageAnim m_ImageAnim;

	public CImageAnim m_FingerClick;

	public CImageAnim m_ArrowLeft;

	public CImageAnim m_ArrowRight;

	private Vector2 m_v2DragOffset;

	public bool m_bFromItem;

	public int m_nDragWeapon = -1;

	public int m_nDragWeaponReady = -1;

	public new void Start()
	{
		m_font_path = "ZombieSniper/Fonts/Materials/";
		m_ui_material_path = "ZombieSniper/UI/Materials/";
		m_ui_cfgxml = "ZombieSniper/UI/ReadyUI";
		base.Start();
		Initialize();
	}

	public new void Update()
	{
		base.Update();
		if (m_GameReadyHelp != null)
		{
			m_GameReadyHelp.Update(Time.deltaTime);
		}
		if (m_ImageAnim != null)
		{
			m_ImageAnim.Update(Time.deltaTime);
		}
		if (m_FingerClick != null)
		{
			m_FingerClick.Update(Time.deltaTime);
		}
		if (m_Stage != GameShopStage.DEFENCE)
		{
			m_ShopCell.Update(Time.deltaTime);
			if (m_ShopCell.IsLeftMore())
			{
				m_ArrowLeft.PlayAnim(0.1f, true);
			}
			else
			{
				m_ArrowLeft.StopAnim();
			}
			if (m_ShopCell.IsRightMore())
			{
				m_ArrowRight.PlayAnim(0.1f, true);
			}
			else
			{
				m_ArrowRight.StopAnim();
			}
		}
		else
		{
			m_ShopPower.Update(Time.deltaTime);
			if (m_ShopPower.IsLeftMore())
			{
				m_ArrowLeft.PlayAnim(0.1f, true);
			}
			else
			{
				m_ArrowLeft.StopAnim();
			}
			if (m_ShopPower.IsRightMore())
			{
				m_ArrowRight.PlayAnim(0.1f, true);
			}
			else
			{
				m_ArrowRight.StopAnim();
			}
		}
		if (m_ArrowLeft != null)
		{
			m_ArrowLeft.Update(Time.deltaTime);
		}
		if (m_ArrowRight != null)
		{
			m_ArrowRight.Update(Time.deltaTime);
		}
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		for (int i = 0; i < array.Length; i++)
		{
			UITouchInner touch = array[i];
			m_UIManagerRef.HandleInput(touch);
			if (m_State == GameShopState.Dialog)
			{
				break;
			}
			if (touch.phase == TouchPhase.Began)
			{
				m_v2TouchPos = touch.position;
				m_bDragMouse = false;
				Rect rect = Utils.CalcScaleRect(new Rect(0f, 155f, 480f, 90f));
				if (!Utils.PtInRect(m_v2TouchPos, rect))
				{
					break;
				}
				if (m_Stage != GameShopStage.DEFENCE)
				{
					if (m_ShopCell.m_bAnim)
					{
						m_ShopCell.m_bAnim = false;
					}
				}
				else if (m_ShopPower.m_bAnim)
				{
					m_ShopPower.m_bAnim = false;
				}
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				if (!m_bDragMouse && Vector2.Distance(m_v2TouchPos, touch.position) > 12f)
				{
					m_bDragMouse = true;
				}
				if (!m_bDragMouse)
				{
					break;
				}
				if (m_nDragWeapon != -1)
				{
					DragIng(touch.position);
					break;
				}
				bool flag = true;
				Rect rect2 = Utils.CalcScaleRect(new Rect(0f, 155f, 480f, 90f));
				if (Utils.PtInRect(m_v2TouchPos, rect2) && m_GameReadyHelp == null && Mathf.Abs(touch.deltaPosition.x) - Mathf.Abs(touch.deltaPosition.y) > 0f)
				{
					flag = false;
					if (m_Stage != GameShopStage.DEFENCE)
					{
						float num = Mathf.Abs(touch.deltaPosition.x);
						m_ShopCell.Move(num * 5f, num * 5f, (touch.deltaPosition.x > 0f) ? 1 : (-1));
					}
					else
					{
						float num2 = Mathf.Abs(touch.deltaPosition.x);
						m_ShopPower.Move(num2 * 5f, num2 * 5f, (touch.deltaPosition.x > 0f) ? 1 : (-1));
					}
				}
				if (flag && m_nDragWeaponReady != -1)
				{
					DragBegin(touch.position);
				}
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				DragEnd(touch.position);
				m_bDragMouse = false;
			}
		}
	}

	public void Initialize()
	{
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_GunCenter = iZombieSniperGameApp.GetInstance().m_GunCenter;
		m_IconCenter = iZombieSniperGameApp.GetInstance().m_IconCenter;
		m_ShopCell = new CShopCell();
		m_ShopCell.Initialize(this, m_UIManagerRef, new Vector2(100f, (float)Screen.height / 3.2f), true);
		m_ShopCell.SetLayer(2);
		m_ShopPower = new CShopPower();
		m_ShopPower.Initialize(this, m_UIManagerRef, new Vector2(100f, (float)Screen.height / 3.2f), true);
		m_ShopPower.SetLayer(2);
		m_ShopPower.PrepareCell(4);
		m_ArrowLeft = new CImageAnim();
		m_ArrowLeft.Initialize(GetUIImage("imgArrowLeft"), 3);
		m_ArrowLeft.Add(LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1")), new Rect(484 * m_GameState.m_nHDFactor, 22 * m_GameState.m_nHDFactor, 24 * m_GameState.m_nHDFactor, 8 * m_GameState.m_nHDFactor));
		m_ArrowLeft.Add(LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1")), new Rect(484 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor, 24 * m_GameState.m_nHDFactor, 8 * m_GameState.m_nHDFactor));
		m_ArrowLeft.Add(LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1")), new Rect(484 * m_GameState.m_nHDFactor, 2 * m_GameState.m_nHDFactor, 24 * m_GameState.m_nHDFactor, 8 * m_GameState.m_nHDFactor));
		m_ArrowRight = new CImageAnim();
		m_ArrowRight.Initialize(GetUIImage("imgArrowRight"), 3);
		m_ArrowRight.Add(LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1")), new Rect(484 * m_GameState.m_nHDFactor, 52 * m_GameState.m_nHDFactor, 24 * m_GameState.m_nHDFactor, 8 * m_GameState.m_nHDFactor));
		m_ArrowRight.Add(LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1")), new Rect(484 * m_GameState.m_nHDFactor, 42 * m_GameState.m_nHDFactor, 24 * m_GameState.m_nHDFactor, 8 * m_GameState.m_nHDFactor));
		m_ArrowRight.Add(LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1")), new Rect(484 * m_GameState.m_nHDFactor, 32 * m_GameState.m_nHDFactor, 24 * m_GameState.m_nHDFactor, 8 * m_GameState.m_nHDFactor));
		PowerUpInfo powerUpInfo = new PowerUpInfo();
		if (m_GameState.m_nTowerLvl > 0)
		{
			powerUpInfo.m_nType = PowerUpEnum.Telephone_Booth_Dmg;
			powerUpInfo.m_sName = "Battle Booth";
			powerUpInfo.m_sMaterial = "shop_ui_part1";
			powerUpInfo.m_Rect = new Rect(392f, 404f, 71f, 51f);
			powerUpInfo.m_nReserves = m_GameState.m_nTowerLvl;
			powerUpInfo.m_bGodPrice = false;
			powerUpInfo.m_nValue = 1;
			powerUpInfo.m_nReservesMax = 5;
			powerUpInfo.m_sDesc = "Has 100 free bullets per stage. +20% attack per upgrade.";
			m_ShopPower.SetData(powerUpInfo);
		}
		if (m_GameState.m_nTowerLvl > 0 && m_GameState.m_nTowerBullet > 10)
		{
			powerUpInfo.m_nType = PowerUpEnum.Telephone_Booth_Bullet;
			powerUpInfo.m_sName = "Ammo";
			powerUpInfo.m_sMaterial = "shop_ui_part1";
			powerUpInfo.m_Rect = new Rect(464f, 380f, 44f, 51f);
			powerUpInfo.m_nReserves = m_GameState.m_nTowerBullet;
			powerUpInfo.m_bGodPrice = false;
			powerUpInfo.m_nValue = 100;
			powerUpInfo.m_nReservesMax = 0;
			powerUpInfo.m_sDesc = "Add extra ammo to the battle booth (no max limit). The battle booth will be overrun if it runs out of ammo.";
			m_ShopPower.SetData(powerUpInfo);
		}
		if (m_GameState.m_nMineCount > 0)
		{
			powerUpInfo.m_nType = PowerUpEnum.LandMine;
			powerUpInfo.m_sName = "Explosives";
			powerUpInfo.m_sMaterial = "shop_ui_part1";
			powerUpInfo.m_Rect = new Rect(258f, 448f, 57f, 54f);
			powerUpInfo.m_nReserves = m_GameState.m_nMineCount;
			powerUpInfo.m_bGodPrice = false;
			powerUpInfo.m_nValue = 1;
			powerUpInfo.m_nReservesMax = 0;
			powerUpInfo.m_sDesc = "Sets a tripwire to blow explosives hidden near the safehouse entrance once per game. ";
			m_ShopPower.SetData(powerUpInfo);
		}
		if (m_GameState.m_nAC130 > 0)
		{
			powerUpInfo.m_nType = PowerUpEnum.AirStrike;
			powerUpInfo.m_sName = "Armageddon";
			powerUpInfo.m_sMaterial = "shop_ui_part1";
			powerUpInfo.m_Rect = new Rect(444f, 324f, 60f, 54f);
			powerUpInfo.m_nReserves = m_GameState.m_nAC130;
			powerUpInfo.m_bGodPrice = true;
			powerUpInfo.m_nValue = 1;
			powerUpInfo.m_nReservesMax = 0;
			powerUpInfo.m_sDesc = "Blasts everything that moves to pieces with an aerial bombardment once per game.";
			m_ShopPower.SetData(powerUpInfo);
		}
		if (m_GameState.m_nInnoKiller > 0 && m_GameState.m_nCurStage != 2)
		{
			powerUpInfo.m_nType = PowerUpEnum.InnoKiller;
			powerUpInfo.m_sName = "Pardon";
			powerUpInfo.m_sMaterial = "shop_ui_part3";
			powerUpInfo.m_Rect = new Rect(365f, 321f, 63f, 51f);
			powerUpInfo.m_nReserves = m_GameState.m_nInnoKiller;
			powerUpInfo.m_bGodPrice = false;
			powerUpInfo.m_nValue = 1;
			powerUpInfo.m_nReservesMax = 0;
			powerUpInfo.m_sDesc = "Grants forgiveness for 3 extra survivor kills, allowing up to 6 survivor kills before losing.";
			m_ShopPower.SetData(powerUpInfo);
		}
		if (m_GameState.m_nMachineGun > 0 && m_GameState.m_nCurStage == 2)
		{
			powerUpInfo.m_nType = PowerUpEnum.MachineGun;
			powerUpInfo.m_sName = "The Equalizer";
			powerUpInfo.m_sMaterial = "shop_ui_part3";
			powerUpInfo.m_Rect = new Rect(365f, 373f, 109f, 59f);
			powerUpInfo.m_nReserves = m_GameState.m_nMachineGun;
			powerUpInfo.m_bGodPrice = true;
			powerUpInfo.m_nValue = 1;
			powerUpInfo.m_nReservesMax = 0;
			powerUpInfo.m_sDesc = "Kill anything in a single shot! Single use item. Once active, has 100 shots before expiring. Only usable in The Fearground.";
			powerUpInfo.m_bNew = true;
			m_ShopPower.SetData(powerUpInfo);
		}
		((UIText)m_control_table["txtInnocents"]).SetText(m_GameState.m_nSaveInnoTotalNum.ToString());
		SetPlayerGold(m_GameState.m_nPlayerTotalCash);
		SetPlayerGodGold(m_GameState.m_nPlayerTotalGod);
		ShowWeaponList(GameShopStage.SNIPER);
		for (int i = 0; i < 3; i++)
		{
			SetCarryWeapon(i, m_GameState.GetCarryWeapon(i));
		}
		if (m_GameState.m_bReadyTutorial)
		{
			((UIClickButton)m_control_table["btnStartGame"]).Enable = m_GameState.IsCarryTwo();
		}
		else
		{
			((UIClickButton)m_control_table["btnStartGame"]).Enable = m_GameState.IsCarry();
		}
		m_State = GameShopState.Normal;
		m_ImageAnim = new CImageAnim();
		m_ImageAnim.Initialize(GetUIImage("imgHelpClick"), 10);
		m_ImageAnim.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(202 * m_GameState.m_nHDFactor, 438 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
		m_ImageAnim.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(152 * m_GameState.m_nHDFactor, 422 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
		m_ImageAnim.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(102 * m_GameState.m_nHDFactor, 422 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
		m_ImageAnim.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(52 * m_GameState.m_nHDFactor, 414 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
		m_ImageAnim.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(2 * m_GameState.m_nHDFactor, 414 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
		m_ImageAnim.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(438 * m_GameState.m_nHDFactor, 388 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
		m_ImageAnim.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(388 * m_GameState.m_nHDFactor, 388 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
		m_ImageAnim.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(338 * m_GameState.m_nHDFactor, 388 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
		m_ImageAnim.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(288 * m_GameState.m_nHDFactor, 388 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
		m_ImageAnim.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(238 * m_GameState.m_nHDFactor, 388 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
		m_FingerClick = new CImageAnim();
		m_FingerClick.Initialize(GetUIImage("imgHelpIcon"), 2);
		m_FingerClick.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(302 * m_GameState.m_nHDFactor, 438 * m_GameState.m_nHDFactor, 48 * m_GameState.m_nHDFactor, 43 * m_GameState.m_nHDFactor));
		m_FingerClick.Add(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3")), new Rect(252 * m_GameState.m_nHDFactor, 438 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 43 * m_GameState.m_nHDFactor));
		if (m_GameState.m_bReadyTutorial)
		{
			m_GameReadyHelp = new iZombieSniperReadyHelp();
			m_GameReadyHelp.Initialize(this);
			m_GameReadyHelp.EnterHelpState(iZombieSniperReadyHelp.GameReadyState.Step1);
			((UIClickButton)m_control_table["btnBack"]).Visible = false;
			((UIClickButton)m_control_table["btnBack"]).Enable = false;
		}
		else
		{
			((UIClickButton)m_control_table["btnBack"]).Visible = true;
			((UIClickButton)m_control_table["btnBack"]).Enable = true;
		}
		if (!Utils.IsPad())
		{
			m_v2DragOffset = new Vector2(-15f, 25f);
		}
		else
		{
			m_v2DragOffset = new Vector2(-5f, 8f);
		}
		OpenClikPlugin.Hide();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		switch (command)
		{
		case 0:
			if (control.Id == GetControlId("btnBack"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				if (m_GameReadyHelp == null && m_State == GameShopState.Normal)
				{
					m_GameState.SaveData();
					m_GunCenter.SaveWeaponData();
					iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kMap);
				}
			}
			else if (control.Id == GetControlId("btnStartGame"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				if ((m_GameReadyHelp == null || m_GameReadyHelp.IsCanStartGame()) && m_GameState.IsCarry())
				{
					if (m_GameReadyHelp != null)
					{
						m_GameReadyHelp.Destroy();
						m_GameReadyHelp = null;
						m_GameState.m_bReadyTutorial = false;
					}
					m_GameState.SaveData();
					m_GunCenter.SaveWeaponData();
					iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kGame);
				}
			}
			else if (control.Id == GetControlId("btnWeapon1on"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickLabel");
				if ((m_GameReadyHelp == null || m_GameReadyHelp.IsCanSwitchLabel(GameShopStage.SNIPER)) && m_State == GameShopState.Normal)
				{
					ShowWeaponList(GameShopStage.SNIPER);
				}
			}
			else if (control.Id == GetControlId("btnWeapon2on"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickLabel");
				if ((m_GameReadyHelp == null || m_GameReadyHelp.IsCanSwitchLabel(GameShopStage.AUTOSHOOT)) && m_State == GameShopState.Normal)
				{
					ShowWeaponList(GameShopStage.AUTOSHOOT);
				}
			}
			else if (control.Id == GetControlId("btnWeapon3on"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickLabel");
				if ((m_GameReadyHelp == null || m_GameReadyHelp.IsCanSwitchLabel(GameShopStage.BAZOOKA)) && m_State == GameShopState.Normal)
				{
					ShowWeaponList(GameShopStage.BAZOOKA);
				}
			}
			else if (control.Id == GetControlId("btnWeapon4on"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickLabel");
				if ((m_GameReadyHelp == null || m_GameReadyHelp.IsCanSwitchLabel(GameShopStage.DEFENCE)) && m_State == GameShopState.Normal)
				{
					ShowWeaponList(GameShopStage.DEFENCE);
				}
			}
			break;
		case 1:
		{
			if (control.Id == GetControlId("btnCarry1"))
			{
				if (m_GameReadyHelp == null)
				{
					m_bFromItem = false;
					m_nDragWeaponReady = m_GameState.GetCarryWeapon(0);
				}
				break;
			}
			if (control.Id == GetControlId("btnCarry2"))
			{
				if (m_GameReadyHelp == null)
				{
					m_bFromItem = false;
					m_nDragWeaponReady = m_GameState.GetCarryWeapon(1);
				}
				break;
			}
			if (control.Id == GetControlId("btnCarry3"))
			{
				if (m_GameReadyHelp == null)
				{
					m_bFromItem = false;
					m_nDragWeaponReady = m_GameState.GetCarryWeapon(2);
				}
				break;
			}
			CShopCell.CellInfo shopCellInfo = m_ShopCell.GetShopCellInfo(control.Id);
			if (shopCellInfo != null)
			{
				m_bFromItem = true;
				m_nDragWeaponReady = control.Id;
			}
			break;
		}
		}
	}

	public void ShowWeaponList(GameShopStage stage)
	{
		m_Stage = stage;
		m_nDragWeaponReady = -1;
		m_nDragWeapon = -1;
		bool flag = stage != GameShopStage.SNIPER;
		((UIClickButton)m_control_table["btnWeapon1on"]).Visible = flag;
		((UIClickButton)m_control_table["btnWeapon1on"]).Enable = flag;
		((UIClickButton)m_control_table["btnWeapon1off"]).Visible = !flag;
		((UIClickButton)m_control_table["btnWeapon1off"]).Enable = !flag;
		flag = stage != GameShopStage.AUTOSHOOT;
		((UIClickButton)m_control_table["btnWeapon2on"]).Visible = flag;
		((UIClickButton)m_control_table["btnWeapon2on"]).Enable = flag;
		((UIClickButton)m_control_table["btnWeapon2off"]).Visible = !flag;
		((UIClickButton)m_control_table["btnWeapon2off"]).Enable = !flag;
		flag = stage != GameShopStage.BAZOOKA;
		((UIClickButton)m_control_table["btnWeapon3on"]).Visible = flag;
		((UIClickButton)m_control_table["btnWeapon3on"]).Enable = flag;
		((UIClickButton)m_control_table["btnWeapon3off"]).Visible = !flag;
		((UIClickButton)m_control_table["btnWeapon3off"]).Enable = !flag;
		flag = stage != GameShopStage.DEFENCE;
		((UIClickButton)m_control_table["btnWeapon4on"]).Visible = flag;
		((UIClickButton)m_control_table["btnWeapon4on"]).Enable = flag;
		((UIClickButton)m_control_table["btnWeapon4off"]).Visible = !flag;
		((UIClickButton)m_control_table["btnWeapon4off"]).Enable = !flag;
		m_ShopCell.Show(false);
		m_ShopPower.Show(false);
		if (stage != GameShopStage.DEFENCE)
		{
			m_ShopCell.Reset();
			foreach (iWeaponInfoBase value in m_GunCenter.m_WeaponInfoBase.Values)
			{
				if (m_GunCenter.GetWeaponData(value.m_nWeaponID) != null)
				{
					if (value.IsRifle() && stage == GameShopStage.SNIPER)
					{
						m_ShopCell.AddWeapon(value.m_nWeaponID);
					}
					else if (value.IsAutoShoot() && stage == GameShopStage.AUTOSHOOT)
					{
						m_ShopCell.AddWeapon(value.m_nWeaponID);
					}
					else if ((value.IsRocket() || value.IsThrowMine()) && stage == GameShopStage.BAZOOKA)
					{
						m_ShopCell.AddWeapon(value.m_nWeaponID);
					}
				}
			}
			m_ShopCell.UpdateShop();
		}
		else
		{
			m_ShopPower.Reset();
			m_ShopPower.UpdateShop();
		}
	}

	public void SetPlayerGold(int nGold)
	{
		((UIText)m_control_table["txtGold"]).SetText(nGold.ToString());
	}

	public void SetPlayerGodGold(int nGold)
	{
		((UIText)m_control_table["txtDollar"]).SetText(nGold.ToString());
	}

	public void DragBegin(Vector2 v2MousePos)
	{
		if (m_nDragWeapon == m_nDragWeaponReady || m_nDragWeaponReady == -1)
		{
			return;
		}
		iWeaponInfoBase iWeaponInfoBase2 = null;
		if (m_bFromItem)
		{
			CShopCell.CellInfo shopCellInfo = m_ShopCell.GetShopCellInfo(m_nDragWeaponReady);
			if (shopCellInfo != null)
			{
				iWeaponInfoBase2 = shopCellInfo.m_WeaponInfo;
			}
		}
		else if (m_GameState.GetCarryIndexByID(m_nDragWeaponReady) != -1)
		{
			iWeaponInfoBase2 = m_GunCenter.GetWeaponInfoBase(m_nDragWeaponReady);
		}
		if (iWeaponInfoBase2 == null || (m_GameReadyHelp != null && !m_GameReadyHelp.IsCanDrag(iWeaponInfoBase2.m_nWeaponID)))
		{
			return;
		}
		IconInfo icon = m_IconCenter.GetIcon(iWeaponInfoBase2.m_nIconID);
		if (icon != null)
		{
			Material material = LoadUIMaterial(Utils.AutoMaterialName(icon.m_sMaterial));
			if (!(material == null))
			{
				((UIImage)m_control_table["imgDragIcon"]).Visible = true;
				((UIImage)m_control_table["imgDragIcon"]).SetTexture(material, new Rect(icon.m_Rect.xMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.yMax * (float)m_GameState.m_nHDFactor, icon.m_Rect.width * (float)m_GameState.m_nHDFactor, icon.m_Rect.height * (float)m_GameState.m_nHDFactor));
				v2MousePos += m_v2DragOffset * m_GameState.m_nHDFactor;
				((UIImage)m_control_table["imgDragIcon"]).SetPosition(v2MousePos);
				((UIImage)m_control_table["imgDragBack"]).SetPosition(v2MousePos);
				m_nDragWeapon = m_nDragWeaponReady;
				m_nDragWeaponReady = -1;
				iZombieSniperGameApp.GetInstance().PlayAudio("WeaponPickUp");
			}
		}
	}

	public void DragIng(Vector2 v2MousePos)
	{
		if (m_nDragWeapon != -1)
		{
			v2MousePos += m_v2DragOffset * m_GameState.m_nHDFactor;
			((UIImage)m_control_table["imgDragIcon"]).SetPosition(v2MousePos);
			((UIImage)m_control_table["imgDragBack"]).SetPosition(v2MousePos);
		}
	}

	public void DragEnd(Vector2 v2MousePos)
	{
		if (m_nDragWeapon == -1)
		{
			return;
		}
		if (m_bFromItem)
		{
			CShopCell.CellInfo shopCellInfo = m_ShopCell.GetShopCellInfo(m_nDragWeapon);
			if (shopCellInfo != null)
			{
				int num = -1;
				for (int i = 0; i < 3; i++)
				{
					if (shopCellInfo.m_WeaponInfo.m_nWeaponID == m_GameState.GetCarryWeapon(i))
					{
						num = i;
						break;
					}
				}
				for (int j = 0; j < 3; j++)
				{
					if (CheckInRect(j, ((UIImage)m_control_table["imgDragIcon"]).Rect) && (m_GameReadyHelp == null || m_GameReadyHelp.IsCanDragEnd(shopCellInfo.m_WeaponInfo.m_nWeaponID, j)))
					{
						if (num != -1)
						{
							ExchangeWeapon(j, num);
						}
						else
						{
							EquipWeapon(j, shopCellInfo.m_WeaponInfo.m_nWeaponID);
						}
						break;
					}
				}
			}
		}
		else if (m_GameReadyHelp == null)
		{
			int carryIndexByID = m_GameState.GetCarryIndexByID(m_nDragWeapon);
			if (carryIndexByID != -1)
			{
				bool flag = false;
				for (int k = 0; k < 3; k++)
				{
					if (CheckInRect(k, ((UIImage)m_control_table["imgDragIcon"]).Rect))
					{
						flag = true;
						if (k != carryIndexByID)
						{
							ExchangeWeapon(k, carryIndexByID);
						}
					}
				}
				if (!flag)
				{
					EquipWeapon(carryIndexByID, -1);
				}
			}
		}
		m_nDragWeapon = -1;
		((UIImage)m_control_table["imgDragIcon"]).Visible = false;
		((UIImage)m_control_table["imgDragBack"]).Visible = false;
		if (m_GameState.m_bReadyTutorial)
		{
			((UIClickButton)m_control_table["btnStartGame"]).Enable = m_GameState.IsCarryTwo();
		}
		else
		{
			((UIClickButton)m_control_table["btnStartGame"]).Enable = m_GameState.IsCarry();
		}
	}

	public bool CheckInRect(int nIndex, Rect rect)
	{
		nIndex++;
		string key = "btnCarry" + nIndex;
		if (!m_control_table.ContainsKey(key))
		{
			return false;
		}
		return Utils.CollideWithRect(((UIClickButton)m_control_table[key]).Rect, rect);
	}

	public void SetCarryWeapon(int nIndex, int nWeaponID)
	{
		nIndex++;
		string key = "btnCarry" + nIndex;
		if (!m_control_table.ContainsKey(key))
		{
			return;
		}
		iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(nWeaponID);
		if (weaponInfoBase == null)
		{
			((UIClickButton)m_control_table[key]).Visible = false;
			((UIClickButton)m_control_table[key]).Enable = false;
			return;
		}
		IconInfo icon = m_IconCenter.GetIcon(weaponInfoBase.m_nIconID);
		if (icon != null)
		{
			float num = 1.6666666f;
			Vector2 vector = ((!(icon.m_Rect.width / icon.m_Rect.height >= num)) ? new Vector2(icon.m_Rect.width * (48f / icon.m_Rect.height), 48f) : new Vector2(80f, icon.m_Rect.height * (80f / icon.m_Rect.width)));
			vector.x = Mathf.Floor(vector.x);
			vector.y = Mathf.Floor(vector.y);
			Material material = LoadUIMaterial(Utils.AutoMaterialName(icon.m_sMaterial));
			if (material != null)
			{
				((UIClickButton)m_control_table[key]).SetTexture(UIButtonBase.State.Normal, material, new Rect(icon.m_Rect.xMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.yMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.width * (float)m_GameState.m_nHDFactor, icon.m_Rect.height * (float)m_GameState.m_nHDFactor), vector * m_GameState.m_nHDFactor);
				((UIClickButton)m_control_table[key]).SetTexture(UIButtonBase.State.Pressed, material, new Rect(icon.m_Rect.xMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.yMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.width * (float)m_GameState.m_nHDFactor, icon.m_Rect.height * (float)m_GameState.m_nHDFactor), vector * m_GameState.m_nHDFactor);
				((UIClickButton)m_control_table[key]).SetTexture(UIButtonBase.State.Disabled, material, new Rect(icon.m_Rect.xMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.yMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.width * (float)m_GameState.m_nHDFactor, icon.m_Rect.height * (float)m_GameState.m_nHDFactor), vector * m_GameState.m_nHDFactor);
				((UIClickButton)m_control_table[key]).Visible = true;
				((UIClickButton)m_control_table[key]).Enable = true;
			}
		}
	}

	public void ExchangeWeapon(int nIndex1, int nIndex2)
	{
		m_GameState.CarryWeaponExchange(nIndex1, nIndex2);
		SetCarryWeapon(nIndex1, m_GameState.GetCarryWeapon(nIndex1));
		SetCarryWeapon(nIndex2, m_GameState.GetCarryWeapon(nIndex2));
		iZombieSniperGameApp.GetInstance().PlayAudio("UITakeWeapon");
	}

	public void EquipWeapon(int nIndex, int nWeaponID)
	{
		if (nWeaponID <= 0)
		{
			m_GameState.CarryWeapon(nIndex, 0);
			SetCarryWeapon(nIndex, -1);
			if (m_Stage != GameShopStage.DEFENCE)
			{
				CShopCell.CellInfo shopCellInfoByWeaponID = m_ShopCell.GetShopCellInfoByWeaponID(m_nDragWeapon);
				if (shopCellInfoByWeaponID != null && shopCellInfoByWeaponID.m_bUsed)
				{
					m_ShopCell.UpdateCell(shopCellInfoByWeaponID.m_nID, m_nDragWeapon);
				}
			}
		}
		else
		{
			int carryWeapon = m_GameState.GetCarryWeapon(nIndex);
			m_GameState.CarryWeapon(nIndex, nWeaponID);
			SetCarryWeapon(nIndex, nWeaponID);
			if (carryWeapon != -1)
			{
				CShopCell.CellInfo shopCellInfoByWeaponID2 = m_ShopCell.GetShopCellInfoByWeaponID(carryWeapon);
				if (shopCellInfoByWeaponID2 != null && shopCellInfoByWeaponID2.m_bUsed)
				{
					m_ShopCell.UpdateCell(shopCellInfoByWeaponID2.m_nID, carryWeapon);
				}
			}
			m_ShopCell.UpdateCell(m_nDragWeapon, nWeaponID);
		}
		iZombieSniperGameApp.GetInstance().PlayAudio("UITakeWeapon");
	}

	public void SetGameHelpUI(string sMaterialName, Vector4 v4Icon, string sDesc, Vector4 v4Desc)
	{
		if (sDesc.Length >= 1)
		{
			((UIText)m_control_table["txtHelpDesc"]).Visible = true;
			((UIText)m_control_table["txtHelpDesc"]).SetRect(new Rect(v4Desc.x - v4Desc.z / 2f, v4Desc.y - v4Desc.w / 2f, v4Desc.z, v4Desc.w));
			((UIText)m_control_table["txtHelpDesc"]).SetText(sDesc);
			((UIText)m_control_table["txtHelpDesc"]).SetAlpha(1f);
			StopAnimation("helpDescFadeOut");
		}
	}

	public void SetGameHelpIconPos(Vector2 v2Pos)
	{
		if (m_ImageAnim != null)
		{
			m_ImageAnim.SetPos(v2Pos);
			m_ImageAnim.Show(true);
			m_ImageAnim.PlayAnim(0.1f, true);
		}
		if (m_FingerClick != null)
		{
			v2Pos += new Vector2(24f, -21f) * m_GameState.m_nHDFactor;
			m_FingerClick.SetPos(v2Pos);
			m_FingerClick.Show(true);
			m_FingerClick.SetFrame(0);
		}
	}

	public void SetGameHelpIconMove(Vector2 v2Src, Vector2 v2Dst)
	{
		if (m_animations.ContainsKey("helpIconMove"))
		{
			UIAnimations uIAnimations = (UIAnimations)m_animations["helpIconMove"];
			((UIImage)m_control_table["imgHelpIcon"]).Visible = true;
			((UIImage)m_control_table["imgHelpIcon"]).SetPosition(v2Src);
			uIAnimations.translate_offset = v2Dst - v2Src;
			StartAnimation("helpIconMove");
			if (m_FingerClick != null)
			{
				m_FingerClick.SetFrame(0);
			}
		}
	}

	public void RemoveGameHelpUI()
	{
		((UIImage)m_control_table["imgHelpIcon"]).Visible = false;
		((UIText)m_control_table["txtHelpDesc"]).Visible = false;
		StopAnimation("helpIconMove");
		m_ImageAnim.Show(false);
	}

	public UIImage GetUIImage(string sName)
	{
		if (m_control_table.ContainsKey(sName))
		{
			return (UIImage)m_control_table[sName];
		}
		return null;
	}
}
