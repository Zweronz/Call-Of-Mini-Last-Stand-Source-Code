using UnityEngine;

public class cgScale : cgBase
{
	public Vector3 srcScale;

	public Vector3 dstScale;

	protected float m_fTimeLen;

	public override void Enter()
	{
		base.transform.localScale = srcScale;
		m_fTimeLen = finishtime - starttime;
	}

	public override void Loop(float deltaTime)
	{
		base.transform.localScale = Vector3.Lerp(srcScale, dstScale, (GetComponent<cgTimer>().TimeTotal - starttime) / m_fTimeLen);
	}

	public override void Exit()
	{
		base.transform.localScale = dstScale;
	}
}
