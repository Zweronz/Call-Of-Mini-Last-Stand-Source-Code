using System.Collections;
using UnityEngine;

public class CShopCell
{
	public class CellInfo
	{
		public int m_nID;

		public UIManager m_UIManager;

		public iWeaponInfoBase m_WeaponInfo;

		public bool m_bUsed;

		public UIClickButton m_btnBackground;

		public Vector2 m_v2Background;

		public UIImage m_imgLock;

		public Vector2 m_v2Lock;

		public UIImage m_imgWeaponIcon;

		public Vector2 m_v2WeaponIcon;

		public UIImage m_imgPriceIcon;

		public Vector2 m_v2PriceIcon;

		public UIText m_txtPrice;

		public Vector2 m_v2Price;

		public UIImage m_imgBuy;

		public Vector2 m_v2Buy;

		public UIImage m_imgEquip;

		public Vector2 m_v2Equip;

		public UIImage m_imgNew;

		public Vector2 m_v2New;

		public UIImage m_imgNewWeapon;

		public Vector2 m_v2NewWeapon;

		public bool m_bLock;

		public bool m_bPurchase;

		public bool m_bEquip;

		public bool m_bNew;

		public bool m_bNewWeapon;

		public int m_nWidth;

		public int m_nHeight;

		public int m_nHDFactor;

