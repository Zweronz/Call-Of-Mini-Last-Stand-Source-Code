using UnityEngine;

public class NailInfo
{
	public int m_nID;

	public UIImage m_Image;

	public bool m_bUsed;

	public float m_fTime;

	public bool m_bRotate;

	public bool m_bRotateLoop;

	public float m_fRotateTime;

	public float m_fRotateCount;

	public float m_fAngleSpeed;

	public bool m_bTranslate;

	public bool m_bTransLoop;

	public Vector2 m_v2Scr;

	public float m_fTransTime;

	public float m_fTransCount;

	public float m_fSpeedX;

	public float m_fSpeedY;

	public float m_fAccX;

	public float m_fAccY;

	public bool m_bAlpha;

	public bool m_bAlphaLoop;

	public float m_fAlphaMax;

	public float m_fAlphaMin;

	public float m_fAlphaSpeed;

	public NailInfo()
	{
		m_nID = 0;
		m_Image = null;
		m_bUsed = false;
		m_fTime = 0f;
		m_bRotate = false;
		m_bRotateLoop = false;
		m_fRotateTime = 0f;
		m_fRotateCount = 0f;
		m_fAlphaSpeed = 0f;
		m_bTranslate = false;
		m_bTransLoop = false;
		m_v2Scr = Vector2.zero;
		m_fTransTime = 0f;
		m_fTransCount = 0f;
		m_fSpeedX = 0f;
		m_fSpeedY = 0f;
		m_fAccX = 0f;
		m_fAccY = 0f;
		m_bAlpha = false;
		m_bAlphaLoop = false;
		m_fAlphaMax = 0f;
		m_fAlphaMin = 0f;
		m_fAlphaSpeed = 0f;
	}
}
