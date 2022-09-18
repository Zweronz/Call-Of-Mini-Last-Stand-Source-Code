using System.IO;
using System.Xml;
using UnityEngine;

public class iZombieSniperGameState
{
	public int m_nHDFactor;

	public int m_nScreenWidth;

	public int m_nScreenHeight;

	public SceneEnum m_LastScene;

	public SceneEnum m_CurrScene;

	public SceneEnum m_LoadScene;

	public int m_nKillNorEnemyNum;

	public int m_nKillGiaEnemyNum;

	public int m_nKillInnoNum;

	public int m_nSaveInnoNum;

	public int m_nFireNum;

	public int m_nHeadshotNum;

	public int m_nHitNum;

	public int m_nPlayerCash;

	public int m_nKillEnemyCash;

	public int m_nKillGiaEnemyCash;

	public int m_nSaveInnoCash;

	public int m_nKillInnoCash;

	public int m_nTimeBonus;

	public bool m_bUseAirStrike;

	public bool m_bDestroyAllBoomer;

	public bool m_bUseInnoKiller;

	public AnimInfoListType[] m_AnimInfoListTypeArray;

	public iWeapon m_CurrWeapon;

	public iWeapon[] m_Weapon;

	public iWeapon m_CurrWeaponTool;

	public int m_nCurrWeaponToolID;

	public Vector2 m_v2ScreenCenter;

	public Vector2 m_v3ShootCenter;

	public float m_fAngleTR;

	public float m_fAngleTL;

	public bool[] m_StagePriceisGod;

	public int[] m_StagePrice;

	public string[] m_StageName;

	public bool m_bInitialSave;

	public bool m_bFirstLoading;

	public float m_fTimeInApp;

	public float m_fTimeInGameScene;

	public float m_fVersion;

	public string m_sVersion;

	public int m_nEnterGameCount;

	public bool m_bSoundOn;

	public bool m_bMusicOn;

	public bool m_bIsTiltControl;

	public bool m_bIsInvertYAixs;

	public float m_fCurrentSensitivty;

	public bool m_bTutorial;

	public bool m_bReadyTutorial;

	public bool m_bCutScenes;

	public bool[] m_arrCGFlag;

	public int m_nCurWeaponIndex;

	public int[] m_nCarryWeapon;

	public int m_nPlayerTotalCash;

	public int m_nPlayerTotalGod;

	public int m_nKillNorZombieTotalNum;

	public int m_nKillGiaZombieTotalNum;

	public int m_nKillInnoTotalNum;

	public int m_nSaveInnoTotalNum;

	public int m_nBestKill;

	public bool[] m_arrBestKillNeedSubmit;

	public bool m_bRefreshAchiArr;

	public int[] m_arrAchieve;

	public bool m_bPlayedScene2;

	public int m_nTowerLvl;

	public int m_nTowerBullet;

	public int m_nMineCount;

	public int m_nAC130;

	public int m_nInnoKiller;

	public int m_nMachineGun;

	public int m_nCurStage;

	public int[] m_nStageState;

	public int[] m_nStageBestTime;

	public int[] m_nStageBestKill;

	public void Initialize()
	{
		m_nHDFactor = ((!Utils.IsRetina()) ? 1 : 2);
		m_nScreenWidth = 480 * m_nHDFactor;
		m_nScreenHeight = 320 * m_nHDFactor;
		m_v2ScreenCenter = new Vector2(m_nScreenWidth / 2, m_nScreenHeight / 2);
		m_v3ShootCenter = new Vector2(Screen.width / 2, Screen.height / 2);
		m_fAngleTR = Vector2.Dot((new Vector2(m_nScreenWidth, 0f) - m_v3ShootCenter).normalized, Vector2.right);
		m_fAngleTL = Vector2.Dot((new Vector2(0f, 0f) - m_v3ShootCenter).normalized, Vector2.right);
		m_nCarryWeapon = new int[3];
		m_Weapon = new iWeapon[3];
		m_nStageState = new int[3];
		m_nStageBestTime = new int[3];
		m_nStageBestKill = new int[3];
		m_arrAchieve = new int[18];
		m_arrBestKillNeedSubmit = new bool[3];
		m_arrCGFlag = new bool[3];
		m_StagePriceisGod = new bool[3];
		m_StagePrice = new int[3] { 0, 10000, 100 };
		m_StageName = new string[3] { "MUERTE VISTA", "CRIMSON SPRINGS", "SCENE THREE" };
		m_bInitialSave = false;
		ResetGameDate();
		ResetData();
		LoadData();
		LoadAnimInfo();
		if (m_sVersion != "1.1")
		{
			m_sVersion = "1.1";
			m_bInitialSave = true;
			NewVersionUpdate();
		}
		if (m_bInitialSave)
		{
			SaveData();
			m_bInitialSave = false;
		}
		for (int i = 0; i < m_nStageState.Length; i++)
		{
			m_nStageState[i] = 1;
		}
		m_bFirstLoading = true;
	}

	public void ResetGameDate()
	{
		m_nKillNorEnemyNum = 0;
		m_nKillGiaEnemyNum = 0;
		m_nKillInnoNum = 0;
		m_nSaveInnoNum = 0;
		m_nFireNum = 0;
		m_nHeadshotNum = 0;
		m_nHitNum = 0;
		m_nPlayerCash = 0;
		m_nKillEnemyCash = 0;
		m_nKillGiaEnemyCash = 0;
		m_nSaveInnoCash = 0;
		m_nKillInnoCash = 0;
		m_nTimeBonus = 0;
		m_bUseAirStrike = false;
		m_bDestroyAllBoomer = false;
		m_bUseInnoKiller = false;
		m_nCurWeaponIndex = 0;
		m_nCurrWeaponToolID = -1;
		iWeapon[] weapon = m_Weapon;
		foreach (iWeapon iWeapon2 in weapon)
		{
			if (iWeapon2 != null)
			{
				iWeapon2.m_WeaponState = iWeapon.WeaponState.kNormal;
				iWeapon2.m_fTimeCount = 0f;
				iWeapon2.m_nCurrBulletNum = iWeapon2.m_nBulletMax;
			}
		}
	}

