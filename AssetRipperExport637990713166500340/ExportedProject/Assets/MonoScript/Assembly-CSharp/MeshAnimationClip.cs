using UnityEngine;

public class MeshAnimationClip : MonoBehaviour
{
	public Mesh[] m_MeshFrames;

	public Vector3[] m_MeshBoxCenter;

	public Vector3[] m_MeshBoxSize;

	public int GetFrameCount()
	{
		return m_MeshFrames.Length;
	}
}
