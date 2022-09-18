using System.Collections;
using UnityEngine;

public class AnimInfoList
{
	public int m_nAnimType;

	public ArrayList m_AnimList;

	public AnimInfoList(int nActionType)
	{
		m_nAnimType = nActionType;
		m_AnimList = new ArrayList();
	}

	public void Add(AnimInfo info)
	{
		m_AnimList.Add(info);
	}

	public void End()
	{
		if (m_AnimList != null)
		{
			m_AnimList.TrimToSize();
		}
	}

	public AnimInfo GetInfo(int nIndex = -1)
	{
		int count = m_AnimList.Count;
		if (count <= 0)
		{
			return null;
		}
		if (nIndex != -1 && (nIndex < 0 || nIndex > count - 1))
		{
			return null;
		}
		if (nIndex == -1)
		{
			nIndex = UnityEngine.Random.Range(0, count);
		}
		return (AnimInfo)m_AnimList[nIndex];
	}
}
