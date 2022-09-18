using System;
using System.Collections;
using UnityEngine;

public class CNail
{
	public UIManager m_UIManagerRef;

	public iZombieSniperGameState m_GameState;

	public Material m_Material;

	public Rect m_rtRect;

	public bool m_bShow;

	public int m_nLayer;

	private int m_nContrlID = 1;

	private Hashtable m_NailInfoMap;

	public virtual void Initialize(UIManager UIManagerRef, Material material, Rect rtRect)
	{
		m_UIManagerRef = UIManagerRef;
		m_Material = material;
		m_rtRect = rtRect;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_NailInfoMap = new Hashtable();
		m_NailInfoMap.Clear();
		m_nLayer = 3;
	}

	public virtual void Destroy()
	{
		if (m_NailInfoMap == null)
		{
			return;
		}
		foreach (NailInfo value in m_NailInfoMap.Values)
		{
			if (value.m_Image != null)
			{
				m_UIManagerRef.Remove(value.m_Image);
				value.m_Image = null;
			}
		}
		m_NailInfoMap.Clear();
		m_NailInfoMap = null;
	}

	public int AddPoint(Vector2 v2Pos, Vector2 v2Size, Color color, Vector2 v2Dir, float fTime = 0f)
	{
		NailInfo nailInfo = null;
		foreach (NailInfo value in m_NailInfoMap.Values)
		{
			if (!value.m_bUsed && value.m_Image != null)
			{
				nailInfo = value;
				break;
			}
		}
		if (nailInfo == null)
		{
			nailInfo = new NailInfo();
			nailInfo.m_nID = m_nContrlID++;
			nailInfo.m_Image = new UIImage();
			nailInfo.m_Image.Id = nailInfo.m_nID;
			nailInfo.m_Image.Layer = m_nLayer;
			m_UIManagerRef.Add(nailInfo.m_Image);
			m_NailInfoMap.Add(nailInfo.m_nID, nailInfo);
		}
		nailInfo.m_bUsed = true;
		if (v2Size == Vector2.zero)
		{
			nailInfo.m_Image.SetTexture(m_Material, m_rtRect);
		}
		else
		{
			nailInfo.m_Image.SetTexture(m_Material, m_rtRect, v2Size);
		}
		nailInfo.m_Image.SetPosition(v2Pos);
		nailInfo.m_Image.SetColor(color);
		nailInfo.m_Image.Visible = m_bShow;
		nailInfo.m_fTime = fTime;
		if (v2Dir != Vector2.zero)
		{
			float num = Mathf.Atan(v2Dir.y / v2Dir.x);
			if (v2Dir.x > 0f)
			{
				nailInfo.m_Image.SetRotation(0f - ((float)Math.PI / 2f - num));
			}
			else
			{
				nailInfo.m_Image.SetRotation((float)Math.PI / 2f + num);
			}
		}
		return nailInfo.m_nID;
	}

	public void SetPointPos(int nID, Vector2 v2Pos, bool bIgnoreRotate = false)
	{
		if (!m_NailInfoMap.ContainsKey(nID) || ((NailInfo)m_NailInfoMap[nID]).m_Image == null || !((NailInfo)m_NailInfoMap[nID]).m_bUsed)
		{
			return;
		}
		NailInfo nailInfo = (NailInfo)m_NailInfoMap[nID];
		nailInfo.m_Image.SetPosition(v2Pos);
		if (bIgnoreRotate)
		{
			return;
		}
		Vector2 vector = v2Pos - m_GameState.m_v3ShootCenter;
		if (vector != Vector2.zero)
		{
			float num = Mathf.Atan(vector.y / vector.x);
			if (vector.x > 0f)
			{
				nailInfo.m_Image.SetRotation(0f - ((float)Math.PI / 2f - num));
			}
			else
			{
				nailInfo.m_Image.SetRotation((float)Math.PI / 2f + num);
			}
		}
	}

	public void RemovePoint(int nID)
	{
		if (m_NailInfoMap.ContainsKey(nID))
		{
			if (((NailInfo)m_NailInfoMap[nID]).m_Image != null)
			{
				((NailInfo)m_NailInfoMap[nID]).m_Image.Visible = false;
			}
			((NailInfo)m_NailInfoMap[nID]).m_bUsed = false;
		}
	}

