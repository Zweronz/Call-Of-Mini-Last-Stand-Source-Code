using UnityEngine;

public class TAudioController : MonoBehaviour
{
	public void PlayAudio(string objName)
	{
		Transform transform = base.transform.Find("Audio");
		if (null == transform)
		{
			GameObject gameObject = new GameObject("Audio");
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			transform = gameObject.transform;
		}
		int num = objName.LastIndexOf('/');
		num++;
		string text = objName.Substring(num);
		GameObject gameObject2 = null;
		Transform transform2 = base.transform.Find("Audio/" + text);
		if (null == transform2)
		{
			gameObject2 = Resources.Load("SoundEvent/" + objName) as GameObject;
			if (null == gameObject2)
			{
				Debug.LogWarning(objName + " is null");
				return;
			}
			gameObject2 = Object.Instantiate(gameObject2) as GameObject;
			if (null == gameObject2)
			{
				Debug.LogWarning(objName + " is null");
				return;
			}
			gameObject2.name = text;
			gameObject2.transform.parent = transform;
			gameObject2.transform.localPosition = Vector3.zero;
		}
		else
		{
			gameObject2 = transform2.gameObject;
		}
		ITAudioEvent iTAudioEvent = (ITAudioEvent)gameObject2.GetComponent(typeof(ITAudioEvent));
		if (iTAudioEvent != null)
		{
			iTAudioEvent.Trigger();
		}
	}

	public void StopAudio(string objName)
	{
		Transform transform = base.transform.Find("Audio/" + objName);
		if (transform != null)
		{
			ITAudioEvent iTAudioEvent = (ITAudioEvent)transform.GetComponent(typeof(ITAudioEvent));
			if (iTAudioEvent != null)
			{
				iTAudioEvent.Stop();
			}
		}
	}
}
