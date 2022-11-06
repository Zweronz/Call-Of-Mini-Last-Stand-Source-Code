using System.Collections;
using UnityEngine;

public class iZombieSniperNpc
{
	public class NpcState
	{
		public delegate void OnMoveEndDelegate();

		public virtual void Contiune(iZombieSniperNpc npc)
		{
		}

		public virtual void Enter(iZombieSniperNpc npc)
		{
		}

		public virtual void Loop(iZombieSniperNpc npc, float deltaTime)
		{
		}

		public virtual void Exit(iZombieSniperNpc npc)
		{
		}

		public virtual void Yield(iZombieSniperNpc npc)
		{
		}
	}

	public class DestroyState : NpcState
	{
		private enum Status
		{
			kFalling = 0,
			kWaiting = 1,
			kDowning = 2
		}

		private float m_fStartDownTime;

		private float m_fAlpha;

		private Status m_Status;

		public override void Enter(iZombieSniperNpc npc)
		{
			if (npc.IsZombiePredator() && npc.m_ModelTransForm.position.y > 0f)
			{
				npc.SetState(((iZombieSniperZombiePredator)npc).stFallState);
				return;
			}
			m_fStartDownTime = 0f;
			m_Status = Status.kWaiting;
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			if (npc.m_bNeedDestroy)
			{
				return;
			}
			switch (m_Status)
			{
			case Status.kWaiting:
				m_fStartDownTime += deltaTime;
				if (m_fStartDownTime > 5f)
				{
					m_Status = Status.kDowning;
					m_fAlpha = 1f;
					npc.Clear();
				}
				break;
			case Status.kDowning:
				m_fAlpha -= deltaTime * 0.5f;
				if (m_fAlpha <= 0f)
				{
					m_fAlpha = 0f;
					npc.m_bNeedDestroy = true;
				}
				if (npc.m_bFrameOrBone)
				{
					Color color = npc.m_MeshRenderer.material.color;
					npc.m_MeshRenderer.material.color = new Color(color.r, color.b, color.g, m_fAlpha);
				}
				else if (npc.m_Model.GetComponent<Renderer>() != null)
				{
					Color color2 = npc.m_Model.GetComponent<Renderer>().material.color;
					npc.m_Model.GetComponent<Renderer>().material.color = new Color(color2.r, color2.b, color2.g, m_fAlpha);
				}
				npc.m_ModelTransForm.position -= new Vector3(0f, (0.8f - 0.8f * m_fAlpha) * 4f * deltaTime, 0f);
				break;
			}
		}
	}

	public class DelayBloodState : NpcState
	{
		private float m_fDelayTime;

		private Vector3 m_v3Point;

		private ACTION_ENUM m_ActionType;

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			m_fDelayTime -= Time.deltaTime;
			if (m_fDelayTime <= 0f)
			{
				npc.m_GameScene.AddBloodEffect(m_v3Point);
				npc.m_Anim.PlayAnim(m_ActionType, -1, 0f);
				npc.SetNextState();
			}
		}

		public override void Exit(iZombieSniperNpc npc)
		{
			m_fDelayTime = 0f;
			m_v3Point = Vector3.zero;
			m_ActionType = ACTION_ENUM.IDLE;
		}

