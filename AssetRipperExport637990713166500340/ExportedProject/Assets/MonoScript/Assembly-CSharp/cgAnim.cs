using UnityEngine;

public class cgAnim : cgBase
{
	public string sAnim = string.Empty;

	public WrapMode wrapmode;

	public bool isSmooth = true;

	public bool isFixPos;

	public Vector3 deltaPos = Vector3.zero;

	public string sAnimEnd = string.Empty;

	public bool isSample;

	public float fSampleTime;

	public override void Enter()
	{
		if (sAnim.Length < 1 || base.GetComponent<Animation>() == null || base.GetComponent<Animation>()[sAnim] == null)
		{
			return;
		}
		base.GetComponent<Animation>()[sAnim].wrapMode = wrapmode;
		base.GetComponent<Animation>()[sAnim].time = 0f;
		base.GetComponent<Animation>()[sAnim].speed = GetComponent<cgTimer>().TimeScale;
		if (isSample)
		{
			base.GetComponent<Animation>()[sAnim].time = fSampleTime;
			base.GetComponent<Animation>()[sAnim].speed = 0f;
			base.GetComponent<Animation>().Play(sAnim);
			base.GetComponent<Animation>().Sample();
			return;
		}
		if (isSmooth)
		{
			base.GetComponent<Animation>().CrossFade(sAnim);
		}
		else
		{
			base.GetComponent<Animation>().Play(sAnim);
		}
		if (isFixPos)
		{
			finishtime = starttime + base.GetComponent<Animation>()[sAnim].length;
		}
	}

	public override void Exit()
	{
		if (isFixPos)
		{
			float y = deltaPos.y;
			deltaPos = base.transform.localToWorldMatrix * new Vector3(deltaPos.x, 0f, deltaPos.z);
			deltaPos.y = y;
			base.transform.position += deltaPos;
			if (sAnimEnd.Length >= 1 && !(base.GetComponent<Animation>() == null) && !(base.GetComponent<Animation>()[sAnimEnd] == null))
			{
				base.GetComponent<Animation>()[sAnimEnd].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>()[sAnimEnd].time = 0f;
				base.GetComponent<Animation>()[sAnimEnd].speed = GetComponent<cgTimer>().TimeScale;
				base.GetComponent<Animation>().Play(sAnimEnd);
			}
		}
	}
}
