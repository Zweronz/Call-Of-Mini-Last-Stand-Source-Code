using System;
using System.Collections.Generic;
using UnityEngine;

public class iZombieSniperGameApp
{
	protected struct IAPInfo
	{
		public bool iscrystal;

		public int value;

		public IAPInfo(bool iscrystal, int value)
		{
			this.iscrystal = iscrystal;
			this.value = value;
		}
	}

	public enum kIAPMode
	{
		Amazon = 0,
		Android = 1
	}

	private static iZombieSniperGameApp m_Instance;

	public iZombieSniperGameState m_GameState;

	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGunCenter m_GunCenter;

	public iZombieSniperIconCenter m_IconCenter;

	public iZombieSniperGridInfoCenter m_GridInfoCenter;

	public iZombieSniperZombieWaveCenter m_ZombieWaveCenter;

	public iZombieSniperWayPointCenter m_WayPointCenter;

	public CAchievement m_Achievement;

	public CRanking m_Ranking;

	public GameObject m_SoundControllerObject;

	public TAudioController m_SoundController;

	public bool m_bSoundSwitch;

	public CDataCollectManager m_DataCollect;

	public GameObject m_DataCollectObject;

	public GameObject m_LocalNotifyObject;

	public string[] m_sIAPID;

	protected Dictionary<string, IAPInfo> m_dictIAPInfo;

	public kIAPMode m_IAPMode = kIAPMode.Android;

	public bool m_isSupported;

	public static iZombieSniperGameApp GetInstance()
	{
		if (m_Instance == null)
		{
			m_Instance = new iZombieSniperGameApp();
			m_Instance.Initialize();
		}
		return m_Instance;
	}

