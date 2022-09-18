using UnityEngine;

public class iZombieSniperInnocence : iZombieSniperNpc
{
	public class ClimbCrossState : NpcState
	{
		protected float m_fCount;

		protected Vector3 m_v3Dir;

		protected float m_fSpeed;

		protected float m_fDis;

		public override void Enter(iZombieSniperNpc npc)
		{
			m_fCount = npc.PlayAnimDirectly("Jump01", 1f);
			m_fSpeed = m_fDis / m_fCount;
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			m_fCount -= deltaTime;
			if (m_fCount > 0f)
			{
				npc.m_ModelTransForm.position += m_v3Dir * m_fSpeed * deltaTime;
				return;
			}
			npc.m_Anim.PlayAnim(ACTION_ENUM.IDLE, -1, 0f);
			npc.SetNextState();
		}

		public void Initialize(Vector3 v3Dir, float dis)
		{
			m_v3Dir = v3Dir.normalized;
			m_fDis = dis;
		}
	}

	public ClimbCrossState stClimbCrossState;

	private GameObject m_Help;

	private bool m_bHelp;

	private float m_fHelpTimeCount;

	private float m_fHelpTimeRemain;

	private float m_fNextHelpTime;

	private bool m_bInSafe;

	public override void InitState()
	{
		base.InitState();
		stClimbCrossState = new ClimbCrossState();
	}

	public override void SetDefaultState()
	{
		stMoveState.Initialize(this, m_ModelTransForm.position);
		SetStateDirectly(stMoveState);
	}

	public override bool Create(int nID, int nUID, string sName, Vector3 v3Pos)
	{
		if (!Initialize(nID, nUID, sName, v3Pos))
		{
			return false;
		}
		SetDefaultState();
		m_bHelp = false;
		m_fHelpTimeCount = 0f;
		m_fHelpTimeRemain = 2f;
		m_fNextHelpTime = UnityEngine.Random.Range(3f, 7f);
		m_bInSafe = false;
		return true;
	}

	public override void Destroy()
	{
		if (m_Help != null)
		{
			Object.Destroy(m_Help);
			m_Help = null;
		}
		base.Destroy();
	}

	public override void EnterGameOverEvent()
	{
		if (!IsDead() && !m_bInSafe)
		{
			m_GameSceneUI.SetInnocenceSafe(++m_GameState.m_nSaveInnoNum);
			switch (m_GameState.m_nCurStage)
			{
			case 0:
			case 1:
				m_bInSafe = true;
				OnDisappear();
				break;
			case 2:
			{
				m_bInSafe = true;
				stMoveState.InitializeFunc(this, m_GameScene.m_v3EndPoint, base.OnDisappear);
				SetStateDirectly(stMoveState);
				Vector3 vector = m_GameScene.m_v3EndPoint - m_ModelTransForm.position;
				vector.y = 0f;
				stClimbCrossState.Initialize(vector.normalized, 1.7f);
				SetState(stClimbCrossState);
				break;
			}
			}
		}
	}

	public override void UpdateLogic(float deltaTime)
	{
		base.UpdateLogic(deltaTime);
	}

	public override void OnDead(DeathMode mode)
	{
		base.OnDead(mode);
		if (m_Help != null)
		{
			Object.Destroy(m_Help);
			m_Help = null;
		}
		m_bHelp = false;
		m_fHelpTimeCount = 0f;
		m_fHelpTimeRemain = 2f;
		m_fNextHelpTime = 0f;
	}

	public void SetHelp(bool bShow)
	{
		m_bHelp = bShow;
		if (bShow)
		{
			m_Help = (GameObject)Object.Instantiate(m_GameScene.m_PerfabManager.HelpAnim, Vector3.zero, Quaternion.identity);
			if (m_Help != null)
			{
				m_Help.transform.parent = m_ModelTransForm;
				m_Help.transform.localPosition = Vector3.zero;
				m_Help.transform.eulerAngles = new Vector3(270f, 0f, 0f);
				m_Help.GetComponent<Animation>()["help"].wrapMode = WrapMode.Loop;
				m_Help.GetComponent<Animation>().Play("help");
			}
		}
		else if (m_Help != null)
		{
			Object.Destroy(m_Help);
			m_Help = null;
		}
	}

	public override bool IsSafe()
	{
		return m_bInSafe;
	}
}
