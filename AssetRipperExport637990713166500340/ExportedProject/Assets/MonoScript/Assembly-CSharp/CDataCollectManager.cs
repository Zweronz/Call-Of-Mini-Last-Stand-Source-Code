using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using LitJson;
using UnityEngine;

public class CDataCollectManager
{
	public iZombieSniperGameState m_GameState;

	public iZombieSniperGunCenter m_GunCenter;

	public string m_sTodayDate;

	public List<string> m_ltDCName;

	public Dictionary<string, CDataCollect> m_dictCollect;

	public CDataCollect m_curDataCollect;

	public int m_nGoldNow;

	public int m_nTCNow;

	public List<int> m_ltWeapon;

	public int[] m_arrItem;

	public float m_fGameTimeTotal;

	public int m_nGoldGainTotal;

	public int m_nTCGainTotal;

	public int[] m_arrScene;

	public bool m_bCrack;

	public string m_sFirstGame;

	public void Initialize()
	{
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_GunCenter = iZombieSniperGameApp.GetInstance().m_GunCenter;
		m_ltDCName = new List<string>();
		m_dictCollect = new Dictionary<string, CDataCollect>();
		m_ltWeapon = new List<int>();
		m_arrItem = new int[5];
		m_arrScene = new int[11];
		ResetData();
		LoadData();
		foreach (string item in m_ltDCName)
		{
			LoadDailyData(item);
		}
		string text = DateTime.Now.ToString("yyyy_MM_dd");
		Debug.Log("today is " + text);
		CreateToday(text);
		List<string> list = new List<string>();
		list.Clear();
		foreach (string item2 in m_ltDCName)
		{
			if (!m_dictCollect.ContainsKey(item2))
			{
				list.Add(item2);
			}
		}
		foreach (string item3 in list)
		{
			m_ltDCName.Remove(item3);
		}
		AddGameLogin();
	}

	public void ResetData()
	{
		m_bCrack = false;
		m_sFirstGame = string.Empty;
		m_nGoldNow = 0;
		m_nTCNow = 0;
		m_ltWeapon.Clear();
		for (int i = 0; i < m_arrItem.Length; i++)
		{
			m_arrItem[i] = 0;
		}
		m_fGameTimeTotal = 0f;
		m_nGoldGainTotal = 0;
		m_nTCGainTotal = 0;
		for (int j = 0; j < m_arrScene.Length; j++)
		{
			m_arrScene[j] = 0;
		}
	}

	public void CreateToday(string sDate)
	{
		m_sTodayDate = sDate;
		if (m_dictCollect.ContainsKey(sDate))
		{
			Debug.Log("already exist " + sDate);
			m_curDataCollect = m_dictCollect[sDate];
			return;
		}
		Debug.Log("create " + sDate);
		if (m_ltDCName.Count > 60)
		{
			string key = m_ltDCName[0];
			m_dictCollect.Remove(key);
			m_ltDCName.RemoveAt(0);
		}
		m_curDataCollect = new CDataCollect();
		m_curDataCollect.Initialize();
		m_curDataCollect.m_sData = sDate;
		m_dictCollect.Add(sDate, m_curDataCollect);
		if (!m_ltDCName.Contains(sDate))
		{
			m_ltDCName.Add(sDate);
		}
	}

