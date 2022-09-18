using UnityEngine;

public class iZombieSniperWeaponManager : MonoBehaviour
{
	public static int m_WeaponCount = 26;

	public GameObject[] m_WeaponList = new GameObject[m_WeaponCount];

	public void Clear()
	{
		for (int i = 0; i < m_WeaponCount; i++)
		{
			m_WeaponList[i] = null;
		}
	}
}
