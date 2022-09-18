using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class iZombieSniperIconCenter
{
	public Dictionary<int, IconInfo> m_IconMap;

	public void Initialize()
	{
		m_IconMap = new Dictionary<int, IconInfo>();
		m_IconMap.Clear();
		LoadIconCfg();
	}

	private void LoadIconCfg()
	{
		XmlDocument xmlDocument = new XmlDocument();
		string text = "ZombieSniper/Config/IconCfg";
		GameObject gameObject = Object.Instantiate(Resources.Load(text)) as GameObject;
		if (gameObject == null)
		{
			Debug.Log("can't find resources " + text);
			return;
		}
		iZombieSniperIconConfig component = gameObject.GetComponent<iZombieSniperIconConfig>();
		if (component == null)
		{
			Debug.Log("can't find script component iZombieSniperIconConfig");
			return;
		}
		xmlDocument.LoadXml(component.m_XmlIconCfg.text);
		Object.Destroy(gameObject);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (!(childNode.Name != "icon"))
			{
				XmlElement xmlElement = (XmlElement)childNode;
				IconInfo iconInfo = new IconInfo();
				iconInfo.m_nID = int.Parse(xmlElement.GetAttribute("id").Trim());
				iconInfo.m_sMaterial = xmlElement.GetAttribute("material").Trim();
				string text2 = xmlElement.GetAttribute("rect").Trim();
				string[] array = text2.Split(',');
				iconInfo.m_Rect = new Rect(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
				m_IconMap.Add(iconInfo.m_nID, iconInfo);
			}
		}
	}

	public IconInfo GetIcon(int nID)
	{
		if (!m_IconMap.ContainsKey(nID))
		{
			return null;
		}
		return m_IconMap[nID];
	}
}
