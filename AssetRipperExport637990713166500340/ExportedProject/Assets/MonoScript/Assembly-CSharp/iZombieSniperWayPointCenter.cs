using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class iZombieSniperWayPointCenter
{
	public Dictionary<int, WayPoint> m_WayPointConfig;

	public Dictionary<int, WayPath> m_WayPathConfig;

	public Dictionary<int, StartZone> m_ZoneConfig;

	public Dictionary<int, JumpZone> m_JumpConfig;

	public Dictionary<int, StartJumpZone> m_StartJumpConfig;

	public Rect m_FinallyZone = default(Rect);

	public Rect m_MineZone = default(Rect);

	public Dictionary<int, OilCanPoint> m_OilCanConfig;

	public void Initialize()
	{
		m_WayPointConfig = new Dictionary<int, WayPoint>();
		m_WayPathConfig = new Dictionary<int, WayPath>();
		m_ZoneConfig = new Dictionary<int, StartZone>();
		m_JumpConfig = new Dictionary<int, JumpZone>();
		m_StartJumpConfig = new Dictionary<int, StartJumpZone>();
		m_OilCanConfig = new Dictionary<int, OilCanPoint>();
	}

	public void Destroy()
	{
		if (m_WayPointConfig != null)
		{
			m_WayPointConfig.Clear();
			m_WayPointConfig = null;
		}
		if (m_WayPathConfig != null)
		{
			foreach (WayPath value in m_WayPathConfig.Values)
			{
				value.Destroy();
			}
			m_WayPathConfig.Clear();
			m_WayPathConfig = null;
		}
		if (m_ZoneConfig != null)
		{
			m_ZoneConfig.Clear();
			m_ZoneConfig = null;
		}
		if (m_JumpConfig != null)
		{
			m_JumpConfig.Clear();
			m_JumpConfig = null;
		}
		if (m_StartJumpConfig != null)
		{
			m_StartJumpConfig.Clear();
			m_StartJumpConfig = null;
		}
		if (m_OilCanConfig != null)
		{
			m_OilCanConfig.Clear();
			m_OilCanConfig = null;
		}
	}

	public WayPoint GetWayPoint(int nID)
	{
		if (m_WayPointConfig.ContainsKey(nID))
		{
			return m_WayPointConfig[nID];
		}
		return null;
	}

	public WayPointPath GetWayPath(int nTypeID, int nPathID)
	{
		if (!m_WayPathConfig.ContainsKey(nTypeID))
		{
			return null;
		}
		if (!m_WayPathConfig[nTypeID].m_WayPointPathTable.ContainsKey(nPathID))
		{
			return null;
		}
		return m_WayPathConfig[nTypeID].m_WayPointPathTable[nPathID];
	}

	public bool GetStartZoneRandomPoint(int nID, ref Vector3 v3Pos)
	{
		if (!m_ZoneConfig.ContainsKey(nID))
		{
			return false;
		}
		Rect rect = m_ZoneConfig[nID].m_Rect;
		bool flag = false;
		int num = 0;
		do
		{
			v3Pos = new Vector3(rect.xMin + UnityEngine.Random.Range(0f, rect.width), 0f, rect.yMin + UnityEngine.Random.Range(0f, rect.height));
			flag = iZombieSniperGameApp.GetInstance().m_GridInfoCenter.IsBlock(v3Pos.x, v3Pos.z);
			if (++num > 10)
			{
				return false;
			}
		}
		while (flag);
		return true;
	}

	public bool GetStartZone(int nID, ref Rect rect)
	{
		if (!m_ZoneConfig.ContainsKey(nID))
		{
			return false;
		}
		rect = m_ZoneConfig[nID].m_Rect;
		return true;
	}

	public int GetStartZoneCount()
	{
		return m_ZoneConfig.Count;
	}

	public bool GetJumpZoneRandomPoint(int nID, ref Vector3 v3Pos)
	{
		if (!m_JumpConfig.ContainsKey(nID))
		{
			return false;
		}
		Rect rect = m_JumpConfig[nID].m_Rect;
		bool flag = false;
		int num = 0;
		do
		{
			v3Pos = new Vector3(rect.xMin + UnityEngine.Random.Range(0f, rect.width), 0f, rect.yMin + UnityEngine.Random.Range(0f, rect.height));
			flag = iZombieSniperGameApp.GetInstance().m_GridInfoCenter.IsBlock(v3Pos.x, v3Pos.z);
			if (++num > 10)
			{
				return false;
			}
		}
		while (flag);
		return true;
	}

	public bool GetJumpZone(int nID, ref Rect rect)
	{
		if (!m_JumpConfig.ContainsKey(nID))
		{
			return false;
		}
		rect = m_JumpConfig[nID].m_Rect;
		return true;
	}

	public bool IsInStartJumpZone(Vector3 v3Pos)
	{
		foreach (StartJumpZone value in m_StartJumpConfig.Values)
		{
			if (Utils.PtInRect(new Vector2(v3Pos.x, v3Pos.z), value.m_Rect))
			{
				return true;
			}
		}
		return false;
	}

	public OilCanPoint GetOilCanPoint(int nID)
	{
		if (!m_OilCanConfig.ContainsKey(nID))
		{
			return null;
		}
		return m_OilCanConfig[nID];
	}

	public IEnumerable WayPointMap()
	{
		foreach (WayPoint value in m_WayPointConfig.Values)
		{
			yield return value;
		}
	}

	public void GenerateWayPath(int nPathType, int nWayPointID, ref ArrayList ltPath)
	{
		ltPath.Clear();
		ltPath.TrimToSize();
		if (!m_WayPathConfig.ContainsKey(nPathType))
		{
			return;
		}
		foreach (WayPointPath value in m_WayPathConfig[nPathType].m_WayPointPathTable.Values)
		{
			bool flag = false;
			foreach (int path in value.m_PathList)
			{
				if (path == nWayPointID)
				{
					flag = true;
				}
				if (flag)
				{
					WayPoint wayPoint = GetWayPoint(path);
					if (wayPoint != null)
					{
						ltPath.Add(wayPoint.m_v3Position);
					}
				}
			}
			if (flag)
			{
				break;
			}
		}
	}

	public void LoadWayPoint(int nStage)
	{
		XmlDocument xmlDocument = new XmlDocument();
		GameObject gameObject = Object.Instantiate(Resources.Load("ZombieSniper/Config/WayPointConfig/WayPointConfig" + nStage)) as GameObject;
		iZombieSniperWayPointConfig component = gameObject.GetComponent<iZombieSniperWayPointConfig>();
		if ((bool)component)
		{
			xmlDocument.LoadXml(component.m_XmlFile.text);
		}
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if (childNode.Name == "Point")
			{
				WayPoint wayPoint = new WayPoint();
				wayPoint.m_nID = int.Parse(xmlElement.GetAttribute("id").Trim());
				wayPoint.m_nFlag = 0;
				string attribute = xmlElement.GetAttribute("pos");
				string[] array = attribute.Split(',');
				wayPoint.m_v3Position = new Vector3(float.Parse(array[0].Trim()), 0f, float.Parse(array[1].Trim()));
				m_WayPointConfig.Add(wayPoint.m_nID, wayPoint);
			}
			else if (childNode.Name == "Path")
			{
				WayPath wayPath = new WayPath();
				wayPath.m_nID = int.Parse(xmlElement.GetAttribute("id").Trim());
				wayPath.m_WayPointPathTable = new Dictionary<int, WayPointPath>();
				foreach (XmlNode childNode2 in childNode.ChildNodes)
				{
					XmlElement xmlElement2 = (XmlElement)childNode2;
					WayPointPath wayPointPath = new WayPointPath();
					wayPointPath.m_nID = int.Parse(xmlElement2.GetAttribute("id").Trim());
					wayPointPath.m_PathList = new ArrayList();
					string attribute = xmlElement2.GetAttribute("path");
					string[] array = attribute.Split(',');
					string[] array2 = array;
					foreach (string text in array2)
					{
						WayPoint wayPoint2 = GetWayPoint(int.Parse(text.Trim()));
						if (wayPoint2 != null)
						{
							wayPoint2.m_nFlag |= 1 << wayPath.m_nID;
							wayPointPath.m_PathList.Add(wayPoint2.m_nID);
						}
					}
					wayPointPath.m_PathList.TrimToSize();
					wayPath.m_WayPointPathTable.Add(wayPointPath.m_nID, wayPointPath);
				}
				m_WayPathConfig.Add(wayPath.m_nID, wayPath);
			}
			else if (childNode.Name == "StartZone")
			{
				StartZone startZone = new StartZone();
				startZone.m_nID = int.Parse(xmlElement.GetAttribute("id").Trim());
				string attribute = xmlElement.GetAttribute("rect");
				string[] array = attribute.Split(',');
				startZone.m_Rect = new Rect(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()), float.Parse(array[3].Trim()));
				m_ZoneConfig.Add(startZone.m_nID, startZone);
			}
			else if (childNode.Name == "FinallyZone")
			{
				string attribute = xmlElement.GetAttribute("rect");
				string[] array = attribute.Split(',');
				m_FinallyZone = new Rect(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()), float.Parse(array[3].Trim()));
			}
			else if (childNode.Name == "MineZone")
			{
				string attribute = xmlElement.GetAttribute("rect");
				string[] array = attribute.Split(',');
				m_MineZone = new Rect(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()), float.Parse(array[3].Trim()));
			}
			else if (childNode.Name == "OilCan")
			{
				OilCanPoint oilCanPoint = new OilCanPoint();
				oilCanPoint.m_nID = int.Parse(xmlElement.GetAttribute("id").Trim());
				string attribute = xmlElement.GetAttribute("pos");
				string[] array = attribute.Split(',');
				oilCanPoint.m_v3Position = new Vector3(float.Parse(array[0].Trim()), 0f, float.Parse(array[1].Trim()));
				m_OilCanConfig.Add(oilCanPoint.m_nID, oilCanPoint);
			}
			else if (childNode.Name == "JumpPoint")
			{
				JumpZone jumpZone = new JumpZone();
				jumpZone.m_nID = int.Parse(xmlElement.GetAttribute("id").Trim());
				string attribute = xmlElement.GetAttribute("rect");
				string[] array = attribute.Split(',');
				jumpZone.m_Rect = new Rect(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()), float.Parse(array[3].Trim()));
				m_JumpConfig.Add(jumpZone.m_nID, jumpZone);
			}
			else if (childNode.Name == "StartJumpPoint")
			{
				StartJumpZone startJumpZone = new StartJumpZone();
				startJumpZone.m_nID = int.Parse(xmlElement.GetAttribute("id").Trim());
				string attribute = xmlElement.GetAttribute("rect");
				string[] array = attribute.Split(',');
				startJumpZone.m_Rect = new Rect(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()), float.Parse(array[3].Trim()));
				m_StartJumpConfig.Add(startJumpZone.m_nID, startJumpZone);
			}
		}
	}
}
