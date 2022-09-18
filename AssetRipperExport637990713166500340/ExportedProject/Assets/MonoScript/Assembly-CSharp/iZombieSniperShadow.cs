using UnityEngine;

public class iZombieSniperShadow : iZombieSniperTexture2D
{
	public GameObject m_SunLightPos;

	public float m_fScale = 1f;

	public Transform m_Transform;

	public new void Start()
	{
		base.Start();
		Vector3[] array = new Vector3[4];
		m_Transform = base.transform;
		array[0] = -m_Transform.right * m_v2Size.x + m_Transform.forward * m_v2Size.y;
		array[1] = -m_Transform.right * m_v2Size.x - m_Transform.forward * 0.1f;
		array[2] = m_Transform.right * m_v2Size.x - m_Transform.forward * 0.1f;
		array[3] = m_Transform.right * m_v2Size.x + m_Transform.forward * m_v2Size.y;
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 1f),
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f)
		};
		m_MeshFilter.mesh.vertices = array;
		m_MeshFilter.mesh.uv = uv;
		m_SunLightPos = GameObject.Find("Sun");
	}

	public new void Update()
	{
		base.Update();
		Vector3 position = m_Transform.position;
		Vector3 forward = position - m_SunLightPos.transform.position;
		float magnitude = forward.magnitude;
		forward.y = 0f;
		base.transform.forward = forward;
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		position.x = m_Transform.position.x;
		position.z = m_Transform.position.z;
		position.y = 0.1f;
		m_Transform.position = position;
	}

	public void Initialize(float scale)
	{
		m_fScale = scale;
	}
}
