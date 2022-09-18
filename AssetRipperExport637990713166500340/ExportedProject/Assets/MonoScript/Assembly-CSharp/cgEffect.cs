using UnityEngine;

public class cgEffect : cgBase
{
	public Vector3 v3Pos;

	public Vector3 v3Forward;

	public GameObject effect;

	protected GameObject m_Effect;

	public override void Enter()
	{
		m_Effect = (GameObject)Object.Instantiate(effect);
		m_Effect.transform.parent = base.transform;
		m_Effect.transform.localEulerAngles = Vector3.zero;
		m_Effect.transform.localPosition = v3Pos;
		m_Effect.transform.forward = v3Forward;
	}

	public override void Exit()
	{
		Object.Destroy(m_Effect);
		m_Effect = null;
	}
}
