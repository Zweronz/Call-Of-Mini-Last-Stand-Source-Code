using UnityEngine;

public class TextImageInfo
{
	public char m_c;

	public Material m_Material;

	public Rect m_Rect;

	public TextImageInfo(char c, Material material, Rect rect)
	{
		m_c = c;
		m_Material = material;
		m_Rect = rect;
	}
}
