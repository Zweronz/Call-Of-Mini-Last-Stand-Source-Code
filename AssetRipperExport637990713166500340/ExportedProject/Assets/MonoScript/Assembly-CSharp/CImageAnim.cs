using UnityEngine;

public class CImageAnim
{
	private struct ImageAnimInfo
	{
		public Material material;

		public Rect rect;
	}

	private UIImage m_Image;

	private ImageAnimInfo[] m_Anim;

	private int m_nIndex;

	private float m_fSpeed;

	private float m_fTimeCount;

	private bool m_bAnim;

	private bool m_bLoop;

	public void Initialize(UIImage image, int nCount)
	{
		m_Image = image;
		m_Anim = new ImageAnimInfo[nCount];
		m_nIndex = 0;
		m_fSpeed = 0.5f;
		m_fTimeCount = 0f;
		m_bAnim = false;
		m_bLoop = false;
	}

	public void Destroy()
	{
		m_Image = null;
		m_Anim = null;
	}

	public void Update(float deltaTime)
	{
		if (!m_bAnim || m_Image == null)
		{
			return;
		}
		m_fTimeCount += deltaTime;
		if (m_fTimeCount < m_fSpeed)
		{
			return;
		}
		m_fTimeCount = 0f;
		m_nIndex++;
		if (m_nIndex >= m_Anim.Length)
		{
			m_nIndex = 0;
			if (!m_bLoop)
			{
				m_bAnim = false;
				return;
			}
		}
		m_Image.SetTexture(m_Anim[m_nIndex].material, m_Anim[m_nIndex].rect);
	}

	public void Add(Material material, Rect rect)
	{
		if (m_nIndex >= 0 && m_nIndex < m_Anim.Length)
		{
			ImageAnimInfo imageAnimInfo = default(ImageAnimInfo);
			imageAnimInfo.material = material;
			imageAnimInfo.rect = rect;
			m_Anim[m_nIndex++] = imageAnimInfo;
		}
	}

	public void PlayAnim(float speed, bool bLoop = false)
	{
		if (m_Anim.Length >= 1 && !m_bAnim)
		{
			m_fSpeed = speed;
			m_bLoop = bLoop;
			m_nIndex = 0;
			m_bAnim = true;
			m_Image.SetTexture(m_Anim[m_nIndex].material, m_Anim[m_nIndex].rect);
		}
	}

	public void StopAnim()
	{
		if (m_bAnim)
		{
			m_nIndex = 0;
			m_bAnim = false;
			m_Image.SetTexture(m_Anim[m_nIndex].material, m_Anim[m_nIndex].rect);
		}
	}

	public void SetFrame(int nIndex)
	{
		if (nIndex >= 0 && nIndex < m_Anim.Length)
		{
			m_bAnim = false;
			m_nIndex = nIndex;
			m_Image.SetTexture(m_Anim[m_nIndex].material, m_Anim[m_nIndex].rect);
		}
	}

	public void SetPos(Vector2 v2)
	{
		if (m_Image != null)
		{
			m_Image.SetPosition(v2);
		}
	}

	public void Show(bool bShow)
	{
		if (m_Image != null)
		{
			m_Image.Visible = bShow;
		}
	}
}
