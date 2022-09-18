using UnityEngine;

public class iZombieSniperMarketAnimEvent : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Trigger()
	{
		iZombieSniperGameSceneBase gameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		if (gameScene != null)
		{
			gameScene.m_CameraScript.Shake(0.5f, 2f * gameScene.GetSlowTimeRate(), true, true);
		}
	}
}
