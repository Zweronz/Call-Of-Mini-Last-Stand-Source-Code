using UnityEngine;

public class iZombieSniperZombieFool : iZombieSniperNpc
{
	public class FreeState : NpcState
	{
		private float m_fThinkTime;

		public override void Enter(iZombieSniperNpc npc)
		{
			m_fThinkTime = 2f;
			npc.m_Anim.PlayAnim(ACTION_ENUM.IDLE, -1, 0f, true);
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			m_fThinkTime -= deltaTime;
			if (m_fThinkTime < 0f)
			{
				npc.stMoveState.Initialize(npc, npc.m_ModelTransForm.position + RandomPosition(), this);
				npc.SetStateDirectly(npc.stMoveState);
			}
		}

		public Vector3 RandomPosition()
		{
			return new Vector3(UnityEngine.Random.Range(-20, 20), 0f, UnityEngine.Random.Range(-20, 20));
		}
	}

	public class AttackBunkerState : NpcState
	{
		private float m_fTimeCount;

		public override void Enter(iZombieSniperNpc npc)
		{
			npc.m_Anim.PlayAnim(ACTION_ENUM.IDLE, -1, 0f, true);
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			iZombieSniperBunker bunkerScript = npc.m_GameScene.GetBunkerScript();
			if (bunkerScript.IsDead())
			{
				npc.SetNextState();
				return;
			}
			m_fTimeCount += deltaTime;
			if (!(m_fTimeCount < npc.m_ZombieBaseInfo.m_fAtkSpeed))
			{
				m_fTimeCount = 0f;
				npc.m_Anim.PlayAnim(ACTION_ENUM.ATTACK, 0, npc.m_ZombieBaseInfo.m_fAtkSpeed);
				bunkerScript.AddHP(0f - npc.m_fDamage);
			}
		}

		public override void Exit(iZombieSniperNpc npc)
		{
			iZombieSniperBunker bunkerScript = npc.m_GameScene.GetBunkerScript();
			if (bunkerScript.IsRaidEnemy(npc.m_nUID))
			{
				bunkerScript.RemoveRaidEnemy(npc.m_nUID);
			}
		}
	}

	public class FoolMoveState : MoveState
	{
		public override void ChangeSpeed(iZombieSniperNpc npc)
		{
			if (m_stMovePurpose == npc.stAttackState || m_stMovePurpose == ((iZombieSniperZombieFool)npc).stAttackBunkerState)
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

		public override bool OnBlockFunc(iZombieSniperNpc npc)
		{
			if (m_stMovePurpose == null)
			{
				return false;
			}
			if (m_stMovePurpose == ((iZombieSniperZombieFool)npc).stFreeState)
			{
				npc.SetStateDirectly(m_stMovePurpose);
				return true;
			}
			if (m_stMovePurpose == ((iZombieSniperZombieFool)npc).stAttackBunkerState)
			{
				iZombieSniperBunker bunkerScript = npc.m_GameScene.GetBunkerScript();
				if (bunkerScript == null)
				{
					return false;
				}
				if ((bunkerScript.GetPostion() - npc.m_ModelTransForm.position).sqrMagnitude > (npc.m_ZombieBaseInfo.m_fSize + bunkerScript.m_fSize) * (npc.m_ZombieBaseInfo.m_fSize + bunkerScript.m_fSize))
				{
					npc.SetStateDirectly(m_stMovePurpose);
					return true;
				}
			}
			return base.OnBlockFunc(npc);
		}
	}

	public class AttackSandbagState : NpcState
	{
		private float m_fTimeCount;

		public override void Enter(iZombieSniperNpc npc)
		{
			npc.m_Anim.PlayAnim(ACTION_ENUM.IDLE, -1, 0f, true);
		}

		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			m_fTimeCount += deltaTime;
			if (!(m_fTimeCount < npc.m_ZombieBaseInfo.m_fAtkSpeed))
			{
				m_fTimeCount = 0f;
				npc.m_Anim.PlayAnim(ACTION_ENUM.ATTACK, 0, npc.m_ZombieBaseInfo.m_fAtkSpeed);
			}
		}

		public override void Exit(iZombieSniperNpc npc)
		{
		}
	}

	public FreeState stFreeState;

	public AttackBunkerState stAttackBunkerState;

	public AttackSandbagState stAttackSandbagState;

	public override void SetDefaultState()
	{
		SetStateDirectly(stFreeState);
	}

	public override bool Create(int nID, int nUID, string sName, Vector3 v3Pos)
	{
		if (!Initialize(nID, nUID, sName, v3Pos))
		{
			return false;
		}
		SetDefaultState();
		stMoveState.Initialize(this, m_ModelTransForm.position);
		SetState(stMoveState);
		return true;
	}

	public override void InitState()
	{
		base.InitState();
		stMoveState = new FoolMoveState();
		stFreeState = new FreeState();
		stAttackBunkerState = new AttackBunkerState();
		stAttackSandbagState = new AttackSandbagState();
	}

	public override void EnterGameOverEvent()
	{
		if (IsDead())
		{
			return;
		}
		switch (m_GameState.m_nCurStage)
		{
		case 0:
		case 1:
			OnDisappear();
			m_GameScene.m_bGameOver = true;
			m_GameScene.m_v3LastDeadInno = Vector3.zero;
			break;
		case 2:
			m_bIsInGameOverRect = true;
			if (!m_bIsGotoAttackSand)
			{
				m_bIsGotoAttackSand = true;
				SetStateDirectly(stAttackSandbagState);
			}
			break;
		}
	}

	public override void UpdateLogic(float deltaTime)
	{
		base.UpdateLogic(deltaTime);
		iZombieSniperBunker bunkerScript = m_GameScene.GetBunkerScript();
		if (!IsDead() && bunkerScript != null && bunkerScript.IsActive() && !bunkerScript.IsDead() && !bunkerScript.IsFull() && !bunkerScript.IsRaidEnemy(m_nUID))
		{
			float sqrMagnitude = (m_ModelTransForm.position - bunkerScript.GetPostion()).sqrMagnitude;
			if (sqrMagnitude <= bunkerScript.m_fAttractRange * bunkerScript.m_fAttractRange)
			{
				bunkerScript.AddRaidEnemy(m_nUID);
				stMoveState.Initialize(this, bunkerScript.GetPostion(), stAttackBunkerState);
				SetStateDirectly(stMoveState);
			}
		}
		if (IsDead())
		{
			return;
		}
		float sqrMagnitude2 = (m_ModelTransForm.position - m_GameScene.m_v3EndPoint).sqrMagnitude;
		if (sqrMagnitude2 <= m_GameScene.m_fWarningDis * m_GameScene.m_fWarningDis)
		{
			if (!base.bColorAnim)
			{
				ModelColorAnimationStart(new Vector3(0.8f, 0f, 0f), 0.15f, 0f);
			}
			m_bAlarmTip = true;
		}
		else
		{
			if (base.bColorAnim)
			{
				ModelColorAnimationStop();
			}
			m_bAlarmTip = false;
		}
	}
}