	public NailInfo GetPoint(int nID)
	{
		if (!m_NailInfoMap.ContainsKey(nID))
		{
			return null;
		}
		if (((NailInfo)m_NailInfoMap[nID]).m_Image == null)
		{
			return null;
		}
		return (NailInfo)m_NailInfoMap[nID];
	}

	public void Show(bool bShow = true)
	{
		foreach (NailInfo value in m_NailInfoMap.Values)
		{
			if (value.m_bUsed && value.m_Image != null)
			{
				value.m_Image.Visible = bShow;
			}
		}
		m_bShow = bShow;
	}

	public void Update(float deltaTime)
	{
		foreach (NailInfo value in m_NailInfoMap.Values)
		{
			if (!value.m_bUsed)
			{
				continue;
			}
			if (value.m_bTranslate)
			{
				if (value.m_Image != null)
				{
					value.m_Image.SetPosition(value.m_Image.GetPosition() + new Vector2(value.m_fSpeedX * deltaTime * (float)m_GameState.m_nHDFactor, value.m_fSpeedY * deltaTime * (float)m_GameState.m_nHDFactor));
				}
				value.m_fSpeedX += value.m_fAccX * deltaTime;
				value.m_fSpeedY += value.m_fAccY * deltaTime;
				if (value.m_fTransTime > 0f)
				{
					value.m_fTransCount += deltaTime;
					if (value.m_fTransCount >= value.m_fTransTime)
					{
						value.m_fTransCount = 0f;
						if (!value.m_bTransLoop)
						{
							value.m_bTranslate = false;
							value.m_bTransLoop = false;
						}
						else
						{
							value.m_Image.SetPosition(value.m_v2Scr);
						}
					}
				}
			}
			if (value.m_bRotate)
			{
				if (value.m_Image != null)
				{
					value.m_Image.SetRotation(value.m_Image.GetRotation() + value.m_fAngleSpeed * deltaTime);
				}
				if (value.m_fRotateTime > 0f)
				{
					value.m_fRotateCount += deltaTime;
					if (value.m_fRotateCount >= value.m_fRotateTime)
					{
						value.m_fRotateCount = 0f;
						if (!value.m_bRotateLoop)
						{
							value.m_bRotate = false;
							value.m_bRotateLoop = false;
						}
					}
				}
			}
			if (value.m_bAlpha && value.m_Image != null)
			{
				float num = value.m_Image.GetAlpha() + value.m_fAlphaSpeed;
				if (num < value.m_fAlphaMin)
				{
					num = value.m_fAlphaMin;
					value.m_fAlphaSpeed *= -1f;
				}
				else if (num > value.m_fAlphaMax)
				{
					num = value.m_fAlphaMax;
					value.m_fAlphaSpeed *= -1f;
				}
				value.m_Image.SetAlpha(num);
			}
			if (value.m_fTime > 0f)
			{
				value.m_fTime -= deltaTime;
				if (value.m_fTime <= 0f)
				{
					RemovePoint(value.m_nID);
				}
			}
		}
	}

	public void AddTransAnim(int nID, float fDX, float fDY, float fAX, float fAY, float fTime, bool bLoop = false)
	{
		NailInfo point = GetPoint(nID);
		if (point != null)
		{
			point.m_bTranslate = true;
			point.m_bTransLoop = bLoop;
			point.m_fSpeedX = fDX;
			point.m_fSpeedY = fDY;
			point.m_fAccX = fAX;
			point.m_fAccY = fAY;
			point.m_fTransTime = fTime;
			point.m_v2Scr = point.m_Image.GetPosition();
			point.m_fTransCount = 0f;
		}
	}

	public void AddRotateAnim(int nID, float fAngle, float fTime, bool bLoop = true)
	{
		NailInfo point = GetPoint(nID);
		if (point != null)
		{
			point.m_bRotate = true;
			point.m_fAngleSpeed = fAngle;
			point.m_bRotateLoop = bLoop;
			point.m_fRotateTime = fTime;
			point.m_fRotateCount = 0f;
		}
	}

	public void AddAlphaAnim(int nID, float fAlphaSpeed, float fMin, float fMax, bool bLoop = true)
	{
		NailInfo point = GetPoint(nID);
		if (point != null)
		{
			point.m_bAlpha = true;
			point.m_fAlphaMin = fMin;
			point.m_fAlphaMax = fMax;
			point.m_fAlphaSpeed = fAlphaSpeed;
			point.m_bAlphaLoop = bLoop;
		}
	}

	public void SetLayer(int layer)
	{
		m_nLayer = layer;
	}
}
