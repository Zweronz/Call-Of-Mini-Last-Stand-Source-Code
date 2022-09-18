using System.Collections;
using UnityEngine;

public class iZombieSniperBunker : MonoBehaviour
{
	private class MyCompare : IComparer
	{
		int IComparer.Compare(object obj1, object obj2)
		{
			if (((ZombieDisInfo)obj1).m_fDis < ((ZombieDisInfo)obj2).m_fDis)
			{
				return -1;
			}
			return 1;
		}
	}

	private enum BunkerState
	{
		kIdle = 0,
		kNoBullet = 1,
		kAttacking = 2
	}

	private class ZombieDisInfo
	{
		public int m_nUID;

		public float m_fDis;
	}

	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameState m_GameState;

	public bool m_bDestroyed;

	private BunkerState m_BunkerState;

	public bool m_bActive;

	private float m_fSearchTime;

	private float m_fSearchCount;

	private float m_fTimeCount;

	private GameObject m_Swat;

	private GameObject m_SwatShootMouse;

	private TAudioController m_SwatAudioController;

	private ArrayList m_ZombieDisList;

	private ArrayList m_ZombieRaidList;

	private MyCompare m_MyCompare;

	public bool m_bDead;

	public float m_fShootDmg;

	public int m_nBulletNum;

	public float m_fShootRate;

	public float m_fWarningRange;

	public float m_fAttractRange;

	public int m_iMaxCount;

	public float m_fLife;

	public float m_fSize;

	public Vector3 m_v3Postion;

	public void Destroy()
	{
		m_bActive = false;
		if (m_ZombieDisList != null)
		{
			m_ZombieDisList.Clear();
			m_ZombieDisList = null;
		}
		if (m_ZombieRaidList != null)
		{
			m_ZombieRaidList.Clear();
			m_ZombieRaidList = null;
		}
		if (m_MyCompare != null)
		{
			m_MyCompare = null;
		}
	}

	private void Awake()
	{
		m_bActive = false;
	}

	private void Update()
	{
		if (!m_bActive || m_bDead)
		{
			return;
		}
		switch (m_BunkerState)
		{
		case BunkerState.kIdle:
			m_fSearchCount += Time.deltaTime;
			if (m_fSearchCount >= m_fSearchTime)
			{
				m_fSearchCount = 0f;
				SearchZombie();
				if (m_ZombieDisList.Count > 0)
				{
					m_BunkerState = BunkerState.kAttacking;
				}
			}
			break;
		case BunkerState.kAttacking:
		{
			ZombieDisInfo zombieDisInfo = FindClosest();
			if (zombieDisInfo != null)
			{
				iZombieSniperNpc nPC = m_GameScene.GetNPC(zombieDisInfo.m_nUID);
				if (nPC != null && !nPC.IsDead())
				{
					m_fTimeCount += Time.deltaTime;
					if (m_fTimeCount >= m_fShootRate)
					{
						nPC.OnHit(m_v3Postion, m_fShootDmg, Vector3.zero);
						m_fTimeCount = 0f;
						m_Swat.transform.forward = nPC.m_ModelTransForm.position - m_Swat.transform.position + new Vector3(0f, 0.6f, 0f);
						m_GameScene.AddGunFlightEffect(FIRE_EFFECT_TYPE.GUN, m_SwatShootMouse, m_Swat.transform.forward);
						if (m_Swat.GetComponent<Animation>() != null && m_Swat.GetComponent<Animation>()["Fire01"] != null)
						{
							m_Swat.GetComponent<Animation>().Play("Fire01");
						}
						if (m_SwatAudioController != null)
						{
							m_SwatAudioController.PlayAudio("RifleFire01");
						}
						m_nBulletNum--;
						if (m_nBulletNum <= 0)
						{
							m_nBulletNum = 0;
							m_GameScene.PlayAudio("VoChar04");
							m_GameScene.m_GameSceneUI.AddImageWarn1(ImageMsgEnum1.NO_AMMO);
							m_BunkerState = BunkerState.kNoBullet;
						}
						if (m_GameScene.m_GameState.m_nTowerBullet > 10)
						{
							m_GameScene.m_GameState.m_nTowerBullet--;
						}
					}
				}
				if (nPC == null || nPC.IsDead())
				{
					RemoveEnemy(zombieDisInfo);
				}
			}
			else
			{
				m_BunkerState = BunkerState.kIdle;
				m_fSearchCount = m_fSearchTime;
				m_Swat.transform.eulerAngles = new Vector3(0f, m_Swat.transform.eulerAngles.y, m_Swat.transform.eulerAngles.z);
				if (m_Swat.GetComponent<Animation>() != null && m_Swat.GetComponent<Animation>()["Idle01"] != null)
				{
					m_Swat.GetComponent<Animation>()["Idle01"].wrapMode = WrapMode.Loop;
					m_Swat.GetComponent<Animation>().Play("Idle01");
				}
			}
			break;
		}
		case BunkerState.kNoBullet:
			break;
		}
	}

