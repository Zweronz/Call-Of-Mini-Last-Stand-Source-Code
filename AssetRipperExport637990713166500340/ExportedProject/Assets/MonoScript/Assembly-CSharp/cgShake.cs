using UnityEngine;

public class cgShake : cgBase
{
	public override void Enter()
	{
		iZombieSniperCamera component = Camera.main.GetComponent<iZombieSniperCamera>();
		if (component != null)
		{
			component.Shake(0.4f, 0.1f, true, true);
		}
	}
}
