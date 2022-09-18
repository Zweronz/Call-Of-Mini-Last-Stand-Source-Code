using UnityEngine;

public class iZombieSniperMiniMap : CNail
{
	public Vector2 m_v2MiniMapPos;

	public Vector3 m_v3MiniMapPos;

	public float m_fMiniMapRadius;

	public float m_fMiniMapScale;

	public override void Initialize(UIManager UIManagerRef, Material material, Rect rtRect)
	{
		base.Initialize(UIManagerRef, material, rtRect);
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_v2MiniMapPos = new Vector2(60f, 75f) * m_GameState.m_nHDFactor;
		m_v3MiniMapPos = ConstantValue.m_v3SniperPosition;
		m_fMiniMapRadius = 50f * (float)m_GameState.m_nHDFactor;
		m_fMiniMapScale = 0.6f * (float)m_GameState.m_nHDFactor;
	}

	public bool WorldPointToMiniMap(Vector3 v3Pos, ref Vector2 v2MiniMapPos)
	{
		v3Pos.y = 0f;
		v3Pos -= m_v3MiniMapPos;
		Vector2 vector = new Vector2(v3Pos.x, v3Pos.z);
		float magnitude = vector.magnitude;
		if (magnitude * m_fMiniMapScale > m_fMiniMapRadius)
		{
			return false;
		}
		vector /= magnitude;
		magnitude *= m_fMiniMapScale;
		v2MiniMapPos = m_v2MiniMapPos + -vector * magnitude;
		return true;
	}
}