	public void LoadData()
	{
		string content = string.Empty;
		Utils.FileGetString("DataCollect.xml", ref content);
		if (content.Length < 1)
		{
			m_sFirstGame = DateTime.Now.ToString("yyyy_MM_dd");
			Debug.Log("first time login " + m_sFirstGame);
			m_bCrack = MiscPlugin.CheckOSIsJailbreak();
			SaveData();
			return;
		}
		Debug.Log("load DataCollect.xml");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(content);
		XmlNode documentElement = xmlDocument.DocumentElement;
		XmlElement xmlElement = (XmlElement)documentElement;
		string empty = string.Empty;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			XmlElement xmlElement2 = (XmlElement)childNode;
			if ("OnceRecord" == xmlElement2.Name)
			{
				empty = xmlElement2.GetAttribute("bCrack").Trim();
				if (empty.Length > 0)
				{
					m_bCrack = bool.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("sFirstGame").Trim();
				if (empty.Length > 0)
				{
					m_sFirstGame = empty;
				}
			}
			else if ("DailyList" == xmlElement2.Name)
			{
				empty = xmlElement2.GetAttribute("List").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int i = 0; i < array.Length; i++)
					{
						m_ltDCName.Add(array[i]);
					}
				}
			}
			else if ("RealTimeRecord" == xmlElement2.Name)
			{
				empty = xmlElement2.GetAttribute("nGoldNow").Trim();
				if (empty.Length > 0)
				{
					m_nGoldNow = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("nTCNow").Trim();
				if (empty.Length > 0)
				{
					m_nTCNow = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("ltWeapon").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int j = 0; j < array.Length; j++)
					{
						m_ltWeapon.Add(int.Parse(array[j]));
					}
				}
				empty = xmlElement2.GetAttribute("arrItem").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int k = 0; k < array.Length && k < m_arrItem.Length; k++)
					{
						m_arrItem[k] = int.Parse(array[k]);
					}
				}
			}
			else
			{
				if (!("TotalRecord" == xmlElement2.Name))
				{
					continue;
				}
				empty = xmlElement2.GetAttribute("fGameTimeTotal").Trim();
				if (empty.Length > 0)
				{
					m_fGameTimeTotal = float.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("nGoldGainTotal").Trim();
				if (empty.Length > 0)
				{
					m_nGoldGainTotal = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("m_nTCGainTotal").Trim();
				if (empty.Length > 0)
				{
					m_nTCGainTotal = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("arrScene").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int l = 0; l < array.Length && l < m_arrScene.Length; l++)
					{
						m_arrScene[l] = int.Parse(array[l]);
					}
				}
			}
		}
	}

	public void SaveData()
	{
		Debug.Log("save DataCollect.xml");
		XmlDocument xmlDocument = new XmlDocument();
		XmlNode newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "no");
		xmlDocument.AppendChild(newChild);
		string empty = string.Empty;
		XmlElement xmlElement = xmlDocument.CreateElement("Game");
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement("OnceRecord");
		xmlElement2.SetAttribute("bCrack", m_bCrack.ToString());
		xmlElement2.SetAttribute("sFirstGame", m_sFirstGame);
		xmlElement.AppendChild(xmlElement2);
		XmlElement xmlElement3 = xmlDocument.CreateElement("RealTimeRecord");
		xmlElement3.SetAttribute("nGoldNow", m_nGoldNow.ToString());
		xmlElement3.SetAttribute("nTCNow", m_nTCNow.ToString());
		empty = string.Empty;
		for (int i = 0; i < m_ltWeapon.Count; i++)
		{
			empty = ((i != 0) ? (empty + "," + m_ltWeapon[i]) : (empty + m_ltWeapon[i]));
		}
		xmlElement3.SetAttribute("ltWeapon", empty);
		empty = string.Empty;
		for (int j = 0; j < m_arrItem.Length; j++)
		{
			empty = ((j != 0) ? (empty + "," + m_arrItem[j]) : (empty + m_arrItem[j]));
		}
		xmlElement3.SetAttribute("arrItem", empty);
		xmlElement.AppendChild(xmlElement3);
		XmlElement xmlElement4 = xmlDocument.CreateElement("TotalRecord");
		xmlElement4.SetAttribute("fGameTimeTotal", m_fGameTimeTotal.ToString());
		xmlElement4.SetAttribute("nGoldGainTotal", m_nGoldGainTotal.ToString());
		xmlElement4.SetAttribute("nTCGainTotal", m_nTCGainTotal.ToString());
		empty = string.Empty;
		for (int k = 0; k < m_arrScene.Length; k++)
		{
			empty = ((k != 0) ? (empty + "," + m_arrScene[k]) : (empty + m_arrScene[k]));
		}
		xmlElement4.SetAttribute("arrScene", empty);
		xmlElement.AppendChild(xmlElement4);
		XmlElement xmlElement5 = xmlDocument.CreateElement("DailyList");
		empty = string.Empty;
		foreach (string item in m_ltDCName)
		{
			empty = ((empty.Length >= 1) ? (empty + "," + item) : (empty + item));
		}
		xmlElement5.SetAttribute("List", empty);
		xmlElement.AppendChild(xmlElement5);
		string filename = Utils.SavePath() + "/DataCollect.xml";
		xmlDocument.Save(filename);
	}

	public void LoadDailyData(string sDate)
	{
		string text = "DataCollectDaily_" + sDate + ".xml";
		Debug.Log("load " + text);
		string content = string.Empty;
		Utils.FileGetString(text, ref content);
		if (content.Length < 1)
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(content);
		XmlNode documentElement = xmlDocument.DocumentElement;
		XmlElement xmlElement = (XmlElement)documentElement;
		string empty = string.Empty;
		CDataCollect cDataCollect = new CDataCollect();
		cDataCollect.Initialize();
		cDataCollect.m_sData = sDate;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			XmlElement xmlElement2 = (XmlElement)childNode;
			if ("data" == xmlElement2.Name)
			{
				empty = xmlElement2.GetAttribute("fGameTimeToday").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_fGameTimeToday = float.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("nLoginTimesToday").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nLoginTimesToday = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("nGoldGainFromGameToday").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nGoldGainFromGameToday = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("nGoldGainFromIAPToday").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nGoldGainFromIAPToday = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("arrGainFromIAPToday").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int i = 0; i < array.Length && i < cDataCollect.m_arrGainFromIAPToday.Length; i++)
					{
						cDataCollect.m_arrGainFromIAPToday[i] = int.Parse(array[i]);
					}
				}
				empty = xmlElement2.GetAttribute("nGoldConsumeToday").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nGoldConsumeToday = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("nTCGainToday").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nTCGainToday = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("nTCConsumeToday").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nTCConsumeToday = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("ltWeaponBuyToday").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int j = 0; j < array.Length; j++)
					{
						cDataCollect.m_ltWeaponBuyToday.Add(int.Parse(array[j]));
					}
				}
				empty = xmlElement2.GetAttribute("arrItemBuyToday").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int k = 0; k < array.Length && k < cDataCollect.m_arrItemBuyToday.Length; k++)
					{
						cDataCollect.m_arrItemBuyToday[k] = int.Parse(array[k]);
					}
				}
				empty = xmlElement2.GetAttribute("arrItemConsumeToday").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int l = 0; l < array.Length && l < cDataCollect.m_arrItemConsumeToday.Length; l++)
					{
						cDataCollect.m_arrItemConsumeToday[l] = int.Parse(array[l]);
					}
				}
				empty = xmlElement2.GetAttribute("nStartGameToday").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nStartGameToday = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("nGameOverToday").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nGameOverToday = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("ltGameBreakOutToday").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int m = 0; m < array.Length; m++)
					{
						cDataCollect.m_ltGameBreakOutToday.Add(float.Parse(array[m]));
					}
				}
			}
			else if ("RealTimeRecord" == xmlElement2.Name)
			{
				empty = xmlElement2.GetAttribute("nGoldNow").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nGoldNow = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("nTCNow").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nTCNow = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("ltWeapon").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int n = 0; n < array.Length; n++)
					{
						cDataCollect.m_ltWeapon.Add(int.Parse(array[n]));
					}
				}
				empty = xmlElement2.GetAttribute("arrItem").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int num = 0; num < array.Length && num < cDataCollect.m_arrItem.Length; num++)
					{
						cDataCollect.m_arrItem[num] = int.Parse(array[num]);
					}
				}
			}
			else
			{
				if (!("TotalRecord" == xmlElement2.Name))
				{
					continue;
				}
				empty = xmlElement2.GetAttribute("fGameTimeTotal").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_fGameTimeTotal = float.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("nGoldGainTotal").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nGoldGainTotal = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("nTCGainTotal").Trim();
				if (empty.Length > 0)
				{
					cDataCollect.m_nTCGainTotal = int.Parse(empty);
				}
				empty = xmlElement2.GetAttribute("arrScene").Trim();
				if (empty.Length > 0)
				{
					string[] array = empty.Split(',');
					for (int num2 = 0; num2 < array.Length && num2 < cDataCollect.m_arrScene.Length; num2++)
					{
						cDataCollect.m_arrScene[num2] = int.Parse(array[num2]);
					}
				}
			}
		}
		if (m_dictCollect.ContainsKey(sDate))
		{
			m_dictCollect.Remove(sDate);
		}
		m_dictCollect.Add(sDate, cDataCollect);
	}

	public void SaveDailyData(string sDate)
	{
		string text = "DataCollectDaily_" + sDate + ".xml";
		Debug.Log("save " + text);
		if (m_dictCollect.ContainsKey(sDate))
		{
			CDataCollect cDataCollect = m_dictCollect[sDate];
			string empty = string.Empty;
			XmlDocument xmlDocument = new XmlDocument();
			XmlNode newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "no");
			xmlDocument.AppendChild(newChild);
			XmlElement xmlElement = xmlDocument.CreateElement("Game");
			xmlDocument.AppendChild(xmlElement);
			XmlElement xmlElement2 = xmlDocument.CreateElement("data");
			xmlElement2.SetAttribute("sData", cDataCollect.m_sData);
			xmlElement2.SetAttribute("fGameTimeToday", cDataCollect.m_fGameTimeToday.ToString());
			xmlElement2.SetAttribute("nLoginTimesToday", cDataCollect.m_nLoginTimesToday.ToString());
			xmlElement2.SetAttribute("nGoldGainFromGameToday", cDataCollect.m_nGoldGainFromGameToday.ToString());
			xmlElement2.SetAttribute("nGoldGainFromIAPToday", cDataCollect.m_nGoldGainFromIAPToday.ToString());
			empty = string.Empty;
			for (int i = 0; i < cDataCollect.m_arrGainFromIAPToday.Length; i++)
			{
				empty = ((i != 0) ? (empty + "," + cDataCollect.m_arrGainFromIAPToday[i]) : (empty + cDataCollect.m_arrGainFromIAPToday[i]));
			}
			xmlElement2.SetAttribute("arrGainFromIAPToday", empty);
			xmlElement2.SetAttribute("nGoldConsumeToday", cDataCollect.m_nGoldConsumeToday.ToString());
			xmlElement2.SetAttribute("nTCGainToday", cDataCollect.m_nTCGainToday.ToString());
			xmlElement2.SetAttribute("nTCConsumeToday", cDataCollect.m_nTCConsumeToday.ToString());
			empty = string.Empty;
			for (int j = 0; j < cDataCollect.m_ltWeaponBuyToday.Count; j++)
			{
				empty = ((j != 0) ? (empty + "," + cDataCollect.m_ltWeaponBuyToday[j]) : (empty + cDataCollect.m_ltWeaponBuyToday[j]));
			}
			xmlElement2.SetAttribute("ltWeaponBuyToday", empty);
			empty = string.Empty;
			for (int k = 0; k < cDataCollect.m_arrItemBuyToday.Length; k++)
			{
				empty = ((k != 0) ? (empty + "," + cDataCollect.m_arrItemBuyToday[k]) : (empty + cDataCollect.m_arrItemBuyToday[k]));
			}
			xmlElement2.SetAttribute("arrItemBuyToday", empty);
			empty = string.Empty;
			for (int l = 0; l < cDataCollect.m_arrItemConsumeToday.Length; l++)
			{
				empty = ((l != 0) ? (empty + "," + cDataCollect.m_arrItemConsumeToday[l]) : (empty + cDataCollect.m_arrItemConsumeToday[l]));
			}
			xmlElement2.SetAttribute("arrItemConsumeToday", empty);
			xmlElement2.SetAttribute("nStartGameToday", cDataCollect.m_nStartGameToday.ToString());
			xmlElement2.SetAttribute("nGameOverToday", cDataCollect.m_nGameOverToday.ToString());
			empty = string.Empty;
			for (int m = 0; m < cDataCollect.m_ltGameBreakOutToday.Count; m++)
			{
				empty = ((m != 0) ? (empty + "," + cDataCollect.m_ltGameBreakOutToday[m]) : (empty + cDataCollect.m_ltGameBreakOutToday[m]));
			}
			xmlElement2.SetAttribute("ltGameBreakOutToday", empty);
			xmlElement.AppendChild(xmlElement2);
			XmlElement xmlElement3 = xmlDocument.CreateElement("RealTimeRecord");
			xmlElement3.SetAttribute("nGoldNow", cDataCollect.m_nGoldNow.ToString());
			xmlElement3.SetAttribute("nTCNow", cDataCollect.m_nTCNow.ToString());
			empty = string.Empty;
			for (int n = 0; n < cDataCollect.m_ltWeapon.Count; n++)
			{
				empty = ((n != 0) ? (empty + "," + cDataCollect.m_ltWeapon[n]) : (empty + cDataCollect.m_ltWeapon[n]));
			}
			xmlElement3.SetAttribute("ltWeapon", empty);
			empty = string.Empty;
			for (int num = 0; num < cDataCollect.m_arrItem.Length; num++)
			{
				empty = ((num != 0) ? (empty + "," + cDataCollect.m_arrItem[num]) : (empty + cDataCollect.m_arrItem[num]));
			}
			xmlElement3.SetAttribute("arrItem", empty);
			xmlElement.AppendChild(xmlElement3);
			XmlElement xmlElement4 = xmlDocument.CreateElement("TotalRecord");
			xmlElement4.SetAttribute("fGameTimeTotal", cDataCollect.m_fGameTimeTotal.ToString());
			xmlElement4.SetAttribute("nGoldGainTotal", cDataCollect.m_nGoldGainTotal.ToString());
			xmlElement4.SetAttribute("nTCGainTotal", cDataCollect.m_nTCGainTotal.ToString());
			empty = string.Empty;
			for (int num2 = 0; num2 < cDataCollect.m_arrScene.Length; num2++)
			{
				empty = ((num2 != 0) ? (empty + "," + cDataCollect.m_arrScene[num2]) : (empty + cDataCollect.m_arrScene[num2]));
			}
			xmlElement4.SetAttribute("arrScene", empty);
			xmlElement.AppendChild(xmlElement4);
			string filename = Utils.SavePath() + "/" + text;
			xmlDocument.Save(filename);
		}
	}

	public void DeleteDailyDate(string sDate)
	{
		string text = "DataCollectDaily_" + sDate + ".xml";
		File.Delete(Utils.SavePath() + "/" + text);
	}

	private void OnRequest(int task_id, string param, int code, string response)
	{
		JsonData jsonData = JsonMapper.ToObject(response);
		string text = jsonData["code"].ToString();
		if (text == "0")
		{
			Debug.Log("OnResponse code success:" + text);
			if (param.Length >= 1 && !(m_sTodayDate == param) && m_dictCollect.ContainsKey(param))
			{
				m_dictCollect.Remove(param);
				m_ltDCName.Remove(param);
				DeleteDailyDate(param);
				SaveData();
			}
		}
		else
		{
			Debug.Log("OnResponse code error:" + text);
		}
	}

	private void OnRequestTimeout(int task_id, string param)
	{
		Debug.Log("OnRequestTimeout");
	}

	public void SendDailyData(string sDate)
	{
		Debug.Log("send data " + sDate);
		if (m_dictCollect.ContainsKey(sDate))
		{
			CDataCollect cDataCollect = m_dictCollect[sDate];
			Hashtable hashtable = null;
			Hashtable hashtable2 = new Hashtable();
			hashtable2["gamename"] = "COMLD";
			hashtable2["macaddress"] = MiscPlugin.GetMacAddr();
			hashtable2["Date"] = sDate;
			hashtable2["fGameTimeToday"] = TransTimeSecond(cDataCollect.m_fGameTimeToday);
			hashtable2["nLoginTimesToday"] = cDataCollect.m_nLoginTimesToday;
			hashtable2["nGoldGainFromGameToday"] = cDataCollect.m_nGoldGainFromGameToday;
			hashtable2["nGoldGainFromIAPToday"] = cDataCollect.m_nGoldGainFromIAPToday;
			hashtable = new Hashtable();
			hashtable["_099cents1"] = cDataCollect.m_arrGainFromIAPToday[0];
			hashtable["_099cents2"] = cDataCollect.m_arrGainFromIAPToday[1];
			hashtable["_199cents1"] = cDataCollect.m_arrGainFromIAPToday[2];
			hashtable["_199cents2"] = cDataCollect.m_arrGainFromIAPToday[3];
			hashtable["_299cents1"] = cDataCollect.m_arrGainFromIAPToday[4];
			hashtable["_299cents2"] = cDataCollect.m_arrGainFromIAPToday[5];
			hashtable["_499cents1"] = cDataCollect.m_arrGainFromIAPToday[6];
			hashtable["_499cents2"] = cDataCollect.m_arrGainFromIAPToday[7];
			hashtable["_999cents1"] = cDataCollect.m_arrGainFromIAPToday[8];
			hashtable["_999cents2"] = cDataCollect.m_arrGainFromIAPToday[9];
			hashtable["_1999cents1"] = cDataCollect.m_arrGainFromIAPToday[10];
			hashtable["_1999cents2"] = cDataCollect.m_arrGainFromIAPToday[11];
			hashtable["_4999cents1"] = cDataCollect.m_arrGainFromIAPToday[12];
			hashtable["_4999cents2"] = cDataCollect.m_arrGainFromIAPToday[13];
			hashtable["_9999cents1"] = cDataCollect.m_arrGainFromIAPToday[14];
			hashtable["_9999cents2"] = cDataCollect.m_arrGainFromIAPToday[15];
			hashtable2["arrGainFromIAPToday"] = hashtable;
			hashtable2["nGoldConsumeToday"] = cDataCollect.m_nGoldConsumeToday;
			hashtable2["nTCGainToday"] = cDataCollect.m_nTCGainToday;
			hashtable2["nTCConsumeToday"] = cDataCollect.m_nTCConsumeToday;
			string text = "None";
			for (int i = 0; i < cDataCollect.m_ltWeaponBuyToday.Count; i++)
			{
				text = ((i != 0) ? (text + "," + cDataCollect.m_ltWeaponBuyToday[i]) : cDataCollect.m_ltWeaponBuyToday[i].ToString());
			}
			hashtable2["ltWeaponBuyToday"] = text;
			hashtable = new Hashtable();
			hashtable["TowerIBT"] = cDataCollect.m_arrItemBuyToday[0];
			hashtable["TowerBulletIBT"] = cDataCollect.m_arrItemBuyToday[1];
			hashtable["LandMineIBT"] = cDataCollect.m_arrItemBuyToday[2];
			hashtable["AirStrikeIBT"] = cDataCollect.m_arrItemBuyToday[3];
			hashtable["InnoKillerIBT"] = cDataCollect.m_arrItemBuyToday[4];
			hashtable2["arrItemBuyToday"] = hashtable;
			hashtable = new Hashtable();
			hashtable["TowerICT"] = cDataCollect.m_arrItemConsumeToday[0];
			hashtable["TowerBulletICT"] = cDataCollect.m_arrItemConsumeToday[1];
			hashtable["LandICT"] = cDataCollect.m_arrItemConsumeToday[2];
			hashtable["AirStrikeICT"] = cDataCollect.m_arrItemConsumeToday[3];
			hashtable["InnoKillerICT"] = cDataCollect.m_arrItemConsumeToday[4];
			hashtable2["arrItemConsumeToday"] = hashtable;
			hashtable2["nStartGameToday"] = cDataCollect.m_nStartGameToday;
			hashtable2["nGameOverToday"] = cDataCollect.m_nGameOverToday;
			text = "None";
			for (int j = 0; j < cDataCollect.m_ltGameBreakOutToday.Count; j++)
			{
				text = ((j != 0) ? (text + "," + TransTimeSecond(cDataCollect.m_ltGameBreakOutToday[j])) : TransTimeSecond(cDataCollect.m_ltGameBreakOutToday[j]));
			}
			hashtable2["ltGameBreakOutToday"] = text;
			hashtable2["bCrack"] = m_bCrack;
			hashtable2["sFirstGame"] = m_sFirstGame;
			hashtable2["totalnGoldNow"] = cDataCollect.m_nGoldNow;
			hashtable2["totalnTCNow"] = cDataCollect.m_nTCNow;
			text = "None";
			for (int k = 0; k < cDataCollect.m_ltWeapon.Count; k++)
			{
				text = ((k != 0) ? (text + "," + cDataCollect.m_ltWeapon[k]) : cDataCollect.m_ltWeapon[k].ToString());
			}
			hashtable2["totalltWeapon"] = text;
			hashtable = new Hashtable();
			hashtable["TowerTAI"] = cDataCollect.m_arrItem[0];
			hashtable["TowerBulletTAI"] = cDataCollect.m_arrItem[1];
			hashtable["LandMineTAI"] = cDataCollect.m_arrItem[2];
			hashtable["AirStrikeTAI"] = cDataCollect.m_arrItem[3];
			hashtable["InnoKillerTAI"] = cDataCollect.m_arrItem[4];
			hashtable2["totalarrItem"] = hashtable;
			hashtable2["totalfGameTimeTotal"] = TransTimeSecond(cDataCollect.m_fGameTimeTotal);
			hashtable2["totalnGoldGainTotal"] = cDataCollect.m_nGoldGainTotal;
			hashtable2["totalnTCGainTotal"] = cDataCollect.m_nTCGainTotal;
			hashtable = new Hashtable();
			hashtable["Menu"] = cDataCollect.m_arrScene[0];
			hashtable["Ready"] = cDataCollect.m_arrScene[1];
			hashtable["Option"] = cDataCollect.m_arrScene[2];
			hashtable["Credit"] = cDataCollect.m_arrScene[3];
			hashtable["Help"] = cDataCollect.m_arrScene[4];
			hashtable["Shop"] = cDataCollect.m_arrScene[5];
			hashtable["IAP"] = cDataCollect.m_arrScene[6];
			hashtable["Map"] = cDataCollect.m_arrScene[7];
			hashtable["Scene0"] = cDataCollect.m_arrScene[8];
			hashtable["Scene1"] = cDataCollect.m_arrScene[9];
			hashtable["Scene2"] = cDataCollect.m_arrScene[10];
			hashtable2["totalarrScene"] = hashtable;
			string text2 = JsonMapper.ToJson(hashtable2);
			Debug.Log(text2);
			Hashtable hashtable3 = new Hashtable();
			hashtable3["gamename"] = "COMLD";
			hashtable3["data"] = text2;
			string request = JsonMapper.ToJson(hashtable3);
			string url = "http://account.trinitigame.com/gameapi/turboPlatform2.do?action=logAllInfo&json=";
			HttpManager.Instance().SendRequest(url, request, sDate, 15f, OnRequest, OnRequestTimeout);
		}
	}

	public void SendData()
	{
	}

	public void AddSceneCount(dcGameSceneEnum type)
	{
		if (type >= dcGameSceneEnum.Menu && (int)type < m_arrScene.Length)
		{
			m_arrScene[(int)type]++;
		}
	}

	public void AddGameTime(float fTime)
	{
		m_curDataCollect.m_fGameTimeToday += fTime;
		m_fGameTimeTotal += fTime;
	}

	public void AddGameLogin()
	{
		m_curDataCollect.m_nLoginTimesToday++;
	}

	public void AddGoldFromGame(int nGold)
	{
		m_curDataCollect.m_nGoldGainFromGameToday += nGold;
		m_nGoldGainTotal += nGold;
	}

	public void AddGoldFromIAP(int nGold)
	{
		m_curDataCollect.m_nGoldGainFromIAPToday += nGold;
		m_nGoldGainTotal += nGold;
	}

	public void AddGainFromIAP(dcIAPEnum type)
	{
		if (type >= dcIAPEnum._099cents1 && (int)type < m_curDataCollect.m_arrGainFromIAPToday.Length)
		{
			m_curDataCollect.m_arrGainFromIAPToday[(int)type]++;
		}
	}

	public void AddGoldConsume(int nGold)
	{
		m_curDataCollect.m_nGoldConsumeToday -= nGold;
	}

	public void AddTCGain(int nTC)
	{
		m_curDataCollect.m_nTCGainToday += nTC;
		m_nTCGainTotal += nTC;
	}

	public void AddTCConsume(int nTC)
	{
		m_curDataCollect.m_nTCConsumeToday += nTC;
	}

	public void AddWeaponBuy(int nWeapon)
	{
		m_curDataCollect.m_ltWeaponBuyToday.Add(nWeapon);
		m_ltWeapon.Add(nWeapon);
	}

	public void AddItemBuy(dcItemEnum type)
	{
		if (type >= dcItemEnum.Tower && type < dcItemEnum.Count)
		{
			if (type == dcItemEnum.TowerBullet)
			{
				m_curDataCollect.m_arrItemBuyToday[(int)type] += 100;
				m_arrItem[(int)type] += 100;
			}
			else
			{
				m_curDataCollect.m_arrItemBuyToday[(int)type]++;
				m_arrItem[(int)type]++;
			}
		}
	}

	public void AddItemConsume(dcItemEnum type)
	{
		if (type >= dcItemEnum.Tower && type < dcItemEnum.Count)
		{
			m_curDataCollect.m_arrItemConsumeToday[(int)type]++;
			m_arrItem[(int)type]--;
		}
	}

	public void AddStartGame()
	{
		m_curDataCollect.m_nStartGameToday++;
	}

	public void AddGameOver()
	{
		m_curDataCollect.m_nGameOverToday++;
	}

	public void AddGameBreak(float fTime)
	{
		m_curDataCollect.m_ltGameBreakOutToday.Add(fTime);
	}

	public void UpdateTotalDataToToday(string sData)
	{
		if (!m_dictCollect.ContainsKey(sData))
		{
			return;
		}
		CDataCollect cDataCollect = m_dictCollect[sData];
		if (cDataCollect == null)
		{
			return;
		}
		cDataCollect.m_nGoldNow = m_nGoldNow;
		cDataCollect.m_nTCNow = m_nTCNow;
		cDataCollect.m_fGameTimeTotal = m_fGameTimeTotal;
		cDataCollect.m_nGoldGainTotal = m_nGoldGainTotal;
		cDataCollect.m_nTCGainTotal = m_nTCGainTotal;
		cDataCollect.m_ltWeapon.Clear();
		foreach (int item in m_ltWeapon)
		{
			cDataCollect.m_ltWeapon.Add(item);
		}
		for (int i = 0; i < m_arrItem.Length && i < cDataCollect.m_arrItem.Length; i++)
		{
			cDataCollect.m_arrItem[i] = m_arrItem[i];
		}
		for (int j = 0; j < m_arrScene.Length && j < cDataCollect.m_arrScene.Length; j++)
		{
			cDataCollect.m_arrScene[j] = m_arrScene[j];
		}
	}

	private string TransTimeSecond(float fV)
	{
		return fV.ToString("f2");
	}
}
