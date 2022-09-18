using UnityEngine;

public class iZombieWayPoint : MonoBehaviour
{
	private bool m_bIsInit;

	private iZombieSniperWayPointCenter m_WayPointCenter;

	private void Start()
	{
		base.transform.position = Vector3.zero;
	}

	private void Update()
	{
		if (!m_bIsInit)
		{
			m_WayPointCenter = iZombieSniperGameApp.GetInstance().m_GameScene.m_WayPointCenter;
			if (m_WayPointCenter.m_WayPointConfig.Count > 0)
			{
				GameObject gameObject = null;
				foreach (WayPoint value in m_WayPointCenter.m_WayPointConfig.Values)
				{
					gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					if ((bool)gameObject)
					{
						gameObject.name = "waypoint_" + value.m_nID;
						gameObject.transform.parent = base.transform;
						gameObject.transform.position = value.m_v3Position;
						gameObject.GetComponent<Renderer>().material.color = Color.green;
						SphereCollider component = gameObject.GetComponent<SphereCollider>();
						if ((bool)component)
						{
							Object.Destroy(component);
						}
					}
				}
			}
		}
		Rect finallyZone = m_WayPointCenter.m_FinallyZone;
		Debug.DrawLine(new Vector3(finallyZone.xMin, 0f, finallyZone.yMin), new Vector3(finallyZone.xMin, 0f, finallyZone.yMax));
		Debug.DrawLine(new Vector3(finallyZone.xMin, 0f, finallyZone.yMin), new Vector3(finallyZone.xMax, 0f, finallyZone.yMin));
		Debug.DrawLine(new Vector3(finallyZone.xMax, 0f, finallyZone.yMax), new Vector3(finallyZone.xMax, 0f, finallyZone.yMin));
		Debug.DrawLine(new Vector3(finallyZone.xMax, 0f, finallyZone.yMax), new Vector3(finallyZone.xMin, 0f, finallyZone.yMax));
		finallyZone = m_WayPointCenter.m_MineZone;
		Debug.DrawLine(new Vector3(finallyZone.xMin, 0f, finallyZone.yMin), new Vector3(finallyZone.xMin, 0f, finallyZone.yMax));
		Debug.DrawLine(new Vector3(finallyZone.xMin, 0f, finallyZone.yMin), new Vector3(finallyZone.xMax, 0f, finallyZone.yMin));
		Debug.DrawLine(new Vector3(finallyZone.xMax, 0f, finallyZone.yMax), new Vector3(finallyZone.xMax, 0f, finallyZone.yMin));
		Debug.DrawLine(new Vector3(finallyZone.xMax, 0f, finallyZone.yMax), new Vector3(finallyZone.xMin, 0f, finallyZone.yMax));
		m_bIsInit = true;
	}
}
