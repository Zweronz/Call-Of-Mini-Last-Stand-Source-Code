using UnityEngine;

public class iZombieSniperIAPUI : UIHelper
{
	public iZombieSniperGameState m_GameState;

	public iZombieSniperIAP m_GameIAP;

	public CIAPCell m_IAPCell;

	public CImageAnim m_ArrowLeft;

	public CImageAnim m_ArrowRight;

	public new void Start()
	{
		m_font_path = "ZombieSniper/Fonts/Materials/";
		m_ui_material_path = "ZombieSniper/UI/Materials/";
		m_ui_cfgxml = "ZombieSniper/UI/IAPUI";
		base.Start();
		Initialize();
	}

	public void Initialize()
	{
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_GameIAP = GameObject.Find("Main Camera").GetComponent<iZombieSniperIAP>();
		m_IAPCell = new CIAPCell();
		m_IAPCell.Initialize(this, m_UIManagerRef, new Vector2(100f, (float)Screen.height / 5.35f));
		m_IAPCell.SetLayer(2);
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
		IAPInfo iAPInfo = new IAPInfo();
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(126f, 72f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[0];
		m_IAPCell.SetData(iAPInfo);
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(374f, 142f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[1];
		m_IAPCell.SetData(iAPInfo);
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(2f, 72f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[2];
		m_IAPCell.SetData(iAPInfo);
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(250f, 142f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[3];
		m_IAPCell.SetData(iAPInfo);
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(374f, 2f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[4];
		m_IAPCell.SetData(iAPInfo);
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(126f, 142f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[5];
		m_IAPCell.SetData(iAPInfo);
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(250f, 2f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[6];
		m_IAPCell.SetData(iAPInfo);
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(2f, 142f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[7];
		m_IAPCell.SetData(iAPInfo);
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(126f, 2f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[8];
		m_IAPCell.SetData(iAPInfo);
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(374f, 72f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[9];
		m_IAPCell.SetData(iAPInfo);
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(2f, 2f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[10];
		m_IAPCell.SetData(iAPInfo);
		iAPInfo.m_sMaterial = "shop_ui_part2";
		iAPInfo.m_Rect = new Rect(250f, 72f, 122f, 69f);
		iAPInfo.m_sIAPString = iZombieSniperGameApp.GetInstance().m_sIAPID[11];
		m_IAPCell.SetData(iAPInfo);
		m_IAPCell.Show(true);
		m_IAPCell.UpdateShop();
		SetPlayerGold(m_GameState.m_nPlayerTotalCash);
		SetPlayerGodGold(m_GameState.m_nPlayerTotalGod);
		OpenClikPlugin.Hide();
	}

	public new void Update()
	{
		base.Update();
		m_IAPCell.Update(Time.deltaTime);
		if (m_IAPCell.IsLeftMore())
		{
			m_ArrowLeft.PlayAnim(0.1f, true);
		}
		else
		{
			m_ArrowLeft.StopAnim();
		}
		if (m_IAPCell.IsRightMore())
		{
			m_ArrowRight.PlayAnim(0.1f, true);
		}
		else
		{
			m_ArrowRight.StopAnim();
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

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		Debug.Log("IAPUI command = " + command + " control = " + control.Id);
		if (command != 0)
		{
			return;
		}
		if (control.Id == GetControlId("btnBack"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			if (!m_GameIAP.IsBuying())
			{
				iZombieSniperGameApp.GetInstance().BackScene();
			}
		}
		else if (control.Id == GetControlId("btnPOPConfirm"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			ShowDialog(false, string.Empty);
		}
		else
		{
			if (m_GameIAP.m_bDragMouse)
			{
				return;
			}
			CIAPCell.CellInfo shopCellInfo = m_IAPCell.GetShopCellInfo(control.Id);
			if (shopCellInfo != null)
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					ShowDialog(true, "Connection timed out.");
				}
				else
				{
					m_GameIAP.Buy(shopCellInfo.m_IAPInfo.m_sIAPString);
				}
			}
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

	public UIImage GetUIImage(string sName)
	{
		if (m_control_table.ContainsKey(sName))
		{
			return (UIImage)m_control_table[sName];
		}
		return null;
	}

	public void ShowDialog(bool bShow, string str = "", bool isOK = true)
	{
		((UIImage)m_control_table["imgPOPMask"]).Visible = bShow;
		((UIImage)m_control_table["imgPOPMask"]).Enable = bShow;
		((UIImage)m_control_table["imgPOPBack"]).Visible = bShow;
		((UIText)m_control_table["txtPOPText"]).Visible = bShow;
		((UIText)m_control_table["txtPOPText"]).SetText(str);
		((UIClickButton)m_control_table["btnPOPConfirm"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPOPConfirm"]).Enable = bShow;
		if (!isOK)
		{
			((UIClickButton)m_control_table["btnPOPConfirm"]).Visible = false;
			((UIClickButton)m_control_table["btnPOPConfirm"]).Enable = false;
		}
		m_GameIAP.m_isMask = bShow;
	}
}
