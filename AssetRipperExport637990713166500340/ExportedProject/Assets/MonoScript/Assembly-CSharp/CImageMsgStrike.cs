public class CImageMsgStrike
{
	private CImageMsg m_str;

	private CImageMsg m_num1;

	private CImageMsg m_num2;

	public void Initialize()
	{
		if (m_str == null)
		{
			m_str = new CImageMsg();
			m_str.Initialize();
		}
	}

	public void Destroy()
	{
	}

	public void Update()
	{
	}
}
