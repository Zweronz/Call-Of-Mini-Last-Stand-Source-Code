using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CSinglePara : MonoBehaviour
{
	public bool m_bDrawGizmos = true;

	public Color m_GizmosColor = Color.white;

	public List<Transform> m_ltPoint;

	private void OnDrawGizmos()
	{
		if (!m_bDrawGizmos || m_ltPoint == null)
		{
			return;
		}
		Gizmos.color = m_GizmosColor;
		for (int i = 0; i < m_ltPoint.Count; i++)
		{
			if (!(m_ltPoint[i] == null))
			{
				Gizmos.DrawSphere(m_ltPoint[i].transform.position, 1f);
			}
		}
	}

	public void AddPoint(GameObject point)
	{
		if (m_ltPoint == null)
		{
			m_ltPoint = new List<Transform>();
		}
		point.transform.parent = base.transform;
		m_ltPoint.Add(point.transform);
	}

	public void RefreshPointSequence()
	{
		m_ltPoint.RemoveAll(IsNull);
		for (int i = 0; i < m_ltPoint.Count; i++)
		{
			if (!(m_ltPoint[i] == null))
			{
				m_ltPoint[i].name = "Point_" + i;
			}
		}
	}

	private static bool IsNull(Transform tf)
	{
		return tf == null;
	}
}