		public CellInfo(UIManager uimanager)
		{
			m_bUsed = false;
			m_UIManager = uimanager;
			m_bLock = false;
			m_bPurchase = false;
			m_bEquip = false;
			m_bNew = false;
			m_bNewWeapon = false;
			m_nWidth = 133;
			m_nHeight = 80;
			m_nHDFactor = ((!Utils.IsRetina()) ? 1 : 2);
			m_v2Lock = new Vector2(-m_nWidth / 2 + 16, m_nHeight / 2 - 14);
			m_v2Buy = new Vector2(-m_nWidth / 2 + 16, m_nHeight / 2 - 14);
			m_v2Equip = new Vector2(-m_nWidth / 2 + 16, m_nHeight / 2 - 14);
			m_v2New = new Vector2(-m_nWidth / 2 + 105, m_nHeight / 2 - 14);
			m_v2WeaponIcon = new Vector2(0f, 0f);
			m_v2PriceIcon = new Vector2(0f, 14f);
			m_v2Price = new Vector2(-10f, 6f);
			m_v2NewWeapon = new Vector2(-m_nWidth / 2 + 28, -m_nHeight / 2 + 14);
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
			if (m_imgEquip != null)
			{
				if (bShow && m_bEquip)
				{
					m_imgEquip.Visible = true;
				}
				else
				{
					m_imgEquip.Visible = false;
				}
			}
			if (m_imgNew != null)
			{
				if (bShow && m_bNew)
				{
					m_imgNew.Visible = true;
				}
				else
				{
					m_imgNew.Visible = false;
				}
			}
			if (m_imgNewWeapon != null)
			{
				if (bShow && m_bNewWeapon)
				{
					m_imgNewWeapon.Visible = true;
				}
				else
				{
					m_imgNewWeapon.Visible = false;
				}
			}
			if (m_imgPriceIcon != null)
			{
				if (bShow && !m_bPurchase)
				{
					m_imgPriceIcon.Visible = true;
				}
				else
				{
					m_imgPriceIcon.Visible = false;
				}
			}
			if (m_txtPrice != null)
			{
				m_txtPrice.Visible = bShow;
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
			if (m_imgEquip != null)
			{
				m_imgEquip.Layer = nLayer;
				m_UIManager.Remove(m_imgEquip);
				m_UIManager.Add(m_imgEquip);
			}
			if (m_imgNew != null)
			{
				m_imgNew.Layer = nLayer;
				m_UIManager.Remove(m_imgNew);
				m_UIManager.Add(m_imgNew);
			}
			if (m_imgNewWeapon != null)
			{
				m_imgNewWeapon.Layer = nLayer;
				m_UIManager.Remove(m_imgNewWeapon);
				m_UIManager.Add(m_imgNewWeapon);
			}
			if (m_imgPriceIcon != null)
			{
				m_imgPriceIcon.Layer = nLayer;
				m_UIManager.Remove(m_imgPriceIcon);
				m_UIManager.Add(m_imgPriceIcon);
			}
			if (m_txtPrice != null)
			{
				m_txtPrice.Layer = nLayer;
				m_UIManager.Remove(m_txtPrice);
				m_UIManager.Add(m_txtPrice);
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
			if (m_imgEquip != null)
			{
				m_imgEquip.SetPosition((v2Pos + m_v2Equip) * m_nHDFactor);
			}
			if (m_imgNew != null)
			{
				m_imgNew.SetPosition((v2Pos + m_v2New) * m_nHDFactor);
			}
			if (m_imgNewWeapon != null)
			{
				m_imgNewWeapon.SetPosition((v2Pos + m_v2NewWeapon) * m_nHDFactor);
			}
			if (m_imgPriceIcon != null)
			{
				Vector2 position = default(Vector2);
				position.y = v2Pos.y + m_v2PriceIcon.y - (float)(m_nHeight / 2);
				position.x = v2Pos.x + m_v2Price.x + (float)(m_nWidth / 2) - m_v2PriceIcon.x;
				position *= (float)m_nHDFactor;
				m_imgPriceIcon.SetPosition(position);
			}
			if (m_txtPrice != null)
			{
				m_txtPrice.Rect = new Rect((v2Pos.x + m_v2Price.x + (float)(m_nWidth / 2) - 100f) * (float)m_nHDFactor, (v2Pos.y + m_v2Price.y - (float)(m_nHeight / 2)) * (float)m_nHDFactor, 100 * m_nHDFactor, 15 * m_nHDFactor);
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

	public ArrayList m_WeaponList;

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
		m_ShopCell.Clear();
		m_WeaponList = new ArrayList();
		m_WeaponList.Clear();
		Debug.Log(m_v2Position);
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
		m_WeaponList.Clear();
		foreach (CellInfo value in m_ShopCell.Values)
		{
			value.m_bUsed = false;
			value.m_bLock = false;
			value.m_bPurchase = false;
			value.m_bEquip = false;
			value.m_bNew = false;
			value.m_bNewWeapon = false;
			value.Show(false);
		}
	}

	public void AddWeapon(int nWeaponID)
	{
		m_WeaponList.Add(nWeaponID);
	}

	public void UpdateShop()
	{
		m_nlen = 0;
		PrepareCell(m_WeaponList.Count);
		foreach (int weapon in m_WeaponList)
		{
			CellInfo freeCellInfo = GetFreeCellInfo();
			if (freeCellInfo == null)
			{
				return;
			}
			SetData(freeCellInfo, weapon);
			freeCellInfo.m_bUsed = true;
			freeCellInfo.Show(true);
		}
		int num = 0;
		foreach (CellInfo value in m_ShopCell.Values)
		{
			if (!value.m_bUsed)
			{
				continue;
			}
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
		if (cellInfo2.m_imgEquip == null)
		{
			cellInfo2.m_imgEquip = new UIImage();
			cellInfo2.m_imgEquip.Layer = m_nLayer;
			cellInfo2.m_imgEquip.Id = num;
			m_UIManager.Add(cellInfo2.m_imgEquip);
		}
		if (cellInfo2.m_imgNew == null)
		{
			cellInfo2.m_imgNew = new UIImage();
			cellInfo2.m_imgNew.Layer = m_nLayer;
			cellInfo2.m_imgNew.Id = num;
			m_UIManager.Add(cellInfo2.m_imgNew);
		}
		if (cellInfo2.m_imgNewWeapon == null)
		{
			cellInfo2.m_imgNewWeapon = new UIImage();
			cellInfo2.m_imgNewWeapon.Layer = m_nLayer;
			cellInfo2.m_imgNewWeapon.Id = num;
			m_UIManager.Add(cellInfo2.m_imgNewWeapon);
		}
		if (cellInfo2.m_imgPriceIcon == null)
		{
			cellInfo2.m_imgPriceIcon = new UIImage();
			cellInfo2.m_imgPriceIcon.Layer = m_nLayer;
			cellInfo2.m_imgPriceIcon.Id = num;
			m_UIManager.Add(cellInfo2.m_imgPriceIcon);
		}
		if (cellInfo2.m_txtPrice == null)
		{
			cellInfo2.m_txtPrice = new UIText();
			cellInfo2.m_txtPrice.Layer = m_nLayer;
			cellInfo2.m_txtPrice.Id = num;
			cellInfo2.m_txtPrice.AlignStyle = UIText.enAlignStyle.right;
			cellInfo2.m_txtPrice.SetFont(m_UIHelper.m_font_path + ((cellInfo2.m_nHDFactor != 1) ? "037_16" : "037_8"));
			cellInfo2.m_txtPrice.SetColor(new Color(0.7921569f, 0.5294118f, 7f / 85f, 1f));
			m_UIManager.Add(cellInfo2.m_txtPrice);
		}
		m_ShopCell.Add(num, cellInfo2);
		return cellInfo2;
	}

	public void SetData(CellInfo cellInfo, int nWeaponID)
	{
		iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(nWeaponID);
		if (weaponInfoBase == null)
		{
			return;
		}
		cellInfo.m_WeaponInfo = weaponInfoBase;
		Material material = null;
		material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
		if (material != null)
		{
			cellInfo.m_btnBackground.SetTexture(UIButtonBase.State.Normal, material, new Rect(2 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, cellInfo.m_nWidth * m_GameState.m_nHDFactor, cellInfo.m_nHeight * m_GameState.m_nHDFactor));
			cellInfo.m_btnBackground.SetTexture(UIButtonBase.State.Pressed, material, new Rect(2 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, cellInfo.m_nWidth * m_GameState.m_nHDFactor, cellInfo.m_nHeight * m_GameState.m_nHDFactor));
			cellInfo.m_btnBackground.SetTexture(UIButtonBase.State.Disabled, material, new Rect(2 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, cellInfo.m_nWidth * m_GameState.m_nHDFactor, cellInfo.m_nHeight * m_GameState.m_nHDFactor));
		}
		IconInfo icon = m_IconCenter.GetIcon(weaponInfoBase.m_nIconID);
		if (icon != null)
		{
			material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName(icon.m_sMaterial));
			if (material != null)
			{
				cellInfo.m_imgWeaponIcon.SetTexture(material, new Rect(icon.m_Rect.xMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.yMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.width * (float)m_GameState.m_nHDFactor, icon.m_Rect.height * (float)m_GameState.m_nHDFactor), new Vector2(Utils.AlignToEveness(icon.m_Rect.width * (float)m_GameState.m_nHDFactor), Utils.AlignToEveness(icon.m_Rect.height * (float)m_GameState.m_nHDFactor)));
			}
		}
		UpdateCell(cellInfo.m_nID, nWeaponID);
	}

	public void UpdateCell(int nID, int nWeaponID)
	{
		iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(nWeaponID);
		if (weaponInfoBase == null)
		{
			return;
		}
		CellInfo shopCellInfo = GetShopCellInfo(nID);
		if (shopCellInfo == null)
		{
			return;
		}
		iWeaponData weaponData = m_GunCenter.GetWeaponData(weaponInfoBase.m_nWeaponID);
		shopCellInfo.m_bPurchase = weaponData != null;
		shopCellInfo.m_bLock = !shopCellInfo.m_bPurchase && weaponInfoBase.IsConditionInno() && m_GameState.m_nSaveInnoTotalNum < weaponInfoBase.m_nCondValue;
		shopCellInfo.m_bEquip = m_GameState.GetCarryIndexByID(weaponInfoBase.m_nWeaponID) != -1;
		shopCellInfo.m_bNew = weaponData != null && weaponData.m_bNew;
		shopCellInfo.m_bNewWeapon = weaponInfoBase.IsNewWeapon();
		Material material = null;
		if (shopCellInfo.m_bNewWeapon)
		{
			material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("ready_ui_texture"));
			if (material != null)
			{
				shopCellInfo.m_imgNewWeapon.SetTexture(material, new Rect(372 * m_GameState.m_nHDFactor, 112 * m_GameState.m_nHDFactor, 48 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor));
			}
			shopCellInfo.m_imgNewWeapon.Visible = true;
		}
		else
		{
			shopCellInfo.m_imgNewWeapon.Visible = false;
		}
		if (shopCellInfo.m_bLock)
		{
			shopCellInfo.m_imgWeaponIcon.SetColor(new Color(1f, 0.1f, 0.1f));
			material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
			if (material != null)
			{
				shopCellInfo.m_imgLock.SetTexture(material, new Rect(90 * m_GameState.m_nHDFactor, 480 * m_GameState.m_nHDFactor, 32 * m_GameState.m_nHDFactor, 28 * m_GameState.m_nHDFactor));
			}
			string text = Utils.PriceToString(weaponInfoBase.m_nCondValue);
			shopCellInfo.m_txtPrice.SetText(text);
			shopCellInfo.m_v2PriceIcon.x = FixWidth(text);
			material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3"));
			if (material != null)
			{
				shopCellInfo.m_imgPriceIcon.SetTexture(material, new Rect(484 * m_GameState.m_nHDFactor, 16 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor));
			}
			shopCellInfo.m_imgPriceIcon.Visible = true;
			shopCellInfo.m_txtPrice.Visible = true;
			shopCellInfo.m_imgLock.Visible = true;
			return;
		}
		shopCellInfo.m_imgWeaponIcon.SetColor(new Color(1f, 1f, 1f));
		if (shopCellInfo.m_bPurchase)
		{
			shopCellInfo.m_imgPriceIcon.Visible = false;
			shopCellInfo.m_txtPrice.SetText(weaponInfoBase.m_sName);
			shopCellInfo.m_txtPrice.Visible = true;
			if (shopCellInfo.m_bEquip && m_bSingleOrDouble)
			{
				material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("ready_ui_texture"));
				if (material != null)
				{
					shopCellInfo.m_imgEquip.SetTexture(material, new Rect(336 * m_GameState.m_nHDFactor, 112 * m_GameState.m_nHDFactor, 35 * m_GameState.m_nHDFactor, 32 * m_GameState.m_nHDFactor));
				}
				shopCellInfo.m_imgEquip.Visible = true;
			}
			else
			{
				shopCellInfo.m_imgEquip.Visible = false;
			}
			if (shopCellInfo.m_bNew && m_bSingleOrDouble)
			{
				material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("ready_ui_texture"));
				if (material != null)
				{
					shopCellInfo.m_imgNew.SetTexture(material, new Rect(372 * m_GameState.m_nHDFactor, 112 * m_GameState.m_nHDFactor, 48 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor));
				}
				shopCellInfo.m_imgNew.Visible = true;
			}
			else
			{
				shopCellInfo.m_imgNew.Visible = false;
			}
			shopCellInfo.m_imgBuy.Visible = false;
		}
		else
		{
			string text2 = Utils.PriceToString(weaponInfoBase.m_nPrice);
			shopCellInfo.m_txtPrice.SetText(text2);
			shopCellInfo.m_v2PriceIcon.x = FixWidth(text2);
			if (weaponInfoBase.m_bGodPrice)
			{
				material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("crystal"));
				if (material != null)
				{
					shopCellInfo.m_imgPriceIcon.SetTexture(material, new Rect(0f, 0f, 9 * m_GameState.m_nHDFactor, 13 * m_GameState.m_nHDFactor));
				}
			}
			else
			{
				material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("game over_jbi"));
				if (material != null)
				{
					shopCellInfo.m_imgPriceIcon.SetTexture(material, new Rect(0f, 0f, 12 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor));
				}
			}
			shopCellInfo.m_imgPriceIcon.Visible = true;
			shopCellInfo.m_txtPrice.Visible = true;
			material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName("shop_ui_part1"));
			if (material != null)
			{
				shopCellInfo.m_imgBuy.SetTexture(material, new Rect(90 * m_GameState.m_nHDFactor, 450 * m_GameState.m_nHDFactor, 34 * m_GameState.m_nHDFactor, 29 * m_GameState.m_nHDFactor));
			}
			shopCellInfo.m_imgBuy.Visible = true;
		}
		shopCellInfo.m_imgLock.Visible = false;
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

	public CellInfo GetShopCellInfoByWeaponID(int nWeaponID)
	{
		foreach (CellInfo value in m_ShopCell.Values)
		{
			if (value.m_WeaponInfo != null && value.m_WeaponInfo.m_nWeaponID == nWeaponID)
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

	public float FixWidth(string text)
	{
		switch (text.Length)
		{
		case 1:
			return 10f;
		case 2:
			return 24f;
		case 3:
			return 32f;
		case 4:
			return 38f;
		default:
			return 50f;
		}
	}
}
