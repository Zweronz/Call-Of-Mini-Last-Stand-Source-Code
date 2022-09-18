using System.Collections;
using UnityEngine;

public class CIAPCell
{
	public class CellInfo
	{
		public int m_nID;

		public UIManager m_UIManager;

		public bool m_bUsed;

		public UIClickButton m_btnBackground;

		public Vector2 m_v2Background;

		public UIImage m_imgIcon;

		public Vector2 m_v2Icon;

		public int m_nWidth;

		public int m_nHeight;

		public int m_nHDFactor;

		public IAPInfo m_IAPInfo;

		public CellInfo(UIManager uimanager)
		{
			m_bUsed = false;
			m_nWidth = 133;
			m_nHeight = 80;
			m_nHDFactor = ((!Utils.IsRetina()) ? 1 : 2);
			m_IAPInfo = new IAPInfo();
			m_v2Icon = Vector2.zero;
		}

		public void Show(bool bShow)
		{
			if (m_btnBackground != null)
			{
				m_btnBackground.Visible = bShow;
				m_btnBackground.Enable = bShow;
			}
			if (m_imgIcon != null)
			{
				m_imgIcon.Visible = bShow;
				m_imgIcon.Enable = bShow;
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
			if (m_imgIcon != null)
			{
				m_imgIcon.Layer = nLayer;
				m_UIManager.Remove(m_imgIcon);
				m_UIManager.Add(m_imgIcon);
			}
		}

		public void SetPos(Vector2 v2Pos)
		{
			m_v2Background = v2Pos;
			if (m_btnBackground != null)
			{
				m_btnBackground.Rect = new Rect((v2Pos.x - (float)(m_nWidth / 2)) * (float)m_nHDFactor, (v2Pos.y - (float)(m_nHeight / 2)) * (float)m_nHDFactor, m_nWidth * m_nHDFactor, m_nHeight * m_nHDFactor);
			}
			if (m_imgIcon != null)
			{
				m_imgIcon.SetPosition((v2Pos + m_v2Icon) * m_nHDFactor);
			}
		}
	}

	public UIManager m_UIManager;

	public iZombieSniperGameState m_GameState;

	public UIHelper m_UIHelper;

	public Hashtable m_ShopCell;

	public Vector2 m_v2Position = Vector2.zero;

	public Vector2 m_v2InitPos = Vector2.zero;

	public int m_nLayer;

	public int m_nlen;

	public bool m_bAnim;

	public float m_fSpeed;

	public float m_fAcc;

	public int m_nDir;

	public void Initialize(UIHelper helper, UIManager uimanager, Vector2 v2CellPos)
	{
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_UIHelper = helper;
		m_UIManager = uimanager;
		m_v2InitPos = v2CellPos;
		m_v2Position = v2CellPos;
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
				value.SetPos(m_v2Position + new Vector2(num / 2 * (value.m_nWidth + 4), (num + 1) % 2 * (value.m_nHeight + 4)));
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
			if (value.m_bUsed)
			{
				value.Show(true);
				value.SetPos(m_v2Position + new Vector2(num / 2 * (value.m_nWidth + 4), (num + 1) % 2 * (value.m_nHeight + 4)));
				m_nlen += num % 2 * (value.m_nWidth + 4);
				num++;
			}
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
		if (cellInfo2.m_imgIcon == null)
		{
			cellInfo2.m_imgIcon = new UIImage();
			cellInfo2.m_imgIcon.Layer = m_nLayer;
			cellInfo2.m_imgIcon.Id = num;
			m_UIManager.Add(cellInfo2.m_imgIcon);
		}
		m_ShopCell.Add(num, cellInfo2);
		return cellInfo2;
	}

	public void SetData(IAPInfo iapInfo)
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
			freeCellInfo.m_IAPInfo.m_sIAPString = iapInfo.m_sIAPString;
			freeCellInfo.m_IAPInfo.m_sMaterial = iapInfo.m_sMaterial;
			freeCellInfo.m_IAPInfo.m_Rect = iapInfo.m_Rect;
			material = m_UIHelper.LoadUIMaterial(Utils.AutoMaterialName(iapInfo.m_sMaterial));
			if (material != null)
			{
				freeCellInfo.m_imgIcon.SetTexture(material, new Rect(iapInfo.m_Rect.xMin * (float)freeCellInfo.m_nHDFactor, iapInfo.m_Rect.yMin * (float)freeCellInfo.m_nHDFactor, iapInfo.m_Rect.width * (float)freeCellInfo.m_nHDFactor, iapInfo.m_Rect.height * (float)freeCellInfo.m_nHDFactor));
			}
		}
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
