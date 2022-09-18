using UnityEngine;

public class iZombieSniperThrowMine : MonoBehaviour
{
	public struct ThrowMineInfo
	{
		public Rect m_Rect;

		public float m_fDamage;

		public float m_fDamageRadius;
	}

	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameState m_GameState;

	public bool m_bActive;

	public bool m_bGround;

	public ThrowMineInfo m_Info;

	public Transform m_Transform;

	private BoxCollider m_BoxCollider;

	private Rigidbody m_Rigidbody;

	private float m_fBoomTime;

	private float m_fSpeed;

	private Vector3 m_v3Dir;

	private GameObject m_Effect;

	private void Awake()
	{
		m_Transform = base.transform;
		m_bActive = false;
		m_bGround = false;
		Transform transform = base.transform.Find("Landmine_01");
		if (transform != null)
		{
			m_Effect = transform.gameObject;
			m_Effect.SetActiveRecursively(false);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!m_bActive)
		{
			return;
		}
		if (m_fBoomTime > 0f)
		{
			m_fBoomTime -= Time.deltaTime;
			if (m_fBoomTime <= 0f)
			{
				m_fBoomTime = 0f;
				m_bActive = false;
				m_GameScene.Boom(m_Transform.position, 10f, 10f, true);
				m_GameScene.PlayAudio("FxExploEnergyLarge01");
				GameObject obj = (GameObject)Object.Instantiate(m_GameScene.m_PerfabManager.m_Boom02, m_Transform.position + new Vector3(0f, 0.2f, 0f), Quaternion.identity);
				Object.Destroy(obj, 5f);
				m_GameScene.AddCinderEffect(base.transform.position);
				m_GameScene.RemoveThrowMineRect(this);
				Object.Destroy(base.gameObject);
			}
		}
		if (!m_bGround && m_bActive && m_Transform.position.y <= 0.5f)
		{
			m_bGround = true;
			m_Info.m_Rect = new Rect(m_Transform.position.x - m_Info.m_Rect.width / 2f, m_Transform.position.z - m_Info.m_Rect.height / 2f, m_Info.m_Rect.width, m_Info.m_Rect.height);
			m_GameScene.PlayAudio("RocketBombFallDown");
			m_Transform.position = new Vector3(m_Transform.position.x, 0f, m_Transform.position.z);
			m_Transform.eulerAngles = new Vector3(0f, m_Transform.eulerAngles.y, 0f);
			m_Transform.localEulerAngles = new Vector3(270f, 0f, 0f);
			m_BoxCollider.enabled = false;
			m_GameScene.AddThrowMineRect(this);
			if (m_Rigidbody != null)
			{
				m_Rigidbody.Sleep();
			}
			if (m_Effect != null)
			{
				m_Effect.SetActiveRecursively(true);
			}
		}
	}

	private void FixedUpdate()
	{
		if (!m_bGround && !m_bActive)
		{
		}
	}

	private void OnTriggerEnter(Collider other)
	{
	}

	public void Initialize(Vector3 v3Dir, float speed, float width, float height)
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_BoxCollider = base.transform.GetComponent<BoxCollider>();
		m_Rigidbody = base.transform.GetComponent<Rigidbody>();
		v3Dir.Normalize();
		m_Transform.forward = v3Dir;
		m_v3Dir = v3Dir;
		m_fSpeed = speed;
		m_fBoomTime = UnityEngine.Random.Range(9f, 12f);
		m_Info = default(ThrowMineInfo);
		m_Info.m_Rect = new Rect(0f, 0f, width, height);
		m_Info.m_fDamage = 20f;
		m_Info.m_fDamageRadius = 5f;
		if (m_Rigidbody != null)
		{
			m_Rigidbody.AddForce(m_v3Dir * speed);
			m_Rigidbody.AddTorque(base.transform.right * 10f);
			m_Rigidbody.AddTorque(base.transform.forward * UnityEngine.Random.Range(-10f, 10f));
			m_Rigidbody.AddTorque(base.transform.up * UnityEngine.Random.Range(-10f, 10f));
		}
		m_bActive = true;
		m_bGround = false;
	}
}
