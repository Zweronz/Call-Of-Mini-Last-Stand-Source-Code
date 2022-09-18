using UnityEngine;

public class iZombieSniperGame : MonoBehaviour
{
	private void Start()
	{
		iZombieSniperGameState gameState = iZombieSniperGameApp.GetInstance().m_GameState;
		if (gameState != null)
		{
			iZombieSniperGameApp.GetInstance().CreateScene(gameState.m_nCurStage);
		}
	}

	private void Update()
	{
		iZombieSniperGameApp.GetInstance().Loop();
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Time.timeScale = 0.2f;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Time.timeScale = 1f;
		}
	}

	private void OnApplicationPause(bool bPause)
	{
		iZombieSniperGameApp.GetInstance().OnApplicationPause(bPause);
	}

	private void OnApplicationQuit()
	{
		iZombieSniperGameApp.GetInstance().OnApplicationQuit();
	}
}
