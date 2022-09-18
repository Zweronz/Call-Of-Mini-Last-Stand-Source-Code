using UnityEngine;

public class CRollText
{
	private class RollTextInfo
	{
		public enum RollState
		{
			None = 0,
			Normal = 1,
			FadeOut = 2
		}

		public UIText m_Text;

		public RollState m_RollState;

		public float m_fTimeCount;

		public void Initialize(Rect rect, string sFont, Color color)
		{
			m_Text = new UIText();
			m_Text.SetFont(sFont);
			m_Text.SetColor(color);
			m_Text.Rect = rect;
			m_Text.AlignStyle = UIText.enAlignStyle.center;
			m_Text.Layer = 5;
			m_RollState = RollState.None;
			m_fTimeCount = 0f;
		}

		public void Destroy()
		{
			m_Text = null;
			m_RollState = RollState.None;
		}

		public void Update(float deltaTime)
		{
			if (m_RollState == RollState.Normal)
			{
				m_fTimeCount += deltaTime;
				if (m_fTimeCount > 1f)
				{
					m_RollState = RollState.FadeOut;
					m_fTimeCount = 0f;
				}
			}
			else if (m_RollState == RollState.FadeOut)
			{
				float alpha = m_Text.GetAlpha();
				alpha -= 0.5f * deltaTime;
				if (alpha < 0f)
				{
					alpha = 0f;
				}
				m_Text.SetAlpha(alpha);
				if (alpha == 0f)
				{
					m_RollState = RollState.None;
					m_Text.Visible = false;
				}
			}
		}

		public void CopyData(RollTextInfo info)
		{
			m_Text.SetText(info.m_Text.GetText());
			m_fTimeCount = info.m_fTimeCount;
			m_RollState = info.m_RollState;
			m_Text.SetAlpha(info.m_Text.GetAlpha());
			m_Text.Visible = true;
		}

		public void SetText(string str)
		{
			m_Text.SetText(str);
			m_fTimeCount = 0f;
			m_RollState = RollState.Normal;
			m_Text.SetAlpha(1f);
			m_Text.Visible = true;
		}
	}

	private UIHelper m_UIHelper;

	private UIManager m_UIManager;

	private int m_nHDFactor;

	private RollTextInfo[] m_TextList;

	private int m_nMaxLine;

	public void Initialize(UIHelper uihelper, UIManager uimanager, Vector2 v2StartPos, int nWidth, int nHeight, int nMaxLine = 3)
	{
		m_UIHelper = uihelper;
		m_UIManager = uimanager;
		m_nHDFactor = ((!Utils.IsRetina()) ? 1 : 2);
		m_nMaxLine = nMaxLine;
		m_TextList = new RollTextInfo[m_nMaxLine];
		string font_path = m_UIHelper.m_font_path;
		font_path = ((m_nHDFactor != 1) ? (font_path + "037_32") : (font_path + "037_16"));
		for (int i = 0; i < m_nMaxLine; i++)
		{
			m_TextList[i] = new RollTextInfo();
			m_TextList[i].Initialize(new Rect(v2StartPos.x - (float)(nWidth / 2), v2StartPos.y + (float)(nHeight / 2) + (float)(i * nHeight), nWidth, nHeight), font_path, Color.white);
			m_UIManager.Add(m_TextList[i].m_Text);
		}
	}

	public void Destroy()
	{
		if (m_TextList != null)
		{
			for (int i = 0; i < m_TextList.Length; i++)
			{
				m_TextList[i].Destroy();
				m_TextList[i] = null;
			}
			m_TextList = null;
		}
	}

	public void AddText(string str)
	{
		for (int num = m_nMaxLine - 1; num > 0; num--)
		{
			m_TextList[num].CopyData(m_TextList[num - 1]);
		}
		m_TextList[0].SetText(str);
	}

	public void Update(float deltaTime)
	{
		for (int i = 0; i < m_nMaxLine; i++)
		{
			m_TextList[i].Update(deltaTime);
		}
	}
}
