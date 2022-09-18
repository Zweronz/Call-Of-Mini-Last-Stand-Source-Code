using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TAudioAuxFade : MonoBehaviour
{
	public delegate void OnFadeOutDegelate();

	public int fadeInTime;

	public int fadeOutTime;

	public bool autoFadeout;

	private void OnAudioTrigger(AudioClip clip)
	{
		StartCoroutine(FadeIn());
		if (autoFadeout && fadeOutTime > 0)
		{
			StartCoroutine(AutoFadeOut(clip));
		}
	}

	private IEnumerator AutoFadeOut(AudioClip clip)
	{
		float len = clip.length;
		float time = (float)fadeOutTime * 0.001f;
		if (len > time)
		{
			yield return new WaitForSeconds(len - time);
		}
		StartCoroutine(FadeOut(null));
	}

	public IEnumerator FadeIn()
	{
		if (fadeInTime <= 0)
		{
			yield break;
		}
		float volumOri = base.GetComponent<AudioSource>().volume;
		float volumSpd = volumOri / ((float)fadeInTime * 0.001f);
		base.GetComponent<AudioSource>().volume = 0f;
		while (true)
		{
			float volum2 = base.GetComponent<AudioSource>().volume;
			volum2 += volumSpd * Time.deltaTime;
			if (volum2 > volumOri)
			{
				break;
			}
			base.GetComponent<AudioSource>().volume = volum2;
			yield return 0;
		}
		base.GetComponent<AudioSource>().volume = volumOri;
	}

	public IEnumerator FadeOut(OnFadeOutDegelate onFadeOutDegelate)
	{
		if (fadeOutTime <= 0)
		{
			yield break;
		}
		float volumOri = base.GetComponent<AudioSource>().volume;
		float volumSpd = volumOri / ((float)fadeOutTime * 0.001f);
		while (true)
		{
			float volum2 = base.GetComponent<AudioSource>().volume;
			volum2 -= volumSpd * Time.deltaTime;
			if (volum2 < 0f)
			{
				break;
			}
			base.GetComponent<AudioSource>().volume = volum2;
			yield return 0;
		}
		base.GetComponent<AudioSource>().volume = 0f;
		TAudioManager.instance.StopSound(base.GetComponent<AudioSource>());
		if (onFadeOutDegelate != null)
		{
			onFadeOutDegelate();
		}
	}
}