	public void Initialize()
	{
		m_GunCenter = new iZombieSniperGunCenter();
		m_GunCenter.Initialize();
		m_GameState = new iZombieSniperGameState();
		m_GameState.Initialize();
		m_IconCenter = new iZombieSniperIconCenter();
		m_IconCenter.Initialize();
		m_SoundControllerObject = new GameObject("soundcontroller");
		UnityEngine.Object.DontDestroyOnLoad(m_SoundControllerObject);
		m_SoundController = m_SoundControllerObject.AddComponent<TAudioController>();
		m_bSoundSwitch = true;
		m_LocalNotifyObject = new GameObject("localnotifyobject");
		UnityEngine.Object.DontDestroyOnLoad(m_LocalNotifyObject);
		m_LocalNotifyObject.AddComponent<iZombieSniperLocalNotification>();
		m_Achievement = new CAchievement();
		m_Achievement.Initialize();
		m_Ranking = new CRanking();
		m_Ranking.Initialize();
		m_GridInfoCenter = new iZombieSniperGridInfoCenter();
		m_ZombieWaveCenter = new iZombieSniperZombieWaveCenter();
		m_WayPointCenter = new iZombieSniperWayPointCenter();
		if (m_IAPMode == kIAPMode.Amazon)
		{
			OpenClikPlugin.m_bActive = true;
			OpenClikPlugin.AudriodInit("50f5048a17ba47af0f000023", "377a515e9855af4be016b96676191c0b4473e00f");
		}
		else if (m_IAPMode == kIAPMode.Android)
		{
			OpenClikPlugin.m_bActive = true;
			OpenClikPlugin.AudriodInit("50e6979316ba474e11000005", "2c76e0abd0c4c1d64bbabb6992df2f2e21a74381");
		}
		else
		{
			OpenClikPlugin.m_bActive = false;
		}
		CGameCenter.Initialize();
		XAdManagerWrapper.SetImageAdUrl("http://itunes.apple.com/us/app/isniper-3d-arctic-warfare/id533741523?ls=1&mt=8");
		Screen.sleepTimeout = -1;
		m_GameState.CheckRankAtStart();
		m_GameState.CheckAchiAtStart();
		m_sIAPID = new string[12];
		m_sIAPID[0] = "com.trinitigame.callofminisniper.099cents1";
		m_sIAPID[1] = "com.trinitigame.callofminisniper.099cents2new";
		m_sIAPID[2] = "com.trinitigame.callofminisniper.199cents1";
		m_sIAPID[3] = "com.trinitigame.callofminisniper.199cents2";
		m_sIAPID[4] = "com.trinitigame.callofminisniper.299cents1";
		m_sIAPID[5] = "com.trinitigame.callofminisniper.299cents2";
		m_sIAPID[6] = "com.trinitigame.callofminisniper.499cents1";
		m_sIAPID[7] = "com.trinitigame.callofminisniper.499cents2new";
		m_sIAPID[8] = "com.trinitigame.callofminisniper.999cents1";
		m_sIAPID[9] = "com.trinitigame.callofminisniper.999cents2";
		m_sIAPID[10] = "com.trinitigame.callofminisniper.1999cents1";
		m_sIAPID[11] = "com.trinitigame.callofminisniper.1999cents2";
		m_dictIAPInfo = new Dictionary<string, IAPInfo>();
		m_dictIAPInfo.Add(m_sIAPID[0], new IAPInfo(true, 10));
		m_dictIAPInfo.Add(m_sIAPID[1], new IAPInfo(false, 10000));
		m_dictIAPInfo.Add(m_sIAPID[2], new IAPInfo(true, 21));
		m_dictIAPInfo.Add(m_sIAPID[3], new IAPInfo(false, 21000));
		m_dictIAPInfo.Add(m_sIAPID[4], new IAPInfo(true, 32));
		m_dictIAPInfo.Add(m_sIAPID[5], new IAPInfo(false, 32000));
		m_dictIAPInfo.Add(m_sIAPID[6], new IAPInfo(true, 54));
		m_dictIAPInfo.Add(m_sIAPID[7], new IAPInfo(false, 54000));
		m_dictIAPInfo.Add(m_sIAPID[8], new IAPInfo(true, 110));
		m_dictIAPInfo.Add(m_sIAPID[9], new IAPInfo(false, 110000));
		m_dictIAPInfo.Add(m_sIAPID[10], new IAPInfo(true, 225));
		m_dictIAPInfo.Add(m_sIAPID[11], new IAPInfo(false, 224000));
		if (m_IAPMode == kIAPMode.Android)
		{
			UnityEngine.Object.Instantiate(Resources.Load("ZombieSniper/IAP/IABAndroidListener"), Vector3.zero, Quaternion.identity);
			UnityEngine.Object.Instantiate(Resources.Load("ZombieSniper/IAP/IABAndroidManager"), Vector3.zero, Quaternion.identity);
			IABAndroid.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvfxRqpA+fjKm64VbNaXM6offkWUUsgCRzZlJFJrjZD5MTcX2p2/nfyOYiNDAh9qrS6hoS7MfIvPYirc38oerql/die8eIsW5JtBkeVt2te9+ZCc2BjmOr2b3g+xirbE1bkReJP5JDARHColJA7lQ/6o4J8rvv9L1rGcYynrWeSdTegeBDRkuMPQjgNArMXzkw7hITPdLXhQtBgnn62tV7zvguxKMuYoqzmXpyMsSyyAFVGDQAvI7ITKXvRR+0LL2ybjmP0+0kwLu7NL+nshBm8msjHbCqclsiOwcEkaMFk/Jgqg8B2MLeL7Ff2PJIJA023FnMfPgzNJIde0hj20j6wIDAQAB");
		}
		else if (m_IAPMode == kIAPMode.Amazon)
		{
			UnityEngine.Object.Instantiate(Resources.Load("ZombieSniper/IAP/AmazonIAPListener"), Vector3.zero, Quaternion.identity);
			UnityEngine.Object.Instantiate(Resources.Load("ZombieSniper/IAP/AmazonIAPManager"), Vector3.zero, Quaternion.identity);
			AmazonIAP.initiateItemDataRequest(m_sIAPID);
		}
		Debug.Log("数据初始化完毕");
	}

