using UnityEngine;

public class iZombieSniperLocalNotification : MonoBehaviour
{
	private string[] m_sLocalNotifyStr = new string[3] { "Zombie hordes incoming! The town's survival is in your hands!", "Zombies have taken the town! Help them rest in peace!", "The people of Muerte Vista need your help! Send the undead intruders back to the grave!" };

	private void Start()
	{
		ClearSchedule();
	}

	private void OnApplicationPause(bool bPause)
	{
		if (bPause)
		{
			SetSchedule();
		}
		else
		{
			ClearSchedule();
		}
	}

	private void OnApplicationQuit()
	{
		SetSchedule();
	}

	private void SetSchedule()
	{
		string message = m_sLocalNotifyStr[UnityEngine.Random.Range(0, m_sLocalNotifyStr.Length)];
		LocalNotificationWrapper.Schedule(message, 604800);
		Debug.Log("alart after 60 * 60 * 24 * 7");
	}

	private void ClearSchedule()
	{
		Debug.Log("clear schedule");
		LocalNotificationWrapper.CancelAll();
	}
}
