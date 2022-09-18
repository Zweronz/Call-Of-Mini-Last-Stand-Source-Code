using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TAudioEffectRandom : MonoBehaviour, ITAudioEvent
{
	public enum LoopMode
	{
		Default = 0,
		SingleLoop = 1,
		MultiLoop = 2
	}

	public bool isSfx = true;

	public AudioClip[] audioClips;

	public float[] probability;

	public float volumOffset;

	public float pitchOffset;

	public LoopMode loopMode;

	public bool cutoff;

	private ITAudioLimit[] m_audioLimits;

	private int m_lastRandomIndex = -1;

	private float m_volumBase;

	private float m_pitchBase;

	private float nullProbability = -1f;

	private void Awake()
	{
		Component[] components = GetComponents(typeof(TAudioLimitTimeAndCount));
		m_audioLimits = new ITAudioLimit[components.Length];
		for (int i = 0; i < m_audioLimits.Length; i++)
		{
			m_audioLimits[i] = (ITAudioLimit)components[i];
		}
		m_volumBase = base.GetComponent<AudioSource>().volume;
		m_pitchBase = base.GetComponent<AudioSource>().pitch;
		if (probability.Length > 0)
		{
			float num = 0f;
			float[] array = probability;
			foreach (float num2 in array)
			{
				num += num2;
			}
			if (num < 0.999f)
			{
				nullProbability = 1f - num;
			}
		}
		if (audioClips.Length == 1 && loopMode == LoopMode.MultiLoop)
		{
			loopMode = LoopMode.Default;
		}
	}

	public void Trigger()
	{
		if (audioClips.Length == 0)
		{
			return;
		}
		ITAudioLimit[] audioLimits = m_audioLimits;
		foreach (ITAudioLimit iTAudioLimit in audioLimits)
		{
			if (!iTAudioLimit.isCanPlay)
			{
				return;
			}
		}
		if (probability.Length == 0)
		{
			if (audioClips.Length == 1)
			{
				m_lastRandomIndex = 0;
			}
			else
			{
				int num = UnityEngine.Random.Range(0, 1000);
				if (m_lastRandomIndex == -1)
				{
					num %= audioClips.Length;
					m_lastRandomIndex = num;
				}
				else
				{
					num %= audioClips.Length - 1;
					m_lastRandomIndex = (m_lastRandomIndex + num + 1) % audioClips.Length;
				}
			}
		}
		else
		{
			float num2 = 0f;
			for (int j = 0; j < probability.Length; j++)
			{
				if (j != m_lastRandomIndex)
				{
					num2 += probability[j];
				}
			}
			if (nullProbability > 0.001f)
			{
				num2 += nullProbability;
			}
			float num3 = UnityEngine.Random.Range(0f, num2);
			if (nullProbability > 0.001f)
			{
				num2 -= nullProbability;
				if (num3 > num2)
				{
					m_lastRandomIndex = -1;
					return;
				}
			}
			for (int num4 = probability.Length - 1; num4 >= 0; num4--)
			{
				if (num4 != m_lastRandomIndex)
				{
					num2 -= probability[num4];
					if (num3 > num2)
					{
						m_lastRandomIndex = num4;
						break;
					}
				}
			}
		}
		AudioClip audioClip = audioClips[m_lastRandomIndex];
		if (!(null != audioClip))
		{
			return;
		}
		base.GetComponent<AudioSource>().volume = Mathf.Clamp01(UnityEngine.Random.Range(m_volumBase - volumOffset, m_volumBase + volumOffset));
		base.GetComponent<AudioSource>().pitch = Mathf.Clamp(UnityEngine.Random.Range(m_pitchBase / (1f + pitchOffset), m_pitchBase * (1f + pitchOffset)), -3f, 3f);
		if (loopMode == LoopMode.Default)
		{
			if (isSfx)
			{
				TAudioManager.instance.PlaySound(base.GetComponent<AudioSource>(), audioClip, base.GetComponent<AudioSource>().loop, cutoff);
			}
			else
			{
				TAudioManager.instance.PlayMusic(base.GetComponent<AudioSource>(), audioClip, base.GetComponent<AudioSource>().loop);
			}
			SendMessage("OnAudioTrigger", audioClip, SendMessageOptions.DontRequireReceiver);
		}
		else if (loopMode == LoopMode.MultiLoop)
		{
			if (!base.GetComponent<AudioSource>().isPlaying)
			{
				SendMessage("OnAudioTrigger", audioClip, SendMessageOptions.DontRequireReceiver);
				if (isSfx)
				{
					TAudioManager.instance.AddLoopSoundEvt(this);
				}
				else
				{
					TAudioManager.instance.AddLoopMusicEvt(this);
				}
			}
			if (isSfx)
			{
				TAudioManager.instance.PlaySound(base.GetComponent<AudioSource>(), audioClip, false, cutoff);
			}
			else
			{
				TAudioManager.instance.PlayMusic(base.GetComponent<AudioSource>(), audioClip, false);
			}
			Invoke("Trigger", audioClip.length - Time.deltaTime);
		}
		else if (loopMode == LoopMode.SingleLoop)
		{
			if (isSfx)
			{
				TAudioManager.instance.PlaySound(base.GetComponent<AudioSource>(), audioClip, true);
			}
			else
			{
				TAudioManager.instance.PlayMusic(base.GetComponent<AudioSource>(), audioClip, true);
			}
			SendMessage("OnAudioTrigger", audioClip, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void Stop()
	{
		if (loopMode == LoopMode.MultiLoop)
		{
			CancelInvoke("Trigger");
		}
		if (isSfx)
		{
			TAudioManager.instance.StopSound(base.GetComponent<AudioSource>());
		}
		else
		{
			TAudioManager.instance.StopMusic(base.GetComponent<AudioSource>());
		}
	}
}
