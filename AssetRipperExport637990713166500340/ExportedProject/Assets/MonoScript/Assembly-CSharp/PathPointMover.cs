using UnityEngine;

public class PathPointMover : MonoBehaviour
{
	public enum MoverState
	{
		None = 0,
		Playing = 1,
		Pause = 2,
		Finish = 3
	}

	public bool m_bLockRotate;

	public WrapMode m_WrapMode;

	public GameObject m_PathManager;

	public GameObject m_PathObject;

	private MoverState m_State;

	private CPathPara m_PathPara;

	private Transform m_Transform;

	private int m_nCurMoveIndex;

	private CMoveBase.MoveType m_curState;

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
	}

	private void Update()
	{
		UpdateLogic(Time.deltaTime);
	}

	public void Initialize(string sPathName)
	{
		m_PathManager = GameObject.Find("PathManager");
		if (m_PathObject == null)
		{
			Transform transform = m_PathManager.transform.Find(sPathName);
			if (transform != null)
			{
				m_PathObject = transform.gameObject;
			}
		}
		m_PathPara = m_PathObject.GetComponent<CPathPara>();
		m_Transform = base.transform;
		m_fTimeCount = 0f;
		m_v3DestPos = Vector3.zero;
		m_v3DestDir = Vector3.zero;
		m_fSpeed = 0f;
		m_nCurMoveIndex = 0;
		m_State = MoverState.None;
	}

	public void Destroy()
	{
	}

	public void Reset()
	{
		m_State = MoverState.None;
		m_nCurMoveIndex = 0;
	}

	public void StartMove(int nIndex = -1)
	{
		if (m_PathPara.m_ltPoint.Count >= 1)
		{
			if (nIndex == -1)
			{
				nIndex = UnityEngine.Random.Range(0, m_PathPara.m_ltPoint.Count);
			}
			m_nCurMoveIndex = nIndex;
			GetNextStep();
			if (m_curState == CMoveBase.MoveType.Move)
			{
				base.transform.position = m_v3DestPos;
				base.transform.forward = m_v3DestDir;
				GetNextStep();
			}
			m_State = MoverState.Playing;
		}
	}

	public void Stop()
	{
		m_State = MoverState.Finish;
	}

	public void Pause()
	{
		if (m_State == MoverState.Playing)
		{
			m_State = MoverState.Pause;
		}
	}

	public void Resume()
	{
		if (m_State == MoverState.Pause)
		{
			m_State = MoverState.Playing;
		}
	}

	public void UpdateLogic(float deltaTime)
	{
		if (m_State != MoverState.Playing)
		{
			return;
		}
		switch (m_curState)
		{
		case CMoveBase.MoveType.Stand:
			m_fTimeCount -= deltaTime;
			if (m_fTimeCount <= 0f && !GetNextStep())
			{
				m_State = MoverState.Finish;
			}
			break;
		case CMoveBase.MoveType.Move:
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
				Vector3 eulerAngles = m_Transform.eulerAngles;
				eulerAngles.z = 0f;
				m_Transform.eulerAngles = eulerAngles;
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
					m_State = MoverState.Finish;
				}
			}
			else
			{
				m_Transform.position += num * vector2;
			}
			break;
		}
		case CMoveBase.MoveType.Rotate:
			m_fDirRate += m_fRotateSpeed * deltaTime;
			if (!m_bLockRotate)
			{
				m_Transform.forward = Vector3.Lerp(m_v3ScrDir, m_v3DestDir, m_fDirRate);
			}
			if (m_fDirRate >= 1f && !GetNextStep())
			{
				m_State = MoverState.Finish;
			}
			break;
		}
	}

	public bool GetNextStep()
	{
		if (m_PathPara == null)
		{
			return false;
		}
		if (m_PathPara.m_ltPoint.Count < 1)
		{
			return false;
		}
		if (m_nCurMoveIndex >= m_PathPara.m_ltPoint.Count)
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
		if (m_nCurMoveIndex < 0 || m_nCurMoveIndex >= m_PathPara.m_ltPoint.Count)
		{
			return false;
		}
		CMoveBase component = m_PathPara.m_ltPoint[m_nCurMoveIndex].GetComponent<CMoveBase>();
		if (component == null)
		{
			return false;
		}
		m_nCurMoveIndex++;
		switch (component.m_State)
		{
		case CMoveBase.MoveType.Stand:
		{
			CMoveStand cMoveStand = (CMoveStand)component;
			m_curState = cMoveStand.m_State;
			m_fTimeCount = cMoveStand.m_fTime;
			break;
		}
		case CMoveBase.MoveType.Move:
		{
			CMoveGo cMoveGo = (CMoveGo)component;
			m_curState = cMoveGo.m_State;
			m_v3DestPos = cMoveGo.m_v3Pos;
			m_fSpeed = cMoveGo.m_fSpeed;
			if (cMoveGo.m_v3Dir != Vector3.zero)
			{
				float magnitude = (m_v3DestPos - m_Transform.position).magnitude;
				TurnRound(cMoveGo.m_v3Dir, 1f / (magnitude / m_fSpeed));
			}
			break;
		}
		case CMoveBase.MoveType.Rotate:
		{
			CMoveRotate cMoveRotate = (CMoveRotate)component;
			m_curState = cMoveRotate.m_State;
			TurnRound(cMoveRotate.m_v3Dir, cMoveRotate.m_fSpeed);
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

	public bool IsPlaying()
	{
		return m_State == MoverState.Playing;
	}

	public bool IsFinished()
	{
		return m_State == MoverState.Finish;
	}
}
