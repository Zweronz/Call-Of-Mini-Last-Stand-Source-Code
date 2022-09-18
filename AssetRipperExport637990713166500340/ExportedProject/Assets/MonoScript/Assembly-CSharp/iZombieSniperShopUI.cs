using System.Collections.Generic;
using UnityEngine;

public class iZombieSniperShopUI : UIHelper
{
	public struct ShopPriorityInfo
	{
		public int weaponid;

		public int priority;
	}

	public iZombieSniperGameState m_GameState;

	public iZombieSniperGunCenter m_GunCenter;

	public iZombieSniperIconCenter m_IconCenter;

	public iZombieSniperGameShop m_GameShop;

	public CShopCell m_ShopCell;

	public CShopPower m_ShopPower;

	public int m_nShopCellID;

	public DialogEnum m_nDialogType;

	public CImageAnim m_ArrowLeft;

	public CImageAnim m_ArrowRight;

	public List<ShopPriorityInfo> m_ShopWeaponList;

	public new void Start()
	{
		m_font_path = "ZombieSniper/Fonts/Materials/";
		m_ui_material_path = "ZombieSniper/UI/Materials/";
		m_ui_cfgxml = "ZombieSniper/UI/ShopUI";
		base.Start();
		Initialize();
	}

	public new void Update()
	{
		base.Update();
		if (m_GameShop.m_Stage != GameShopStage.DEFENCE)
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
	}

