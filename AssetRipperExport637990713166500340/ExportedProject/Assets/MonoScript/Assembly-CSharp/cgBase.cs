using UnityEngine;

[RequireComponent(typeof(cgTimer))]
public class cgBase : MonoBehaviour
{
	public float starttime;

	public float finishtime;

	protected bool m_bEnter;

	protected bool m_bExit;

	public virtual void Init()
	{
	}

	public virtual void Enter()
	{
	}

	public virtual void Loop(float deltaTime)
	{
	}

	public virtual void Exit()
	{
	}

	private void Start()
	{
		m_bEnter = false;
		m_bExit = false;
		Init();
	}

	private void Update()
	{
		if (m_bExit)
		{
			return;
		}
		cgTimer component = GetComponent<cgTimer>();
		if (component == null || !component.isPlay)
		{
			return;
		}
		if (m_bEnter)
		{
			Loop(Time.deltaTime * component.TimeScale);
			if (component.TimeTotal >= finishtime)
			{
				Exit();
				m_bExit = true;
			}
		}
		else if (GetComponent<cgTimer>().TimeTotal >= starttime)
		{
			Enter();
			m_bEnter = true;
		}
	}
}
