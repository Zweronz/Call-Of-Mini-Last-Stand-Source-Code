using System.Collections;
using UnityEngine;

public class iZombieSniperGameScene2 : iZombieSniperGameSceneBase
{
	public enum MovieState
	{
		kMovieSTEP1 = 0
	}

	public GameObject m_PathManager;

	private iZombieSniperMoonWalk m_CameraMove;

	public GameObject m_Ahhhh;

	public GameObject m_Bunker;

	public iZombieSniperBunker m_iBunkerScript;

	public GameObject m_TNT;

	public MovieState m_MovieState;

	private float m_fMovieTime;

	private float m_fGameOverTime;

	private float m_fGameOverTimeRecover;

	private float m_fGameOverTimeMax;

	private GameObject m_SandBag;

	private GameObject m_Player;

	private bool m_bAppearPredator;

	private bool m_bAppearWaWaZombie;

	private GameObject m_CGAppearPredator;

	private GameObject m_CGWaWaZombie;

	private GameObject m_WaWaSquare;

	public float m_fAppearPredatorAnimTime;

	public float m_fAppearWaWaAnimTime;

	public float m_fSlowTimeRate = 0.1f;

	public bool m_bUsedMachine;

	public Vector2 m_v2TouchPos = Vector2.zero;

	private float m_fGameOverTimeingCount = 2f;

	private int m_nGameOverNPCCount;

	public override void Initialize()
	{
		m_bUsedMachine = false;
		base.Initialize();
		m_CameraScript.SetRotateLimit(-20f, 20f, 10f, 145f);
		m_v3EndPoint = new Vector3(-38f, 0f, -1f);
		m_fWarningDis = 25f;
		m_CameraMove = new iZombieSniperMoonWalk();
		m_PathManager = GameObject.Find("PathManager");
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
		if (m_Ahhhh == null)
		{
			m_Ahhhh = GameObject.Find("ah_pfb");
		}
		if (m_TNT == null)
		{
			m_TNT = GameObject.Find("LandMine");
		}
		m_fGameOverTimeMax = 100f;
		m_fGameOverTime = m_fGameOverTimeMax;
		if (m_SandBag == null)
		{
			m_SandBag = GameObject.Find("Sandbag");
		}
		if (m_Player == null)
		{
			m_Player = GameObject.Find("Player");
		}
	}

	public override void ResetData()
	{
		base.ResetData();
		m_Ahhhh.GetComponent<Animation>().Stop("ah");
		m_Ahhhh.SetActiveRecursively(false);
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
		if (m_SandBag != null)
		{
			PlaySceneAnim(m_SandBag, "Dao01", true, 1f);
		}
		if (m_Player != null)
		{
			m_Player.SetActiveRecursively(false);
		}
		m_bAppearPredator = false;
		m_bAppearWaWaZombie = false;
		m_fAppearPredatorAnimTime = 0f;
		m_fAppearWaWaAnimTime = 0f;
		m_fSlowTimeRate = 0f;
	}

