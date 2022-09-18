using UnityEngine;

public class iZombieSniperAnimEvent : MonoBehaviour
{
	public GameObject m_Clip;

	public Transform m_BoneClip;

	public Transform m_trClip;

	public Transform m_trRightHand;

	private void Start()
	{
		Transform transform = base.transform.Find("Clip");
		if (!(transform == null))
		{
			m_Clip = transform.gameObject;
		}
	}

	private void Update()
	{
	}

	public void HideClip()
	{
		m_Clip.active = false;
	}

	public void ShowClip()
	{
		m_Clip.active = true;
	}

	public void TakeOnClip()
	{
	}
}
