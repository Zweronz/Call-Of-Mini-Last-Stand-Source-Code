using System.Collections;
using UnityEngine;

public class iZombieSniperGameScene1 : iZombieSniperGameSceneBase
{
	public enum MovieState
	{
		kMovieSTEP1 = 0
	}

	public GameObject m_PathManager;

	private iZombieSniperMoonWalk m_CameraMove;

	public GameObject m_PlaneObject;

	private PathPointMover m_PathMover;

	private bool m_bMarketAnim;

	private bool m_bUnderWayAnim;

	private bool m_bTruckRushAnim;

	private bool m_bTruckBoom;

	private GameObject m_NewTruck;

	private GameObject m_Market;

	private GameObject m_MarketDestroy;

	private GameObject m_CGStation;

	private GameObject m_CGOP;

	private iZombieSniperGameUI.AlarmScreenSide m_AlarmSide;

	private float m_fAlarmRefreshTime;

	public GameObject m_SearchLight1;

	public GameObject m_SearchLight2;

	public GameObject m_SearchLight3;

	public GameObject m_SearchLight4;

	public GameObject m_SearchLightL1_1;

	public GameObject m_SearchLightL1_2;

	public GameObject m_SearchLightL2_1;

	public GameObject m_SearchLightL2_2;

	public GameObject m_SearchLightL3_1;

	public GameObject m_SearchLightL3_2;

	public GameObject m_SearchLightL4_1;

	public GameObject m_SearchLightL4_2;

	public GameObject m_SearchLightP1;

	public GameObject m_SearchLightP2;

	public GameObject m_SearchLightP3;

	public GameObject m_SearchLightP4;

	public GameObject m_Ahhhh;

	public GameObject m_Bunker;

	public iZombieSniperBunker m_iBunkerScript;

	public ArrayList m_UnFireList;

	public ArrayList m_FireList;

	public GameObject m_GroundFire;

	public GameObject m_TNT;

	public iZombieSniperTruck m_iTruckScript;

	public GameObject m_Truck;

	public GameObject m_TruckDestroy;

	public Vector3 m_v3TruckPos = Vector3.zero;

	private GameObject m_TruckFire;

	private ParticleEmitter m_Emitter1;

	private ParticleEmitter m_Emitter2;

	private ParticleEmitter m_Emitter3;

	private float m_fSmokeCount;

	public MovieState m_MovieState;

	private float m_fMovieTime;

	private GameObject m_PlayerPlane;

	public float m_fMarketAnimTime;

	public float m_fUnderWayAnimTime;

	public float m_fTruckAnimTime;

	public float m_fTruckAnimRushTime;

	public float m_fSlowTimeRate = 0.1f;

	public Vector2 m_v2TouchPos = Vector2.zero;

