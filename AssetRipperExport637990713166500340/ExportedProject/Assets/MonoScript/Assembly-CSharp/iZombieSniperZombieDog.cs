using UnityEngine;

public class iZombieSniperZombieDog : iZombieSniperZombieSmart
{
	public class DogYipState : NpcState
	{
		public override void Loop(iZombieSniperNpc npc, float deltaTime)
		{
			npc.m_GameScene.PlayAudio("MonDogClamour");
			npc.m_GameScene.SoundStrike(npc.m_ModelTransForm.position, 10f, npc.m_nUID);
			npc.SetNextState();
		}
	}

	public DogYipState stZombieDogYipState = new DogYipState();

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

	public override void EatPerson(iZombieSniperNpc target)
	{
		base.EatPerson(target);
		SetState(stZombieDogYipState);
	}
}
