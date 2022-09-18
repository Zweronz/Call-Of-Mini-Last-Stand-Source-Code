using UnityEngine;

public class UIImage : UIControlVisible
{
	public enum Command
	{
		Click = 0,
		Pressing = 1,
		PressEnd = 2
	}

	public override Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			Vector2 position = new Vector2(value.x + value.width / 2f, value.y + value.height / 2f);
			SetSpritePosition(0, position);
		}
	}

	public UIImage()
	{
		CreateSprite(1);
	}

	public void SetTexture(Material material, Rect texture_rect, Vector2 size)
	{
		SetSpriteTexture(0, material, texture_rect, size);
	}

	public void SetTexture(Material material, Rect texture_rect)
	{
		SetSpriteTexture(0, material, texture_rect);
	}

	public void SetTexture(Rect texture_rect)
	{
		SetSpriteTexture(0, texture_rect);
	}

	public void SetTexture(Material material)
	{
		SetSpriteTexture(0, material);
	}

	public void SetTextureSize(Vector2 size)
	{
		SetSpriteSize(0, size);
	}

	public Vector2 GetTextureSize()
	{
		return GetSpriteSize(0);
	}

	public void SetRotation(float rotation)
	{
		SetSpriteRotation(0, rotation);
	}

	public float GetRotation()
	{
		return GetSpriteRotation(0);
	}

	public void SetColor(Color color)
	{
		SetSpriteColor(0, color);
	}

	public void SetAlpha(float alpha)
	{
		SetSpriteAlpha(0, alpha);
	}

	public float GetAlpha()
	{
		return GetSpriteAlpha(0);
	}

	public void SetPosition(Vector2 position)
	{
		Rect = new Rect(position.x - Rect.width / 2f, position.y - Rect.height / 2f, Rect.width, Rect.height);
	}

	public Vector2 GetPosition()
	{
		return new Vector2(Rect.left + Rect.width / 2f, Rect.top + Rect.height / 2f);
	}

	public override void Draw()
	{
		m_Parent.DrawSprite(m_Sprite[0]);
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_Parent.SendEvent(this, 1, 0f, 0f);
				return true;
			}
			return false;
		}
		return false;
	}
}
