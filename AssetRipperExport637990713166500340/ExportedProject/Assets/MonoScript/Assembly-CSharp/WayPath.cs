using System.Collections.Generic;

public class WayPath
{
	public int m_nID;

	public Dictionary<int, WayPointPath> m_WayPointPathTable;

	public WayPath()
	{
		m_nID = -1;
		m_WayPointPathTable = null;
	}

	public void Destroy()
	{
		if (m_WayPointPathTable == null)
		{
			return;
		}
		foreach (WayPointPath value in m_WayPointPathTable.Values)
		{
			value.Destroy();
		}
		m_WayPointPathTable.Clear();
		m_WayPointPathTable = null;
	}
}
