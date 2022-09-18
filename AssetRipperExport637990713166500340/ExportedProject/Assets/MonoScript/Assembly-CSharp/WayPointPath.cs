using System.Collections;

public class WayPointPath
{
	public int m_nID;

	public ArrayList m_PathList;

	public WayPointPath()
	{
		m_nID = -1;
		m_PathList = null;
	}

	public void Destroy()
	{
		if (m_PathList != null)
		{
			m_PathList.Clear();
			m_PathList = null;
		}
	}
}
