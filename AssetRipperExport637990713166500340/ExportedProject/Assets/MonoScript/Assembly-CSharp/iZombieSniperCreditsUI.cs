public class iZombieSniperCreditsUI : UIHelper
{
	public new void Start()
	{
		m_font_path = "ZombieSniper/Fonts/Materials/";
		m_ui_material_path = "ZombieSniper/UI/Materials/";
		m_ui_cfgxml = "ZombieSniper/UI/CreditsUI";
		base.Start();
	}

	public new void Update()
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
		if (command == 0 && control.Id == GetControlId("btnBack"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kOptions);
		}
	}
}
