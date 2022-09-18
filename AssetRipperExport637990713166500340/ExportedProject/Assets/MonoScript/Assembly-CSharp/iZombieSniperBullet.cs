using UnityEngine;

public class iZombieSniperBullet : MonoBehaviour
{
	private float m_fSpeed;

	private float m_fTime;

	private void Start()
	{
		m_fTime = 0f;
	}

	private void Update()
	{
		if (!(m_fTime <= 0f))
		{
			m_fTime -= Time.deltaTime;
			base.transform.position += base.transform.forward * m_fSpeed * Time.deltaTime;
		}
	}

	public void Initialize(Vector3 scr, Vector3 dir, float fTime)
	{
		m_fSpeed = 200f;
		base.transform.forward = dir;
		base.transform.position = scr;
		m_fTime = fTime;
	}
}
