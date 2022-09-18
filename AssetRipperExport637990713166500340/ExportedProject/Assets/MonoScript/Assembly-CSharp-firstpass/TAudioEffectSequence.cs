using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TAudioEffectSequence : MonoBehaviour, ITAudioEvent
{
	public bool isSfx = true;

	public AudioClip[] audioClips;

	public float deltaTime = 0.2f;

	private ITAudioLimit[] m_audioLimits;

	private int m_lastPlayIndex = -1;

	private float m_triggerTime;

	private bool m_isTimeout = true;

	private void Awake()
	{
		Component[] components = GetComponents(typeof(ITAudioLimit));
		m_audioLimits = new ITAudioLimit[components.Length];
		for (int i = 0; i < m_audioLimits.Length; i++)
		{
			m_audioLimits[i] = (ITAudioLimit)components[i];
		}
	}

	private void Update()
	{
		if (!m_isTimeout)
		{
			m_triggerTime += Time.deltaTime;
			if (m_triggerTime > deltaTime)
			{
				m_isTimeout = true;
			}
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
		if (deltaTime < 0f)
		{
			m_lastPlayIndex = Mathf.Min(m_lastPlayIndex + 1, audioClips.Length - 1);
		}
		else
		{
			m_lastPlayIndex = ((!m_isTimeout) ? Mathf.Min(m_lastPlayIndex + 1, audioClips.Length - 1) : 0);
			m_isTimeout = false;
			m_triggerTime = 0f;
		}
		AudioClip audioClip = audioClips[m_lastPlayIndex];
		if (null != audioClip)
		{
			if (isSfx)
			{
				TAudioManager.instance.PlaySound(base.GetComponent<AudioSource>(), audioClip, base.GetComponent<AudioSource>().loop);
			}
			else
			{
				TAudioManager.instance.PlayMusic(base.GetComponent<AudioSource>(), audioClip, base.GetComponent<AudioSource>().loop);
			}
			SendMessage("OnAudioTrigger", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void Stop()
	{
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