	public void ResetData()
	{
		m_nCurStage = 0;
		m_nStageState[0] = 1;
		m_nEnterGameCount = 0;
		m_bRefreshAchiArr = true;
		m_bIsTiltControl = false;
		m_fCurrentSensitivty = 27.5f;
		m_nPlayerTotalCash = 0;
		if (iZombieSniperGameApp.GetInstance().m_IAPMode == iZombieSniperGameApp.kIAPMode.Amazon)
		{
			m_nPlayerTotalGod = 10;
		}
		else
		{
			m_nPlayerTotalGod = 0;
		}
		m_nKillNorZombieTotalNum = 0;
		m_nKillGiaZombieTotalNum = 0;
		m_nKillInnoTotalNum = 0;
		m_nSaveInnoTotalNum = 0;
		m_nBestKill = 0;
		for (int i = 0; i < 3; i++)
		{
			m_arrCGFlag[i] = true;
			m_arrBestKillNeedSubmit[i] = true;
		}
		m_nTowerLvl = 0;
		m_nTowerBullet = 10;
		m_nMineCount = 0;
		m_nAC130 = 0;
		m_nInnoKiller = 0;
		m_nMachineGun = 1;
		m_bSoundOn = true;
		m_bMusicOn = true;
		m_bTutorial = true;
		m_bReadyTutorial = true;
		m_bCutScenes = true;
		m_fTimeInApp = 0f;
		m_fTimeInGameScene = 0f;
		m_bPlayedScene2 = false;
	}

