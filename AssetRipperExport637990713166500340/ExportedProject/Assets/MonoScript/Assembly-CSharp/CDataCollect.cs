using System.Collections.Generic;

public class CDataCollect
{
	public string m_sData;

	public float m_fGameTimeToday;

	public int m_nLoginTimesToday;

	public int m_nGoldGainFromGameToday;

	public int m_nGoldGainFromIAPToday;

	public int[] m_arrGainFromIAPToday;

	public int m_nGoldConsumeToday;

	public int m_nTCGainToday;

	public int m_nTCConsumeToday;

	public List<int> m_ltWeaponBuyToday;

	public int[] m_arrItemBuyToday;

	public int[] m_arrItemConsumeToday;

	public int m_nStartGameToday;

	public int m_nGameOverToday;

	public List<float> m_ltGameBreakOutToday;

	public int m_nGoldNow;

	public int m_nTCNow;

	public List<int> m_ltWeapon;

	public int[] m_arrItem;

	public float m_fGameTimeTotal;

	public int m_nGoldGainTotal;

	public int m_nTCGainTotal;

	public int[] m_arrScene;

	public void Initialize()
	{
		m_ltWeaponBuyToday = new List<int>();
		m_ltGameBreakOutToday = new List<float>();
		m_arrGainFromIAPToday = new int[16];
		m_arrItemBuyToday = new int[5];
		m_arrItemConsumeToday = new int[5];
		m_ltWeapon = new List<int>();
		m_arrItem = new int[5];
		m_arrScene = new int[11];
		ResetData();
	}

	public void ResetData()
	{
		m_fGameTimeToday = 0f;
		m_nLoginTimesToday = 0;
		m_nGoldGainFromGameToday = 0;
		m_nGoldGainFromIAPToday = 0;
		for (int i = 0; i < m_arrGainFromIAPToday.Length; i++)
		{
			m_arrGainFromIAPToday[i] = 0;
		}
		m_nGoldConsumeToday = 0;
		m_nTCGainToday = 0;
		m_nTCConsumeToday = 0;
		m_ltWeaponBuyToday.Clear();
		for (int j = 0; j < m_arrItemBuyToday.Length; j++)
		{
			m_arrItemBuyToday[j] = 0;
		}
		for (int k = 0; k < m_arrItemConsumeToday.Length; k++)
		{
			m_arrItemConsumeToday[k] = 0;
		}
		m_nStartGameToday = 0;
		m_nGameOverToday = 0;
		m_ltGameBreakOutToday.Clear();
		m_nGoldNow = 0;
		m_nTCNow = 0;
		m_ltWeapon.Clear();
		for (int l = 0; l < m_arrItem.Length; l++)
		{
			m_arrItem[l] = 0;
		}
		m_fGameTimeTotal = 0f;
		m_nGoldGainTotal = 0;
		m_nTCGainTotal = 0;
		for (int m = 0; m < m_arrScene.Length; m++)
		{
			m_arrScene[m] = 0;
		}
	}
}
