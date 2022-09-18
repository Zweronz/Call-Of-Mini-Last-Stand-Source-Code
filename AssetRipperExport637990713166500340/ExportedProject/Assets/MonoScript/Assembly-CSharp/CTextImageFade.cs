public class CTextImageFade : CTextImage
{
	public enum FadeMode
	{
		None = 0,
		Fadein = 1,
		Fadeout = 2
	}

	private FadeMode m_FadeMode;

	private float m_fCurAlpha;

	private float m_fFadeRate;

	public void FadeIn(float fFadeTime)
	{
	}

	public void FadeOut(float fFadeTime)
	{
		m_FadeMode = FadeMode.Fadeout;
		m_fCurAlpha = 1f;
		m_fFadeRate = 1f / fFadeTime;
		Show(true);
	}

	public void Update(float deltaTime)
	{
		if (m_FadeMode == FadeMode.None || m_FadeMode == FadeMode.Fadein || m_FadeMode != FadeMode.Fadeout)
		{
			return;
		}
		m_fCurAlpha -= m_fFadeRate * deltaTime;
		if (m_fCurAlpha <= 0f)
		{
			m_fCurAlpha = 0f;
		}
		foreach (UIImage image in m_ImageList)
		{
			image.SetAlpha(m_fCurAlpha);
		}
		if (m_fCurAlpha <= 0f)
		{
			m_FadeMode = FadeMode.None;
			Show(false);
		}
	}
}
