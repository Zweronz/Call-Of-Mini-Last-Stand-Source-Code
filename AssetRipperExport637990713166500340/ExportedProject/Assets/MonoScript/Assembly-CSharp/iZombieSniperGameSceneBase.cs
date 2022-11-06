using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iZombieSniperGameSceneBase
{
	public enum State
	{
		kGameReady = 0,
		kGameMoonWalk = 1,
		kGameTurtorial = 2,
		kGameStart = 3,
		kGaming = 4,
		kGameOvering = 5,
		kGameOver = 6,
		kGameMovie = 7
	}

	public enum MineState
	{
		NONE = 0,
		READY = 1,
		USED = 2
	}

	private bool ShouldLock;

	public State m_State;

	public iZombieSniperCamera m_CameraScript;

	public iZombieSniperGameState m_GameState;

	public iZombieSniperGunCenter m_GunCenter;

	public iZombieSniperWayPointCenter m_WayPointCenter;

	public iZombieSniperGridInfoCenter m_GridInfoCenter;

	public iZombieSniperGameUI m_GameSceneUI;

	public iZombieSniperZombieWaveCenter m_ZombieWaveCenter;

	public CZombieWaveManager m_ZobieWaveManager;

	public CZombieWaveManagerLogic m_ZobieWaveManagerLogic;

	public TAudioController m_AudioController;

	public iZombieSniperPerfabManager m_PerfabManager;

	public bool m_bTutorial;

	public bool m_bCG;

	protected bool m_bIsDragMouse;

	protected bool m_bAim;

	protected bool m_bAimAfterReload;

	protected bool m_bAimAction;

	protected float m_fAimTimeCount;

	protected bool m_bAutoShoot;

	protected GameObject m_ShootPos;

	protected bool m_bPause;

	protected float m_fGameOveringTime;

	public int m_nMaxKillInnoNum;

	public Vector3 m_v3SniperPosition;

	public Vector3 m_v3SniperLookAt;

	public Vector3 m_v3SniperForwards;

	public float m_fSniperFov;

	public iWeapon m_CurrWeapon;

	public int m_nCurrWeaponID = -1;

	public GameObject m_Solider;

	public GameObject m_SoliderMesh;

	public SkinnedMeshRenderer m_CartridgeClip;

	public UVAnim m_CartridgeAnim;

	public float m_fCurCrossDis;

	public float m_fCrossDis;

	public float m_fCurCrossBackSpeed;

	public float m_fCrossBackSpeed;

	protected bool m_bWeaponSwitching;

	protected int m_nNewWeapon;

	protected int m_nOldWeapon;

	protected float m_fSwitchWeaponCount;

	protected float m_fSwitchWeaponTime;

	protected float m_fWaitReload;

	public float m_fGameTimeTotal;

	public bool m_bGameOver;

	public Vector3 m_v3LastDeadInno;

	public Vector3 m_v3EndPoint;

	public float m_fWarningDis;

	public GameObject m_BackGroundSound;

	public bool m_bAlarmScreen;

	protected static int m_nUID = 1;

	public Dictionary<int, iZombieSniperNpc> m_NPCMap;

	protected Dictionary<int, iZombieSniperOilCan> m_OilCanMap;

	protected ArrayList m_OilCanEmptyList;

	public bool m_bUsedACE;

	public MineState m_MineState;

	public List<iZombieSniperThrowMine> m_ltThrowMine;

	public CMemoryPool m_BulletPool;

	public CMemoryPool m_GunFlightPool;

	public CMemoryPool m_GunHitPool;

	public CMemoryPool m_BloodPool;

	public bool m_isSlow;

	private float m_fCinderHeight;

	private int m_nAutoShootCount;

	private bool m_bFirstShoot = true;

	public float m_fOilCanRefreshCount;

	private float m_fCollideCount = 2f;

	protected bool m_bSlowScale;

	protected float m_nSlowScale = 1f;

	public Vector3 m_v3CameraCurrPos;

	public Vector3 m_v3CameraCurrEuler;

	public float m_fCameraCurrFov;

	public bool m_bCameraCurrAim;

	public iZombieSniperCamera.CameraState m_CameraCurrState;

	public bool m_isShowDefenceUI;

	private Workarounds m_workArounds;

	public Workarounds WorkArounds
	{
		get
		{
			if (m_workArounds == null)
			{
				m_workArounds = GameObject.Find("Workarounds").GetComponent<Workarounds>();
			}
			return m_workArounds;
		}
	}

	public virtual void Initialize()
	{
		if (m_CameraScript == null)
		{
			GameObject gameObject = GameObject.Find("Main Camera");
			m_CameraScript = gameObject.GetComponent<iZombieSniperCamera>();
			m_AudioController = m_CameraScript.GetComponent<TAudioController>();
		}
		m_v3SniperPosition = ConstantValue.m_v3SniperPosition;
		m_v3SniperLookAt = ConstantValue.m_v3SniperLookAt;
		m_fSniperFov = 45f;
		m_CameraScript.Initialize();
		m_CameraScript.transform.LookAt(m_v3SniperLookAt);
		m_v3SniperForwards = m_CameraScript.transform.forward;
		if (m_GameState == null)
		{
			m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
			m_bTutorial = m_GameState.m_bTutorial;
			m_bCG = m_GameState.GetCGFlag(m_GameState.m_nCurStage);
		}
		m_GameState.ResetGameDate();
		if (m_GunCenter == null)
		{
			m_GunCenter = iZombieSniperGameApp.GetInstance().m_GunCenter;
		}
		if (m_WayPointCenter == null)
		{
			m_WayPointCenter = iZombieSniperGameApp.GetInstance().m_WayPointCenter;
		}
		if (m_GridInfoCenter == null)
		{
			m_GridInfoCenter = iZombieSniperGameApp.GetInstance().m_GridInfoCenter;
		}
		m_CurrWeapon = m_GameState.GetUserWeapon();
		m_nMaxKillInnoNum = ((m_GameState.m_nInnoKiller <= 0) ? 3 : 6);
		if (m_Solider == null)
		{
			m_Solider = GameObject.Find("Soldier");
		}
		if (m_GameSceneUI == null)
		{
			GameObject gameObject2 = GameObject.Find("Main Camera");
			m_GameSceneUI = gameObject2.GetComponent<iZombieSniperGameUI>();
		}
		m_bUsedACE = false;
		m_GameSceneUI.Initialize();
		if (m_ZombieWaveCenter == null)
		{
			m_ZombieWaveCenter = iZombieSniperGameApp.GetInstance().m_ZombieWaveCenter;
		}
		if (m_ZobieWaveManager == null)
		{
			m_ZobieWaveManager = new CZombieWaveManager();
		}
		if (m_ZobieWaveManagerLogic == null)
		{
			m_ZobieWaveManagerLogic = new CZombieWaveManagerLogic();
		}
		if (m_PerfabManager == null)
		{
			GameObject gameObject3 = GameObject.Find("PerfabManager");
			m_PerfabManager = gameObject3.GetComponent<iZombieSniperPerfabManager>();
		}
		if (m_NPCMap == null)
		{
			m_NPCMap = new Dictionary<int, iZombieSniperNpc>();
		}
		if (m_OilCanMap == null)
		{
			m_OilCanMap = new Dictionary<int, iZombieSniperOilCan>();
		}
		if (m_OilCanEmptyList == null)
		{
			m_OilCanEmptyList = new ArrayList();
		}
		m_v3EndPoint = Vector3.zero;
		m_fWarningDis = 0f;
		if (m_BackGroundSound == null)
		{
			m_BackGroundSound = GameObject.Find("Map01");
		}
		if (m_BackGroundSound != null)
		{
			m_BackGroundSound.active = m_GameState.m_bSoundOn;
		}
		if (m_ltThrowMine == null)
		{
			m_ltThrowMine = new List<iZombieSniperThrowMine>();
		}
		if (m_BulletPool == null)
		{
			m_BulletPool = new CMemoryPool();
			m_BulletPool.Initialize(m_PerfabManager.BulletEffect, 10);
		}
		if (m_GunFlightPool == null)
		{
			m_GunFlightPool = new CMemoryPool();
			m_GunFlightPool.Initialize(m_PerfabManager.GunFlightEffect, 5);
		}
		if (m_GunHitPool == null)
		{
			m_GunHitPool = new CMemoryPool();
			m_GunHitPool.Initialize(m_PerfabManager.GunHitEffect, 15);
		}
		if (m_BloodPool == null)
		{
			m_BloodPool = new CMemoryPool();
			m_BloodPool.Initialize(m_PerfabManager.BloodEffect, 10);
		}
		InitCross(6f, 10f);
		CrossFat(5f, 5f);
	}

	public virtual void Destroy()
	{
		if (m_NPCMap != null)
		{
			ClearNPC();
			m_NPCMap = null;
		}
		if (m_OilCanMap != null)
		{
			ClearOilCan();
			m_OilCanMap = null;
		}
		if (m_OilCanEmptyList != null)
		{
			m_OilCanEmptyList.Clear();
			m_OilCanEmptyList = null;
		}
		if (m_ltThrowMine != null)
		{
			m_ltThrowMine.Clear();
			m_ltThrowMine = null;
		}
		m_WayPointCenter = null;
		if (m_ZobieWaveManager != null)
		{
			m_ZobieWaveManager.Destroy();
			m_ZobieWaveManager = null;
		}
		if (m_ZobieWaveManagerLogic != null)
		{
			m_ZobieWaveManagerLogic.Destroy();
			m_ZobieWaveManagerLogic = null;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Rocket");
		GameObject[] array2 = array;
		foreach (GameObject obj in array2)
		{
			UnityEngine.Object.Destroy(obj);
		}
		array = GameObject.FindGameObjectsWithTag("ThrowMine");
		GameObject[] array3 = array;
		foreach (GameObject obj2 in array3)
		{
			UnityEngine.Object.Destroy(obj2);
		}
		if (m_BulletPool != null)
		{
			m_BulletPool.Destroy();
			m_BulletPool = null;
		}
		if (m_GunFlightPool != null)
		{
			m_GunFlightPool.Destroy();
			m_GunFlightPool = null;
		}
		if (m_GunHitPool != null)
		{
			m_GunHitPool.Destroy();
			m_GunHitPool = null;
		}
		if (m_BloodPool != null)
		{
			m_BloodPool.Destroy();
			m_BloodPool = null;
		}
		GC.Collect();
		Resources.UnloadUnusedAssets();
	}

	public virtual void ResetData()
	{
		m_fGameTimeTotal = 0f;
		m_bGameOver = false;
		m_v3LastDeadInno = Vector3.zero;
		m_bIsDragMouse = false;
		m_bAim = false;
		m_bAimAfterReload = false;
		m_bAimAction = false;
		m_fAimTimeCount = 0f;
		m_bAutoShoot = false;
		m_bUsedACE = false;
		m_nCurrWeaponID = -1;
		m_bAlarmScreen = false;
		m_fGameOveringTime = 0f;
		SetPause(false);
		SetPauseScale(false, 1f);
		m_fWaitReload = 0f;
		m_OilCanEmptyList.Clear();
		foreach (OilCanPoint value in m_WayPointCenter.m_OilCanConfig.Values)
		{
			m_OilCanEmptyList.Add(value.m_nID);
		}
		StopAudio("MusicMap01");
		StopAudio("MusicMap02");
		m_bWeaponSwitching = false;
		m_nNewWeapon = -1;
		m_nOldWeapon = -1;
		m_fSwitchWeaponCount = 0f;
		m_fSwitchWeaponTime = 0f;
		for (int i = 0; i < 3; i++)
		{
			if (m_GameState.GetCarryWeapon(i) > 0)
			{
				SwitchWeapon(i);
				break;
			}
		}
		m_isSlow = false;
		OpenClikPlugin.Hide();
	}

	public virtual void StartGame()
	{
		ResetData();
		m_ZombieWaveCenter.LoadZombieWaveInfoList(m_GameState.m_nCurStage);
		m_ZobieWaveManager.Initialize();
		m_ZombieWaveCenter.LoadLogicWaveInfo(m_GameState.m_nCurStage);
		m_ZobieWaveManagerLogic.Initialize();
		SetGameState(State.kGaming);
		iZombieSniperGameApp.GetInstance().m_bSoundSwitch = true;
		iZombieSniperGameApp.GetInstance().ClearAudio("MusicMap01");
		iZombieSniperGameApp.GetInstance().ClearAudio("MusicMap02");
	}

	public virtual void ReadyGameOver(Vector3 v3LookPoint)
	{
		m_GameSceneUI.StopAlarmScreen(iZombieSniperGameUI.AlarmScreenSide.All);
		m_GameSceneUI.HideBloodScreen();
		StopAudio("FxAlarmLoop01");
		if (m_bAim && m_CameraScript.CloseAim())
		{
			m_bAim = false;
			m_GameSceneUI.ShowCross(true);
			m_GameSceneUI.HideAimUI();
			m_SoliderMesh.SetActiveRecursively(true);
		}
		SetAutoShoot(false);
		if (m_bWeaponSwitching)
		{
			FinishSwitchWeapon();
		}
		UnityEngine.Object.Destroy(m_SoliderMesh);
		m_SoliderMesh = null;
		m_GameSceneUI.ShowCross(false);
		m_GameSceneUI.ShowGameUI(false);
		m_GameSceneUI.ShowWarn(false);
		m_CameraScript.ShakeEnd();
		m_CameraScript.Restore();
		SetGameState(State.kGameOvering);
	}

	public virtual void GameOver()
	{
		m_GameState.CaculatePrice(m_fGameTimeTotal);
		m_GameState.SaveData();
		m_GameSceneUI.ShowGameResult(true);
		Destroy();
		iZombieSniperGameApp.GetInstance().m_bSoundSwitch = false;
		iZombieSniperGameApp.GetInstance().ClearAudio("MusicMenu");
		iZombieSniperGameApp.GetInstance().PlayAudio("MusicMenu");
		SetGameState(State.kGameOver);
		OpenClikPlugin.Show(false);
	}

	public virtual void Update(float deltaTime)
	{
		if (Input.GetKeyDown(KeyCode.F1))
		ShouldLock = !ShouldLock;
		UpdateSwitchWeapon(deltaTime);
		UpdateCrossAnim(deltaTime);
		if (m_BulletPool != null)
		{
			m_BulletPool.Update(deltaTime);
		}
		if (m_GunFlightPool != null)
		{
			m_GunFlightPool.Update(deltaTime);
		}
		if (m_GunHitPool != null)
		{
			m_GunHitPool.Update(deltaTime);
		}
		if (m_BloodPool != null)
		{
			m_BloodPool.Update(deltaTime);
		}
		if (m_State == State.kGameMoonWalk)
		{
			UpdateMoonWalk(deltaTime);
		}
		else if (m_State == State.kGameTurtorial)
		{
			UpdateTutorial(deltaTime);
		}
		else if (m_State == State.kGameMovie)
		{
			UpdateMovie(deltaTime);
		}
		else if (m_State == State.kGaming)
		{
			UpdateGameing(deltaTime);
		}
		else if (m_State == State.kGameOvering)
		{
			foreach (iZombieSniperNpc value in m_NPCMap.Values)
			{
				value.Update(deltaTime);
			}
			m_fGameOveringTime -= deltaTime;
			if (m_fGameOveringTime <= 0f)
			{
				m_fGameOveringTime = 0f;
				GameOver();
			}
			UITouchInner[] array = (Application.isMobilePlatform) ? iPhoneInputMgr.MockTouches() : WindowsInputMgr.MockTouches();
			foreach (UITouchInner touch in array)
			{
				if (m_GameSceneUI.m_UIManagerRef.HandleInput(touch))
				{
				}
			}
		}
		else
		{
			if (m_State != State.kGameOver)
			{
				return;
			}
			UITouchInner[] array2 = iPhoneInputMgr.MockTouches();
			foreach (UITouchInner touch2 in array2)
			{
				if (!(m_GameSceneUI != null) || m_GameSceneUI.m_UIManagerRef.HandleInput(touch2))
				{
				}
			}
		}
	}

	public virtual void UpdateMoonWalk(float deltaTime)
	{
	}

	public virtual void UpdateTutorial(float deltaTime)
	{
	}

	public virtual void UpdateMovie(float deltaTime)
	{
	}

	public virtual void UpdateGameing(float deltaTime)
	{
		m_fGameTimeTotal += deltaTime;
		m_GameSceneUI.SetGameTime(m_fGameTimeTotal);
		if (!Application.isMobilePlatform)
		WindowsInput();
	}

	public void Aim(Vector2 v2AimPos)
	{
		if (!m_bAimAction && m_CurrWeapon.IsCanFire() && m_fWaitReload == 0f)
		{
			m_bAimAction = true;
			if (IsSwitching())
			{
				FinishSwitchWeapon();
			}
			if (m_SoliderMesh.GetComponent<Animation>()["UpFire01"] != null)
			{
				m_SoliderMesh.GetComponent<Animation>()["UpFire01"].time = 0f;
				m_SoliderMesh.GetComponent<Animation>().Play("UpFire01");
				m_fAimTimeCount = m_SoliderMesh.GetComponent<Animation>()["UpFire01"].length;
				m_fAimTimeCount /= 2f;
			}
			if (m_SoliderMesh.GetComponent<Animation>()["Idle01"] != null)
			{
				m_SoliderMesh.GetComponent<Animation>().clip = m_SoliderMesh.GetComponent<Animation>()["Idle01"].clip;
				m_SoliderMesh.GetComponent<Animation>().playAutomatically = true;
			}
			if (v2AimPos != m_GameState.GetShootCenter())
			{
				m_CameraScript.AimLook(v2AimPos, 1f / m_fAimTimeCount + 0.2f);
			}
		}
	}

	public void CloseAim(bool actuallyDoIt)
	{
		if (!Application.isMobilePlatform && actuallyDoIt || Application.isMobilePlatform)
		{
			if (m_CameraScript.CloseAim())
			{
				m_bAim = false;
				m_GameSceneUI.ShowCross(true);
				m_GameSceneUI.HideAimUI();
				m_SoliderMesh.SetActiveRecursively(true);
				if (m_SoliderMesh.GetComponent<Animation>()["Idle01"] != null)
				{
					m_SoliderMesh.GetComponent<Animation>()["Idle01"].wrapMode = WrapMode.Loop;
					m_SoliderMesh.GetComponent<Animation>()["Idle01"].layer = -1;
					m_SoliderMesh.GetComponent<Animation>().CrossFade("Idle01");
				}
				PlayAudio("WeaponZoomOut");
			}
		}
	}

	public void SetGameState(State state)
	{
		switch (state)
		{
		}
		m_State = state;
	}

	public bool Fire(Vector2 v2ScreenPos, bool bMakeSound = true)
	{
		if (m_CurrWeapon == null)
		{
			return false;
		}
		iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(m_CurrWeapon.m_nWeaponID);
		if (weaponInfoBase == null)
		{
			return false;
		}
		if (!m_CurrWeapon.IsCanFire() || m_fWaitReload > 0f)
		{
			return false;
		}
		if (IsSwitching())
		{
			FinishSwitchWeapon();
		}
		if (weaponInfoBase.m_sFireSound != "AnimSound")
		{
			PlayAudio(weaponInfoBase.m_sFireSound);
		}
		if (weaponInfoBase.m_nFireEffect != 0)
		{
			AddGunFlightEffect((FIRE_EFFECT_TYPE)weaponInfoBase.m_nFireEffect, m_ShootPos, m_SoliderMesh.transform.forward);
		}
		if (weaponInfoBase.IsRifle())
		{
			CrossFat(5f);
		}
		else if (weaponInfoBase.IsAutoShoot())
		{
			CrossFat(2f);
		}
		else if (weaponInfoBase.IsRocket() || weaponInfoBase.IsThrowMine())
		{
			CrossFat(6f);
		}
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(v2ScreenPos.x, v2ScreenPos.y, 0f));
		if (weaponInfoBase.IsRifle() || weaponInfoBase.IsAutoShoot() || weaponInfoBase.IsMachineGun())
		{
			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo, 1000f, -1342177280))
			{
				return false;
			}
			float num = Vector3.Distance(hitInfo.point, m_CameraScript.GetPosition());
			if (num <= m_GameState.GetWeaponSGbyStage(m_CurrWeapon.m_fSG))
			{
				FireHit(ray, hitInfo, bMakeSound);
			}
			else
			{
				PlayAudio("Voice03");
				m_GameSceneUI.AddImageWarn2(ImageMsgEnum2.OUT_OF_RANGE);
			}
			if (!m_bGameOver)
			{
				AddBulletEffect(m_ShootPos.transform.position, hitInfo.point - m_ShootPos.transform.position);
			}
			m_GameState.m_nFireNum++;
			if (!weaponInfoBase.IsAutoShoot())
			{
				m_CameraScript.Shake(0.5f, 0.2f);
			}
			else
			{
				m_CameraScript.Shake(0.2f, 0.1f);
			}
			m_GameSceneUI.PlayBulletAnim(m_CurrWeapon.m_fSS);
			m_GameSceneUI.PlayBulletJump();
		}
		else if (weaponInfoBase.IsRocket())
		{
			GameObject gameObject = null;
			gameObject = ((!weaponInfoBase.IsRocketBlue()) ? ((GameObject)UnityEngine.Object.Instantiate(m_PerfabManager.Rocket01, m_ShootPos.transform.position, Quaternion.identity)) : ((GameObject)UnityEngine.Object.Instantiate(m_PerfabManager.RocketBlue, m_ShootPos.transform.position, Quaternion.identity)));
			if (gameObject == null)
			{
				return false;
			}
			iZombieSniperRocket component = gameObject.GetComponent<iZombieSniperRocket>();
			if (component == null)
			{
				return false;
			}
			component.Initialize(ray.direction, 50f, m_CurrWeapon.m_fSW, m_CurrWeapon.m_fSD, true, weaponInfoBase.IsRocketBlue());
			m_CameraScript.Shake(1.5f, 0.3f);
			m_GameSceneUI.PlayBulletAnim(m_CurrWeapon.m_fSS);
		}
		else if (weaponInfoBase.IsThrowMine())
		{
			AddThrowMine(1);
			m_CameraScript.Shake(1.5f, 0.3f);
			m_GameSceneUI.PlayBulletAnim(m_CurrWeapon.m_fSS);
		}
		if (m_SoliderMesh.GetComponent<Animation>()["Fire01"] != null)
		{
			m_SoliderMesh.GetComponent<Animation>()["Fire01"].time = 0f;
			m_SoliderMesh.GetComponent<Animation>().Play("Fire01");
		}
		m_CurrWeapon.m_nCurrBulletNum--;
		if (m_CurrWeapon.m_nCurrBulletNum <= 0)
		{
			m_CurrWeapon.m_nCurrBulletNum = 0;
			if (weaponInfoBase.IsMachineGun())
			{
				SwitchWeapon();
				m_GameSceneUI.ShowWeaponUI(true);
			}
			else
			{
				m_fWaitReload = m_SoliderMesh.GetComponent<Animation>()["Fire01"].length;
				if (m_CurrWeapon.m_nWeaponID == 3)
				{
					m_fWaitReload = m_SoliderMesh.GetComponent<Animation>()["Fire01"].length * 0.29411766f;
				}
				else if (m_CurrWeapon.m_nWeaponID == 8)
				{
					m_fWaitReload = m_SoliderMesh.GetComponent<Animation>()["Fire01"].length * (7f / 32f);
				}
				else if (m_CurrWeapon.m_nWeaponID == 6)
				{
					m_fWaitReload = m_SoliderMesh.GetComponent<Animation>()["Fire01"].length * 0.31506848f;
				}
				else if (m_CurrWeapon.m_nWeaponID == 23 && m_CartridgeClip != null)
				{
					m_CartridgeClip.gameObject.active = false;
				}
			}
		}
		else
		{
			m_CurrWeapon.SetInterval();
		}
		m_GameSceneUI.UpdateBulletUI(m_CurrWeapon.m_nCurrBulletNum);
		return true;
	}

	public virtual void FireHit(Ray ray, RaycastHit hit, bool bMakeSound)
	{
		if (hit.transform.root.tag == "NPC")
		{
			ModelInfo component = hit.transform.root.GetComponent<ModelInfo>();
			if (!(component != null))
			{
				return;
			}
			iZombieSniperNpc nPC = GetNPC(component.m_nUID);
			if (nPC != null && !nPC.IsDead() && !nPC.IsSafe() && (!nPC.IsInnocents() || !m_GameState.IsInWeaponTool()))
			{
				m_GameState.m_nHitNum++;
				nPC.OnHit(ray, hit, m_CurrWeapon.m_fSD);
				if (bMakeSound)
				{
				}
				PlayAudio("FxBodyHit02");
			}
		}
		else if (hit.transform.root.tag == "OIL")
		{
			ModelInfo component2 = hit.transform.root.GetComponent<ModelInfo>();
			if (component2 != null)
			{
				iZombieSniperOilCan oilCan = GetOilCan(component2.m_nUID);
				if (oilCan != null)
				{
					oilCan.m_bBoom = true;
					AddGunHitEffect(hit.point);
					PlayAudio("FxMetalHit01");
				}
			}
		}
		else if (hit.transform.name.IndexOf("car") != -1 || hit.transform.root.name == "Bunker" || hit.transform.root.name == "Zbs-well" || hit.transform.root.name == "Zbs-container")
		{
			AddGunHitEffect(hit.point);
			PlayAudio("FxMetalHit01");
		}
		else
		{
			AddGunHitEffect(hit.point);
			if (!bMakeSound)
			{
			}
		}
	}

	public void Boom(Vector3 v3DamSource, float fRange, float fDamage, bool bIgnore = false)
	{
		int num = 0;
		foreach (iZombieSniperNpc value in m_NPCMap.Values)
		{
			if ((!value.IsInnocents() || !bIgnore) && !value.IsDead() && Vector3.Distance(v3DamSource, value.m_ModelTransForm.position) <= fRange)
			{
				value.OnHit(v3DamSource, fDamage, Vector3.zero);
				if (value.IsDead())
				{
					num++;
				}
			}
		}
		if (num >= 5)
		{
			PlayAudio("Voice02");
			m_GameSceneUI.AddImageWarn1(ImageMsgEnum1.KILL_MANY2_ZOMBIE);
		}
		else if (num >= 3)
		{
			PlayAudio("VoChar01");
			m_GameSceneUI.AddImageWarn1(ImageMsgEnum1.KILL_MANY_ZOMBIE);
		}
		if (bIgnore)
		{
			return;
		}
		foreach (iZombieSniperOilCan value2 in m_OilCanMap.Values)
		{
			if (!value2.m_bBoom && Vector3.Distance(v3DamSource, value2.m_ModelTransForm.position) <= fRange)
			{
				value2.m_bBoom = true;
			}
		}
	}

	public virtual Vector3 GetAC130Start()
	{
		return Vector3.zero;
	}

	public virtual Vector2 GetAC130Range()
	{
		return Vector2.zero;
	}

	public virtual Vector2 GetAC130Space()
	{
		return Vector2.zero;
	}

	public virtual void AC130()
	{
		if (m_bUsedACE || m_GameState.m_nAC130 < 1)
		{
			return;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(m_PerfabManager.AC130, Vector3.zero, Quaternion.identity);
		if (!(gameObject == null))
		{
			iZombieSniperAC130 component = gameObject.GetComponent<iZombieSniperAC130>();
			if (!(component == null))
			{
				component.Initialize(GetAC130Start(), 0.5f, GetAC130Range().x, GetAC130Range().y, GetAC130Space().x, GetAC130Space().y);
				gameObject.tag = "Rocket";
				m_bUsedACE = true;
				m_GameState.m_nAC130--;
				PlayAudio("Voice02");
				PlayAudio("FxPreExplo01");
			}
		}
	}

	public virtual void TruckBoom()
	{
	}

	public void AddBloodEffect(Vector3 point)
	{
		if (m_BloodPool != null)
		{
			GameObject free = m_BloodPool.GetFree(0.4f);
			if (!(free == null))
			{
				free.transform.position = point;
			}
		}
	}

	public void AddMachineGunDead(Vector3 point)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(m_PerfabManager.MachineGunDead);
		gameObject.transform.position = new Vector3(point.x, gameObject.transform.position.y, point.z);
		UnityEngine.Object.Destroy(gameObject, 3f);
	}

	public void AddGunHitEffect(Vector3 point)
	{
		if (m_GunHitPool != null)
		{
			GameObject free = m_GunHitPool.GetFree(0.3f);
			if (!(free == null))
			{
				free.transform.position = point;
			}
		}
	}

	public void AddGunFlightEffect(FIRE_EFFECT_TYPE effecttype, GameObject mouse, Vector3 dir)
	{
		GameObject gameObject = null;
		switch (effecttype)
		{
		case FIRE_EFFECT_TYPE.GUN:
			if (m_GunFlightPool != null)
			{
				gameObject = m_GunFlightPool.GetFree(0.1f);
				if (gameObject == null)
				{
					return;
				}
				gameObject.transform.parent = mouse.transform;
				gameObject.transform.localPosition = Vector3.zero;
			}
			break;
		case FIRE_EFFECT_TYPE.ROCKET:
			gameObject = (GameObject)UnityEngine.Object.Instantiate(m_PerfabManager.RocketFire, Vector3.zero, Quaternion.identity);
			gameObject.transform.parent = mouse.transform;
			gameObject.transform.localPosition = Vector3.zero;
			UnityEngine.Object.Destroy(gameObject, 0.5f);
			break;
		case FIRE_EFFECT_TYPE.ROCKET_BLUE:
			gameObject = (GameObject)UnityEngine.Object.Instantiate(m_PerfabManager.RocketFireBlue, Vector3.zero, Quaternion.identity);
			gameObject.transform.parent = mouse.transform;
			gameObject.transform.localPosition = Vector3.zero;
			UnityEngine.Object.Destroy(gameObject, 0.5f);
			break;
		case FIRE_EFFECT_TYPE.THROWMINE:
			gameObject = (GameObject)UnityEngine.Object.Instantiate(m_PerfabManager.ThrowFlightEffect, Vector3.zero, Quaternion.identity);
			gameObject.transform.parent = mouse.transform;
			gameObject.transform.localPosition = Vector3.zero;
			UnityEngine.Object.Destroy(gameObject, 1.5f);
			break;
		case FIRE_EFFECT_TYPE.MACHINE:
			gameObject = (GameObject)UnityEngine.Object.Instantiate(m_PerfabManager.MachineFire, Vector3.zero, Quaternion.identity);
			gameObject.transform.parent = mouse.transform;
			gameObject.transform.localPosition = Vector3.zero;
			UnityEngine.Object.Destroy(gameObject, 1.5f);
			break;
		}
		gameObject.transform.forward = dir;
	}

	public void AddBulletEffect(Vector3 scr, Vector3 dir)
	{
		if (m_BulletPool == null)
		{
			return;
		}
		GameObject free = m_BulletPool.GetFree(0.5f);
		if (free != null)
		{
			iZombieSniperBullet component = free.GetComponent<iZombieSniperBullet>();
			if (component != null)
			{
				component.Initialize(scr, dir, 0.5f);
			}
		}
	}

	public void AddCinderEffect(Vector3 scr)
	{
		if (m_fCinderHeight > 0.2f)
		{
			m_fCinderHeight = 0.1f;
		}
		else
		{
			m_fCinderHeight += 0.01f;
		}
		scr.y = m_fCinderHeight;
		UnityEngine.Object.Instantiate(m_PerfabManager.m_CinderEffect, scr, Quaternion.identity);
	}

	public void SetAutoShoot(bool bAuto)
	{
		if (bAuto)
		{
			if (m_CurrWeapon == null)
			{
				return;
			}
			iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(m_CurrWeapon.m_nWeaponID);
			if (weaponInfoBase == null)
			{
				return;
			}
			m_nAutoShootCount = 0;
			m_bFirstShoot = true;
		}
		m_bAutoShoot = bAuto;
		if (m_SoliderMesh != null)
		{
			m_CartridgeAnim = m_SoliderMesh.GetComponentInChildren<UVAnim>();
			if (m_CartridgeAnim != null)
			{
				if (bAuto)
				{
					m_CartridgeAnim.Play();
				}
				else
				{
					m_CartridgeAnim.Stop();
				}
			}
		}
		else
		{
			m_CartridgeAnim = null;
		}
	}

	public void UpdateAutoShoot(float deltaTime)
	{
		if (m_CurrWeapon == null)
		{
			return;
		}
		iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(m_CurrWeapon.m_nWeaponID);
		if (weaponInfoBase == null || !m_bAutoShoot || m_CurrWeapon.m_WeaponState == iWeapon.WeaponState.kInterval)
		{
			return;
		}
		if (m_CurrWeapon.m_WeaponState == iWeapon.WeaponState.kReload)
		{
			m_GameSceneUI.AddImageWarn2(ImageMsgEnum2.RELOADING);
			return;
		}
		if (IsSwitching())
		{
			iWeaponInfoBase weaponInfoBase2 = m_GunCenter.GetWeaponInfoBase(m_nOldWeapon);
			if (weaponInfoBase2 != null && weaponInfoBase2.IsMachineGun())
			{
				if (m_CartridgeAnim != null)
				{
					m_CartridgeAnim.Stop();
				}
				return;
			}
			iWeaponInfoBase weaponInfoBase3 = m_GunCenter.GetWeaponInfoBase(m_nNewWeapon);
			if (weaponInfoBase3 != null && weaponInfoBase3.IsMachineGun())
			{
				if (m_CartridgeAnim != null)
				{
					m_CartridgeAnim.Stop();
				}
				return;
			}
		}
		Vector2 v2ScreenPos = CaculateShootPos(m_GameState.GetShootCenter(), m_nAutoShootCount);
		Fire(v2ScreenPos, m_bFirstShoot);
		m_nAutoShootCount++;
		if (m_bFirstShoot)
		{
			m_bFirstShoot = false;
		}
	}

	public Vector2 CaculateShootPos(Vector2 v2ScreenPos, int nShootCount)
	{
		if (nShootCount < 5)
		{
			return v2ScreenPos;
		}
		float num = nShootCount;
		if (num > 25f)
		{
			num = 25f;
		}
		return v2ScreenPos + new Vector2(UnityEngine.Random.Range(0f - num, num), UnityEngine.Random.Range(0f, num));
	}

	public IEnumerator DoTheAim()
	{
		while (m_CurrWeapon.m_WeaponState == iWeapon.WeaponState.kReload)
		{
			yield return new WaitForEndOfFrame();
		}
		iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(m_CurrWeapon.m_nWeaponID);
		if (!IsAim())
		{
			if (m_CameraScript.Aim(m_GameState.GetShootCenter()))
			{
				PlayAudio("WeaponZoomIn");
				Aim(m_GameState.GetShootCenter());
				m_SoliderMesh.SetActiveRecursively(false);
				m_bAim = true;
				m_GameSceneUI.ShowCross(false);
				m_GameSceneUI.ShowAimUI((WEAPON_TYPE)weaponInfoBase.m_nWeaponType);
			}
			yield break;
		}
		CloseAim(true);
	}

	public void UpdateCurrWeapon(float deltaTime)
	{
		if (m_CurrWeapon == null)
		{
			return;
		}
		iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(m_CurrWeapon.m_nWeaponID);
		if (weaponInfoBase == null)
		{
			return;
		}
		if (m_fWaitReload > 0f)
		{
			m_fWaitReload -= deltaTime;
			if (m_fWaitReload <= 0f)
			{
				m_fWaitReload = 0f;
				m_CurrWeapon.SetReload();
				m_GameSceneUI.StartWeaponMask(m_CurrWeapon.m_fSR);
				if (!IsHasReloadAnimEvent(m_CurrWeapon.m_nWeaponID) && m_CartridgeClip != null)
				{
					m_CartridgeClip.gameObject.active = false;
				}
				if (IsAim())
				{
					m_bAimAfterReload = true;
				}
				CloseAim(true);
				if (!weaponInfoBase.IsRocket() && !weaponInfoBase.IsThrowMine())
				{
					if (m_SoliderMesh.GetComponent<Animation>()["Reload01"] != null)
					{
						m_SoliderMesh.GetComponent<Animation>()["Reload01"].speed = m_SoliderMesh.GetComponent<Animation>()["Reload01"].length / m_CurrWeapon.m_fSR;
						m_SoliderMesh.GetComponent<Animation>()["Reload01"].time = 0f;
						m_SoliderMesh.GetComponent<Animation>().CrossFade("Reload01");
					}
					m_CurrWeapon.m_nRocketReloadState = 0;
				}
				else
				{
					m_CurrWeapon.m_fRocketReloadCount = 0f;
					m_CurrWeapon.m_fRocketReloadTimeOut = 0f;
					m_CurrWeapon.m_fRocketReloadTimeWait = 0f;
					if (m_SoliderMesh.GetComponent<Animation>()["PickGun01"] != null)
					{
						m_SoliderMesh.GetComponent<Animation>()["PickGun01"].speed = 1f;
						m_SoliderMesh.GetComponent<Animation>()["PickGun01"].time = 0f;
						m_SoliderMesh.GetComponent<Animation>().CrossFade("PickGun01");
						if (m_CurrWeapon.m_nWeaponID == 23)
						{
							m_CurrWeapon.m_fRocketReloadTimeOut = m_SoliderMesh.GetComponent<Animation>()["PickGun01"].length * 0.125f;
							m_CurrWeapon.m_fRocketReloadTimeWait = m_CurrWeapon.m_fSR - m_SoliderMesh.GetComponent<Animation>()["PickGun01"].length;
						}
						else if (m_CurrWeapon.m_nWeaponID == 24)
						{
							m_CurrWeapon.m_fRocketReloadTimeOut = m_SoliderMesh.GetComponent<Animation>()["PickGun01"].length * 0.11764706f;
							m_CurrWeapon.m_fRocketReloadTimeWait = m_CurrWeapon.m_fSR - m_SoliderMesh.GetComponent<Animation>()["PickGun01"].length;
						}
						else if (m_CurrWeapon.m_nWeaponID == 26)
						{
							m_CurrWeapon.m_fRocketReloadTimeOut = m_SoliderMesh.GetComponent<Animation>()["PickGun01"].length * 0.13953489f;
							m_CurrWeapon.m_fRocketReloadTimeWait = m_CurrWeapon.m_fSR - m_SoliderMesh.GetComponent<Animation>()["PickGun01"].length;
						}
						else if (m_CurrWeapon.m_nWeaponID == 41)
						{
							m_CurrWeapon.m_fRocketReloadTimeOut = m_SoliderMesh.GetComponent<Animation>()["PickGun01"].length * 0.11764706f;
							m_CurrWeapon.m_fRocketReloadTimeWait = m_CurrWeapon.m_fSR - m_SoliderMesh.GetComponent<Animation>()["PickGun01"].length;
						}
						else
						{
							m_CurrWeapon.m_fRocketReloadTimeOut = m_SoliderMesh.GetComponent<Animation>()["PickGun01"].length / 2f;
							m_CurrWeapon.m_fRocketReloadTimeWait = m_CurrWeapon.m_fSR - m_CurrWeapon.m_fRocketReloadTimeOut * 2f;
						}
					}
					m_CurrWeapon.m_nRocketReloadState = 1;
				}
			}
		}
		if (m_bAimAction && m_fAimTimeCount > 0f)
		{
			m_fAimTimeCount -= deltaTime;
			if (m_fAimTimeCount <= 0f)
			{
				m_fAimTimeCount = 0f;
				m_bAimAction = false;
				if (m_CameraScript.Aim(m_GameState.GetShootCenter()))
				{
					m_bAim = true;
					m_GameSceneUI.ShowCross(false);
					m_GameSceneUI.ShowAimUI((WEAPON_TYPE)weaponInfoBase.m_nWeaponType);
					m_SoliderMesh.SetActiveRecursively(false);
					PlayAudio("WeaponZoomIn");
				}
			}
		}
		switch (m_CurrWeapon.m_WeaponState)
		{
		case iWeapon.WeaponState.kInterval:
			m_CurrWeapon.m_fTimeCount += deltaTime;
			if (m_CurrWeapon.m_fTimeCount >= m_CurrWeapon.m_fSS)
			{
				m_CurrWeapon.m_fTimeCount = m_CurrWeapon.m_fSS;
				m_CurrWeapon.m_WeaponState = iWeapon.WeaponState.kNormal;
			}
			break;
		case iWeapon.WeaponState.kReload:
			if (m_CurrWeapon.m_nRocketReloadState > 0)
			{
				m_CurrWeapon.m_fRocketReloadCount += deltaTime;
				switch (m_CurrWeapon.m_nRocketReloadState)
				{
				case 1:
					if (m_CurrWeapon.m_fRocketReloadCount > m_CurrWeapon.m_fRocketReloadTimeOut)
					{
						m_CurrWeapon.m_fRocketReloadCount = 0f;
						m_CurrWeapon.m_nRocketReloadState = 2;
						if (m_SoliderMesh.GetComponent<Animation>()["PickGun01"] != null)
						{
							m_SoliderMesh.GetComponent<Animation>()["PickGun01"].speed = 0f;
							m_SoliderMesh.GetComponent<Animation>().CrossFade("PickGun01");
						}
					}
					break;
				case 2:
					if (m_CurrWeapon.m_fRocketReloadCount > m_CurrWeapon.m_fRocketReloadTimeWait)
					{
						m_CurrWeapon.m_fRocketReloadCount = 0f;
						m_CurrWeapon.m_nRocketReloadState = 0;
						if (m_SoliderMesh.GetComponent<Animation>()["PickGun01"] != null)
						{
							m_SoliderMesh.GetComponent<Animation>()["PickGun01"].speed = 1f;
							m_SoliderMesh.GetComponent<Animation>().CrossFade("PickGun01");
						}
					}
					break;
				}
			}
			if (m_CurrWeapon.m_fTimeCount > m_CurrWeapon.m_fSR * 0.5f)
			{
				if (!IsHasReloadAnimEvent(m_CurrWeapon.m_nWeaponID) && m_CartridgeClip != null && !m_CartridgeClip.gameObject.active)
				{
					m_CartridgeClip.gameObject.active = true;
				}
				if (!m_CurrWeapon.m_bReloadSound)
				{
					m_CurrWeapon.m_bReloadSound = true;
				}
			}
			m_CurrWeapon.m_fTimeCount += deltaTime;
			if (m_CurrWeapon.m_fTimeCount >= m_CurrWeapon.m_fSR)
			{
				m_CurrWeapon.m_fTimeCount = m_CurrWeapon.m_fSR;
				m_CurrWeapon.m_WeaponState = iWeapon.WeaponState.kNormal;
				m_CurrWeapon.m_nCurrBulletNum = weaponInfoBase.m_nBulletMax;
				m_GameSceneUI.UpdateBulletUI(m_CurrWeapon.m_nCurrBulletNum);
				if (m_bAimAfterReload)
				{
					m_bAimAfterReload = false;
					Aim(m_GameState.m_v3ShootCenter);
				}
			}
			break;
		}
		for (int i = 0; i < 3; i++)
		{
			iWeapon userWeapon = m_GameState.GetUserWeapon(i);
			if (userWeapon == null || userWeapon.m_nWeaponID == m_CurrWeapon.m_nWeaponID)
			{
				continue;
			}
			iWeaponInfoBase weaponInfoBase2 = m_GunCenter.GetWeaponInfoBase(userWeapon.m_nWeaponID);
			if (weaponInfoBase2 == null)
			{
				continue;
			}
			switch (userWeapon.m_WeaponState)
			{
			case iWeapon.WeaponState.kInterval:
				userWeapon.m_fTimeCount += deltaTime;
				if (userWeapon.m_fTimeCount >= m_CurrWeapon.m_fSS)
				{
					userWeapon.m_fTimeCount = m_CurrWeapon.m_fSS;
					userWeapon.m_WeaponState = iWeapon.WeaponState.kNormal;
				}
				break;
			case iWeapon.WeaponState.kReload:
				userWeapon.m_fTimeCount += deltaTime;
				if (userWeapon.m_fTimeCount >= userWeapon.m_fSR)
				{
					userWeapon.m_fTimeCount = m_CurrWeapon.m_fSR;
					userWeapon.m_WeaponState = iWeapon.WeaponState.kNormal;
					userWeapon.m_nCurrBulletNum = weaponInfoBase2.m_nBulletMax;
				}
				break;
			}
		}
	}

	public void UpdateOilCanRefresh(float deltaTime)
	{
		m_fOilCanRefreshCount += deltaTime;
		if (!(m_fOilCanRefreshCount < 5f))
		{
			m_fOilCanRefreshCount = 0f;
			AddOilCan(50f, 10f);
		}
	}

	public int GetUID()
	{
		m_nUID++;
		if (m_nUID > 10000)
		{
			m_nUID = 1;
		}
		return m_nUID;
	}

	public bool IsMaxNPC()
	{
		return m_NPCMap.Count >= 80;
	}

	public int AddNPC(int nID, Vector3 v3Pos, int nStartZone)
	{
		ZombieBaseInfo zombieBaseInfo = m_ZombieWaveCenter.GetZombieBaseInfo(nID);
		if (zombieBaseInfo == null)
		{
			return -1;
		}
		iZombieSniperNpc iZombieSniperNpc2 = null;
		switch (zombieBaseInfo.m_nAiType)
		{
		case 1:
			iZombieSniperNpc2 = new iZombieSniperZombieFool();
			if (!iZombieSniperNpc2.Create(nID, GetUID(), "ZombieFool", v3Pos))
			{
				return -1;
			}
			if (nStartZone >= 10)
			{
				iZombieSniperNpc2.SetStateDirectly(((iZombieSniperZombieFool)iZombieSniperNpc2).stFreeState);
			}
			break;
		case 2:
			iZombieSniperNpc2 = new iZombieSniperZombieSmart();
			if (!iZombieSniperNpc2.Create(nID, GetUID(), "ZombieSmart", v3Pos))
			{
				return -1;
			}
			break;
		case 3:
			iZombieSniperNpc2 = new iZombieSniperZombieDog();
			if (!iZombieSniperNpc2.Create(nID, GetUID(), "ZombieDog", v3Pos))
			{
				return -1;
			}
			break;
		case 5:
			iZombieSniperNpc2 = new iZombieSniperZombiePredator();
			if (!iZombieSniperNpc2.Create(nID, GetUID(), "ZombiePredator", v3Pos))
			{
				return -1;
			}
			break;
		case 4:
			iZombieSniperNpc2 = new iZombieSniperInnocence();
			if (!iZombieSniperNpc2.Create(nID, GetUID(), "innocence", v3Pos))
			{
				return -1;
			}
			break;
		}
		if (iZombieSniperNpc2 == null)
		{
			return -1;
		}
		if (IsNeedClimbout(nStartZone))
		{
			iZombieSniperNpc2.SetState(iZombieSniperNpc2.stClimbState);
		}
		m_NPCMap.Add(iZombieSniperNpc2.m_nUID, iZombieSniperNpc2);
		return iZombieSniperNpc2.m_nUID;
	}

	public iZombieSniperNpc GetNPC(int nUID)
	{
		if (m_NPCMap == null)
		{
			return null;
		}
		if (m_NPCMap.ContainsKey(nUID))
		{
			return m_NPCMap[nUID];
		}
		return null;
	}

	public void RemoveNPC(iZombieSniperNpc obj)
	{
		if (m_NPCMap != null)
		{
			m_NPCMap.Remove(obj.m_nUID);
		}
	}

	public void ClearNPC()
	{
		if (m_NPCMap == null)
		{
			return;
		}
		foreach (iZombieSniperNpc value in m_NPCMap.Values)
		{
			value.Destroy();
		}
		m_NPCMap.Clear();
	}

	public bool SwitchWeapon()
	{
		if (m_fWaitReload > 0f)
		{
			return false;
		}
		if (IsSwitching())
		{
			FinishSwitchWeapon();
		}
		int nOldWeaponID = -1;
		if (m_CurrWeapon != null)
		{
			nOldWeaponID = m_CurrWeapon.m_nWeaponID;
		}
		if (!m_GameState.SwitchWeapon())
		{
			return false;
		}
		m_bAimAfterReload = false;
		m_CurrWeapon = m_GameState.GetUserWeapon();
		iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(m_CurrWeapon.m_nWeaponID);
		if (weaponInfoBase == null)
		{
			return false;
		}
		if (m_bAim)
		{
			m_CameraScript.AdjustFov();
		}
		m_GameSceneUI.UpdateBulletUI(m_CurrWeapon.m_nCurrBulletNum);
		m_GameSceneUI.UpdateWeaponButton();
		m_GameSceneUI.UpdateWeaponPic(m_CurrWeapon.m_nWeaponID);
		m_GameSceneUI.SwitchAimUI((WEAPON_TYPE)weaponInfoBase.m_nWeaponType);
		m_GameSceneUI.SwitchBulletUI((WEAPON_TYPE)weaponInfoBase.m_nWeaponType);
		if (m_CurrWeapon.m_WeaponState == iWeapon.WeaponState.kInterval)
		{
			m_GameSceneUI.PlayBulletAnim(m_CurrWeapon.m_fSS, m_CurrWeapon.m_fTimeCount);
		}
		else
		{
			m_GameSceneUI.StopBulletAnim();
		}
		StartSwitchWeapon(m_CurrWeapon.m_nWeaponID, nOldWeaponID);
		if (m_CurrWeapon.m_WeaponState == iWeapon.WeaponState.kReload)
		{
			m_GameSceneUI.StartWeaponMask(m_CurrWeapon.m_fSR - m_CurrWeapon.m_fTimeCount);
		}
		else
		{
			m_GameSceneUI.StopWeaponMask();
		}
		PlayAudio("WeaponPickUp");
		return true;
	}

	public bool SwitchWeapon(int nIndex)
	{
		if (!m_GameState.SwitchWeapon(nIndex))
		{
			return false;
		}
		m_bAimAfterReload = false;
		m_CurrWeapon = m_GameState.GetUserWeapon();
		iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(m_CurrWeapon.m_nWeaponID);
		if (weaponInfoBase == null)
		{
			return false;
		}
		if (m_bAim)
		{
			m_CameraScript.AdjustFov();
		}
		m_GameSceneUI.UpdateBulletUI(m_CurrWeapon.m_nCurrBulletNum);
		m_GameSceneUI.UpdateWeaponButton();
		m_GameSceneUI.UpdateWeaponPic(m_CurrWeapon.m_nWeaponID);
		m_GameSceneUI.SwitchAimUI((WEAPON_TYPE)weaponInfoBase.m_nWeaponType);
		m_GameSceneUI.SwitchBulletUI((WEAPON_TYPE)weaponInfoBase.m_nWeaponType);
		if (m_CurrWeapon.m_WeaponState == iWeapon.WeaponState.kInterval)
		{
			m_GameSceneUI.StopBulletAnim();
			m_GameSceneUI.PlayBulletAnim(m_CurrWeapon.m_fSS, m_CurrWeapon.m_fTimeCount);
		}
		UpdateWeaponMeshInfo(m_CurrWeapon.m_nWeaponID);
		if (m_CurrWeapon.m_WeaponState == iWeapon.WeaponState.kReload)
		{
			m_GameSceneUI.StartWeaponMask(m_CurrWeapon.m_fSR - m_CurrWeapon.m_fTimeCount);
		}
		else
		{
			m_GameSceneUI.StopWeaponMask();
		}
		return true;
	}

	public void SoundStrike(Vector3 v3SoundPos, float fEffectDis, int igoreuid = 0)
	{
		v3SoundPos.y = 0f;
		bool bBlock = m_GridInfoCenter.IsBlock(v3SoundPos.x, v3SoundPos.z);
		foreach (iZombieSniperNpc value in m_NPCMap.Values)
		{
			if (!value.IsDead() && !value.IsInnocents() && !value.IsZombieZombieDog() && (igoreuid <= 0 || value.m_nUID != igoreuid) && value.IsWillCatchSound())
			{
				Vector3 vector = value.m_ModelTransForm.position - v3SoundPos;
				vector.y = 0f;
				if (vector.sqrMagnitude < fEffectDis * fEffectDis)
				{
					value.stHearState.Initialize(v3SoundPos, bBlock, UnityEngine.Random.Range(1.5f, 2.5f));
					value.SetState(value.stHearState);
				}
			}
		}
	}

	public void UpdateNPCCollide(float deltaTime)
	{
		m_fCollideCount -= deltaTime;
		if (m_fCollideCount > 0f)
		{
			return;
		}
		m_fCollideCount = (float)m_NPCMap.Count * 0.1f;
		foreach (iZombieSniperNpc value in m_NPCMap.Values)
		{
			value.m_bCollideFlag = false;
		}
		foreach (iZombieSniperNpc value2 in m_NPCMap.Values)
		{
			foreach (iZombieSniperNpc value3 in m_NPCMap.Values)
			{
				if (value2 != value3 && !value2.m_bCollideFlag && !value3.m_bCollideFlag && Utils.IsCollide(value2.m_ModelTransForm.position, value2.m_ZombieBaseInfo.m_fSize, value3.m_ModelTransForm.position, value3.m_ZombieBaseInfo.m_fSize))
				{
					value2.OnCollide(value3);
					value3.OnCollide(value2);
					value2.m_bCollideFlag = true;
					value3.m_bCollideFlag = true;
				}
			}
		}
	}

	public bool IsAim()
	{
		return m_bAim;
	}

	public void SetPause(bool bPause)
	{
		if (m_bPause != bPause)
		{
			m_bPause = bPause;
			if (bPause)
			{
				Time.timeScale = 0f;
			}
			else if (m_bSlowScale)
			{
				Time.timeScale = m_nSlowScale;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
	}

	public void SetGamePause(bool bPause)
	{
		if (!bPause || (m_State != State.kGameOver && m_State != State.kGameOvering && m_State != State.kGameMoonWalk && m_State != State.kGameMovie))
		{
			if (m_GameSceneUI != null)
			{
				m_GameSceneUI.ShowGamePause(bPause);
			}
			SetPause(bPause);
			SetPauseAudio(bPause);
			if (bPause)
			{
				OpenClikPlugin.Show(false);
				Resources.UnloadUnusedAssets();
				GC.Collect();
			}
			else
			{
				OpenClikPlugin.Hide();
			}
		}
	}

	public void SetPauseScale(bool bSlowScale, float fScale = 1f)
	{
		if (m_bSlowScale != bSlowScale)
		{
			m_bSlowScale = bSlowScale;
			if (bSlowScale)
			{
				Time.timeScale = fScale;
				m_nSlowScale = fScale;
			}
			else
			{
				Time.timeScale = 1f;
				m_nSlowScale = 1f;
			}
		}
	}

	public bool GetPause()
	{
		return m_bPause;
	}

	public void AddOilCan(float fDamage, float fRange)
	{
		if (m_OilCanEmptyList.Count > 0)
		{
			int num = (int)m_OilCanEmptyList[UnityEngine.Random.Range(0, m_OilCanEmptyList.Count)];
			m_OilCanEmptyList.Remove(num);
			OilCanPoint oilCanPoint = m_WayPointCenter.GetOilCanPoint(num);
			if (oilCanPoint != null)
			{
				iZombieSniperOilCan iZombieSniperOilCan2 = new iZombieSniperOilCan();
				iZombieSniperOilCan2.Initialize(GetUID(), oilCanPoint.m_nID, oilCanPoint.m_v3Position, fDamage, fRange);
				m_OilCanMap.Add(iZombieSniperOilCan2.m_nUID, iZombieSniperOilCan2);
			}
		}
	}

	public void RemoveOilCan(iZombieSniperOilCan obj)
	{
		m_OilCanMap.Remove(obj.m_nUID);
	}

	public iZombieSniperOilCan GetOilCan(int nUID)
	{
		if (!m_OilCanMap.ContainsKey(nUID))
		{
			return null;
		}
		return m_OilCanMap[nUID];
	}

	public void ClearOilCan()
	{
		if (m_OilCanMap == null)
		{
			return;
		}
		foreach (iZombieSniperOilCan value in m_OilCanMap.Values)
		{
			value.Destroy();
		}
		m_OilCanMap.Clear();
	}

	public virtual void UpdateGameEvent(float deltaTime)
	{
	}

	public void StartSlow(Vector3 v3Pos, Vector3 v3Euler, float fov)
	{
		m_CameraScript.ShakeEnd();
		m_CameraScript.SkipAimProcess();
		m_CameraCurrState = m_CameraScript.m_CameraState;
		m_CameraScript.m_CameraState = iZombieSniperCamera.CameraState.kMovie;
		m_bAimAction = false;
		m_fAimTimeCount = 0f;
		m_v3CameraCurrPos = m_CameraScript.GetPosition();
		m_v3CameraCurrEuler = m_CameraScript.GetComponent<Camera>().transform.eulerAngles;
		m_fCameraCurrFov = m_CameraScript.GetComponent<Camera>().fov;
		m_bCameraCurrAim = m_bAim;
		m_isShowDefenceUI = m_GameSceneUI.IsDefenceUIShow();
		if (IsSwitching())
		{
			FinishSwitchWeapon();
		}
		SetAutoShoot(false);
		m_GameSceneUI.ShowCross(false);
		m_SoliderMesh.SetActiveRecursively(false);
		m_GameSceneUI.ShowGameUI(false);
		m_GameSceneUI.ShowWarn(false);
		if (m_bCameraCurrAim)
		{
			m_GameSceneUI.HideAimUI();
		}
		if (m_isShowDefenceUI)
		{
			m_GameSceneUI.ShowDefenceUI(false);
		}
		m_CameraScript.SetPosition(v3Pos);
		m_CameraScript.GetComponent<Camera>().transform.eulerAngles = v3Euler;
		m_CameraScript.GetComponent<Camera>().fov = fov;
		m_isSlow = true;
	}

	public void EndSlow()
	{
		m_CameraScript.ShakeEnd();
		m_CameraScript.m_CameraState = m_CameraCurrState;
		m_CameraScript.SetPosition(m_v3CameraCurrPos);
		m_CameraScript.GetComponent<Camera>().transform.eulerAngles = m_v3CameraCurrEuler;
		m_CameraScript.GetComponent<Camera>().fov = m_fCameraCurrFov;
		m_GameSceneUI.ShowCross(true);
		m_SoliderMesh.SetActiveRecursively(true);
		if (m_SoliderMesh.GetComponent<Animation>()["Idle01"] != null)
		{
			m_SoliderMesh.GetComponent<Animation>()["Idle01"].wrapMode = WrapMode.Loop;
			m_SoliderMesh.GetComponent<Animation>()["Idle01"].layer = -1;
			m_SoliderMesh.GetComponent<Animation>().CrossFade("Idle01");
		}
		m_GameSceneUI.ShowGameUI(true);
		m_GameSceneUI.ShowWarn(true);
		if (m_bCameraCurrAim)
		{
			m_GameSceneUI.ShowCross(false);
			m_SoliderMesh.SetActiveRecursively(false);
			iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(m_CurrWeapon.m_nWeaponID);
			if (weaponInfoBase != null)
			{
				m_GameSceneUI.ShowAimUI((WEAPON_TYPE)weaponInfoBase.m_nWeaponType);
			}
		}
		if (m_isShowDefenceUI)
		{
			m_GameSceneUI.ShowDefenceUI(true);
		}
		m_isSlow = false;
	}

	public float PlaySceneAnim(GameObject gameobject, string sAnim, bool bStayFirst = false, float speed = 1f)
	{
		if (gameobject == null)
		{
			return 0f;
		}
		if (gameobject.GetComponent<Animation>() != null && gameobject.GetComponent<Animation>()[sAnim] != null)
		{
			gameobject.GetComponent<Animation>()[sAnim].time = 0f;
			gameobject.GetComponent<Animation>()[sAnim].speed = ((!bStayFirst) ? speed : 0f);
			gameobject.GetComponent<Animation>()[sAnim].wrapMode = WrapMode.Once;
			gameobject.GetComponent<Animation>().Play(sAnim);
			return gameobject.GetComponent<Animation>()[sAnim].length;
		}
		return 0f;
	}

	protected void UpdateWeaponMeshInfo(int nWeaponID)
	{
		if (m_nCurrWeaponID == nWeaponID)
		{
			return;
		}
		GameObject weaponPerfab = m_PerfabManager.GetWeaponPerfab(nWeaponID);
		if (!(weaponPerfab == null))
		{
			if (m_SoliderMesh != null)
			{
				UnityEngine.Object.Destroy(m_SoliderMesh);
			}
			m_SoliderMesh = (GameObject)UnityEngine.Object.Instantiate(weaponPerfab);
			if (m_SoliderMesh.GetComponent<Animation>()["Idle01"] != null)
			{
				m_SoliderMesh.GetComponent<Animation>()["Idle01"].wrapMode = WrapMode.Loop;
				m_SoliderMesh.GetComponent<Animation>()["Idle01"].layer = -1;
				m_SoliderMesh.GetComponent<Animation>().CrossFade("Idle01");
			}
			if (m_SoliderMesh.GetComponent<Animation>()["PickGunUp01"] != null)
			{
				m_SoliderMesh.GetComponent<Animation>()["PickGunUp01"].speed = 1f;
				m_SoliderMesh.GetComponent<Animation>()["PickGunUp01"].time = 0f;
				m_SoliderMesh.GetComponent<Animation>().Play("PickGunUp01");
				m_SoliderMesh.GetComponent<Animation>().Sample();
				m_SoliderMesh.GetComponent<Animation>().CrossFade("PickGunUp01", 0.1f);
			}
			if (m_bAim && m_SoliderMesh.GetComponent<Animation>()["Idle01"] != null)
			{
				m_SoliderMesh.GetComponent<Animation>().clip = m_SoliderMesh.GetComponent<Animation>()["Idle01"].clip;
				m_SoliderMesh.GetComponent<Animation>().playAutomatically = true;
			}
			Vector3 position = m_SoliderMesh.transform.position;
			m_SoliderMesh.transform.parent = m_Solider.transform;
			m_SoliderMesh.transform.localPosition = position;
			m_SoliderMesh.transform.forward = m_Solider.transform.forward;
			Transform transform = m_SoliderMesh.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bone_Weapon/shootmouse");
			if (transform == null)
			{
				transform = m_SoliderMesh.transform.Find("Bone_Weapon/shootmouse");
			}
			if (transform != null)
			{
				m_ShootPos = transform.gameObject;
			}
			else
			{
				m_ShootPos = m_SoliderMesh;
			}
			Transform transform2 = m_SoliderMesh.transform.Find("Clip");
			if (transform2 == null)
			{
				transform2 = m_SoliderMesh.transform.Find("Clip01");
			}
			if (transform2 != null)
			{
				m_CartridgeClip = transform2.gameObject.GetComponent<SkinnedMeshRenderer>();
			}
			else
			{
				m_CartridgeClip = null;
			}
			if (m_bAim)
			{
				m_SoliderMesh.SetActiveRecursively(false);
				return;
			}
			m_SoliderMesh.SetActiveRecursively(true);
			CrossFat(5f, 5f);
		}
	}

	public void PlayAudio(string sName)
	{
		iZombieSniperGameApp.GetInstance().PlayAudio(sName);
	}

	public void StopAudio(string sName)
	{
		iZombieSniperGameApp.GetInstance().StopAudio(sName);
	}

	public void ClearGameObjectAudio(GameObject o)
	{
		if (o == null)
		{
			return;
		}
		TAudioController component = o.GetComponent<TAudioController>();
		if (!(component != null))
		{
			return;
		}
		Transform transform = component.transform.Find("Audio");
		if (transform == null)
		{
			return;
		}
		foreach (Transform item in transform)
		{
			component.StopAudio(item.name);
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	public void StartSwitchWeapon(int nWeaponID, int nOldWeaponID)
	{
		m_nNewWeapon = nWeaponID;
		m_nOldWeapon = nOldWeaponID;
		if (m_SoliderMesh == null || m_SoliderMesh.GetComponent<Animation>() == null || m_SoliderMesh.GetComponent<Animation>()["PickGunDown01"] == null)
		{
			FinishSwitchWeapon();
			return;
		}
		m_bWeaponSwitching = true;
		m_fSwitchWeaponCount = 0f;
		m_fSwitchWeaponTime = m_SoliderMesh.GetComponent<Animation>()["PickGunDown01"].length;
		m_SoliderMesh.GetComponent<Animation>().Play("PickGunDown01");
	}

	public void FinishSwitchWeapon()
	{
		if (m_nNewWeapon != -1)
		{
			UpdateWeaponMeshInfo(m_nNewWeapon);
			m_nNewWeapon = -1;
			m_nOldWeapon = -1;
			m_fSwitchWeaponCount = 0f;
			m_bWeaponSwitching = false;
		}
	}

	public void UpdateSwitchWeapon(float deltaTime)
	{
		if (m_bWeaponSwitching)
		{
			m_fSwitchWeaponCount += deltaTime;
			if (!(m_fSwitchWeaponCount < m_fSwitchWeaponTime))
			{
				m_fSwitchWeaponCount = 0f;
				m_bWeaponSwitching = false;
				FinishSwitchWeapon();
			}
		}
	}

	public bool IsSwitching()
	{
		return m_bWeaponSwitching;
	}

	private void WindowsInput()
	{
		if (!ShouldLock)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if (!ShouldLock)
		{
			if (!m_bAim)
			{
				m_CameraScript.YawCamera(Input.GetAxis("Mouse X") / (float)Screen.width * 5);
				m_CameraScript.PitchCamera(Input.GetAxis("Mouse Y") / (float)Screen.height * 5);
			}
			else
			{
				//m_CameraScript.AimMove(Input.GetAxis("Mouse X") / (float)Screen.width, Input.GetAxis("Mouse Y")  / (float)Screen.height * 5);
				m_CameraScript.YawCamera(Input.GetAxis("Mouse X") / (float)Screen.width * 7);
				m_CameraScript.PitchCamera(Input.GetAxis("Mouse Y") / (float)Screen.height * 7);
			}
		
			if (Input.GetMouseButtonDown(0))
			{
				SetAutoShoot(true);
			}
			if (Input.GetMouseButtonUp(0))
			{
				SetAutoShoot(false);
			}
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			SwitchWeapon();
		}
		if (Input.GetMouseButtonDown(1))
		{
			WorkArounds.DoTheCoroutine(DoTheAim());
		}
	}

	private bool IsHasReloadAnimEvent(int nWeaponID)
	{
		if (nWeaponID == 1 || nWeaponID == 2 || nWeaponID == 7 || nWeaponID == 4 || nWeaponID == 12 || nWeaponID == 16 || nWeaponID == 32)
		{
			return true;
		}
		return false;
	}

	public virtual void SetPauseAudio(bool bPause)
	{
	}

	public virtual int GetTurtorialNPC()
	{
		return -1;
	}

	public virtual bool IsLidAnim()
	{
		return false;
	}

	public virtual bool IsSuperMarketAnim()
	{
		return false;
	}

	public virtual bool IsUnderWayAnim()
	{
		return false;
	}

	public virtual GameObject GetTNT()
	{
		return null;
	}

	public virtual iZombieSniperBunker GetBunkerScript()
	{
		return null;
	}

	public virtual iZombieSniperTruck GetTruckScript()
	{
		return null;
	}

	public virtual float GetSlowTimeRate()
	{
		return 0f;
	}

	public virtual bool IsPlayCG()
	{
		return false;
	}

	public virtual void SetPlayCG(bool bValue)
	{
	}

	public virtual bool IsWaWaBegun()
	{
		return false;
	}

	public void InitCross(float dis, float backspeed)
	{
		m_fCrossDis = dis * (float)m_GameState.m_nHDFactor;
		m_fCurCrossDis = m_fCrossDis;
		m_fCrossBackSpeed = backspeed * (float)m_GameState.m_nHDFactor;
		m_fCurCrossBackSpeed = m_fCrossBackSpeed;
		m_GameSceneUI.SetCrossPos(m_fCurCrossDis);
	}

	public void UpdateCrossAnim(float deltaTime)
	{
		if (m_fCurCrossDis > m_fCrossDis)
		{
			m_fCurCrossDis -= m_fCurCrossBackSpeed * deltaTime;
			if (m_fCurCrossDis < m_fCrossDis)
			{
				m_fCurCrossDis = m_fCrossDis;
			}
			m_GameSceneUI.SetCrossPos(m_fCurCrossDis);
		}
	}

	public void CrossFat(float dis, float backspeed = 0f)
	{
		m_fCurCrossDis += dis * (float)m_GameState.m_nHDFactor;
		if (m_fCurCrossDis >= 12f * (float)m_GameState.m_nHDFactor)
		{
			m_fCurCrossDis = 12f * (float)m_GameState.m_nHDFactor;
		}
		if (backspeed > 0f)
		{
			m_fCurCrossBackSpeed = backspeed * (float)m_GameState.m_nHDFactor;
		}
		else
		{
			m_fCurCrossBackSpeed = m_fCrossBackSpeed;
		}
	}

	public void AddThrowMine(int nCount)
	{
		for (int i = 0; i < nCount; i++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(m_PerfabManager.ThrowMine, m_ShootPos.transform.position, Quaternion.identity);
			if (gameObject != null)
			{
				gameObject.tag = "ThrowMine";
				iZombieSniperThrowMine component = gameObject.GetComponent<iZombieSniperThrowMine>();
				if (component != null)
				{
					component.Initialize(m_SoliderMesh.transform.forward, 2000f, 10f, 10f);
				}
			}
		}
	}

	public void AddThrowMineRect(iZombieSniperThrowMine script)
	{
		m_ltThrowMine.Add(script);
	}

	public void RemoveThrowMineRect(iZombieSniperThrowMine script)
	{
		m_ltThrowMine.Remove(script);
	}

	public void AddHeadShootDeath(Vector3 v3Pos, Vector3 v3Dir, NpcType NT)
	{
		if (NT == NpcType.Zombie)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(m_PerfabManager.ZombiePop, v3Pos, Quaternion.LookRotation(v3Dir));
		}
		if (NT == NpcType.ZombieElite)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(m_PerfabManager.ZombieElitePop, v3Pos, Quaternion.LookRotation(v3Dir));
		}
	}

	public virtual iZombieSniperGameHelp GetGameHelp()
	{
		return null;
	}

	public virtual bool SwitchWeaponTool()
	{
		return false;
	}

	public virtual bool IsSpecialButtonDone()
	{
		return m_bUsedACE;
	}

	public virtual bool IsNeedClimbout(int nStartPoint)
	{
		return false;
	}
}
