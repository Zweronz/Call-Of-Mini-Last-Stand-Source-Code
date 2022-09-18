using UnityEngine;

public class iZombieSniperContainerAnimEvent : MonoBehaviour
{
	public Vector3 m_v3Pos;

	private void Start()
	{
		Transform transform = base.transform.Find("Dummy01");
		if (transform != null)
		{
			m_v3Pos = transform.position;
			m_v3Pos.y = 0f;
		}
	}

	private void Update()
	{
	}

	public void Trigger()
	{
		iZombieSniperGameSceneBase gameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		if (gameScene == null)
		{
			return;
		}
		GameObject containerSmoke = gameScene.m_PerfabManager.m_ContainerSmoke;
		containerSmoke = (GameObject)Object.Instantiate(containerSmoke, m_v3Pos, Quaternion.identity);
		Object.Destroy(containerSmoke, 1.5f);
		containerSmoke = GameObject.Find("Zombie Sniper_3D");
		if (containerSmoke != null)
		{
			Transform transform = containerSmoke.transform.Find("sundry_00");
			if (transform != null)
			{
				transform.gameObject.active = false;
			}
		}
		gameScene.m_CameraScript.Shake(1f, 1f * gameScene.GetSlowTimeRate(), true, true);
	}
}