	public void LoadData()
	{
		iZombieSniperGunCenter gunCenter = iZombieSniperGameApp.GetInstance().m_GunCenter;
		string content = string.Empty;
		Utils.FileGetString("gamedata.xml", ref content);
		if (content.Length < 1)
		{
			gunCenter.PurchaseWeapon(1);
			gunCenter.PurchaseWeapon(11);
			m_Weapon[0] = new iWeapon(1);
			gunCenter.UpdateWeaponProperty(m_Weapon[0]);
			m_Weapon[1] = new iWeapon(11);
			gunCenter.UpdateWeaponProperty(m_Weapon[1]);
			gunCenter.SaveWeaponData();
			SaveData();
			return;
		}
		content = XXTEAUtils.Decrypt(content, iZombieSniperGameApp.GetInstance().GetKey());
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(content);
		XmlNode documentElement = xmlDocument.DocumentElement;
		XmlElement xmlElement = (XmlElement)documentElement;
		string text = xmlElement.GetAttribute("Version").Trim();
		if (text.Length > 0)
		{
			m_fVersion = float.Parse(text);
		}
		text = xmlElement.GetAttribute("VersionDesc").Trim();
		if (text.Length > 0)
		{
			m_sVersion = text;
		}
		text = xmlElement.GetAttribute("bRefreshAchiArr").Trim();
		if (text.Length > 0)
		{
			m_bRefreshAchiArr = bool.Parse(text);
		}
		text = xmlElement.GetAttribute("nEnterGameCount").Trim();
		if (text.Length > 0)
		{
			m_nEnterGameCount = int.Parse(text);
		}
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			XmlElement xmlElement2 = (XmlElement)childNode;
			if ("Option" == xmlElement2.Name)
			{
				text = xmlElement2.GetAttribute("Sound").Trim();
				if (text.Length > 0)
				{
					m_bSoundOn = bool.Parse(text);
				}
				text = xmlElement2.GetAttribute("Music").Trim();
				if (text.Length > 0)
				{
					m_bMusicOn = bool.Parse(text);
				}
				text = xmlElement2.GetAttribute("Turtorial").Trim();
				if (text.Length > 0)
				{
					m_bTutorial = bool.Parse(text);
				}
				text = xmlElement2.GetAttribute("ReadyTurtorial").Trim();
				if (text.Length > 0)
				{
					m_bReadyTutorial = bool.Parse(text);
				}
				text = xmlElement2.GetAttribute("Cutscenes").Trim();
				if (text.Length > 0)
				{
					m_bCutScenes = bool.Parse(text);
				}
				text = xmlElement2.GetAttribute("CGFlag").Trim();
				if (text.Length > 0)
				{
					string[] array = text.Split(',');
					for (int i = 0; i < array.Length && i < m_arrCGFlag.Length; i++)
					{
						m_arrCGFlag[i] = bool.Parse(array[i].Trim());
					}
				}
			}
			else if ("Player" == xmlElement2.Name)
			{
				text = xmlElement2.GetAttribute("CarryWeapon").Trim();
				string[] array2 = text.Split(',');
				for (int j = 0; j < array2.Length && j < 3; j++)
				{
					m_nCarryWeapon[j] = int.Parse(array2[j].Trim());
					if (m_nCarryWeapon[j] != 0 && gunCenter.GetWeaponData(m_nCarryWeapon[j]) != null)
					{
						m_Weapon[j] = new iWeapon(m_nCarryWeapon[j]);
						gunCenter.UpdateWeaponProperty(m_Weapon[j]);
					}
				}
				text = xmlElement2.GetAttribute("TotalCash").Trim();
				if (text.Length > 0)
				{
					m_nPlayerTotalCash = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("TotalGod").Trim();
				if (text.Length > 0)
				{
					m_nPlayerTotalGod = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("TotalKillNorZombie").Trim();
				if (text.Length > 0)
				{
					m_nKillNorZombieTotalNum = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("TotalKillGiaZombie").Trim();
				if (text.Length > 0)
				{
					m_nKillGiaZombieTotalNum = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("TotalKillInno").Trim();
				if (text.Length > 0)
				{
					m_nKillInnoTotalNum = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("TotalInno").Trim();
				if (text.Length > 0)
				{
					m_nSaveInnoTotalNum = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("BestKill").Trim();
				if (text.Length > 0)
				{
					m_nBestKill = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("BestKillNeedSubmit").Trim();
				if (text.Length > 0)
				{
					array2 = text.Split(',');
					for (int k = 0; k < array2.Length && k < m_arrBestKillNeedSubmit.Length; k++)
					{
						m_arrBestKillNeedSubmit[k] = bool.Parse(array2[k]);
					}
				}
				text = xmlElement2.GetAttribute("Achieve").Trim();
				if (text.Length > 0)
				{
					array2 = text.Split(',');
					if (m_bRefreshAchiArr)
					{
						for (int l = 0; l < array2.Length; l++)
						{
							int num = int.Parse(array2[l].Trim());
							if (num >= 0 && num < m_arrAchieve.Length)
							{
								m_arrAchieve[num] = 1;
							}
						}
						m_bRefreshAchiArr = false;
						m_bInitialSave = true;
					}
					else
					{
						for (int m = 0; m < array2.Length && m < m_arrAchieve.Length; m++)
						{
							m_arrAchieve[m] = int.Parse(array2[m].Trim());
						}
					}
				}
				text = xmlElement2.GetAttribute("isPlayedScene2").Trim();
				if (text.Length > 0)
				{
					m_bPlayedScene2 = bool.Parse(text);
				}
			}
			else if ("DefencePower" == xmlElement2.Name)
			{
				text = xmlElement2.GetAttribute("TowerDmg").Trim();
				if (text.Length > 0)
				{
					m_nTowerLvl = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("TowerBullet").Trim();
				if (text.Length > 0)
				{
					m_nTowerBullet = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("AC130").Trim();
				if (text.Length > 0)
				{
					m_nAC130 = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("LANDMINE").Trim();
				if (text.Length > 0)
				{
					m_nMineCount = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("INNOKILLER").Trim();
				if (text.Length > 0)
				{
					m_nInnoKiller = int.Parse(text);
				}
				text = xmlElement2.GetAttribute("MACHINEGUN").Trim();
				if (text.Length > 0)
				{
					m_nMachineGun = int.Parse(text);
				}
			}
			else
			{
				if (!("StageData" == xmlElement2.Name))
				{
					continue;
				}
				text = xmlElement2.GetAttribute("Stage").Trim();
				if (text.Length > 0)
				{
					m_nCurStage = int.Parse(text);
				}
				for (int n = 0; n < m_nStageState.Length && n < m_nStageBestTime.Length && n < m_nStageBestKill.Length; n++)
				{
					text = xmlElement2.GetAttribute("s" + n).Trim();
					if (text.Length >= 1)
					{
						string[] array3 = text.Split(',');
						if (array3.Length >= 3)
						{
							m_nStageState[n] = int.Parse(array3[0]);
							m_nStageBestTime[n] = int.Parse(array3[1]);
							m_nStageBestKill[n] = int.Parse(array3[2]);
						}
					}
				}
			}
		}
	}

	public void SaveData()
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlNode newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "no");
		xmlDocument.AppendChild(newChild);
		string empty = string.Empty;
		XmlElement xmlElement = xmlDocument.CreateElement("Game");
		xmlElement.SetAttribute("Version", m_fVersion.ToString());
		xmlElement.SetAttribute("VersionDesc", m_sVersion);
		xmlElement.SetAttribute("bRefreshAchiArr", m_bRefreshAchiArr.ToString());
		xmlElement.SetAttribute("nEnterGameCount", m_nEnterGameCount.ToString());
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement("Option");
		xmlElement2.SetAttribute("TiltCtrl", m_bIsTiltControl.ToString());
		xmlElement2.SetAttribute("InvertYAixs", m_bIsInvertYAixs.ToString());
		xmlElement2.SetAttribute("Sensitivity", m_fCurrentSensitivty.ToString());
		xmlElement2.SetAttribute("Sound", m_bSoundOn.ToString());
		xmlElement2.SetAttribute("Music", m_bMusicOn.ToString());
		xmlElement2.SetAttribute("Turtorial", m_bTutorial.ToString());
		xmlElement2.SetAttribute("ReadyTurtorial", m_bReadyTutorial.ToString());
		xmlElement2.SetAttribute("Cutscenes", m_bCutScenes.ToString());
		empty = string.Empty;
		for (int i = 0; i < m_arrCGFlag.Length; i++)
		{
			empty = ((i != 0) ? (empty + "," + m_arrCGFlag[i]) : m_arrCGFlag[i].ToString());
		}
		xmlElement2.SetAttribute("CGFlag", empty);
		xmlElement.AppendChild(xmlElement2);
		XmlElement xmlElement3 = xmlDocument.CreateElement("Player");
		xmlElement3.SetAttribute("CarryWeapon", m_nCarryWeapon[0] + "," + m_nCarryWeapon[1] + "," + m_nCarryWeapon[2]);
		xmlElement3.SetAttribute("TotalCash", m_nPlayerTotalCash.ToString());
		xmlElement3.SetAttribute("TotalGod", m_nPlayerTotalGod.ToString());
		xmlElement3.SetAttribute("TotalKillNorZombie", m_nKillNorZombieTotalNum.ToString());
		xmlElement3.SetAttribute("TotalKillGiaZombie", m_nKillGiaZombieTotalNum.ToString());
		xmlElement3.SetAttribute("TotalKillInno", m_nKillInnoTotalNum.ToString());
		xmlElement3.SetAttribute("TotalInno", m_nSaveInnoTotalNum.ToString());
		xmlElement3.SetAttribute("BestKill", m_nBestKill.ToString());
		empty = string.Empty;
		for (int j = 0; j < m_arrBestKillNeedSubmit.Length; j++)
		{
			empty = ((j != 0) ? (empty + "," + m_arrBestKillNeedSubmit[j]) : m_arrBestKillNeedSubmit[j].ToString());
		}
		xmlElement3.SetAttribute("BestKillNeedSubmit", empty);
		empty = string.Empty;
		for (int k = 0; k < m_arrAchieve.Length; k++)
		{
			empty = ((k != 0) ? (empty + "," + m_arrAchieve[k]) : m_arrAchieve[k].ToString());
		}
		xmlElement3.SetAttribute("Achieve", empty);
		xmlElement3.SetAttribute("isPlayedScene2", m_bPlayedScene2.ToString());
		xmlElement.AppendChild(xmlElement3);
		XmlElement xmlElement4 = xmlDocument.CreateElement("DefencePower");
		xmlElement4.SetAttribute("TowerDmg", m_nTowerLvl.ToString());
		xmlElement4.SetAttribute("TowerBullet", m_nTowerBullet.ToString());
		xmlElement4.SetAttribute("AC130", m_nAC130.ToString());
		xmlElement4.SetAttribute("LANDMINE", m_nMineCount.ToString());
		xmlElement4.SetAttribute("INNOKILLER", m_nInnoKiller.ToString());
		xmlElement4.SetAttribute("MACHINEGUN", m_nMachineGun.ToString());
		xmlElement.AppendChild(xmlElement4);
		XmlElement xmlElement5 = xmlDocument.CreateElement("StageData");
		xmlElement5.SetAttribute("Stage", m_nCurStage.ToString());
		empty = string.Empty;
		for (int l = 0; l < m_nStageState.Length && l < m_nStageBestTime.Length && l < m_nStageBestKill.Length; l++)
		{
			empty = m_nStageState[l] + "," + m_nStageBestTime[l] + "," + m_nStageBestKill[l];
			xmlElement5.SetAttribute("s" + l, empty);
		}
		xmlElement.AppendChild(xmlElement5);
		StringWriter stringWriter = new StringWriter();
		xmlDocument.Save(stringWriter);
		string content = XXTEAUtils.Encrypt(stringWriter.ToString(), iZombieSniperGameApp.GetInstance().GetKey());
		Utils.FileSaveString("gamedata.xml", content);
	}

	public bool SwitchWeapon(int nIndex)
	{
		if (nIndex < 0 || nIndex >= 3)
		{
			return false;
		}
		if (m_Weapon[nIndex] == null)
		{
			return false;
		}
		m_nCurWeaponIndex = nIndex;
		m_nCurrWeaponToolID = -1;
		return true;
	}

	public bool SwitchWeapon()
	{
		int num = m_nCurWeaponIndex;
		if (!IsInWeaponTool())
		{
			do
			{
				num++;
				if (num > m_Weapon.Length - 1)
				{
					num = 0;
				}
				if (num == m_nCurWeaponIndex)
				{
					return false;
				}
			}
			while (m_Weapon[num] == null);
		}
		m_nCurWeaponIndex = num;
		m_nCurrWeaponToolID = -1;
		return true;
	}

	public bool IsInWeaponTool()
	{
		return m_nCurrWeaponToolID != -1;
	}

	public bool SwitchWeaponByID(int nWeaponID)
	{
		iZombieSniperGunCenter gunCenter = iZombieSniperGameApp.GetInstance().m_GunCenter;
		if (gunCenter == null)
		{
			return false;
		}
		iWeaponInfoBase weaponInfoBase = gunCenter.GetWeaponInfoBase(nWeaponID);
		if (weaponInfoBase == null)
		{
			return false;
		}
		if (m_CurrWeaponTool == null || m_nCurrWeaponToolID != nWeaponID)
		{
			m_CurrWeaponTool = new iWeapon(nWeaponID);
			gunCenter.UpdateWeaponProperty(m_CurrWeaponTool);
		}
		m_nCurrWeaponToolID = nWeaponID;
		return true;
	}

	public iWeapon GetUserWeapon()
	{
		if (m_nCurrWeaponToolID != -1)
		{
			return m_CurrWeaponTool;
		}
		if (m_nCurWeaponIndex < 0 || m_nCurWeaponIndex >= m_Weapon.Length)
		{
			return null;
		}
		return m_Weapon[m_nCurWeaponIndex];
	}

	public iWeapon GetUserWeapon(int nIndex)
	{
		if (nIndex < 0 || nIndex >= m_Weapon.Length)
		{
			return null;
		}
		return m_Weapon[nIndex];
	}

	public int AdjustZombiePrice(int nPrice)
	{
		switch (m_nCurStage)
		{
		case 1:
			nPrice = (int)((float)nPrice * 2f);
			break;
		}
		return nPrice;
	}

	public void CaculatePrice(float fGameTime)
	{
		m_nTimeBonus = 0;
		m_nKillEnemyCash = m_nKillNorEnemyNum * AdjustZombiePrice(10);
		m_nTimeBonus += IncomeFormula(m_nKillEnemyCash, fGameTime) - m_nKillEnemyCash;
		m_nKillGiaEnemyCash = m_nKillGiaEnemyNum * AdjustZombiePrice(100);
		m_nTimeBonus += IncomeFormula(m_nKillGiaEnemyCash, fGameTime) - m_nKillGiaEnemyCash;
		m_nSaveInnoCash = m_nSaveInnoNum * AdjustZombiePrice(50);
		m_nKillInnoCash = m_nKillInnoNum * AdjustZombiePrice(-50);
		if (m_nTimeBonus < 0)
		{
			m_nTimeBonus = 0;
		}
		m_nPlayerCash = m_nKillEnemyCash + m_nKillGiaEnemyCash + m_nSaveInnoCash + m_nKillInnoCash + m_nTimeBonus;
		m_nPlayerTotalCash += m_nPlayerCash;
		m_nKillNorZombieTotalNum += m_nKillNorEnemyNum;
		m_nKillGiaZombieTotalNum += m_nKillGiaEnemyNum;
		m_nKillInnoTotalNum += m_nKillInnoNum;
		m_nSaveInnoTotalNum += m_nSaveInnoNum;
		int num = m_nKillNorZombieTotalNum + m_nKillGiaZombieTotalNum;
		int num2 = m_nKillNorEnemyNum + m_nKillGiaEnemyNum;
		int bestKill = GetBestKill(m_nCurStage);
		if (num2 > bestKill)
		{
			bestKill = num2;
			SetBestKillSubmitFlag(m_nCurStage, true);
			SetBestKill(m_nCurStage, bestKill);
			switch (m_nCurStage)
			{
			case 0:
				iZombieSniperGameApp.GetInstance().m_Ranking.CompleteRanking(CRanking.RankEnum.Scene0, bestKill);
				break;
			case 1:
				iZombieSniperGameApp.GetInstance().m_Ranking.CompleteRanking(CRanking.RankEnum.Scene1, bestKill);
				break;
			case 2:
				iZombieSniperGameApp.GetInstance().m_Ranking.CompleteRanking(CRanking.RankEnum.Scene2, bestKill);
				break;
			}
		}
		if (m_nCurStage >= 0 && m_nCurStage < m_nStageBestKill.Length && num2 > m_nStageBestKill[m_nCurStage])
		{
			m_nStageBestKill[m_nCurStage] = num2;
		}
		if (m_nCurStage >= 0 && m_nCurStage < m_nStageBestTime.Length && fGameTime > (float)m_nStageBestTime[m_nCurStage])
		{
			m_nStageBestTime[m_nCurStage] = (int)fGameTime;
		}
		if (m_nCurStage >= 0 && m_nCurStage < 2 && m_nStageState[m_nCurStage + 1] == 0)
		{
			m_nStageState[m_nCurStage + 1] = 1;
		}
		if (m_nKillInnoTotalNum >= 10)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi1);
		}
		if (m_nKillInnoTotalNum >= 100)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi2);
		}
		if (m_nKillInnoTotalNum >= 500)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi3);
		}
		if (num >= 100)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi4);
		}
		if (num >= 10000)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi5);
		}
		if (m_nSaveInnoTotalNum >= 100)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi6);
		}
		if (m_nSaveInnoTotalNum >= 1000)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi7);
		}
		if (m_bUseAirStrike)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi8);
		}
		if (fGameTime >= 1800f)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi11);
		}
		if (m_bDestroyAllBoomer)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi12);
		}
		if (num >= 500)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi13);
		}
		if (num >= 1000)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi14);
		}
		if (num >= 5000)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi15);
		}
		if (m_nSaveInnoTotalNum >= 10)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi16);
		}
		if (m_nSaveInnoTotalNum >= 50)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi17);
		}
		if (m_nSaveInnoTotalNum >= 500)
		{
			iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement(CAchievement.AchiEnum.Achi18);
		}
	}

	private int IncomeFormula(int nInCome, float fGameTime)
	{
		int num = Mathf.Max(0, (int)(fGameTime / 180f));
		if (num <= 0)
		{
			return nInCome;
		}
		return (int)((float)nInCome * (1f + (float)num * 0.1f));
	}

	public void LoadAnimInfo()
	{
		m_AnimInfoListTypeArray = new AnimInfoListType[12];
		m_AnimInfoListTypeArray[1] = new AnimInfoListType(1);
		m_AnimInfoListTypeArray[1].Add(1, new AnimInfo("Idle01", 1f, 24));
		m_AnimInfoListTypeArray[1].Add(2, new AnimInfo("Forward01", 0.5f, 24));
		m_AnimInfoListTypeArray[1].Add(3, new AnimInfo("Run01", 1f, 24));
		m_AnimInfoListTypeArray[1].Add(4, new AnimInfo("Attack01", 1f, 24));
		m_AnimInfoListTypeArray[1].Add(7, new AnimInfo("Death03", 1f, 24));
		m_AnimInfoListTypeArray[1].Add(9, new AnimInfo("Death01", 1f, 24));
		m_AnimInfoListTypeArray[1].Add(9, new AnimInfo("Death02", 1f, 24));
		m_AnimInfoListTypeArray[1].Add(8, new AnimInfo("Death04", 1f, 24));
		m_AnimInfoListTypeArray[1].Add(10, new AnimInfo("Death06", 1f, 24));
		m_AnimInfoListTypeArray[1].Add(11, new AnimInfo("Death05", 1f, 24));
		m_AnimInfoListTypeArray[1].Add(12, new AnimInfo("Death07", 1f, 24));
		m_AnimInfoListTypeArray[1].Add(6, new AnimInfo("Response01", 1f, 24));
		m_AnimInfoListTypeArray[1].End();
		m_AnimInfoListTypeArray[2] = new AnimInfoListType(2);
		m_AnimInfoListTypeArray[2].Add(1, new AnimInfo("Idle01", 1f, 24));
		m_AnimInfoListTypeArray[2].Add(2, new AnimInfo("Forward01", 0.5f, 24));
		m_AnimInfoListTypeArray[2].Add(3, new AnimInfo("Run01", 1f, 24));
		m_AnimInfoListTypeArray[2].Add(4, new AnimInfo("Attack01", 1f, 24));
		m_AnimInfoListTypeArray[2].Add(7, new AnimInfo("Death03", 1f, 24));
		m_AnimInfoListTypeArray[2].Add(9, new AnimInfo("Death01", 1f, 24));
		m_AnimInfoListTypeArray[2].Add(9, new AnimInfo("Death02", 1f, 24));
		m_AnimInfoListTypeArray[2].Add(8, new AnimInfo("Death04", 1f, 24));
		m_AnimInfoListTypeArray[2].Add(10, new AnimInfo("Death06", 1f, 24));
		m_AnimInfoListTypeArray[2].Add(11, new AnimInfo("Death05", 1f, 24));
		m_AnimInfoListTypeArray[2].Add(12, new AnimInfo("Death07", 1f, 24));
		m_AnimInfoListTypeArray[2].Add(6, new AnimInfo("Response01", 1f, 24));
		m_AnimInfoListTypeArray[2].End();
		m_AnimInfoListTypeArray[3] = new AnimInfoListType(3);
		m_AnimInfoListTypeArray[3].Add(1, new AnimInfo("Idle01", 1f, 24));
		m_AnimInfoListTypeArray[3].Add(2, new AnimInfo("Forward01", 0.5f, 24));
		m_AnimInfoListTypeArray[3].Add(3, new AnimInfo("Run01", 1f, 24));
		m_AnimInfoListTypeArray[3].Add(4, new AnimInfo("Attack01", 1f, 24));
		m_AnimInfoListTypeArray[3].Add(7, new AnimInfo("Death03", 1f, 24));
		m_AnimInfoListTypeArray[3].Add(9, new AnimInfo("Death01", 1f, 24));
		m_AnimInfoListTypeArray[3].Add(9, new AnimInfo("Death02", 1f, 24));
		m_AnimInfoListTypeArray[3].Add(8, new AnimInfo("Death04", 1f, 24));
		m_AnimInfoListTypeArray[3].Add(10, new AnimInfo("Death06", 1f, 24));
		m_AnimInfoListTypeArray[3].Add(11, new AnimInfo("Death05", 1f, 24));
		m_AnimInfoListTypeArray[3].Add(12, new AnimInfo("Death07", 1f, 24));
		m_AnimInfoListTypeArray[3].Add(6, new AnimInfo("Response01", 1f, 24));
		m_AnimInfoListTypeArray[3].End();
		m_AnimInfoListTypeArray[4] = new AnimInfoListType(4);
		m_AnimInfoListTypeArray[4].Add(1, new AnimInfo("Idle01", 1f, 20));
		m_AnimInfoListTypeArray[4].Add(2, new AnimInfo("Forward01", 0.5f, 20));
		m_AnimInfoListTypeArray[4].Add(3, new AnimInfo("Run01", 1f, 20));
		m_AnimInfoListTypeArray[4].Add(4, new AnimInfo("Attack01", 1f, 20));
		m_AnimInfoListTypeArray[4].Add(7, new AnimInfo("Death03", 1f, 20));
		m_AnimInfoListTypeArray[4].Add(9, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[4].Add(9, new AnimInfo("Death02", 1f, 20));
		m_AnimInfoListTypeArray[4].Add(8, new AnimInfo("Death04", 1f, 20));
		m_AnimInfoListTypeArray[4].Add(10, new AnimInfo("Death06", 1f, 20));
		m_AnimInfoListTypeArray[4].Add(11, new AnimInfo("Death05", 1f, 20));
		m_AnimInfoListTypeArray[4].Add(12, new AnimInfo("Death07", 1f, 20));
		m_AnimInfoListTypeArray[4].Add(6, new AnimInfo("Response01", 1f, 20));
		m_AnimInfoListTypeArray[4].End();
		m_AnimInfoListTypeArray[5] = new AnimInfoListType(5);
		m_AnimInfoListTypeArray[5].Add(1, new AnimInfo("Idle01", 1f, 20));
		m_AnimInfoListTypeArray[5].Add(2, new AnimInfo("Forward01", 0.5f, 20));
		m_AnimInfoListTypeArray[5].Add(3, new AnimInfo("Forward05", 1f, 20));
		m_AnimInfoListTypeArray[5].Add(4, new AnimInfo("Attack01", 1f, 20));
		m_AnimInfoListTypeArray[5].Add(7, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[5].Add(7, new AnimInfo("Death03", 1f, 20));
		m_AnimInfoListTypeArray[5].Add(9, new AnimInfo("Death02", 1f, 20));
		m_AnimInfoListTypeArray[5].Add(8, new AnimInfo("Death03", 1f, 20));
		m_AnimInfoListTypeArray[5].Add(10, new AnimInfo("death05", 1f, 20));
		m_AnimInfoListTypeArray[5].Add(11, new AnimInfo("death04", 1f, 20));
		m_AnimInfoListTypeArray[5].Add(12, new AnimInfo("Death03", 1f, 20));
		m_AnimInfoListTypeArray[5].Add(6, new AnimInfo("Rush01", 1f, 20));
		m_AnimInfoListTypeArray[5].End();
		m_AnimInfoListTypeArray[6] = new AnimInfoListType(6);
		m_AnimInfoListTypeArray[6].Add(1, new AnimInfo("Idle01", 1f, 20));
		m_AnimInfoListTypeArray[6].Add(2, new AnimInfo("Run01", 1.5f, 20));
		m_AnimInfoListTypeArray[6].Add(3, new AnimInfo("Run01", 1.5f, 20));
		m_AnimInfoListTypeArray[6].Add(4, new AnimInfo("Attack01", 1f, 20));
		m_AnimInfoListTypeArray[6].Add(7, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[6].Add(9, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[6].Add(8, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[6].Add(10, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[6].Add(11, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[6].Add(12, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[6].Add(6, new AnimInfo("Search", 1f, 20));
		m_AnimInfoListTypeArray[6].Add(6, new AnimInfo("Smell", 1f, 20));
		m_AnimInfoListTypeArray[6].End();
		m_AnimInfoListTypeArray[7] = new AnimInfoListType(7);
		m_AnimInfoListTypeArray[7].Add(1, new AnimInfo("Idle01", 1f, 20));
		m_AnimInfoListTypeArray[7].Add(2, new AnimInfo("Run01", 1f, 20));
		m_AnimInfoListTypeArray[7].Add(3, new AnimInfo("Run01", 3f, 10));
		m_AnimInfoListTypeArray[7].Add(4, new AnimInfo("Tear01", 1f, 20));
		m_AnimInfoListTypeArray[7].Add(7, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[7].Add(8, new AnimInfo("Death02", 1f, 20));
		m_AnimInfoListTypeArray[7].Add(9, new AnimInfo("Death04", 1f, 20));
		m_AnimInfoListTypeArray[7].Add(8, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[7].Add(10, new AnimInfo("Death05", 1f, 20));
		m_AnimInfoListTypeArray[7].Add(11, new AnimInfo("Death05", 1f, 20));
		m_AnimInfoListTypeArray[7].Add(12, new AnimInfo("Death03", 1f, 20));
		m_AnimInfoListTypeArray[7].End();
		m_AnimInfoListTypeArray[8] = new AnimInfoListType(8);
		m_AnimInfoListTypeArray[8].Add(1, new AnimInfo("Idle01", 1f, 20));
		m_AnimInfoListTypeArray[8].Add(2, new AnimInfo("Run01", 1f, 20));
		m_AnimInfoListTypeArray[8].Add(3, new AnimInfo("Run01", 3f, 10));
		m_AnimInfoListTypeArray[8].Add(4, new AnimInfo("Tear01", 1f, 20));
		m_AnimInfoListTypeArray[8].Add(7, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[8].Add(8, new AnimInfo("Death02", 1f, 20));
		m_AnimInfoListTypeArray[8].Add(9, new AnimInfo("Death04", 1f, 20));
		m_AnimInfoListTypeArray[8].Add(8, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[8].Add(10, new AnimInfo("Death05", 1f, 20));
		m_AnimInfoListTypeArray[8].Add(11, new AnimInfo("Death05", 1f, 20));
		m_AnimInfoListTypeArray[8].Add(12, new AnimInfo("Death03", 1f, 20));
		m_AnimInfoListTypeArray[8].End();
		m_AnimInfoListTypeArray[9] = new AnimInfoListType(9);
		m_AnimInfoListTypeArray[9].Add(1, new AnimInfo("Idle01", 1f, 20));
		m_AnimInfoListTypeArray[9].Add(2, new AnimInfo("Run01", 1f, 20));
		m_AnimInfoListTypeArray[9].Add(3, new AnimInfo("Run01", 3f, 10));
		m_AnimInfoListTypeArray[9].Add(4, new AnimInfo("Tear01", 1f, 20));
		m_AnimInfoListTypeArray[9].Add(7, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[9].Add(8, new AnimInfo("Death02", 1f, 20));
		m_AnimInfoListTypeArray[9].Add(9, new AnimInfo("Death04", 1f, 20));
		m_AnimInfoListTypeArray[9].Add(8, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[9].Add(10, new AnimInfo("Death05", 1f, 20));
		m_AnimInfoListTypeArray[9].Add(11, new AnimInfo("Death05", 1f, 20));
		m_AnimInfoListTypeArray[9].Add(12, new AnimInfo("Death03", 1f, 20));
		m_AnimInfoListTypeArray[9].End();
		m_AnimInfoListTypeArray[10] = new AnimInfoListType(10);
		m_AnimInfoListTypeArray[10].Add(1, new AnimInfo("Idle01", 1f, 20));
		m_AnimInfoListTypeArray[10].Add(2, new AnimInfo("Run01", 1f, 20));
		m_AnimInfoListTypeArray[10].Add(3, new AnimInfo("Run01", 3f, 10));
		m_AnimInfoListTypeArray[10].Add(4, new AnimInfo("Tear01", 1f, 20));
		m_AnimInfoListTypeArray[10].Add(7, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[10].Add(8, new AnimInfo("Death02", 1f, 20));
		m_AnimInfoListTypeArray[10].Add(9, new AnimInfo("Death04", 1f, 20));
		m_AnimInfoListTypeArray[10].Add(8, new AnimInfo("Death01", 1f, 20));
		m_AnimInfoListTypeArray[10].Add(10, new AnimInfo("Death05", 1f, 20));
		m_AnimInfoListTypeArray[10].Add(11, new AnimInfo("Death05", 1f, 20));
		m_AnimInfoListTypeArray[10].Add(12, new AnimInfo("Death03", 1f, 20));
		m_AnimInfoListTypeArray[10].End();
		m_AnimInfoListTypeArray[11] = new AnimInfoListType(11);
		m_AnimInfoListTypeArray[11].Add(1, new AnimInfo("Idle01", 1f, 24));
		m_AnimInfoListTypeArray[11].Add(2, new AnimInfo("Run01", 5f, 24));
		m_AnimInfoListTypeArray[11].Add(3, new AnimInfo("Run01", 5f, 24));
		m_AnimInfoListTypeArray[11].Add(4, new AnimInfo("Attack01", 1f, 24));
		m_AnimInfoListTypeArray[11].Add(7, new AnimInfo("Death01", 1f, 24));
		m_AnimInfoListTypeArray[11].Add(9, new AnimInfo("Death02", 1f, 24));
		m_AnimInfoListTypeArray[11].Add(8, new AnimInfo("Death01", 1f, 24));
		m_AnimInfoListTypeArray[11].Add(10, new AnimInfo("Death01", 1f, 24));
		m_AnimInfoListTypeArray[11].Add(11, new AnimInfo("Death01", 1f, 24));
		m_AnimInfoListTypeArray[11].Add(12, new AnimInfo("Death01", 1f, 24));
		m_AnimInfoListTypeArray[11].Add(6, new AnimInfo("Idle01", 1f, 24));
		m_AnimInfoListTypeArray[11].End();
	}

	public AnimInfo GetAnimInfo(int nNpcType, int nActionType, int nIndex = -1)
	{
		if (m_AnimInfoListTypeArray[nNpcType] == null)
		{
			return null;
		}
		AnimInfoList info = m_AnimInfoListTypeArray[nNpcType].GetInfo(nActionType);
		if (info == null)
		{
			return null;
		}
		AnimInfo info2 = info.GetInfo(nIndex);
		if (info2 == null)
		{
			return null;
		}
		return info2;
	}

	public void CarryWeapon(int nIndex, int nWeaponID)
	{
		if (nIndex < 0 || nIndex >= m_nCarryWeapon.Length)
		{
			return;
		}
		m_nCarryWeapon[nIndex] = nWeaponID;
		if (nWeaponID > 0)
		{
			m_Weapon[nIndex] = new iWeapon(nWeaponID);
			iZombieSniperGunCenter gunCenter = iZombieSniperGameApp.GetInstance().m_GunCenter;
			if (gunCenter != null)
			{
				gunCenter.UpdateWeaponProperty(m_Weapon[nIndex]);
				iWeaponData weaponData = gunCenter.GetWeaponData(nWeaponID);
				if (weaponData != null)
				{
					weaponData.m_bNew = false;
				}
			}
		}
		else
		{
			m_Weapon[nIndex] = null;
		}
	}

	public void CarryWeaponExchange(int nIndex1, int nIndex2)
	{
		if (nIndex1 != nIndex2 && nIndex1 >= 0 && nIndex1 < m_nCarryWeapon.Length && nIndex2 >= 0 && nIndex2 < m_nCarryWeapon.Length)
		{
			int num = m_nCarryWeapon[nIndex1];
			m_nCarryWeapon[nIndex1] = m_nCarryWeapon[nIndex2];
			m_nCarryWeapon[nIndex2] = num;
			iWeapon iWeapon2 = m_Weapon[nIndex1];
			m_Weapon[nIndex1] = m_Weapon[nIndex2];
			m_Weapon[nIndex2] = iWeapon2;
		}
	}

	public int GetCarryWeapon(int nIndex)
	{
		if (nIndex < 0 || nIndex >= 3)
		{
			return -1;
		}
		return m_nCarryWeapon[nIndex];
	}

	public int GetCarryIndexByID(int nWeaponID)
	{
		for (int i = 0; i < 3; i++)
		{
			if (m_nCarryWeapon[i] == nWeaponID)
			{
				return i;
			}
		}
		return -1;
	}

	public bool IsCarry()
	{
		for (int i = 0; i < 3; i++)
		{
			if (m_nCarryWeapon[i] != 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsCarryTwo()
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			if (m_nCarryWeapon[i] != 0 && ++num >= 2)
			{
				return true;
			}
		}
		return false;
	}

	public int GetStageState(int nStage)
	{
		if (nStage < 0 || nStage >= m_nStageState.Length)
		{
			return 0;
		}
		return m_nStageState[nStage];
	}

	public bool GetStagePrice(int nStage, ref bool bPriceGod, ref int nStagePrice)
	{
		if (nStage < 0 || nStage >= m_StagePrice.Length || nStage >= m_StagePriceisGod.Length)
		{
			return false;
		}
		bPriceGod = m_StagePriceisGod[nStage];
		nStagePrice = m_StagePrice[nStage];
		return true;
	}

	public int GetBestTime(int nStage)
	{
		if (nStage < 0 || nStage >= m_nStageBestTime.Length)
		{
			return 0;
		}
		return m_nStageBestTime[nStage];
	}

	public int GetBestKill(int nStage)
	{
		if (nStage < 0 || nStage >= m_nStageBestKill.Length)
		{
			return 0;
		}
		return m_nStageBestKill[nStage];
	}

	public void SetBestKill(int nStage, int bestkill)
	{
		if (nStage >= 0 && nStage < m_nStageBestKill.Length)
		{
			m_nStageBestKill[nStage] = bestkill;
		}
	}

	public void SetBestKillSubmitFlag(int nStage, bool flag)
	{
		if (nStage >= 0 && nStage < m_arrBestKillNeedSubmit.Length)
		{
			m_arrBestKillNeedSubmit[nStage] = flag;
		}
	}

	public void SetAchievementFlag(int nIndex, int flag)
	{
		if (nIndex >= 0 && nIndex < m_arrAchieve.Length)
		{
			m_arrAchieve[nIndex] = flag;
		}
	}

	public int GetAchievementFlag(int nIndex)
	{
		if (nIndex < 0 || nIndex >= m_arrAchieve.Length)
		{
			return -1;
		}
		return m_arrAchieve[nIndex];
	}

	public void UnLockStage(int nStage)
	{
		if (nStage >= 0 && nStage < m_nStageState.Length)
		{
			m_nStageState[nStage] = 1;
		}
	}

	public float GetWeaponSGbyStage(float src)
	{
		switch (m_nCurStage)
		{
		case 1:
			src *= 2f;
			break;
		case 2:
			src *= 2f;
			break;
		}
		return src;
	}

	public void CheckRankAtStart()
	{
		for (int i = 0; i < m_arrBestKillNeedSubmit.Length && i < 3 && i < m_nStageBestKill.Length; i++)
		{
			if (m_arrBestKillNeedSubmit[i])
			{
				iZombieSniperGameApp.GetInstance().m_Ranking.CompleteRanking((CRanking.RankEnum)i, m_nStageBestKill[i]);
			}
		}
	}

	public void CheckAchiAtStart()
	{
		for (int i = 0; i < m_arrAchieve.Length && i < 18; i++)
		{
			if (GetAchievementFlag(i) == 1)
			{
				iZombieSniperGameApp.GetInstance().m_Achievement.CompleteAchievement((CAchievement.AchiEnum)i);
			}
		}
	}

	public void SetCGFlag(int nIndex, bool flag)
	{
		if (nIndex >= 0 && nIndex < 3)
		{
			m_arrCGFlag[nIndex] = flag;
		}
	}

	public bool GetCGFlag(int nIndex)
	{
		if (nIndex < 0 || nIndex >= 3)
		{
			return false;
		}
		return m_arrCGFlag[nIndex];
	}

	public void ResetCGFlag()
	{
		for (int i = 0; i < 3; i++)
		{
			m_arrCGFlag[i] = true;
		}
	}

	private void NewVersionUpdate()
	{
		m_nEnterGameCount = 0;
	}

	public bool IsInnocentsHasOwnPath()
	{
		int nCurStage = m_nCurStage;
		if (nCurStage == 2)
		{
			return true;
		}
		return false;
	}

	public Vector2 GetShootCenter()
	{
		return new Vector2(Screen.width / 2, Screen.height / 2);
	}
}
