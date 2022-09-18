using System.Collections;
using UnityEngine;

public class CShopPower
{
	public class CellInfo
	{
		public int m_nID;

		public UIManager m_UIManager;

		public bool m_bUsed;

		public UIClickButton m_btnBackground;

		public Vector2 m_v2Background;

		public UIImage m_imgLock;

		public Vector2 m_v2Lock;

		public UIImage m_imgWeaponIcon;

		public Vector2 m_v2WeaponIcon;

		public UIText m_txtName;

		public Vector2 m_v2Name;

		public UIText m_txtReserve;

		public Vector2 m_v2Reserve;

		public UIImage m_imgBuy;

		public Vector2 m_v2Buy;

		public UIImage m_imgNew;

		public Vector2 m_v2New;

		public bool m_bLock;

		public bool m_bPurchase;

		public int m_nWidth;

		public int m_nHeight;

		public int m_nHDFactor;

		public PowerUpInfo m_PowerInfo;

		public CellInfo(UIManager uimanager)
		{
			m_bUsed = false;
			m_UIManager = uimanager;
			m_nWidth = 133;
			m_nHeight = 80;
			m_nHDFactor = ((!Utils.IsRetina()) ? 1 : 2);
			m_PowerInfo = new PowerUpInfo();
			m_v2Lock = new Vector2(-m_nWidth / 2 + 16, m_nHeight / 2 - 14);
			m_v2Buy = new Vector2(-m_nWidth / 2 + 16, m_nHeight / 2 - 14);
			m_v2WeaponIcon = new Vector2(0f, 0f);
			m_v2Name = new Vector2(0f, 0f);
			m_v2Reserve = new Vector2(0f, 55f);
			m_v2New = new Vector2(-m_nWidth / 2 + 28, -m_nHeight / 2 + 14);
		}

		public void Show(bool bShow)
		{
			if (m_btnBackground != null)
			{
				m_btnBackground.Visible = bShow;
				m_btnBackground.Enable = bShow;
			}
			if (m_imgWeaponIcon != null)
			{
				m_imgWeaponIcon.Visible = bShow;
			}
			if (m_imgLock != null)
			{
				if (bShow && m_bLock)
				{
					m_imgLock.Visible = true;
				}
				else
				{
					m_imgLock.Visible = false;
				}
			}
			if (m_imgBuy != null)
			{
				if (bShow && !m_bLock && !m_bPurchase)
				{
					m_imgBuy.Visible = true;
				}
				else
				{
					m_imgBuy.Visible = false;
				}
			}
			if (m_txtName != null)
			{
				m_txtName.Visible = bShow;
			}
			if (m_txtReserve != null)
			{
				m_txtReserve.Visible = bShow;
			}
			if (m_imgNew != null)
			{
				m_imgNew.Visible = bShow;
			}
		}

		public void SetLayer(int nLayer)
		{
			if (m_btnBackground != null)
			{
				m_btnBackground.Layer = nLayer;
				m_UIManager.Remove(m_btnBackground);
				m_UIManager.Add(m_btnBackground);
			}
			if (m_imgWeaponIcon != null)
			{
				m_imgWeaponIcon.Layer = nLayer;
				m_UIManager.Remove(m_imgWeaponIcon);
				m_UIManager.Add(m_imgWeaponIcon);
			}
			if (m_imgLock != null)
			{
				m_imgLock.Layer = nLayer;
				m_UIManager.Remove(m_imgLock);
				m_UIManager.Add(m_imgLock);
			}
			if (m_imgBuy != null)
			{
				m_imgBuy.Layer = nLayer;
				m_UIManager.Remove(m_imgBuy);
				m_UIManager.Add(m_imgBuy);
			}
			if (m_txtName != null)
			{
				m_txtName.Layer = nLayer;
				m_UIManager.Remove(m_txtName);
				m_UIManager.Add(m_txtName);
			}
			if (m_txtReserve != null)
			{
				m_txtReserve.Layer = nLayer;
				m_UIManager.Remove(m_txtReserve);
				m_UIManager.Add(m_txtReserve);
			}
			if (m_imgNew != null)
			{
				m_imgNew.Layer = nLayer;
				m_UIManager.Remove(m_imgNew);
				m_UIManager.Add(m_imgNew);
			}
		}

