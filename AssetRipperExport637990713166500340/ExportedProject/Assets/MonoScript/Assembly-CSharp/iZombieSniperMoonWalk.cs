using System.Collections;
using UnityEngine;

public class iZombieSniperMoonWalk
{
	public enum MoonWalkEnum
	{
		Stand = 0,
		Move = 1,
		Rotate = 2
	}

	public enum MoonWalkType
	{
		None = 0,
		Begin = 1,
		LookNPC = 2
	}

	public class MoonWalkBase
	{
		public MoonWalkEnum m_State;
	}

	public class MoonWalkStand : MoonWalkBase
	{
		public float m_fTime;
	}

	public class MoonWalkMove : MoonWalkBase
	{
		public Vector3 m_v3Position;

		public Vector3 m_v3Direction;

		public float m_fSpeed;
	}

	public class MoonWalkRotate : MoonWalkBase
	{
		public Vector3 m_v3Direction;

		public float m_fSpeed;
	}

	public MoonWalkType m_Type;

	public WrapMode m_WrapMode;

	public Camera m_CameraObject;

	public iZombieSniperCamera m_CameraScript;

	public ArrayList m_GameWalkMove;

	public int m_nCurWalkIndex;

	public bool m_bFinished;

	public MoonWalkEnum m_State;

	public float m_fTimeCount;

	public Vector3 m_v3DestPos;

	public Vector3 m_v3DestDir;

	public float m_fSpeed;

	public float m_fRotateSpeed;

	public Vector3 m_v3ScrDir;

	public float m_fDirRate;

	public bool m_bRotateBody;

	public bool m_bLockRotate;

	public void Initialize(Camera camera, bool bLockRotate = true)
	{
		if (m_GameWalkMove == null)
		{
			m_GameWalkMove = new ArrayList();
		}
		m_GameWalkMove.Clear();
		m_CameraObject = camera;
		m_CameraScript = camera.GetComponent<iZombieSniperCamera>();
		m_fTimeCount = 0f;
		m_v3DestPos = Vector3.zero;
		m_v3DestDir = Vector3.zero;
		m_fSpeed = 0f;
		m_bLockRotate = bLockRotate;
		m_WrapMode = WrapMode.Once;
		m_nCurWalkIndex = 0;
	}

	public void Destroy()
	{
		if (m_GameWalkMove != null)
		{
			m_GameWalkMove.Clear();
			m_GameWalkMove = null;
		}
	}

	public void Reset()
	{
		if (m_GameWalkMove != null)
		{
			m_GameWalkMove.Clear();
		}
	}

	public void Update(float deltaTime)
	{
		if (m_bFinished)
		{
			return;
		}
		switch (m_State)
		{
		case MoonWalkEnum.Stand:
			m_fTimeCount -= deltaTime;
			if (m_fTimeCount <= 0f && !GetNextStep())
			{
				m_bFinished = true;
			}
			break;
		case MoonWalkEnum.Move:
		{
			if (m_fDirRate < 1f)
			{
				m_fDirRate += m_fRotateSpeed * deltaTime;
				if (!m_bLockRotate)
				{
					m_CameraObject.transform.forward = Vector3.Lerp(m_v3ScrDir, m_v3DestDir, m_fDirRate);
				}
				if (m_fDirRate >= 1f)
				{
					m_fDirRate = 1f;
				}
			}
			Vector3 vector = m_v3DestPos - m_CameraScript.GetPosition();
			float magnitude = vector.magnitude;
			Vector3 vector2 = vector / magnitude;
			float num = m_fSpeed * deltaTime;
			if (num > magnitude)
			{
				m_CameraScript.SetPosition(m_v3DestPos);
				if (!GetNextStep())
				{
					m_bFinished = true;
				}
			}
			else
			{
				m_CameraScript.SetPosition(m_CameraScript.GetPosition() + num * vector2);
			}
			break;
		}
		case MoonWalkEnum.Rotate:
			m_fDirRate += m_fRotateSpeed * deltaTime;
			if (!m_bLockRotate)
			{
				m_CameraObject.transform.forward = Vector3.Lerp(m_v3ScrDir, m_v3DestDir, m_fDirRate);
			}
			if (m_fDirRate >= 1f && !GetNextStep())
			{
				m_bFinished = true;
			}
			break;
		}
	}

	public void Start(MoonWalkType nType, WrapMode wrapmode = WrapMode.Once)
	{
		if (m_GameWalkMove.Count > 0)
		{
			m_nCurWalkIndex = 0;
			GetNextStep();
			m_Type = nType;
			m_bFinished = false;
			m_WrapMode = wrapmode;
		}
	}

