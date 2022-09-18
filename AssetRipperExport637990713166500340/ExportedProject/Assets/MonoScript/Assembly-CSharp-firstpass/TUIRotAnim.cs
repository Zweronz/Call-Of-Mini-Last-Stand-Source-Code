using UnityEngine;

public class TUIRotAnim : MonoBehaviour
{
	public float m_fSpeed = 1f;

	private Transform m_Transform;

	private void Awake()
	{
		m_Transform = base.transform;
	}

	private void Update()
	{
		m_Transform.RotateAroundLocal(Vector3.forward, m_fSpeed);
	}
}
