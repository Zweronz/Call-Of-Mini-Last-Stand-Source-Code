using System.Collections;
using UnityEngine;

public class iZombieSniperGameScene : iZombieSniperGameSceneBase
{
	public enum MovieState
	{
		kMovieSTEP1 = 0,
		kMovieSTEP2 = 1,
		kMovieSTEP3 = 2,
		kMovieSTEP4 = 3,
		kMovieSTEP5 = 4,
		kMovieEnd = 5
	}

	public GameObject m_MainChar;

	public iZombieSniperGameHelp m_GameHelp;

	public GameObject m_Bunker;

	public iZombieSniperBunker m_iBunkerScript;

	public iZombieSniperTruck m_iTruckScript;

	public GameObject m_SearchLight1;

	public GameObject m_SearchLight2;

	public GameObject m_SearchLightL1_1;

	public GameObject m_SearchLightL1_2;

	public GameObject m_SearchLightL2_1;

	public GameObject m_SearchLightL2_2;

	public GameObject m_SearchLightP1;

	public GameObject m_SearchLightP2;

	public GameObject m_Ahhhh;

	public ArrayList m_UnFireList;

	public ArrayList m_FireList;

	public GameObject m_GroundFire;

	public GameObject m_TNT;

	public GameObject m_Container;

	public GameObject m_ContainerDestroy;

	public GameObject m_ContainerAnim;

	public GameObject m_Truck;

	public GameObject m_TruckDestroy;

	public GameObject m_Lid;

	public GameObject m_LidDestroy;

	public GameObject m_LidAnim;

	public bool m_bContainerAnim;

	public bool m_bTruckBoom;

	public bool m_bLidAnim;

	public Vector3 m_v3TruckPos = Vector3.zero;

	private GameObject m_TruckFire;

	private ParticleEmitter m_Emitter1;

	private ParticleEmitter m_Emitter2;

	private ParticleEmitter m_Emitter3;

	private float m_fSmokeCount;

	public iZombieSniperMoonWalk m_MoonWalk;

	public MovieState m_MovieState;

	private float m_fMovieTime;

	private bool m_bMovieKillZombie;

	private bool m_bZombieSound;

	private bool m_bInnoSound;

	private bool m_bIntroSound;

	public Vector2 m_v2TouchPos = Vector2.zero;

	private iZombieSniperNpc m_MovieZombie;

	private iZombieSniperNpc m_MovieInno;

	public int m_nTurtorialNPC;

	private float m_fTempCount;

	public float m_fContainerAnimTime;

	public float m_fLidAnimTime;

	public float m_fTruckAnimTime;

	public float m_fSlowTimeRate = 0.1f;

	public float m_fAnimStopVoice;