	public void Initialize()
	{
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_GunCenter = iZombieSniperGameApp.GetInstance().m_GunCenter;
		m_IconCenter = iZombieSniperGameApp.GetInstance().m_IconCenter;
		m_GameShop = GameObject.Find("Main Camera").GetComponent<iZombieSniperGameShop>();
		m_ShopCell = new CShopCell();
		m_ShopCell.Initialize(this, m_UIManagerRef, new Vector2(100f, (float)Screen.height / 5.35f), false);
		m_ShopCell.SetLayer(2);
		m_ShopPower = new CShopPower();
		m_ShopPower.Initialize(this, m_UIManagerRef, new Vector2(100f, (float)Screen.height / 5.35f), false);
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
		m_nShopCellID = -1;
		((UIText)m_control_table["txtInnocents"]).SetText(m_GameState.m_nSaveInnoTotalNum.ToString());
		SetPlayerGold(m_GameState.m_nPlayerTotalCash);
		SetPlayerGodGold(m_GameState.m_nPlayerTotalGod);
		InitializeShopList();
		ShowWeaponList(GameShopStage.SNIPER);
		ShowWeaponTip();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (m_nDialogType != 0)
		{
			if (command != 0)
			{
				return;
			}
			if (control.Id == GetControlId("btnPOPConfirm"))
			{
				DialogEnum nDialogType = m_nDialogType;
				if (nDialogType == DialogEnum.Market)
				{
					iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
					iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kIAP);
					HideDialog();
				}
			}
			else if (control.Id == GetControlId("btnPOPCancel"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				HideDialog();
			}
		}
		else
		{
			if (command != 0)
			{
				return;
			}
			if (control.Id == GetControlId("btnBack"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				if (m_GameShop.GetState() == GameShopState.Normal)
				{
					m_GameState.SaveData();
					iZombieSniperGameApp.GetInstance().BackScene();
				}
			}
			else if (control.Id == GetControlId("btnWeapon1on"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickLabel");
				if (m_GameShop.GetState() == GameShopState.Normal)
				{
					ShowWeaponList(GameShopStage.SNIPER);
				}
			}
			else if (control.Id == GetControlId("btnWeapon2on"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickLabel");
				if (m_GameShop.GetState() == GameShopState.Normal)
				{
					ShowWeaponList(GameShopStage.AUTOSHOOT);
				}
			}
			else if (control.Id == GetControlId("btnWeapon3on"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickLabel");
				if (m_GameShop.GetState() == GameShopState.Normal)
				{
					ShowWeaponList(GameShopStage.BAZOOKA);
				}
			}
			else if (control.Id == GetControlId("btnWeapon4on"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickLabel");
				if (m_GameShop.GetState() == GameShopState.Normal)
				{
					ShowWeaponList(GameShopStage.DEFENCE);
				}
			}
			else if (control.Id == GetControlId("btnBuyCancel"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				if (m_GameShop.m_Stage != GameShopStage.DEFENCE)
				{
					ShowWeaponBuy(false);
				}
				else
				{
					ShowPowerBuy(false);
				}
			}
			else if (control.Id == GetControlId("btnBuyConfirm"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				if (m_GameShop.m_Stage != GameShopStage.DEFENCE)
				{
					if (m_GameShop.ConfirmBuyWeapon(m_nShopCellID))
					{
						ShowWeaponBuy(false);
					}
				}
				else if (m_GameShop.ConfirmBuyPower(m_nShopCellID))
				{
					CShopPower.CellInfo shopCellInfo = m_ShopPower.GetShopCellInfo(m_nShopCellID);
					if (shopCellInfo != null)
					{
						UpdateBuyPowerData(shopCellInfo);
					}
				}
			}
			else if (control.Id == GetControlId("btnWarnConfirm"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				ShowWeaponWarn(false);
			}
			else if (control.Id == GetControlId("btnGetMoreCash"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kIAP);
			}
			else
			{
				if (m_GameShop.m_bDragMouse)
				{
					return;
				}
				CShopCell.CellInfo shopCellInfo2 = m_ShopCell.GetShopCellInfo(control.Id);
				if (shopCellInfo2 != null)
				{
					iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
					if (shopCellInfo2.m_bLock)
					{
						((UIText)m_control_table["txtWarnContext"]).SetText("Save " + shopCellInfo2.m_WeaponInfo.m_nCondValue + " survivors to unlock this gun!");
						ShowWeaponWarn(true);
					}
					else
					{
						m_nShopCellID = shopCellInfo2.m_nID;
						UpdateBuyWeaponData(shopCellInfo2);
						ShowWeaponBuy(true);
					}
					return;
				}
				CShopPower.CellInfo shopCellInfo3 = m_ShopPower.GetShopCellInfo(control.Id);
				if (shopCellInfo3 != null)
				{
					iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
					if (shopCellInfo3.m_bLock)
					{
						((UIText)m_control_table["txtWarnContext"]).SetText("You can't use ammo without a Battle Booth!\n Buy a Battle Booth first.");
						ShowWeaponWarn(true);
					}
					else
					{
						m_nShopCellID = shopCellInfo3.m_nID;
						UpdateBuyPowerData(shopCellInfo3);
						ShowPowerBuy(true);
					}
				}
			}
		}
	}

	public void ShowWeaponBuy(bool bShow)
	{
		if (bShow)
		{
			m_GameShop.SetState(GameShopState.Dialog);
			((UIImage)m_control_table["imgBuyWeapon"]).SetScale(0f);
			((UIImage)m_control_table["imgBuyWeapon"]).Visible = bShow;
			((UIImage)m_control_table["imgBuyWeapon"]).Enable = bShow;
			((UIImage)m_control_table["imgBuyWeaponMask"]).Visible = bShow;
			((UIImage)m_control_table["imgBuyWeaponMask"]).Enable = bShow;
			StartAnimation("buypopin");
			return;
		}
		m_GameShop.SetState(GameShopState.Normal);
		m_nShopCellID = -1;
		((UIImage)m_control_table["imgBuyBack1"]).Visible = bShow;
		((UIImage)m_control_table["imgBuyIcon"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoName"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoDmg"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoDmgValue"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoRT"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoRTValue"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoSR"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoSRValue"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoSZ"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoSZValue"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoSW"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoSWValue"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoSS"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoSSValue"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoGC"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoGCValue"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoDesc"]).Visible = bShow;
		((UIImage)m_control_table["imgPriceIcon"]).Visible = bShow;
		((UIText)m_control_table["txtPriceNum"]).Visible = bShow;
		((UIClickButton)m_control_table["btnBuyConfirm"]).Visible = bShow;
		((UIClickButton)m_control_table["btnBuyConfirm"]).Enable = bShow;
		((UIClickButton)m_control_table["btnBuyCancel"]).Visible = bShow;
		((UIClickButton)m_control_table["btnBuyCancel"]).Enable = bShow;
		StartAnimation("buypopout");
	}

	public void ShowWeaponWarn(bool bShow)
	{
		if (bShow)
		{
			m_GameShop.SetState(GameShopState.Dialog);
			((UIImage)m_control_table["imgWarnWeapon"]).SetScale(0f);
			((UIImage)m_control_table["imgWarnWeapon"]).Visible = bShow;
			((UIImage)m_control_table["imgWarnWeapon"]).Enable = bShow;
			((UIImage)m_control_table["imgWarnWeaponMask"]).Visible = bShow;
			((UIImage)m_control_table["imgWarnWeaponMask"]).Enable = bShow;
			StartAnimation("warnpopin");
		}
		else
		{
			m_GameShop.SetState(GameShopState.Normal);
			((UIClickButton)m_control_table["btnWarnConfirm"]).Visible = bShow;
			((UIClickButton)m_control_table["btnWarnConfirm"]).Enable = bShow;
			((UIText)m_control_table["txtWarnContext"]).Visible = bShow;
			StartAnimation("warnpopout");
		}
	}

	public void ShowPowerBuy(bool bShow)
	{
		if (bShow)
		{
			m_GameShop.SetState(GameShopState.Dialog);
			((UIImage)m_control_table["imgBuyWeapon"]).SetScale(0f);
			((UIImage)m_control_table["imgBuyWeapon"]).Visible = bShow;
			((UIImage)m_control_table["imgBuyWeapon"]).Enable = bShow;
			((UIImage)m_control_table["imgBuyWeaponMask"]).Visible = bShow;
			((UIImage)m_control_table["imgBuyWeaponMask"]).Enable = bShow;
			StartAnimation("buypopin");
			return;
		}
		m_GameShop.SetState(GameShopState.Normal);
		m_nShopCellID = -1;
		((UIImage)m_control_table["imgBuyBack1"]).Visible = bShow;
		((UIImage)m_control_table["imgBuyIcon"]).Visible = bShow;
		((UIText)m_control_table["txtBuyInfoName"]).Visible = bShow;
		((UIText)m_control_table["txtPowerInfoReserve"]).Visible = bShow;
		((UIText)m_control_table["txtPowerInfoDesc"]).Visible = bShow;
		((UIImage)m_control_table["imgPowerInfoStar1"]).Visible = bShow;
		((UIImage)m_control_table["imgPowerInfoStar2"]).Visible = bShow;
		((UIImage)m_control_table["imgPowerInfoStar3"]).Visible = bShow;
		((UIImage)m_control_table["imgPowerInfoStar4"]).Visible = bShow;
		((UIImage)m_control_table["imgPowerInfoStar5"]).Visible = bShow;
		((UIImage)m_control_table["imgPriceIcon"]).Visible = bShow;
		((UIText)m_control_table["txtPriceNum"]).Visible = bShow;
		((UIClickButton)m_control_table["btnBuyConfirm"]).Visible = bShow;
		((UIClickButton)m_control_table["btnBuyConfirm"]).Enable = bShow;
		((UIClickButton)m_control_table["btnBuyCancel"]).Visible = bShow;
		((UIClickButton)m_control_table["btnBuyCancel"]).Enable = bShow;
		StartAnimation("buypopout");
	}

	public override void OnAnimationFinished(string name)
	{
		switch (name)
		{
		case "buypopin":
			if (m_GameShop.m_Stage != GameShopStage.DEFENCE)
			{
				CShopCell.CellInfo shopCellInfo = m_ShopCell.GetShopCellInfo(m_nShopCellID);
				if (shopCellInfo == null)
				{
					break;
				}
				((UIImage)m_control_table["imgBuyBack1"]).Visible = true;
				((UIImage)m_control_table["imgBuyIcon"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoName"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoDmg"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoDmgValue"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoRT"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoRTValue"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoSR"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoSRValue"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoSZ"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoSZValue"]).Visible = true;
				if (shopCellInfo.m_WeaponInfo.IsRocket() || shopCellInfo.m_WeaponInfo.IsThrowMine())
				{
					((UIText)m_control_table["txtBuyInfoSW"]).Visible = true;
					((UIText)m_control_table["txtBuyInfoSWValue"]).Visible = true;
				}
				else
				{
					((UIText)m_control_table["txtBuyInfoSS"]).Visible = true;
					((UIText)m_control_table["txtBuyInfoSSValue"]).Visible = true;
				}
				((UIText)m_control_table["txtBuyInfoGC"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoGCValue"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoDesc"]).Visible = true;
				if (!shopCellInfo.m_bPurchase)
				{
					((UIClickButton)m_control_table["btnBuyConfirm"]).Visible = true;
					((UIClickButton)m_control_table["btnBuyConfirm"]).Enable = true;
				}
				((UIImage)m_control_table["imgPriceIcon"]).Visible = true;
				((UIText)m_control_table["txtPriceNum"]).Visible = true;
			}
			else
			{
				CShopPower.CellInfo shopCellInfo2 = m_ShopPower.GetShopCellInfo(m_nShopCellID);
				if (shopCellInfo2 == null)
				{
					break;
				}
				((UIImage)m_control_table["imgBuyBack1"]).Visible = true;
				((UIImage)m_control_table["imgBuyIcon"]).Visible = true;
				((UIText)m_control_table["txtBuyInfoName"]).Visible = true;
				if (shopCellInfo2.m_PowerInfo.m_nType != PowerUpEnum.Telephone_Booth_Dmg)
				{
					((UIText)m_control_table["txtPowerInfoReserve"]).Visible = true;
				}
				else
				{
					((UIImage)m_control_table["imgPowerInfoStar1"]).Visible = true;
					((UIImage)m_control_table["imgPowerInfoStar2"]).Visible = true;
					((UIImage)m_control_table["imgPowerInfoStar3"]).Visible = true;
					((UIImage)m_control_table["imgPowerInfoStar4"]).Visible = true;
					((UIImage)m_control_table["imgPowerInfoStar5"]).Visible = true;
				}
				((UIText)m_control_table["txtPowerInfoDesc"]).Visible = true;
				if (!shopCellInfo2.m_PowerInfo.IsMax())
				{
					((UIClickButton)m_control_table["btnBuyConfirm"]).Visible = true;
					((UIClickButton)m_control_table["btnBuyConfirm"]).Enable = true;
					((UIImage)m_control_table["imgPriceIcon"]).Visible = true;
					((UIText)m_control_table["txtPriceNum"]).Visible = true;
				}
			}
			((UIClickButton)m_control_table["btnBuyCancel"]).Visible = true;
			((UIClickButton)m_control_table["btnBuyCancel"]).Enable = true;
			break;
		case "buypopout":
			((UIImage)m_control_table["imgBuyWeapon"]).Visible = false;
			((UIImage)m_control_table["imgBuyWeapon"]).Enable = false;
			((UIImage)m_control_table["imgBuyWeaponMask"]).Visible = false;
			((UIImage)m_control_table["imgBuyWeaponMask"]).Enable = false;
			break;
		case "warnpopin":
			((UIClickButton)m_control_table["btnWarnConfirm"]).Visible = true;
			((UIClickButton)m_control_table["btnWarnConfirm"]).Enable = true;
			((UIText)m_control_table["txtWarnContext"]).Visible = true;
			break;
		case "warnpopout":
			((UIImage)m_control_table["imgWarnWeapon"]).Visible = false;
			((UIImage)m_control_table["imgWarnWeapon"]).Enable = false;
			((UIImage)m_control_table["imgWarnWeaponMask"]).Visible = false;
			((UIImage)m_control_table["imgWarnWeaponMask"]).Enable = false;
			break;
		}
	}

	public void ShowWeaponList(GameShopStage stage)
	{
		m_GameShop.m_Stage = stage;
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
			iWeaponInfoBase iWeaponInfoBase2 = null;
			foreach (ShopPriorityInfo shopWeapon in m_ShopWeaponList)
			{
				iWeaponInfoBase2 = m_GunCenter.GetWeaponInfoBase(shopWeapon.weaponid);
				if (iWeaponInfoBase2 != null)
				{
					if (iWeaponInfoBase2.IsRifle() && stage == GameShopStage.SNIPER)
					{
						m_ShopCell.AddWeapon(iWeaponInfoBase2.m_nWeaponID);
					}
					else if (iWeaponInfoBase2.IsAutoShoot() && stage == GameShopStage.AUTOSHOOT)
					{
						m_ShopCell.AddWeapon(iWeaponInfoBase2.m_nWeaponID);
					}
					else if ((iWeaponInfoBase2.IsRocket() || iWeaponInfoBase2.IsThrowMine()) && stage == GameShopStage.BAZOOKA)
					{
						m_ShopCell.AddWeapon(iWeaponInfoBase2.m_nWeaponID);
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

	private void UpdateBuyWeaponData(CShopCell.CellInfo cellInfo)
	{
		iWeaponInfoBase weaponInfo = cellInfo.m_WeaponInfo;
		Material material = null;
		IconInfo icon = m_IconCenter.GetIcon(weaponInfo.m_nIconID);
		if (icon != null)
		{
			material = LoadUIMaterial(Utils.AutoMaterialName(icon.m_sMaterial));
			if (material != null)
			{
				((UIImage)m_control_table["imgBuyIcon"]).SetTexture(material, new Rect(icon.m_Rect.xMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.yMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.width * (float)m_GameState.m_nHDFactor, icon.m_Rect.height * (float)m_GameState.m_nHDFactor));
			}
		}
		((UIText)m_control_table["txtBuyInfoName"]).SetText(weaponInfo.m_sName);
		((UIText)m_control_table["txtBuyInfoDmgValue"]).SetText(weaponInfo.m_fBaseSD.ToString());
		((UIText)m_control_table["txtBuyInfoRTValue"]).SetText(weaponInfo.m_fBaseSR.ToString());
		((UIText)m_control_table["txtBuyInfoSRValue"]).SetText(weaponInfo.m_fBaseSG.ToString());
		((UIText)m_control_table["txtBuyInfoSZValue"]).SetText(weaponInfo.m_fBaseSZ.ToString());
		((UIText)m_control_table["txtBuyInfoSWValue"]).SetText(weaponInfo.m_fBaseSW.ToString());
		((UIText)m_control_table["txtBuyInfoSSValue"]).SetText((60f / weaponInfo.m_fBaseSS).ToString());
		((UIText)m_control_table["txtBuyInfoGCValue"]).SetText(weaponInfo.m_nBulletMax.ToString());
		material = LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
		if (material != null)
		{
			((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Normal, material, new Rect(264 * m_GameState.m_nHDFactor, 404 * m_GameState.m_nHDFactor, 127 * m_GameState.m_nHDFactor, 42 * m_GameState.m_nHDFactor));
		}
		material = LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
		if (material != null)
		{
			((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Pressed, material, new Rect(136 * m_GameState.m_nHDFactor, 404 * m_GameState.m_nHDFactor, 127 * m_GameState.m_nHDFactor, 42 * m_GameState.m_nHDFactor));
			((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Disabled, material, new Rect(136 * m_GameState.m_nHDFactor, 404 * m_GameState.m_nHDFactor, 127 * m_GameState.m_nHDFactor, 42 * m_GameState.m_nHDFactor));
		}
		((UIText)m_control_table["txtBuyInfoDesc"]).SetText(weaponInfo.m_sDesc.ToString());
		if (weaponInfo.m_bGodPrice)
		{
			material = LoadUIMaterial(Utils.AutoMaterialName("crystal"));
			if (material != null)
			{
				((UIImage)m_control_table["imgPriceIcon"]).SetTexture(material, new Rect(0f, 0f, 9 * m_GameState.m_nHDFactor, 13 * m_GameState.m_nHDFactor));
			}
		}
		else
		{
			material = LoadUIMaterial(Utils.AutoMaterialName("game over_jbi"));
			if (material != null)
			{
				((UIImage)m_control_table["imgPriceIcon"]).SetTexture(material, new Rect(0f, 0f, 12 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor));
			}
		}
		((UIText)m_control_table["txtPriceNum"]).SetText(Utils.PriceToString(weaponInfo.m_nPrice));
	}

	private void UpdateBuyPowerData(CShopPower.CellInfo cellInfo)
	{
		if (cellInfo.m_PowerInfo.IsMax())
		{
			((UIClickButton)m_control_table["btnBuyConfirm"]).Visible = false;
			((UIClickButton)m_control_table["btnBuyConfirm"]).Enable = false;
			((UIImage)m_control_table["imgPriceIcon"]).Visible = false;
			((UIText)m_control_table["txtPriceNum"]).Visible = false;
		}
		Material material = LoadUIMaterial(Utils.AutoMaterialName(cellInfo.m_PowerInfo.m_sMaterial));
		if (material != null)
		{
			((UIImage)m_control_table["imgBuyIcon"]).SetTexture(material, new Rect(cellInfo.m_PowerInfo.m_Rect.xMin * (float)m_GameState.m_nHDFactor, cellInfo.m_PowerInfo.m_Rect.yMin * (float)m_GameState.m_nHDFactor, cellInfo.m_PowerInfo.m_Rect.width * (float)m_GameState.m_nHDFactor, cellInfo.m_PowerInfo.m_Rect.height * (float)m_GameState.m_nHDFactor));
		}
		((UIText)m_control_table["txtBuyInfoName"]).SetText(cellInfo.m_PowerInfo.m_sName);
		if (cellInfo.m_PowerInfo.m_nType != PowerUpEnum.Telephone_Booth_Dmg)
		{
			if (cellInfo.m_PowerInfo.m_nType == PowerUpEnum.Telephone_Booth_Bullet)
			{
				if (cellInfo.m_PowerInfo.m_nReserves > 10)
				{
					((UIText)m_control_table["txtPowerInfoReserve"]).SetText("IN RESERVE: " + (cellInfo.m_PowerInfo.m_nReserves - 10));
				}
				else
				{
					((UIText)m_control_table["txtPowerInfoReserve"]).SetText("IN RESERVE: " + cellInfo.m_PowerInfo.m_nReserves);
				}
			}
			else
			{
				((UIText)m_control_table["txtPowerInfoReserve"]).SetText("IN RESERVE: " + cellInfo.m_PowerInfo.m_nReserves);
			}
		}
		else
		{
			Rect rect = new Rect(496 * cellInfo.m_nHDFactor, 62 * cellInfo.m_nHDFactor, 11 * cellInfo.m_nHDFactor, 9 * cellInfo.m_nHDFactor);
			Rect rect2 = new Rect(484 * cellInfo.m_nHDFactor, 62 * cellInfo.m_nHDFactor, 11 * cellInfo.m_nHDFactor, 9 * cellInfo.m_nHDFactor);
			((UIImage)m_control_table["imgPowerInfoStar1"]).SetTexture(LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1")), (cellInfo.m_PowerInfo.m_nReserves < 1) ? rect2 : rect);
			((UIImage)m_control_table["imgPowerInfoStar2"]).SetTexture(LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1")), (cellInfo.m_PowerInfo.m_nReserves < 2) ? rect2 : rect);
			((UIImage)m_control_table["imgPowerInfoStar3"]).SetTexture(LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1")), (cellInfo.m_PowerInfo.m_nReserves < 3) ? rect2 : rect);
			((UIImage)m_control_table["imgPowerInfoStar4"]).SetTexture(LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1")), (cellInfo.m_PowerInfo.m_nReserves < 4) ? rect2 : rect);
			((UIImage)m_control_table["imgPowerInfoStar5"]).SetTexture(LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1")), (cellInfo.m_PowerInfo.m_nReserves < 5) ? rect2 : rect);
		}
		switch (cellInfo.m_PowerInfo.m_nType)
		{
		case PowerUpEnum.Telephone_Booth_Dmg:
			material = LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part5"));
			if (material != null)
			{
				((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Normal, material, new Rect(156 * m_GameState.m_nHDFactor, 46 * m_GameState.m_nHDFactor, 153 * m_GameState.m_nHDFactor, 38 * m_GameState.m_nHDFactor));
			}
			material = LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part5"));
			if (material != null)
			{
				((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Pressed, material, new Rect(2 * m_GameState.m_nHDFactor, 46 * m_GameState.m_nHDFactor, 153 * m_GameState.m_nHDFactor, 38 * m_GameState.m_nHDFactor));
				((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Disabled, material, new Rect(2 * m_GameState.m_nHDFactor, 46 * m_GameState.m_nHDFactor, 153 * m_GameState.m_nHDFactor, 38 * m_GameState.m_nHDFactor));
			}
			break;
		case PowerUpEnum.Telephone_Booth_Bullet:
			material = LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
			if (material != null)
			{
				((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Normal, material, new Rect(290 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, 153 * m_GameState.m_nHDFactor, 38 * m_GameState.m_nHDFactor));
			}
			material = LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
			if (material != null)
			{
				((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Pressed, material, new Rect(136 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, 153 * m_GameState.m_nHDFactor, 38 * m_GameState.m_nHDFactor));
				((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Disabled, material, new Rect(136 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, 153 * m_GameState.m_nHDFactor, 38 * m_GameState.m_nHDFactor));
			}
			break;
		case PowerUpEnum.AirStrike:
		case PowerUpEnum.LandMine:
		case PowerUpEnum.InnoKiller:
		case PowerUpEnum.MachineGun:
			material = LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
			if (material != null)
			{
				((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Normal, material, new Rect(290 * m_GameState.m_nHDFactor, 364 * m_GameState.m_nHDFactor, 153 * m_GameState.m_nHDFactor, 38 * m_GameState.m_nHDFactor));
			}
			material = LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
			if (material != null)
			{
				((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Pressed, material, new Rect(136 * m_GameState.m_nHDFactor, 364 * m_GameState.m_nHDFactor, 153 * m_GameState.m_nHDFactor, 38 * m_GameState.m_nHDFactor));
				((UIClickButton)m_control_table["btnBuyConfirm"]).SetTexture(UIButtonBase.State.Disabled, material, new Rect(136 * m_GameState.m_nHDFactor, 364 * m_GameState.m_nHDFactor, 153 * m_GameState.m_nHDFactor, 38 * m_GameState.m_nHDFactor));
			}
			break;
		}
		((UIText)m_control_table["txtPowerInfoDesc"]).SetText(cellInfo.m_PowerInfo.m_sDesc);
		if (cellInfo.m_PowerInfo.m_bGodPrice)
		{
			material = LoadUIMaterial(Utils.AutoMaterialName("crystal"));
			if (material != null)
			{
				((UIImage)m_control_table["imgPriceIcon"]).SetTexture(material, new Rect(0f, 0f, 9 * m_GameState.m_nHDFactor, 13 * m_GameState.m_nHDFactor));
			}
		}
		else
		{
			material = LoadUIMaterial(Utils.AutoMaterialName("game over_jbi"));
			if (material != null)
			{
				((UIImage)m_control_table["imgPriceIcon"]).SetTexture(material, new Rect(1008 * m_GameState.m_nHDFactor, 216 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor));
			}
		}
		((UIText)m_control_table["txtPriceNum"]).SetText(cellInfo.m_PowerInfo.GetPrice().ToString());
	}

	public void SetPlayerGold(int nGold)
	{
		((UIText)m_control_table["txtGold"]).SetText(nGold.ToString());
	}

	public void SetPlayerGodGold(int nGold)
	{
		((UIText)m_control_table["txtDollar"]).SetText(nGold.ToString());
	}

	public void ShowWeaponTip()
	{
		int num = 0;
		bool flag = false;
		num = iZombieSniperGameApp.GetInstance().GetCanBuyCount(GameShopStage.SNIPER);
		flag = num > 0;
		((UIImage)m_control_table["imgShopTip1"]).Visible = flag;
		((UIText)m_control_table["txtShopTip1"]).Visible = flag;
		if (flag)
		{
			((UIText)m_control_table["txtShopTip1"]).SetText(num.ToString());
		}
		num = iZombieSniperGameApp.GetInstance().GetCanBuyCount(GameShopStage.AUTOSHOOT);
		flag = num > 0;
		((UIImage)m_control_table["imgShopTip2"]).Visible = flag;
		((UIText)m_control_table["txtShopTip2"]).Visible = flag;
		if (flag)
		{
			((UIText)m_control_table["txtShopTip2"]).SetText(num.ToString());
		}
		num = iZombieSniperGameApp.GetInstance().GetCanBuyCount(GameShopStage.BAZOOKA);
		flag = num > 0;
		((UIImage)m_control_table["imgShopTip3"]).Visible = flag;
		((UIText)m_control_table["txtShopTip3"]).Visible = flag;
		if (flag)
		{
			((UIText)m_control_table["txtShopTip3"]).SetText(num.ToString());
		}
		num = iZombieSniperGameApp.GetInstance().GetCanBuyCount(GameShopStage.DEFENCE);
		flag = num > 0;
		((UIImage)m_control_table["imgShopTip4"]).Visible = flag;
		((UIText)m_control_table["txtShopTip4"]).Visible = flag;
		if (flag)
		{
			((UIText)m_control_table["txtShopTip4"]).SetText(num.ToString());
		}
	}

	public void ShowDialog(DialogEnum type, string text)
	{
		m_nDialogType = type;
		((UIText)m_control_table["txtPOPText"]).Visible = true;
		((UIText)m_control_table["txtPOPText"]).SetText(text);
		((UIImage)m_control_table["imgPOPMask"]).Visible = true;
		((UIImage)m_control_table["imgPOPMask"]).Enable = true;
		((UIImage)m_control_table["imgPOPBack"]).Visible = true;
		((UIClickButton)m_control_table["btnPOPConfirm"]).Visible = true;
		((UIClickButton)m_control_table["btnPOPConfirm"]).Enable = true;
		((UIClickButton)m_control_table["btnPOPCancel"]).Visible = true;
		((UIClickButton)m_control_table["btnPOPCancel"]).Enable = true;
	}

	public void HideDialog()
	{
		m_nDialogType = DialogEnum.None;
		((UIText)m_control_table["txtPOPText"]).Visible = false;
		((UIImage)m_control_table["imgPOPMask"]).Visible = false;
		((UIImage)m_control_table["imgPOPMask"]).Enable = false;
		((UIImage)m_control_table["imgPOPBack"]).Visible = false;
		((UIClickButton)m_control_table["btnPOPConfirm"]).Visible = false;
		((UIClickButton)m_control_table["btnPOPConfirm"]).Enable = false;
		((UIClickButton)m_control_table["btnPOPCancel"]).Visible = false;
		((UIClickButton)m_control_table["btnPOPCancel"]).Enable = false;
	}

	public UIImage GetUIImage(string sName)
	{
		if (m_control_table.ContainsKey(sName))
		{
			return (UIImage)m_control_table[sName];
		}
		return null;
	}

	private void InitializeShopList()
	{
		if (m_ShopWeaponList == null)
		{
			m_ShopWeaponList = new List<ShopPriorityInfo>();
		}
		m_ShopWeaponList.Clear();
		foreach (iWeaponInfoBase value in m_GunCenter.m_WeaponInfoBase.Values)
		{
			int num = 0;
			for (int i = 0; i < m_ShopWeaponList.Count; i++)
			{
				if (value.m_nShopPriority >= m_ShopWeaponList[i].priority)
				{
					num = i + 1;
				}
			}
			ShopPriorityInfo item = default(ShopPriorityInfo);
			item.weaponid = value.m_nWeaponID;
			item.priority = value.m_nShopPriority;
			if (num > 0 && num == m_ShopWeaponList.Count)
			{
				m_ShopWeaponList.Add(item);
			}
			else
			{
				m_ShopWeaponList.Insert(num, item);
			}
		}
	}
}
