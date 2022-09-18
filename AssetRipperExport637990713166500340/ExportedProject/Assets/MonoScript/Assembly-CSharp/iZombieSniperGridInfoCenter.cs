using System.Xml;
using UnityEngine;

public class iZombieSniperGridInfoCenter
{
	public int m_iGridCountX;

	public int m_iGridCountZ;

	public float m_fGridWidth;

	public Vector2 m_v2Anchor;

	public bool[,] m_arrGridBlock;

	public void Initialize()
	{
	}

	public void Destroy()
	{
		m_arrGridBlock = null;
	}

	public void Update(float deltaTime)
	{
		for (int i = 0; i < m_iGridCountZ; i++)
		{
			for (int j = 0; j < m_iGridCountX; j++)
			{
				if (IsBlock(j, i))
				{
					Vector2 vector = default(Vector2);
					GridToWorldPos(j, i, ref vector.x, ref vector.y);
					Debug.DrawLine(new Vector3(vector.x - m_fGridWidth, 0f, vector.y - m_fGridWidth), new Vector3(vector.x + m_fGridWidth, 0f, vector.y - m_fGridWidth));
					Debug.DrawLine(new Vector3(vector.x - m_fGridWidth, 0f, vector.y - m_fGridWidth), new Vector3(vector.x - m_fGridWidth, 0f, vector.y + m_fGridWidth));
					if (!IsBlock(j + 1, i))
					{
						Debug.DrawLine(new Vector3(vector.x + m_fGridWidth, 0f, vector.y - m_fGridWidth), new Vector3(vector.x + m_fGridWidth, 0f, vector.y + m_fGridWidth));
					}
					if (!IsBlock(j, i + 1))
					{
						Debug.DrawLine(new Vector3(vector.x - m_fGridWidth, 0f, vector.y + m_fGridWidth), new Vector3(vector.x + m_fGridWidth, 0f, vector.y + m_fGridWidth));
					}
				}
			}
		}
	}

	public bool GridToWorldPos(int iGridX, int iGridZ, ref float fX, ref float fZ)
	{
		if (iGridX < 0 || iGridX >= m_iGridCountX)
		{
			return false;
		}
		if (iGridZ < 0 || iGridZ >= m_iGridCountZ)
		{
			return false;
		}
		fX = (float)iGridX * m_fGridWidth + m_fGridWidth / 2f - m_v2Anchor.x;
		fZ = (float)iGridZ * m_fGridWidth + m_fGridWidth / 2f - m_v2Anchor.y;
		return true;
	}

	public bool WorldPosToGrid(float fX, float fZ, ref int iGridX, ref int iGridZ)
	{
		int num = (int)Mathf.Floor(fX + m_v2Anchor.x / m_fGridWidth);
		if (num < 0 || num >= m_iGridCountX)
		{
			return false;
		}
		int num2 = (int)Mathf.Floor(fZ + m_v2Anchor.y / m_fGridWidth);
		if (num2 < 0 || num2 >= m_iGridCountZ)
		{
			return false;
		}
		iGridX = num;
		iGridZ = num2;
		return true;
	}

	public bool IsBlock(int iGridX, int iGridZ)
	{
		if (m_arrGridBlock == null)
		{
			return true;
		}
		if (iGridX < 0 || iGridX >= m_iGridCountX)
		{
			return true;
		}
		if (iGridZ < 0 || iGridZ >= m_iGridCountZ)
		{
			return true;
		}
		return !m_arrGridBlock[iGridX, iGridZ];
	}

	public bool IsBlock(float x, float z)
	{
		int iGridX = (int)Mathf.Floor((x + m_v2Anchor.x) / m_fGridWidth);
		int iGridZ = (int)Mathf.Floor((z + m_v2Anchor.y) / m_fGridWidth);
		return IsBlock(iGridX, iGridZ);
	}

	public bool IsBlock(Vector2 v2Pos)
	{
		return IsBlock(v2Pos.x, v2Pos.y);
	}

	public void LoadData(int nStage)
	{
		XmlDocument xmlDocument = new XmlDocument();
		GameObject gameObject = Object.Instantiate(Resources.Load("ZombieSniper/Config/GridInfoConfig/GridInfoConfig" + nStage)) as GameObject;
		iZombieSniperGridInfoConfig component = gameObject.GetComponent<iZombieSniperGridInfoConfig>();
		if ((bool)component)
		{
			xmlDocument.LoadXml(component.m_XmlFile.ToString());
		}
		Object.Destroy(gameObject);
		XmlNode documentElement = xmlDocument.DocumentElement;
		XmlElement xmlElement = (XmlElement)documentElement;
		m_iGridCountX = int.Parse(xmlElement.GetAttribute("widthcount").Trim());
		m_iGridCountZ = int.Parse(xmlElement.GetAttribute("heightcount").Trim());
		m_fGridWidth = float.Parse(xmlElement.GetAttribute("width").Trim());
		string text = xmlElement.GetAttribute("anchor").Trim();
		string[] array = text.Split(',');
		m_v2Anchor = new Vector2(float.Parse(array[0]), float.Parse(array[1]));
		m_arrGridBlock = new bool[m_iGridCountX, m_iGridCountZ];
		Debug.Log("gridblock total size = " + m_iGridCountX * m_iGridCountZ);
		int num = 0;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			XmlElement xmlElement2 = (XmlElement)childNode;
			if (childNode.Name == "node")
			{
				text = xmlElement2.GetAttribute("x").Trim();
				array = text.Split(',');
				for (int i = 0; i < array.Length; i++)
				{
					m_arrGridBlock[i, num] = ((int.Parse(array[i]) == 0) ? true : false);
				}
			}
			num++;
			if (num >= m_iGridCountZ)
			{
				break;
			}
		}
	}
}
