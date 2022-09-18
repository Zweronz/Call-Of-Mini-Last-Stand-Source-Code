using System;
using System.Collections;
using UnityEngine;

public class iZombieSniperGameUI : UIHelper
{
	private class NumRollInfo
	{
		public bool m_bIsOK;

		public int m_nDestNum;

		public int m_nCurrNum;

		public int m_nSpeed;

		public int m_nDir;

		public string m_sStr;

		public UIText m_Control;

		public NumRollInfo(int curNum, int dstNum, float time, string str, UIText control)
		{
			m_bIsOK = false;
			m_nCurrNum = curNum;
			m_nDestNum = dstNum;
			m_nDir = ((m_nCurrNum < m_nDestNum) ? 1 : (-1));
			m_nSpeed = (int)((float)(dstNum - curNum) / time) * m_nDir;
			if (m_nSpeed < 1)
			{
				m_nSpeed = 1;
			}
			m_sStr = str;
			m_Control = control;
		}

		public void Update(float deltaTime)
		{
			if (!m_bIsOK)
			{
				m_nCurrNum += m_nSpeed * m_nDir;
				if ((m_nDir > 0 && m_nCurrNum > m_nDestNum) || (m_nDir < 0 && m_nCurrNum < m_nDestNum))
				{
					m_nCurrNum = m_nDestNum;
					m_bIsOK = true;
				}
				if (m_Control != null)
				{
					m_Control.SetText(m_sStr.Replace("%d", m_nCurrNum.ToString()));
				}
			}
		}
	}

	public enum AlarmScreenSide
	{
		None = 0,
		All = 1,
		Left = 2,
		Right = 3
	}

	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameState m_GameState;

	public iZombieSniperGunCenter m_GunCenter;

	public iZombieSniperIconCenter m_IconCenter;

	public iZombieSniperGameHelp m_GameHelp;

	public iZombieSniperMiniMap m_MiniMap;

	public CNail m_SignZombie;

	public CNail m_SignPeople;

	public CNail m_WarnArrow;

	public CNail m_WarnIcon;

	public CNail m_BulletJump;

	public CTextImage m_tImgBulletNum;

	public CTextImage m_tImgKillZombie;

	public CTextImage m_tImgSaveInnocent;

	public CTextImage m_tImgKillInnocent;

	public CTextImage m_tImgResultBest;

	public CRollText m_RollText;

	public CImageAnim m_ImageAnim;

	public CImageAnim m_FingerClick;

	public Vector2 m_v2BulletPos = Vector2.zero;

	public DialogEnum m_nDialogType;

	public CImageMsg m_ImageMsg1;

	public CImageMsg m_ImageMsg2;

	public CImageMsg m_ImageMsg3;

	public CTextImageFade m_ImageMsgStrike;

	private ArrayList m_NumRollList;

	private bool bGameResult;

	private float fGameResultCount;

	private int nCurrLine;

	public new void Start()
	{
		m_font_path = "ZombieSniper/Fonts/Materials/";
		m_ui_material_path = "ZombieSniper/UI/Materials/";
		m_ui_cfgxml = "ZombieSniper/UI/GameUI";
		base.Start();
	}

	public new void Update()
	{
		base.Update();
		if (m_BulletJump != null)
		{
			m_BulletJump.Update(Time.deltaTime);
		}
		if (m_RollText != null)
		{
			m_RollText.Update(Time.deltaTime);
		}
		if (m_ImageAnim != null)
		{
			m_ImageAnim.Update(Time.deltaTime);
		}
		if (m_FingerClick != null)
		{
			m_FingerClick.Update(Time.deltaTime);
		}
		if (m_SignZombie != null)
		{
			m_SignZombie.Update(Time.deltaTime);
		}
		if (m_SignPeople != null)
		{
			m_SignPeople.Update(Time.deltaTime);
		}
		if (m_WarnArrow != null)
		{
			m_WarnArrow.Update(Time.deltaTime);
		}
		if (m_WarnIcon != null)
		{
			m_WarnIcon.Update(Time.deltaTime);
		}
		if (m_ImageMsgStrike != null)
		{
			m_ImageMsgStrike.Update(Time.deltaTime);
		}
		UpdateGameResultLogic(Time.deltaTime);
	}

	public new void OnGUI()
	{
		base.OnGUI();
	}

	public void Destroy()
	{
		m_MiniMap = null;
		if (m_SignZombie != null)
		{
			m_SignZombie.Destroy();
			m_SignZombie = null;
		}
		if (m_SignPeople != null)
		{
			m_SignPeople.Destroy();
			m_SignPeople = null;
		}
		if (m_WarnArrow != null)
		{
			m_WarnArrow.Destroy();
			m_WarnArrow = null;
		}
		if (m_WarnIcon != null)
		{
			m_WarnIcon.Destroy();
			m_WarnIcon = null;
		}
		if (m_BulletJump != null)
		{
			m_BulletJump.Destroy();
			m_BulletJump = null;
		}
		if (m_tImgBulletNum != null)
		{
			m_tImgBulletNum.Destroy();
			m_tImgBulletNum = null;
		}
		if (m_tImgKillZombie != null)
		{
			m_tImgKillZombie.Destroy();
			m_tImgKillZombie = null;
		}
		if (m_tImgSaveInnocent != null)
		{
			m_tImgSaveInnocent.Destroy();
			m_tImgSaveInnocent = null;
		}
		if (m_tImgKillInnocent != null)
		{
			m_tImgKillInnocent.Destroy();
			m_tImgKillInnocent = null;
		}
		if (m_RollText != null)
		{
			m_RollText.Destroy();
			m_RollText = null;
		}
		if (m_ImageAnim != null)
		{
			m_ImageAnim.Destroy();
			m_ImageAnim = null;
		}
		if (m_FingerClick != null)
		{
			m_FingerClick.Destroy();
			m_FingerClick = null;
		}
		if (m_ImageMsg1 != null)
		{
			m_ImageMsg1.Destroy();
			m_ImageMsg1 = null;
		}
		if (m_ImageMsg2 != null)
		{
			m_ImageMsg2.Destroy();
			m_ImageMsg2 = null;
		}
		if (m_ImageMsg3 != null)
		{
			m_ImageMsg3.Destroy();
			m_ImageMsg3 = null;
		}
		if (m_ImageMsgStrike != null)
		{
			m_ImageMsgStrike.Destroy();
			m_ImageMsgStrike = null;
		}
		if (m_NumRollList != null)
		{
			m_NumRollList.Clear();
			m_NumRollList = null;
		}
		Resources.UnloadUnusedAssets();
	}

