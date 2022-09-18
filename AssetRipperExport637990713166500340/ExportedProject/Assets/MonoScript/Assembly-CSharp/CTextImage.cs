using System.Collections;
using UnityEngine;

public class CTextImage
{
	public enum enAlignStyle
	{
		left = 0,
		center = 1,
		right = 2
	}

	public UIManager m_UIManagerRef;

	protected string m_sText;

	protected enAlignStyle m_AlignStyle;

	protected Rect m_Rect;

	protected int m_nLayer;

	protected Color m_Color;

	protected Hashtable m_TextImageInfoMap;

	protected ArrayList m_ImageList;

	~CTextImage()
	{
	}

	public void Initialize(UIManager UIManagerRef, TextImageInfo[] imageInfoList, int nLayer = 5)
	{
		m_UIManagerRef = UIManagerRef;
		m_ImageList = new ArrayList();
		m_TextImageInfoMap = new Hashtable();
		foreach (TextImageInfo textImageInfo in imageInfoList)
		{
			m_TextImageInfoMap.Add(textImageInfo.m_c, textImageInfo);
		}
		m_nLayer = nLayer;
		m_Color = Color.white;
	}

	public void SetColor(Color color)
	{
		m_Color = color;
	}

	public void Destroy()
	{
		if (m_ImageList != null)
		{
			foreach (UIImage image in m_ImageList)
			{
				if (image != null)
				{
					m_UIManagerRef.Remove(image);
				}
			}
			m_ImageList.Clear();
			m_ImageList = null;
		}
		if (m_TextImageInfoMap != null)
		{
			m_TextImageInfoMap.Clear();
			m_TextImageInfoMap = null;
		}
	}

	public void SetText(string str, Color color)
	{
		m_sText = str;
		UpdateText();
	}

	public void SetRect(Rect rect)
	{
		m_Rect = rect;
	}

	public void SetTextAlign(enAlignStyle align)
	{
		m_AlignStyle = align;
	}

	private void UpdateText()
	{
		int num = 0;
		string sText = m_sText;
		foreach (char c in sText)
		{
			if (m_TextImageInfoMap.ContainsKey(c))
			{
				num++;
			}
		}
		int j;
		for (j = m_ImageList.Count; j < num; j++)
		{
			UIImage uIImage = new UIImage();
			uIImage.Layer = m_nLayer;
			uIImage.SetColor(m_Color);
			m_UIManagerRef.Add(uIImage);
			m_ImageList.Add(uIImage);
		}
		while (j > num)
		{
			int index = j - 1;
			UIImage control = (UIImage)m_ImageList[index];
			m_UIManagerRef.Remove(control);
			m_ImageList.RemoveAt(index);
			j--;
		}
		int num2 = 0;
		int num3 = 0;
		Vector2 zero = Vector2.zero;
		while (num2 < m_sText.Length && num3 < m_ImageList.Count)
		{
			char c2 = m_sText[num2];
			if (!m_TextImageInfoMap.ContainsKey(c2))
			{
				num2++;
				zero.x += 5 * ((!Utils.IsRetina()) ? 1 : 2);
				continue;
			}
			UIImage uIImage2 = (UIImage)m_ImageList[num3];
			if (uIImage2 == null)
			{
				break;
			}
			TextImageInfo textImageInfo = (TextImageInfo)m_TextImageInfoMap[c2];
			uIImage2.Visible = true;
			uIImage2.SetTexture(textImageInfo.m_Material, textImageInfo.m_Rect);
			uIImage2.SetPosition(zero);
			zero.x += uIImage2.GetTextureSize().x;
			num2++;
			num3++;
		}
		Vector2 vector = Vector2.zero;
		switch (m_AlignStyle)
		{
		case enAlignStyle.center:
			vector = new Vector2((m_Rect.xMax - zero.x) / 2f, m_Rect.yMax);
			break;
		case enAlignStyle.left:
			vector = new Vector2(m_Rect.xMin, m_Rect.yMax);
			break;
		case enAlignStyle.right:
			vector = new Vector2(m_Rect.xMax - zero.x, m_Rect.yMax);
			break;
		}
		foreach (UIImage image in m_ImageList)
		{
			image.SetPosition(image.GetPosition() + vector);
		}
	}

	public void Show(bool bShow)
	{
		foreach (UIImage image in m_ImageList)
		{
			image.Visible = bShow;
		}
	}
}
