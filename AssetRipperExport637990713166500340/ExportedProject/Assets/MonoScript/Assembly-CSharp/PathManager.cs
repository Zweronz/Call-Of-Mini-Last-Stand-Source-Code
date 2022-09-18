using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

[ExecuteInEditMode]
public class PathManager : MonoBehaviour
{
	public enum PathEnum
	{
		Path = 0,
		Point = 1,
		Rect = 2
	}

	[NonSerialized]
	public string[] m_sOptions = new string[3] { "Path", "Point", "Rect" };

	public GameObject[] m_arrSaveList;

	public string m_sSavePath;

	private void Awake()
	{
		if (base.transform.name != "PathManager")
		{
			base.transform.name = "PathManager";
		}
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
		base.transform.position = Vector3.zero;
		base.transform.eulerAngles = Vector3.zero;
	}

	public void Create(string sPathName, PathEnum type)
	{
		if (sPathName.Length < 1)
		{
			return;
		}
		switch (type)
		{
		case PathEnum.Path:
		{
			Transform transform3 = base.transform.Find("PathList");
			if (transform3 == null)
			{
				transform3 = new GameObject("PathList").transform;
				transform3.parent = base.transform;
			}
			else if (transform3.Find(sPathName) != null)
			{
				break;
			}
			GameObject gameObject3 = new GameObject(sPathName);
			gameObject3.transform.parent = transform3;
			gameObject3.AddComponent<CPathPara>();
			gameObject3.AddComponent<PathPointMaker>();
			break;
		}
		case PathEnum.Point:
		{
			Transform transform2 = base.transform.Find("PointList");
			if (transform2 == null)
			{
				transform2 = new GameObject("PointList").transform;
				transform2.parent = base.transform;
			}
			else if (transform2.Find(sPathName) != null)
			{
				break;
			}
			GameObject gameObject2 = new GameObject(sPathName);
			gameObject2.transform.parent = transform2;
			gameObject2.AddComponent<CSinglePara>();
			gameObject2.AddComponent<PathSingleMaker>();
			break;
		}
		case PathEnum.Rect:
		{
			Transform transform = base.transform.Find("RectList");
			if (transform == null)
			{
				transform = new GameObject("RectList").transform;
				transform.parent = base.transform;
			}
			else if (transform.Find(sPathName) != null)
			{
				break;
			}
			GameObject gameObject = new GameObject(sPathName);
			gameObject.transform.parent = transform;
			gameObject.AddComponent<CRectPara>();
			gameObject.AddComponent<PathRectMaker>();
			break;
		}
		}
	}