	public override void Initialize()
	{
		if (m_MainChar == null)
		{
			m_MainChar = GameObject.Find("MainChar");
		}
		if (m_GameHelp == null)
		{
			m_GameHelp = new iZombieSniperGameHelp();
		}
		base.Initialize();
		m_MainChar.SetActiveRecursively(false);
		m_GameHelp.Initialize();
		if (m_iBunkerScript == null)
		{
			m_Bunker = GameObject.Find("Bunker");
			if (m_Bunker != null)
			{
				m_iBunkerScript = m_Bunker.GetComponent<iZombieSniperBunker>();
			}
		}
		if (m_iBunkerScript != null)
		{
			m_iBunkerScript.Initialize(0.6f + 0.35f * (float)(m_GameState.m_nTowerLvl - 1), m_GameState.m_nTowerBullet, 0.3f, 10f);
		}
		if (m_iTruckScript == null)
		{
			GameObject gameObject = GameObject.Find("truck");
			m_iTruckScript = gameObject.GetComponent<iZombieSniperTruck>();
		}
		m_iTruckScript.Initialize(20f);
		if (m_SearchLight1 == null)
		{
			m_SearchLight1 = GameObject.Find("searchlight_01");
			Transform transform = null;
			transform = m_SearchLight1.transform.Find("light_01");
			if (transform != null)
			{
				m_SearchLightL1_1 = transform.gameObject;
				m_SearchLightL1_1.active = false;
			}
			transform = m_SearchLight1.transform.Find("light_02");
			if (transform != null)
			{
				m_SearchLightL1_2 = transform.gameObject;
				m_SearchLightL1_2.active = false;
			}
			if (m_SearchLightP1 == null)
			{
				m_SearchLightP1 = GameObject.Find("searchlight_effect_01");
				m_SearchLightP1.active = false;
			}
		}
		if (m_SearchLight2 == null)
		{
			m_SearchLight2 = GameObject.Find("searchlight_02");
			Transform transform2 = null;
			transform2 = m_SearchLight2.transform.Find("light_01");
			if (transform2 != null)
			{
				m_SearchLightL2_1 = transform2.gameObject;
				m_SearchLightL2_1.active = false;
			}
			transform2 = m_SearchLight2.transform.Find("light_02");
			if (transform2 != null)
			{
				m_SearchLightL2_2 = transform2.gameObject;
				m_SearchLightL2_2.active = false;
			}
			if (m_SearchLightP2 == null)
			{
				m_SearchLightP2 = GameObject.Find("searchlight_effect_02");
				m_SearchLightP2.active = false;
			}
		}
		if (m_Ahhhh == null)
		{
			m_Ahhhh = GameObject.Find("ah_pfb");
		}
		if (m_Container == null)
		{
			m_Container = GameObject.Find("Zbs-container_house_001");
		}
		if (m_ContainerDestroy == null)
		{
			m_ContainerDestroy = GameObject.Find("Zbs-container_house_002");
		}
		if (m_Truck == null)
		{
			m_Truck = GameObject.Find("truck");
			m_v3TruckPos = m_Truck.transform.position;
		}
		if (m_TruckFire == null)
		{
			m_TruckFire = (GameObject)Object.Instantiate(m_PerfabManager.m_TruckFire, m_v3TruckPos + new Vector3(0f, 3f, 0f), Quaternion.identity);
			Transform transform3 = m_TruckFire.transform.Find("combustion_01");
			if (transform3 != null)
			{
				m_Emitter1 = transform3.GetComponent<ParticleEmitter>();
			}
			transform3 = m_TruckFire.transform.Find("combustion_02");
			if (transform3 != null)
			{
				m_Emitter2 = transform3.GetComponent<ParticleEmitter>();
			}
			transform3 = m_TruckFire.transform.Find("combustion_03");
			if (transform3 != null)
			{
				m_Emitter3 = transform3.GetComponent<ParticleEmitter>();
			}
		}
		m_TruckFire.SetActiveRecursively(false);
		m_Emitter1.emit = false;
		m_Emitter2.emit = false;
		m_Emitter3.emit = false;
		if (m_TruckDestroy == null)
		{
			m_TruckDestroy = GameObject.Find("truck_01");
		}
		if (m_LidAnim == null)
		{
			m_LidAnim = GameObject.Find("Zbs-well");
			if (m_Lid == null)
			{
				Transform transform4 = m_LidAnim.transform.Find("lid__03");
				m_Lid = transform4.gameObject;
			}
			if (m_LidDestroy == null)
			{
				Transform transform5 = m_LidAnim.transform.Find("lid__02");
				m_LidDestroy = transform5.gameObject;
			}
		}
		if (m_UnFireList == null)
		{
			GameObject gameObject2 = GameObject.Find("Zombie Sniper_3D");
			m_UnFireList = new ArrayList();
			m_FireList = new ArrayList();
			if (gameObject2 != null)
			{
				foreach (Transform item in gameObject2.transform)
				{
					if (item.name.IndexOf("tree_t_fire") != -1)
					{
						m_FireList.Add(item.gameObject);
						int num = item.name.LastIndexOf('e');
						string text = item.name.Substring(num + 1, item.name.Length - num - 1);
						string name = "tree_t_" + text;
						GameObject gameObject3 = GameObject.Find(name);
						if (gameObject3 != null)
						{
							m_UnFireList.Add(gameObject3);
						}
					}
					else if (item.name == "tree_t_80")
					{
						m_UnFireList.Add(item.gameObject);
					}
					else if (item.name == "gound_02")
					{
						m_GroundFire = item.gameObject;
					}
				}
			}
			m_UnFireList.TrimToSize();
			m_FireList.TrimToSize();
		}
		if (m_TNT == null)
		{
			m_TNT = GameObject.Find("LandMine");
		}
		if (m_MoonWalk == null)
		{
			m_MoonWalk = new iZombieSniperMoonWalk();
		}
		SetFireSceneShow(false);
		m_v3EndPoint = new Vector3(-77f, 0f, -20f);
		m_fWarningDis = 20f;
		m_CameraScript.SetRotateLimit(8f, 53f, 58f, 213f);
	}

	public override void Destroy()
	{
		if (m_GameHelp != null)
		{
			m_GameHelp.Destroy();
			m_GameHelp = null;
		}
		if (m_MoonWalk != null)
		{
			m_MoonWalk.Destroy();
			m_MoonWalk = null;
		}
		if (m_MovieZombie != null)
		{
			m_MovieZombie.Destroy();
			m_MovieZombie = null;
		}
		if (m_MovieInno != null)
		{
			m_MovieInno.Destroy();
			m_MovieInno = null;
		}
		DestroyContainerAnim();
		if (m_iBunkerScript != null)
		{
			m_iBunkerScript.Destroy();
			m_iBunkerScript = null;
		}
		if (m_TruckFire != null)
		{
			ClearGameObjectAudio(m_TruckFire);
			Object.Destroy(m_TruckFire);
			m_TruckFire = null;
		}
		base.Destroy();
	}

	public override void ResetData()
	{
		base.ResetData();
		m_bMovieKillZombie = false;
		m_bZombieSound = false;
		m_bInnoSound = false;
		m_bIntroSound = false;
		m_Ahhhh.GetComponent<Animation>().Stop("ah");
		m_Ahhhh.SetActiveRecursively(false);
		m_SearchLightL1_1.active = false;
		m_SearchLightL1_2.active = false;
		m_SearchLightL2_1.active = false;
		m_SearchLightL2_2.active = false;
		m_SearchLightP1.active = false;
		m_SearchLightP2.active = false;
		m_SearchLightP1.GetComponent<Animation>().Stop("light_01");
		m_SearchLightP2.GetComponent<Animation>().Stop("light_02");
		if (m_iBunkerScript != null)
		{
			PlaySceneAnim(m_iBunkerScript.gameObject, "daoda", true, 1f);
		}
		DestroyContainerAnim();
		m_Container.SetActiveRecursively(true);
		m_ContainerDestroy.SetActiveRecursively(false);
		m_bContainerAnim = false;
		GameObject gameObject = GameObject.Find("Zombie Sniper_3D");
		if (gameObject != null)
		{
			Transform transform = gameObject.transform.Find("sundry_00");
			if (transform != null)
			{
				transform.gameObject.active = true;
			}
		}
		m_Truck.SetActiveRecursively(true);
		m_TruckDestroy.SetActiveRecursively(false);
		PlaySceneAnim(m_Truck, "truck", true, 1f);
		m_bTruckBoom = false;
		m_LidAnim.SetActiveRecursively(false);
		m_Lid.SetActiveRecursively(true);
		m_LidDestroy.SetActiveRecursively(false);
		m_bLidAnim = false;
		if (m_GameState.m_nMineCount > 0)
		{
			m_MineState = MineState.READY;
			if (m_TNT != null)
			{
				m_TNT.SetActiveRecursively(true);
			}
		}
		else
		{
			m_MineState = MineState.NONE;
			if (m_TNT != null)
			{
				m_TNT.SetActiveRecursively(false);
			}
		}
		StopAudio("MusicMap01");
		OpenClikPlugin.Hide();
	}

