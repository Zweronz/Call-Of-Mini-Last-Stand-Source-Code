using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
	public float m_CGTime = 5f;

	private void Update()
	{
		m_CGTime -= Time.deltaTime;
		if (m_CGTime <= 0f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
