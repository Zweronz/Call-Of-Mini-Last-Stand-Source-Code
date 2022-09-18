using UnityEngine;

public class RunForward : CustomBase
{
	public float m_fSpeed = 1f;

	protected Transform m_Transform;

	protected void Start()
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

	protected void Update()
	{
		m_Transform.position += m_Scale * m_Transform.forward * m_fSpeed * Time.deltaTime;
	}
}
