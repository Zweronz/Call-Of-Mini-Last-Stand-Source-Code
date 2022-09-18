using System.Collections;

public class ZombieGroupInfo
{
	public int m_nGroupID;

	public ArrayList m_GroupUnitList;

	public void Destroy()
	{
		if (m_GroupUnitList != null)
		{
			m_GroupUnitList.Clear();
			m_GroupUnitList = null;
		}
	}
}