		public void SetPos(Vector2 v2Pos)
		{
			m_v2Background = v2Pos;
			if (m_btnBackground != null)
			{
				m_btnBackground.Rect = new Rect((v2Pos.x - (float)(m_nWidth / 2)) * (float)m_nHDFactor, (v2Pos.y - (float)(m_nHeight / 2)) * (float)m_nHDFactor, m_nWidth * m_nHDFactor, m_nHeight * m_nHDFactor);
			}
			if (m_imgWeaponIcon != null)
			{
				m_imgWeaponIcon.SetPosition((v2Pos + m_v2WeaponIcon) * m_nHDFactor);
			}
			if (m_imgLock != null)
			{
				m_imgLock.SetPosition((v2Pos + m_v2Lock) * m_nHDFactor);
			}
			if (m_imgBuy != null)
			{
				m_imgBuy.SetPosition((v2Pos + m_v2Buy) * m_nHDFactor);
			}
			if (m_txtName != null)
			{
				m_txtName.Rect = new Rect((v2Pos.x - (float)(m_nWidth / 2) + m_v2Name.x) * (float)m_nHDFactor, (v2Pos.y - (float)(m_nHeight / 2) + m_v2Name.y) * (float)m_nHDFactor, 127 * m_nHDFactor, 20 * m_nHDFactor);
			}
			if (m_txtReserve != null)
			{
				m_txtReserve.Rect = new Rect((v2Pos.x - (float)(m_nWidth / 2) + m_v2Reserve.x) * (float)m_nHDFactor, (v2Pos.y - (float)(m_nHeight / 2) + m_v2Reserve.y) * (float)m_nHDFactor, 127 * m_nHDFactor, 20 * m_nHDFactor);
			}
			if (m_imgNew != null)
			{
				m_imgNew.SetPosition((v2Pos + m_v2New) * m_nHDFactor);
			}
		}
	}

	public UIManager m_UIManager;

	public iZombieSniperGameState m_GameState;

	public iZombieSniperGunCenter m_GunCenter;

	public iZombieSniperIconCenter m_IconCenter;

	public iZombieSniperGameShop m_GameShop;

	public UIHelper m_UIHelper;

	public Hashtable m_ShopCell;

	public Vector2 m_v2Position = Vector2.zero;

	public Vector2 m_v2InitPos = Vector2.zero;

	public int m_nLayer;

	public int m_nlen;

	public bool m_bSingleOrDouble;

	public int m_nSelectWeaponID;

	public bool m_bAnim;

	public float m_fSpeed;

	public float m_fAcc;

	public int m_nDir;

	public void Initialize(UIHelper helper, UIManager uimanager, Vector2 v2CellPos, bool bSingleOrDouble)
	{
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_GunCenter = iZombieSniperGameApp.GetInstance().m_GunCenter;
		m_IconCenter = iZombieSniperGameApp.GetInstance().m_IconCenter;
		m_GameShop = GameObject.Find("Main Camera").GetComponent<iZombieSniperGameShop>();
		m_UIHelper = helper;
		m_UIManager = uimanager;
		m_v2InitPos = v2CellPos;
		m_v2Position = v2CellPos;
		m_bSingleOrDouble = bSingleOrDouble;
		m_nLayer = 3;
		m_ShopCell = new Hashtable();
	}

	public void Move(float speed, float acc, int dir)
	{
		m_bAnim = true;
		m_fSpeed = speed * 3f;
		m_fAcc = acc * 3f;
		m_nDir = dir;
	}

	public void Update(float deltaTime)
	{
		if (!m_bAnim)
		{
			return;
		}
		m_fSpeed -= m_fAcc * deltaTime;
		if (m_fSpeed <= 5f)
		{
			m_bAnim = false;
		}
		if (m_bAnim)
		{
			m_v2Position.x += m_fSpeed * (float)m_nDir * deltaTime;
			if (m_v2Position.x > m_v2InitPos.x)
			{
				m_v2Position.x = m_v2InitPos.x;
			}
			else if (m_v2Position.x < (float)m_nlen)
			{
				m_v2Position.x = m_nlen;
			}
		}
		else
		{
			m_v2Position.x = Utils.AlignToEveness(m_v2Position.x);
		}
		int num = 0;
		foreach (CellInfo value in m_ShopCell.Values)
		{
			if (value.m_bUsed)
			{
				if (m_bSingleOrDouble)
				{
					value.SetPos(m_v2Position + new Vector2(num * (value.m_nWidth + 4), 0f));
				}
				else
				{
					value.SetPos(m_v2Position + new Vector2(num / 2 * (value.m_nWidth + 4), (num + 1) % 2 * (value.m_nHeight + 4)));
				}
				num++;
			}
		}
	}

	public void Reset()
	{
		m_bAnim = false;
		m_v2Position = m_v2InitPos;
		foreach (CellInfo value in m_ShopCell.Values)
		{
			value.Show(false);
		}
	}