	public void Initialize()
	{
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = m_GameScene.m_GameState;
		m_IconCenter = iZombieSniperGameApp.GetInstance().m_IconCenter;
		m_GunCenter = iZombieSniperGameApp.GetInstance().m_GunCenter;
		m_GameHelp = m_GameScene.GetGameHelp();
		m_v2BulletPos = ((UIImage)m_control_table["imgBullet1"]).GetPosition();
		Material material = LoadUIMaterial(Utils.AutoMaterialName("game_ui_part1"));
		Material material2 = LoadUIMaterial(Utils.AutoMaterialName("game_ui_part3"));
		Material material3 = LoadUIMaterial(Utils.AutoMaterialName("game_ui_part4"));
		if (m_SignZombie == null)
		{
			m_SignZombie = new CNail();
			m_SignZombie.Initialize(m_UIManagerRef, material, new Rect(494 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, 13 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor));
			m_SignZombie.SetLayer(5);
			m_SignZombie.Show(false);
		}
		if (m_SignPeople == null)
		{
			m_SignPeople = new CNail();
			m_SignPeople.Initialize(m_UIManagerRef, material, new Rect(2 * m_GameState.m_nHDFactor, 384 * m_GameState.m_nHDFactor, 13 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor));
			m_SignPeople.SetLayer(5);
			m_SignPeople.Show(false);
		}
		if (m_WarnArrow == null)
		{
			m_WarnArrow = new CNail();
			m_WarnArrow.Initialize(m_UIManagerRef, material, new Rect(130 * m_GameState.m_nHDFactor, 386 * m_GameState.m_nHDFactor, 9 * m_GameState.m_nHDFactor, 10 * m_GameState.m_nHDFactor));
			m_WarnArrow.SetLayer(5);
			m_WarnArrow.Show();
		}
		if (m_WarnIcon == null)
		{
			m_WarnIcon = new CNail();
			m_WarnIcon.Initialize(m_UIManagerRef, material, new Rect(280 * m_GameState.m_nHDFactor, 378 * m_GameState.m_nHDFactor, 20 * m_GameState.m_nHDFactor, 20 * m_GameState.m_nHDFactor));
			m_WarnIcon.SetLayer(5);
			m_WarnIcon.Show();
		}
		if (m_BulletJump == null)
		{
			m_BulletJump = new CNail();
			m_BulletJump.Initialize(m_UIManagerRef, material, new Rect(364 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, 66 * m_GameState.m_nHDFactor, 20 * m_GameState.m_nHDFactor));
			m_BulletJump.Show();
		}
		if (m_RollText == null)
		{
			m_RollText = new CRollText();
			m_RollText.Initialize(this, m_UIManagerRef, m_GameState.m_v3ShootCenter + Utils.CalcScale(new Vector2(0f, 80f)), 200 * m_GameState.m_nHDFactor, 25 * m_GameState.m_nHDFactor, 5);
		}
		TextImageInfo[] imageInfoList = new TextImageInfo[12]
		{
			new TextImageInfo('1', material, new Rect(70 * m_GameState.m_nHDFactor, 384 * m_GameState.m_nHDFactor, 9 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor)),
			new TextImageInfo('2', material, new Rect(60 * m_GameState.m_nHDFactor, 384 * m_GameState.m_nHDFactor, 9 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor)),
			new TextImageInfo('3', material, new Rect(90 * m_GameState.m_nHDFactor, 384 * m_GameState.m_nHDFactor, 9 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor)),
			new TextImageInfo('4', material, new Rect(100 * m_GameState.m_nHDFactor, 384 * m_GameState.m_nHDFactor, 9 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor)),
			new TextImageInfo('5', material, new Rect(110 * m_GameState.m_nHDFactor, 384 * m_GameState.m_nHDFactor, 9 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor)),
			new TextImageInfo('6', material, new Rect(120 * m_GameState.m_nHDFactor, 386 * m_GameState.m_nHDFactor, 9 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor)),
			new TextImageInfo('7', material, new Rect(50 * m_GameState.m_nHDFactor, 384 * m_GameState.m_nHDFactor, 9 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor)),
			new TextImageInfo('8', material, new Rect(40 * m_GameState.m_nHDFactor, 384 * m_GameState.m_nHDFactor, 9 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor)),
			new TextImageInfo('9', material, new Rect(30 * m_GameState.m_nHDFactor, 384 * m_GameState.m_nHDFactor, 9 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor)),
			new TextImageInfo('0', material, new Rect(80 * m_GameState.m_nHDFactor, 384 * m_GameState.m_nHDFactor, 9 * m_GameState.m_nHDFactor, 12 * m_GameState.m_nHDFactor)),
			new TextImageInfo('x', material, new Rect(494 * m_GameState.m_nHDFactor, 340 * m_GameState.m_nHDFactor, 5 * m_GameState.m_nHDFactor, 5 * m_GameState.m_nHDFactor)),
			new TextImageInfo('/', material, new Rect(5 * m_GameState.m_nHDFactor, 404 * m_GameState.m_nHDFactor, 7 * m_GameState.m_nHDFactor, 10 * m_GameState.m_nHDFactor))
		};
		if (m_tImgBulletNum == null)
		{
			m_tImgBulletNum = new CTextImage();
			m_tImgBulletNum.Initialize(m_UIManagerRef, imageInfoList);
			m_tImgBulletNum.SetTextAlign(CTextImage.enAlignStyle.right);
			Vector2 value = new Vector2(380f, 120f);
			Vector2 value2 = new Vector2(100f, 12f);
			value = Utils.CalcScale(value);
			value2 = Utils.CalcScale(value2);
			m_tImgBulletNum.SetRect(new Rect(value.x, value.y, value2.x, value2.y));
		}
		if (m_tImgKillZombie == null)
		{
			m_tImgKillZombie = new CTextImage();
			m_tImgKillZombie.Initialize(m_UIManagerRef, imageInfoList);
			Vector2 value3 = new Vector2(60f, 290f);
			Vector2 value4 = new Vector2(100f, 12f);
			value3 = Utils.CalcScale(value3);
			value4 = Utils.CalcScale(value4);
			m_tImgKillZombie.SetRect(new Rect(value3.x, value3.y, value4.x, value4.y));
		}
		if (m_tImgSaveInnocent == null)
		{
			m_tImgSaveInnocent = new CTextImage();
			m_tImgSaveInnocent.Initialize(m_UIManagerRef, imageInfoList);
			Vector2 value5 = new Vector2(120f, 290f);
			Vector2 value6 = new Vector2(100f, 12f);
			value5 = Utils.CalcScale(value5);
			value6 = Utils.CalcScale(value6);
			m_tImgSaveInnocent.SetRect(new Rect(value5.x, value5.y, value6.x, value6.y));
		}
		if (m_tImgKillInnocent == null)
		{
			m_tImgKillInnocent = new CTextImage();
			m_tImgKillInnocent.Initialize(m_UIManagerRef, imageInfoList);
			Vector2 value7 = new Vector2(178f, 290f);
			Vector2 value8 = new Vector2(100f, 12f);
			value7 = Utils.CalcScale(value7);
			value8 = Utils.CalcScale(value8);
			m_tImgKillInnocent.SetRect(new Rect(value7.x, value7.y, value8.x, value8.y));
			m_tImgKillInnocent.SetColor(new Color(255f, 0f, 0f));
		}
		if (m_tImgResultBest == null)
		{
			m_tImgResultBest = new CTextImage();
			m_tImgResultBest.Initialize(m_UIManagerRef, imageInfoList, 6);
			Vector2 value9 = new Vector2(373f, 242f);
			Vector2 value10 = new Vector2(100f, 12f);
			value9 = Utils.CalcScale(value9);
			value10 = Utils.CalcScale(value10);
			m_tImgResultBest.SetRect(new Rect(value9.x, value9.y, value10.x, value10.y));
		}
		if (m_ImageAnim == null)
		{
			m_ImageAnim = new CImageAnim();
			m_ImageAnim.Initialize(GetUIImage("imgHelpClick"), 10);
			m_ImageAnim.Add(material2, new Rect(202 * m_GameState.m_nHDFactor, 438 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
			m_ImageAnim.Add(material2, new Rect(152 * m_GameState.m_nHDFactor, 422 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
			m_ImageAnim.Add(material2, new Rect(102 * m_GameState.m_nHDFactor, 422 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
			m_ImageAnim.Add(material2, new Rect(52 * m_GameState.m_nHDFactor, 414 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
			m_ImageAnim.Add(material2, new Rect(2 * m_GameState.m_nHDFactor, 414 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
			m_ImageAnim.Add(material2, new Rect(438 * m_GameState.m_nHDFactor, 388 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
			m_ImageAnim.Add(material2, new Rect(388 * m_GameState.m_nHDFactor, 388 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
			m_ImageAnim.Add(material2, new Rect(338 * m_GameState.m_nHDFactor, 388 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
			m_ImageAnim.Add(material2, new Rect(288 * m_GameState.m_nHDFactor, 388 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
			m_ImageAnim.Add(material2, new Rect(238 * m_GameState.m_nHDFactor, 388 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor));
		}
		if (m_FingerClick == null)
		{
			m_FingerClick = new CImageAnim();
			m_FingerClick.Initialize(GetUIImage("imgHelpIcon"), 2);
			m_FingerClick.Add(material2, new Rect(302 * m_GameState.m_nHDFactor, 438 * m_GameState.m_nHDFactor, 48 * m_GameState.m_nHDFactor, 43 * m_GameState.m_nHDFactor));
			m_FingerClick.Add(material2, new Rect(252 * m_GameState.m_nHDFactor, 438 * m_GameState.m_nHDFactor, 49 * m_GameState.m_nHDFactor, 43 * m_GameState.m_nHDFactor));
		}
		if (m_ImageMsg1 == null)
		{
			m_ImageMsg1 = new CImageMsg();
			m_ImageMsg1.Initialize();
			m_ImageMsg1.Add(1, 4, "imgWarnZombieClose");
			m_ImageMsg1.Add(2, 2, "imgWarnKillMany");
			m_ImageMsg1.Add(3, 3, "imgWarnAC130");
			m_ImageMsg1.Add(4, 1, "imgWarnMissKill");
			m_ImageMsg1.Add(5, 1, "imgWarnHeadShoot");
		}
		if (m_ImageMsg2 == null)
		{
			m_ImageMsg2 = new CImageMsg();
			m_ImageMsg2.Initialize();
			m_ImageMsg2.Add(1, 1, "imgWarnOutOfRange");
			m_ImageMsg2.Add(2, 2, "imgWarnReloading");
			m_ImageMsg2.Add(3, 3, "imgDontKillInoo");
		}
		if (m_ImageMsg3 == null)
		{
			m_ImageMsg3 = new CImageMsg();
			m_ImageMsg3.Initialize();
			m_ImageMsg3.Add(1, 1, "imgDontKillInooStr");
		}
		if (m_ImageMsgStrike == null)
		{
			imageInfoList = new TextImageInfo[10]
			{
				new TextImageInfo('0', material3, new Rect(484 * m_GameState.m_nHDFactor, 126 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor, 17 * m_GameState.m_nHDFactor)),
				new TextImageInfo('1', material3, new Rect(484 * m_GameState.m_nHDFactor, 144 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor, 17 * m_GameState.m_nHDFactor)),
				new TextImageInfo('2', material3, new Rect(484 * m_GameState.m_nHDFactor, 162 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor, 17 * m_GameState.m_nHDFactor)),
				new TextImageInfo('3', material3, new Rect(484 * m_GameState.m_nHDFactor, 0 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor, 17 * m_GameState.m_nHDFactor)),
				new TextImageInfo('4', material3, new Rect(484 * m_GameState.m_nHDFactor, 18 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor, 17 * m_GameState.m_nHDFactor)),
				new TextImageInfo('5', material3, new Rect(484 * m_GameState.m_nHDFactor, 36 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor, 17 * m_GameState.m_nHDFactor)),
				new TextImageInfo('6', material3, new Rect(484 * m_GameState.m_nHDFactor, 54 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor, 17 * m_GameState.m_nHDFactor)),
				new TextImageInfo('7', material3, new Rect(484 * m_GameState.m_nHDFactor, 72 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor, 17 * m_GameState.m_nHDFactor)),
				new TextImageInfo('8', material3, new Rect(484 * m_GameState.m_nHDFactor, 90 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor, 17 * m_GameState.m_nHDFactor)),
				new TextImageInfo('9', material3, new Rect(484 * m_GameState.m_nHDFactor, 108 * m_GameState.m_nHDFactor, 14 * m_GameState.m_nHDFactor, 17 * m_GameState.m_nHDFactor))
			};
			m_ImageMsgStrike = new CTextImageFade();
			m_ImageMsgStrike.Initialize(m_UIManagerRef, imageInfoList);
			m_ImageMsgStrike.SetTextAlign(CTextImage.enAlignStyle.left);
			Vector2 value11 = new Vector2(262f, 192f);
			Vector2 value12 = new Vector2(34f, 17f);
			value11 = Utils.CalcScale(value11);
			value12 = Utils.CalcScale(value12);
			m_ImageMsgStrike.SetRect(new Rect(value11.x, value11.y, value12.x, value12.y));
		}
		((UIImage)m_control_table["preload1"]).Visible = true;
		((UIImage)m_control_table["preload2"]).Visible = true;
		((UIImage)m_control_table["preload3"]).Visible = true;
		((UIImage)m_control_table["preload4"]).Visible = true;
		((UIImage)m_control_table["imgAimCrossUp"]).SetColor(Color.green);
		((UIImage)m_control_table["imgAimCrossDown"]).SetColor(Color.green);
		((UIImage)m_control_table["imgAimCrossLeft"]).SetColor(Color.green);
		((UIImage)m_control_table["imgAimCrossRight"]).SetColor(Color.green);
		ResetData();
	}

	public void ResetData()
	{
		UpdateWeaponButton();
		if (m_GameScene.m_CurrWeapon != null)
		{
			UpdateWeaponPic(m_GameScene.m_CurrWeapon.m_nWeaponID);
			UpdateBulletUI(m_GameScene.m_CurrWeapon.m_nCurrBulletNum);
		}
		RemoveGameHelpUI();
		StopAlarmScreen(AlarmScreenSide.All);
		SetZombieKill(m_GameState.m_nKillNorEnemyNum + m_GameState.m_nKillGiaEnemyNum, true);
		SetInnocenceSafe(m_GameState.m_nSaveInnoNum, true);
		SetInnocenceDead(m_GameState.m_nKillInnoNum, true);
		((UIClickButton)m_control_table["btnFire"]).Visible = true;
		((UIClickButton)m_control_table["btnFire"]).Enable = true;
		((UIImage)m_control_table["imgSniperAimRB"]).Visible = true;
		((UIClickButton)m_control_table["btnAC130"]).Enable = false;
		((UIClickButton)m_control_table["btnAC130"]).Visible = false;
		((UIClickButton)m_control_table["btnMachine"]).Enable = false;
		((UIClickButton)m_control_table["btnMachine"]).Visible = false;
		((UIImage)m_control_table["imgAC130Back"]).Visible = false;
		if (!m_GameScene.IsSpecialButtonDone())
		{
			switch (m_GameState.m_nCurStage)
			{
			case 0:
			case 1:
				if (m_GameState.m_nAC130 > 0)
				{
					((UIClickButton)m_control_table["btnAC130"]).Enable = true;
					((UIClickButton)m_control_table["btnAC130"]).Visible = true;
					((UIClickButton)m_control_table["btnAC130"]).SetRect(Utils.CalcScaleRect(new Rect(370f, 140f, 100f, 100f)));
				}
				break;
			case 2:
				if (m_GameState.m_nMachineGun > 0)
				{
					((UIClickButton)m_control_table["btnMachine"]).Enable = true;
					((UIClickButton)m_control_table["btnMachine"]).Visible = true;
					((UIClickButton)m_control_table["btnMachine"]).SetRect(Utils.CalcScaleRect(new Rect(358f, 140f, 100f, 100f)));
				}
				break;
			}
			((UIImage)m_control_table["imgAC130Back"]).Visible = true;
			((UIImage)m_control_table["imgAC130Back"]).SetRect(Utils.CalcScaleRect(new Rect(440f, 170f, 13f, 31f)));
		}
		((UIClickButton)m_control_table["btnPauseOn"]).Enable = true;
		((UIClickButton)m_control_table["btnPauseOn"]).Visible = true;
		((UIClickButton)m_control_table["btnGunBackGround"]).Enable = true;
		((UIClickButton)m_control_table["btnGunBackGround"]).Visible = true;
		SetCrossPos(m_GameScene.m_fCurCrossDis);
		ShowCross(true);
		ShowWarn(true);
		switch (m_GameState.m_nCurStage)
		{
		case 0:
			((UIClickButton)m_control_table["btnSkip"]).Visible = m_GameScene.m_bTutorial;
			((UIClickButton)m_control_table["btnSkip"]).Enable = m_GameScene.m_bTutorial;
			((UIImage)m_control_table["imgScore"]).Visible = !m_GameScene.m_bTutorial;
			m_tImgKillZombie.Show(!m_GameScene.m_bTutorial);
			m_tImgKillInnocent.Show(!m_GameScene.m_bTutorial);
			m_tImgSaveInnocent.Show(!m_GameScene.m_bTutorial);
			break;
		case 1:
			((UIClickButton)m_control_table["btnSkip2"]).Visible = m_GameScene.IsPlayCG();
			((UIClickButton)m_control_table["btnSkip2"]).Enable = m_GameScene.IsPlayCG();
			((UIImage)m_control_table["imgScore"]).Visible = !m_GameScene.IsPlayCG();
			m_tImgKillZombie.Show(!m_GameScene.IsPlayCG());
			m_tImgKillInnocent.Show(!m_GameScene.IsPlayCG());
			m_tImgSaveInnocent.Show(!m_GameScene.IsPlayCG());
			break;
		case 2:
			((UIClickButton)m_control_table["btnSkip2"]).Visible = m_GameScene.IsPlayCG();
			((UIClickButton)m_control_table["btnSkip2"]).Enable = m_GameScene.IsPlayCG();
			((UIImage)m_control_table["imgScore"]).Visible = !m_GameScene.IsPlayCG();
			m_tImgKillZombie.Show(!m_GameScene.IsPlayCG());
			m_tImgKillInnocent.Show(!m_GameScene.IsPlayCG());
			m_tImgSaveInnocent.Show(!m_GameScene.IsPlayCG());
			break;
		}
		((UIText)m_control_table["txtGameTime"]).Visible = false;
		ShowMainPic(false);
		HideMovieUI();
		ShowAimUI(WEAPON_TYPE.NONE);
		FadeInReady();
		ShowDefenceUI(false);
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (command == 0)
		{
			if (control.Id == GetControlId("btnSkip"))
			{
				m_GameScene.PlayAudio("UIClickGeneral");
				m_GameState.m_bTutorial = false;
				m_GameState.SaveData();
				m_GameScene.m_bTutorial = false;
				m_GameScene.Destroy();
				m_GameScene.Initialize();
				m_GameScene.ResetData();
				m_GameScene.StartGame();
			}
			else if (control.Id == GetControlId("btnSkip2"))
			{
				m_GameScene.PlayAudio("UIClickGeneral");
				m_GameState.SetCGFlag(m_GameState.m_nCurStage, false);
				m_GameState.SaveData();
				m_GameScene.SetPlayCG(false);
				m_GameScene.Destroy();
				m_GameScene.Initialize();
				m_GameScene.ResetData();
				m_GameScene.StartGame();
			}
		}
		if (m_nDialogType != 0)
		{
			if (command != 0)
			{
				return;
			}
			if (control.Id == GetControlId("btnPOPConfirm"))
			{
				switch (m_nDialogType)
				{
				case DialogEnum.Menu:
					m_GameScene.PlayAudio("UIClickGeneral");
					m_GameScene.SetPause(false);
					m_GameScene.Destroy();
					iZombieSniperGameApp.GetInstance().StopAudio("MusicMap01");
					iZombieSniperGameApp.GetInstance().StopAudio("MusicMap02");
					iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kMap);
					break;
				case DialogEnum.Retry:
					m_GameScene.PlayAudio("UIClickGeneral");
					HideDialog();
					ShowGamePause(false);
					m_GameScene.Destroy();
					Destroy();
					m_GameScene.Initialize();
					m_GameScene.ResetData();
					m_GameScene.StartGame();
					break;
				}
			}
			else if (control.Id == GetControlId("btnPOPCancel"))
			{
				m_GameScene.PlayAudio("UIClickGeneral");
				HideDialog();
			}
			return;
		}
		if (command == 1)
		{
			if (control.Id == GetControlId("btnFire"))
			{
				m_GameScene.SetAutoShoot(true);
			}
			else
			{
				m_GameScene.SetAutoShoot(false);
			}
		}
		else
		{
			m_GameScene.SetAutoShoot(false);
		}
		if (command != 0)
		{
			return;
		}
		if (control.Id == GetControlId("btnGunBackGround"))
		{
			if (m_GameScene.m_State == iZombieSniperGameSceneBase.State.kGameTurtorial)
			{
				if (m_GameHelp != null && m_GameHelp.IsCanSwitchWeapon())
				{
					m_GameScene.SwitchWeapon();
					m_GameHelp.FinishHelpState(GameHelpState.Step5);
				}
			}
			else
			{
				m_GameScene.SwitchWeapon();
			}
			m_GameScene.PlayAudio("UIClickGeneral");
		}
		else if (control.Id == GetControlId("btnAC130"))
		{
			m_GameScene.PlayAudio("UIClickGeneral");
			m_GameScene.AC130();
			HideAC130();
		}
		else if (control.Id == GetControlId("btnMachine"))
		{
			m_GameScene.PlayAudio("UIClickGeneral");
			m_GameScene.AC130();
			HideAC130();
		}
		else if (control.Id == GetControlId("btnPauseBack"))
		{
			ShowDialog(DialogEnum.Menu, "ARE YOU SURE YOU\n WANT TO QUIT THE GAME?");
			m_GameScene.PlayAudio("UIClickGeneral");
		}
		else if (control.Id == GetControlId("btnBack"))
		{
			m_GameScene.PlayAudio("UIClickGeneral");
			m_GameScene.SetPause(false);
			m_GameScene.Destroy();
			iZombieSniperGameApp.GetInstance().StopAudio("MusicMap01");
			iZombieSniperGameApp.GetInstance().StopAudio("MusicMap02");
			iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kMap);
		}
		else if (control.Id == GetControlId("btnRetry"))
		{
			ShowGameResult(false);
			m_GameScene.Initialize();
			m_GameScene.ResetData();
			m_GameScene.StartGame();
			m_GameScene.PlayAudio("UIClickGeneral");
		}
		else if (control.Id == GetControlId("btnPauseRetry"))
		{
			ShowDialog(DialogEnum.Retry, "You'll lose your current score.\n Restart anyway?");
			m_GameScene.PlayAudio("UIClickGeneral");
		}
		else if (control.Id == GetControlId("btnGear"))
		{
			m_GameScene.SetPause(false);
			iZombieSniperGameApp.GetInstance().EnterScene(SceneEnum.kShop, true, 3);
			m_GameScene.PlayAudio("UIClickGeneral");
		}
		else if (control.Id == GetControlId("btnPauseResume"))
		{
			m_GameScene.SetGamePause(false);
			m_GameScene.PlayAudio("UIClickGeneral");
		}
		else if (control.Id == GetControlId("btnPauseOn"))
		{
			m_GameScene.SetGamePause(true);
			m_GameScene.PlayAudio("UIClickGeneral");
		}
		else if (control.Id == GetControlId("btnPauseSoundON"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			m_GameState.m_bSoundOn = true;
			m_GameState.SaveData();
			SetSound(m_GameState.m_bSoundOn);
			TAudioManager.instance.isSoundOn = true;
		}
		else if (control.Id == GetControlId("btnPauseSoundOFF"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			m_GameState.m_bSoundOn = false;
			m_GameState.SaveData();
			SetSound(m_GameState.m_bSoundOn);
			TAudioManager.instance.isSoundOn = false;
		}
		else if (control.Id == GetControlId("btnPauseMusicON"))
		{
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			m_GameState.m_bMusicOn = true;
			m_GameState.SaveData();
			SetMusic(m_GameState.m_bMusicOn);
			TAudioManager.instance.isMusicOn = true;
		}
		else if (control.Id == GetControlId("btnPauseMusicOFF"))
		{
			iZombieSniperGameApp.GetInstance().ClearAudio("UIClickGeneral");
			iZombieSniperGameApp.GetInstance().PlayAudio("UIClickGeneral");
			m_GameState.m_bMusicOn = false;
			m_GameState.SaveData();
			SetMusic(m_GameState.m_bMusicOn);
			TAudioManager.instance.isMusicOn = false;
		}
	}

	public void FadeIn()
	{
		((UIImage)m_control_table["fadeimage"]).Visible = true;
		((UIImage)m_control_table["fadeimage"]).Enable = true;
		((UIImage)m_control_table["fadeimage"]).SetAlpha(1f);
		StartAnimation("fadein");
	}

	public void FadeInReady()
	{
		((UIImage)m_control_table["fadeimage"]).Visible = true;
		((UIImage)m_control_table["fadeimage"]).Enable = true;
		((UIImage)m_control_table["fadeimage"]).SetAlpha(1f);
		StartAnimation("fadeready");
	}

	public bool IsFadeIn()
	{
		return ((UIImage)m_control_table["fadeimage"]).Visible;
	}

	public void UpdateBulletUI(int nBulletNum)
	{
		m_tImgBulletNum.SetText("x " + nBulletNum, Color.white);
		((UIImage)m_control_table["imgBullet1"]).Visible = nBulletNum >= 1;
	}

	public void SetZombieKill(int nNum, bool bForce = false)
	{
		if (bForce || m_GameScene.m_State == iZombieSniperGameSceneBase.State.kGaming)
		{
			m_tImgKillZombie.SetText(nNum.ToString(), Color.white);
		}
	}

	public void SetInnocenceSafe(int nNum, bool bForce = false)
	{
		if (bForce || m_GameScene.m_State == iZombieSniperGameSceneBase.State.kGaming)
		{
			m_tImgSaveInnocent.SetText(nNum.ToString(), Color.white);
		}
	}

	public void SetInnocenceDead(int nNum, bool bForce = false)
	{
		if (bForce || m_GameScene.m_State == iZombieSniperGameSceneBase.State.kGaming)
		{
			m_tImgKillInnocent.SetText(nNum.ToString() + "/" + m_GameScene.m_nMaxKillInnoNum, Color.white);
		}
	}

	public void PlayBulletAnim(float fInterval, float fBegin = 0f)
	{
		if (m_animations.ContainsKey("bulletInterval"))
		{
			Vector2 v2BulletPos = m_v2BulletPos;
			float num = 20f * ((fInterval - fBegin) / fInterval) * (float)m_GameState.m_nHDFactor;
			((UIAnimations)m_animations["bulletInterval"]).translate_time = fInterval;
			((UIAnimations)m_animations["bulletInterval"]).translate_offset = new Vector2(0f - num, 0f);
			v2BulletPos.x += num;
			((UIImage)m_control_table["imgBullet1"]).SetPosition(v2BulletPos);
			StartAnimation("bulletInterval");
		}
	}

	public void StopBulletAnim()
	{
		if (m_animations.ContainsKey("bulletInterval"))
		{
			StopAnimation("bulletInterval");
			((UIImage)m_control_table["imgBullet1"]).SetPosition(m_v2BulletPos);
		}
	}

	public void PlayBulletJump()
	{
		Vector2 v2Pos = new Vector2(290f, 70f) * m_GameState.m_nHDFactor;
		int nID = m_BulletJump.AddPoint(v2Pos, Vector2.zero, Color.white, Vector2.zero, 2f);
		m_BulletJump.AddRotateAnim(nID, (float)Math.PI * 4f, 2f);
		m_BulletJump.AddTransAnim(nID, UnityEngine.Random.Range(-500f, -900f), UnityEngine.Random.Range(600f, 800f), 0f, -3000f, 2f);
	}

	public void ShowAimUI(WEAPON_TYPE nType)
	{
		SwitchAimUI(nType);
		((UIImage)m_control_table["imgSniperAimLB"]).Visible = true;
		m_SignZombie.Show();
		m_SignPeople.Show();
	}

	public void HideAimUI()
	{
		((UIImage)m_control_table["imgSniperAim"]).Visible = false;
		((UIImage)m_control_table["imgAutoShootAim"]).Visible = false;
		((UIImage)m_control_table["imgRocketAim"]).Visible = false;
		((UIImage)m_control_table["imgSniperAimLB"]).Visible = false;
		m_SignZombie.Show(false);
		m_SignPeople.Show(false);
	}

	public void ShowWarn(bool bShow)
	{
		m_WarnArrow.Show(bShow);
		m_WarnIcon.Show(bShow);
	}

	public void ShowCross(bool bShow)
	{
		((UIImage)m_control_table["imgAimCrossUp"]).Visible = bShow;
		((UIImage)m_control_table["imgAimCrossDown"]).Visible = bShow;
		((UIImage)m_control_table["imgAimCrossLeft"]).Visible = bShow;
		((UIImage)m_control_table["imgAimCrossRight"]).Visible = bShow;
	}

	public void SwitchAimUI(WEAPON_TYPE nType)
	{
		if (m_GameScene.IsAim())
		{
			((UIImage)m_control_table["imgSniperAim"]).Visible = false;
			((UIImage)m_control_table["imgAutoShootAim"]).Visible = false;
			((UIImage)m_control_table["imgRocketAim"]).Visible = false;
			switch (nType)
			{
			case WEAPON_TYPE.NONE:
				((UIImage)m_control_table["imgSniperAim"]).Visible = true;
				((UIImage)m_control_table["imgAutoShootAim"]).Visible = true;
				((UIImage)m_control_table["imgRocketAim"]).Visible = true;
				break;
			case WEAPON_TYPE.SNIPER_RIFLE:
				((UIImage)m_control_table["imgSniperAim"]).Visible = true;
				break;
			case WEAPON_TYPE.AUTOSHOOT:
				((UIImage)m_control_table["imgAutoShootAim"]).Visible = true;
				break;
			case WEAPON_TYPE.BAZOOKA:
			case WEAPON_TYPE.THROWMINE:
				((UIImage)m_control_table["imgRocketAim"]).Visible = true;
				break;
			}
		}
	}

	public void UpdateWeaponButton()
	{
		if (!m_GameState.IsInWeaponTool())
		{
			bool flag = false;
			flag = m_GameState.m_nCurWeaponIndex == 0;
			((UIImage)m_control_table["imgWeapon1_on"]).Visible = flag;
			((UIImage)m_control_table["imgWeapon1_off"]).Visible = !flag;
			flag = m_GameState.m_nCurWeaponIndex == 1;
			((UIImage)m_control_table["imgWeapon2_on"]).Visible = flag;
			((UIImage)m_control_table["imgWeapon2_off"]).Visible = !flag;
			flag = m_GameState.m_nCurWeaponIndex == 2;
			((UIImage)m_control_table["imgWeapon3_on"]).Visible = flag;
			((UIImage)m_control_table["imgWeapon3_off"]).Visible = !flag;
		}
	}

	public void UpdateWeaponPic(int nWeaponID)
	{
		((UIImage)m_control_table["weaponIcon"]).Visible = false;
		iWeaponInfoBase weaponInfoBase = iZombieSniperGameApp.GetInstance().m_GunCenter.GetWeaponInfoBase(nWeaponID);
		if (weaponInfoBase == null)
		{
			return;
		}
		IconInfo icon = m_IconCenter.GetIcon(weaponInfoBase.m_nIconID);
		if (icon != null)
		{
			Material material = LoadUIMaterial(Utils.AutoMaterialName(icon.m_sMaterial));
			if (material != null)
			{
				((UIImage)m_control_table["weaponIcon"]).SetTexture(material, new Rect(icon.m_Rect.xMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.yMin * (float)m_GameState.m_nHDFactor, icon.m_Rect.width * (float)m_GameState.m_nHDFactor, icon.m_Rect.height * (float)m_GameState.m_nHDFactor), new Vector2(icon.m_Rect.width * 0.8f * (float)m_GameState.m_nHDFactor, icon.m_Rect.height * 0.8f * (float)m_GameState.m_nHDFactor));
				((UIImage)m_control_table["weaponIcon"]).Visible = true;
			}
		}
	}

	public void SetSteadyHoldTime(float m_fHoldTimeRate)
	{
	}

	public void SetSteadyShowUI(bool bShow)
	{
	}

	public void ShowGameResult(bool bShow)
	{
		((UIImage)m_control_table["imgGameResult"]).Visible = bShow;
		((UIImage)m_control_table["imgGameResult"]).Enable = bShow;
		((UIImage)m_control_table["imgGameResultTitle"]).Visible = bShow;
		((UIImage)m_control_table["imgGameResultTitle"]).Enable = bShow;
		((UIClickButton)m_control_table["btnBack"]).Visible = bShow;
		((UIClickButton)m_control_table["btnBack"]).Enable = bShow;
		((UIClickButton)m_control_table["btnGear"]).Visible = bShow;
		((UIClickButton)m_control_table["btnGear"]).Enable = bShow;
		((UIClickButton)m_control_table["btnRetry"]).Visible = bShow;
		((UIClickButton)m_control_table["btnRetry"]).Enable = bShow;
		((UIImage)m_control_table["imgDataInfo"]).Visible = bShow;
		((UIText)m_control_table["txtInnocents"]).Visible = bShow;
		((UIText)m_control_table["txtGold"]).Visible = bShow;
		((UIText)m_control_table["txtDollar"]).Visible = bShow;
		((UIImage)m_control_table["imgResultBest"]).Visible = bShow;
		((UIImage)m_control_table["imgResultBestIcon"]).Visible = bShow;
		m_tImgResultBest.Show(bShow);
		bGameResult = bShow;
		if (bShow)
		{
			if (m_NumRollList == null)
			{
				m_NumRollList = new ArrayList();
			}
			m_NumRollList.Clear();
			fGameResultCount = 0f;
			nCurrLine = 0;
			ShowGameResultLine(nCurrLine, bShow);
			((UIText)m_control_table["txtInnocents"]).SetText(m_GameState.m_nSaveInnoTotalNum.ToString());
			((UIText)m_control_table["txtGold"]).SetText(m_GameState.m_nPlayerTotalCash.ToString());
			((UIText)m_control_table["txtDollar"]).SetText(m_GameState.m_nPlayerTotalGod.ToString());
			m_tImgResultBest.SetText(m_GameState.GetBestKill(m_GameState.m_nCurStage).ToString(), new Color(202f, 135f, 21f, 255f));
			return;
		}
		((UIText)m_control_table["txtResultZomKill"]).Visible = bShow;
		((UIText)m_control_table["txtResultZomKillNum"]).Visible = bShow;
		((UIText)m_control_table["txtResultZomKillNumPrice"]).Visible = bShow;
		((UIText)m_control_table["txtResultZomKillNumTotalPrice"]).Visible = bShow;
		((UIImage)m_control_table["imgResultZomKillMoney"]).Visible = bShow;
		((UIImage)m_control_table["imgResultZomKillTotalMoney"]).Visible = bShow;
		((UIImage)m_control_table["imgResultZomKillIcon"]).Visible = bShow;
		((UIText)m_control_table["txtResultGiaKill"]).Visible = bShow;
		((UIText)m_control_table["txtResultGiaKillNum"]).Visible = bShow;
		((UIText)m_control_table["txtResultGiaKillNumPrice"]).Visible = bShow;
		((UIText)m_control_table["txtResultGiaKillNumTotalPrice"]).Visible = bShow;
		((UIImage)m_control_table["imgResultGiaKillMoney"]).Visible = bShow;
		((UIImage)m_control_table["imgResultGiaKillTotalMoney"]).Visible = bShow;
		((UIImage)m_control_table["imgResultGiaKillIcon"]).Visible = bShow;
		((UIText)m_control_table["txtResultInoSave"]).Visible = bShow;
		((UIText)m_control_table["txtResultInoSaveNum"]).Visible = bShow;
		((UIText)m_control_table["txtResultInoSaveNumPrice"]).Visible = bShow;
		((UIText)m_control_table["txtResultInoSaveNumTotalPrice"]).Visible = bShow;
		((UIImage)m_control_table["imgResultInoSaveMoney"]).Visible = bShow;
		((UIImage)m_control_table["imgResultInoSaveTotalMoney"]).Visible = bShow;
		((UIImage)m_control_table["imgResultInoSaveIcon"]).Visible = bShow;
		((UIText)m_control_table["txtResultInoKill"]).Visible = bShow;
		((UIText)m_control_table["txtResultInoKillNum"]).Visible = bShow;
		((UIText)m_control_table["txtResultInoKillNumPrice"]).Visible = bShow;
		((UIText)m_control_table["txtResultInoKillNumTotalPrice"]).Visible = bShow;
		((UIImage)m_control_table["imgResultInoKillMoney"]).Visible = bShow;
		((UIImage)m_control_table["imgResultInoKillTotalMoney"]).Visible = bShow;
		((UIImage)m_control_table["imgResultInoKillIcon"]).Visible = bShow;
		((UIText)m_control_table["txtResultTimeTotal"]).Visible = bShow;
		((UIText)m_control_table["txtResultTimeBonus"]).Visible = bShow;
		((UIText)m_control_table["txtResultTimeBonusNum"]).Visible = bShow;
		((UIImage)m_control_table["imgResultTimeBonusMoney"]).Visible = bShow;
		((UIText)m_control_table["txtResultCash"]).Visible = bShow;
		((UIText)m_control_table["txtResultCashNum"]).Visible = bShow;
		((UIImage)m_control_table["imgResultCashMoney"]).Visible = bShow;
		((UIText)m_control_table["txtResultAch"]).Visible = bShow;
		((UIText)m_control_table["txtResultAchContext"]).Visible = bShow;
		((UIImage)m_control_table["imgResultAchBackGround"]).Visible = bShow;
	}

	public void UpdateGameResultLogic(float deltaTime)
	{
		if (!bGameResult)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		foreach (NumRollInfo numRoll in m_NumRollList)
		{
			numRoll.Update(deltaTime);
			if (numRoll.m_bIsOK)
			{
				arrayList.Add(numRoll);
			}
		}
		foreach (NumRollInfo item in arrayList)
		{
			m_NumRollList.Remove(item);
		}
		fGameResultCount += deltaTime;
		if (!(fGameResultCount < 1f))
		{
			fGameResultCount = 0f;
			ShowGameResultLine(++nCurrLine, true);
		}
	}

	public void ShowGameResultLine(int nLine, bool bShow)
	{
		switch (nLine)
		{
		case 0:
			((UIText)m_control_table["txtResultZomKill"]).Visible = bShow;
			((UIText)m_control_table["txtResultZomKillNum"]).Visible = bShow;
			((UIText)m_control_table["txtResultZomKillNumPrice"]).Visible = bShow;
			((UIText)m_control_table["txtResultZomKillNumTotalPrice"]).Visible = bShow;
			((UIImage)m_control_table["imgResultZomKillMoney"]).Visible = bShow;
			((UIImage)m_control_table["imgResultZomKillTotalMoney"]).Visible = bShow;
			((UIImage)m_control_table["imgResultZomKillIcon"]).Visible = bShow;
			if (bShow)
			{
				((UIText)m_control_table["txtResultZomKill"]).SetText("ZOMBIES KILLED:");
				((UIText)m_control_table["txtResultZomKillNumPrice"]).SetText("X       " + m_GameState.AdjustZombiePrice(10));
				NumRollInfo value4 = new NumRollInfo(0, m_GameState.m_nKillNorEnemyNum, 2f, "%d", (UIText)m_control_table["txtResultZomKillNum"]);
				m_NumRollList.Add(value4);
				value4 = new NumRollInfo(0, m_GameState.m_nKillEnemyCash, 2f, "=         %d", (UIText)m_control_table["txtResultZomKillNumTotalPrice"]);
				m_NumRollList.Add(value4);
				m_GameScene.PlayAudio("UICash");
			}
			break;
		case 1:
			((UIText)m_control_table["txtResultGiaKill"]).Visible = bShow;
			((UIText)m_control_table["txtResultGiaKillNum"]).Visible = bShow;
			((UIText)m_control_table["txtResultGiaKillNumPrice"]).Visible = bShow;
			((UIText)m_control_table["txtResultGiaKillNumTotalPrice"]).Visible = bShow;
			((UIImage)m_control_table["imgResultGiaKillMoney"]).Visible = bShow;
			((UIImage)m_control_table["imgResultGiaKillTotalMoney"]).Visible = bShow;
			((UIImage)m_control_table["imgResultGiaKillIcon"]).Visible = bShow;
			if (bShow)
			{
				((UIText)m_control_table["txtResultGiaKill"]).SetText("GIANTS KILLED:");
				((UIText)m_control_table["txtResultGiaKillNumPrice"]).SetText("X       " + m_GameState.AdjustZombiePrice(100));
				NumRollInfo value6 = new NumRollInfo(0, m_GameState.m_nKillGiaEnemyNum, 2f, "%d", (UIText)m_control_table["txtResultGiaKillNum"]);
				m_NumRollList.Add(value6);
				value6 = new NumRollInfo(0, m_GameState.m_nKillGiaEnemyCash, 2f, "=         %d", (UIText)m_control_table["txtResultGiaKillNumTotalPrice"]);
				m_NumRollList.Add(value6);
				m_GameScene.PlayAudio("UICash");
			}
			break;
		case 2:
			((UIText)m_control_table["txtResultInoSave"]).Visible = bShow;
			((UIText)m_control_table["txtResultInoSaveNum"]).Visible = bShow;
			((UIText)m_control_table["txtResultInoSaveNumPrice"]).Visible = bShow;
			((UIText)m_control_table["txtResultInoSaveNumTotalPrice"]).Visible = bShow;
			((UIImage)m_control_table["imgResultInoSaveMoney"]).Visible = bShow;
			((UIImage)m_control_table["imgResultInoSaveTotalMoney"]).Visible = bShow;
			((UIImage)m_control_table["imgResultInoSaveIcon"]).Visible = bShow;
			if (bShow)
			{
				((UIText)m_control_table["txtResultInoSave"]).SetText("CIVS SAVED:");
				((UIText)m_control_table["txtResultInoSaveNumPrice"]).SetText("X       " + m_GameState.AdjustZombiePrice(50));
				NumRollInfo value2 = new NumRollInfo(0, m_GameState.m_nSaveInnoNum, 2f, "%d", (UIText)m_control_table["txtResultInoSaveNum"]);
				m_NumRollList.Add(value2);
				value2 = new NumRollInfo(0, m_GameState.m_nSaveInnoCash, 2f, "=         %d", (UIText)m_control_table["txtResultInoSaveNumTotalPrice"]);
				m_NumRollList.Add(value2);
				m_GameScene.PlayAudio("UICash");
			}
			break;
		case 3:
			((UIText)m_control_table["txtResultInoKill"]).Visible = bShow;
			((UIText)m_control_table["txtResultInoKillNum"]).Visible = bShow;
			((UIText)m_control_table["txtResultInoKillNumPrice"]).Visible = bShow;
			((UIText)m_control_table["txtResultInoKillNumTotalPrice"]).Visible = bShow;
			((UIImage)m_control_table["imgResultInoKillMoney"]).Visible = bShow;
			((UIImage)m_control_table["imgResultInoKillTotalMoney"]).Visible = bShow;
			((UIImage)m_control_table["imgResultInoKillIcon"]).Visible = bShow;
			if (bShow)
			{
				((UIText)m_control_table["txtResultInoKill"]).SetText("CIVS KILLED:");
				((UIText)m_control_table["txtResultInoKillNumPrice"]).SetText("X       " + m_GameState.AdjustZombiePrice(-50));
				NumRollInfo value5 = new NumRollInfo(0, m_GameState.m_nKillInnoNum, 2f, "%d", (UIText)m_control_table["txtResultInoKillNum"]);
				m_NumRollList.Add(value5);
				value5 = new NumRollInfo(0, m_GameState.m_nKillInnoCash, 2f, "=         %d", (UIText)m_control_table["txtResultInoKillNumTotalPrice"]);
				m_NumRollList.Add(value5);
				m_GameScene.PlayAudio("UICash");
			}
			break;
		case 4:
			((UIText)m_control_table["txtResultTimeTotal"]).Visible = bShow;
			((UIText)m_control_table["txtResultTimeBonus"]).Visible = bShow;
			((UIText)m_control_table["txtResultTimeBonusNum"]).Visible = bShow;
			((UIImage)m_control_table["imgResultTimeBonusMoney"]).Visible = bShow;
			if (bShow)
			{
				((UIText)m_control_table["txtResultTimeTotal"]).SetText("Time: " + Utils.TimeToString(m_GameScene.m_fGameTimeTotal));
				((UIText)m_control_table["txtResultTimeBonus"]).SetText("TIME BONUS    =");
				NumRollInfo value3 = new NumRollInfo(0, m_GameState.m_nTimeBonus, 2f, "%d", (UIText)m_control_table["txtResultTimeBonusNum"]);
				m_NumRollList.Add(value3);
				m_GameScene.PlayAudio("UICash");
			}
			break;
		case 5:
			((UIText)m_control_table["txtResultCash"]).Visible = bShow;
			((UIText)m_control_table["txtResultCashNum"]).Visible = bShow;
			((UIImage)m_control_table["imgResultCashMoney"]).Visible = bShow;
			if (bShow)
			{
				((UIText)m_control_table["txtResultCash"]).SetText("CASH    =");
				NumRollInfo value = new NumRollInfo(0, m_GameState.m_nPlayerCash, 2f, "%d", (UIText)m_control_table["txtResultCashNum"]);
				m_NumRollList.Add(value);
				m_GameScene.PlayAudio("UICash");
			}
			break;
		case 6:
			break;
		}
	}

	public void SetWarnText(string str)
	{
		((UIText)m_control_table["txtOutOfRange"]).Visible = true;
		((UIText)m_control_table["txtOutOfRange"]).SetAlpha(1f);
		((UIText)m_control_table["txtOutOfRange"]).SetText(str);
		StartAnimation("outofrange");
	}

	public void SetGameHelpUI(string sMaterialName, Vector4 v4Icon, string sDesc, Vector4 v4Desc)
	{
		if (sDesc.Length >= 1)
		{
			((UIText)m_control_table["txtHelpDesc"]).Visible = true;
			((UIText)m_control_table["txtHelpDesc"]).Rect = new Rect(v4Desc.x - v4Desc.z / 2f, v4Desc.y - v4Desc.w / 2f, v4Desc.z, v4Desc.w);
			((UIText)m_control_table["txtHelpDesc"]).SetText(sDesc);
			((UIText)m_control_table["txtHelpDesc"]).SetAlpha(1f);
			StopAnimation("helpDescFadeOut");
		}
	}

	public void SetGameHelpIconPos(Vector2 v2Pos)
	{
		if (m_ImageAnim != null)
		{
			m_ImageAnim.SetPos(v2Pos);
			m_ImageAnim.Show(true);
			m_ImageAnim.PlayAnim(0.1f, true);
		}
		if (m_FingerClick != null)
		{
			v2Pos += new Vector2(24f, -21f) * m_GameState.m_nHDFactor;
			m_FingerClick.SetPos(v2Pos);
			m_FingerClick.Show(true);
			m_FingerClick.SetFrame(0);
		}
	}

	public void RemoveGameHelpUI()
	{
		((UIImage)m_control_table["imgHelpIcon"]).Visible = false;
		((UIText)m_control_table["txtHelpDesc"]).Visible = false;
		m_ImageAnim.StopAnim();
		m_ImageAnim.Show(false);
		m_FingerClick.StopAnim();
		m_FingerClick.Show(false);
	}

	public void AddImageWarn1(ImageMsgEnum1 type)
	{
		if (m_GameScene.m_State == iZombieSniperGameSceneBase.State.kGaming && m_ImageMsg1 != null)
		{
			m_ImageMsg1.Play((int)type);
		}
	}

	public void AddImageWarn2(ImageMsgEnum2 type)
	{
		if (m_GameScene.m_State == iZombieSniperGameSceneBase.State.kGaming && m_ImageMsg2 != null)
		{
			m_ImageMsg2.Play((int)type);
		}
	}

	public void AddImageWarn3(ImageMsgEnum3 type, int nCount)
	{
		if (m_GameScene.m_State == iZombieSniperGameSceneBase.State.kGaming && m_ImageMsg3 != null)
		{
			m_ImageMsg3.Play((int)type);
			if (m_ImageMsgStrike != null)
			{
				m_ImageMsgStrike.SetText(nCount.ToString(), Color.white);
				m_ImageMsgStrike.FadeOut(2f);
			}
		}
	}

	public void AddRollText(string str)
	{
		if (!m_GameScene.m_bTutorial)
		{
			m_RollText.AddText(str);
		}
	}

	public UIText GetTextPrint()
	{
		return (UIText)m_control_table["txtMainChar"];
	}

	public UIImage GetUIImage(string sName)
	{
		if (m_control_table.ContainsKey(sName))
		{
			return (UIImage)m_control_table[sName];
		}
		return null;
	}

	public void ShowGamePause(bool bShow)
	{
		((UIImage)m_control_table["imgGamePauseMask"]).Visible = bShow;
		((UIImage)m_control_table["imgGamePauseMask"]).Enable = bShow;
		((UIImage)m_control_table["imgGamePause"]).Visible = bShow;
		((UIImage)m_control_table["imgGamePause"]).Enable = bShow;
		((UIImage)m_control_table["imgGamePauseTitle"]).Visible = bShow;
		((UIImage)m_control_table["imgGamePauseTitle"]).Enable = bShow;
		((UIImage)m_control_table["imgGamePauseMusic"]).Visible = bShow;
		((UIImage)m_control_table["imgGamePauseSound"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPauseMusicON"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPauseMusicON"]).Enable = bShow;
		((UIClickButton)m_control_table["btnPauseMusicOFF"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPauseMusicOFF"]).Enable = bShow;
		((UIClickButton)m_control_table["btnPauseSoundON"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPauseSoundON"]).Enable = bShow;
		((UIClickButton)m_control_table["btnPauseSoundOFF"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPauseSoundOFF"]).Enable = bShow;
		if (bShow)
		{
			SetMusic(m_GameState.m_bMusicOn);
			SetSound(m_GameState.m_bSoundOn);
			((UIImage)m_control_table["fadeimage"]).Visible = false;
			((UIImage)m_control_table["fadeimage"]).Enable = false;
		}
		((UIClickButton)m_control_table["btnPauseBack"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPauseBack"]).Enable = bShow;
		((UIClickButton)m_control_table["btnPauseRetry"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPauseRetry"]).Enable = bShow;
		((UIClickButton)m_control_table["btnPauseResume"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPauseResume"]).Enable = bShow;
	}

	public void SetMusic(bool bOn)
	{
		((UIClickButton)m_control_table["btnPauseMusicON"]).Enable = !bOn;
		((UIClickButton)m_control_table["btnPauseMusicOFF"]).Enable = bOn;
	}

	public void SetSound(bool bOn)
	{
		((UIClickButton)m_control_table["btnPauseSoundON"]).Enable = !bOn;
		((UIClickButton)m_control_table["btnPauseSoundOFF"]).Enable = bOn;
	}

	public void ShowGameUI(bool bShow)
	{
		ShowWeaponUI(bShow);
		((UIImage)m_control_table["imgScore"]).Visible = bShow;
		if (m_tImgKillZombie != null)
		{
			m_tImgKillZombie.Show(bShow);
		}
		if (m_tImgKillInnocent != null)
		{
			m_tImgKillInnocent.Show(bShow);
		}
		if (m_tImgSaveInnocent != null)
		{
			m_tImgSaveInnocent.Show(bShow);
		}
		((UIText)m_control_table["txtGameTime"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPauseOn"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPauseOn"]).Enable = bShow;
		((UIClickButton)m_control_table["btnPauseOff"]).Visible = bShow;
		((UIClickButton)m_control_table["btnPauseOff"]).Enable = bShow;
		((UIClickButton)m_control_table["btnFire"]).Visible = bShow;
		((UIClickButton)m_control_table["btnFire"]).Enable = bShow;
		((UIClickButton)m_control_table["btnAC130"]).Visible = bShow;
		((UIClickButton)m_control_table["btnAC130"]).Enable = bShow;
		((UIImage)m_control_table["imgAC130Back"]).Visible = bShow;
		((UIClickButton)m_control_table["btnMachine"]).Visible = bShow;
		((UIClickButton)m_control_table["btnMachine"]).Enable = bShow;
		if (bShow)
		{
			if (m_GameScene.m_State != iZombieSniperGameSceneBase.State.kGaming)
			{
				((UIImage)m_control_table["imgScore"]).Visible = false;
				if (m_tImgKillZombie != null)
				{
					m_tImgKillZombie.Show(false);
				}
				if (m_tImgKillInnocent != null)
				{
					m_tImgKillInnocent.Show(false);
				}
				if (m_tImgSaveInnocent != null)
				{
					m_tImgSaveInnocent.Show(false);
				}
			}
			if (m_GameScene.GetPause())
			{
				((UIClickButton)m_control_table["btnPauseOn"]).Visible = false;
				((UIClickButton)m_control_table["btnPauseOn"]).Enable = false;
			}
			else
			{
				((UIClickButton)m_control_table["btnPauseOff"]).Visible = false;
				((UIClickButton)m_control_table["btnPauseOff"]).Enable = false;
			}
			float num = 100f * (float)m_GameState.m_nHDFactor;
			Rect rect = ((UIClickButton)m_control_table["btnFire"]).Rect;
			((UIClickButton)m_control_table["btnFire"]).Rect = new Rect(rect.left + num, rect.top, rect.width, rect.height);
			((UIClickButton)m_control_table["btnAC130"]).Enable = false;
			((UIClickButton)m_control_table["btnAC130"]).Visible = false;
			((UIClickButton)m_control_table["btnMachine"]).Enable = false;
			((UIClickButton)m_control_table["btnMachine"]).Visible = false;
			((UIImage)m_control_table["imgAC130Back"]).Visible = false;
			if (m_GameScene.m_State == iZombieSniperGameSceneBase.State.kGaming && !m_GameScene.IsSpecialButtonDone())
			{
				switch (m_GameState.m_nCurStage)
				{
				case 0:
				case 1:
					if (m_GameState.m_nAC130 > 0)
					{
						((UIClickButton)m_control_table["btnAC130"]).Enable = true;
						((UIClickButton)m_control_table["btnAC130"]).Visible = true;
						((UIClickButton)m_control_table["btnAC130"]).SetRect(Utils.CalcScaleRect(new Rect(470f, 140f, 100f, 100f)));
					}
					break;
				case 2:
					if (m_GameState.m_nMachineGun > 0)
					{
						((UIClickButton)m_control_table["btnMachine"]).Enable = true;
						((UIClickButton)m_control_table["btnMachine"]).Visible = true;
						((UIClickButton)m_control_table["btnMachine"]).SetRect(Utils.CalcScaleRect(new Rect(458f, 140f, 100f, 100f)));
					}
					break;
				}
				((UIImage)m_control_table["imgAC130Back"]).Visible = true;
				((UIImage)m_control_table["imgAC130Back"]).SetRect(Utils.CalcScaleRect(new Rect(540f, 170f, 13f, 31f)));
			}
			StartAnimation("gameui_movein");
		}
		else
		{
			m_tImgBulletNum.Show(false);
			((UIImage)m_control_table["imgBullet1"]).Visible = false;
			((UIImage)m_control_table["imgSniperAimRB"]).Visible = false;
		}
	}

	public void ShowWeaponUI(bool bShow)
	{
		if (bShow && m_GameState.IsInWeaponTool())
		{
			return;
		}
		((UIClickButton)m_control_table["btnGunBackGround"]).Visible = bShow;
		((UIImage)m_control_table["imgGunBackGroundMask"]).Visible = bShow;
		((UIImage)m_control_table["weaponIcon"]).Visible = bShow;
		((UIImage)m_control_table["imgWeapon1_on"]).Visible = bShow;
		((UIImage)m_control_table["imgWeapon1_off"]).Visible = bShow;
		((UIImage)m_control_table["imgWeapon2_on"]).Visible = bShow;
		((UIImage)m_control_table["imgWeapon2_off"]).Visible = bShow;
		((UIImage)m_control_table["imgWeapon3_on"]).Visible = bShow;
		((UIImage)m_control_table["imgWeapon3_off"]).Visible = bShow;
		if (bShow)
		{
			float num = 100f * (float)m_GameState.m_nHDFactor;
			Rect rect = ((UIClickButton)m_control_table["btnGunBackGround"]).Rect;
			((UIClickButton)m_control_table["btnGunBackGround"]).Rect = new Rect(rect.left + num, rect.top, rect.width, rect.height);
			((UIImage)m_control_table["weaponIcon"]).SetPosition(((UIImage)m_control_table["weaponIcon"]).GetPosition() + new Vector2(num, 0f));
			((UIImage)m_control_table["imgWeapon1_on"]).SetPosition(((UIImage)m_control_table["imgWeapon1_on"]).GetPosition() + new Vector2(num, 0f));
			((UIImage)m_control_table["imgWeapon1_off"]).SetPosition(((UIImage)m_control_table["imgWeapon1_off"]).GetPosition() + new Vector2(num, 0f));
			((UIImage)m_control_table["imgWeapon2_on"]).SetPosition(((UIImage)m_control_table["imgWeapon2_on"]).GetPosition() + new Vector2(num, 0f));
			((UIImage)m_control_table["imgWeapon2_off"]).SetPosition(((UIImage)m_control_table["imgWeapon2_off"]).GetPosition() + new Vector2(num, 0f));
			((UIImage)m_control_table["imgWeapon3_on"]).SetPosition(((UIImage)m_control_table["imgWeapon3_on"]).GetPosition() + new Vector2(num, 0f));
			((UIImage)m_control_table["imgWeapon3_off"]).SetPosition(((UIImage)m_control_table["imgWeapon3_off"]).GetPosition() + new Vector2(num, 0f));
			UpdateWeaponButton();
			if (m_GameScene.m_CurrWeapon.m_WeaponState != iWeapon.WeaponState.kReload)
			{
				((UIImage)m_control_table["imgGunBackGroundMask"]).Visible = false;
			}
			StartAnimation("gameui_weapon_movein");
		}
		else
		{
			((UIClickButton)m_control_table["btnGunBackGround"]).Enable = false;
		}
	}

	public override void OnAnimationFinished(string name)
	{
		switch (name)
		{
		case "fadeready":
			HideAimUI();
			FadeIn();
			break;
		case "fadein":
			((UIImage)m_control_table["fadeimage"]).Visible = false;
			((UIImage)m_control_table["fadeimage"]).Enable = false;
			break;
		case "gameui_movein":
			m_tImgBulletNum.Show(true);
			((UIImage)m_control_table["imgBullet1"]).Visible = true;
			((UIImage)m_control_table["imgSniperAimRB"]).Visible = true;
			break;
		case "gameui_weapon_movein":
			((UIClickButton)m_control_table["btnGunBackGround"]).Enable = true;
			break;
		case "gameui_weaponmask":
			((UIImage)m_control_table["imgGunBackGroundMask"]).Visible = false;
			break;
		case "gameui_ac130moveout":
			((UIClickButton)m_control_table["btnAC130"]).Visible = false;
			((UIClickButton)m_control_table["btnMachine"]).Visible = false;
			((UIImage)m_control_table["imgAC130Back"]).Visible = false;
			break;
		case "imgWarnZombieClose_fadeout":
		case "imgWarnKillMany_fadeout":
		case "imgWarnMissKill_fadeout":
		case "imgWarnHeadShoot_fadeout":
		case "imgWarnAC130_fadeout":
			if (m_ImageMsg1 != null)
			{
				m_ImageMsg1.Stop();
			}
			break;
		case "imgWarnOutOfRange_fadeout":
		case "imgWarnReloading_fadeout":
		case "imgDontKillInoo_fadeout":
			if (m_ImageMsg2 != null)
			{
				m_ImageMsg2.Stop();
			}
			break;
		case "imgKillInooStrike1_fadeout":
		case "imgKillInooStrike2_fadeout":
		case "imgKillInooStrike3_fadeout":
			if (m_ImageMsg3 != null)
			{
				m_ImageMsg3.Stop();
			}
			break;
		case "imgimgDefenceLife_fadeout":
			ShowDefenceUI(false);
			break;
		}
	}

	public void ShowDialog(DialogEnum type, string text)
	{
		m_nDialogType = type;
		((UIText)m_control_table["txtPOPText"]).Visible = true;
		((UIText)m_control_table["txtPOPText"]).SetText(text);
		((UIImage)m_control_table["imgPOPMask"]).Visible = true;
		((UIImage)m_control_table["imgPOPMask"]).Enable = true;
		((UIImage)m_control_table["imgPOPBack"]).Visible = true;
		((UIClickButton)m_control_table["btnPOPConfirm"]).Visible = true;
		((UIClickButton)m_control_table["btnPOPConfirm"]).Enable = true;
		((UIClickButton)m_control_table["btnPOPCancel"]).Visible = true;
		((UIClickButton)m_control_table["btnPOPCancel"]).Enable = true;
	}

	public void HideDialog()
	{
		m_nDialogType = DialogEnum.None;
		((UIText)m_control_table["txtPOPText"]).Visible = false;
		((UIImage)m_control_table["imgPOPMask"]).Visible = false;
		((UIImage)m_control_table["imgPOPMask"]).Enable = false;
		((UIImage)m_control_table["imgPOPBack"]).Visible = false;
		((UIClickButton)m_control_table["btnPOPConfirm"]).Visible = false;
		((UIClickButton)m_control_table["btnPOPConfirm"]).Enable = false;
		((UIClickButton)m_control_table["btnPOPCancel"]).Visible = false;
		((UIClickButton)m_control_table["btnPOPCancel"]).Enable = false;
	}

	public void StartWeaponMask(float fTime)
	{
		if (m_animations.ContainsKey("gameui_weaponmask"))
		{
			((UIImage)m_control_table["imgGunBackGroundMask"]).Visible = true;
			((UIImage)m_control_table["imgGunBackGroundMask"]).SetClip(new Rect(((UIImage)m_control_table["imgGunBackGroundMask"]).Rect));
			UIAnimations uIAnimations = (UIAnimations)m_animations["gameui_weaponmask"];
			uIAnimations.clip_time = fTime;
			StartAnimation("gameui_weaponmask");
		}
	}

	public void StopWeaponMask()
	{
		((UIImage)m_control_table["imgGunBackGroundMask"]).Visible = false;
		StopAnimation("gameui_weaponmask");
	}

	public void StartAlarmScreen(AlarmScreenSide side)
	{
		if (side == AlarmScreenSide.Left || side == AlarmScreenSide.All)
		{
			((UIImage)m_control_table["imgAlarmScreenLeft"]).Visible = true;
			((UIImage)m_control_table["imgAlarmScreenLeft"]).SetAlpha(1f);
			StartAnimation("gameui_alarmscreen_left");
		}
		if (side == AlarmScreenSide.Right || side == AlarmScreenSide.All)
		{
			((UIImage)m_control_table["imgAlarmScreenRight"]).Visible = true;
			((UIImage)m_control_table["imgAlarmScreenRight"]).SetAlpha(1f);
			StartAnimation("gameui_alarmscreen_right");
		}
	}

	public void StopAlarmScreen(AlarmScreenSide side)
	{
		if (side == AlarmScreenSide.Left || side == AlarmScreenSide.All)
		{
			((UIImage)m_control_table["imgAlarmScreenLeft"]).Visible = false;
			StopAnimation("gameui_alarmscreen_left");
		}
		if (side == AlarmScreenSide.Right || side == AlarmScreenSide.All)
		{
			((UIImage)m_control_table["imgAlarmScreenRight"]).Visible = false;
			StopAnimation("gameui_alarmscreen_right");
		}
	}

	public void HideAC130()
	{
		switch (m_GameState.m_nCurStage)
		{
		case 0:
		case 1:
			((UIClickButton)m_control_table["btnAC130"]).SetRect(Utils.CalcScaleRect(new Rect(370f, 140f, 100f, 100f)));
			((UIClickButton)m_control_table["btnAC130"]).Enable = false;
			break;
		case 2:
			((UIClickButton)m_control_table["btnMachine"]).SetRect(Utils.CalcScaleRect(new Rect(358f, 140f, 100f, 100f)));
			((UIClickButton)m_control_table["btnMachine"]).Enable = false;
			break;
		}
		((UIImage)m_control_table["imgAC130Back"]).SetRect(Utils.CalcScaleRect(new Rect(440f, 170f, 13f, 31f)));
		StartAnimation("gameui_ac130moveout");
	}

	public void EnableFire(bool bEnable)
	{
		((UIClickButton)m_control_table["btnFire"]).Enable = bEnable;
		if (!bEnable)
		{
			m_GameScene.SetAutoShoot(false);
		}
	}

	public void ShowMovieUI()
	{
		((UIImage)m_control_table["imgMovieUp"]).Visible = true;
		((UIImage)m_control_table["imgMovieDown"]).Visible = true;
		Vector2 position = ((UIImage)m_control_table["imgMovieUp"]).GetPosition();
		((UIImage)m_control_table["imgMovieUp"]).SetPosition(new Vector2(position.x, position.y + (float)(40 * m_GameState.m_nHDFactor)));
		position = ((UIImage)m_control_table["imgMovieDown"]).GetPosition();
		((UIImage)m_control_table["imgMovieDown"]).SetPosition(new Vector2(position.x, position.y - (float)(40 * m_GameState.m_nHDFactor)));
		StartAnimation("gameui_movieup");
		StartAnimation("gameui_moviedown");
	}

	public void HideMovieUI()
	{
		((UIImage)m_control_table["imgMovieUp"]).Visible = false;
		((UIImage)m_control_table["imgMovieDown"]).Visible = false;
	}

	public void SwitchBulletUI(WEAPON_TYPE nType)
	{
		switch (nType)
		{
		case WEAPON_TYPE.SNIPER_RIFLE:
		case WEAPON_TYPE.AUTOSHOOT:
		case WEAPON_TYPE.MACHINE_GUN:
			((UIImage)m_control_table["imgBullet1"]).SetTexture(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part1")), new Rect(448 * m_GameState.m_nHDFactor, 346 * m_GameState.m_nHDFactor, 60 * m_GameState.m_nHDFactor, 19 * m_GameState.m_nHDFactor));
			break;
		case WEAPON_TYPE.BAZOOKA:
		case WEAPON_TYPE.THROWMINE:
			((UIImage)m_control_table["imgBullet1"]).SetTexture(LoadUIMaterial(Utils.AutoMaterialName("game_ui_part1")), new Rect(432 * m_GameState.m_nHDFactor, 324 * m_GameState.m_nHDFactor, 60 * m_GameState.m_nHDFactor, 21 * m_GameState.m_nHDFactor));
			break;
		}
	}

	public void SetGameTime(float fTime)
	{
		((UIText)m_control_table["txtGameTime"]).Visible = true;
		((UIText)m_control_table["txtGameTime"]).SetText(Utils.TimeToString(fTime));
	}

	public void ShowMainPic(bool bShow)
	{
		((UIImage)m_control_table["imgRod"]).Visible = bShow;
	}

	public void SetCrossPos(float dis)
	{
		((UIImage)m_control_table["imgAimCrossUp"]).SetPosition(m_GameState.m_v3ShootCenter + Vector2.up * dis);
		((UIImage)m_control_table["imgAimCrossDown"]).SetPosition(m_GameState.m_v3ShootCenter - Vector2.up * dis);
		((UIImage)m_control_table["imgAimCrossLeft"]).SetPosition(m_GameState.m_v3ShootCenter + Vector2.right * dis);
		((UIImage)m_control_table["imgAimCrossRight"]).SetPosition(m_GameState.m_v3ShootCenter - Vector2.right * dis);
	}

	public void ShowBloodScreen(float fAlpha)
	{
		((UIImage)m_control_table["imgAlarmScreenLeftScene2"]).Visible = true;
		((UIImage)m_control_table["imgAlarmScreenRightScene2"]).Visible = true;
		((UIImage)m_control_table["imgAlarmScreenTopScene2"]).Visible = true;
		((UIImage)m_control_table["imgAlarmScreenBottomScene2"]).Visible = true;
		((UIImage)m_control_table["imgAlarmScreenLeftScene2"]).SetAlpha(fAlpha);
		((UIImage)m_control_table["imgAlarmScreenRightScene2"]).SetAlpha(fAlpha);
		((UIImage)m_control_table["imgAlarmScreenTopScene2"]).SetAlpha(fAlpha);
		((UIImage)m_control_table["imgAlarmScreenBottomScene2"]).SetAlpha(fAlpha);
	}

	public void HideBloodScreen()
	{
		((UIImage)m_control_table["imgAlarmScreenLeftScene2"]).Visible = false;
		((UIImage)m_control_table["imgAlarmScreenRightScene2"]).Visible = false;
		((UIImage)m_control_table["imgAlarmScreenTopScene2"]).Visible = false;
		((UIImage)m_control_table["imgAlarmScreenBottomScene2"]).Visible = false;
	}

	public void ShowDefenceUI(bool bShow)
	{
		((UIImage)m_control_table["imgDefenceLife"]).Visible = bShow;
		((UIImage)m_control_table["imgDefenceLifeMask"]).Visible = bShow;
	}

	public void SetDefenceUI(float rate)
	{
		Rect rect = ((UIImage)m_control_table["imgDefenceLifeMask"]).Rect;
		rect.height *= rate;
		((UIImage)m_control_table["imgDefenceLifeMask"]).SetClip(rect);
	}

	public bool IsDefenceUIShow()
	{
		return ((UIImage)m_control_table["imgDefenceLife"]).Visible;
	}

	public void FadeInDefenceUI()
	{
		if (!IsDefenceUIShow())
		{
			ShowDefenceUI(true);
			((UIImage)m_control_table["imgDefenceLife"]).SetAlpha(0f);
			((UIImage)m_control_table["imgDefenceLifeMask"]).SetAlpha(0f);
			StartAnimation("imgimgDefenceLife_fadein");
		}
	}

	public void FadeOutDefenceUI()
	{
		if (IsDefenceUIShow() && !((UIAnimations)m_animations["imgimgDefenceLife_fadeout"]).IsRuning())
		{
			((UIImage)m_control_table["imgDefenceLife"]).SetAlpha(1f);
			((UIImage)m_control_table["imgDefenceLifeMask"]).SetAlpha(1f);
			StartAnimation("imgimgDefenceLife_fadeout");
		}
	}
}
