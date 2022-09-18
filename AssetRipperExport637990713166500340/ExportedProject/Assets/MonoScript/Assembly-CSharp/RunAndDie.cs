using UnityEngine;

public class RunAndDie : RunForward
{
	public float m_fDieTime = 1f;

	private new void Start()
	{
		m_Transform = base.transform;
		if (base.GetComponent<Animation>()["Run01"] != null)
		{
			base.GetComponent<Animation>()["Run01"].time = UnityEngine.Random.Range(0f, base.GetComponent<Animation>()["Run01"].length);
			base.GetComponent<Animation>()["Run01"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>()["Run01"].speed = m_Scale * m_fSpeed / 1f * base.GetComponent<Animation>()["Run01"].length;
			base.GetComponent<Animation>().CrossFade("Run01");
		}
	}

	private new void Update()
	{
		if (m_fDieTime == 0f)
		{
			return;
		}
		m_fDieTime -= Time.deltaTime;
		if (m_fDieTime <= 0f)
		{
			m_fDieTime = 0f;
			if (base.GetComponent<Animation>()["Death07"] != null)
			{
				base.GetComponent<Animation>()["Death07"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Death07");
			}
			if (iZombieSniperGameApp.GetInstance().m_GameScene != null)
			{
				iZombieSniperGameApp.GetInstance().m_GameScene.AddBloodEffect(m_Transform.position + new Vector3(-0.1f, 0.7f, 0.1f));
				iZombieSniperGameApp.GetInstance().m_GameScene.AddBloodEffect(m_Transform.position + new Vector3(-0.1f, 0.8f, 0.1f));
				iZombieSniperGameApp.GetInstance().PlayAudio("AWPFire01");
				iZombieSniperGameApp.GetInstance().PlayAudio("MonZombieDeath");
			}
		}
		else
		{
			m_Transform.position += m_Scale * m_Transform.forward * m_fSpeed * Time.deltaTime;
		}
	}
}
