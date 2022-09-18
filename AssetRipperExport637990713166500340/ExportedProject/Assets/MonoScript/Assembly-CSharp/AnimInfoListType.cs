public class AnimInfoListType
{
	public int m_nNpcType;

	public AnimInfoList[] m_AnimListArray;

	public AnimInfoListType(int nNpcType)
	{
		m_nNpcType = nNpcType;
		m_AnimListArray = new AnimInfoList[13];
	}

	public void Add(int nActionType, AnimInfo info)
	{
		if (m_AnimListArray[nActionType] == null)
		{
			m_AnimListArray[nActionType] = new AnimInfoList(nActionType);
		}
		m_AnimListArray[nActionType].Add(info);
	}

	public void End()
	{
		for (int i = 0; i < m_AnimListArray.Length; i++)
		{
			if (m_AnimListArray[i] != null)
			{
				m_AnimListArray[i].End();
			}
		}
	}

	public AnimInfoList GetInfo(int nActionType)
	{
		if (m_AnimListArray[nActionType] == null)
		{
			return null;
		}
		return m_AnimListArray[nActionType];
	}
}
