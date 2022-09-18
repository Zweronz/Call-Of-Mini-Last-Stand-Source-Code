using System;
using UnityEngine;

public class TAudioEffectTogether : MonoBehaviour, ITAudioEvent
{
	private struct DelayAndIndex
	{
		public int index;

		public float deltaTime;

		public DelayAndIndex(int index, float deltaTime)
		{
			this.index = index;
			this.deltaTime = deltaTime;
		}
	}

	public GameObject[] audioEffectRefs;

	public float maxDelayTime;

	public bool useDelayPlay;

	[HideInInspector]
	public float[] delayTimeArray;

	private ITAudioEvent[] audioEvts;

	private ITAudioLimit[] audioLimits;

	private float playTime;

	private bool isPlay;

	private int lastPlayIndex = -1;

	private DelayAndIndex[] delayAndIndex;

	private void Awake()
	{
		Component[] components = GetComponents(typeof(TAudioLimitTimeAndCount));
		audioLimits = new ITAudioLimit[components.Length];
		for (int i = 0; i < audioLimits.Length; i++)
		{
			audioLimits[i] = (ITAudioLimit)components[i];
		}
		audioEvts = new ITAudioEvent[audioEffectRefs.Length];
		for (int j = 0; j < audioEffectRefs.Length; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(audioEffectRefs[j]) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			audioEvts[j] = (ITAudioEvent)gameObject.GetComponent(typeof(ITAudioEvent));
		}
		delayAndIndex = new DelayAndIndex[delayTimeArray.Length];
		for (int k = 0; k < delayAndIndex.Length; k++)
		{
			delayAndIndex[k] = new DelayAndIndex(k, delayTimeArray[k]);
		}
		Array.Sort(delayAndIndex, Compare);
	}

	private void Update()
	{
		if (!isPlay)
		{
			return;
		}
		playTime += Time.deltaTime;
		for (int i = lastPlayIndex + 1; i < delayAndIndex.Length; i++)
		{
			float num = delayAndIndex[i].deltaTime * maxDelayTime * 0.001f;
			if (playTime > num)
			{
				audioEvts[delayAndIndex[i].index].Trigger();
				lastPlayIndex = i;
			}
		}
		if (playTime > maxDelayTime * 0.001f)
		{
			isPlay = false;
		}
	}

	public void Trigger()
	{
		if (audioEvts.Length == 0)
		{
			return;
		}
		ITAudioLimit[] array = audioLimits;
		foreach (ITAudioLimit iTAudioLimit in array)
		{
			if (!iTAudioLimit.isCanPlay)
			{
				return;
			}
		}
		if (useDelayPlay)
		{
			playTime = 0f;
			isPlay = true;
			lastPlayIndex = -1;
		}
		else
		{
			ITAudioEvent[] array2 = audioEvts;
			foreach (ITAudioEvent iTAudioEvent in array2)
			{
				iTAudioEvent.Trigger();
			}
		}
		SendMessage("OnAudioTrigger", SendMessageOptions.DontRequireReceiver);
	}

	public void Stop()
	{
		ITAudioEvent[] array = audioEvts;
		foreach (ITAudioEvent iTAudioEvent in array)
		{
			iTAudioEvent.Stop();
		}
	}

	private static int Compare(DelayAndIndex l, DelayAndIndex r)
	{
		if (l.deltaTime < r.deltaTime)
		{
			return -1;
		}
		if (l.deltaTime > r.deltaTime)
		{
			return 1;
		}
		return 0;
	}
}
