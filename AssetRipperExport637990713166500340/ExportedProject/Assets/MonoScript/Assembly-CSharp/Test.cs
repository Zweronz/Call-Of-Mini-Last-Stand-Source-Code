using UnityEngine;

public class Test : MonoBehaviour
{
	private void Start()
	{
		iZombieSniperGameApp.GetInstance();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			iZombieSniperGameApp.GetInstance().m_GridInfoCenter.Initialize();
			iZombieSniperGameApp.GetInstance().m_GridInfoCenter.LoadData(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			iZombieSniperGameApp.GetInstance().m_ZombieWaveCenter.Initialize();
			iZombieSniperGameApp.GetInstance().m_ZombieWaveCenter.LoadData(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			iZombieSniperGameApp.GetInstance().m_WayPointCenter.Initialize();
			iZombieSniperGameApp.GetInstance().m_WayPointCenter.LoadWayPoint(0);
		}
	}
}
