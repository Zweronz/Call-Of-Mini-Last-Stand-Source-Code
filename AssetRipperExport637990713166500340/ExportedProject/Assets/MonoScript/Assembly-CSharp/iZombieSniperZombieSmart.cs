using UnityEngine;

public class iZombieSniperZombieSmart : iZombieSniperZombieFool
{
	public float m_fWillEatCount;

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

	public override void UpdateLogic(float deltaTime)
	{
		base.UpdateLogic(deltaTime);
		if (!IsWillEat() || !(m_fWillEatCount <= m_GameScene.m_fGameTimeTotal))
		{
			return;
		}
		m_fWillEatCount = m_GameScene.m_fGameTimeTotal + 5f;
		if (UnityEngine.Random.Range(1, 10000) < 5000)
		{
			iZombieSniperNpc iZombieSniperNpc2 = FindClosestInnocents();
			if (iZombieSniperNpc2 != null)
			{
				EatPerson(iZombieSniperNpc2);
			}
		}
	}

	public override bool IsWillEat()
	{
		if (m_State == stMoveState && (stMoveState.m_stMovePurpose == stAttackState || stMoveState.m_stMovePurpose == stAttackBunkerState))
		{
			return false;
		}
		return base.IsWillEat();
	}

	public virtual void EatPerson(iZombieSniperNpc target)
	{
		stAttackState.SetTarget(target.m_nUID);
		SetState(stAttackState);
	}
}
