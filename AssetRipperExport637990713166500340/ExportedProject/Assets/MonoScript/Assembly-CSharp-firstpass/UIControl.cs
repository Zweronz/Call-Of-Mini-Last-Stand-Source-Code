using UnityEngine;

public class UIControl
{
	protected UIContainer m_Parent;

	protected int m_Id;

	protected Rect m_Rect;

	protected bool m_Visible;

	protected bool m_Enable;

	protected bool m_Clip;

	protected Rect m_ClipRect;

	protected int m_Layer;

	public int Id
	{
		get
		{
			return m_Id;
		}
		set
		{
			m_Id = value;
		}
	}

	public virtual Rect Rect
	{
		get
		{
			return m_Rect;
		}
		set
		{
			m_Rect = value;
		}
	}

	public bool Visible
	{
		get
		{
			return m_Visible;
		}
		set
		{
			m_Visible = value;
		}
	}

	public bool Enable
	{
		get
		{
			return m_Enable;
		}
		set
		{
			m_Enable = value;
		}
	}

	public int Layer
	{
		get
		{
			return m_Layer;
		}
		set
		{
			if (value > 7)
			{
				value = 7;
			}
			else if (value < 0)
			{
				value = 0;
			}
			m_Layer = value;
		}
	}

	public Rect Clip
	{
		get
		{
			if (m_Clip)
			{
				return m_ClipRect;
			}
			return m_Rect;
		}
	}

	public UIControl()
	{
		m_Parent = null;
		m_Id = 0;
		m_Rect = new Rect(0f, 0f, 0f, 0f);
		m_Visible = true;
		m_Enable = true;
		m_Layer = 3;
	}

	public void SetParent(UIContainer parent)
	{
		m_Parent = parent;
	}

	public void SetClip(Rect clip_rect)
	{
		m_Clip = true;
		m_ClipRect = clip_rect;
	}

	public void ClearClip()
	{
		m_Clip = false;
	}

	public virtual bool PtInRect(Vector2 pt)
	{
		if (pt.x >= m_Rect.xMin && pt.x < m_Rect.xMax && pt.y >= m_Rect.yMin && pt.y < m_Rect.yMax)
		{
			if (m_Clip)
			{
				return pt.x >= m_ClipRect.xMin && pt.x < m_ClipRect.xMax && pt.y >= m_ClipRect.yMin && pt.y < m_ClipRect.yMax;
			}
			return true;
		}
		return false;
	}

	public virtual void Draw()
	{
	}

	public virtual void Update()
	{
	}

	public virtual bool HandleInput(UITouchInner touch)
	{
		return false;
	}
}
