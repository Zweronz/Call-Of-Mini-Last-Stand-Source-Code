using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CPathPara : MonoBehaviour
{
	public WrapMode m_WrapMode = WrapMode.Once;

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
		CMoveBase cMoveBase = null;
		for (int i = 0; i < m_ltPoint.Count; i++)
		{
			if (m_ltPoint[i] == null)
			{
				continue;
			}
			cMoveBase = m_ltPoint[i].GetComponent<CMoveBase>();
			if (!(cMoveBase == null))
			{
				switch (cMoveBase.m_State)
				{
				case CMoveBase.MoveType.Stand:
					Gizmos.DrawCube(m_ltPoint[i].position, new Vector3(2f, 2f, 2f));
					break;
				case CMoveBase.MoveType.Move:
					Gizmos.DrawSphere(m_ltPoint[i].position, 1f);
					break;
				case CMoveBase.MoveType.Rotate:
					Gizmos.DrawSphere(m_ltPoint[i].position, 2f);
					break;
				}
				if (i > 0 && m_ltPoint[i - 1] != null && m_ltPoint[i] != null)
				{
					Gizmos.DrawLine(m_ltPoint[i - 1].position + new Vector3(0f, 0.2f, 0f), m_ltPoint[i].position + new Vector3(0f, 0.2f, 0f));
				}
			}
		}
	}

	public void AddPoint(GameObject point, int nIndex = -1)
	{
		if (m_ltPoint == null)
		{
			m_ltPoint = new List<Transform>();
		}
		point.transform.parent = base.transform;
		if (nIndex < 0 || nIndex >= m_ltPoint.Count)
		{
			m_ltPoint.Add(point.transform);
		}
		else
		{
			m_ltPoint.Insert(nIndex, point.transform);
		}
	}

	public void RefreshPointSequence()
	{
		m_ltPoint.RemoveAll(IsNull);
		for (int i = 0; i < m_ltPoint.Count; i++)
		{
			if (!(m_ltPoint[i] == null))
			{
				if (m_ltPoint[i].GetComponent<CMoveStand>() != null)
				{
					m_ltPoint[i].name = "Point_" + i + "_Stand";
				}
				else if (m_ltPoint[i].GetComponent<CMoveGo>() != null)
				{
					m_ltPoint[i].name = "Point_" + i + "_Go";
				}
				else if (m_ltPoint[i].GetComponent<CMoveRotate>() != null)
				{
					m_ltPoint[i].name = "Point_" + i + "_Rotate";
				}
			}
		}
	}

	private static bool IsNull(Transform tf)
	{
		return tf == null;
	}

	public void RefreshPointDirection()
	{
		for (int i = 0; i < m_ltPoint.Count; i++)
		{
			if (i > 0 && m_ltPoint[i] != null && m_ltPoint[i - 1] != null)
			{
				CMoveBase component = m_ltPoint[i].GetComponent<CMoveBase>();
				if (component != null && component.m_State == CMoveBase.MoveType.Move)
				{
					m_ltPoint[i - 1].transform.forward = m_ltPoint[i].position - m_ltPoint[i - 1].position;
				}
			}
		}
	}
}
