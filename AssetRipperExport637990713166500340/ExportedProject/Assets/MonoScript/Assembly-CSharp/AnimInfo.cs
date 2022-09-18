public class AnimInfo
{
	public string m_sAnimName;

	public int m_nNormalRate;

	public float m_fNormalSpeed;

	public AnimInfo(string sAnimName, float speed, int rate)
	{
		m_sAnimName = sAnimName;
		m_fNormalSpeed = speed;
		m_nNormalRate = rate;
	}
}
