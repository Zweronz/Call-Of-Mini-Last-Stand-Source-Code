using UnityEngine;

public class iZombieSniperLoadUI : UIHelper
{
	private int n;

	public iZombieSniperGameState m_GameState;

	private string[] m_sLoadTipList = new string[12]
	{
		"Tips: See those cement and fuel trucks? \nShoot them to bail yourself out \nof tough spots!", "Tips: When the siren sounds, the\n chapel's about to be overrun! ", "Tips: If a single zombie gets into\n the safehouse, it's game over!", "Tips: Headshots do double damage!", "Tips: Use a rifle for rapid fire!", "Tips: Snipers excel at taking out\n zombies from afar!", "Tips: Rifles are great for mowing\n down nearby zombies!", "Tips: Switch weapons while reloading\n and never miss a kill!", "Tips: In the shop, the numbers in\n the upper right tell you how\n many of each item you can afford.", "Tips: Once you've got the tCoins,\n buy a Battle Booth for backup!",
		"Tips: You can use Armageddon once\n per game to annihilate\n everything that moves!", "Tips: Buy Explosives to make sure\n the first zombie to reach\n the safehouse never gets inside!"
	};

	protected float m_fTime;

	protected float m_fTimeCount;

	public new void Start()
	{
		m_font_path = "ZombieSniper/Fonts/Materials/";
		m_ui_material_path = "ZombieSniper/UI/Materials/";
		m_ui_cfgxml = "ZombieSniper/UI/LoadingUI";
		base.Start();
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		Resources.UnloadUnusedAssets();
		ShowTip(UnityEngine.Random.Range(0, m_sLoadTipList.Length));
		Initialize();
		OpenClikPlugin.Show(false);
	}

	public void Initialize()
	{
		switch (m_GameState.m_LoadScene)
		{
		case SceneEnum.kGame:
			m_GameState.m_CurrScene = SceneEnum.kGame;
			switch (m_GameState.m_nCurStage)
			{
			case 0:
				Application.LoadLevelAsync("Zombies.SceneGame");
				break;
			case 1:
				Application.LoadLevelAsync("Zombies.SceneGame1");
				break;
			case 2:
				Application.LoadLevelAsync("Zombies.SceneGame2");
				break;
			}
			break;
		case SceneEnum.kMenu:
			m_GameState.m_CurrScene = SceneEnum.kMenu;
			Application.LoadLevelAsync("Zombies.SceneMenu");
			break;
		case SceneEnum.kShop:
			m_GameState.m_CurrScene = SceneEnum.kShop;
			Application.LoadLevelAsync("Zombies.SceneShop");
			break;
		case SceneEnum.kReady:
			m_GameState.m_CurrScene = SceneEnum.kReady;
			Application.LoadLevelAsync("Zombies.SceneReady");
			break;
		case SceneEnum.kIAP:
			m_GameState.m_CurrScene = SceneEnum.kIAP;
			Application.LoadLevelAsync("Zombies.SceneIAP");
			break;
		case SceneEnum.kHelp:
			m_GameState.m_CurrScene = SceneEnum.kHelp;
			Application.LoadLevelAsync("Zombies.SceneHelp");
			break;
		case SceneEnum.kMap:
			m_GameState.m_CurrScene = SceneEnum.kMap;
			Application.LoadLevelAsync("Zombies.SceneMap");
			break;
		}
	}

	public new void Update()
	{
		base.Update();
		if (m_GameState.m_bFirstLoading)
		{
			m_fTimeCount += Time.deltaTime;
			if (!(m_fTimeCount < m_fTime))
			{
				m_GameState.m_bFirstLoading = false;
				Initialize();
			}
		}
	}

	private void ShowTip(int nIndex)
	{
		if (nIndex >= 0 && nIndex < m_sLoadTipList.Length)
		{
			((UIText)m_control_table["txtLoading"]).SetText(m_sLoadTipList[nIndex]);
		}
	}
}
