using System.Collections.Generic;
using UnityEngine;

public class TAudioManager
{
	private static TAudioManager s_instance;

	private List<AudioSource> m_loopSounds = new List<AudioSource>();

	private List<AudioSource> m_loopMusics = new List<AudioSource>();

	private List<ITAudioEvent> m_loopSoundEvts = new List<ITAudioEvent>();

	private List<ITAudioEvent> m_loopMusicEvts = new List<ITAudioEvent>();

	private bool m_isMusicOn = true;

	private bool m_isSoundOn = true;

	private AudioListener audioListener;

	public static TAudioManager instance
	{
		get
		{
			if (s_instance == null)
			{
				s_instance = new TAudioManager();
			}
			return s_instance;
		}
	}

	public bool isMusicOn
	{
		get
		{
			return m_isMusicOn;
		}
		set
		{
			m_isMusicOn = value;
			if (m_isMusicOn)
			{
				foreach (AudioSource loopMusic in m_loopMusics)
				{
					loopMusic.Play();
				}
				foreach (ITAudioEvent loopMusicEvt in m_loopMusicEvts)
				{
					loopMusicEvt.Trigger();
				}
			}
			else
			{
				foreach (AudioSource loopMusic2 in m_loopMusics)
				{
					loopMusic2.Stop();
				}
				foreach (ITAudioEvent loopMusicEvt2 in m_loopMusicEvts)
				{
					loopMusicEvt2.Stop();
				}
			}
			PlayerPrefs.SetInt("MusicOff", (!m_isMusicOn) ? 1 : 0);
		}
	}

	public bool isSoundOn
	{
		get
		{
			return m_isSoundOn;
		}
		set
		{
			m_isSoundOn = value;
			if (m_isSoundOn)
			{
				foreach (AudioSource loopSound in m_loopSounds)
				{
					loopSound.Play();
				}
				foreach (ITAudioEvent loopSoundEvt in m_loopSoundEvts)
				{
					loopSoundEvt.Trigger();
				}
			}
			else
			{
				foreach (AudioSource loopSound2 in m_loopSounds)
				{
					loopSound2.Stop();
				}
				foreach (ITAudioEvent loopSoundEvt2 in m_loopSoundEvts)
				{
					loopSoundEvt2.Stop();
				}
			}
			PlayerPrefs.SetInt("SoundOff", (!m_isSoundOn) ? 1 : 0);
		}
	}

	public AudioListener AudioListener
	{
		get
		{
			return audioListener;
		}
	}

	public TAudioManager()
	{
		m_isMusicOn = PlayerPrefs.GetInt("MusicOff") == 0;
		m_isSoundOn = PlayerPrefs.GetInt("SoundOff") == 0;
	}

	public void Pause(AudioSource audio)
	{
		audio.Pause();
	}

	public void PlaySound(AudioSource audio)
	{
		if (audio.loop && !m_loopSounds.Contains(audio))
		{
			m_loopSounds.Add(audio);
		}
		if (m_isSoundOn)
		{
			audio.Play();
		}
	}

	public AudioSource PlaySound(AudioClip clip, bool loop)
	{
		GameObject gameObject = new GameObject(clip.name + "_SFX", typeof(AudioSource));
		gameObject.GetComponent<AudioSource>().loop = loop;
		gameObject.GetComponent<AudioSource>().clip = clip;
		gameObject.GetComponent<AudioSource>().playOnAwake = false;
		if (loop)
		{
			m_loopSounds.Add(gameObject.GetComponent<AudioSource>());
		}
		if (m_isSoundOn)
		{
			gameObject.GetComponent<AudioSource>().Play();
		}
		return gameObject.GetComponent<AudioSource>();
	}

	public void PlaySound(AudioSource audio, AudioClip clip, bool loop, bool cutoff)
	{
		if (loop)
		{
			if (!m_loopSounds.Contains(audio))
			{
				m_loopSounds.Add(audio);
			}
			audio.loop = true;
			audio.clip = clip;
			if (m_isSoundOn)
			{
				audio.Play();
			}
			return;
		}
		if (m_loopSounds.Contains(audio))
		{
			m_loopSounds.Remove(audio);
		}
		audio.loop = false;
		if (m_isSoundOn)
		{
			if (cutoff)
			{
				audio.clip = clip;
				audio.Play();
			}
			else
			{
				audio.PlayOneShot(clip);
			}
		}
	}

	public void PlaySound(AudioSource audio, AudioClip clip, bool loop)
	{
		PlaySound(audio, clip, loop, false);
	}

	public void StopSound(AudioSource audio)
	{
		audio.Stop();
		if (m_loopSounds.Contains(audio))
		{
			m_loopSounds.Remove(audio);
		}
	}

	public void PlayMusic(AudioSource audio)
	{
		if (audio.loop && !m_loopMusics.Contains(audio))
		{
			m_loopMusics.Add(audio);
		}
		if (m_isMusicOn)
		{
			audio.Play();
		}
	}

	public AudioSource PlayMusic(AudioClip clip, bool loop)
	{
		GameObject gameObject = new GameObject(clip.name + "_BGM", typeof(AudioSource));
		gameObject.GetComponent<AudioSource>().loop = loop;
		gameObject.GetComponent<AudioSource>().clip = clip;
		gameObject.GetComponent<AudioSource>().playOnAwake = false;
		if (loop)
		{
			m_loopMusics.Add(gameObject.GetComponent<AudioSource>());
		}
		if (m_isMusicOn)
		{
			gameObject.GetComponent<AudioSource>().Play();
		}
		return gameObject.GetComponent<AudioSource>();
	}

	public void PlayMusic(AudioSource audio, AudioClip clip, bool loop)
	{
		if (loop)
		{
			audio.loop = true;
			audio.clip = clip;
			if (!m_loopMusics.Contains(audio))
			{
				m_loopMusics.Add(audio);
			}
			if (m_isMusicOn)
			{
				audio.Play();
			}
		}
		else
		{
			audio.loop = false;
			if (m_loopMusics.Contains(audio))
			{
				m_loopMusics.Remove(audio);
			}
			if (m_isMusicOn)
			{
				audio.PlayOneShot(clip);
			}
		}
	}

	public void StopMusic(AudioSource audio)
	{
		audio.Stop();
		if (m_loopMusics.Contains(audio))
		{
			m_loopMusics.Remove(audio);
		}
	}

	public void ClearLoop()
	{
		m_loopMusics.Clear();
		m_loopSounds.Clear();
		m_loopSoundEvts.Clear();
	}

	public void AddLoopSoundEvt(ITAudioEvent evt)
	{
		if (m_loopSoundEvts.Contains(evt))
		{
			m_loopSoundEvts.Add(evt);
		}
	}

	public void AddLoopMusicEvt(ITAudioEvent evt)
	{
		if (m_loopMusicEvts.Contains(evt))
		{
			m_loopMusicEvts.Add(evt);
		}
	}
}