	public void SaveXml()
	{
		if (m_sSavePath.Length < 2 || m_arrSaveList.Length < 1)
		{
			return;
		}
		List<Vector3> list = new List<Vector3>();
		List<List<int>> list2 = new List<List<int>>();
		List<List<int>> list3 = new List<List<int>>();
		List<List<int>> list4 = new List<List<int>>();
		List<CRect> list5 = new List<CRect>();
		List<CRect> list6 = new List<CRect>();
		List<CRect> list7 = new List<CRect>();
		CRect cRect = null;
		CRect cRect2 = null;
		List<Vector3> list8 = new List<Vector3>();
		for (int i = 0; i < m_arrSaveList.Length; i++)
		{
			List<int> list9 = new List<int>();
			CPathPara component = m_arrSaveList[i].GetComponent<CPathPara>();
			if (component != null && component.m_ltPoint != null)
			{
				foreach (Transform item in component.m_ltPoint)
				{
					list.Add(item.transform.position);
					list9.Add(list.Count - 1);
				}
				if (m_arrSaveList[i].name.IndexOf("Dog") != -1)
				{
					list3.Add(list9);
				}
				else if (m_arrSaveList[i].name.IndexOf("Innocents") != -1)
				{
					list4.Add(list9);
				}
				else
				{
					list2.Add(list9);
				}
			}
			CRectPara component2 = m_arrSaveList[i].GetComponent<CRectPara>();
			if (component2 != null && component2.m_ltRect != null)
			{
				foreach (Transform item2 in component2.m_ltRect)
				{
					CRect component3 = item2.GetComponent<CRect>();
					if (!(component3 == null))
					{
						if (m_arrSaveList[i].name.IndexOf("StartZone") != -1)
						{
							list5.Add(component3);
						}
						if (m_arrSaveList[i].name.IndexOf("FinallyZone") != -1)
						{
							cRect = component3;
						}
						if (m_arrSaveList[i].name.IndexOf("MineZone") != -1)
						{
							cRect2 = component3;
						}
						if (m_arrSaveList[i].name == "JumpZone")
						{
							list6.Add(component3);
						}
						if (m_arrSaveList[i].name == "StartJumpZone")
						{
							list7.Add(component3);
						}
					}
				}
			}
			CSinglePara component4 = m_arrSaveList[i].GetComponent<CSinglePara>();
			if (!(component4 != null) || component4.m_ltPoint == null)
			{
				continue;
			}
			foreach (Transform item3 in component4.m_ltPoint)
			{
				if (m_arrSaveList[i].name.IndexOf("OilCan") != -1)
				{
					list8.Add(item3.position);
				}
			}
		}
		XmlDocument xmlDocument = new XmlDocument();
		XmlNode newChild = xmlDocument.CreateXmlDeclaration("1.0", "GBK", "no");
		xmlDocument.AppendChild(newChild);
		XmlElement xmlElement = xmlDocument.CreateElement("WayPoint");
		xmlDocument.AppendChild(xmlElement);
		for (int j = 0; j < list.Count; j++)
		{
			XmlElement xmlElement2 = xmlDocument.CreateElement("Point");
			xmlElement2.SetAttribute("id", j.ToString());
			xmlElement2.SetAttribute("pos", list[j].x.ToString("f2") + "," + list[j].z.ToString("f2"));
			xmlElement.AppendChild(xmlElement2);
		}
		XmlElement xmlElement3 = xmlDocument.CreateElement("Path");
		xmlElement3.SetAttribute("id", "1");
		for (int k = 0; k < list2.Count; k++)
		{
			XmlElement xmlElement4 = xmlDocument.CreateElement("node");
			xmlElement4.SetAttribute("id", k.ToString());
			string text = string.Empty;
			for (int l = 0; l < list2[k].Count; l++)
			{
				text = ((l != 0) ? (text + "," + list2[k][l]) : list2[k][l].ToString());
			}
			xmlElement4.SetAttribute("path", text);
			xmlElement3.AppendChild(xmlElement4);
		}
		xmlElement.AppendChild(xmlElement3);
		XmlElement xmlElement5 = xmlDocument.CreateElement("Path");
		xmlElement5.SetAttribute("id", "2");
		for (int m = 0; m < list3.Count; m++)
		{
			XmlElement xmlElement6 = xmlDocument.CreateElement("node");
			xmlElement6.SetAttribute("id", m.ToString());
			string text2 = string.Empty;
			for (int n = 0; n < list3[m].Count; n++)
			{
				text2 = ((n != 0) ? (text2 + "," + list3[m][n]) : list3[m][n].ToString());
			}
			xmlElement6.SetAttribute("path", text2);
			xmlElement5.AppendChild(xmlElement6);
		}
		xmlElement.AppendChild(xmlElement5);
		XmlElement xmlElement7 = xmlDocument.CreateElement("Path");
		xmlElement7.SetAttribute("id", "3");
		for (int num = 0; num < list4.Count; num++)
		{
			XmlElement xmlElement8 = xmlDocument.CreateElement("node");
			xmlElement8.SetAttribute("id", num.ToString());
			string text3 = string.Empty;
			for (int num2 = 0; num2 < list4[num].Count; num2++)
			{
				text3 = ((num2 != 0) ? (text3 + "," + list4[num][num2]) : list4[num][num2].ToString());
			}
			xmlElement8.SetAttribute("path", text3);
			xmlElement7.AppendChild(xmlElement8);
		}
		xmlElement.AppendChild(xmlElement7);
		for (int num3 = 0; num3 < list5.Count; num3++)
		{
			XmlElement xmlElement9 = xmlDocument.CreateElement("StartZone");
			xmlElement9.SetAttribute("id", (num3 + 1).ToString());
			float widthX = list5[num3].WidthX;
			float widthZ = list5[num3].WidthZ;
			float num4 = list5[num3].transform.position.x - widthX / 2f;
			float num5 = list5[num3].transform.position.z - widthZ / 2f;
			string value = num4.ToString("f2") + "," + num5.ToString("f2") + "," + widthX.ToString("f2") + "," + widthZ.ToString("f2");
			xmlElement9.SetAttribute("rect", value);
			xmlElement.AppendChild(xmlElement9);
		}
		if (cRect != null)
		{
			XmlElement xmlElement10 = xmlDocument.CreateElement("FinallyZone");
			float widthX2 = cRect.WidthX;
			float widthZ2 = cRect.WidthZ;
			float num6 = cRect.transform.position.x - widthX2 / 2f;
			float num7 = cRect.transform.position.z - widthZ2 / 2f;
			string value2 = num6.ToString("f2") + "," + num7.ToString("f2") + "," + widthX2.ToString("f2") + "," + widthZ2.ToString("f2");
			xmlElement10.SetAttribute("rect", value2);
			xmlElement.AppendChild(xmlElement10);
		}
		if (cRect2 != null)
		{
			XmlElement xmlElement11 = xmlDocument.CreateElement("MineZone");
			float widthX3 = cRect2.WidthX;
			float widthZ3 = cRect2.WidthZ;
			float num8 = cRect2.transform.position.x - widthX3 / 2f;
			float num9 = cRect2.transform.position.z - widthZ3 / 2f;
			string value3 = num8.ToString("f2") + "," + num9.ToString("f2") + "," + widthX3.ToString("f2") + "," + widthZ3.ToString("f2");
			xmlElement11.SetAttribute("rect", value3);
			xmlElement.AppendChild(xmlElement11);
		}
		for (int num10 = 0; num10 < list8.Count; num10++)
		{
			XmlElement xmlElement12 = xmlDocument.CreateElement("OilCan");
			xmlElement12.SetAttribute("id", num10.ToString());
			xmlElement12.SetAttribute("pos", list8[num10].x.ToString("f2") + "," + list8[num10].z.ToString("f2"));
			xmlElement.AppendChild(xmlElement12);
		}
		for (int num11 = 0; num11 < list6.Count; num11++)
		{
			XmlElement xmlElement13 = xmlDocument.CreateElement("JumpPoint");
			xmlElement13.SetAttribute("id", (num11 + 1).ToString());
			float widthX4 = list6[num11].WidthX;
			float widthZ4 = list6[num11].WidthZ;
			float num12 = list6[num11].transform.position.x - widthX4 / 2f;
			float num13 = list6[num11].transform.position.z - widthZ4 / 2f;
			string value4 = num12.ToString("f2") + "," + num13.ToString("f2") + "," + widthX4.ToString("f2") + "," + widthZ4.ToString("f2");
			xmlElement13.SetAttribute("rect", value4);
			xmlElement.AppendChild(xmlElement13);
		}
		for (int num14 = 0; num14 < list7.Count; num14++)
		{
			XmlElement xmlElement14 = xmlDocument.CreateElement("StartJumpPoint");
			xmlElement14.SetAttribute("id", (num14 + 1).ToString());
			float widthX5 = list7[num14].WidthX;
			float widthZ5 = list7[num14].WidthZ;
			float num15 = list7[num14].transform.position.x - widthX5 / 2f;
			float num16 = list7[num14].transform.position.z - widthZ5 / 2f;
			string value5 = num15.ToString("f2") + "," + num16.ToString("f2") + "," + widthX5.ToString("f2") + "," + widthZ5.ToString("f2");
			xmlElement14.SetAttribute("rect", value5);
			xmlElement.AppendChild(xmlElement14);
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		int num17 = m_sSavePath.LastIndexOf("/");
		if (num17 != -1)
		{
			empty2 = m_sSavePath.Substring(0, num17);
			empty = m_sSavePath.Substring(num17 + 1, m_sSavePath.Length - num17 - 1);
			if (empty2[0] == '/')
			{
				empty2 = empty2.Substring(1, empty2.Length - 1);
			}
			Debug.Log("filepath = " + empty2);
			empty2 = Utils.SavePath() + "/" + empty2;
		}
		else
		{
			empty = m_sSavePath;
			empty2 = Utils.SavePath();
		}
		Debug.Log("filename = " + empty);
		if (!Directory.Exists(empty2))
		{
			Directory.CreateDirectory(empty2);
		}
		empty2 = empty2 + "/" + empty;
		xmlDocument.Save(empty2);
	}
}