	public override void StartGame()
	{
		ResetData();
		AddOilCan(50f, 10f);
		AddOilCan(50f, 10f);
		AddOilCan(50f, 10f);
		AddOilCan(50f, 10f);
		if (m_bTutorial)
		{
			m_fMovieTime = 0f;
			m_CameraScript.GetComponent<Camera>().transform.forward = Vector3.forward;
			m_CameraScript.SetPosition(new Vector3(29f, 2f, 5f));
			m_SoliderMesh.SetActiveRecursively(false);
			m_GameSceneUI.ShowCross(false);
			m_GameSceneUI.ShowGameUI(false);
			m_MainChar.SetActiveRecursively(true);
			if (m_MainChar.GetComponent<Animation>() != null && m_MainChar.GetComponent<Animation>()["Idle01"] != null)
			{
				m_MainChar.GetComponent<Animation>()["Idle01"].wrapMode = WrapMode.Loop;
				m_MainChar.GetComponent<Animation>()["Idle01"].time = 0f;
				m_MainChar.GetComponent<Animation>().Play("Idle01");
			}
			m_MoonWalk.Initialize(m_CameraScript.GetComponent<Camera>(), false);
			m_MoonWalk.AddMove(new Vector3(29f, 2f, 6.35f), new Vector3(-0.3f, 0f, 1f), 3f);
			m_MoonWalk.AddMove(new Vector3(23.3f, 2f, 18.3f), new Vector3(-1f, 0f, 0f), 3f);
			m_MoonWalk.AddMove(new Vector3(-2.28f, 2f, 17f), new Vector3(-1f, 0f, 0f), 10f);
			m_MoonWalk.AddMove(new Vector3(-15.58f, 2f, 17f), new Vector3(-0.9f, 0.3f, -0.2f), 10f);
			m_MoonWalk.AddMove(new Vector3(-59.86f, 16f, 10.24f), new Vector3(-0.9f, 0.3f, 0.1f), 55f);
			m_MoonWalk.AddMove(new Vector3(-59f, 15.8f, 9.4f), new Vector3(-0.9f, 0.3f, 0.1f), 0.5f);
			m_MovieState = MovieState.kMovieSTEP1;
			m_MoonWalk.Start(iZombieSniperMoonWalk.MoonWalkType.Begin);
			int nUID = AddNPC(1001, new Vector3(-4f, 0f, 21f), -1);
			m_MovieZombie = GetNPC(nUID);
			nUID = AddNPC(1002, new Vector3(-4f, 0f, 20f), -1);
			m_MovieInno = GetNPC(nUID);
			if (m_MovieZombie != null)
			{
				m_MovieZombie.stWaitState.Initialize(0f);
				m_MovieZombie.SetStateDirectly(m_MovieZombie.stWaitState);
				m_MovieZombie.stMoveState.Initialize(m_MovieZombie, new Vector3(-4f, 0f, 13f), null, true);
				m_MovieZombie.SetState(m_MovieZombie.stMoveState);
				m_MovieZombie.stWaitState.Initialize(1f);
				m_MovieZombie.SetState(m_MovieZombie.stWaitState);
			}
			if (m_MovieInno != null)
			{
				m_MovieInno.stMoveState.Initialize(m_MovieInno, new Vector3(-4f, 0f, 12f), null, true);
				m_MovieInno.SetState(m_MovieInno.stMoveState);
				m_MovieInno.stWaitState.Initialize(1f);
				m_MovieInno.SetState(m_MovieInno.stWaitState);
			}
			SetGameState(State.kGameMoonWalk);
		}
		else
		{
			m_ZombieWaveCenter.LoadZombieWaveInfoList(m_GameState.m_nCurStage);
			m_ZobieWaveManager.Initialize();
			m_ZombieWaveCenter.LoadLogicWaveInfo(m_GameState.m_nCurStage);
			m_ZobieWaveManagerLogic.Initialize();
			SetGameState(State.kGaming);
			iZombieSniperGameApp.GetInstance().m_bSoundSwitch = true;
			iZombieSniperGameApp.GetInstance().ClearAudio("MusicMap01");
			PlayAudio("MusicMap01");
		}
	}

