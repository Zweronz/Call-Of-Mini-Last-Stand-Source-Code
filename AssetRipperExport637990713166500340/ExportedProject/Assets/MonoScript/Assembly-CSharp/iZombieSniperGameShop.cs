using UnityEngine;

public class iZombieSniperGameShop : MonoBehaviour
{
	public iZombieSniperShopUI m_GameShopUI;

	public iZombieSniperGameState m_GameState;

	public iZombieSniperGunCenter m_GunCenter;

	public int m_nSelectWeapon;

	public Vector2 m_v2TouchPos = Vector2.zero;

	public GameShopState m_State = GameShopState.Normal;

	public GameShopStage m_Stage;

	public bool m_bDragMouse;

	private string sMoneyIsNotEnough = "Not enough cash, do you want to buy more?";

	private void Start()
	{
		GameObject gameObject = GameObject.Find("Main Camera");
		m_GameShopUI = gameObject.GetComponent<iZombieSniperShopUI>();
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_GunCenter = iZombieSniperGameApp.GetInstance().m_GunCenter;
		m_bDragMouse = false;
		OpenClikPlugin.Hide();
	}

	private void Update()
	{
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		for (int i = 0; i < array.Length; i++)
		{
			UITouchInner touch = array[i];
			if (m_GameShopUI != null)
			{
				m_GameShopUI.m_UIManagerRef.HandleInput(touch);
			}
			if (m_State == GameShopState.Dialog)
			{
				break;
			}
			if (touch.phase == TouchPhase.Began)
			{
				m_v2TouchPos = touch.position;
				Debug.Log(m_v2TouchPos);
				m_bDragMouse = false;
				if (!Utils.PtInRect(m_v2TouchPos, Utils.CalcScaleRect(new Rect(0f, 75f, 480f, 165f))))
				{
					break;
				}
				if (m_Stage != GameShopStage.DEFENCE)
				{
					if (m_GameShopUI.m_ShopCell.m_bAnim)
					{
						m_GameShopUI.m_ShopCell.m_bAnim = false;
					}
				}
				else if (m_GameShopUI.m_ShopPower.m_bAnim)
				{
					m_GameShopUI.m_ShopPower.m_bAnim = false;
				}
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				if (!m_bDragMouse && Vector2.Distance(m_v2TouchPos, touch.position) > 12f)
				{
					m_bDragMouse = true;
				}
				if (m_bDragMouse)
				{
					if (!Utils.PtInRect(m_v2TouchPos, Utils.CalcScaleRect(new Rect(0f, 75f, 480f, 165f))))
					{
						break;
					}
					if (m_Stage != GameShopStage.DEFENCE)
					{
						float num = Mathf.Abs(touch.deltaPosition.x);
						m_GameShopUI.m_ShopCell.Move(num * 5f, num * 5f, (touch.deltaPosition.x > 0f) ? 1 : (-1));
					}
					else
					{
						float num2 = Mathf.Abs(touch.deltaPosition.x);
						m_GameShopUI.m_ShopPower.Move(num2 * 5f, num2 * 5f, (touch.deltaPosition.x > 0f) ? 1 : (-1));
					}
				}
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				m_bDragMouse = false;
			}
		}
	}

	public bool IsLock(int nWeaponID)
	{
		iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(nWeaponID);
		if (weaponInfoBase == null)
		{
			return true;
		}
		if (weaponInfoBase.IsConditionInno() && m_GameState.m_nSaveInnoTotalNum < weaponInfoBase.m_nCondValue)
		{
			return true;
		}
		return false;
	}

	public bool IsBuy(int nWeaponID)
	{
		return m_GunCenter.GetWeaponData(nWeaponID) != null;
	}

