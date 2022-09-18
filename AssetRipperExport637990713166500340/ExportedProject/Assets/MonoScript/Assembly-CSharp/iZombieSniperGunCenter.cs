using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class iZombieSniperGunCenter
{
	public Dictionary<int, iWeaponInfoBase> m_WeaponInfoBase;

	public Dictionary<int, iWeaponData> m_WeaponData;

	public void Initialize()
	{
		m_WeaponInfoBase = new Dictionary<int, iWeaponInfoBase>();
		m_WeaponInfoBase.Clear();
		m_WeaponData = new Dictionary<int, iWeaponData>();
		m_WeaponData.Clear();
		LoadWeaponBaseInfo();
		LoadWeaponData();
	}

	public void Destroy()
	{
		if (m_WeaponData != null)
		{
			m_WeaponData.Clear();
			m_WeaponData = null;
		}
		if (m_WeaponInfoBase != null)
		{
			m_WeaponInfoBase.Clear();
			m_WeaponInfoBase = null;
		}
	}

	public iWeaponInfoBase GetWeaponInfoBase(int nWeaponID)
	{
		if (m_WeaponInfoBase.ContainsKey(nWeaponID))
		{
			return m_WeaponInfoBase[nWeaponID];
		}
		return null;
	}

	public iWeaponData GetWeaponData(int nWeaponID)
	{
		if (m_WeaponData.ContainsKey(nWeaponID))
		{
			return m_WeaponData[nWeaponID];
		}
		return null;
	}

	private void LoadWeaponBaseInfo()
	{
		XmlDocument xmlDocument = new XmlDocument();
		string text = "ZombieSniper/Config/GunConfig";
		GameObject gameObject = Object.Instantiate(Resources.Load(text)) as GameObject;
		if (gameObject == null)
		{
			Debug.Log("can't find resources " + text);
			return;
		}
		iZombieSniperGunConfig component = gameObject.GetComponent<iZombieSniperGunConfig>();
		if (component == null)
		{
			Debug.Log("can't find script component iZombieSniperGunConfig");
			return;
		}
		xmlDocument.LoadXml(component.m_XmlWeaponInfoBase.text);
		Object.Destroy(gameObject);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (!(childNode.Name != "Item"))
			{
				XmlElement xmlElement = (XmlElement)childNode;
				iWeaponInfoBase iWeaponInfoBase2 = new iWeaponInfoBase();
				iWeaponInfoBase2.m_nWeaponID = int.Parse(xmlElement.GetAttribute("id").Trim());
				iWeaponInfoBase2.m_nWeaponType = int.Parse(xmlElement.GetAttribute("type").Trim());
				iWeaponInfoBase2.m_nIconID = int.Parse(xmlElement.GetAttribute("icon").Trim());
				string text2 = xmlElement.GetAttribute("firesound").Trim();
				if (text2.Length > 0)
				{
					iWeaponInfoBase2.m_sFireSound = text2;
				}
				else if (iWeaponInfoBase2.IsRifle())
				{
					iWeaponInfoBase2.m_sFireSound = "AWPFire01";
				}
				else if (iWeaponInfoBase2.IsAutoShoot())
				{
					iWeaponInfoBase2.m_sFireSound = "RifleFire01";
				}
				else if (iWeaponInfoBase2.IsRocket() || iWeaponInfoBase2.IsThrowMine())
				{
					iWeaponInfoBase2.m_sFireSound = "RocketFire01";
				}
				text2 = xmlElement.GetAttribute("fireeffect").Trim();
				if (text2.Length > 0)
				{
					iWeaponInfoBase2.m_nFireEffect = int.Parse(text2);
				}
				else if (!iWeaponInfoBase2.IsRocket())
				{
					iWeaponInfoBase2.m_nFireEffect = 1;
				}
				else
				{
					iWeaponInfoBase2.m_nFireEffect = 2;
				}
				iWeaponInfoBase2.m_sName = xmlElement.GetAttribute("name").Trim();
				iWeaponInfoBase2.m_sDesc = xmlElement.GetAttribute("desc").Trim();
				iWeaponInfoBase2.m_nCondition = int.Parse(xmlElement.GetAttribute("condition").Trim());
				iWeaponInfoBase2.m_nCondValue = int.Parse(xmlElement.GetAttribute("condvalue").Trim());
				iWeaponInfoBase2.m_bGodPrice = false;
				iWeaponInfoBase2.m_nPrice = 0;
				text2 = xmlElement.GetAttribute("price").Trim();
				if (text2.Length > 0)
				{
					iWeaponInfoBase2.m_bGodPrice = false;
					iWeaponInfoBase2.m_nPrice = int.Parse(text2);
				}
				text2 = xmlElement.GetAttribute("godprice").Trim();
				if (text2.Length > 0 && int.Parse(text2) > 0)
				{
					iWeaponInfoBase2.m_bGodPrice = true;
					iWeaponInfoBase2.m_nPrice = int.Parse(text2);
				}
				iWeaponInfoBase2.m_nBulletMax = int.Parse(xmlElement.GetAttribute("bulletmax").Trim());
				text2 = xmlElement.GetAttribute("shoppriority").Trim();
				if (text2.Length > 0)
				{
					iWeaponInfoBase2.m_nShopPriority = int.Parse(text2);
				}
				text2 = xmlElement.GetAttribute("newweapon").Trim();
				if (text2.Length > 0)
				{
					iWeaponInfoBase2.m_bNewWeapon = bool.Parse(text2);
				}
				else
				{
					iWeaponInfoBase2.m_bNewWeapon = false;
				}
				iWeaponInfoBase2.m_fBaseSD = float.Parse(xmlElement.GetAttribute("baseSD").Trim());
				iWeaponInfoBase2.m_fBaseSR = float.Parse(xmlElement.GetAttribute("baseSR").Trim());
				iWeaponInfoBase2.m_fBaseSG = float.Parse(xmlElement.GetAttribute("baseSG").Trim());
				iWeaponInfoBase2.m_fBaseSZ = float.Parse(xmlElement.GetAttribute("baseSZ").Trim());
				iWeaponInfoBase2.m_fBaseSS = float.Parse(xmlElement.GetAttribute("baseSS").Trim());
				iWeaponInfoBase2.m_fBaseSW = float.Parse(xmlElement.GetAttribute("baseSW").Trim());
				text2 = xmlElement.GetAttribute("nightvision").Trim();
				if (text2.Length > 0)
				{
					iWeaponInfoBase2.m_bNightVision = bool.Parse(text2);
				}
				else
				{
					iWeaponInfoBase2.m_bNightVision = false;
				}
				m_WeaponInfoBase.Add(iWeaponInfoBase2.m_nWeaponID, iWeaponInfoBase2);
			}
		}
	}

	private void LoadWeaponData()
	{
		string content = string.Empty;
		Utils.FileGetString("weapondata.xml", ref content);
		content = XXTEAUtils.Decrypt(content, iZombieSniperGameApp.GetInstance().GetKey());
		if (content.Length < 1)
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(content);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (!(childNode.Name != "Item"))
			{
				XmlElement xmlElement = (XmlElement)childNode;
				iWeaponData iWeaponData2 = new iWeaponData();
				iWeaponData2.m_nWeaponID = int.Parse(xmlElement.GetAttribute("id").Trim());
				iWeaponData2.m_bNew = bool.Parse(xmlElement.GetAttribute("new").Trim());
				m_WeaponData.Add(iWeaponData2.m_nWeaponID, iWeaponData2);
			}
		}
	}

	public void SaveWeaponData()
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlNode newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "no");
		xmlDocument.AppendChild(newChild);
		XmlElement xmlElement = xmlDocument.CreateElement("Weapon");
		foreach (iWeaponData value in m_WeaponData.Values)
		{
			XmlElement xmlElement2 = xmlDocument.CreateElement("Item");
			xmlElement2.SetAttribute("id", value.m_nWeaponID.ToString());
			xmlElement2.SetAttribute("new", value.m_bNew.ToString());
			xmlElement.AppendChild(xmlElement2);
		}
		xmlDocument.AppendChild(xmlElement);
		StringWriter stringWriter = new StringWriter();
		xmlDocument.Save(stringWriter);
		string content = XXTEAUtils.Encrypt(stringWriter.ToString(), iZombieSniperGameApp.GetInstance().GetKey());
		Utils.FileSaveString("weapondata.xml", content);
	}

	public bool PurchaseWeapon(int nWeaponID)
	{
		if (m_WeaponData.ContainsKey(nWeaponID))
		{
			return false;
		}
		iWeaponData iWeaponData2 = new iWeaponData(nWeaponID);
		m_WeaponData.Add(iWeaponData2.m_nWeaponID, iWeaponData2);
		return true;
	}

	public void UpdateWeaponProperty(iWeapon weapon)
	{
		if (weapon.m_nWeaponID != 0)
		{
			iWeaponInfoBase weaponInfoBase = GetWeaponInfoBase(weapon.m_nWeaponID);
			if (weaponInfoBase != null)
			{
				weapon.m_nBulletMax = weaponInfoBase.m_nBulletMax;
				weapon.m_nCurrBulletNum = weapon.m_nBulletMax;
				weapon.m_fSD = weaponInfoBase.m_fBaseSD;
				weapon.m_fSR = weaponInfoBase.m_fBaseSR;
				weapon.m_fSG = weaponInfoBase.m_fBaseSG;
				weapon.m_fSZ = weaponInfoBase.m_fBaseSZ;
				weapon.m_fSS = weaponInfoBase.m_fBaseSS;
				weapon.m_fSW = weaponInfoBase.m_fBaseSW;
			}
		}
	}

	private float WeaponPropertyFormula(float baseValue, int baseLvl, int lvlup)
	{
		return baseValue + baseValue / (float)baseLvl * (float)(baseLvl + lvlup);
	}
}
