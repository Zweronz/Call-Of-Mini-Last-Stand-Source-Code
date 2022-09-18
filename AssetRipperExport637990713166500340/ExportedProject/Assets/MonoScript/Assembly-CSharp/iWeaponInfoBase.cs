public class iWeaponInfoBase
{
	public int m_nWeaponID;

	public int m_nWeaponType;

	public int m_nIconID;

	public string m_sFireSound;

	public int m_nFireEffect;

	public string m_sName;

	public string m_sDesc;

	public int m_nCondition;

	public int m_nCondValue;

	public bool m_bGodPrice;

	public int m_nPrice;

	public int m_nBulletMax;

	public bool m_bNightVision;

	public int m_nShopPriority;

	public bool m_bNewWeapon;

	public float m_fBaseSD;

	public float m_fBaseSR;

	public float m_fBaseSG;

	public float m_fBaseSZ;

	public float m_fBaseSS;

	public float m_fBaseSW;

	public bool IsRocket()
	{
		return m_nWeaponType == 3;
	}

	public bool IsRifle()
	{
		return m_nWeaponType == 1;
	}

	public bool IsAutoShoot()
	{
		return m_nWeaponType == 2;
	}

	public bool IsThrowMine()
	{
		return m_nWeaponType == 4;
	}

	public bool IsMachineGun()
	{
		return m_nWeaponType == 5;
	}

	public bool IsConditionInno()
	{
		return m_nCondition == 1;
	}

	public bool IsRocketBlue()
	{
		return m_nFireEffect == 3;
	}

	public bool IsNewWeapon()
	{
		return m_bNewWeapon;
	}
}
