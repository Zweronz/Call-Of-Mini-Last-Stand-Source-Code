using UnityEngine;

public class iZombieSniperZombiePredator : iZombieSniperZombieSmart
{
	public class JumpState : NpcState
	{
		public enum JumpStateEnum
		{
			None = 0,
			JumpBegin = 1,
			JumpRaise = 2,
			JumpFall = 3,
			JumpEnd = 4
		}

		protected JumpStateEnum m_JumpState;

		protected float m_fJumpCount;

		protected Vector3 m_v3Scr;

		protected Vector3 m_v3Dst;

		protected float m_fJumpDis;

		protected float m_fJumpHeight;

		protected float m_fJumpSpeedZ;

		protected float m_fJumpSpeedY;

		protected float m_fJumpSpeedYInt;

		protected float m_fJumpSpeedYAcc;

		protected float m_fJumpTime;

		protected float m_fCurJumpDis;

		protected Vector3 m_v3Dir;

		public override void Enter(iZombieSniperNpc npc)
		{
			m_v3Dir = m_v3Dst - npc.m_ModelTransForm.position;
			m_v3Dir.y = 0f;
			npc.m_ModelTransForm.forward = m_v3Dir;
			m_fJumpDis = m_v3Dir.magnitude;
			m_v3Dir /= m_fJumpDis;
			m_fJumpHeight = m_fJumpDis / 4f;
			m_fJumpTime = m_fJumpDis / m_fJumpSpeedZ;
			m_fJumpSpeedYAcc = (0f - m_fJumpHeight) * 2f / (m_fJumpTime * m_fJumpTime * 0.25f);
			float num = m_fJumpTime;
			if (npc.m_Model != null && npc.m_Model.GetComponent<Animation>() != null && npc.m_Model.GetComponent<Animation>()["Run02"] != null)
			{
				num = npc.m_Model.GetComponent<Animation>()["Run02"].length;
			}
			npc.PlayAnimDirectly("Run02", num / m_fJumpTime, true);
			m_JumpState = JumpStateEnum.JumpRaise;
			m_fCurJumpDis = 0f;
			m_fJumpCount = 0f;
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			switch (m_JumpState)
			{
			case JumpStateEnum.JumpBegin:
				m_fJumpCount -= deltaTime;
				if (!(m_fJumpCount > 0f))
				{
					m_fJumpCount = 0f;
					m_JumpState = JumpStateEnum.JumpRaise;
				}
				break;
			case JumpStateEnum.JumpRaise:
			{
				m_fJumpCount += deltaTime;
				float num2 = m_fJumpSpeedZ * deltaTime;
				npc.m_ModelTransForm.position += m_v3Dir * num2;
				m_fCurJumpDis += num2;
				if (m_fJumpSpeedYInt + m_fJumpSpeedYAcc * m_fJumpCount < 0f)
				{
					float y2 = m_fJumpSpeedYInt * m_fJumpCount + 0.5f * m_fJumpSpeedYAcc * m_fJumpCount * m_fJumpCount;
					npc.m_ModelTransForm.position = new Vector3(npc.m_ModelTransForm.position.x, y2, npc.m_ModelTransForm.position.z);
				}
				else
				{
					m_JumpState = JumpStateEnum.JumpFall;
					m_fJumpSpeedY = 0f;
				}
				break;
			}
			case JumpStateEnum.JumpFall:
			{
				m_fJumpCount += deltaTime;
				float num = m_fJumpSpeedZ * deltaTime;
				npc.m_ModelTransForm.position += m_v3Dir * num;
				m_fCurJumpDis += num;
				if (m_fCurJumpDis >= m_fJumpDis || npc.m_ModelTransForm.position.y < 0f)
				{
					npc.m_ModelTransForm.position = new Vector3(npc.m_ModelTransForm.position.x, 0f, npc.m_ModelTransForm.position.z);
					npc.PlayAnimDirectly("Idle01", 1f, true);
					m_fJumpCount = 0.5f;
					m_JumpState = JumpStateEnum.JumpEnd;
				}
				else
				{
					float y = m_fJumpSpeedYInt * m_fJumpCount + 0.5f * m_fJumpSpeedYAcc * m_fJumpCount * m_fJumpCount;
					npc.m_ModelTransForm.position = new Vector3(npc.m_ModelTransForm.position.x, y, npc.m_ModelTransForm.position.z);
				}
				break;
			}
			case JumpStateEnum.JumpEnd:
				m_fJumpCount -= deltaTime;
				if (!(m_fJumpCount > 0f))
				{
					m_fJumpCount = 0f;
					m_JumpState = JumpStateEnum.None;
					npc.SetNextState();
				}
				break;
			}
		}

		public void Initialize(Vector3 v3Dst, float fJumpSpeedZ, float fJumpSpeedYInt)
		{
			m_v3Dst = v3Dst;
			m_fJumpSpeedZ = fJumpSpeedZ;
			m_fJumpSpeedYInt = fJumpSpeedYInt;
		}
	}

	public class FallState : NpcState
	{
		protected float m_fJumpSpeedYInt;

		protected float m_fJumpSpeedYAcc = -20f;

		public override void Enter(iZombieSniperNpc npc)
		{
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			npc.m_ModelTransForm.position += new Vector3(0f, m_fJumpSpeedYInt * deltaTime, 0f);
			m_fJumpSpeedYInt += m_fJumpSpeedYAcc * deltaTime;
			if (npc.m_ModelTransForm.position.y <= 0f)
			{
				npc.m_ModelTransForm.position = new Vector3(npc.m_ModelTransForm.position.x, 0f, npc.m_ModelTransForm.position.z);
				npc.SetNextState();
			}
		}

		public void Initialize(float fJumpSpeedYInt, float fJumpSpeedYAcc)
		{
			m_fJumpSpeedYInt = fJumpSpeedYInt;
			m_fJumpSpeedYAcc = fJumpSpeedYAcc;
		}
	}

	public JumpState stJumpState;

	public FallState stFallState;

	public float m_fJumpThinkTime = 1f;

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
		return true;
	}

	public override void InitState()
	{
		base.InitState();
		stJumpState = new JumpState();
		stFallState = new FallState();
	}

	public override void UpdateLogic(float deltaTime)
	{
		base.UpdateLogic(deltaTime);
		if (IsDead() || m_State == stJumpState)
		{
			return;
		}
		m_fJumpThinkTime -= deltaTime;
		if (m_fJumpThinkTime > 0f)
		{
			return;
		}
		m_fJumpThinkTime = 1f;
		if (m_GameScene == null)
		{
			return;
		}
		iZombieSniperWayPointCenter wayPointCenter = m_GameScene.m_WayPointCenter;
		if (wayPointCenter != null && wayPointCenter.IsInStartJumpZone(m_ModelTransForm.position))
		{
			Vector3 v3Pos = Vector3.zero;
			if (wayPointCenter.GetJumpZoneRandomPoint(UnityEngine.Random.Range(0, 3), ref v3Pos))
			{
				stJumpState.Initialize(v3Pos, 25f, 25f);
				SetStateDirectly(stJumpState);
			}
		}
	}
}