	public void Initialize(float fShootDmg, int nBulletNum, float fShootRate, float fWarningRange)
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = m_GameScene.m_GameState;
		m_ZombieDisList = new ArrayList();
		m_ZombieRaidList = new ArrayList();
		m_MyCompare = new MyCompare();
		m_BunkerState = BunkerState.kIdle;
		m_bActive = true;
		m_fSearchTime = 1f;
		m_fSearchCount = 0f;
		m_fTimeCount = 0f;
		m_bDead = false;
		m_fShootDmg = fShootDmg;
		m_nBulletNum = nBulletNum;
		m_fShootRate = fShootRate;
		m_fAttractRange = 10f;
		m_fWarningRange = fWarningRange;
		m_iMaxCount = 5;
		m_fLife = 60f;
		m_fSize = 2f;
		Transform transform = base.transform.Find("Box01");
		if (transform != null)
		{
			m_v3Postion = transform.position;
		}
		base.GetComponent<Animation>()["daoda"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["daoda"].time = 0f;
		base.GetComponent<Animation>()["daoda"].speed = 0f;
		base.GetComponent<Animation>().Play("daoda");
		if (m_GameState.m_nTowerLvl == 0)
		{
			m_bActive = false;
			base.gameObject.SetActiveRecursively(false);
		}
		else
		{
			m_bActive = true;
			base.gameObject.SetActiveRecursively(true);
		}
		m_Swat = base.transform.Find("Box01/Swat").gameObject;
		if (m_Swat.transform.parent.gameObject.activeInHierarchy)
		{
			m_Swat.SetActiveRecursively(true);
		}
		m_SwatShootMouse = m_Swat.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Weapon_Dummy/WeaponShot").gameObject;
		if (m_Swat.GetComponent<Animation>() != null && m_Swat.GetComponent<Animation>()["Idle01"] != null)
		{
			m_Swat.GetComponent<Animation>()["Idle01"].wrapMode = WrapMode.Loop;
			m_Swat.GetComponent<Animation>().Play("Idle01");
		}
		m_SwatAudioController = m_Swat.GetComponent<TAudioController>();
	}

	public void SearchZombie()
	{
		if (m_GameScene == null || m_GameScene.m_NPCMap == null)
		{
			return;
		}
		m_ZombieDisList.Clear();
		float num = 0f;
		foreach (iZombieSniperNpc value in m_GameScene.m_NPCMap.Values)
		{
			if (!value.IsDead() && !value.IsInnocents())
			{
				num = Vector3.Distance(m_v3Postion, value.m_ModelTransForm.position);
				if (num <= m_fWarningRange)
				{
					ZombieDisInfo zombieDisInfo = new ZombieDisInfo();
					zombieDisInfo.m_nUID = value.m_nUID;
					zombieDisInfo.m_fDis = num;
					m_ZombieDisList.Add(zombieDisInfo);
				}
			}
		}
		m_ZombieDisList.Sort(m_MyCompare);
	}

	private ZombieDisInfo FindClosest()
	{
		if (m_ZombieDisList.Count <= 0)
		{
			return null;
		}
		return (ZombieDisInfo)m_ZombieDisList[0];
	}

	private void RemoveEnemy(ZombieDisInfo info)
	{
		m_ZombieDisList.Remove(info);
	}

	public void AddRaidEnemy(int nUID)
	{
		if (!m_ZombieRaidList.Contains(nUID))
		{
			m_ZombieRaidList.Add(nUID);
		}
	}

	public void RemoveRaidEnemy(int nUID)
	{
		m_ZombieRaidList.Remove(nUID);
	}

	public bool IsRaidEnemy(int nUID)
	{
		return m_ZombieRaidList.Contains(nUID);
	}

	public bool IsFull()
	{
		return m_ZombieRaidList.Count >= m_iMaxCount;
	}

	public void AddHP(float fHP)
	{
		m_fLife += fHP;
		if (m_fLife <= 0f)
		{
			OnDead();
		}
	}

	public void OnDead()
	{
		m_bDead = true;
		m_ZombieDisList.Clear();
		m_ZombieRaidList.Clear();
		base.GetComponent<Animation>()["daoda"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["daoda"].time = 0f;
		base.GetComponent<Animation>()["daoda"].speed = 1f;
		base.GetComponent<Animation>().Play("daoda");
		m_Swat.SetActiveRecursively(false);
	}

	public bool IsDead()
	{
		return m_bDead;
	}

	public bool IsActive()
	{
		return m_bActive;
	}

	public Vector3 GetPostion()
	{
		return m_v3Postion;
	}
}