	public override void ReadyGameOver(Vector3 v3LookPoint)
	{
		base.ReadyGameOver(v3LookPoint);
		if (v3LookPoint != Vector3.zero)
		{
			m_fGameOveringTime = 2f;
			m_CameraScript.transform.forward = v3LookPoint - m_CameraScript.GetPosition();
			m_CameraScript.HeadShotCamera();
		}
		else
		{
			m_CameraScript.SetPosition(new Vector3(-54f, 0.68f, -15.3f));
			m_CameraScript.transform.eulerAngles = new Vector3(342f, 254f, 0.2f);
			m_CameraScript.m_CameraState = iZombieSniperCamera.CameraState.kMovie;
			if (m_Ahhhh != null)
			{
				m_Ahhhh.SetActiveRecursively(true);
				m_Ahhhh.GetComponent<Animation>()["ah"].wrapMode = WrapMode.Loop;
				m_Ahhhh.GetComponent<Animation>().Play("ah");
			}
			iZombieSniperGameApp.GetInstance().ClearAudio("VoGAMEOVER");
			PlayAudio("VoGAMEOVER");
			m_fGameOveringTime = 4f;
		}
		SetGameState(State.kGameOvering);
		Resources.UnloadUnusedAssets();
	}

	public override void GameOver()
	{
		if (m_OilCanMap.Count == 0 && m_TruckDestroy != null && m_TruckDestroy.active)
		{
			m_GameState.m_bDestroyAllBoomer = true;
		}
		if (IsSpecialButtonDone())
		{
			m_GameState.m_bUseAirStrike = true;
		}
		base.GameOver();
	}

	public void StartTurtorial()
	{
		SetGameState(State.kGameTurtorial);
		m_nTurtorialNPC = AddNPC(103, new Vector3(-33f, 0f, -24f), -1);
		AddNPC(100, new Vector3(-51f, 0f, -12f), -1);
		AddNPC(101, new Vector3(-50f, 0f, -10f), -1);
		AddNPC(102, new Vector3(-52f, 0f, -10f), -1);
		if (m_iBunkerScript != null)
		{
			m_iBunkerScript.m_bActive = false;
		}
		m_MoonWalk.Reset();
		m_MoonWalk.AddRotate(new Vector3(0.3f, -0.4f, -0.8f), 0.5f);
		m_MoonWalk.AddRotate(new Vector3(-0.4f, -0.5f, -0.8f), 0.3f);
		m_MoonWalk.AddRotate(new Vector3(0.3f, -0.4f, -0.8f), 1f);
		m_MoonWalk.Start(iZombieSniperMoonWalk.MoonWalkType.LookNPC);
	}

	public override void UpdateMoonWalk(float deltaTime)
	{
		m_fMovieTime += deltaTime;
		if (m_fMovieTime > 0.1f && !m_bIntroSound)
		{
			m_bIntroSound = true;
			PlayAudio("MusicIntro01");
		}
		else if (m_fMovieTime > 3f && !m_bInnoSound)
		{
			m_bInnoSound = true;
			PlayAudio("MonHumanHelp");
		}
		else if (m_fMovieTime > 3.5f && !m_bZombieSound)
		{
			m_bZombieSound = true;
			PlayAudio("MonZombieHappy");
		}
		else if (m_fMovieTime > 4f && !m_bMovieKillZombie)
		{
			m_bMovieKillZombie = true;
			PlayAudio("AWPFire01");
			PlayAudio("MonZombieDeath");
			if (m_MovieZombie != null)
			{
				m_MovieZombie.stWaitState.Initialize(0f);
				m_MovieZombie.SetStateDirectly(m_MovieZombie.stWaitState);
				m_MovieZombie.m_Anim.PlayAnim(ACTION_ENUM.DEAD_LEFT, -1, 0f);
				AddBloodEffect(m_MovieZombie.m_ModelTransForm.position + new Vector3(0f, 0.6f, 0f));
			}
		}
		if (m_MovieState == MovieState.kMovieSTEP1)
		{
			m_MoonWalk.Update(deltaTime);
			if (m_MoonWalk.IsFinished(iZombieSniperMoonWalk.MoonWalkType.Begin))
			{
				m_MovieState = MovieState.kMovieSTEP4;
				m_GameSceneUI.ShowMainPic(true);
				m_fTempCount = 3f;
				PlayAudio("MusicIntro02");
			}
		}
		else if (m_MovieState == MovieState.kMovieSTEP2)
		{
			m_MoonWalk.Update(deltaTime);
			if (m_MoonWalk.IsFinished(iZombieSniperMoonWalk.MoonWalkType.Begin))
			{
				m_MovieState = MovieState.kMovieSTEP3;
			}
		}
		else if (m_MovieState == MovieState.kMovieSTEP3)
		{
			m_MoonWalk.Update(deltaTime);
		}
		else if (m_MovieState == MovieState.kMovieSTEP4)
		{
			m_fTempCount -= deltaTime;
			if (m_fTempCount <= 0f)
			{
				m_fTempCount = 0f;
				m_GameSceneUI.ShowMainPic(false);
				m_MovieState = MovieState.kMovieSTEP5;
				m_MoonWalk.Reset();
				m_MoonWalk.AddMove(m_v3SniperPosition, new Vector3(0.9f, -0.3f, 0.1f), 2f);
				m_MoonWalk.Start(iZombieSniperMoonWalk.MoonWalkType.Begin);
			}
		}
		else if (m_MovieState == MovieState.kMovieSTEP5)
		{
			m_MoonWalk.Update(deltaTime);
			if (m_MoonWalk.IsFinished(iZombieSniperMoonWalk.MoonWalkType.Begin))
			{
				m_MoonWalk.m_Type = iZombieSniperMoonWalk.MoonWalkType.None;
				StartTurtorial();
			}
		}
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		for (int i = 0; i < array.Length; i++)
		{
			UITouchInner touch = array[i];
			if ((!(m_GameSceneUI != null) || !m_GameSceneUI.m_UIManagerRef.HandleInput(touch)) && touch.phase == TouchPhase.Ended)
			{
				m_fTempCount = 0f;
			}
		}
		ArrayList arrayList = new ArrayList();
		foreach (iZombieSniperNpc value in m_NPCMap.Values)
		{
			value.Update(deltaTime);
			if (value.m_bNeedDestroy)
			{
				arrayList.Add(value);
			}
		}
		foreach (iZombieSniperNpc item in arrayList)
		{
			RemoveNPC(item);
			item.Destroy();
		}
		arrayList.Clear();
		foreach (iZombieSniperOilCan value2 in m_OilCanMap.Values)
		{
			value2.Update(deltaTime);
			if (value2.m_bDestroy)
			{
				arrayList.Add(value2);
			}
		}
		foreach (iZombieSniperOilCan item2 in arrayList)
		{
			RemoveOilCan(item2);
			m_OilCanEmptyList.Add(item2.m_nPointID);
			item2.Destroy();
		}
		arrayList.Clear();
	}