	public override void Destroy()
	{
		if (m_iBunkerScript != null)
		{
			m_iBunkerScript.Destroy();
			m_iBunkerScript = null;
		}
		if (m_CGAppearPredator != null)
		{
			Object.Destroy(m_CGAppearPredator);
			m_CGAppearPredator = null;
		}
		if (m_CGWaWaZombie != null)
		{
			Object.Destroy(m_CGWaWaZombie);
			m_CGWaWaZombie = null;
		}
		if (m_WaWaSquare != null)
		{
			Object.Destroy(m_WaWaSquare);
			m_WaWaSquare = null;
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
			m_CameraScript.SetPosition(new Vector3(22f, 1.97f, -48f));
			m_CameraScript.transform.forward = new Vector3(-0.15f, 0.4f, 0.9f);
			m_SoliderMesh.SetActiveRecursively(false);
			m_Player.SetActiveRecursively(true);
			if (m_Player.GetComponent<Animation>() != null && m_Player.GetComponent<Animation>()["01"] != null)
			{
				m_Player.GetComponent<Animation>()["01"].time = 0f;
				m_Player.GetComponent<Animation>()["01"].wrapMode = WrapMode.Loop;
				m_Player.GetComponent<Animation>()["01"].speed = 1f;
				m_Player.GetComponent<Animation>().CrossFade("01");
			}
			m_GameSceneUI.ShowCross(false);
			m_GameSceneUI.ShowGameUI(false);
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
			SetGameState(State.kGameMovie);
			PlayAudio("MusicMap01");
		}
		else
		{
			m_CameraMove = null;
			m_CameraScript.SetPosition(new Vector3(-33f, 2f, -2.6f));
			m_CameraScript.transform.eulerAngles = new Vector3(7f, 85f, 0f);
			m_ZombieWaveCenter.LoadZombieWaveInfoList(m_GameState.m_nCurStage);
			m_ZobieWaveManager.Initialize();
			m_ZombieWaveCenter.LoadLogicWaveInfo(m_GameState.m_nCurStage);
			m_ZobieWaveManagerLogic.Initialize();
			SetGameState(State.kGaming);
			iZombieSniperGameApp.GetInstance().m_bSoundSwitch = true;
			iZombieSniperGameApp.GetInstance().ClearAudio("MusicMap02");
			PlayAudio("MusicMap02");
			m_Player.SetActiveRecursively(false);
			m_BackGroundSound.active = m_GameState.m_bMusicOn;
			if (m_CGAppearPredator == null)
			{
				m_CGAppearPredator = (GameObject)Object.Instantiate(m_PerfabManager.AppearPredator);
				m_CGAppearPredator.SetActiveRecursively(false);
			}
			if (m_CGWaWaZombie == null)
			{
				m_CGWaWaZombie = (GameObject)Object.Instantiate(m_PerfabManager.AppearWaWa);
				m_CGWaWaZombie.SetActiveRecursively(false);
			}
			if (m_WaWaSquare == null)
			{
				m_WaWaSquare = (GameObject)Object.Instantiate(m_PerfabManager.WaWaSquare);
				m_WaWaSquare.transform.position = m_CGWaWaZombie.transform.position;
				m_WaWaSquare.SetActiveRecursively(false);
			}
		}
	}