	public void CreateScene(int nStage)
	{
		m_GridInfoCenter.Initialize();
		m_GridInfoCenter.LoadData(nStage);
		m_ZombieWaveCenter.Initialize();
		m_ZombieWaveCenter.LoadData(nStage);
		m_WayPointCenter.Initialize();
		m_WayPointCenter.LoadWayPoint(nStage);
		switch (nStage)
		{
		case 0:
			m_GameScene = new iZombieSniperGameScene();
			break;
		case 1:
			m_GameScene = new iZombieSniperGameScene1();
			break;
		case 2:
			m_GameScene = new iZombieSniperGameScene2();
			break;
		default:
			m_GameScene = new iZombieSniperGameScene();
			break;
		}
		m_GameScene.Initialize();
		m_GameScene.ResetData();
		m_GameScene.StartGame();
		m_GameState.m_fTimeInGameScene = 0f;
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	public void DestroyScene()
	{
		if (m_GameScene != null)
		{
			if (m_GameScene.m_GameSceneUI != null)
			{
				m_GameScene.m_GameSceneUI.Destroy();
				m_GameScene.m_GameSceneUI = null;
			}
			m_GameScene.Destroy();
			m_GameScene = null;
		}
		if (m_GridInfoCenter != null)
		{
			m_GridInfoCenter.Destroy();
		}
		if (m_ZombieWaveCenter != null)
		{
			m_ZombieWaveCenter.Destroy();
		}
		if (m_WayPointCenter != null)
		{
			m_WayPointCenter.Destroy();
		}
		m_GameState.m_fTimeInGameScene = 0f;
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	public void Loop()
	{
		if (m_GameScene != null)
		{
			m_GameScene.Update(Time.deltaTime);
		}
		if (m_GridInfoCenter != null)
		{
		}
		CGameCenter.Update(Time.deltaTime);
		if (m_Achievement != null)
		{
			m_Achievement.Update(Time.deltaTime);
		}
		if (m_Ranking != null)
		{
			m_Ranking.Update(Time.deltaTime);
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
		}
		if (!Input.GetKeyDown(KeyCode.Alpha2))
		{
		}
	}

	public void OnApplicationPause(bool bPause)
	{
		if (m_GameScene != null && bPause)
		{
			m_GameScene.SetGamePause(true);
		}
		if (m_GameState != null && !bPause && m_GameState.m_nEnterGameCount != -1)
		{
			m_GameState.m_nEnterGameCount++;
		}
	}

	public void OnApplicationQuit()
	{
		if (m_IAPMode != 0 && m_IAPMode == kIAPMode.Android)
		{
			IABAndroid.stopBillingService();
		}
	}

	public string GetKey()
	{
		return "0123456789ABC";
	}

	public void PlayMusic(SceneEnum currScene, SceneEnum scene)
	{
		if (!m_bSoundSwitch)
		{
			m_bSoundSwitch = true;
		}
		else if (scene == SceneEnum.kGame)
		{
			ClearAudio(string.Empty);
		}
		else if (currScene == SceneEnum.kGame)
		{
			ClearAudio("MusicMenu");
			PlayAudio("MusicMenu");
		}
	}

	public void EnterScene(SceneEnum scene, bool bNeedBack = true, int nBackSceneEnum = 0)
	{
		Debug.Log(string.Concat(m_GameState.m_CurrScene, " go to ", scene));
		if (scene != SceneEnum.kMenu)
		{
			XAdManagerWrapper.HideImageAd();
		}
		PlayMusic(m_GameState.m_CurrScene, scene);
		if (m_GameState.m_CurrScene == SceneEnum.kGame)
		{
			DestroyScene();
		}
		if (bNeedBack && m_GameState.m_CurrScene != 0)
		{
			if (nBackSceneEnum != 0)
			{
				m_GameState.m_LastScene = (SceneEnum)nBackSceneEnum;
			}
			else
			{
				m_GameState.m_LastScene = m_GameState.m_CurrScene;
			}
		}
		else
		{
			m_GameState.m_LastScene = SceneEnum.kNone;
		}
		switch (scene)
		{
		case SceneEnum.kGame:
			m_GameState.m_CurrScene = SceneEnum.kLoad;
			m_GameState.m_LoadScene = SceneEnum.kGame;
			Application.LoadLevel("Zombies.SceneLoad");
			break;
		case SceneEnum.kMenu:
			if (m_GameState.m_CurrScene == SceneEnum.kOptions)
			{
				m_GameState.m_CurrScene = SceneEnum.kMenu;
				Application.LoadLevel("Zombies.SceneMenu");
				OpenClikPlugin.Show(false);
			}
			else
			{
				m_GameState.m_CurrScene = SceneEnum.kLoad;
				m_GameState.m_LoadScene = SceneEnum.kMenu;
				Application.LoadLevel("Zombies.SceneLoad");
			}
			break;
		case SceneEnum.kShop:
			m_GameState.m_CurrScene = SceneEnum.kLoad;
			m_GameState.m_LoadScene = SceneEnum.kShop;
			Application.LoadLevel("Zombies.SceneLoad");
			break;
		case SceneEnum.kReady:
			m_GameState.m_CurrScene = SceneEnum.kLoad;
			m_GameState.m_LoadScene = SceneEnum.kReady;
			Application.LoadLevel("Zombies.SceneLoad");
			break;
		case SceneEnum.kOptions:
			m_GameState.m_CurrScene = SceneEnum.kOptions;
			Application.LoadLevel("Zombies.SceneOption");
			OpenClikPlugin.Show(false);
			break;
		case SceneEnum.kCredits:
			m_GameState.m_CurrScene = SceneEnum.kCredits;
			Application.LoadLevel("Zombies.SceneCredits");
			OpenClikPlugin.Show(false);
			break;
		case SceneEnum.kIAP:
			m_GameState.m_CurrScene = SceneEnum.kLoad;
			m_GameState.m_LoadScene = SceneEnum.kIAP;
			Application.LoadLevel("Zombies.SceneLoad");
			break;
		case SceneEnum.kHelp:
			m_GameState.m_CurrScene = SceneEnum.kLoad;
			m_GameState.m_LoadScene = SceneEnum.kHelp;
			Application.LoadLevel("Zombies.SceneLoad");
			break;
		case SceneEnum.kMap:
			m_GameState.m_CurrScene = SceneEnum.kLoad;
			m_GameState.m_LoadScene = SceneEnum.kMap;
			Application.LoadLevel("Zombies.SceneLoad");
			break;
		}
	}

	public void BackScene()
	{
		if (m_GameState.m_LastScene == SceneEnum.kNone)
		{
			EnterScene(SceneEnum.kMap);
		}
		else
		{
			EnterScene(m_GameState.m_LastScene, false);
		}
	}

	public int GetCanBuyCount()
	{
		if (m_GameState == null || m_GunCenter == null)
		{
			return 0;
		}
		int num = 0;
		foreach (iWeaponInfoBase value in m_GunCenter.m_WeaponInfoBase.Values)
		{
			if ((!value.IsConditionInno() || m_GameState.m_nSaveInnoTotalNum >= value.m_nCondValue) && m_GunCenter.GetWeaponData(value.m_nWeaponID) == null && !value.IsMachineGun() && ((value.m_bGodPrice && m_GameState.m_nPlayerTotalGod >= value.m_nPrice) || (!value.m_bGodPrice && m_GameState.m_nPlayerTotalCash >= value.m_nPrice)))
			{
				num++;
			}
		}
		int num2 = 0;
		num2 = GetPrice(PowerUpEnum.Telephone_Booth_Dmg, m_GameState.m_nTowerLvl);
		if (false || m_GameState.m_nPlayerTotalCash >= num2)
		{
			num++;
		}
		num2 = GetPrice(PowerUpEnum.Telephone_Booth_Bullet);
		if (m_GameState.m_nTowerLvl > 0 && (false || m_GameState.m_nPlayerTotalCash >= num2))
		{
			num++;
		}
		num2 = GetPrice(PowerUpEnum.LandMine);
		if (false || m_GameState.m_nPlayerTotalCash >= num2)
		{
			num++;
		}
		num2 = GetPrice(PowerUpEnum.AirStrike);
		if ((true && m_GameState.m_nPlayerTotalGod >= num2) ? true : false)
		{
			num++;
		}
		num2 = GetPrice(PowerUpEnum.InnoKiller);
		if (false || m_GameState.m_nPlayerTotalCash >= num2)
		{
			num++;
		}
		num2 = GetPrice(PowerUpEnum.MachineGun);
		if ((true && m_GameState.m_nPlayerTotalGod >= num2) ? true : false)
		{
			num++;
		}
		return num;
	}

	public int GetCanBuyCount(GameShopStage nStage)
	{
		if (m_GameState == null || m_GunCenter == null)
		{
			return 0;
		}
		int num = 0;
		if (nStage != GameShopStage.DEFENCE)
		{
			foreach (iWeaponInfoBase value in m_GunCenter.m_WeaponInfoBase.Values)
			{
				if ((!value.IsConditionInno() || m_GameState.m_nSaveInnoTotalNum >= value.m_nCondValue) && m_GunCenter.GetWeaponData(value.m_nWeaponID) == null && !value.IsMachineGun() && (!value.IsRifle() || nStage == GameShopStage.SNIPER) && (!value.IsAutoShoot() || nStage == GameShopStage.AUTOSHOOT) && ((!value.IsRocket() && !value.IsThrowMine()) || nStage == GameShopStage.BAZOOKA) && ((value.m_bGodPrice && m_GameState.m_nPlayerTotalGod >= value.m_nPrice) || (!value.m_bGodPrice && m_GameState.m_nPlayerTotalCash >= value.m_nPrice)))
				{
					num++;
				}
			}
			return num;
		}
		int num2 = 0;
		num2 = GetPrice(PowerUpEnum.Telephone_Booth_Dmg, m_GameState.m_nTowerLvl);
		if (false || m_GameState.m_nPlayerTotalCash >= num2)
		{
			num++;
		}
		num2 = GetPrice(PowerUpEnum.Telephone_Booth_Bullet);
		if (m_GameState.m_nTowerLvl > 0 && (false || m_GameState.m_nPlayerTotalCash >= num2))
		{
			num++;
		}
		num2 = GetPrice(PowerUpEnum.LandMine);
		if (false || m_GameState.m_nPlayerTotalCash >= num2)
		{
			num++;
		}
		num2 = GetPrice(PowerUpEnum.AirStrike);
		if ((true && m_GameState.m_nPlayerTotalGod >= num2) ? true : false)
		{
			num++;
		}
		num2 = GetPrice(PowerUpEnum.InnoKiller);
		if (false || m_GameState.m_nPlayerTotalCash >= num2)
		{
			num++;
		}
		num2 = GetPrice(PowerUpEnum.MachineGun);
		if ((true && m_GameState.m_nPlayerTotalGod >= num2) ? true : false)
		{
			num++;
		}
		return num;
	}

	public void PlayAudio(string sName)
	{
		if (!(m_SoundControllerObject == null))
		{
			GameObject gameObject = GameObject.Find("Main Camera");
			if (!(gameObject == null))
			{
				m_SoundControllerObject.transform.position = gameObject.transform.position;
				m_SoundControllerObject.transform.eulerAngles = gameObject.transform.eulerAngles;
				m_SoundController.PlayAudio(sName);
			}
		}
	}

	public void StopAudio(string sName)
	{
		if (!(m_SoundControllerObject == null))
		{
			m_SoundController.StopAudio(sName);
		}
	}

	public void ClearAudio(string sExcept = "")
	{
		Debug.Log("clearaudio except " + sExcept);
		if (m_SoundControllerObject == null)
		{
			return;
		}
		Transform transform = m_SoundControllerObject.transform.Find("Audio");
		if (transform == null)
		{
			return;
		}
		Debug.Log("ClearAudio");
		foreach (Transform item in transform)
		{
			if (!(item.name == "UIClickGeneral") && sExcept != item.name)
			{
				StopAudio(item.name);
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}

	public int GetPrice(PowerUpEnum nType, int nReserves = 0)
	{
		switch (nType)
		{
		case PowerUpEnum.Telephone_Booth_Dmg:
			switch (nReserves)
			{
			case 0:
				return 2000;
			case 1:
				return 3000;
			case 2:
				return 4000;
			case 3:
				return 5000;
			case 4:
				return 8000;
			}
			break;
		case PowerUpEnum.Telephone_Booth_Bullet:
			return 1000;
		case PowerUpEnum.LandMine:
			return 1000;
		case PowerUpEnum.AirStrike:
			return 1;
		case PowerUpEnum.InnoKiller:
			return 1000;
		case PowerUpEnum.MachineGun:
			return 2;
		}
		return 99999999;
	}

	protected bool GetIAPInfo(string sID, ref bool iscrystal, ref int value)
	{
		if (m_dictIAPInfo == null || !m_dictIAPInfo.ContainsKey(sID))
		{
			return false;
		}
		IAPInfo iAPInfo = m_dictIAPInfo[sID];
		iscrystal = iAPInfo.iscrystal;
		value = iAPInfo.value;
		return true;
	}

	public bool Purchase(string sID)
	{
		if (!m_isSupported)
		{
			return false;
		}
		if (m_IAPMode == kIAPMode.Amazon)
		{
			AmazonIAP.initiatePurchaseRequest(sID);
		}
		else if (m_IAPMode == kIAPMode.Android)
		{
			IABAndroid.purchaseProduct(sID);
		}
		return true;
	}

	public void OnPurchaseSuccess(string sID)
	{
		if (m_GameState == null)
		{
			return;
		}
		bool iscrystal = false;
		int value = 0;
		if (GetIAPInfo(sID, ref iscrystal, ref value))
		{
			if (iscrystal)
			{
				m_GameState.m_nPlayerTotalGod += value;
			}
			else
			{
				m_GameState.m_nPlayerTotalCash += value;
			}
			m_GameState.SaveData();
		}
	}
}
