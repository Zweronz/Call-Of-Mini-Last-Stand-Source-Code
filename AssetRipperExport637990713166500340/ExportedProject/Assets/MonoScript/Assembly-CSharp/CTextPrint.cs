public class CTextPrint
{
	private enum State
	{
		None = 0,
		Print = 1,
		Hold = 2,
		Fade = 3
	}

	private State m_State;

	private bool m_bFinished;

	private UIText m_Text;

	private string m_sContext;

	private int m_nContextIndex;

	private float m_fInterval;

	private float m_fFadeTime;

	private float m_fTimeCount;

	public bool IsFinished
	{
		get
		{
			return m_bFinished;
		}
	}

	public void Initialize(UIText uitext, float fInterval, float fFadeTime)
	{
		m_Text = uitext;
		m_fInterval = fInterval;
		m_fFadeTime = fFadeTime;
		Reset();
	}

	public void Reset()
	{
		m_Text.Visible = false;
		m_State = State.Print;
		m_bFinished = true;
	}

	public void Start(string str)
	{
		m_Text.SetText(str);
		Start();
	}

	public void Start()
	{
		m_Text.Visible = true;
		m_sContext = m_Text.GetText();
		m_nContextIndex = 0;
		m_Text.SetText(string.Empty);
		m_fTimeCount = 0f;
		m_State = State.Print;
		m_bFinished = false;
	}

	public void Update(float deltaTime)
	{
		if (m_bFinished)
		{
			return;
		}
		if (m_State == State.Print)
		{
			m_fTimeCount += deltaTime;
			if (m_fTimeCount < m_fInterval)
			{
				return;
			}
			m_fTimeCount = 0f;
			string text = string.Empty;
			do
			{
				text += m_sContext.Substring(m_nContextIndex++, 1);
				if (m_nContextIndex >= m_sContext.Length)
				{
					m_State = State.Hold;
					m_fTimeCount = 0f;
					break;
				}
			}
			while (text.Trim() == string.Empty);
			if (text != string.Empty)
			{
				m_Text.SetText(m_Text.GetText() + text);
			}
		}
		else if (m_State == State.Hold)
		{
			m_fTimeCount += deltaTime;
			if (!(m_fTimeCount < m_fFadeTime))
			{
				m_State = State.Fade;
				m_fTimeCount = 0f;
			}
		}
		else if (m_State == State.Fade)
		{
			m_fTimeCount += deltaTime;
			if (m_fTimeCount > m_fFadeTime)
			{
				m_fTimeCount = m_fFadeTime;
				m_State = State.None;
				m_bFinished = true;
				m_Text.Visible = false;
			}
			m_Text.SetAlpha(1f - m_fTimeCount / m_fFadeTime);
		}
	}

	public void SetPrintEnd()
	{
		if (m_Text != null)
		{
			if (m_State == State.Print)
			{
				m_Text.SetText(m_sContext);
				m_State = State.Hold;
				m_fTimeCount = 0f;
			}
			else if (m_State == State.Hold || m_State == State.Fade)
			{
				m_State = State.None;
				m_bFinished = true;
				m_Text.Visible = false;
			}
		}
	}
}