	public override void UpdateMovie(float deltaTime)
	{
		m_fMovieTime += deltaTime;
		m_CameraMove.Update(deltaTime);
		if (m_CameraMove.IsFinished(iZombieSniperMoonWalk.MoonWalkType.Begin))
		{
			SetPlayCG(false);
			Destroy();
			Initialize();
			ResetData();
			StartGame();
			return;
		}
		UITouchInner[] array = (Application.isMobilePlatform) ? iPhoneInputMgr.MockTouches() : WindowsInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (!(m_GameSceneUI != null) || m_GameSceneUI.m_UIManagerRef.HandleInput(touch))
			{
			}
		}
	}

	private void UpdateGameOverTimeing(float deltaTime)
	{
		if (m_bGameOver)
		{
			return;
		}
		if (m_fGameOverTime < m_fGameOverTimeMax)
		{
			m_GameSceneUI.ShowBloodScreen(1f - m_fGameOverTime / m_fGameOverTimeMax);
			if (!m_isSlow)
			{
				m_GameSceneUI.FadeInDefenceUI();
			}
		}
		else
		{
			m_GameSceneUI.HideBloodScreen();
			if (!m_isSlow)
			{
				m_GameSceneUI.FadeOutDefenceUI();
			}
		}
		if (m_nGameOverNPCCount == 0 && m_fGameOverTime < m_fGameOverTimeMax)
		{
			m_fGameOverTime += 5f * deltaTime;
			if (m_fGameOverTime >= m_fGameOverTimeMax)
			{
				m_fGameOverTime = m_fGameOverTimeMax;
			}
		}
		m_nGameOverNPCCount = 0;
		foreach (iZombieSniperNpc value in m_NPCMap.Values)
		{
			if (value.IsInGameOverRect())
			{
				m_nGameOverNPCCount++;
			}
		}
		m_fGameOverTime -= (float)m_nGameOverNPCCount * 2f * deltaTime;
		m_GameSceneUI.SetDefenceUI(m_fGameOverTime / m_fGameOverTimeMax);
		if (m_fGameOverTime <= 0f)
		{
			m_bGameOver = true;
			m_v3LastDeadInno = Vector3.zero;
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
					m_GameSceneUI.StartAlarmScreen(iZombieSniperGameUI.AlarmScreenSide.All);
					PlayAudio("FxAlarmLoop01");
				}
				else
				{
					m_GameSceneUI.StopAlarmScreen(iZombieSniperGameUI.AlarmScreenSide.All);
					StopAudio("FxAlarmLoop01");
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
			UpdateGameOverTimeing(deltaTime);
			if (m_CameraMove != null)
			{
				m_CameraMove.Update(deltaTime);
			}
		}
		UITouchInner[] array = (Application.isMobilePlatform) ? iPhoneInputMgr.MockTouches() : WindowsInputMgr.MockTouches();
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
							if (Application.isMobilePlatform)
							Aim(touch.position);
						}
						else
						{
							CloseAim(false);
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
		if (!m_bAppearPredator && m_fGameTimeTotal > 120f)
		{
			m_bAppearPredator = true;
			if (m_GameState.m_bCutScenes)
			{
				m_fSlowTimeRate = 0.1f;
				m_fAppearPredatorAnimTime = 8f;
				if (m_CGAppearPredator != null)
				{
					m_CGAppearPredator.SetActiveRecursively(true);
					cgController component = m_CGAppearPredator.GetComponent<cgController>();
					if (component != null)
					{
						component.Play(1f / m_fSlowTimeRate);
					}
				}
				SetPauseScale(true, m_fSlowTimeRate);
				StartSlow(new Vector3(12.59f, 0.96f, 12.61f), new Vector3(0.9f, 74.16f, 0f), 45f);
				m_GameSceneUI.ShowMovieUI();
			}
			else
			{
				m_fSlowTimeRate = 1f;
				m_fAppearPredatorAnimTime = 10f;
			}
		}
		if (!m_bAppearWaWaZombie && m_fGameTimeTotal > 240f)
		{
			m_bAppearWaWaZombie = true;
			if (m_WaWaSquare != null)
			{
				m_WaWaSquare.SetActiveRecursively(true);
			}
			if (m_GameState.m_bCutScenes)
			{
				m_fSlowTimeRate = 0.1f;
				m_fAppearWaWaAnimTime = 6f;
				if (m_CGWaWaZombie != null)
				{
					m_CGWaWaZombie.SetActiveRecursively(true);
					cgController component2 = m_CGWaWaZombie.GetComponent<cgController>();
					if (component2 != null)
					{
						component2.Play(1f / m_fSlowTimeRate);
					}
				}
				SetPauseScale(true, m_fSlowTimeRate);
				StartSlow(new Vector3(-0.07f, 0.88f, -11.42f), new Vector3(6.14f, 503.36f, 0f), 45f);
				m_GameSceneUI.ShowMovieUI();
			}
			else
			{
				m_fSlowTimeRate = 1f;
				m_fAppearWaWaAnimTime = 10f;
			}
		}
		if (m_bAppearPredator && m_fAppearPredatorAnimTime > 0f)
		{
			m_fAppearPredatorAnimTime -= deltaTime * (1f / m_fSlowTimeRate);
			if (m_fAppearPredatorAnimTime <= 0f)
			{
				m_fAppearPredatorAnimTime = 0f;
				if (m_CGAppearPredator != null)
				{
					Object.Destroy(m_CGAppearPredator);
					m_CGAppearPredator = null;
				}
				if (m_bSlowScale)
				{
					SetPauseScale(false, 1f);
					EndSlow();
					m_GameSceneUI.HideMovieUI();
				}
			}
		}
		if (!m_bAppearWaWaZombie || !(m_fAppearWaWaAnimTime > 0f))
		{
			return;
		}
		m_fAppearWaWaAnimTime -= deltaTime * (1f / m_fSlowTimeRate);
		if (m_fAppearWaWaAnimTime <= 0f)
		{
			m_fAppearWaWaAnimTime = 0f;
			if (m_CGWaWaZombie != null)
			{
				Object.Destroy(m_CGWaWaZombie);
				m_CGWaWaZombie = null;
			}
			if (m_bSlowScale)
			{
				SetPauseScale(false, 1f);
				EndSlow();
				m_GameSceneUI.HideMovieUI();
			}
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
			m_CameraScript.SetPosition(new Vector3(-29.03543f, 5.108403f, -7.282847f));
			m_CameraScript.transform.eulerAngles = new Vector3(22.13716f, -6.62262f, 25.62555f);
			m_CameraScript.m_CameraState = iZombieSniperCamera.CameraState.kMovie;
			if (m_Ahhhh != null)
			{
				m_Ahhhh.SetActiveRecursively(true);
				m_Ahhhh.GetComponent<Animation>()["ah"].wrapMode = WrapMode.Loop;
				m_Ahhhh.GetComponent<Animation>().Play("ah");
			}
			if (m_SandBag != null)
			{
				PlayAudio("FxSandBag01");
				PlaySceneAnim(m_SandBag, "Dao01", false, 1f);
				SetPauseScale(true, 0.5f);
			}
			if (m_Player != null)
			{
				m_Player.SetActiveRecursively(true);
				if (m_Player.GetComponent<Animation>() != null && m_Player.GetComponent<Animation>()["Death01"] != null)
				{
					m_Player.GetComponent<Animation>()["Death01"].time = 0f;
					m_Player.GetComponent<Animation>()["Death01"].wrapMode = WrapMode.ClampForever;
					m_Player.GetComponent<Animation>()["Death01"].speed = 0.5f;
					m_Player.GetComponent<Animation>().CrossFade("Death01");
				}
			}
			iZombieSniperGameApp.GetInstance().ClearAudio("VoGAMEOVER");
			PlayAudio("VoGAMEOVER");
			m_fGameOveringTime = 2f;
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
			PlayAudio("FxMetalHit01");
		}
	}

	public override void GameOver()
	{
		if (m_OilCanMap.Count == 0)
		{
			m_GameState.m_bDestroyAllBoomer = true;
		}
		if (m_BackGroundSound != null)
		{
			m_BackGroundSound.active = false;
		}
		SetPauseScale(false, 1f);
		m_GameSceneUI.ShowDefenceUI(false);
		base.GameOver();
	}

	public void SetFireSceneShow(bool bShow)
	{
	}

	public override bool SwitchWeaponTool()
	{
		if (IsSwitching())
		{
			FinishSwitchWeapon();
		}
		int nOldWeaponID = -1;
		if (m_CurrWeapon != null)
		{
			nOldWeaponID = m_CurrWeapon.m_nWeaponID;
		}
		if (!m_GameState.SwitchWeaponByID(40))
		{
			return false;
		}
		m_CurrWeapon = m_GameState.GetUserWeapon();
		iWeaponInfoBase weaponInfoBase = m_GunCenter.GetWeaponInfoBase(m_CurrWeapon.m_nWeaponID);
		if (weaponInfoBase == null)
		{
			return false;
		}
		m_GameSceneUI.UpdateBulletUI(m_CurrWeapon.m_nCurrBulletNum);
		m_GameSceneUI.UpdateWeaponButton();
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
		PlayAudio("WeaponPickUp");
		return true;
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

	public override float GetSlowTimeRate()
	{
		return 0.1f;
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

	public override bool IsSpecialButtonDone()
	{
		return m_bUsedMachine;
	}

	public override void AC130()
	{
		if (!m_bUsedMachine && m_GameState.m_nMachineGun >= 1 && !m_GameState.IsInWeaponTool())
		{
			if (IsAim())
			{
				CloseAim(true);
			}
			SwitchWeaponTool();
			m_GameSceneUI.ShowWeaponUI(false);
			m_bUsedMachine = true;
			m_GameState.m_nMachineGun--;
		}
	}

	public override bool IsNeedClimbout(int nStartPoint)
	{
		if (nStartPoint == 9)
		{
			return true;
		}
		return false;
	}

	public override bool IsWaWaBegun()
	{
		return m_bAppearWaWaZombie;
	}
}
