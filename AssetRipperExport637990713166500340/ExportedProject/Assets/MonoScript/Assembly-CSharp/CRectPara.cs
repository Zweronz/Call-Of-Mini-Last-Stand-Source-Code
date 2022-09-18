using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CRectPara : MonoBehaviour
{
	public bool m_bDrawGizmos = true;

	public Color m_GizmosColor = Color.white;

	public List<Transform> m_ltRect;

	private void OnDrawGizmos()
	{
		if (!m_bDrawGizmos || m_ltRect == null)
		{
			return;
		}
		Gizmos.color = m_GizmosColor;
		CRect cRect = null;
		for (int i = 0; i < m_ltRect.Count; i++)
		{
			if (!(m_ltRect[i] == null))
			{
				cRect = m_ltRect[i].GetComponent<CRect>();
				if (!(cRect == null))
				{
					Gizmos.DrawCube(m_ltRect[i].transform.position, new Vector3(cRect.WidthX, 0f, cRect.WidthZ));
				}
			}
		}
	}

	public void AddRect(GameObject rect)
	{
		if (m_ltRect == null)
		{
			m_ltRect = new List<Transform>();
		}
		rect.transform.parent = base.transform;
		m_ltRect.Add(rect.transform);
	}

	public void RefreshRectSequence()
	{
		m_ltRect.RemoveAll(IsNull);
		for (int i = 0; i < m_ltRect.Count; i++)
		{
			if (!(m_ltRect[i] == null))
			{
				m_ltRect[i].name = "Rect_" + i;
			}
		}
	}

	private static bool IsNull(Transform tf)
	{
		return tf == null;
	}
}
