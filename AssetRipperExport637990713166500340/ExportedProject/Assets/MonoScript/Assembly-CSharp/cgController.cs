using UnityEngine;

public class cgController : MonoBehaviour
{
	public bool isPlay;

	private void Awake()
	{
		if (isPlay)
		{
			Play(1f);
		}
	}

	private void Update()
	{
	}

	public void Play(float fTimeScale)
	{
		isPlay = true;
		cgTimer[] componentsInChildren = base.transform.GetComponentsInChildren<cgTimer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].isPlay = true;
			componentsInChildren[i].TimeScale = fTimeScale;
		}
	}
}
