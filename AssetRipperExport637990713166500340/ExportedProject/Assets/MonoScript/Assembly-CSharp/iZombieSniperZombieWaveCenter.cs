using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class iZombieSniperZombieWaveCenter
{
	public Dictionary<int, ZombieBaseInfo> m_ZombieBaseInfoMap;

	public Dictionary<int, ZombieGroupInfo> m_ZombieGroupInfoMap;

	public ArrayList m_ZombieWaveInfoList;

	public LogicWaveGlobal m_LogicWaveGlobal;

	public Dictionary<int, LogicWaveFormula> m_dictLogicWaveFormula;

	public Dictionary<int, LogicWaveInfo> m_dictLogicWaveInfo;

	public void Initialize()
	{
		m_ZombieBaseInfoMap = new Dictionary<int, ZombieBaseInfo>();
		m_ZombieBaseInfoMap.Clear();
		m_ZombieGroupInfoMap = new Dictionary<int, ZombieGroupInfo>();
		m_ZombieGroupInfoMap.Clear();
		m_ZombieWaveInfoList = new ArrayList();
		m_ZombieWaveInfoList.Clear();
		m_LogicWaveGlobal = new LogicWaveGlobal();
		m_dictLogicWaveFormula = new Dictionary<int, LogicWaveFormula>();
		m_dictLogicWaveFormula.Clear();
		m_dictLogicWaveInfo = new Dictionary<int, LogicWaveInfo>();
		m_dictLogicWaveInfo.Clear();
	}

	public void Destroy()
	{
		if (m_ZombieBaseInfoMap != null)
		{
			m_ZombieBaseInfoMap.Clear();
			m_ZombieBaseInfoMap = null;
		}
		if (m_ZombieGroupInfoMap != null)
		{
			foreach (ZombieGroupInfo value in m_ZombieGroupInfoMap.Values)
			{
				value.Destroy();
			}
			m_ZombieGroupInfoMap.Clear();
			m_ZombieGroupInfoMap = null;
		}
		if (m_ZombieWaveInfoList != null)
		{
			m_ZombieWaveInfoList.Clear();
			m_ZombieWaveInfoList = null;
		}
		m_LogicWaveGlobal = null;
		if (m_dictLogicWaveFormula != null)
		{
			m_dictLogicWaveFormula.Clear();
			m_dictLogicWaveFormula = null;
		}
		if (m_dictLogicWaveInfo != null)
		{
			m_dictLogicWaveInfo.Clear();
			m_dictLogicWaveInfo = null;
		}
	}

	public ZombieBaseInfo GetZombieBaseInfo(int nID)
	{
		if (!m_ZombieBaseInfoMap.ContainsKey(nID))
		{
			return null;
		}
		return m_ZombieBaseInfoMap[nID];
	}

	public ZombieGroupInfo GetZombieGroupInfo(int nGroupID)
	{
		if (!m_ZombieGroupInfoMap.ContainsKey(nGroupID))
		{
			return null;
		}
		return m_ZombieGroupInfoMap[nGroupID];
	}

	public LogicWaveInfo GetLogicWaveInfo(int nID)
	{
		if (!m_dictLogicWaveInfo.ContainsKey(nID))
		{
			return null;
		}
		return m_dictLogicWaveInfo[nID];
	}

	public int CaculateZombieNum(int nFomulaID, float fGameTotalTime)
	{
		if (!m_dictLogicWaveFormula.ContainsKey(nFomulaID))
		{
			return 0;
		}
		LogicWaveFormula logicWaveFormula = m_dictLogicWaveFormula[nFomulaID];
		float num = logicWaveFormula.a * Mathf.Pow(fGameTotalTime / 60f - logicWaveFormula.b, logicWaveFormula.k) + logicWaveFormula.c;
		Debug.Log("a = " + logicWaveFormula.a + " b = " + logicWaveFormula.b + " c = " + logicWaveFormula.c + " k = " + logicWaveFormula.k);
		return (int)num;
	}

	public void LoadData(int nStage)
	{
		XmlDocument xmlDocument = new XmlDocument();
		GameObject gameObject = Object.Instantiate(Resources.Load("ZombieSniper/Config/ZombieWaveConfig/ZombieWaveConfig" + nStage)) as GameObject;
		iZombieSniperZombieWaveConfig component = gameObject.GetComponent<iZombieSniperZombieWaveConfig>();
		if ((bool)component)
		{
			xmlDocument.LoadXml(component.m_XmlFile.ToString());
		}
		Object.Destroy(gameObject);
		XmlNode documentElement = xmlDocument.DocumentElement;
		string empty = string.Empty;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (childNode.Name == "Zombie")
			{
				foreach (XmlNode childNode2 in childNode.ChildNodes)
				{
					XmlElement xmlElement = (XmlElement)childNode2;
					ZombieBaseInfo zombieBaseInfo = new ZombieBaseInfo();
					zombieBaseInfo.m_nID = int.Parse(xmlElement.GetAttribute("id").Trim());
					zombieBaseInfo.m_nType = int.Parse(xmlElement.GetAttribute("type").Trim());
					zombieBaseInfo.m_nAiType = int.Parse(xmlElement.GetAttribute("aitype").Trim());
					zombieBaseInfo.m_fLife = float.Parse(xmlElement.GetAttribute("life").Trim());
					zombieBaseInfo.m_fSize = float.Parse(xmlElement.GetAttribute("size").Trim());
					zombieBaseInfo.m_fMoveSpeed = float.Parse(xmlElement.GetAttribute("movespeed").Trim());
					zombieBaseInfo.m_fDamage = float.Parse(xmlElement.GetAttribute("damage").Trim());
					zombieBaseInfo.m_fAtkSpeed = float.Parse(xmlElement.GetAttribute("atkSpeed").Trim());
					zombieBaseInfo.m_fAtkRange = float.Parse(xmlElement.GetAttribute("atkRange").Trim());
					zombieBaseInfo.m_fWarnRange = float.Parse(xmlElement.GetAttribute("warnRange").Trim());
					zombieBaseInfo.m_fFreezeTime = float.Parse(xmlElement.GetAttribute("freezeTime").Trim());
					m_ZombieBaseInfoMap.Add(zombieBaseInfo.m_nID, zombieBaseInfo);
				}
			}
			else
			{
				if (!(childNode.Name == "ZombieGroup"))
				{
					continue;
				}
				foreach (XmlNode childNode3 in childNode.ChildNodes)
				{
					XmlElement xmlElement2 = (XmlElement)childNode3;
					ZombieGroupInfo zombieGroupInfo = new ZombieGroupInfo();
					zombieGroupInfo.m_nGroupID = int.Parse(xmlElement2.GetAttribute("id").Trim());
					zombieGroupInfo.m_GroupUnitList = new ArrayList();
					foreach (XmlNode childNode4 in childNode3.ChildNodes)
					{
						XmlElement xmlElement3 = (XmlElement)childNode4;
						ZombieGroupUnit zombieGroupUnit = new ZombieGroupUnit();
						zombieGroupUnit.m_nZombieID = int.Parse(xmlElement3.GetAttribute("zombieid").Trim());
						empty = xmlElement3.GetAttribute("time").Trim();
						string[] array = empty.Split(':');
						if (array.Length > 1)
						{
							zombieGroupUnit.m_fTimeDelay = float.Parse(array[0].Trim()) * 60f + float.Parse(array[1].Trim());
						}
						else
						{
							zombieGroupUnit.m_fTimeDelay = float.Parse(array[0].Trim());
						}
						zombieGroupInfo.m_GroupUnitList.Add(zombieGroupUnit);
					}
					zombieGroupInfo.m_GroupUnitList.TrimToSize();
					m_ZombieGroupInfoMap.Add(zombieGroupInfo.m_nGroupID, zombieGroupInfo);
				}
			}
		}
	}

	public void LoadZombieWaveInfoList(int nStage)
	{
		XmlDocument xmlDocument = new XmlDocument();
		GameObject gameObject = Object.Instantiate(Resources.Load("ZombieSniper/Config/ZombieWaveConfig/ZombieWaveConfig" + nStage)) as GameObject;
		iZombieSniperZombieWaveConfig component = gameObject.GetComponent<iZombieSniperZombieWaveConfig>();
		if ((bool)component)
		{
			xmlDocument.LoadXml(component.m_XmlFile.ToString());
		}
		Object.Destroy(gameObject);
		XmlNode documentElement = xmlDocument.DocumentElement;
		string empty = string.Empty;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (!(childNode.Name == "ZombieWave"))
			{
				continue;
			}
			m_ZombieWaveInfoList.Clear();
			foreach (XmlNode childNode2 in childNode.ChildNodes)
			{
				XmlElement xmlElement = (XmlElement)childNode2;
				ZombieWaveInfo zombieWaveInfo = new ZombieWaveInfo();
				zombieWaveInfo.m_nGroupID = int.Parse(xmlElement.GetAttribute("groupid").Trim());
				zombieWaveInfo.m_nStartZone = int.Parse(xmlElement.GetAttribute("startzone").Trim());
				empty = xmlElement.GetAttribute("time").Trim();
				string[] array = empty.Split(':');
				if (array.Length > 1)
				{
					zombieWaveInfo.m_fTimePoint = float.Parse(array[0].Trim()) * 60f + float.Parse(array[1].Trim());
				}
				else
				{
					zombieWaveInfo.m_fTimePoint = float.Parse(array[0].Trim());
				}
				empty = xmlElement.GetAttribute("loop").Trim();
				array = empty.Split(':');
				if (array.Length > 1)
				{
					zombieWaveInfo.m_fLoopTime = float.Parse(array[0].Trim()) * 60f + float.Parse(array[1].Trim());
				}
				else
				{
					zombieWaveInfo.m_fLoopTime = float.Parse(array[0].Trim());
				}
				m_ZombieWaveInfoList.Add(zombieWaveInfo);
			}
			m_ZombieWaveInfoList.TrimToSize();
		}
	}

	public void LoadLogicWaveInfo(int nStage)
	{
		XmlDocument xmlDocument = new XmlDocument();
		GameObject gameObject = Object.Instantiate(Resources.Load("ZombieSniper/Config/ZombieWaveConfig/ZombieWaveLogicConfig" + nStage)) as GameObject;
		iZombieSniperZombieWaveLogicConfig component = gameObject.GetComponent<iZombieSniperZombieWaveLogicConfig>();
		if ((bool)component)
		{
			xmlDocument.LoadXml(component.m_XmlFile.ToString());
		}
		Object.Destroy(gameObject);
		XmlNode documentElement = xmlDocument.DocumentElement;
		string empty = string.Empty;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (childNode.Name == "global")
			{
				XmlElement xmlElement = (XmlElement)childNode;
				empty = xmlElement.GetAttribute("maxnum").Trim();
				if (empty.Length > 0)
				{
					m_LogicWaveGlobal.maxnum = int.Parse(empty);
				}
				empty = xmlElement.GetAttribute("maxtime").Trim();
				if (empty.Length > 0)
				{
					m_LogicWaveGlobal.maxtime = float.Parse(empty);
				}
			}
			else if (childNode.Name == "formula")
			{
				m_dictLogicWaveFormula.Clear();
				foreach (XmlNode childNode2 in childNode.ChildNodes)
				{
					XmlElement xmlElement2 = (XmlElement)childNode2;
					LogicWaveFormula logicWaveFormula = new LogicWaveFormula();
					logicWaveFormula.id = int.Parse(xmlElement2.GetAttribute("id").Trim());
					empty = xmlElement2.GetAttribute("x1").Trim();
					if (empty.Length > 0)
					{
						logicWaveFormula.x1 = float.Parse(empty);
					}
					empty = xmlElement2.GetAttribute("x2").Trim();
					if (empty.Length > 0)
					{
						logicWaveFormula.x2 = float.Parse(empty);
					}
					empty = xmlElement2.GetAttribute("y1").Trim();
					if (empty.Length > 0)
					{
						logicWaveFormula.y1 = float.Parse(empty);
					}
					empty = xmlElement2.GetAttribute("y2").Trim();
					if (empty.Length > 0)
					{
						logicWaveFormula.y2 = float.Parse(empty);
					}
					empty = xmlElement2.GetAttribute("c").Trim();
					if (empty.Length > 0)
					{
						logicWaveFormula.c = float.Parse(empty);
					}
					empty = xmlElement2.GetAttribute("k").Trim();
					if (empty.Length > 0)
					{
						logicWaveFormula.k = float.Parse(empty);
					}
					logicWaveFormula.Initialize();
					m_dictLogicWaveFormula.Add(logicWaveFormula.id, logicWaveFormula);
				}
			}
			else
			{
				if (!(childNode.Name == "logicwave"))
				{
					continue;
				}
				m_dictLogicWaveInfo.Clear();
				foreach (XmlNode childNode3 in childNode.ChildNodes)
				{
					XmlElement xmlElement3 = (XmlElement)childNode3;
					LogicWaveInfo logicWaveInfo = new LogicWaveInfo();
					logicWaveInfo.m_nID = int.Parse(xmlElement3.GetAttribute("id").Trim());
					empty = xmlElement3.GetAttribute("zombieid").Trim();
					if (empty.Length > 0)
					{
						logicWaveInfo.m_nZombieID = int.Parse(empty);
					}
					empty = xmlElement3.GetAttribute("formulaid").Trim();
					if (empty.Length > 0)
					{
						logicWaveInfo.m_nFormulaID = int.Parse(empty);
					}
					empty = xmlElement3.GetAttribute("startpoint").Trim();
					if (empty.Length > 0)
					{
						logicWaveInfo.m_nStartPoint = int.Parse(empty);
					}
					empty = xmlElement3.GetAttribute("starttime").Trim();
					if (empty.Length > 0)
					{
						string[] array = empty.Split(',');
						if (array.Length > 1)
						{
							logicWaveInfo.m_arrTimeStart[0] = float.Parse(array[0]);
							logicWaveInfo.m_arrTimeStart[1] = float.Parse(array[1]);
						}
					}
					empty = xmlElement3.GetAttribute("looptime").Trim();
					if (empty.Length > 0)
					{
						string[] array = empty.Split(',');
						if (array.Length > 1)
						{
							logicWaveInfo.m_arrTimeLoop[0] = float.Parse(array[0]);
							logicWaveInfo.m_arrTimeLoop[1] = float.Parse(array[1]);
						}
					}
					m_dictLogicWaveInfo.Add(logicWaveInfo.m_nID, logicWaveInfo);
				}
			}
		}
	}
}