	public override void Initialize()
	{
		base.Initialize();
		m_bMarketAnim = false;
		m_bUnderWayAnim = false;
		m_bTruckRushAnim = false;
		m_AlarmSide = iZombieSniperGameUI.AlarmScreenSide.None;
		m_fAlarmRefreshTime = 1f;
		m_CameraScript.SetRotateLimit(-20f, 80f, 0f, 0f);
		m_v3EndPoint = new Vector3(53f, 0f, -45f);
		m_fWarningDis = 25f;
		m_CameraMove = new iZombieSniperMoonWalk();
		m_PlaneObject = GameObject.Find("_PlaneObject");
		if (m_PlaneObject != null)
		{
			m_PathMover = m_PlaneObject.GetComponent<PathPointMover>();
		}
		m_PathManager = GameObject.Find("PathManager");
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
		if (m_SearchLight3 == null)
		{
			m_SearchLight3 = GameObject.Find("searchlight_03");
			Transform transform3 = null;
			transform3 = m_SearchLight3.transform.Find("light_01");
			if (transform3 != null)
			{
				m_SearchLightL3_1 = transform3.gameObject;
				m_SearchLightL3_1.active = false;
			}
			transform3 = m_SearchLight3.transform.Find("light_02");
			if (transform3 != null)
			{
				m_SearchLightL3_2 = transform3.gameObject;
				m_SearchLightL3_2.active = false;
			}
			if (m_SearchLightP3 == null)
			{
				m_SearchLightP3 = GameObject.Find("searchlight_effect_03");
				m_SearchLightP3.active = false;
			}
		}
		if (m_SearchLight4 == null)
		{
			m_SearchLight4 = GameObject.Find("searchlight_04");
			Transform transform4 = null;
			transform4 = m_SearchLight4.transform.Find("light_01");
			if (transform4 != null)
			{
				m_SearchLightL4_1 = transform4.gameObject;
				m_SearchLightL4_1.active = false;
			}
			transform4 = m_SearchLight4.transform.Find("light_02");
			if (transform4 != null)
			{
				m_SearchLightL4_2 = transform4.gameObject;
				m_SearchLightL4_2.active = false;
			}
			if (m_SearchLightP4 == null)
			{
				m_SearchLightP4 = GameObject.Find("searchlight_effect_04");
				m_SearchLightP4.active = false;
			}
		}
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
			m_iBunkerScript.Initialize(0.6f + 0.35f * (float)(m_GameState.m_nTowerLvl - 1), m_GameState.m_nTowerBullet, 0.3f, 30f);
		}
		if (m_TruckFire == null)
		{
			m_TruckFire = (GameObject)Object.Instantiate(m_PerfabManager.m_TruckFire, m_v3TruckPos + new Vector3(0f, 3f, 0f), Quaternion.identity);
			Transform transform5 = m_TruckFire.transform.Find("combustion_01");
			if (transform5 != null)
			{
				m_Emitter1 = transform5.GetComponent<ParticleEmitter>();
			}
			transform5 = m_TruckFire.transform.Find("combustion_02");
			if (transform5 != null)
			{
				m_Emitter2 = transform5.GetComponent<ParticleEmitter>();
			}
			transform5 = m_TruckFire.transform.Find("combustion_03");
			if (transform5 != null)
			{
				m_Emitter3 = transform5.GetComponent<ParticleEmitter>();
			}
		}
		m_TruckFire.SetActiveRecursively(false);
		m_Emitter1.emit = false;
		m_Emitter2.emit = false;
		m_Emitter3.emit = false;
		if (m_Ahhhh == null)
		{
			m_Ahhhh = GameObject.Find("ah_pfb");
		}
		if (m_Market == null)
		{
			m_Market = (GameObject)Object.Instantiate(m_PerfabManager.Market);
			PlaySceneAnim(m_Market, "polie", true, 1f);
		}
		if (m_PlayerPlane == null && m_bCG)
		{
			m_PlayerPlane = (GameObject)Object.Instantiate(m_PerfabManager.PlayerPlane);
			m_PlayerPlane.SetActiveRecursively(false);
		}
		if (m_UnFireList == null)
		{
			GameObject gameObject = GameObject.Find("Z_airport01");
			m_UnFireList = new ArrayList();
			m_FireList = new ArrayList();
			if (gameObject != null)
			{
				foreach (Transform item in gameObject.transform)
				{
					if (item.name.IndexOf("tree_t_fire") != -1)
					{
						m_FireList.Add(item.gameObject);
						int num = item.name.LastIndexOf('e');
						string text = item.name.Substring(num + 1, item.name.Length - num - 1);
						string name = "tree_t_" + text;
						GameObject gameObject2 = GameObject.Find(name);
						if (gameObject2 != null)
						{
							m_UnFireList.Add(gameObject2);
						}
					}
					else if (item.name == "Z_airport_floor03")
					{
						m_GroundFire = item.gameObject;
					}
				}
			}
			m_UnFireList.TrimToSize();
			m_FireList.TrimToSize();
		}
		SetFireSceneShow(false);
		if (m_TNT == null)
		{
			m_TNT = GameObject.Find("LandMine");
		}
	}

	public override void ResetData()
	{
		base.ResetData();
		m_bUnderWayAnim = false;
		m_bMarketAnim = false;
		m_bTruckRushAnim = false;
		m_bTruckBoom = false;
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
		m_SearchLightL1_1.active = false;
		m_SearchLightL1_2.active = false;
		m_SearchLightL2_1.active = false;
		m_SearchLightL2_2.active = false;
		m_SearchLightL3_1.active = false;
		m_SearchLightL3_2.active = false;
		m_SearchLightL4_1.active = false;
		m_SearchLightL4_2.active = false;
		m_SearchLightP1.active = false;
		m_SearchLightP2.active = false;
		m_SearchLightP3.active = false;
		m_SearchLightP4.active = false;
		m_SearchLightP1.GetComponent<Animation>().Stop("light_01");
		m_SearchLightP2.GetComponent<Animation>().Stop("light_02");
		m_SearchLightP3.GetComponent<Animation>().Stop("light_01");
		m_SearchLightP4.GetComponent<Animation>().Stop("light_02");
		if (m_iBunkerScript != null)
		{
			PlaySceneAnim(m_iBunkerScript.gameObject, "daoda", true, 1f);
		}
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
		m_fMarketAnimTime = 0f;
		m_fUnderWayAnimTime = 0f;
		m_fTruckAnimTime = 0f;
		m_fTruckAnimRushTime = 0f;
		m_fSlowTimeRate = 0f;
	}

	public override void Destroy()
	{
		if (m_PlayerPlane != null)
		{
			ClearGameObjectAudio(m_PlayerPlane);
			Object.Destroy(m_PlayerPlane);
			m_PlayerPlane = null;
		}
		if (m_CGOP != null)
		{
			Object.Destroy(m_CGOP);
			m_CGOP = null;
		}
		if (m_CGStation != null)
		{
			Object.Destroy(m_CGStation);
			m_CGStation = null;
		}
		if (m_iBunkerScript != null)
		{
			m_iBunkerScript.Destroy();
			m_iBunkerScript = null;
		}
		if (m_Truck != null)
		{
			Object.Destroy(m_Truck);
			m_Truck = null;
		}
		if (m_TruckDestroy != null)
		{
			Object.Destroy(m_TruckDestroy);
			m_TruckDestroy = null;
		}
		if (m_NewTruck != null)
		{
			ClearGameObjectAudio(m_NewTruck);
			Object.Destroy(m_NewTruck);
			m_NewTruck = null;
		}
		if (m_Market != null)
		{
			ClearGameObjectAudio(m_Market);
			Object.Destroy(m_Market);
			m_Market = null;
		}
		if (m_MarketDestroy != null)
		{
			ClearGameObjectAudio(m_MarketDestroy);
			Object.Destroy(m_MarketDestroy);
			m_MarketDestroy = null;
		}
		if (m_TruckFire != null)
		{
			ClearGameObjectAudio(m_TruckFire);
			Object.Destroy(m_TruckFire);
			m_TruckFire = null;
		}
		base.Destroy();
	}

	public override void StartGame()
	{
		ResetData();
		AddOilCan(50f, 10f);
		AddOilCan(50f, 10f);
		AddOilCan(50f, 10f);
		AddOilCan(50f, 10f);
		AddOilCan(50f, 10f);
		AddOilCan(50f, 10f);
		if (m_bCG)
		{
			m_fMovieTime = 0f;
			m_CameraScript.gameObject.transform.parent = null;
			m_CameraScript.SetPosition(new Vector3(10f, 1.97f, 16.8f));
			m_CameraScript.transform.forward = new Vector3(0.177f, 0.055f, -0.98f);
			m_SoliderMesh.SetActiveRecursively(false);
			m_GameSceneUI.ShowCross(false);
			m_GameSceneUI.ShowGameUI(false);
			if (m_PlayerPlane != null && m_PlayerPlane.GetComponent<Animation>() != null)
			{
				m_PlayerPlane.SetActiveRecursively(true);
				if (m_PlayerPlane.GetComponent<Animation>()["fly02"] != null)
				{
					m_PlayerPlane.GetComponent<Animation>()["fly02"].layer = -1;
					m_PlayerPlane.GetComponent<Animation>()["fly02"].wrapMode = WrapMode.Loop;
					m_PlayerPlane.GetComponent<Animation>()["fly02"].time = 0f;
					m_PlayerPlane.GetComponent<Animation>().Play("fly02");
				}
				if (m_PlayerPlane.GetComponent<Animation>()["fly01"] != null)
				{
					m_PlayerPlane.GetComponent<Animation>()["fly01"].time = 0f;
					m_PlayerPlane.GetComponent<Animation>().CrossFade("fly01");
				}
			}
			m_CameraMove.Initialize(m_CameraScript.GetComponent<Camera>(), false);
			Transform transform = m_PathManager.transform.Find("PathList/CGPath");
			if (transform != null)
			{
				CPathPara component = transform.GetComponent<CPathPara>();
				if (component != null)
				{
					CMoveBase cMoveBase = null;
					for (int i = 0; i < component.m_ltPoint.Count; i++)
					{
						cMoveBase = component.m_ltPoint[i].GetComponent<CMoveBase>();
						if (!(cMoveBase == null))
						{
							switch (cMoveBase.m_State)
							{
							case CMoveBase.MoveType.Stand:
							{
								CMoveStand cMoveStand = (CMoveStand)cMoveBase;
								m_CameraMove.AddStand(cMoveStand.m_fTime);
								break;
							}
							case CMoveBase.MoveType.Move:
							{
								CMoveGo cMoveGo = (CMoveGo)cMoveBase;
								m_CameraMove.AddMove(cMoveGo.m_v3Pos, cMoveGo.m_v3Dir, cMoveGo.m_fSpeed);
								break;
							}
							case CMoveBase.MoveType.Rotate:
							{
								CMoveRotate cMoveRotate = (CMoveRotate)cMoveBase;
								m_CameraMove.AddRotate(cMoveRotate.m_v3Dir, cMoveRotate.m_fSpeed);
								break;
							}
							}
						}
					}
				}
				m_CameraMove.Start(iZombieSniperMoonWalk.MoonWalkType.Begin);
			}
			m_BackGroundSound.active = false;
			if (m_CGOP == null)
			{
				m_CGOP = (GameObject)Object.Instantiate(m_PerfabManager.CGOP);
			}
			m_CGOP.SetActiveRecursively(false);
			SetGameState(State.kGameMovie);
		}
		else
		{
			m_CameraMove = null;
			m_PathMover.Initialize(string.Empty);
			m_PathMover.StartMove();
			m_CameraScript.gameObject.transform.parent = m_PlaneObject.transform;
			m_CameraScript.gameObject.transform.localPosition = Vector3.zero;
			m_CameraScript.gameObject.transform.forward = m_PlaneObject.transform.forward;
			m_ZombieWaveCenter.LoadZombieWaveInfoList(m_GameState.m_nCurStage);
			m_ZobieWaveManager.Initialize();
			m_ZombieWaveCenter.LoadLogicWaveInfo(m_GameState.m_nCurStage);
			m_ZobieWaveManagerLogic.Initialize();
			SetGameState(State.kGaming);
			iZombieSniperGameApp.GetInstance().m_bSoundSwitch = true;
			iZombieSniperGameApp.GetInstance().ClearAudio("MusicMap02");
			PlayAudio("MusicMap02");
			m_BackGroundSound.active = m_GameState.m_bSoundOn;
			if (m_CGStation == null)
			{
				m_CGStation = (GameObject)Object.Instantiate(m_PerfabManager.CGStation, Vector3.zero, Quaternion.identity);
			}
			m_CGStation.SetActiveRecursively(false);
		}
	}

	public override void UpdateMovie(float deltaTime)
	{
		m_fMovieTime += deltaTime;
		if (m_fMovieTime > 5f && m_CGOP != null)
		{
			m_CGOP.SetActiveRecursively(true);
		}
		m_CameraMove.Update(deltaTime);
		if (m_CameraMove.IsFinished(iZombieSniperMoonWalk.MoonWalkType.Begin))
		{
			if (m_CGOP != null)
			{
				Object.Destroy(m_CGOP);
				m_CGOP = null;
			}
			SetPlayCG(false);
			Destroy();
			Initialize();
			ResetData();
			StartGame();
			return;
		}
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (!(m_GameSceneUI != null) || m_GameSceneUI.m_UIManagerRef.HandleInput(touch))
			{
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
			if (m_bAlarmScreen)
			{
				m_fAlarmRefreshTime -= deltaTime;
				if (m_fAlarmRefreshTime <= 0f)
				{
					m_fAlarmRefreshTime = 1f;
					Vector2 vector = m_CameraScript.m_Camera.WorldToScreenPoint(m_v3EndPoint);
					float num = Vector3.Dot(m_CameraScript.transform.forward, (m_v3EndPoint - m_CameraScript.GetPosition()).normalized);
					if ((num > 0f && vector.x > m_GameState.GetShootCenter().x) || (num <= 0f && vector.x < m_GameState.GetShootCenter().x))
					{
						if (m_AlarmSide == iZombieSniperGameUI.AlarmScreenSide.Left || m_AlarmSide == iZombieSniperGameUI.AlarmScreenSide.None)
						{
							m_GameSceneUI.StartAlarmScreen(iZombieSniperGameUI.AlarmScreenSide.Right);
							m_GameSceneUI.StopAlarmScreen(iZombieSniperGameUI.AlarmScreenSide.Left);
							m_AlarmSide = iZombieSniperGameUI.AlarmScreenSide.Right;
						}
					}
					else if (m_AlarmSide == iZombieSniperGameUI.AlarmScreenSide.Right || m_AlarmSide == iZombieSniperGameUI.AlarmScreenSide.None)
					{
						m_GameSceneUI.StartAlarmScreen(iZombieSniperGameUI.AlarmScreenSide.Left);
						m_GameSceneUI.StopAlarmScreen(iZombieSniperGameUI.AlarmScreenSide.Right);
						m_AlarmSide = iZombieSniperGameUI.AlarmScreenSide.Left;
					}
				}
			}
			else if (m_AlarmSide != 0)
			{
				m_GameSceneUI.StopAlarmScreen(iZombieSniperGameUI.AlarmScreenSide.All);
				m_AlarmSide = iZombieSniperGameUI.AlarmScreenSide.None;
				m_fAlarmRefreshTime = 1f;
			}
			if (flag != m_bAlarmScreen)
			{
				m_bAlarmScreen = flag;
				if (m_bAlarmScreen)
				{
					m_GameSceneUI.AddImageWarn1(ImageMsgEnum1.ZOMBIE_CLOSE);
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
					if (m_SearchLightL3_1 != null && m_SearchLightL3_2 != null && m_SearchLightP3 != null)
					{
						m_SearchLightL3_1.active = true;
						m_SearchLightL3_2.active = true;
						m_SearchLightP3.active = true;
						if (m_SearchLightP3.GetComponent<Animation>() != null && m_SearchLightP3.GetComponent<Animation>()["light_01"] != null)
						{
							m_SearchLightP3.GetComponent<Animation>()["light_01"].wrapMode = WrapMode.Loop;
							m_SearchLightP3.GetComponent<Animation>().Play("light_01");
						}
					}
					if (m_SearchLightL4_1 != null && m_SearchLightL4_2 != null && m_SearchLightP4 != null)
					{
						m_SearchLightL4_1.active = true;
						m_SearchLightL4_2.active = true;
						m_SearchLightP4.active = true;
						if (m_SearchLightP4.GetComponent<Animation>() != null && m_SearchLightP4.GetComponent<Animation>()["light_02"] != null)
						{
							m_SearchLightP4.GetComponent<Animation>()["light_02"].wrapMode = WrapMode.Loop;
							m_SearchLightP4.GetComponent<Animation>().Play("light_02");
						}
					}
				}
				else
				{
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
					if (m_SearchLightL3_1 != null && m_SearchLightL3_2 != null && m_SearchLightP3 != null)
					{
						if (m_SearchLightP3.GetComponent<Animation>() != null && m_SearchLightP3.GetComponent<Animation>()["light_01"] != null)
						{
							m_SearchLightP3.GetComponent<Animation>().Stop("light_01");
						}
						m_SearchLightL3_1.active = false;
						m_SearchLightL3_2.active = false;
						m_SearchLightP3.active = false;
					}
					if (m_SearchLightL4_1 != null && m_SearchLightL4_2 != null && m_SearchLightP4 != null)
					{
						if (m_SearchLightP4.GetComponent<Animation>() != null && m_SearchLightP4.GetComponent<Animation>()["light_02"] != null)
						{
							m_SearchLightP4.GetComponent<Animation>().Stop("light_02");
						}
						m_SearchLightL4_1.active = false;
						m_SearchLightL4_2.active = false;
						m_SearchLightP4.active = false;
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
			m_ZobieWaveManager.Update(deltaTime);
			m_ZobieWaveManagerLogic.Update(deltaTime);
			UpdateGameEvent(deltaTime);
			UpdateCurrWeapon(deltaTime);
			UpdateAutoShoot(deltaTime);
			if (m_CameraMove != null)
			{
				m_CameraMove.Update(deltaTime);
			}
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

	public override void UpdateGameEvent(float deltaTime)
	{
		if (!m_bMarketAnim && m_fGameTimeTotal > 360f)
		{
			m_bMarketAnim = true;
			if (m_GameState.m_bCutScenes)
			{
				m_fSlowTimeRate = 0.1f;
				m_fMarketAnimTime = PlaySceneAnim(m_Market, "polie", false, 1f / m_fSlowTimeRate);
				SetPauseScale(true, m_fSlowTimeRate);
				m_PathMover.Pause();
				StartSlow(new Vector3(51.37f, 0.78f, 14.45f), new Vector3(-16f, 117f, 0f), 60f);
				m_GameSceneUI.ShowMovieUI();
			}
			else
			{
				m_fSlowTimeRate = 1f;
				m_fMarketAnimTime = 10f;
			}
			PlayAudio("MusicMap02");
		}
		if (!m_bUnderWayAnim && m_fGameTimeTotal > 180f)
		{
			m_bUnderWayAnim = true;
			PlayAudio("MonZombieIdleGroup01");
			if (m_GameState.m_bCutScenes)
			{
				m_fSlowTimeRate = 0.1f;
				m_fUnderWayAnimTime = 40f * m_fSlowTimeRate;
				if (m_CGStation != null)
				{
					m_CGStation.SetActiveRecursively(true);
				}
				SetPauseScale(true, m_fSlowTimeRate);
				m_PathMover.Pause();
				StartSlow(new Vector3(-15.8f, 0.78f, -13.34f), new Vector3(-11f, 287f, -1.5f), 60f);
				m_GameSceneUI.ShowMovieUI();
			}
			else
			{
				m_fSlowTimeRate = 1f;
				m_fUnderWayAnimTime = 10f;
			}
			PlayAudio("MusicMap02");
		}
		if (!m_bTruckRushAnim && m_fGameTimeTotal > 120f)
		{
			m_bTruckRushAnim = true;
			if (m_NewTruck == null)
			{
				m_NewTruck = (GameObject)Object.Instantiate(m_PerfabManager.TruckScene1, Vector3.zero, Quaternion.identity);
				m_NewTruck.name = "TruckScene1";
			}
			if (m_NewTruck != null && m_NewTruck.GetComponent<Animation>() != null && m_NewTruck.GetComponent<Animation>()["Take 001"] != null)
			{
				m_NewTruck.GetComponent<Animation>()["Take 001"].time = 0f;
				m_NewTruck.GetComponent<Animation>().Play("Take 001");
				m_NewTruck.GetComponent<Animation>().Sample();
				m_NewTruck.GetComponent<Animation>()["Take 001"].speed = 0.5f;
				m_NewTruck.GetComponent<Animation>().Play("Take 001");
				m_fTruckAnimRushTime = m_NewTruck.GetComponent<Animation>()["Take 001"].length / 0.5f;
			}
		}
		if (m_fTruckAnimRushTime > 0f)
		{
			m_fTruckAnimRushTime -= deltaTime;
			if (m_fTruckAnimRushTime <= 0f && m_NewTruck != null && m_NewTruck.GetComponent<Animation>() != null && m_NewTruck.GetComponent<Animation>()["Take 001"] != null)
			{
				m_NewTruck.GetComponent<Animation>()["Take 001"].time = m_NewTruck.GetComponent<Animation>()["Take 001"].length;
				m_NewTruck.GetComponent<Animation>().Play("Take 001");
				m_NewTruck.GetComponent<Animation>().Sample();
				if (m_iTruckScript == null)
				{
					m_iTruckScript = m_NewTruck.GetComponent<iZombieSniperTruck>();
				}
				m_iTruckScript.Initialize(20f);
			}
		}
		if (m_bTruckBoom)
		{
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
				if (m_Truck != null)
				{
					Object.Destroy(m_Truck);
					m_Truck = null;
				}
				if (m_TruckDestroy == null)
				{
					m_TruckDestroy = (GameObject)Object.Instantiate(m_PerfabManager.truck_01);
				}
			}
		}
		if (m_bMarketAnim && m_fMarketAnimTime > 0f)
		{
			m_fMarketAnimTime -= deltaTime * (1f / m_fSlowTimeRate);
			if (m_fMarketAnimTime <= 0f)
			{
				m_fMarketAnimTime = 0f;
				if (m_Market != null)
				{
					ClearGameObjectAudio(m_Market);
					Object.Destroy(m_Market);
					m_Market = null;
				}
				if (m_MarketDestroy == null)
				{
					m_MarketDestroy = (GameObject)Object.Instantiate(m_PerfabManager.Market_01);
				}
				if (m_bSlowScale)
				{
					SetPauseScale(false, 1f);
					EndSlow();
					m_PathMover.Resume();
					m_GameSceneUI.HideMovieUI();
				}
			}
		}
		if (!m_bUnderWayAnim || !(m_fUnderWayAnimTime > 0f))
		{
			return;
		}
		m_fUnderWayAnimTime -= deltaTime * (1f / m_fSlowTimeRate);
		if (m_fUnderWayAnimTime <= 0f)
		{
			m_fUnderWayAnimTime = 0f;
			if (m_CGStation != null)
			{
				Object.Destroy(m_CGStation);
				m_CGStation = null;
			}
			if (m_bSlowScale)
			{
				SetPauseScale(false, 1f);
				EndSlow();
				m_PathMover.Resume();
				m_GameSceneUI.HideMovieUI();
			}
		}
	}

	public override void ReadyGameOver(Vector3 v3LookPoint)
	{
		m_PathMover.Pause();
		base.ReadyGameOver(v3LookPoint);
		m_AlarmSide = iZombieSniperGameUI.AlarmScreenSide.None;
		m_fAlarmRefreshTime = 1f;
		if (v3LookPoint != Vector3.zero)
		{
			m_fGameOveringTime = 2f;
			m_CameraScript.transform.forward = v3LookPoint - m_CameraScript.GetPosition();
			m_CameraScript.HeadShotCamera();
		}
		else
		{
			m_CameraScript.SetPosition(new Vector3(37f, 3.45f, -34.2f));
			m_CameraScript.transform.eulerAngles = new Vector3(-10.8f, 488.6f, 0f);
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

	public override void FireHit(Ray ray, RaycastHit hit, bool bMakeSound)
	{
		base.FireHit(ray, hit, bMakeSound);
		if (hit.transform.root.name == "TruckScene1")
		{
			AddGunHitEffect(hit.point);
			if (m_iTruckScript != null && !m_iTruckScript.IsDead())
			{
				m_iTruckScript.AddHP(0f - m_CurrWeapon.m_fSD);
			}
			PlayAudio("FxMetalHit01");
		}
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
		if (m_BackGroundSound != null)
		{
			m_BackGroundSound.active = false;
		}
		base.GameOver();
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

	public override Vector3 GetAC130Start()
	{
		return new Vector3(-100f, 50f, -90f);
	}

	public override Vector2 GetAC130Range()
	{
		return new Vector2(170f, 160f);
	}

	public override Vector2 GetAC130Space()
	{
		return new Vector2(20f, 20f);
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

	public override bool IsSuperMarketAnim()
	{
		return m_bMarketAnim;
	}

	public override bool IsUnderWayAnim()
	{
		return m_bUnderWayAnim;
	}

	public override GameObject GetTNT()
	{
		return m_TNT;
	}

	public override bool IsPlayCG()
	{
		return m_bCG;
	}

	public override void SetPlayCG(bool bValue)
	{
		m_bCG = bValue;
	}

	public override void TruckBoom()
	{
		if (m_NewTruck != null)
		{
			ClearGameObjectAudio(m_NewTruck);
			Object.Destroy(m_NewTruck);
			m_NewTruck = null;
		}
		if (m_Truck == null)
		{
			m_Truck = (GameObject)Object.Instantiate(m_PerfabManager.truck);
			m_v3TruckPos = m_Truck.transform.position;
			m_fTruckAnimTime = PlaySceneAnim(m_Truck, "truck", false, 1f);
		}
		GameObject obj = (GameObject)Object.Instantiate(m_PerfabManager.m_TruckBoom, m_v3TruckPos, Quaternion.identity);
		Object.Destroy(obj, 2f);
		m_bTruckBoom = true;
		Boom(m_v3TruckPos, 30f, 200f);
		SetFireSceneShow(true);
		m_CameraScript.Shake(1.5f, m_fTruckAnimTime);
		m_TruckFire.SetActiveRecursively(true);
		m_TruckFire.transform.position = m_v3TruckPos + new Vector3(0f, 3f, 0f);
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

	public override void SetPauseAudio(bool bPause)
	{
		if (bPause)
		{
			if (m_bAlarmScreen)
			{
				StopAudio("FxAlarmLoop01");
			}
			if (m_fGameTimeTotal < 30f)
			{
				StopAudio("MusicMap02");
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
			PlayAudio("MusicMap02");
		}
		if (m_GameState.m_bSoundOn)
		{
			m_BackGroundSound.active = true;
		}
	}
}
