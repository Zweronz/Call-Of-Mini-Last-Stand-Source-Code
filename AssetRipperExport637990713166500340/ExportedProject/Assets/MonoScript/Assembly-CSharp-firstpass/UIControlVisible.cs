using UnityEngine;

public class UIControlVisible : UIControl
{
	protected UISprite[] m_Sprite;

	public UIControlVisible()
	{
		m_Sprite = null;
	}

	protected void CreateSprite(int number)
	{
		m_Sprite = new UISprite[number];
		for (int i = 0; i < number; i++)
		{
			m_Sprite[i] = new UISprite();
		}
	}

	protected void SetSpriteTexture(int index, Material material, Rect texture_rect, Vector2 size)
	{
		m_Sprite[index].Material = material;
		m_Sprite[index].TextureRect = texture_rect;
		m_Sprite[index].Size = size;
	}

	protected void SetSpriteTexture(int index, Material material, Rect texture_rect)
	{
		m_Sprite[index].Material = material;
		m_Sprite[index].TextureRect = texture_rect;
		m_Sprite[index].Size = new Vector2(texture_rect.width, texture_rect.height);
	}

	protected void SetSpriteTexture(int index, Rect texture_rect)
	{
		m_Sprite[index].TextureRect = texture_rect;
	}

	protected void SetSpriteTexture(int index, Material material)
	{
		m_Sprite[index].Material = material;
	}

	protected void SetSpriteSize(int index, Vector2 size)
	{
		m_Sprite[index].Size = size;
	}

	protected Vector2 GetSpriteSize(int index)
	{
		return m_Sprite[index].Size;
	}

	protected void SetSpriteColor(int index, Color color)
	{
		m_Sprite[index].Color = color;
	}

	protected void SetSpriteAlpha(int index, float alpha)
	{
		m_Sprite[index].Color = new Color(m_Sprite[index].Color.r, m_Sprite[index].Color.g, m_Sprite[index].Color.b, alpha);
	}

	protected float GetSpriteAlpha(int index)
	{
		return m_Sprite[index].Color.a;
	}

	protected void SetSpritePosition(int index, Vector2 position)
	{
		m_Sprite[index].Position = position;
	}

	protected void SetSpriteRotation(int index, float rotation)
	{
		m_Sprite[index].Rotation = rotation;
	}

	protected float GetSpriteRotation(int index)
	{
		return m_Sprite[index].Rotation;
	}

	public void SetScale(float scale)
	{
		for (int i = 0; i < m_Sprite.Length; i++)
		{
			m_Sprite[i].Scale = scale;
		}
	}

	public float GetScale(int index)
	{
		return m_Sprite[index].Scale;
	}

	public new void SetClip(Rect clip_rect)
	{
		base.SetClip(clip_rect);
		if (m_Sprite != null)
		{
			for (int i = 0; i < m_Sprite.Length; i++)
			{
				m_Sprite[i].SetClip(clip_rect);
			}
		}
	}

	public new void ClearClip()
	{
		base.ClearClip();
		if (m_Sprite != null)
		{
			for (int i = 0; i < m_Sprite.Length; i++)
			{
				m_Sprite[i].ClearClip();
			}
		}
	}

	public void SetRect(Rect rect)
	{
		if (!Utils.IsIPhoneOrITouch())
		{
			float num = 480 * ((!Utils.IsRetina()) ? 1 : 2);
			float num2 = 320 * ((!Utils.IsRetina()) ? 1 : 2);
			rect.x = (int)(rect.x * ((float)Screen.width / num));
			rect.y = (int)(rect.y * ((float)Screen.height / num2));
			rect.width = (int)(rect.width * ((float)Screen.width / num));
			rect.height = (int)(rect.height * ((float)Screen.height / num2));
		}
		Rect = rect;
	}
}
