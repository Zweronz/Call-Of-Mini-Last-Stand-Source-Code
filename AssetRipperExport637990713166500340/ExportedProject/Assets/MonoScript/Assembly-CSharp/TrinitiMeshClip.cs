using System.Collections.Generic;
using UnityEngine;

public class TrinitiMeshClip : MonoBehaviour
{
	public List<Mesh> m_MeshFrames;

	public int GetFrameCount()
	{
		return m_MeshFrames.Count;
	}
}