	public void UpdateShop()
	{
		m_nlen = 0;
		int num = 0;
		foreach (CellInfo value in m_ShopCell.Values)
		{
			if (!value.m_bUsed)
			{
				continue;
			}
			value.Show(true);
			if (m_bSingleOrDouble)
			{
				value.SetPos(m_v2Position + new Vector2(num * (value.m_nWidth + 4), 0f));
				m_nlen += value.m_nWidth + 4;
			}
			else
			{
				value.SetPos(m_v2Position + new Vector2(num / 2 * (value.m_nWidth + 4), (num + 1) % 2 * (value.m_nHeight + 4)));
				if (num % 2 == 0)
				{
					m_nlen += value.m_nWidth + 4;
				}
			}
			num++;
		}
		if (m_nlen < 400)
		{
			m_nlen = (int)m_v2InitPos.x;
		}
		else
		{
			m_nlen = (int)m_v2InitPos.x - m_nlen + 400;
		}
	}

	public void PrepareCell(int nCount)
	{
		while (nCount > 0)
		{
			CellInfo freeCellInfo = GetFreeCellInfo();
			freeCellInfo.m_bUsed = true;
			nCount--;
		}
		foreach (CellInfo value in m_ShopCell.Values)
		{
			value.m_bUsed = false;
		}
	}

	public CellInfo GetFreeCellInfo()
	{
		foreach (CellInfo value in m_ShopCell.Values)
		{
			if (!value.m_bUsed)
			{
				return value;
			}
		}
		CellInfo cellInfo2 = new CellInfo(m_UIManager);
		if (cellInfo2 == null)
		{
			return null;
		}
		int num = (cellInfo2.m_nID = m_UIHelper.cur_control_id++);
		if (cellInfo2.m_btnBackground == null)
		{
			cellInfo2.m_btnBackground = new UIClickButton();
			cellInfo2.m_btnBackground.Layer = m_nLayer;
			cellInfo2.m_btnBackground.Id = num;
			m_UIManager.Add(cellInfo2.m_btnBackground);
		}
		if (cellInfo2.m_imgWeaponIcon == null)
		{
			cellInfo2.m_imgWeaponIcon = new UIImage();
			cellInfo2.m_imgWeaponIcon.Layer = m_nLayer;
			cellInfo2.m_imgWeaponIcon.Id = num;
			m_UIManager.Add(cellInfo2.m_imgWeaponIcon);
		}
		if (cellInfo2.m_imgLock == null)
		{
			cellInfo2.m_imgLock = new UIImage();
			cellInfo2.m_imgLock.Layer = m_nLayer;
			cellInfo2.m_imgLock.Id = num;
			m_UIManager.Add(cellInfo2.m_imgLock);
		}
		if (cellInfo2.m_imgBuy == null)
		{
			cellInfo2.m_imgBuy = new UIImage();
			cellInfo2.m_imgBuy.Layer = m_nLayer;
			cellInfo2.m_imgBuy.Id = num;
			m_UIManager.Add(cellInfo2.m_imgBuy);
		}
		if (cellInfo2.m_txtName == null)
		{
			cellInfo2.m_txtName = new UIText();
			cellInfo2.m_txtName.Layer = m_nLayer;
			cellInfo2.m_txtName.Id = num;
			cellInfo2.m_txtName.AlignStyle = UIText.enAlignStyle.right;
			cellInfo2.m_txtName.SetFont(m_UIHelper.m_font_path + ((cellInfo2.m_nHDFactor != 1) ? "037_16" : "037_8"));
			cellInfo2.m_txtName.SetColor(new Color(0.7921569f, 0.5294118f, 7f / 85f, 1f));
			m_UIManager.Add(cellInfo2.m_txtName);
		}
		if (cellInfo2.m_txtReserve == null)
		{
			cellInfo2.m_txtReserve = new UIText();
			cellInfo2.m_txtReserve.Layer = m_nLayer;
			cellInfo2.m_txtReserve.Id = num;
			cellInfo2.m_txtReserve.AlignStyle = UIText.enAlignStyle.right;
			cellInfo2.m_txtReserve.SetFont(m_UIHelper.m_font_path + ((cellInfo2.m_nHDFactor != 1) ? "037_16" : "037_8"));
			cellInfo2.m_txtReserve.SetColor(new Color(0.7921569f, 0.5294118f, 7f / 85f, 1f));
			m_UIManager.Add(cellInfo2.m_txtReserve);
		}
		if (cellInfo2.m_imgNew == null)
		{
			cellInfo2.m_imgNew = new UIImage();
			cellInfo2.m_imgNew.Layer = m_nLayer;
			cellInfo2.m_imgNew.Id = num;
			m_UIManager.Add(cellInfo2.m_imgNew);
		}
		m_ShopCell.Add(num, cellInfo2);
		return cellInfo2;
	}

