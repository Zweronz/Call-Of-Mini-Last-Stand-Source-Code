using UnityEngine;

public class iZombieSniperTruck : MonoBehaviour
{
	public iZombieSniperGameSceneBase m_GameScene;

	private float m_fLife;

	private bool m_bDead;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Initialize(float fLife)
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_fLife = fLife;
		m_bDead = false;
	}

	public void AddHP(float fHP)
	{
		m_fLife += fHP;
		if (m_fLife <= 0f)
		{
			m_fLife = 0f;
			OnDead();
		}
	}

	public void OnDead()
	{
		m_bDead = true;
		m_GameScene.TruckBoom();
		m_GameScene.m_CameraScript.Shake(1.5f, 3f, false, true);
		m_GameScene.PlayAudio("FxExploMed01");
	}

	public bool IsDead()
	{
		return m_bDead;
	}
}
