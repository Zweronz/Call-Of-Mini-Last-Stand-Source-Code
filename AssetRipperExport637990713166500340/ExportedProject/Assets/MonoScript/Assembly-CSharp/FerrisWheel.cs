using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
	public Vector2 randomtime;

	public string sAnim = string.Empty;

	private Transform m_Transform;

	private float m_fCurTime;

	private void Start()
	{
		m_Transform = base.transform;
		m_fCurTime = UnityEngine.Random.Range(randomtime.x, randomtime.y);
		if (!(base.GetComponent<Animation>() == null) && !(base.GetComponent<Animation>()[sAnim] == null))
		{
			base.GetComponent<Animation>()[sAnim].time = 0f;
			base.GetComponent<Animation>().Play(sAnim);
		}
	}

	private void Update()
	{
		if (!(base.GetComponent<Animation>() == null) && !(base.GetComponent<Animation>()[sAnim] == null))
		{
			m_fCurTime -= Time.deltaTime;
			if (!(m_fCurTime > 0f))
			{
				m_fCurTime = UnityEngine.Random.Range(randomtime.x, randomtime.y);
				base.GetComponent<Animation>()[sAnim].time = 0f;
				base.GetComponent<Animation>().Play(sAnim);
			}
		}
	}
}