	public void SetData(PowerUpInfo powerInfo)
	{
		CellInfo freeCellInfo = GetFreeCellInfo();
		if (freeCellInfo != null)
		{
			freeCellInfo.m_bUsed = true;
			Material material = null;
			material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
			if (material != null)
			{
				freeCellInfo.m_btnBackground.SetTexture(UIButtonBase.State.Normal, material, new Rect(2 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, freeCellInfo.m_nWidth * m_GameState.m_nHDFactor, freeCellInfo.m_nHeight * m_GameState.m_nHDFactor));
				freeCellInfo.m_btnBackground.SetTexture(UIButtonBase.State.Pressed, material, new Rect(2 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, freeCellInfo.m_nWidth * m_GameState.m_nHDFactor, freeCellInfo.m_nHeight * m_GameState.m_nHDFactor));
				freeCellInfo.m_btnBackground.SetTexture(UIButtonBase.State.Disabled, material, new Rect(2 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, freeCellInfo.m_nWidth * m_GameState.m_nHDFactor, freeCellInfo.m_nHeight * m_GameState.m_nHDFactor));
			}
			freeCellInfo.m_PowerInfo.m_nType = powerInfo.m_nType;
			freeCellInfo.m_PowerInfo.m_sMaterial = powerInfo.m_sMaterial;
			freeCellInfo.m_PowerInfo.m_sName = powerInfo.m_sName;
			freeCellInfo.m_PowerInfo.m_Rect = powerInfo.m_Rect;
			freeCellInfo.m_PowerInfo.m_bGodPrice = powerInfo.m_bGodPrice;
			freeCellInfo.m_PowerInfo.m_nReserves = powerInfo.m_nReserves;
			freeCellInfo.m_PowerInfo.m_nValue = powerInfo.m_nValue;
			freeCellInfo.m_PowerInfo.m_nReservesMax = powerInfo.m_nReservesMax;
			freeCellInfo.m_PowerInfo.m_sDesc = powerInfo.m_sDesc;
			freeCellInfo.m_PowerInfo.m_bNew = powerInfo.m_bNew;
			material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName(powerInfo.m_sMaterial));
			if (material != null)
			{
				freeCellInfo.m_imgWeaponIcon.SetTexture(material, new Rect(powerInfo.m_Rect.xMin * (float)freeCellInfo.m_nHDFactor, powerInfo.m_Rect.yMin * (float)freeCellInfo.m_nHDFactor, powerInfo.m_Rect.width * (float)freeCellInfo.m_nHDFactor, powerInfo.m_Rect.height * (float)freeCellInfo.m_nHDFactor));
			}
			UpdateCell(freeCellInfo.m_nID, powerInfo.m_nType);
		}
	}

	public void UpdateCell(int nID, PowerUpEnum nType)
	{
		CellInfo shopCellInfo = GetShopCellInfo(nID);
		if (shopCellInfo == null)
		{
			return;
		}
		if (shopCellInfo.m_PowerInfo.m_nType == PowerUpEnum.Telephone_Booth_Bullet)
		{
			shopCellInfo.m_bLock = m_GameState.m_nTowerLvl == 0;
		}
		else
		{
			shopCellInfo.m_bLock = false;
		}
		shopCellInfo.m_bPurchase = !shopCellInfo.m_bLock && ((shopCellInfo.m_PowerInfo.m_nType == PowerUpEnum.Telephone_Booth_Dmg && m_GameState.m_nTowerLvl > 0) || (shopCellInfo.m_PowerInfo.m_nType == PowerUpEnum.Telephone_Booth_Bullet && m_GameState.m_nTowerBullet > 10) || (shopCellInfo.m_PowerInfo.m_nType == PowerUpEnum.LandMine && m_GameState.m_nMineCount > 0) || (shopCellInfo.m_PowerInfo.m_nType == PowerUpEnum.AirStrike && m_GameState.m_nAC130 > 0) || (shopCellInfo.m_PowerInfo.m_nType == PowerUpEnum.InnoKiller && m_GameState.m_nInnoKiller > 0) || (shopCellInfo.m_PowerInfo.m_nType == PowerUpEnum.MachineGun && m_GameState.m_nMachineGun > 0));
		shopCellInfo.m_txtName.SetText(shopCellInfo.m_PowerInfo.m_sName);
		Material material = null;
		Debug.Log(shopCellInfo.m_PowerInfo.m_bNew);
		if (shopCellInfo.m_PowerInfo.m_bNew)
		{
			material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("ready_ui_texture"));
			if (material != null)
			{
				shopCellInfo.m_imgNew.SetTexture(material, new Rect(372 * m_GameState.m_nHDFactor, 112 * m_GameState.m_nHDFactor, 48 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor));
			}
		}
		else
		{
			shopCellInfo.m_imgNew.Visible = false;
		}
		if (shopCellInfo.m_bLock)
		{
			shopCellInfo.m_imgWeaponIcon.SetColor(new Color(1f, 0.1f, 0.1f));
			material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
			if (material != null)
			{
				shopCellInfo.m_imgLock.SetTexture(material, new Rect(90 * m_GameState.m_nHDFactor, 480 * m_GameState.m_nHDFactor, 32 * m_GameState.m_nHDFactor, 28 * m_GameState.m_nHDFactor));
			}
			shopCellInfo.m_txtReserve.Visible = false;
			shopCellInfo.m_imgLock.Visible = true;
			return;
		}
		shopCellInfo.m_imgWeaponIcon.SetColor(new Color(1f, 1f, 1f));
		shopCellInfo.m_txtReserve.SetText(string.Empty);
		switch (nType)
		{
		case PowerUpEnum.Telephone_Booth_Dmg:
			if (m_GameState.m_nTowerLvl > 0)
			{
				shopCellInfo.m_txtReserve.SetText("LV. " + m_GameState.m_nTowerLvl);
			}
			break;
		case PowerUpEnum.Telephone_Booth_Bullet:
			if (m_GameState.m_nTowerBullet > 10)
			{
				shopCellInfo.m_txtReserve.SetText("x " + (m_GameState.m_nTowerBullet - 10));
			}
			break;
		case PowerUpEnum.LandMine:
			if (m_GameState.m_nMineCount > 0)
			{
				shopCellInfo.m_txtReserve.SetText("x " + m_GameState.m_nMineCount);
			}
			break;
		case PowerUpEnum.AirStrike:
			if (m_GameState.m_nAC130 > 0)
			{
				shopCellInfo.m_txtReserve.SetText("x " + m_GameState.m_nAC130);
			}
			break;
		case PowerUpEnum.InnoKiller:
			if (m_GameState.m_nInnoKiller > 0)
			{
				shopCellInfo.m_txtReserve.SetText("x " + m_GameState.m_nInnoKiller);
			}
			break;
		case PowerUpEnum.MachineGun:
			if (m_GameState.m_nMachineGun > 0)
			{
				shopCellInfo.m_txtReserve.SetText("x " + m_GameState.m_nMachineGun);
			}
			break;
		}
		shopCellInfo.m_txtReserve.Visible = true;
		shopCellInfo.m_imgLock.Visible = false;
		if (shopCellInfo.m_bPurchase)
		{
			shopCellInfo.m_imgBuy.Visible = false;
			return;
		}
		material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
		if (material != null)
		{
			shopCellInfo.m_imgBuy.SetTexture(material, new Rect(90 * m_GameState.m_nHDFactor, 450 * m_GameState.m_nHDFactor, 34 * m_GameState.m_nHDFactor, 29 * m_GameState.m_nHDFactor));
		}
		shopCellInfo.m_imgBuy.Visible = true;
	}

	public void Show(bool bShow)
	{
		foreach (CellInfo value in m_ShopCell.Values)
		{
			if (value.m_bUsed)
			{
				value.Show(bShow);
			}
		}
	}

	public CellInfo GetShopCellInfo(int nID)
	{
		if (m_ShopCell.ContainsKey(nID))
		{
			return (CellInfo)m_ShopCell[nID];
		}
		return null;
	}

	public CellInfo GetShopCellInfoByType(PowerUpEnum nType)
	{
		foreach (CellInfo value in m_ShopCell.Values)
		{
			if (value.m_PowerInfo.m_nType == nType)
			{
				return value;
			}
		}
		return null;
	}

	public void SetLayer(int nLayer)
	{
		m_nLayer = nLayer;
	}

	public bool IsRightMore()
	{
		if (m_v2Position.x > (float)m_nlen)
		{
			return true;
		}
		return false;
	}

	public bool IsLeftMore()
	{
		if (m_v2Position.x < m_v2InitPos.x)
		{
			return true;
		}
		return false;
	}
}
