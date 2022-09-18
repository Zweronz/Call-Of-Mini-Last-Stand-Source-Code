using UnityEngine;

public class ModelAnimation : MonoBehaviour
{
	public MeshAnimation[] m_MeshAnimations;

	private void Awake()
	{
		m_MeshAnimations = base.gameObject.GetComponentsInChildren<MeshAnimation>();
	}

	private void Start()
	{
	}

	public void Play(string name, int rate, WrapMode mode, bool bRandomFrameIndex)
	{
		int iFrameIndex = 0;
		if (bRandomFrameIndex)
		{
			iFrameIndex = UnityEngine.Random.Range(0, AnimationFrameCounts(name));
		}
		for (int i = 0; i < m_MeshAnimations.Length; i++)
		{
			m_MeshAnimations[i].PlayAnimation(name, rate, mode, iFrameIndex);
		}
	}

	public int AnimationFrameCounts(string name)
	{
		if (m_MeshAnimations == null || m_MeshAnimations.Length <= 0)
		{
			return 0;
		}
		return m_MeshAnimations[0].AnimationFrameCounts(name);
	}
}
