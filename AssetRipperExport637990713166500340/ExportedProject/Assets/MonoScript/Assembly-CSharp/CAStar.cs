using System.Collections;

public class CAStar
{
	public int m_iStartX;

	public int m_iStartZ;

	public int m_iEndX;

	public int m_iEndZ;

	public ArrayList m_ltPath;

	public iZombieSniperGridInfoCenter m_GridInfoCenter;

	public void Initialize()
	{
		m_GridInfoCenter = iZombieSniperGameApp.GetInstance().m_GridInfoCenter;
	}

	public bool FindPath(int iSX, int iSZ, int iDX, int iDZ)
	{
		return false;
	}
}
