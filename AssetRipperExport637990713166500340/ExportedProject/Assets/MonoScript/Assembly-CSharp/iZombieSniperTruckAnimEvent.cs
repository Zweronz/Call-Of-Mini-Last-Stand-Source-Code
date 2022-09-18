using UnityEngine;

public class iZombieSniperTruckAnimEvent : MonoBehaviour
{
	public GameObject friction;

	public GameObject hit;

	public GameObject TailSmoke1;

	public GameObject TailSmoke2;

	public void TruckAnumStart()
	{
		StopFriction();
		StopTailSmoke1();
		PlayTailSmoke2();
	}

	public void PlayFriction()
	{
		friction.SetActiveRecursively(true);
	}

	public void StopFriction()
	{
		friction.SetActiveRecursively(false);
	}

	public void PlayHit()
	{
		GameObject original = (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/hit");
		hit = (GameObject)Object.Instantiate(original);
		hit.transform.parent = friction.transform.parent;
		hit.transform.localPosition = Vector3.zero;
		hit.transform.localEulerAngles = Vector3.zero;
		Object.Destroy(hit, 0.25f);
	}

	public void PlayTailSmoke1()
	{
		TailSmoke1.SetActiveRecursively(true);
	}

	public void StopTailSmoke1()
	{
		TailSmoke1.SetActiveRecursively(false);
	}

	public void PlayTailSmoke2()
	{
		TailSmoke2.SetActiveRecursively(true);
	}

	public void StopTailSmoke2()
	{
		TailSmoke2.SetActiveRecursively(false);
	}
}
