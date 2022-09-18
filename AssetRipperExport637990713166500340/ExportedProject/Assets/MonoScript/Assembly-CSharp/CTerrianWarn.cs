using System.Collections;
using UnityEngine;

public class CTerrianWarn
{
	private iZombieSniperGameSceneBase m_GameScene;

	private iZombieSniperGameState m_GameState;

	private iZombieSniperGameUI m_GameSceneUI;

	private int m_nIDCount;

	private Hashtable m_TerrianWarnMap;

	public void Initialize()
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_GameSceneUI = m_GameScene.m_GameSceneUI;
		m_nIDCount = 0;
		m_TerrianWarnMap = new Hashtable();
	}

	public void Destroy()
	{
		RemoveAll();
		m_TerrianWarnMap = null;
	}

	public void Update(float deltaTime)
	{
		foreach (TerrianWarnInfo value in m_TerrianWarnMap.Values)
		{
			if (!value.m_bShow)
			{
				continue;
			}
			Vector2 vector = m_GameScene.m_CameraScript.m_Camera.WorldToScreenPoint(value.m_v3Pos);
			Vector2 vector2 = vector - m_GameState.m_v3ShootCenter;
			if (m_GameScene.IsAim())
			{
				if (vector2.sqrMagnitude > 130f * (float)m_GameState.m_nHDFactor * 130f * (float)m_GameState.m_nHDFactor)
				{
					Vector2 vector3 = m_GameState.m_v3ShootCenter + vector2.normalized * 130f * m_GameState.m_nHDFactor;
					if (value.m_nWarnArrowID != -1)
					{
						m_GameSceneUI.m_WarnArrow.SetPointPos(value.m_nWarnArrowID, vector3);
					}
					else
					{
						value.m_nWarnArrowID = m_GameSceneUI.m_WarnArrow.AddPoint(vector3, Vector2.zero, new Color(1f, 1f, 0f), vector3 - m_GameState.m_v3ShootCenter);
					}
					if (value.m_nWarnIconID != -1)
					{
						m_GameSceneUI.m_WarnIcon.SetPointPos(value.m_nWarnIconID, vector3 - vector2.normalized * 17f * m_GameState.m_nHDFactor, true);
						continue;
					}
					value.m_nWarnIconID = m_GameSceneUI.m_WarnIcon.AddPoint(vector3 - vector2.normalized * 17f * m_GameState.m_nHDFactor, Vector2.zero, new Color(1f, 1f, 0f), Vector2.zero);
					m_GameSceneUI.m_WarnIcon.AddAlphaAnim(value.m_nWarnIconID, 0.05f, 0.2f, 1f);
				}
				else
				{
					if (value.m_nWarnArrowID != -1)
					{
						m_GameSceneUI.m_WarnArrow.RemovePoint(value.m_nWarnArrowID);
						value.m_nWarnArrowID = -1;
					}
					if (value.m_nWarnIconID != -1)
					{
						m_GameSceneUI.m_WarnIcon.RemovePoint(value.m_nWarnIconID);
						value.m_nWarnIconID = -1;
					}
				}
			}
			else if (vector.x < 0f || vector.x > (float)Screen.width || vector.y < 0f || vector.y > (float)Screen.height)
			{
				float magnitude = vector2.magnitude;
				Vector2 vector4 = vector2 / magnitude;
				float num = Vector2.Dot(vector4, Vector2.right);
				Vector2 vector5;
				if (num > m_GameState.m_fAngleTR || num < m_GameState.m_fAngleTL)
				{
					float num2 = vector2.x / magnitude;
					float f = (float)Screen.width * 0.5f / num2;
					vector5 = m_GameState.m_v3ShootCenter + vector4 * (Mathf.Abs(f) - 10f);
				}
				else
				{
					float num3 = vector2.y / magnitude;
					float f2 = (float)Screen.height * 0.5f / num3;
					vector5 = m_GameState.m_v3ShootCenter + vector4 * (Mathf.Abs(f2) - 10f);
				}
				if (value.m_nWarnArrowID != -1)
				{
					m_GameSceneUI.m_WarnArrow.SetPointPos(value.m_nWarnArrowID, vector5);
				}
				else
				{
					value.m_nWarnArrowID = m_GameSceneUI.m_WarnArrow.AddPoint(vector5, Vector2.zero, new Color(1f, 1f, 0f), vector5 - m_GameState.m_v3ShootCenter);
				}
				if (value.m_nWarnIconID != -1)
				{
					m_GameSceneUI.m_WarnIcon.SetPointPos(value.m_nWarnIconID, vector5 - vector2.normalized * 17f * m_GameState.m_nHDFactor, true);
					continue;
				}
				value.m_nWarnIconID = m_GameSceneUI.m_WarnIcon.AddPoint(vector5 - vector2.normalized * 17f * m_GameState.m_nHDFactor, Vector2.zero, new Color(1f, 1f, 0f), Vector2.zero);
				m_GameSceneUI.m_WarnIcon.AddAlphaAnim(value.m_nWarnIconID, 0.05f, 0.2f, 1f);
			}
			else
			{
				if (value.m_nWarnArrowID != -1)
				{
					m_GameSceneUI.m_WarnArrow.RemovePoint(value.m_nWarnArrowID);
					value.m_nWarnArrowID = -1;
				}
				if (value.m_nWarnIconID != -1)
				{
					m_GameSceneUI.m_WarnIcon.RemovePoint(value.m_nWarnIconID);
					value.m_nWarnIconID = -1;
				}
			}
		}
	}

	public int Add(Vector3 v3Pos)
	{
		TerrianWarnInfo terrianWarnInfo = new TerrianWarnInfo();
		if (terrianWarnInfo == null)
		{
			return -1;
		}
		terrianWarnInfo.m_nID = ++m_nIDCount;
		terrianWarnInfo.m_v3Pos = v3Pos;
		terrianWarnInfo.m_bShow = true;
		m_TerrianWarnMap.Add(terrianWarnInfo.m_nID, terrianWarnInfo);
		return terrianWarnInfo.m_nID;
	}

	public void Remove(int nID)
	{
		if (!m_TerrianWarnMap.ContainsKey(nID))
		{
			return;
		}
		TerrianWarnInfo terrianWarnInfo = (TerrianWarnInfo)m_TerrianWarnMap[nID];
		if (terrianWarnInfo != null)
		{
			if (terrianWarnInfo.m_nWarnArrowID != -1)
			{
				m_GameSceneUI.m_WarnArrow.RemovePoint(terrianWarnInfo.m_nWarnArrowID);
				terrianWarnInfo.m_nWarnArrowID = -1;
			}
			if (terrianWarnInfo.m_nWarnIconID != -1)
			{
				m_GameSceneUI.m_WarnIcon.RemovePoint(terrianWarnInfo.m_nWarnIconID);
				terrianWarnInfo.m_nWarnIconID = -1;
			}
			m_TerrianWarnMap.Remove(nID);
		}
	}

	public void RemoveAll()
	{
		foreach (TerrianWarnInfo value in m_TerrianWarnMap.Values)
		{
			if (value.m_nWarnArrowID != -1)
			{
				m_GameSceneUI.m_WarnArrow.RemovePoint(value.m_nWarnArrowID);
				value.m_nWarnArrowID = -1;
			}
			if (value.m_nWarnIconID != -1)
			{
				m_GameSceneUI.m_WarnIcon.RemovePoint(value.m_nWarnIconID);
				value.m_nWarnIconID = -1;
			}
		}
		m_TerrianWarnMap.Clear();
	}
}
