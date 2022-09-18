using UnityEngine;

public class cgJump : cgBase
{
	protected enum JumpStateEnum
	{
		None = 0,
		JumpBegin = 1,
		JumpRaise = 2,
		JumpFall = 3,
		JumpEnd = 4
	}

	public Vector3 m_v3Scr;

	public Vector3 m_v3Dst;

	public float m_fJumpHeight;

	public string sJumpAnim = string.Empty;

	protected JumpStateEnum m_JumpState;

	protected float m_fJumpCount;

	protected float m_fJumpDis;

	protected float m_fJumpSpeedZ;

	protected float m_fJumpSpeedY;

	protected float m_fJumpSpeedYInt;

	protected float m_fJumpSpeedYAcc;

	protected float m_fJumpTime;

	protected float m_fCurJumpDis;

	protected Vector3 m_v3Dir;

	public override void Enter()
	{
		base.transform.localPosition = m_v3Scr;
		m_v3Dir = m_v3Dst - m_v3Scr;
		m_v3Dir.y = 0f;
		base.transform.forward = base.transform.localToWorldMatrix * m_v3Dir;
		m_fJumpDis = m_v3Dir.magnitude;
		m_v3Dir /= m_fJumpDis;
		m_fJumpTime = finishtime - starttime;
		m_fJumpSpeedZ = m_fJumpDis / m_fJumpTime;
		m_fJumpSpeedYAcc = (0f - m_fJumpHeight) * 2f / (m_fJumpTime * m_fJumpTime * 0.25f);
		m_fJumpSpeedYInt = (0f - m_fJumpSpeedYAcc) * (m_fJumpTime * 0.5f);
		if (sJumpAnim.Length >= 1)
		{
			float fJumpTime = m_fJumpTime;
			if (base.GetComponent<Animation>() != null && base.GetComponent<Animation>()[sJumpAnim] != null)
			{
				fJumpTime = base.GetComponent<Animation>()[sJumpAnim].length;
				base.GetComponent<Animation>()[sJumpAnim].speed = fJumpTime / m_fJumpTime * GetComponent<cgTimer>().TimeScale;
				base.GetComponent<Animation>()[sJumpAnim].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade(sJumpAnim);
			}
			m_JumpState = JumpStateEnum.JumpRaise;
			m_fCurJumpDis = 0f;
			m_fJumpCount = 0f;
		}
	}

	public override void Loop(float deltaTime)
	{
		switch (m_JumpState)
		{
		case JumpStateEnum.JumpBegin:
			m_fJumpCount -= deltaTime;
			if (!(m_fJumpCount > 0f))
			{
				m_fJumpCount = 0f;
				m_JumpState = JumpStateEnum.JumpRaise;
			}
			break;
		case JumpStateEnum.JumpRaise:
		{
			m_fJumpCount += deltaTime;
			float num2 = m_fJumpSpeedZ * deltaTime;
			base.transform.localPosition += m_v3Dir * num2;
			m_fCurJumpDis += num2;
			if (m_fJumpSpeedYInt + m_fJumpSpeedYAcc * m_fJumpCount < 0f)
			{
				float y2 = m_fJumpSpeedYInt * m_fJumpCount + 0.5f * m_fJumpSpeedYAcc * m_fJumpCount * m_fJumpCount;
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, y2, base.transform.localPosition.z);
			}
			else
			{
				m_JumpState = JumpStateEnum.JumpFall;
				m_fJumpSpeedY = 0f;
			}
			break;
		}
		case JumpStateEnum.JumpFall:
		{
			m_fJumpCount += deltaTime;
			float num = m_fJumpSpeedZ * deltaTime;
			base.transform.localPosition += m_v3Dir * num;
			m_fCurJumpDis += num;
			if (m_fCurJumpDis >= m_fJumpDis || base.transform.localPosition.y < 0f)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, 0f, base.transform.localPosition.z);
				m_fJumpCount = 0.5f;
				m_JumpState = JumpStateEnum.JumpEnd;
			}
			else
			{
				float y = m_fJumpSpeedYInt * m_fJumpCount + 0.5f * m_fJumpSpeedYAcc * m_fJumpCount * m_fJumpCount;
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, y, base.transform.localPosition.z);
			}
			break;
		}
		case JumpStateEnum.JumpEnd:
			m_fJumpCount -= deltaTime;
			if (!(m_fJumpCount > 0f))
			{
				m_fJumpCount = 0f;
				m_JumpState = JumpStateEnum.None;
			}
			break;
		}
	}

	public override void Exit()
	{
		base.transform.localPosition = m_v3Dst;
	}
}
