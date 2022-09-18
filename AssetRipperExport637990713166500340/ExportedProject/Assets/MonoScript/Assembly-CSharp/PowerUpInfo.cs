using UnityEngine;

public class PowerUpInfo
{
	public PowerUpEnum m_nType;

	public string m_sMaterial;

	public Rect m_Rect;

	public string m_sName;

	public int m_nReserves;

	public bool m_bGodPrice;

	public int m_nValue;

	public int m_nReservesMax;

	public string m_sDesc;

	public bool m_bNew;

	public bool IsMax()
	{
		return m_nReservesMax > 0 && m_nReserves >= m_nReservesMax;
	}

	public int GetPrice()
	{
		return iZombieSniperGameApp.GetInstance().GetPrice(m_nType, m_nReserves);
	}
}
