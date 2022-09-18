using UnityEngine;

public class CBillBoard : MonoBehaviour
{
	public bool x = true;

	public bool y = true;

	public bool z = true;

	private Camera m_MainCamera;

	private void Start()
	{
		m_MainCamera = Camera.main;
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		if (x || y || z)
		{
			Vector3 forward = m_MainCamera.transform.position - base.transform.position;
			if (!x)
			{
				forward.x = 0f;
			}
			if (!y)
			{
				forward.y = 0f;
			}
			if (!z)
			{
				forward.z = 0f;
			}
			base.transform.forward = forward;
		}
	}
}
