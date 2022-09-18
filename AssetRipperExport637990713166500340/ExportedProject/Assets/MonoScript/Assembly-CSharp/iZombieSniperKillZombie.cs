using UnityEngine;

public class iZombieSniperKillZombie : MonoBehaviour
{
	public float m_fDamage = 100f;

	private Transform m_Transform;

	private void Start()
	{
		m_Transform = base.transform;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (!(collider.transform.root.tag == "NPC"))
		{
			return;
		}
		iZombieSniperGameSceneBase gameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		if (gameScene == null)
		{
			return;
		}
		ModelInfo component = collider.transform.root.GetComponent<ModelInfo>();
		if (component == null)
		{
			return;
		}
		iZombieSniperNpc nPC = gameScene.GetNPC(component.m_nUID);
		if (nPC != null && !nPC.IsDead())
		{
			if (nPC.IsGiantZombie())
			{
				Vector3 vector = nPC.m_ModelTransForm.position - m_Transform.position;
				vector.y = 0f;
				nPC.m_ModelTransForm.position -= vector.normalized * 0.2f;
			}
			else if (nPC.IsInnocents())
			{
				Vector3 forward = m_Transform.position - nPC.m_ModelTransForm.position;
				forward.y = 0f;
				nPC.m_ModelTransForm.forward = forward;
				nPC.m_ModelTransForm.position -= forward.normalized * 0.2f;
			}
			else
			{
				nPC.OnHit(m_Transform.position, m_fDamage, Vector3.zero);
				gameScene.PlayAudio("FxBodyHit02");
			}
		}
	}
}
