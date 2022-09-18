using UnityEngine;

public class LogicWaveInfo
{
	public int m_nID;

	public int m_nZombieID;

	public int m_nFormulaID;

	public int m_nStartPoint;

	public float[] m_arrTimeStart;

	public float[] m_arrTimeLoop;

	public LogicWaveInfo()
	{
		m_arrTimeStart = new float[2];
		m_arrTimeLoop = new float[2];
	}

	public float GetStartTime()
	{
		return UnityEngine.Random.Range(m_arrTimeStart[0], m_arrTimeStart[1]);
	}

	public float GetLoopTime()
	{
		return UnityEngine.Random.Range(m_arrTimeLoop[0], m_arrTimeLoop[1]);
	}
}
