using UnityEngine;

public class cgSound : cgBase
{
	public string sound = string.Empty;

	public bool loop;

	protected TAudioController m_AudioController;

	public override void Init()
	{
		GameObject gameObject = GameObject.Find("soundcontroller");
		if (gameObject != null)
		{
			m_AudioController = gameObject.GetComponent<TAudioController>();
		}
	}

	public override void Enter()
	{
		if (sound.Length >= 1)
		{
			m_AudioController.PlayAudio(sound);
		}
	}

	public override void Exit()
	{
		if (sound.Length >= 1 && loop)
		{
			m_AudioController.StopAudio(sound);
		}
	}
}
