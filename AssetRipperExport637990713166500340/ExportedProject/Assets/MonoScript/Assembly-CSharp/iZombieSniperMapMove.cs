using UnityEngine;

public class iZombieSniperMapMove : MonoBehaviour
{
	public Vector2 size = new Vector2(480f, 320f);

	public Vector2 screensize = new Vector2(480f, 320f);

	public float movespeed = 5f;

	protected Transform m_Transform;

	protected float min_x;

	protected float max_x;

	protected float min_y;

	protected float max_y;

	protected bool m_bSlip;

	protected float m_fCurSpeed;

	protected float m_fCurSpeedAcc;

	protected Vector2 m_v2Dir;

	protected int HD;

	private void Start()
	{
		HD = ((!Utils.IsRetina()) ? 1 : 2);
		size *= (float)HD;
		screensize *= (float)HD;
		m_Transform = base.transform;
		min_x = (screensize.x - size.x) / 2f;
		max_x = (size.x - screensize.x) / 2f;
		min_y = (screensize.y - size.y) / 2f;
		max_y = (size.y - screensize.y) / 2f;
	}

	private void Update()
	{
		if (m_bSlip)
		{
			UpdatePosition(m_v2Dir, m_fCurSpeed);
			m_fCurSpeed += m_fCurSpeedAcc * Time.deltaTime;
			if (m_fCurSpeed <= 0f)
			{
				m_bSlip = false;
			}
		}
	}

	public void Move(Vector2 deltaPosition)
	{
		float magnitude = deltaPosition.magnitude;
		m_v2Dir = deltaPosition / magnitude;
		m_fCurSpeed = movespeed * magnitude * Time.deltaTime;
		UpdatePosition(m_v2Dir, m_fCurSpeed);
	}

	public void Slip()
	{
		if (!(m_fCurSpeed < 2f * (float)HD))
		{
			m_bSlip = true;
			m_fCurSpeedAcc = (0f - m_fCurSpeed) * 0.5f;
		}
	}

	public void Stop()
	{
		m_bSlip = false;
	}

	private void UpdatePosition(Vector2 v2Dir, float speed)
	{
		Vector2 vector = v2Dir * speed;
		if (!(vector == Vector2.zero))
		{
			Vector3 position = m_Transform.position + new Vector3(vector.x, vector.y, 0f);
			if (position.x < min_x)
			{
				position.x = min_x;
			}
			else if (position.x > max_x)
			{
				position.x = max_x;
			}
			if (position.y < min_y)
			{
				position.y = min_y;
			}
			else if (position.y > max_y)
			{
				position.y = max_y;
			}
			m_Transform.position = position;
		}
	}
}