	public bool ConfirmBuyWeapon(int nShopCellID)
	{
		CShopCell.CellInfo shopCellInfo = m_GameShopUI.m_ShopCell.GetShopCellInfo(nShopCellID);
		if (shopCellInfo == null)
		{
			return false;
		}
		if (shopCellInfo.m_bLock)
		{
			return false;
		}
		if (shopCellInfo.m_bPurchase)
		{
			return false;
		}
		iWeaponInfoBase weaponInfo = shopCellInfo.m_WeaponInfo;
		if (m_GunCenter.GetWeaponData(weaponInfo.m_nWeaponID) != null)
		{
			return false;
		}
		if (weaponInfo.m_bGodPrice)
		{
			if (m_GameState.m_nPlayerTotalGod < weaponInfo.m_nPrice)
			{
				m_GameShopUI.ShowDialog(DialogEnum.Market, sMoneyIsNotEnough);
				return false;
			}
			m_GameState.m_nPlayerTotalGod -= weaponInfo.m_nPrice;
			m_GameShopUI.SetPlayerGodGold(m_GameState.m_nPlayerTotalGod);
		}
		else
		{
			if (m_GameState.m_nPlayerTotalCash < weaponInfo.m_nPrice)
			{
				m_GameShopUI.ShowDialog(DialogEnum.Market, sMoneyIsNotEnough);
				return false;
			}
			m_GameState.m_nPlayerTotalCash -= weaponInfo.m_nPrice;
			m_GameShopUI.SetPlayerGold(m_GameState.m_nPlayerTotalCash);
		}
		m_GunCenter.PurchaseWeapon(weaponInfo.m_nWeaponID);
		m_GameShopUI.m_ShopCell.UpdateCell(nShopCellID, weaponInfo.m_nWeaponID);
		m_GameState.SaveData();
		m_GunCenter.SaveWeaponData();
		m_GameShopUI.ShowWeaponTip();
		iZombieSniperGameApp.GetInstance().PlayAudio("UIClickBuy");
		if (m_GunCenter.GetWeaponData(30) != null && m_GunCenter.GetWeaponData(34) != null && m_GunCenter.GetWeaponData(38) != null)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi9);
		}
		if (m_GunCenter.GetWeaponData(10) != null && m_GunCenter.GetWeaponData(22) != null && m_GunCenter.GetWeaponData(26) != null)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi10);
		}
		return true;
	}

	public bool ConfirmBuyPower(int nShopCellID)
	{
		CShopPower.CellInfo shopCellInfo = m_GameShopUI.m_ShopPower.GetShopCellInfo(nShopCellID);
		if (shopCellInfo == null)
		{
			return false;
		}
		if (shopCellInfo.m_PowerInfo.m_bGodPrice)
		{
			if (m_GameState.m_nPlayerTotalGod < shopCellInfo.m_PowerInfo.GetPrice())
			{
				m_GameShopUI.ShowDialog(DialogEnum.Market, sMoneyIsNotEnough);
				return false;
			}
			m_GameState.m_nPlayerTotalGod -= shopCellInfo.m_PowerInfo.GetPrice();
			m_GameShopUI.SetPlayerGodGold(m_GameState.m_nPlayerTotalGod);
		}
		else
		{
			if (m_GameState.m_nPlayerTotalCash < shopCellInfo.m_PowerInfo.GetPrice())
			{
				m_GameShopUI.ShowDialog(DialogEnum.Market, sMoneyIsNotEnough);
				return false;
			}
			m_GameState.m_nPlayerTotalCash -= shopCellInfo.m_PowerInfo.GetPrice();
			m_GameShopUI.SetPlayerGold(m_GameState.m_nPlayerTotalCash);
		}
		switch (shopCellInfo.m_PowerInfo.m_nType)
		{
		case PowerUpEnum.Telephone_Booth_Dmg:
			m_GameState.m_nTowerLvl += shopCellInfo.m_PowerInfo.m_nValue;
			if (m_GameState.m_nTowerLvl > shopCellInfo.m_PowerInfo.m_nReservesMax)
			{
				m_GameState.m_nTowerLvl = shopCellInfo.m_PowerInfo.m_nReservesMax;
			}
			shopCellInfo.m_PowerInfo.m_nReserves = m_GameState.m_nTowerLvl;
			break;
		case PowerUpEnum.Telephone_Booth_Bullet:
			m_GameState.m_nTowerBullet += shopCellInfo.m_PowerInfo.m_nValue;
			shopCellInfo.m_PowerInfo.m_nReserves = m_GameState.m_nTowerBullet;
			break;
		case PowerUpEnum.LandMine:
			m_GameState.m_nMineCount += shopCellInfo.m_PowerInfo.m_nValue;
			shopCellInfo.m_PowerInfo.m_nReserves = m_GameState.m_nMineCount;
			break;
		case PowerUpEnum.AirStrike:
			m_GameState.m_nAC130 += shopCellInfo.m_PowerInfo.m_nValue;
			shopCellInfo.m_PowerInfo.m_nReserves = m_GameState.m_nAC130;
			break;
		case PowerUpEnum.InnoKiller:
			m_GameState.m_nInnoKiller += shopCellInfo.m_PowerInfo.m_nValue;
			shopCellInfo.m_PowerInfo.m_nReserves = m_GameState.m_nInnoKiller;
			break;
		case PowerUpEnum.MachineGun:
			m_GameState.m_nMachineGun += shopCellInfo.m_PowerInfo.m_nValue;
			shopCellInfo.m_PowerInfo.m_nReserves = m_GameState.m_nMachineGun;
			break;
		}
		m_GameShopUI.m_ShopPower.UpdateCell(nShopCellID, shopCellInfo.m_PowerInfo.m_nType);
		if (shopCellInfo.m_PowerInfo.m_nType == PowerUpEnum.Telephone_Booth_Dmg)
		{
			CShopPower.CellInfo shopCellInfoByType = m_GameShopUI.m_ShopPower.GetShopCellInfoByType(PowerUpEnum.Telephone_Booth_Bullet);
			if (shopCellInfoByType != null)
			{
				m_GameShopUI.m_ShopPower.UpdateCell(shopCellInfoByType.m_nID, shopCellInfoByType.m_PowerInfo.m_nType);
			}
		}
		m_GameState.SaveData();
		m_GameShopUI.ShowWeaponTip();
		iZombieSniperGameApp.GetInstance().PlayAudio("UIClickBuy");
		return true;
	}

	public void SetState(GameShopState state)
	{
		m_State = state;
	}

	public GameShopState GetState()
	{
		return m_State;
	}
}