	public override void UpdateTutorial(float deltaTime)
	{
		m_MoonWalk.Update(deltaTime);
		if (m_MoonWalk.IsFinished(iZombieSniperMoonWalk.MoonWalkType.LookNPC))
		{
			m_MoonWalk.m_Type = iZombieSniperMoonWalk.MoonWalkType.None;
			m_SoliderMesh.SetActiveRecursively(true);
			m_GameSceneUI.ShowCross(true);
			m_GameSceneUI.ShowGameUI(true);
			m_MainChar.SetActiveRecursively(false);
			m_GameHelp.EnterHelpState(GameHelpState.Step1);
			iZombieSniperNpc nPC = GetNPC(m_nTurtorialNPC);
			if (nPC != null)
			{
				nPC.SetStateDirectly(nPC.stWaitState);
			}
		}
		if (!m_bPause)
		{
			ArrayList arrayList = new ArrayList();
			foreach (iZombieSniperNpc value in m_NPCMap.Values)
			{
				value.Update(deltaTime);
				if (value.m_bNeedDestroy)
				{
					arrayList.Add(value);
				}
			}
			foreach (iZombieSniperNpc item in arrayList)
			{
				RemoveNPC(item);
				item.Destroy();
			}
			arrayList.Clear();
			foreach (iZombieSniperOilCan value2 in m_OilCanMap.Values)
			{
				value2.Update(deltaTime);
				if (value2.m_bDestroy)
				{
					arrayList.Add(value2);
				}
			}
			foreach (iZombieSniperOilCan item2 in arrayList)
			{
				RemoveOilCan(item2);
				m_OilCanEmptyList.Add(item2.m_nPointID);
				item2.Destroy();
			}
			arrayList.Clear();
			UpdateCurrWeapon(deltaTime);
			UpdateAutoShoot(deltaTime);
			m_GameHelp.Update(deltaTime);
		}
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		for (int i = 0; i < array.Length; i++)
		{
			UITouchInner touch = array[i];
			if ((m_GameSceneUI != null && m_GameSceneUI.m_UIManagerRef.HandleInput(touch)) || m_bPause)
			{
				continue;
			}
			if (touch.phase == TouchPhase.Began)
			{
				m_v2TouchPos = touch.position;
				m_bIsDragMouse = false;
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				if (!m_bIsDragMouse && Vector2.Distance(m_v2TouchPos, touch.position) > (float)(12 * m_GameState.m_nHDFactor))
				{
					m_bIsDragMouse = true;
				}
				if (!m_bIsDragMouse || (m_MoonWalk != null && !m_MoonWalk.m_bFinished) || (m_GameHelp.IsInTutorial() && m_GameHelp.IsWaitConfirm()))
				{
					break;
				}
				if (!m_GameHelp.IsCanSlipScreen())
				{
					continue;
				}
				if (!m_bAim)
				{
					if (Utils.IsPad())
					{
						m_CameraScript.YawCamera(touch.deltaPosition.x / (float)Screen.width * 2.4f);
						m_CameraScript.PitchCamera(touch.deltaPosition.y / (float)Screen.height * 2.4f);
					}
					else
					{
						m_CameraScript.YawCamera(touch.deltaPosition.x / (float)Screen.width * 1.2f);
						m_CameraScript.PitchCamera(touch.deltaPosition.y / (float)Screen.height * 1.2f);
					}
				}
				else if (Utils.IsPad())
				{
					m_CameraScript.AimMove(touch.deltaPosition.x * 2f, touch.deltaPosition.y * 2f);
				}
				else
				{
					m_CameraScript.AimMove(touch.deltaPosition.x, touch.deltaPosition.y);
				}
				if (m_GameHelp.m_HelpState == GameHelpState.Step2 || m_GameHelp.m_HelpState == GameHelpState.Step6)
				{
					Ray ray = m_CameraScript.GetComponent<Camera>().ScreenPointToRay(new Vector3(m_GameState.GetShootCenter().x, m_GameState.GetShootCenter().y, 0f));
					RaycastHit hitInfo;
					if (Physics.Raycast(ray, out hitInfo, 1000f) && hitInfo.transform.root.name.IndexOf("Zombie") != -1 && hitInfo.transform.root.tag == "NPC")
					{
						m_GameHelp.FinishHelpState(m_GameHelp.m_HelpState);
					}
				}
			}
			else
			{
				if (touch.phase != TouchPhase.Ended)
				{
					continue;
				}
				if (!m_bIsDragMouse)
				{
					if (m_GameHelp.IsInTutorial() && m_GameHelp.IsWaitConfirm())
					{
						m_GameHelp.NextHelpState();
						break;
					}
					if (!m_bAim)
					{
						iZombieSniperNpc nPC2 = GetNPC(m_nTurtorialNPC);
						if (nPC2 != null && m_GameHelp.IsCanAim())
						{
							Ray ray2 = m_CameraScript.GetComponent<Camera>().ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0f));
							RaycastHit hitInfo2;
							if (Physics.Raycast(ray2, out hitInfo2, 1000f) && Vector3.Distance(hitInfo2.point, nPC2.m_ModelTransForm.position) <= 5f)
							{
								Aim(touch.position);
								m_GameHelp.FinishHelpState(GameHelpState.Step1);
							}
						}
					}
					else if (m_GameHelp.IsCanCloseAim())
					{
						CloseAim();
						m_GameHelp.FinishHelpState(GameHelpState.Step4);
					}
				}
				else
				{
					m_bIsDragMouse = false;
				}
			}
		}
	}

	public override void UpdateGameing(float deltaTime)
	{
		if (m_bGameOver)
		{
			ReadyGameOver(m_v3LastDeadInno);
			return;
		}
		base.UpdateGameing(deltaTime);
		if (!m_bPause)
		{
			bool flag = false;
			ArrayList arrayList = new ArrayList();
			foreach (iZombieSniperNpc value in m_NPCMap.Values)
			{
				value.Update(deltaTime);
				if (value.m_bNeedDestroy)
				{
					arrayList.Add(value);
				}
				if (!value.IsDead() && value.m_bAlarmTip)
				{
					flag = true;
				}
			}
			if (flag != m_bAlarmScreen)
			{
				m_bAlarmScreen = flag;
				if (m_bAlarmScreen)
				{
					m_GameSceneUI.AddImageWarn1(ImageMsgEnum1.ZOMBIE_CLOSE);
					m_GameSceneUI.StartAlarmScreen(iZombieSniperGameUI.AlarmScreenSide.Right);
					PlayAudio("FxAlarmLoop01");
					if (m_SearchLightL1_1 != null && m_SearchLightL1_2 != null && m_SearchLightP1 != null)
					{
						m_SearchLightL1_1.active = true;
						m_SearchLightL1_2.active = true;
						m_SearchLightP1.active = true;
						if (m_SearchLightP1.GetComponent<Animation>() != null && m_SearchLightP1.GetComponent<Animation>()["light_01"] != null)
						{
							m_SearchLightP1.GetComponent<Animation>()["light_01"].wrapMode = WrapMode.Loop;
							m_SearchLightP1.GetComponent<Animation>().Play("light_01");
						}
					}
					if (m_SearchLightL2_1 != null && m_SearchLightL2_2 != null && m_SearchLightP2 != null)
					{
						m_SearchLightL2_1.active = true;
						m_SearchLightL2_2.active = true;
						m_SearchLightP2.active = true;
						if (m_SearchLightP2.GetComponent<Animation>() != null && m_SearchLightP2.GetComponent<Animation>()["light_02"] != null)
						{
							m_SearchLightP2.GetComponent<Animation>()["light_02"].wrapMode = WrapMode.Loop;
							m_SearchLightP2.GetComponent<Animation>().Play("light_02");
						}
					}
				}
				else
				{
					m_GameSceneUI.StopAlarmScreen(iZombieSniperGameUI.AlarmScreenSide.Right);
					StopAudio("FxAlarmLoop01");
					if (m_SearchLightL1_1 != null && m_SearchLightL1_2 != null && m_SearchLightP1 != null)
					{
						if (m_SearchLightP1.GetComponent<Animation>() != null && m_SearchLightP1.GetComponent<Animation>()["light_01"] != null)
						{
							m_SearchLightP1.GetComponent<Animation>().Stop("light_01");
						}
						m_SearchLightL1_1.active = false;
						m_SearchLightL1_2.active = false;
						m_SearchLightP1.active = false;
					}
					if (m_SearchLightL2_1 != null && m_SearchLightL2_2 != null && m_SearchLightP2 != null)
					{
						if (m_SearchLightP2.GetComponent<Animation>() != null && m_SearchLightP2.GetComponent<Animation>()["light_02"] != null)
						{
							m_SearchLightP2.GetComponent<Animation>().Stop("light_02");
						}
						m_SearchLightL2_1.active = false;
						m_SearchLightL2_2.active = false;
						m_SearchLightP2.active = false;
					}
				}
			}
			foreach (iZombieSniperNpc item in arrayList)
			{
				RemoveNPC(item);
				item.Destroy();
			}
			arrayList.Clear();
			foreach (iZombieSniperOilCan value2 in m_OilCanMap.Values)
			{
				value2.Update(deltaTime);
				if (value2.m_bDestroy)
				{
					arrayList.Add(value2);
				}
			}
			foreach (iZombieSniperOilCan item2 in arrayList)
			{
				RemoveOilCan(item2);
				m_OilCanEmptyList.Add(item2.m_nPointID);
				item2.Destroy();
			}
			arrayList.Clear();
			if (!m_bTutorial)
			{
				m_ZobieWaveManager.Update(deltaTime);
			}
			UpdateGameEvent(deltaTime);
			UpdateCurrWeapon(deltaTime);
			UpdateAutoShoot(deltaTime);
		}
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		for (int i = 0; i < array.Length; i++)
		{
			UITouchInner touch = array[i];
			if ((m_GameSceneUI != null && m_GameSceneUI.m_UIManagerRef.HandleInput(touch)) || m_bPause || m_bSlowScale)
			{
				continue;
			}
			if (touch.phase == TouchPhase.Began)
			{
				m_v2TouchPos = touch.position;
				m_bIsDragMouse = false;
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				if (!m_bIsDragMouse && Vector2.Distance(m_v2TouchPos, touch.position) > (float)(12 * m_GameState.m_nHDFactor))
				{
					m_bIsDragMouse = true;
				}
				if (!m_bIsDragMouse)
				{
					break;
				}
				if (!m_bAim)
				{
					if (Utils.IsPad())
					{
						m_CameraScript.YawCamera(touch.deltaPosition.x / (float)Screen.width * 2.4f);
						m_CameraScript.PitchCamera(touch.deltaPosition.y / (float)Screen.height * 2.4f);
					}
					else
					{
						m_CameraScript.YawCamera(touch.deltaPosition.x / (float)Screen.width * 1.2f);
						m_CameraScript.PitchCamera(touch.deltaPosition.y / (float)Screen.height * 1.2f);
					}
				}
				else if (Utils.IsPad())
				{
					m_CameraScript.AimMove(touch.deltaPosition.x * 2f, touch.deltaPosition.y * 2f);
				}
				else
				{
					m_CameraScript.AimMove(touch.deltaPosition.x, touch.deltaPosition.y);
				}
			}
			else
			{
				if (touch.phase != TouchPhase.Ended)
				{
					continue;
				}
				if (!m_bIsDragMouse)
				{
					if (!m_GameSceneUI.IsFadeIn())
					{
						if (!m_bAim)
						{
							Aim(touch.position);
						}
						else
						{
							CloseAim();
						}
					}
				}
				else
				{
					m_bIsDragMouse = false;
				}
			}
		}
	}

	public override void FireHit(Ray ray, RaycastHit hit, bool bMakeSound)
	{
		base.FireHit(ray, hit, bMakeSound);
		if (hit.transform.root.name == "truck")
		{
			AddGunHitEffect(hit.point);
			if (m_iTruckScript != null && !m_iTruckScript.IsDead())
			{
				m_iTruckScript.AddHP(0f - m_CurrWeapon.m_fSD);
			}
			PlayAudio("FxMetalHit01");
		}
	}

	public void SetFireSceneShow(bool bShow)
	{
		if (bShow)
		{
			foreach (GameObject fire in m_FireList)
			{
				fire.active = true;
			}
			foreach (GameObject unFire in m_UnFireList)
			{
				unFire.active = false;
			}
			if ((bool)m_GroundFire)
			{
				m_GroundFire.active = true;
			}
			return;
		}
		foreach (GameObject fire2 in m_FireList)
		{
			fire2.active = false;
		}
		foreach (GameObject unFire2 in m_UnFireList)
		{
			unFire2.active = true;
		}
		if ((bool)m_GroundFire)
		{
			m_GroundFire.active = false;
		}
	}

	public override void UpdateGameEvent(float deltaTime)
	{
		if (m_fAnimStopVoice > 0f)
		{
			m_fAnimStopVoice -= deltaTime;
			if (m_fAnimStopVoice <= 0f)
			{
				m_fAnimStopVoice = 0f;
			}
		}
		if (!m_bContainerAnim && m_fGameTimeTotal > 180f)
		{
			m_bContainerAnim = true;
			m_Container.SetActiveRecursively(false);
			if (m_GameState.m_bCutScenes)
			{
				m_fSlowTimeRate = 0.1f;
				CreateContainerAnim();
				m_fContainerAnimTime = PlaySceneAnim(m_ContainerAnim, "01", false, 1f / m_fSlowTimeRate);
				SetPauseScale(true, m_fSlowTimeRate);
				StartSlow(new Vector3(-15.6f, 0.3f, 13.4f), new Vector3(343f, 402f, 360f), 45f);
				m_GameSceneUI.ShowMovieUI();
			}
			else
			{
				m_fSlowTimeRate = 1f;
				CreateContainerAnim();
				m_fContainerAnimTime = PlaySceneAnim(m_ContainerAnim, "01", false, 1f);
			}
			PlayAudio("MusicMap01");
			m_fAnimStopVoice = 4f;
		}
		if (!m_bLidAnim && m_fGameTimeTotal > 360f)
		{
			m_bLidAnim = true;
			m_LidAnim.SetActiveRecursively(true);
			m_Lid.SetActiveRecursively(false);
			m_LidDestroy.SetActiveRecursively(false);
			if (m_GameState.m_bCutScenes)
			{
				m_fSlowTimeRate = 0.1f;
				m_fLidAnimTime = PlaySceneAnim(m_LidAnim, "01", false, 1f / m_fSlowTimeRate);
				SetPauseScale(true, m_fSlowTimeRate);
				StartSlow(new Vector3(-50f, 0.3f, -24.2f), new Vector3(364f, 165f, 360f), 45f);
				m_GameSceneUI.ShowMovieUI();
			}
			else
			{
				m_fSlowTimeRate = 1f;
				m_fLidAnimTime = PlaySceneAnim(m_LidAnim, "01", false, 1f);
			}
			PlayAudio("MusicMap01");
			m_fAnimStopVoice = 4f;
		}
		if (m_bContainerAnim && m_fContainerAnimTime > 0f)
		{
			m_fContainerAnimTime -= deltaTime * (1f / m_fSlowTimeRate);
			if (m_fContainerAnimTime <= 0f)
			{
				m_fContainerAnimTime = 0f;
				DestroyContainerAnim();
				m_ContainerDestroy.SetActiveRecursively(true);
				if (m_bSlowScale)
				{
					SetPauseScale(false, 1f);
					EndSlow();
					m_GameSceneUI.HideMovieUI();
				}
			}
		}
		if (m_bLidAnim && m_fLidAnimTime > 0f)
		{
			m_fLidAnimTime -= deltaTime * (1f / m_fSlowTimeRate);
			if (m_fLidAnimTime <= 0f)
			{
				m_fLidAnimTime = 0f;
				m_LidAnim.SetActiveRecursively(false);
				m_Lid.SetActiveRecursively(false);
				m_LidDestroy.SetActiveRecursively(true);
				if (m_bSlowScale)
				{
					SetPauseScale(false, 1f);
					EndSlow();
					m_GameSceneUI.HideMovieUI();
				}
			}
		}
		if (!m_bTruckBoom)
		{
			return;
		}
		if (m_TruckFire.active)
		{
			m_fSmokeCount -= Time.deltaTime;
			if (m_fSmokeCount < 0f)
			{
				m_fSmokeCount = 0f;
				m_Emitter1.emit = false;
				m_Emitter2.emit = false;
				m_Emitter3.emit = false;
				TAudioController component = m_TruckFire.GetComponent<TAudioController>();
				if (component != null)
				{
					component.StopAudio("FxFireLoop01");
				}
			}
			if (m_fSmokeCount < 10f)
			{
				m_Emitter1.minEmission = 20f * m_fSmokeCount / 10f;
				m_Emitter1.maxEmission = 22f * m_fSmokeCount / 10f;
				m_Emitter2.minEmission = 9f * m_fSmokeCount / 10f;
				m_Emitter2.maxEmission = 12f * m_fSmokeCount / 10f;
				m_Emitter3.minEmission = 8f * m_fSmokeCount / 10f;
				m_Emitter3.maxEmission = 9f * m_fSmokeCount / 10f;
			}
		}
		m_fTruckAnimTime -= deltaTime;
		if (m_fTruckAnimTime <= 0f)
		{
			m_fTruckAnimTime = 0f;
			m_Truck.SetActiveRecursively(false);
			m_TruckDestroy.SetActiveRecursively(true);
		}
	}

	public override void TruckBoom()
	{
		Boom(m_v3TruckPos, 30f, 200f);
		GameObject obj = (GameObject)Object.Instantiate(m_PerfabManager.m_TruckBoom, m_v3TruckPos, Quaternion.identity);
		Object.Destroy(obj, 2f);
		SetFireSceneShow(true);
		m_bTruckBoom = true;
		m_fTruckAnimTime = PlaySceneAnim(m_Truck, "truck", false, 1f);
		m_CameraScript.Shake(1.5f, m_fTruckAnimTime);
		m_TruckFire.SetActiveRecursively(true);
		m_fSmokeCount = m_fTruckAnimTime * 10f;
		m_Emitter1.emit = true;
		m_Emitter1.minEmission = 20f;
		m_Emitter1.maxEmission = 22f;
		m_Emitter2.emit = true;
		m_Emitter2.minEmission = 9f;
		m_Emitter2.maxEmission = 12f;
		m_Emitter3.emit = true;
		m_Emitter3.minEmission = 8f;
		m_Emitter3.maxEmission = 9f;
		TAudioController component = m_TruckFire.GetComponent<TAudioController>();
		if (component != null)
		{
			component.PlayAudio("FxFireLoop01");
		}
	}

	private void CreateContainerAnim()
	{
		if (m_ContainerAnim == null)
		{
			m_ContainerAnim = (GameObject)Object.Instantiate(m_PerfabManager.m_ContainerAnim, Vector3.zero, Quaternion.identity);
		}
	}

	private void DestroyContainerAnim()
	{
		if (m_ContainerAnim != null)
		{
			Object.Destroy(m_ContainerAnim);
			m_ContainerAnim = null;
		}
	}

	public override int GetTurtorialNPC()
	{
		return m_nTurtorialNPC;
	}

	public override bool IsLidAnim()
	{
		return m_bLidAnim;
	}

	public override GameObject GetTNT()
	{
		return m_TNT;
	}

	public override iZombieSniperBunker GetBunkerScript()
	{
		return m_iBunkerScript;
	}

	public override iZombieSniperTruck GetTruckScript()
	{
		return m_iTruckScript;
	}

	public override float GetSlowTimeRate()
	{
		return m_fSlowTimeRate;
	}

	public override Vector3 GetAC130Start()
	{
		return new Vector3(-120f, 50f, -50f) + new Vector3(20f, 0f, 10f);
	}

	public override Vector2 GetAC130Range()
	{
		return new Vector2(120f, 80f);
	}

	public override Vector2 GetAC130Space()
	{
		return new Vector2(10f, 10f);
	}

	public override void SetPauseAudio(bool bPause)
	{
		if (bPause)
		{
			if (m_bAlarmScreen)
			{
				StopAudio("FxAlarmLoop01");
			}
			if (m_fGameTimeTotal < 23f)
			{
				StopAudio("MusicMap01");
			}
			m_BackGroundSound.active = false;
			return;
		}
		if (m_bAlarmScreen)
		{
			PlayAudio("FxAlarmLoop01");
		}
		if (m_fGameTimeTotal < 13f)
		{
			PlayAudio("MusicMap01");
		}
		if (m_GameState.m_bSoundOn)
		{
			m_BackGroundSound.active = true;
		}
	}

	public override iZombieSniperGameHelp GetGameHelp()
	{
		return m_GameHelp;
	}

	public override bool IsNeedClimbout(int nStartPoint)
	{
		if (nStartPoint == 5)
		{
			return true;
		}
		return false;
	}
}
