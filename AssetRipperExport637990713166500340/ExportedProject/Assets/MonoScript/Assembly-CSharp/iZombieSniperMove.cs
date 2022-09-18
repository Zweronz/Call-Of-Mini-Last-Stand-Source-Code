using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class iZombieSniperMove : MonoBehaviour
{
	public enum MoveType
	{
		Stand = 0,
		Move = 1,
		Rotate = 2
	}

	public class MoveBase
	{
		public MoveType m_State;
	}

	public class MoveStand : MoveBase
	{
		public float m_fTime;
	}

	public class MoveGo : MoveBase
	{
		public Vector3 m_v3Pos;

		public Vector3 m_v3Dir;

		public float m_fSpeed;
	}

	public class MoveRotate : MoveBase
	{
		public Vector3 m_v3Dir;

		public float m_fSpeed;
	}

	public bool m_bLockRotate;

	public WrapMode m_WrapMode;

	public bool m_bInEditMode;

	private GameObject m_Path;

	private static int m_nPointID;

	private Transform m_Transform;

	private List<MoveBase> m_ltMove;

	private int m_nCurMoveIndex;

	private bool m_bFinished;

	private MoveType m_curState;

	private float m_fTimeCount;

	private Vector3 m_v3DestPos;

	private Vector3 m_v3DestDir;

	private float m_fSpeed;

	private float m_fRotateSpeed;

	private Vector3 m_v3ScrDir;

	private float m_fDirRate;

	private bool m_bRotateBody;

	private void Start()
	{
		m_bInEditMode = false;
		m_nPointID = 0;
		m_Transform = base.transform;
		m_Path = GameObject.Find(base.transform.name + "Path");
		if (m_Path == null)
		{
			m_Path = new GameObject(base.transform.name + "Path");
			m_Path.transform.localPosition = Vector3.zero;
			m_Path.transform.localEulerAngles = Vector3.zero;
			Debug.Log(m_Path);
		}
		Initialize();
	}

	private void Update()
	{
		if (!m_bFinished)
		{
			UpdateLogic(Time.deltaTime);
		}
	}

	private void OnDrawGizmos()
	{
		if (!m_bInEditMode || m_Path == null)
		{
			return;
		}
		foreach (Transform item in m_Path.transform)
		{
			Gizmos.color = new Color(1f, 1f, 0f);
			Gizmos.DrawSphere(item.position, 1f);
		}
	}

	public void AddPoint()
	{
		if (!(m_Path == null))
		{
			GameObject gameObject = new GameObject("Point" + ++m_nPointID);
			gameObject.transform.parent = m_Path.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
		}
	}

	public void Initialize()
	{
		if (m_ltMove == null)
		{
			m_ltMove = new List<MoveBase>();
		}
		m_ltMove.Clear();
		if (m_Path != null)
		{
			Transform transform = null;
			Transform transform2 = null;
			int num = 0;
			while (true)
			{
				transform2 = transform;
				transform = m_Path.transform.Find("Point" + ++num);
				if (transform == null)
				{
					break;
				}
				MoveGo moveGo = new MoveGo();
				moveGo.m_State = MoveType.Move;
				moveGo.m_v3Pos = transform.position;
				if (transform2 != null)
				{
					moveGo.m_v3Dir = transform.position - transform2.position;
				}
				else
				{
					moveGo.m_v3Dir = transform.position - m_Transform.position;
				}
				moveGo.m_fSpeed = 3f;
				m_ltMove.Add(moveGo);
			}
		}
		m_fTimeCount = 0f;
		m_v3DestPos = Vector3.zero;
		m_v3DestDir = Vector3.zero;
		m_fSpeed = 0f;
		m_nCurMoveIndex = 0;
		m_bFinished = false;
		GetNextStep();
	}

	public void Destroy()
	{
		if (m_ltMove != null)
		{
			m_ltMove.Clear();
			m_ltMove = null;
		}
	}

	public void UpdateLogic(float deltaTime)
	{
		if (m_bFinished)
		{
			return;
		}
		switch (m_curState)
		{
		case MoveType.Stand:
			m_fTimeCount -= deltaTime;
			if (m_fTimeCount <= 0f && !GetNextStep())
			{
				m_bFinished = true;
			}
			break;
		case MoveType.Move:
		{
			if (m_fDirRate < 1f)
			{
				m_fDirRate += m_fRotateSpeed * deltaTime;
				if (!m_bLockRotate)
				{
					m_Transform.forward = Vector3.Lerp(m_v3ScrDir, m_v3DestDir, m_fDirRate);
				}
				if (m_fDirRate >= 1f)
				{
					m_fDirRate = 1f;
				}
			}
			Vector3 vector = m_v3DestPos - m_Transform.position;
			float magnitude = vector.magnitude;
			Vector3 vector2 = vector / magnitude;
			float num = m_fSpeed * deltaTime;
			if (num > magnitude)
			{
				m_Transform.position = m_v3DestPos;
				if (!GetNextStep())
				{
					m_bFinished = true;
				}
			}
			else
			{
				m_Transform.position += num * vector2;
			}
			break;
		}
		case MoveType.Rotate:
			m_fDirRate += m_fRotateSpeed * deltaTime;
			if (!m_bLockRotate)
			{
				m_Transform.forward = Vector3.Lerp(m_v3ScrDir, m_v3DestDir, m_fDirRate);
			}
			if (m_fDirRate >= 1f && !GetNextStep())
			{
				m_bFinished = true;
			}
			break;
		}
	}

	public bool GetNextStep()
	{
		if (m_ltMove.Count < 1)
		{
			return false;
		}
		if (m_nCurMoveIndex < 0 || m_nCurMoveIndex >= m_ltMove.Count)
		{
			return false;
		}
		if (m_nCurMoveIndex >= m_ltMove.Count)
		{
			if (m_WrapMode == WrapMode.Once)
			{
				return false;
			}
			if (m_WrapMode == WrapMode.Loop)
			{
				m_nCurMoveIndex = 0;
			}
		}
		MoveBase moveBase = m_ltMove[m_nCurMoveIndex];
		m_nCurMoveIndex++;
		switch (moveBase.m_State)
		{
		case MoveType.Stand:
		{
			MoveStand moveStand = (MoveStand)moveBase;
			m_curState = moveStand.m_State;
			m_fTimeCount = moveStand.m_fTime;
			break;
		}
		case MoveType.Move:
		{
			MoveGo moveGo = (MoveGo)moveBase;
			m_curState = moveGo.m_State;
			m_v3DestPos = moveGo.m_v3Pos;
			m_fSpeed = moveGo.m_fSpeed;
			if (moveGo.m_v3Dir != Vector3.zero)
			{
				float magnitude = (m_v3DestPos - m_Transform.position).magnitude;
				TurnRound(moveGo.m_v3Dir, 1f / (magnitude / m_fSpeed));
			}
			break;
		}
		case MoveType.Rotate:
		{
			MoveRotate moveRotate = (MoveRotate)moveBase;
			m_curState = moveRotate.m_State;
			TurnRound(moveRotate.m_v3Dir, moveRotate.m_fSpeed);
			break;
		}
		default:
			return false;
		}
		return true;
	}

	public void TurnRound(Vector3 v3Dir, float fSpeed)
	{
		m_v3ScrDir = m_Transform.forward;
		m_v3DestDir = v3Dir;
		m_fRotateSpeed = fSpeed;
		m_fDirRate = 0f;
	}

	public bool IsFinished()
	{
		return m_bFinished;
	}
}
