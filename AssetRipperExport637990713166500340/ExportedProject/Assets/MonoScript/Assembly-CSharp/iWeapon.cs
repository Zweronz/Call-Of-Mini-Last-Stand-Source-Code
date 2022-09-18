public class iWeapon
{
	public enum WeaponState
	{
		kNormal = 0,
		kInterval = 1,
		kReload = 2
	}

	public int m_nWeaponID;

	public float m_fSD;

	public float m_fSR;

	public float m_fSG;

	public float m_fSZ;

	public float m_fSS;

	public float m_fSW;

	public WeaponState m_WeaponState;

	public float m_fTimeCount;

	public int m_nCurrBulletNum;

	public int m_nBulletMax;

	public bool m_bReloadSound;

	public int m_nRocketReloadState;

	public float m_fRocketReloadTimeOut;

	public float m_fRocketReloadTimeWait;

	public float m_fRocketReloadCount;

	public iWeapon(int nWeaponID)
	{
		Reset();
		m_nWeaponID = nWeaponID;
		m_WeaponState = WeaponState.kNormal;
		m_fTimeCount = 0f;
		m_bReloadSound = false;
	}

	public void Reset()
	{
		m_nWeaponID = 0;
		m_nCurrBulletNum = 0;
		m_nBulletMax = 0;
		m_fSD = 0f;
		m_fSR = 0f;
		m_fSG = 0f;
		m_fSZ = 0f;
		m_fSS = 0f;
		m_fSW = 0f;
		m_nRocketReloadState = 0;
		m_fRocketReloadTimeOut = 0f;
		m_fRocketReloadTimeWait = 0f;
		m_fRocketReloadCount = 0f;
	}

	public void SetInterval()
	{
		if (m_WeaponState != WeaponState.kReload && m_WeaponState != WeaponState.kInterval)
		{
			m_WeaponState = WeaponState.kInterval;
			m_fTimeCount = 0f;
		}
	}

	public void SetReload()
	{
		if (m_WeaponState != WeaponState.kReload)
		{
			m_WeaponState = WeaponState.kReload;
			m_fTimeCount = 0f;
			m_bReloadSound = false;
		}
	}

	public bool IsCanFire()
	{
		return m_WeaponState == WeaponState.kNormal;
	}
}