		public void Initialize(float fDelayTime, Vector3 v3Point, ACTION_ENUM actionType)
		{
			m_fDelayTime = fDelayTime;
			m_v3Point = v3Point;
			m_ActionType = actionType;
		}
	}

	public class MoveState : NpcState
	{
		protected bool m_bAvoiding;

		protected float m_fAvoidTime;

		protected bool m_bDestBlock;

		protected Vector3 m_v3Dest = Vector3.zero;

		protected ArrayList m_Path = new ArrayList();

		protected ArrayList m_AvoidPath = new ArrayList();

		protected bool m_bRotateBody;

		protected Vector3 m_v3BeginRot = Vector3.zero;

		protected Vector3 m_v3EndRot = Vector3.zero;

		protected float m_fRotRate;

		protected float m_fMoveSpeed;

		protected bool m_bRandPos = true;

		public NpcState m_stMovePurpose;

		public OnMoveEndDelegate m_OnMoveEndFunc;

		public void Destroy()
		{
			if (m_Path != null)
			{
				m_Path.Clear();
				m_Path = null;
			}
			if (m_AvoidPath != null)
			{
				m_AvoidPath.Clear();
				m_AvoidPath = null;
			}
		}

		public void Reset()
		{
			m_bAvoiding = false;
			m_fAvoidTime = 0f;
			m_bDestBlock = false;
			m_v3Dest = Vector3.zero;
			m_Path.Clear();
			m_AvoidPath.Clear();
			m_bRotateBody = false;
			m_v3BeginRot = Vector3.zero;
			m_v3EndRot = Vector3.zero;
			m_fRotRate = 0f;
			m_fMoveSpeed = 0f;
			m_stMovePurpose = null;
			m_bRandPos = true;
		}

		public virtual bool OnBlockFunc(iZombieSniperNpc npc)
		{
			return false;
		}

		public virtual void OnMoveEndFunc(iZombieSniperNpc npc)
		{
			if (m_OnMoveEndFunc != null)
			{
				m_OnMoveEndFunc();
			}
		}

		public virtual bool OnMoveFunc(iZombieSniperNpc npc, Vector3 v3Dest)
		{
			if (m_stMovePurpose == null)
			{
				return false;
			}
			if (m_stMovePurpose == npc.stAttackState)
			{
				int target = npc.stAttackState.GetTarget();
				iZombieSniperNpc nPC = npc.m_GameScene.GetNPC(target);
				if (nPC == null || nPC.IsDead())
				{
					npc.SetNextState();
					m_stMovePurpose = null;
					return true;
				}
				if (Utils.IsCollide(v3Dest, npc.m_ZombieBaseInfo.m_fSize, nPC.m_ModelTransForm.position, nPC.m_ZombieBaseInfo.m_fSize))
				{
					Vector3 normalized = (npc.m_ModelTransForm.position - nPC.m_ModelTransForm.position).normalized;
					npc.m_ModelTransForm.position = nPC.m_ModelTransForm.position + normalized * (npc.m_ZombieBaseInfo.m_fSize + nPC.m_ZombieBaseInfo.m_fSize);
					npc.SetStateDirectly(m_stMovePurpose);
					m_stMovePurpose = null;
					return true;
				}
				m_v3Dest = nPC.m_ModelTransForm.position;
				Vector3 vector = m_v3Dest - npc.m_ModelTransForm.position;
				vector.y = 0f;
				if (vector != Vector3.zero)
				{
					npc.m_ModelTransForm.forward = vector;
				}
			}
			return false;
		}

		public virtual void ChangeSpeed(iZombieSniperNpc npc)
		{
			if (m_stMovePurpose == npc.stAttackState)
			{
				m_fMoveSpeed = npc.m_ZombieBaseInfo.m_fMoveSpeed * 1.5f;
			}
			else
			{
				m_fMoveSpeed = npc.m_ZombieBaseInfo.m_fMoveSpeed;
			}
			if (m_fMoveSpeed > 1f)
			{
				npc.m_Anim.PlayAnimRandom(ACTION_ENUM.RUN, 0, m_fMoveSpeed, true);
			}
			else
			{
				npc.m_Anim.PlayAnimRandom(ACTION_ENUM.WALK, 0, m_fMoveSpeed, true);
			}
		}

		public override void Enter(iZombieSniperNpc npc)
		{
			ChangeSpeed(npc);
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			if (npc.m_Model == null)
			{
				return;
			}
			if (m_v3Dest == Vector3.zero)
			{
				m_v3Dest = GetNextPoint();
				if (m_v3Dest == Vector3.zero)
				{
					npc.SetNextState();
					OnMoveEndFunc(npc);
					return;
				}
				TurnRound(npc, (m_v3Dest - npc.m_ModelTransForm.position).normalized);
			}
			if (m_bRotateBody)
			{
				m_fRotRate += deltaTime * 5f;
				Vector3 vector = Vector3.Lerp(m_v3BeginRot, m_v3EndRot, m_fRotRate);
				if (vector != Vector3.zero)
				{
					npc.m_ModelTransForm.forward = vector;
				}
				if (m_fRotRate < 1f)
				{
					return;
				}
				m_bRotateBody = false;
			}
			Vector3 vector2 = m_v3Dest - npc.m_ModelTransForm.position;
			vector2.y = 0f;
			float magnitude = vector2.magnitude;
			Vector3 vector3 = vector2 / magnitude;
			float num = m_fMoveSpeed * deltaTime;
			Vector3 vector4 = npc.m_ModelTransForm.position + num * vector3;
			if (m_bAvoiding)
			{
				m_fAvoidTime += deltaTime;
			}
			if (npc.m_GridInfoCenter.IsBlock(vector4.x, vector4.z))
			{
				if (!OnBlockFunc(npc))
				{
					if (!m_bDestBlock && m_fAvoidTime < 5f)
					{
						MoveAvoid(npc);
						return;
					}
					npc.SetNextState();
					OnMoveEndFunc(npc);
				}
			}
			else
			{
				if (OnMoveFunc(npc, vector4))
				{
					return;
				}
				if (magnitude <= num)
				{
					npc.m_ModelTransForm.position = m_v3Dest;
					m_v3Dest = GetNextPoint();
					if (m_v3Dest == Vector3.zero)
					{
						npc.SetNextState();
						OnMoveEndFunc(npc);
					}
					else
					{
						TurnRound(npc, (m_v3Dest - npc.m_ModelTransForm.position).normalized);
					}
				}
				else
				{
					npc.m_ModelTransForm.position = vector4;
					npc.m_ModelTransForm.forward = vector3;
				}
			}
		}

		public override void Exit(iZombieSniperNpc npc)
		{
			Reset();
		}

		public override void Yield(iZombieSniperNpc npc)
		{
			if (!(m_v3Dest == Vector3.zero))
			{
				if (m_bAvoiding)
				{
					m_AvoidPath.Insert(0, m_v3Dest);
				}
				else
				{
					m_Path.Insert(0, m_v3Dest);
				}
				m_v3Dest = Vector3.zero;
			}
		}

		public void Initialize(iZombieSniperNpc npc, Vector3 v3Begin)
		{
			Reset();
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			foreach (WayPoint item in npc.m_WayPointCenter.WayPointMap())
			{
				if (npc.IsZombieZombieDog())
				{
					if ((4 & item.m_nFlag) == 0)
					{
						continue;
					}
				}
				else if (npc.IsInnocents() && npc.m_GameState.IsInnocentsHasOwnPath())
				{
					if ((8 & item.m_nFlag) == 0)
					{
						continue;
					}
				}
				else if ((2 & item.m_nFlag) == 0)
				{
					continue;
				}
				num3 = Vector3.Distance(item.m_v3Position, v3Begin);
				if (num2 == 0f || num3 < num2)
				{
					num2 = num3;
					num = item.m_nID;
				}
			}
			if (num != 0)
			{
				int nPathType = 1;
				if (npc.IsZombieZombieDog())
				{
					nPathType = 2;
				}
				else if (npc.IsInnocents() && npc.m_GameState.IsInnocentsHasOwnPath())
				{
					nPathType = 3;
				}
				npc.m_WayPointCenter.GenerateWayPath(nPathType, num, ref m_Path);
				if (npc.IsInnocents())
				{
					m_bRandPos = false;
				}
				else
				{
					m_bRandPos = true;
				}
				ChangeSpeed(npc);
			}
		}

		public void Initialize(iZombieSniperNpc npc, Vector3 v3End, NpcState stState, bool bBlock = false)
		{
			Reset();
			m_Path.Add(v3End);
			m_v3Dest = GetNextPoint();
			TurnRound(npc, (m_v3Dest - npc.m_ModelTransForm.position).normalized);
			m_stMovePurpose = stState;
			m_OnMoveEndFunc = null;
			m_bDestBlock = bBlock;
			m_bRandPos = false;
			ChangeSpeed(npc);
		}

		public void InitializeFunc(iZombieSniperNpc npc, Vector3 v3End, OnMoveEndDelegate OnMoveEnd, bool bBlock = false)
		{
			Reset();
			m_Path.Add(v3End);
			m_v3Dest = GetNextPoint();
			TurnRound(npc, (m_v3Dest - npc.m_ModelTransForm.position).normalized);
			m_stMovePurpose = null;
			m_OnMoveEndFunc = OnMoveEnd;
			m_bDestBlock = bBlock;
			m_bRandPos = false;
			ChangeSpeed(npc);
		}

		public void MoveAvoid(iZombieSniperNpc npc)
		{
			GenerateAvoidPath(npc);
			m_v3Dest = GetNextPoint();
			TurnRound(npc, (m_v3Dest - npc.m_ModelTransForm.position).normalized);
		}

		public void GenerateAvoidPath(iZombieSniperNpc npc)
		{
			Vector3 vector = npc.m_ModelTransForm.position + npc.m_ModelTransForm.right * 1f;
			Vector3 vector2 = vector + npc.m_ModelTransForm.forward * 2f;
			m_AvoidPath.Clear();
			m_AvoidPath.Insert(0, vector2);
			m_AvoidPath.Insert(0, vector);
			Vector3 v3Dest = m_v3Dest;
			Vector3 nextPointPath = GetNextPointPath();
			if (nextPointPath != Vector3.zero && Vector3.Dot(v3Dest - vector2, nextPointPath - vector2) < 0.5f)
			{
				m_Path.Insert(0, nextPointPath);
				return;
			}
			m_Path.Insert(0, nextPointPath);
			if (!m_bAvoiding)
			{
				m_Path.Insert(0, v3Dest);
			}
		}

		public Vector3 GetNextPoint()
		{
			Vector3 vector = GetNextPointAvoid();
			if (vector == Vector3.zero)
			{
				vector = GetNextPointPath();
				m_bAvoiding = false;
				m_fAvoidTime = 0f;
			}
			else
			{
				m_bAvoiding = true;
			}
			return vector;
		}

		public Vector3 GetNextPointPath()
		{
			if (m_Path.Count > 0)
			{
				Vector3 result = (Vector3)m_Path[0];
				if (m_bRandPos)
				{
					result.x += UnityEngine.Random.Range(-2f, 2f);
					result.z += UnityEngine.Random.Range(-2f, 2f);
				}
				m_Path.RemoveAt(0);
				return result;
			}
			return Vector3.zero;
		}

		public Vector3 GetNextPointAvoid()
		{
			if (m_AvoidPath.Count > 0)
			{
				Vector3 result = (Vector3)m_AvoidPath[0];
				m_AvoidPath.RemoveAt(0);
				return result;
			}
			return Vector3.zero;
		}

		public void TurnRound(iZombieSniperNpc npc, Vector3 v3Forward)
		{
			m_bRotateBody = true;
			m_v3BeginRot = npc.m_ModelTransForm.forward;
			m_v3EndRot = v3Forward;
			m_fRotRate = 0f;
		}
	}

	public class WaitState : NpcState
	{
		private float m_fWaitTime;

		private float m_fTimeCount;

		public override void Enter(iZombieSniperNpc npc)
		{
			m_fTimeCount = 0f;
			npc.m_Anim.PlayAnim(ACTION_ENUM.IDLE, -1, 0f, true);
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			if (m_fWaitTime > 0f)
			{
				m_fTimeCount += deltaTime;
				if (m_fTimeCount >= m_fWaitTime)
				{
					m_fTimeCount = 0f;
					m_fWaitTime = 0f;
					npc.SetNextState();
				}
			}
		}

		public void Initialize(float fWaitTime)
		{
			m_fWaitTime = fWaitTime;
		}
	}

	public class AttackState : NpcState
	{
		private int m_nTargetID;

		private float m_fTimeCount;

		private Vector3 m_v3Dir;

		public override void Enter(iZombieSniperNpc npc)
		{
			npc.m_Anim.PlayAnim(ACTION_ENUM.IDLE, -1, 0f, true);
			if (npc.IsInnocents())
			{
				npc.SetWarn(true);
				((iZombieSniperInnocence)npc).SetHelp(true);
			}
		}

		public override void Exit(iZombieSniperNpc npc)
		{
			if (npc.IsInnocents())
			{
				npc.SetWarn(false);
				((iZombieSniperInnocence)npc).SetHelp(false);
			}
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			iZombieSniperNpc nPC = npc.m_GameScene.GetNPC(m_nTargetID);
			if (nPC == null || nPC.IsDead())
			{
				m_nTargetID = 0;
				npc.SetNextState();
				return;
			}
			m_v3Dir = nPC.m_ModelTransForm.position - npc.m_ModelTransForm.position;
			m_v3Dir.y = 0f;
			if (m_v3Dir != Vector3.zero)
			{
				npc.m_ModelTransForm.forward = m_v3Dir;
			}
			m_fTimeCount += deltaTime;
			if (m_fTimeCount < npc.m_ZombieBaseInfo.m_fAtkSpeed)
			{
				return;
			}
			float num = m_v3Dir.sqrMagnitude - nPC.m_ZombieBaseInfo.m_fSize;
			if (num > npc.m_ZombieBaseInfo.m_fAtkRange * npc.m_ZombieBaseInfo.m_fAtkRange)
			{
				npc.stMoveState.Initialize(npc, nPC.m_ModelTransForm.position, this);
				npc.SetStateDirectly(npc.stMoveState);
				return;
			}
			m_fTimeCount = 0f;
			npc.m_Anim.PlayAnim(ACTION_ENUM.ATTACK, 0, npc.m_ZombieBaseInfo.m_fAtkSpeed);
			nPC.AddHP(0f - npc.m_fDamage);
			if (nPC.IsInnocents())
			{
				nPC.m_GameScene.AddBloodEffect(nPC.m_ModelTransForm.position + new Vector3(0f, 0.7f, 0f));
			}
			if (!nPC.IsDead())
			{
				if (nPC.m_State != nPC.stAttackState)
				{
					nPC.stAttackState.SetTarget(npc.m_nUID);
					nPC.SetStateDirectly(nPC.stAttackState);
				}
				if (!nPC.IsInnocents())
				{
				}
			}
			else
			{
				nPC.m_Anim.PlayAnim(ACTION_ENUM.DEAD, -1, 0f);
				npc.SetNextState();
			}
		}

		public void SetTarget(int m_nUID)
		{
			m_nTargetID = m_nUID;
		}

		public int GetTarget()
		{
			return m_nTargetID;
		}
	}

	public class HurtState : NpcState
	{
		private float m_fTimeCount;

		public override void Enter(iZombieSniperNpc npc)
		{
			m_fTimeCount = 0f;
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			m_fTimeCount += deltaTime;
			if (m_fTimeCount >= npc.m_ZombieBaseInfo.m_fFreezeTime)
			{
				m_fTimeCount = 0f;
				npc.SetNextState();
			}
		}
	}

	public class HearState : NpcState
	{
		private Vector3 m_v3SoundPos;

		private bool m_bBlock;

		private float m_fThinkCount;

		private float m_fThinkTime;

		public override void Enter(iZombieSniperNpc npc)
		{
			m_fThinkCount = 0f;
			npc.m_Anim.PlayAnim(ACTION_ENUM.HEAR, 0, 0f);
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			m_fThinkCount += deltaTime;
			if (m_fThinkCount >= m_fThinkTime)
			{
				m_fThinkCount = 0f;
				if (UnityEngine.Random.Range(0, 100) > 30)
				{
					npc.stMoveState.Initialize(npc, m_v3SoundPos, null, m_bBlock);
					npc.SetStateDirectly(npc.stMoveState);
				}
				else
				{
					npc.SetNextState();
				}
			}
		}

		public void Initialize(Vector3 v3SoundPos, bool bBlock, float fThinkTime)
		{
			m_v3SoundPos = v3SoundPos;
			m_bBlock = bBlock;
			m_fThinkTime = fThinkTime;
		}
	}

	public class ClimbState : NpcState
	{
		private float m_fTimeCount;

		public override void Enter(iZombieSniperNpc npc)
		{
			if (npc.m_Model != null && npc.m_Model.GetComponent<Animation>() != null && npc.m_Model.GetComponent<Animation>()["Shin01"] != null)
			{
				npc.m_Model.GetComponent<Animation>()["Shin01"].wrapMode = WrapMode.Once;
				npc.m_Model.GetComponent<Animation>().Play("Shin01");
				m_fTimeCount = npc.m_Model.GetComponent<Animation>()["Shin01"].length;
			}
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			m_fTimeCount -= deltaTime;
			if (m_fTimeCount <= 0f)
			{
				npc.SetNextState();
			}
		}
	}

	public enum Part
	{
		kHead = 0,
		kBody = 1,
		kLegL = 2,
		kLegR = 3
	}

	public int m_nID;

	public int m_nUID;

	public bool m_bCollideFlag;

	public bool m_bIsGotoAttackSand;

	public bool m_bIsInGameOverRect;

	public Transform m_ModelTransForm;

	public GameObject m_Model;

	public GameObject m_ModelHead;

	public GameObject m_ModelBody;

	public MeshRenderer m_MeshRenderer;

	public MeshFilter m_MeshFilter;

	public SkinnedMeshRenderer m_SkinnedMeshRenderer;

	public GameObject m_ModelShadow;

	public iZombieSniperShadow m_ModelShadowScript;

	public GameObject m_ModelBoneStep;

	public bool m_bFrameOrBone = true;

	public CAnim m_Anim;

	public ModelInfo m_ModelInfo;

	public iZombieSniperCamera m_CameraScript;

	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameState m_GameState;

	public iZombieSniperGameUI m_GameSceneUI;

	public iZombieSniperPerfabManager m_PerfabManager;

	public iZombieSniperWayPointCenter m_WayPointCenter;

	public iZombieSniperGridInfoCenter m_GridInfoCenter;

	public bool m_bNeedDestroy;

	public bool m_bDead;

	public ZombieBaseInfo m_ZombieBaseInfo;

	public float m_fCurHP;

	public float m_fMaxHP;

	public float m_fAttackInterval;

	public float m_fDamage;

	public int m_nMiniPointID;

	public int m_nSignPointID;

	public int m_nWarnArrowID;

	public int m_nWarnIconID;

	public CNail m_Sign;

	public bool m_bWarn;

	public GameObject m_WarnObject;

	public bool m_bAlarmTip;

	public bool m_bColorAnim;

	public Vector3 m_v3CurrColor;

	public Vector3 m_v3DestColor;

	public float m_fColorAnimTime;

	public float m_fColorAnimSpeed;

	public float m_fColorFactor;

	public float m_fColorDir = 1f;

	public NpcState m_State;

	public Stack m_StateStack;

	public DestroyState stDestroyState;

	public MoveState stMoveState;

	public DelayBloodState stDelayBloodState;

	public WaitState stWaitState;

	public AttackState stAttackState;

	public HurtState stHurtState;

	public HearState stHearState;

	public ClimbState stClimbState;

	private float m_fThrowMineTimeCheck = 2f;

	private float m_fRefreshMeshColorCount;

	private float m_fUpdateMiniMapTime;

	public bool bColorAnim
	{
		get
		{
			return m_bColorAnim;
		}
	}

	public virtual bool Create(int nID, int nUID, string sName, Vector3 v3Pos)
	{
		return false;
	}

	public virtual void DestroyStateMemory()
	{
		stDestroyState = null;
		if (stMoveState != null)
		{
			stMoveState.Destroy();
		}
		stMoveState = null;
		stDelayBloodState = null;
		stWaitState = null;
		stAttackState = null;
		stHurtState = null;
		stHearState = null;
		stClimbState = null;
	}

	public virtual void InitState()
	{
		stDestroyState = new DestroyState();
		stMoveState = new MoveState();
		stDelayBloodState = new DelayBloodState();
		stWaitState = new WaitState();
		stAttackState = new AttackState();
		stHurtState = new HurtState();
		stHearState = new HearState();
		stClimbState = new ClimbState();
	}

	public virtual void SetDefaultState()
	{
	}

	public virtual bool IsDead()
	{
		return m_bDead;
	}

	public virtual bool IsWillEat()
	{
		return m_State == stMoveState || m_State == stWaitState;
	}

	public virtual bool IsWillCatchSound()
	{
		return m_State == stMoveState || m_State == stWaitState;
	}

	public virtual void EnterGameOverEvent()
	{
	}

	public virtual void UpdateLogic(float deltaTime)
	{
		m_bIsInGameOverRect = false;
		if (Utils.PtInRect(new Vector2(m_ModelTransForm.position.x, m_ModelTransForm.position.z), m_WayPointCenter.m_FinallyZone))
		{
			EnterGameOverEvent();
		}
		else if (m_GameScene.m_MineState == iZombieSniperGameSceneBase.MineState.READY && IsZombie())
		{
			if (Utils.PtInRect(new Vector2(m_ModelTransForm.position.x, m_ModelTransForm.position.z), m_WayPointCenter.m_MineZone) && m_GameState.m_nMineCount > 0)
			{
				Vector3 vector = new Vector3((m_WayPointCenter.m_MineZone.xMin + m_WayPointCenter.m_MineZone.xMax) / 2f, 0f, (m_WayPointCenter.m_MineZone.yMax + m_WayPointCenter.m_MineZone.yMax) / 2f);
				m_GameScene.Boom(vector, 10f, 1000f, true);
				m_GameScene.PlayAudio("FxExploLarge01");
				GameObject obj = (GameObject)Object.Instantiate(m_GameScene.m_PerfabManager.m_Boom02, vector + new Vector3(0f, 1.3f, 0f), Quaternion.identity);
				Object.Destroy(obj, 5f);
				m_GameScene.m_MineState = iZombieSniperGameSceneBase.MineState.USED;
				m_GameState.m_nMineCount--;
				GameObject tNT = m_GameScene.GetTNT();
				if (tNT != null)
				{
					tNT.SetActiveRecursively(false);
				}
				m_GameScene.PlayAudio("Voice02");
				m_GameSceneUI.AddImageWarn1(ImageMsgEnum1.KILL_MANY2_ZOMBIE);
			}
		}
		else
		{
			if (!IsZombie())
			{
				return;
			}
			m_fThrowMineTimeCheck -= deltaTime;
			if (!(m_fThrowMineTimeCheck <= 0f))
			{
				return;
			}
			m_fThrowMineTimeCheck = 1f;
			if (m_GameScene == null || m_GameScene.m_ltThrowMine == null)
			{
				return;
			}
			foreach (iZombieSniperThrowMine item in m_GameScene.m_ltThrowMine)
			{
				if (!item.m_bActive || !Utils.PtInRect(new Vector2(m_ModelTransForm.position.x, m_ModelTransForm.position.z), item.m_Info.m_Rect))
				{
					continue;
				}
				m_GameScene.Boom(item.m_Transform.position, item.m_Info.m_fDamageRadius, item.m_Info.m_fDamage, true);
				m_GameScene.PlayAudio("FxExploEnergyLarge01");
				GameObject obj2 = (GameObject)Object.Instantiate(m_GameScene.m_PerfabManager.m_BoomThrowMine, item.m_Transform.position + new Vector3(0f, 1.3f, 0f), Quaternion.identity);
				Object.Destroy(obj2, 5f);
				m_GameScene.m_ltThrowMine.Remove(item);
				Object.Destroy(item.gameObject);
				break;
			}
		}
	}

	public bool Initialize(int nID, int nUID, string sName, Vector3 v3Pos)
	{
		m_nID = nID;
		m_nUID = nUID;
		m_bIsInGameOverRect = false;
		m_bIsGotoAttackSand = false;
		m_bNeedDestroy = false;
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = m_GameScene.m_GameState;
		m_CameraScript = m_GameScene.m_CameraScript;
		m_GameSceneUI = m_GameScene.m_GameSceneUI;
		m_WayPointCenter = m_GameScene.m_WayPointCenter;
		m_PerfabManager = m_GameScene.m_PerfabManager;
		m_GridInfoCenter = m_GameScene.m_GridInfoCenter;
		m_bFrameOrBone = false;
		m_ZombieBaseInfo = m_GameScene.m_ZombieWaveCenter.GetZombieBaseInfo(nID);
		if (m_ZombieBaseInfo == null)
		{
			return false;
		}
		if (!CreateModel((NpcType)m_ZombieBaseInfo.m_nType, sName, v3Pos))
		{
			return false;
		}
		m_bDead = false;
		m_fCurHP = m_ZombieBaseInfo.m_fLife;
		m_fMaxHP = m_ZombieBaseInfo.m_fLife;
		m_fAttackInterval = 1f;
		m_fDamage = m_ZombieBaseInfo.m_fDamage;
		m_nMiniPointID = -1;
		m_nSignPointID = -1;
		m_nWarnArrowID = -1;
		m_nWarnIconID = -1;
		if (IsZombie())
		{
			m_Sign = m_GameSceneUI.m_SignZombie;
		}
		else
		{
			m_Sign = m_GameSceneUI.m_SignPeople;
		}
		m_bWarn = false;
		m_bAlarmTip = false;
		m_bColorAnim = false;
		m_v3CurrColor = Vector3.zero;
		m_v3DestColor = Vector3.zero;
		m_fColorAnimTime = 0f;
		m_fColorAnimSpeed = 0f;
		m_StateStack = new Stack();
		m_StateStack.Clear();
		InitState();
		return true;
	}

	public bool CreateModel(NpcType nType, string sName, Vector3 v3Pos)
	{
		switch (nType)
		{
		case NpcType.Innocents:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.Innocents, v3Pos, Quaternion.identity);
			break;
		case NpcType.InnocentsDoctor:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.InnocentsDoctor, v3Pos, Quaternion.identity);
			break;
		case NpcType.InnocentsPastor:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.InnocentsPastor, v3Pos, Quaternion.identity);
			break;
		case NpcType.InnocentsPlumber:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.InnocentsPlumber, v3Pos, Quaternion.identity);
			break;
		case NpcType.Zombie:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.Zombie, v3Pos, Quaternion.identity);
			break;
		case NpcType.ZombieElite:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.ZombieElite, v3Pos, Quaternion.identity);
			break;
		case NpcType.Giant:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.ZombieFat, v3Pos, Quaternion.identity);
			break;
		case NpcType.ZombieDog:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.ZombieDog, v3Pos, Quaternion.identity);
			break;
		case NpcType.ZombieNurse:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.ZombieNurse, v3Pos, Quaternion.identity);
			break;
		case NpcType.ZombiePolice:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.ZombiePolice, v3Pos, Quaternion.identity);
			break;
		case NpcType.ZombieSwat:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.ZombieSwat, v3Pos, Quaternion.identity);
			break;
		case NpcType.ZombiePredator:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.ZombiePredator, v3Pos, Quaternion.identity);
			break;
		case NpcType.GiantElite:
			m_Model = (GameObject)Object.Instantiate(m_PerfabManager.ZombieFatElite, v3Pos, Quaternion.identity);
			break;
		}
		if (m_Model == null)
		{
			return false;
		}
		m_Model.name = sName + m_nUID;
		m_ModelTransForm = m_Model.transform;
		m_ModelInfo = m_Model.AddComponent<ModelInfo>();
		m_ModelInfo.m_nUID = m_nUID;
		m_ModelShadow = (GameObject)Object.Instantiate(m_PerfabManager.m_ShadowEffect);
		m_ModelShadow.transform.parent = m_ModelTransForm;
		m_ModelShadow.transform.localPosition = Vector3.zero;
		m_ModelShadowScript = m_ModelShadow.GetComponent<iZombieSniperShadow>();
		if (m_ModelShadowScript != null)
		{
			m_ModelShadowScript.Initialize(m_ZombieBaseInfo.m_fSize / 0.4f);
		}
		Transform transform = null;
		if (m_bFrameOrBone)
		{
			CFrameAnim cFrameAnim = new CFrameAnim();
			cFrameAnim.Initialize(m_Model, this);
			m_Anim = cFrameAnim;
			m_MeshRenderer = m_Model.GetComponentInChildren<MeshRenderer>();
			m_MeshFilter = m_Model.GetComponentInChildren<MeshFilter>();
			transform = m_ModelTransForm.Find("HeadBox");
			if (transform != null)
			{
				m_ModelHead = transform.gameObject;
				m_ModelHead.layer = 28;
			}
			transform = m_ModelTransForm.Find("BodyBox");
			if (transform != null)
			{
				m_ModelBody = transform.gameObject;
				m_ModelBody.layer = 28;
			}
		}
		else
		{
			CBoneAnim cBoneAnim = new CBoneAnim();
			cBoneAnim.Initialize(m_Model, this);
			m_Anim = cBoneAnim;
			foreach (Transform item in m_ModelTransForm)
			{
				m_SkinnedMeshRenderer = item.GetComponent<SkinnedMeshRenderer>();
				if (m_SkinnedMeshRenderer != null)
				{
					break;
				}
			}
			transform = m_ModelTransForm.Find("Bip01/Bip01 Footsteps");
			if (transform != null)
			{
				m_ModelBoneStep = transform.gameObject;
			}
		}
		v3Pos.y = 0.01f;
		m_ModelTransForm.position = v3Pos;
		m_Model.tag = "NPC";
		m_Model.layer = 28;
		return true;
	}

	public virtual void Destroy()
	{
		if (m_StateStack != null)
		{
			m_StateStack.Clear();
			m_StateStack = null;
		}
		if (m_Anim != null)
		{
			m_Anim.Destroy();
			m_Anim = null;
		}
		DestroyStateMemory();
		Clear();
		if (m_Model != null)
		{
			Object.Destroy(m_Model);
			m_Model = null;
		}
	}

	public void Clear()
	{
		if (m_nMiniPointID != -1)
		{
			m_nMiniPointID = -1;
		}
		if (m_nSignPointID != -1)
		{
			if (m_Sign != null)
			{
				m_Sign.RemovePoint(m_nSignPointID);
			}
			m_nSignPointID = -1;
		}
		if (m_nWarnArrowID != -1)
		{
			if (m_GameSceneUI != null && m_GameSceneUI.m_WarnArrow != null)
			{
				m_GameSceneUI.m_WarnArrow.RemovePoint(m_nWarnArrowID);
			}
			m_nWarnArrowID = -1;
		}
		if (m_nWarnIconID != -1)
		{
			if (m_GameSceneUI != null && m_GameSceneUI.m_WarnIcon != null)
			{
				m_GameSceneUI.m_WarnIcon.RemovePoint(m_nWarnIconID);
			}
			m_nWarnIconID = -1;
		}
		if (m_WarnObject != null)
		{
			Object.Destroy(m_WarnObject);
			m_WarnObject = null;
		}
		if (m_ModelShadowScript != null)
		{
			m_ModelShadowScript = null;
		}
		if (m_ModelShadow != null)
		{
			Object.Destroy(m_ModelShadow);
			m_ModelShadow = null;
		}
		iZombieSniperBunker bunkerScript = m_GameScene.GetBunkerScript();
		if (bunkerScript != null && bunkerScript.IsRaidEnemy(m_nUID))
		{
			bunkerScript.RemoveRaidEnemy(m_nUID);
		}
	}

	public Vector3 CheckGround(Vector3 v3Pos)
	{
		Vector3 result = v3Pos;
		v3Pos.y = 1000f;
		RaycastHit hitInfo;
		Physics.Raycast(v3Pos, Vector3.down, out hitInfo, float.PositiveInfinity, 536870912);
		result.y = hitInfo.point.y;
		return result;
	}

	public void SetState(NpcState state)
	{
		if (m_State != state)
		{
			if (m_State != null)
			{
				m_State.Yield(this);
				m_StateStack.Push(m_State);
			}
			m_State = state;
			if (m_State != null)
			{
				m_State.Enter(this);
			}
		}
	}

	public void ContinueState(NpcState state)
	{
		if (m_State != null)
		{
			m_State.Exit(this);
		}
		m_State = state;
		m_State.Contiune(this);
	}

	public void SetStateDirectly(NpcState state)
	{
		if (m_State != state)
		{
			if (m_State != null)
			{
				m_State.Exit(this);
			}
			m_State = state;
			if (m_State != null)
			{
				m_State.Enter(this);
			}
		}
	}

	public void SetNextState()
	{
		if (m_StateStack.Count > 0)
		{
			SetStateDirectly((NpcState)m_StateStack.Pop());
		}
		else
		{
			SetDefaultState();
		}
	}

	public bool ClearStateStack()
	{
		if (m_StateStack.Count > 0)
		{
			m_StateStack.Clear();
			return true;
		}
		return false;
	}

	public void Update(float deltaTime)
	{
		if (m_State != null)
		{
			m_State.Loop(this, deltaTime);
		}
		UpdateModelColorAnimation(deltaTime);
		UpdateAimSign(deltaTime);
		UpdateWarnSign(deltaTime);
		UpdateLogic(deltaTime);
		if (m_State != stDestroyState)
		{
			return;
		}
		if (m_bFrameOrBone)
		{
			if (m_ModelShadow != null && m_Anim != null)
			{
				m_ModelShadow.transform.localPosition = new Vector3(m_Anim.GetCenter().x, 0.1f, m_Anim.GetCenter().z);
			}
		}
		else if (m_ModelShadow != null && m_ModelBoneStep != null)
		{
			m_ModelShadow.transform.position = new Vector3(m_ModelBoneStep.transform.position.x, 0f, m_ModelBoneStep.transform.position.z);
		}
	}

	public bool IsHitHead(GameObject obj)
	{
		if (m_bFrameOrBone)
		{
			return m_ModelHead == obj;
		}
		return obj.name == "Bip01 Head";
	}

	public bool IsHitBody(GameObject obj)
	{
		if (m_bFrameOrBone)
		{
			return m_ModelBody == obj;
		}
		return obj.name == "Bip01 Spine";
	}

	public void BrilliantSnap(Vector3 v3Pos, ACTION_ENUM actionType)
	{
		m_CameraScript.HeadShotCamera();
		stDelayBloodState.Initialize(0.4f, v3Pos, actionType);
		SetState(stDelayBloodState);
	}

	public bool OnHit(Ray ray, RaycastHit hit, float fDamage)
	{
		bool flag = false;
		DeathMode deathMode = DeathMode.None;
		if (m_GameScene != null && m_GameScene.m_CurrWeapon != null)
		{
			iWeaponInfoBase weaponInfoBase = iZombieSniperGameApp.GetInstance().m_GunCenter.GetWeaponInfoBase(m_GameScene.m_CurrWeapon.m_nWeaponID);
			if (weaponInfoBase != null && weaponInfoBase.IsMachineGun())
			{
				deathMode = DeathMode.MachineDead;
			}
		}
		if (IsHitHead(hit.collider.gameObject))
		{
			if (deathMode == DeathMode.None)
			{
				deathMode = DeathMode.HeadBreak;
			}
			if (IsGiantZombie())
			{
				AddHP((0f - fDamage) * 3f, deathMode);
			}
			else
			{
				AddHP((0f - fDamage) * 2f, deathMode);
			}
			flag = true;
		}
		else
		{
			AddHP(0f - fDamage, deathMode);
		}
		if (IsDead())
		{
			if (IsZombie())
			{
				if (IsNormalZombie())
				{
					m_GameState.m_nKillNorEnemyNum++;
				}
				else if (IsGiantZombie())
				{
					m_GameState.m_nKillGiaEnemyNum++;
				}
				m_GameSceneUI.SetZombieKill(m_GameState.m_nKillNorEnemyNum + m_GameState.m_nKillGiaEnemyNum);
				if (flag)
				{
					m_GameState.m_nHeadshotNum++;
					m_GameScene.PlayAudio("VoChar03");
					m_GameSceneUI.AddImageWarn1(ImageMsgEnum1.HEADSHOOT);
				}
			}
			else if (IsInnocents())
			{
				m_GameSceneUI.SetInnocenceDead(++m_GameState.m_nKillInnoNum);
				m_GameSceneUI.AddImageWarn2(ImageMsgEnum2.DONT_KILL_INOO);
				m_GameSceneUI.AddImageWarn3(ImageMsgEnum3.STRIKE, m_GameState.m_nKillInnoNum);
				if (m_GameState.m_nKillInnoNum >= m_GameScene.m_nMaxKillInnoNum)
				{
					m_GameScene.m_bGameOver = true;
					m_GameScene.m_v3LastDeadInno = m_ModelTransForm.position;
				}
				else if (m_GameState.m_nKillInnoNum >= 3 && !m_GameState.m_bUseInnoKiller)
				{
					m_GameState.m_bUseInnoKiller = true;
					if (m_GameState.m_nInnoKiller > 0)
					{
						m_GameState.m_nInnoKiller--;
					}
				}
			}
		}
		else
		{
			iWeaponInfoBase weaponInfoBase2 = m_GameScene.m_GunCenter.GetWeaponInfoBase(m_GameScene.m_CurrWeapon.m_nWeaponID);
			if (IsZombie() && weaponInfoBase2 != null && weaponInfoBase2.IsRifle() && m_fCurHP < weaponInfoBase2.m_fBaseSD)
			{
				m_GameScene.PlayAudio("VoChar02");
				m_GameSceneUI.AddImageWarn1(ImageMsgEnum1.HEADSHOOT_MISSKILL);
			}
		}
		ACTION_ENUM actionType = ACTION_ENUM.NONE;
		if (IsDead())
		{
			actionType = ACTION_ENUM.DEAD;
			if (m_ModelTransForm.position.y > 1f)
			{
				actionType = ACTION_ENUM.DEAD_BOOM;
				Vector3 vector = ray.origin - m_ModelTransForm.position;
				vector.y = 0f;
				if (vector != Vector3.zero)
				{
					m_ModelTransForm.forward = vector;
				}
			}
			else
			{
				Vector3 lhs = new Vector3(ray.direction.x, 0f, ray.direction.z);
				Vector3 rhs = new Vector3(m_ModelTransForm.forward.x, 0f, m_ModelTransForm.forward.z);
				float num = Vector3.Dot(lhs, rhs);
				actionType = ((num >= 0.4f) ? ACTION_ENUM.DEAD_BACK : ((num < 0.4f && num > -0.4f) ? ((!(Vector3.Cross(lhs, rhs).y >= 0f)) ? ACTION_ENUM.DEAD_RIGHT : ACTION_ENUM.DEAD_LEFT) : ((!flag) ? ACTION_ENUM.DEAD : ACTION_ENUM.DEAD_HEAD)));
			}
		}
		if (flag)
		{
			m_GameScene.AddBloodEffect(hit.point + new Vector3(0f, 0.01f, 0f));
			m_GameScene.AddBloodEffect(hit.point + new Vector3(0f, -0.01f, 0f));
			m_Anim.PlayAnim(actionType, -1, 0f);
		}
		else
		{
			m_GameScene.AddBloodEffect(hit.point);
			m_Anim.PlayAnim(actionType, -1, 0f);
		}
		return true;
	}

	public void OnHit(Vector3 v3DamSource, float fDamage, Vector3 v3Pos)
	{
		ACTION_ENUM actionType = ACTION_ENUM.NONE;
		AddHP(0f - fDamage);
		if (IsDead())
		{
			actionType = ACTION_ENUM.DEAD_BOOM;
		}
		if (v3Pos == Vector3.zero)
		{
			v3Pos = OnHitRandom();
		}
		m_GameScene.AddBloodEffect(v3Pos);
		if (IsDead())
		{
			Vector3 vector = v3DamSource - m_ModelTransForm.position;
			vector.y = 0f;
			if (vector != Vector3.zero)
			{
				m_ModelTransForm.forward = vector;
			}
			if (IsZombie())
			{
				if (IsNormalZombie())
				{
					m_GameState.m_nKillNorEnemyNum++;
				}
				else if (IsGiantZombie())
				{
					m_GameState.m_nKillGiaEnemyNum++;
				}
				m_GameSceneUI.SetZombieKill(m_GameState.m_nKillNorEnemyNum + m_GameState.m_nKillGiaEnemyNum);
			}
			else if (IsInnocents())
			{
				m_GameSceneUI.SetInnocenceDead(++m_GameState.m_nKillInnoNum);
				m_GameSceneUI.AddImageWarn2(ImageMsgEnum2.DONT_KILL_INOO);
				m_GameSceneUI.AddImageWarn3(ImageMsgEnum3.STRIKE, m_GameState.m_nKillInnoNum);
				if (m_GameState.m_nKillInnoNum >= m_GameScene.m_nMaxKillInnoNum)
				{
					m_GameScene.m_bGameOver = true;
					m_GameScene.m_v3LastDeadInno = m_ModelTransForm.position;
				}
				else if (m_GameState.m_nKillInnoNum >= 3 && !m_GameState.m_bUseInnoKiller)
				{
					m_GameState.m_bUseInnoKiller = true;
					if (m_GameState.m_nInnoKiller > 0)
					{
						m_GameState.m_nInnoKiller--;
					}
				}
			}
		}
		m_Anim.PlayAnim(actionType, -1, 0f);
	}

	public Vector3 OnHitRandom()
	{
		if (m_bFrameOrBone)
		{
			if (m_MeshFilter != null)
			{
				int num = UnityEngine.Random.Range(0, m_MeshFilter.mesh.vertexCount);
				return m_MeshFilter.transform.position + m_MeshFilter.transform.localToWorldMatrix.MultiplyVector(m_MeshFilter.mesh.vertices[num]);
			}
		}
		else if (m_SkinnedMeshRenderer != null)
		{
			int num2 = UnityEngine.Random.Range(0, m_SkinnedMeshRenderer.sharedMesh.vertexCount);
			return m_SkinnedMeshRenderer.transform.position + m_SkinnedMeshRenderer.transform.localToWorldMatrix.MultiplyVector(m_SkinnedMeshRenderer.sharedMesh.vertices[num2]);
		}
		return m_ModelTransForm.position;
	}

	public void AddHP(float fLife, DeathMode mode = DeathMode.None)
	{
		m_fCurHP += fLife;
		if (m_fCurHP <= 0f)
		{
			m_fCurHP = 0f;
			OnDead(mode);
		}
		else if (m_fCurHP > m_fMaxHP)
		{
			m_fCurHP = m_fMaxHP;
		}
	}

	public virtual void OnDead(DeathMode mode)
	{
		m_bDead = true;
		switch (GetNpcType())
		{
		case NpcType.ZombieDog:
			m_GameScene.PlayAudio("MonDogDeath");
			break;
		case NpcType.ZombieNurse:
			m_GameScene.PlayAudio("MonZombieFemaleDeath");
			break;
		case NpcType.Giant:
			m_GameScene.PlayAudio("MonZombieFatDeath");
			break;
		case NpcType.GiantElite:
			m_GameScene.PlayAudio("MonZombieFatDeath");
			break;
		case NpcType.Innocents:
		case NpcType.InnocentsDoctor:
		case NpcType.InnocentsPastor:
		case NpcType.InnocentsPlumber:
			m_GameScene.PlayAudio("MonHumanDeath");
			break;
		case NpcType.ZombiePredator:
			m_GameScene.PlayAudio("NewZombie01_Death01");
			break;
		default:
			m_GameScene.PlayAudio("MonZombieDeath");
			break;
		}
		SetState(stDestroyState);
		ModelColorAnimationStop();
		SetWarn(false);
		if (m_ModelBoneStep != null)
		{
			m_ModelShadow.transform.parent = null;
			m_ModelShadow.transform.position = m_ModelBoneStep.transform.position;
		}
		switch (mode)
		{
		case DeathMode.HeadBreak:
			if (GetNpcType() == NpcType.Zombie || GetNpcType() == NpcType.ZombieElite)
			{
				m_Model.SetActiveRecursively(false);
				m_bNeedDestroy = true;
				m_GameScene.AddHeadShootDeath(m_ModelTransForm.position, m_ModelTransForm.forward, GetNpcType());
			}
			break;
		case DeathMode.MachineDead:
			m_Model.SetActiveRecursively(false);
			m_bNeedDestroy = true;
			m_GameScene.AddMachineGunDead(m_ModelTransForm.position);
			break;
		}
	}

	public void OnDisappear()
	{
		SetState(stDestroyState);
		ModelColorAnimationStop();
		SetWarn(false);
	}

	public void ModelColorAnimationStart(Vector3 v3Color, float fSpeed, float fTime)
	{
		if (m_Model == null)
		{
			return;
		}
		if (m_bFrameOrBone)
		{
			if (m_MeshRenderer == null)
			{
				return;
			}
			m_v3CurrColor = new Vector3(m_MeshRenderer.material.color.r, m_MeshRenderer.material.color.g, m_MeshRenderer.material.color.b);
		}
		else
		{
			if (m_SkinnedMeshRenderer == null)
			{
				return;
			}
			m_v3CurrColor = new Vector3(m_SkinnedMeshRenderer.material.color.r, m_SkinnedMeshRenderer.material.color.g, m_SkinnedMeshRenderer.material.color.b);
		}
		m_bColorAnim = true;
		m_v3DestColor = v3Color;
		m_fColorAnimSpeed = fSpeed;
		m_fColorFactor = 0f;
		m_fColorDir = 1f;
		m_fColorAnimTime = fTime;
	}

	public void ModelColorAnimationStop()
	{
		if (!m_bColorAnim)
		{
			return;
		}
		m_bColorAnim = false;
		if (m_bFrameOrBone)
		{
			if (m_MeshRenderer != null)
			{
				m_MeshRenderer.material.color = new Color(m_v3CurrColor.x, m_v3CurrColor.y, m_v3CurrColor.z);
			}
		}
		else if (m_SkinnedMeshRenderer != null)
		{
			m_SkinnedMeshRenderer.material.color = new Color(m_v3CurrColor.x, m_v3CurrColor.y, m_v3CurrColor.z);
		}
	}

	public void UpdateModelColorAnimation(float deltaTime)
	{
		if (!m_bColorAnim)
		{
			return;
		}
		if (m_fColorAnimTime > 0f)
		{
			m_fColorAnimTime -= deltaTime;
			if (m_fColorAnimTime <= 0f)
			{
				ModelColorAnimationStop();
			}
		}
		if (m_fRefreshMeshColorCount > m_GameScene.m_fGameTimeTotal)
		{
			return;
		}
		m_fRefreshMeshColorCount = m_GameScene.m_fGameTimeTotal + 0.02f;
		Vector3 vector = Vector3.Lerp(m_v3CurrColor, m_v3DestColor, m_fColorFactor);
		if (m_bFrameOrBone)
		{
			if (m_MeshRenderer != null)
			{
				m_MeshRenderer.material.color = new Color(vector.x, vector.y, vector.z);
			}
		}
		else if (m_SkinnedMeshRenderer != null)
		{
			m_SkinnedMeshRenderer.material.color = new Color(vector.x, vector.y, vector.z);
		}
		m_fColorFactor += 0.1f * m_fColorDir;
		if (m_fColorFactor >= 1f || m_fColorFactor <= 0f)
		{
			m_fColorDir *= -1f;
		}
	}

	public void SetFreeze()
	{
		if (m_State == stHurtState)
		{
			SetStateDirectly(stHurtState);
		}
		else
		{
			SetState(stHurtState);
		}
	}

	public iZombieSniperNpc FindClosestInnocents()
	{
		if (m_GameScene == null || m_GameScene.m_NPCMap == null)
		{
			return null;
		}
		foreach (iZombieSniperNpc value in m_GameScene.m_NPCMap.Values)
		{
			if (!value.IsDead() && value.IsInnocents())
			{
				float sqrMagnitude = (value.m_ModelTransForm.position - m_ModelTransForm.position).sqrMagnitude;
				if (sqrMagnitude <= m_ZombieBaseInfo.m_fWarnRange)
				{
					return value;
				}
			}
		}
		return null;
	}

	public void UpdateMiniMap(float deltaTime)
	{
		if (IsDead() || m_fUpdateMiniMapTime > m_GameScene.m_fGameTimeTotal)
		{
			return;
		}
		m_fUpdateMiniMapTime = m_GameScene.m_fGameTimeTotal + 2f;
		if (!m_GameSceneUI.m_MiniMap.m_bShow)
		{
			return;
		}
		Vector2 v2MiniMapPos = Vector2.zero;
		if (m_GameSceneUI.m_MiniMap.WorldPointToMiniMap(m_ModelTransForm.position, ref v2MiniMapPos))
		{
			if (m_nMiniPointID != -1)
			{
				m_GameSceneUI.m_MiniMap.SetPointPos(m_nMiniPointID, v2MiniMapPos);
			}
			else if (IsInnocents())
			{
				m_nMiniPointID = m_GameSceneUI.m_MiniMap.AddPoint(v2MiniMapPos, new Vector2(10f, 10f), new Color(1f, 1f, 1f), v2MiniMapPos - m_GameState.m_v3ShootCenter);
			}
			else
			{
				m_nMiniPointID = m_GameSceneUI.m_MiniMap.AddPoint(v2MiniMapPos, new Vector2(10f, 10f), new Color(1f, 0f, 0f), v2MiniMapPos - m_GameState.m_v3ShootCenter);
			}
		}
	}

	public void UpdateAimSign(float deltaTime)
	{
		if (IsDead() || !m_Sign.m_bShow || m_GameScene.m_CameraScript.m_CameraState == iZombieSniperCamera.CameraState.kDynmaic)
		{
			return;
		}
		Vector2 vector = m_GameScene.m_CameraScript.m_Camera.WorldToScreenPoint(m_ModelTransForm.position);
		if (vector.x < 0f || vector.x > (float)Screen.width || vector.y < 0f || vector.y > (float)Screen.height)
		{
			if (m_nSignPointID != -1)
			{
				m_Sign.RemovePoint(m_nSignPointID);
				m_nSignPointID = -1;
			}
			return;
		}
		Vector2 v2Dir = vector - m_GameState.m_v3ShootCenter;
		if (v2Dir.sqrMagnitude > 130f * (float)m_GameState.m_nHDFactor * 130f * (float)m_GameState.m_nHDFactor)
		{
			Vector2 v2Pos = m_GameState.m_v3ShootCenter + v2Dir.normalized * 130f * m_GameState.m_nHDFactor;
			if (m_nSignPointID != -1)
			{
				m_Sign.SetPointPos(m_nSignPointID, v2Pos);
			}
			else if (IsInnocents())
			{
				m_nSignPointID = m_Sign.AddPoint(v2Pos, Vector2.zero, new Color(1f, 1f, 1f), v2Dir);
			}
			else
			{
				m_nSignPointID = m_Sign.AddPoint(v2Pos, Vector2.zero, new Color(1f, 1f, 1f), v2Dir);
			}
		}
		else if (m_nSignPointID != -1)
		{
			m_Sign.RemovePoint(m_nSignPointID);
			m_nSignPointID = -1;
		}
	}

	private void UpdateWarnSign(float deltaTime)
	{
		if (IsDead() || !m_bWarn || m_GameSceneUI.m_WarnArrow == null || m_GameSceneUI.m_WarnIcon == null || m_GameScene.m_CameraScript.m_CameraState == iZombieSniperCamera.CameraState.kDynmaic)
		{
			return;
		}
		Vector2 vector = m_GameScene.m_CameraScript.m_Camera.WorldToScreenPoint(m_ModelTransForm.position);
		if (vector.x < -5000f || vector.x > 5000f)
		{
			if (m_nWarnArrowID != -1)
			{
				m_GameSceneUI.m_WarnArrow.RemovePoint(m_nWarnArrowID);
				m_nWarnArrowID = -1;
			}
			if (m_nWarnIconID != -1)
			{
				m_GameSceneUI.m_WarnIcon.RemovePoint(m_nWarnIconID);
				m_nWarnIconID = -1;
			}
			return;
		}
		Vector2 vector2 = vector - m_GameState.m_v3ShootCenter;
		if (m_GameScene.IsAim())
		{
			if (vector2.sqrMagnitude > 130f * (float)m_GameState.m_nHDFactor * 130f * (float)m_GameState.m_nHDFactor)
			{
				Vector2 vector3 = m_GameState.m_v3ShootCenter + vector2.normalized * 130f * m_GameState.m_nHDFactor;
				if (m_nWarnArrowID != -1)
				{
					m_GameSceneUI.m_WarnArrow.SetPointPos(m_nWarnArrowID, vector3);
				}
				else
				{
					m_nWarnArrowID = m_GameSceneUI.m_WarnArrow.AddPoint(vector3, Vector2.zero, new Color(1f, 1f, 0f), vector3 - m_GameState.m_v3ShootCenter);
				}
				if (m_nWarnIconID != -1)
				{
					m_GameSceneUI.m_WarnIcon.SetPointPos(m_nWarnIconID, vector3 - vector2.normalized * 17f * m_GameState.m_nHDFactor, true);
					return;
				}
				m_nWarnIconID = m_GameSceneUI.m_WarnIcon.AddPoint(vector3 - vector2.normalized * 17f * m_GameState.m_nHDFactor, Vector2.zero, new Color(1f, 1f, 0f), Vector2.zero);
				m_GameSceneUI.m_WarnIcon.AddAlphaAnim(m_nWarnIconID, 0.05f, 0.2f, 1f);
			}
			else
			{
				if (m_nWarnArrowID != -1)
				{
					m_GameSceneUI.m_WarnArrow.RemovePoint(m_nWarnArrowID);
					m_nWarnArrowID = -1;
				}
				if (m_nWarnIconID != -1)
				{
					m_GameSceneUI.m_WarnIcon.RemovePoint(m_nWarnIconID);
					m_nWarnIconID = -1;
				}
			}
		}
		else if (vector.x < 0f || vector.x > (float)Screen.width || vector.y < 0f || vector.y > (float)Screen.height)
		{
			float magnitude = vector2.magnitude;
			Vector2 vector4 = vector2 / magnitude;
			float num = Vector2.Dot(vector4, Vector2.right);
			Vector2 vector5;
			if (num > m_GameState.m_fAngleTR || num < m_GameState.m_fAngleTL)
			{
				float num2 = vector2.x / magnitude;
				float f = (float)m_GameState.m_nScreenWidth * 0.5f / num2;
				vector5 = m_GameState.m_v3ShootCenter + vector4 * (Mathf.Abs(f) - 10f);
			}
			else
			{
				float num3 = vector2.y / magnitude;
				float f2 = (float)m_GameState.m_nScreenHeight * 0.5f / num3;
				vector5 = m_GameState.m_v3ShootCenter + vector4 * (Mathf.Abs(f2) - 10f);
			}
			if (m_nWarnArrowID != -1)
			{
				m_GameSceneUI.m_WarnArrow.SetPointPos(m_nWarnArrowID, vector5);
			}
			else
			{
				m_nWarnArrowID = m_GameSceneUI.m_WarnArrow.AddPoint(vector5, Vector2.zero, new Color(1f, 1f, 0f), vector5 - m_GameState.m_v3ShootCenter);
			}
			if (m_nWarnIconID != -1)
			{
				m_GameSceneUI.m_WarnIcon.SetPointPos(m_nWarnIconID, vector5 - vector2.normalized * 17f * m_GameState.m_nHDFactor, true);
				return;
			}
			m_nWarnIconID = m_GameSceneUI.m_WarnIcon.AddPoint(vector5 - vector2.normalized * 17f * m_GameState.m_nHDFactor, Vector2.zero, new Color(1f, 1f, 0f), Vector2.zero);
			m_GameSceneUI.m_WarnIcon.AddAlphaAnim(m_nWarnIconID, 0.05f, 0.2f, 1f);
		}
		else
		{
			if (m_nWarnArrowID != -1)
			{
				m_GameSceneUI.m_WarnArrow.RemovePoint(m_nWarnArrowID);
				m_nWarnArrowID = -1;
			}
			if (m_nWarnIconID != -1)
			{
				m_GameSceneUI.m_WarnIcon.RemovePoint(m_nWarnIconID);
				m_nWarnIconID = -1;
			}
		}
	}

	public bool IsInnocents()
	{
		return GetNpcType() == NpcType.Innocents || GetNpcType() == NpcType.InnocentsDoctor || GetNpcType() == NpcType.InnocentsPastor || GetNpcType() == NpcType.InnocentsPlumber;
	}

	public bool IsZombie()
	{
		return GetNpcType() == NpcType.Zombie || GetNpcType() == NpcType.ZombieElite || GetNpcType() == NpcType.ZombieNurse || GetNpcType() == NpcType.ZombiePolice || GetNpcType() == NpcType.ZombieSwat || GetNpcType() == NpcType.Giant || GetNpcType() == NpcType.ZombieDog || GetNpcType() == NpcType.ZombiePredator;
	}

	public bool IsNormalZombie()
	{
		return GetNpcType() == NpcType.Zombie || GetNpcType() == NpcType.ZombieElite || GetNpcType() == NpcType.ZombieNurse || GetNpcType() == NpcType.ZombiePolice || GetNpcType() == NpcType.ZombieSwat || GetNpcType() == NpcType.ZombieDog || GetNpcType() == NpcType.ZombiePredator;
	}

	public bool IsGiantZombie()
	{
		return GetNpcType() == NpcType.Giant || GetNpcType() == NpcType.GiantElite;
	}

	public bool IsZombieZombieDog()
	{
		return GetNpcType() == NpcType.ZombieDog;
	}

	public bool IsZombiePredator()
	{
		return GetNpcType() == NpcType.ZombiePredator;
	}

	public NpcType GetNpcType()
	{
		return (NpcType)m_ZombieBaseInfo.m_nType;
	}

	public NpcAiType GetNpcAiType()
	{
		return (NpcAiType)m_ZombieBaseInfo.m_nAiType;
	}

	public void OnCollide(iZombieSniperNpc other)
	{
		if (m_State == stMoveState && !other.IsDead())
		{
			Vector3 normalized = (other.m_ModelTransForm.position - m_ModelTransForm.position).normalized;
			float num = Vector3.Dot(m_ModelTransForm.forward, normalized);
			if (num > 0f)
			{
				stMoveState.MoveAvoid(this);
			}
		}
	}

	public void SetWarn(bool bWarn)
	{
		if (m_bWarn == bWarn)
		{
			return;
		}
		m_bWarn = bWarn;
		if (bWarn)
		{
			return;
		}
		if (m_nWarnArrowID != -1)
		{
			if (m_GameSceneUI != null && m_GameSceneUI.m_WarnArrow != null)
			{
				m_GameSceneUI.m_WarnArrow.RemovePoint(m_nWarnArrowID);
			}
			m_nWarnArrowID = -1;
		}
		if (m_nWarnIconID != -1)
		{
			if (m_GameSceneUI != null && m_GameSceneUI.m_WarnIcon != null)
			{
				m_GameSceneUI.m_WarnIcon.RemovePoint(m_nWarnIconID);
			}
			m_nWarnIconID = -1;
		}
	}

	public float PlayAnimDirectly(string sAnim, float speed = 1f, bool bLoop = false)
	{
		if (m_bFrameOrBone)
		{
			return 0f;
		}
		if (m_Model == null || m_Model.GetComponent<Animation>() == null || m_Model.GetComponent<Animation>()[sAnim] == null)
		{
			return 0f;
		}
		m_Model.GetComponent<Animation>()[sAnim].speed = speed;
		m_Model.GetComponent<Animation>()[sAnim].wrapMode = ((!bLoop) ? WrapMode.Once : WrapMode.Loop);
		m_Model.GetComponent<Animation>().CrossFade(sAnim);
		return m_Model.GetComponent<Animation>()[sAnim].length / speed;
	}

	public void PlayAnimDirectlyRandom(string sAnim, float speed = 1f, bool bLoop = false)
	{
		if (!m_bFrameOrBone && !(m_Model == null) && !(m_Model.GetComponent<Animation>() == null) && !(m_Model.GetComponent<Animation>()[sAnim] == null))
		{
			m_Model.GetComponent<Animation>()[sAnim].time = UnityEngine.Random.Range(0f, m_Model.GetComponent<Animation>()[sAnim].length);
			m_Model.GetComponent<Animation>()[sAnim].speed = speed;
			m_Model.GetComponent<Animation>()[sAnim].wrapMode = ((!bLoop) ? WrapMode.Once : WrapMode.Loop);
			m_Model.GetComponent<Animation>().Play(sAnim);
		}
	}

	public bool IsInGameOverRect()
	{
		return m_bIsInGameOverRect;
	}

	public bool IsGotoAttackSand()
	{
		return m_bIsGotoAttackSand;
	}

	public virtual void OnMoveEnd()
	{
	}

	public virtual bool IsSafe()
	{
		return false;
	}
}