	public void StartAtIndex(int nIndex = -1)
	{
		if (m_GameWalkMove.Count <= 0)
		{
			return;
		}
		if (nIndex == -1)
		{
			nIndex = UnityEngine.Random.Range(0, m_GameWalkMove.Count);
		}
		if (nIndex >= 0 && nIndex < m_GameWalkMove.Count)
		{
			m_nCurWalkIndex = nIndex;
			GetNextStep();
			if (m_State == MoonWalkEnum.Move)
			{
				m_CameraObject.transform.forward = m_v3DestDir;
				m_CameraScript.SetPosition(m_v3DestPos);
			}
			GetNextStep();
		}
	}

	public void Pause()
	{
		m_bFinished = true;
	}

	public void Continue()
	{
		m_bFinished = false;
	}

	public void AddMove(Vector3 v3Pos, Vector3 v3Dir, float fSpeed)
	{
		MoonWalkMove moonWalkMove = new MoonWalkMove();
		moonWalkMove.m_State = MoonWalkEnum.Move;
		moonWalkMove.m_v3Position = v3Pos;
		moonWalkMove.m_fSpeed = fSpeed;
		if (v3Dir != Vector3.zero)
		{
			moonWalkMove.m_v3Direction = v3Dir.normalized;
		}
		else
		{
			moonWalkMove.m_v3Direction = v3Dir;
		}
		AddPoint(moonWalkMove);
	}

	public void AddRotate(Vector3 v3Dir, float fSpeed)
	{
		MoonWalkRotate moonWalkRotate = new MoonWalkRotate();
		moonWalkRotate.m_State = MoonWalkEnum.Rotate;
		moonWalkRotate.m_fSpeed = fSpeed;
		if (v3Dir != Vector3.zero)
		{
			moonWalkRotate.m_v3Direction = v3Dir.normalized;
		}
		else
		{
			moonWalkRotate.m_v3Direction = v3Dir;
		}
		AddPoint(moonWalkRotate);
	}

	public void AddStand(float fTime)
	{
		MoonWalkStand moonWalkStand = new MoonWalkStand();
		moonWalkStand.m_State = MoonWalkEnum.Stand;
		moonWalkStand.m_fTime = fTime;
		AddPoint(moonWalkStand);
	}

	public void AddPoint(MoonWalkBase info)
	{
		m_GameWalkMove.Add(info);
	}

	public bool GetNextStep()
	{
		if (m_GameWalkMove.Count <= 0)
		{
			return false;
		}
		if (m_nCurWalkIndex < 0)
		{
			return false;
		}
		if (m_nCurWalkIndex >= m_GameWalkMove.Count)
		{
			if (m_WrapMode == WrapMode.Once)
			{
				return false;
			}
			if (m_WrapMode == WrapMode.Loop)
			{
				m_nCurWalkIndex = 0;
			}
		}
		MoonWalkBase moonWalkBase = (MoonWalkBase)m_GameWalkMove[m_nCurWalkIndex];
		m_nCurWalkIndex++;
		switch (moonWalkBase.m_State)
		{
		case MoonWalkEnum.Stand:
		{
			MoonWalkStand moonWalkStand = (MoonWalkStand)moonWalkBase;
			m_State = moonWalkStand.m_State;
			m_fTimeCount = moonWalkStand.m_fTime;
			break;
		}
		case MoonWalkEnum.Move:
		{
			MoonWalkMove moonWalkMove = (MoonWalkMove)moonWalkBase;
			m_State = moonWalkMove.m_State;
			m_v3DestPos = moonWalkMove.m_v3Position;
			m_fSpeed = moonWalkMove.m_fSpeed;
			if (moonWalkMove.m_v3Direction != Vector3.zero)
			{
				float magnitude = (m_v3DestPos - m_CameraObject.transform.position).magnitude;
				TurnRound(moonWalkMove.m_v3Direction, 1f / (magnitude / m_fSpeed));
			}
			break;
		}
		case MoonWalkEnum.Rotate:
		{
			MoonWalkRotate moonWalkRotate = (MoonWalkRotate)moonWalkBase;
			m_State = moonWalkRotate.m_State;
			TurnRound(moonWalkRotate.m_v3Direction, moonWalkRotate.m_fSpeed);
			break;
		}
		default:
			return false;
		}
		return true;
	}

	public void TurnRound(Vector3 v3Dir, float fSpeed)
	{
		m_v3ScrDir = m_CameraObject.transform.forward;
		m_v3DestDir = v3Dir;
		m_fRotateSpeed = fSpeed;
		m_fDirRate = 0f;
	}

	public bool IsFinished(MoonWalkType nType)
	{
		if (m_bFinished && nType == m_Type)
		{
			return true;
		}
		return false;
	}
}
