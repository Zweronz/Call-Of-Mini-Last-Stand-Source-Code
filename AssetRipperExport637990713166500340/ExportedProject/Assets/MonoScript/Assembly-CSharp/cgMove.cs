using UnityEngine;

public class cgMove : cgBase
{
	public bool isSrc;

	public Vector3 srcPos;

	public Vector3 dstPos;

	public string sAnim = string.Empty;

	public int animspeed = 1;

	protected float m_fTimeLen;

	public override void Enter()
	{
		if (isSrc)
		{
			base.transform.localPosition = srcPos;
		}
		else
		{
			srcPos = base.transform.localPosition;
		}
		Vector3 vector = dstPos - srcPos;
		vector.y = 0f;
		base.transform.forward = base.transform.parent.localToWorldMatrix * vector;
		m_fTimeLen = finishtime - starttime;
		if (sAnim.Length >= 1 && !(base.GetComponent<Animation>() == null) && !(base.GetComponent<Animation>()[sAnim] == null))
		{
			base.GetComponent<Animation>()[sAnim].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>()[sAnim].time = 0f;
			base.GetComponent<Animation>()[sAnim].speed = (float)animspeed * GetComponent<cgTimer>().TimeScale;
			base.GetComponent<Animation>().GetComponent<Animation>().Play(sAnim);
		}
	}

	public override void Loop(float deltaTime)
	{
		base.transform.localPosition = Vector3.Lerp(srcPos, dstPos, (GetComponent<cgTimer>().TimeTotal - starttime) / m_fTimeLen);
	}

	public override void Exit()
	{
		base.transform.localPosition = dstPos;
	}
}
