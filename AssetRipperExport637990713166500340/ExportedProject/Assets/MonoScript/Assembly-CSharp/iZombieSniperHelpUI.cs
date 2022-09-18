public class iZombieSniperHelpUI : UIHelper
{
	private int m_nImageIndex;

	private int m_nImageCount;

	private new void Start()
	{
		m_font_path = "ZombieSniper/Fonts/Materials/";
		m_ui_material_path = "ZombieSniper/UI/Materials/";
		m_ui_cfgxml = "ZombieSniper/UI/HelpUI";
		base.Start();
		Initialize();
	}

	private void Initialize()
	{
		m_nImageIndex = 0;
		m_nImageCount = 4;
		((UIClickButton)m_control_table["btnPrev"]).Visible = false;
		((UIClickButton)m_control_table["btnPrev"]).Enable = false;
	}

	private new void Update()
	{
		base.Update();
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (m_UIManagerRef.HandleInput(touch))
			{
			}
		}
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (command == 0)
		{
			if (control.Id == GetControlId("btnPrev"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				ShowPrev();
				UpdateButton();
			}
			else if (control.Id == GetControlId("btnNext"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				ShowNext();
				UpdateButton();
			}
			else if (control.Id == GetControlId("btnBack"))
			{
				iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
				iZombieSniperGameApp.GetInstance().BackScene();
			}
		}
	}

	public void ShowPrev()
	{
		int num = m_nImageIndex - 1;
		if (num >= 0 && num < m_nImageCount)
		{
			m_nImageIndex = num;
			HideAll();
			ShowImage(m_nImageIndex);
		}
	}

	public void ShowNext()
	{
		int num = m_nImageIndex + 1;
		if (num >= 0 && num < m_nImageCount)
		{
			m_nImageIndex = num;
			HideAll();
			ShowImage(m_nImageIndex);
		}
	}

	public void ShowImage(int nIndex)
	{
		string key = "imgHelpContext" + (nIndex + 1);
		if (m_control_table.ContainsKey(key))
		{
			((UIImage)m_control_table[key]).Visible = true;
		}
	}

	public void HideAll()
	{
		for (int i = 0; i < m_nImageCount; i++)
		{
			string key = "imgHelpContext" + (i + 1);
			if (m_control_table.ContainsKey(key))
			{
				((UIImage)m_control_table[key]).Visible = false;
			}
		}
	}

	private void UpdateButton()
	{
		((UIClickButton)m_control_table["btnPrev"]).Visible = true;
		((UIClickButton)m_control_table["btnPrev"]).Enable = true;
		((UIClickButton)m_control_table["btnNext"]).Visible = true;
		((UIClickButton)m_control_table["btnNext"]).Enable = true;
		if (m_nImageIndex == 0)
		{
			((UIClickButton)m_control_table["btnPrev"]).Visible = false;
			((UIClickButton)m_control_table["btnPrev"]).Enable = false;
		}
		if (m_nImageIndex == m_nImageCount - 1)
		{
			((UIClickButton)m_control_table["btnNext"]).Visible = false;
			((UIClickButton)m_control_table["btnNext"]).Enable = false;
		}
	}
}
