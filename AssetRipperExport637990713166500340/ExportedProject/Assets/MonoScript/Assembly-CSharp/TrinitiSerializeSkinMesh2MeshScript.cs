using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class TrinitiSerializeSkinMesh2MeshScript : MonoBehaviour
{
	[Serializable]
	public class Item
	{
		public string name = string.Empty;

		public int iRate = 24;
	}

	private class MeshPoints
	{
		public Vector3[] m_vPoints;
	}

	private class MeshInfo
	{
		public Vector2[] m_uv;

		public int[] m_triangles;

		public List<MeshPoints> m_frames;
	}

	private class AnimationInfo
	{
		public int m_iFrameCount;

		public int m_iFrameRate;

		public Dictionary<string, MeshInfo> m_mapMesh;
	}

	public Item[] m_RateList;

	private int m_iFrameRate = 30;

	private string m_strDocumentDir = string.Empty;

	public void RefreshAnimationList()
	{
		m_RateList = new Item[base.GetComponent<Animation>().GetClipCount()];
		int num = 0;
		IEnumerator enumerator = base.GetComponent<Animation>().GetEnumerator();
		while (enumerator.MoveNext())
		{
			AnimationState animationState = (AnimationState)enumerator.Current;
			Item item = new Item();
			item.name = animationState.name;
			m_RateList[num] = item;
			num++;
		}
	}

	public void SaveAnimationList()
	{
		for (int i = 0; i < m_RateList.Length; i++)
		{
			Item item = m_RateList[i];
			m_iFrameRate = item.iRate;
			SaveAnimation(item.name);
		}
	}

	private void SaveAnimation(string aniName)
	{
		base.GetComponent<Animation>()[aniName].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>().Play(aniName);
		float num = 1f / (float)m_iFrameRate;
		int i;
		for (i = 0; (float)i * num <= base.GetComponent<Animation>()[aniName].length; i++)
		{
		}
		AnimationInfo animationInfo = new AnimationInfo();
		animationInfo.m_iFrameCount = i;
		animationInfo.m_iFrameRate = m_iFrameRate;
		animationInfo.m_mapMesh = new Dictionary<string, MeshInfo>();
		SkinnedMeshRenderer[] componentsInChildren = GetComponentsInChildren<SkinnedMeshRenderer>();
		SkinnedMeshRenderer[] array = componentsInChildren;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array)
		{
			Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
			MeshInfo meshInfo = new MeshInfo();
			animationInfo.m_mapMesh.Add(sharedMesh.name, meshInfo);
			meshInfo.m_uv = sharedMesh.uv;
			meshInfo.m_triangles = sharedMesh.triangles;
			meshInfo.m_frames = new List<MeshPoints>();
		}
		List<Vector3> list = new List<Vector3>();
		for (int k = 0; k < i; k++)
		{
			base.GetComponent<Animation>()[aniName].enabled = true;
			base.GetComponent<Animation>()[aniName].time = (float)k * num;
			base.GetComponent<Animation>().Sample();
			base.GetComponent<Animation>()[aniName].enabled = false;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			SkinnedMeshRenderer[] array2 = componentsInChildren;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in array2)
			{
				Mesh sharedMesh2 = skinnedMeshRenderer2.sharedMesh;
				Vector3[] array3 = new Vector3[sharedMesh2.vertices.Length];
				for (int m = 0; m < sharedMesh2.vertices.Length; m++)
				{
					Vector3 v = sharedMesh2.vertices[m];
					BoneWeight boneWeight = sharedMesh2.boneWeights[m];
					Transform[] bones = skinnedMeshRenderer2.bones;
					Transform transform = bones[boneWeight.boneIndex0];
					Transform transform2 = bones[boneWeight.boneIndex1];
					Transform transform3 = bones[boneWeight.boneIndex2];
					Transform transform4 = bones[boneWeight.boneIndex3];
					Matrix4x4 matrix4x = transform.localToWorldMatrix * sharedMesh2.bindposes[boneWeight.boneIndex0];
					Matrix4x4 matrix4x2 = transform2.localToWorldMatrix * sharedMesh2.bindposes[boneWeight.boneIndex1];
					Matrix4x4 matrix4x3 = transform3.localToWorldMatrix * sharedMesh2.bindposes[boneWeight.boneIndex2];
					Matrix4x4 matrix4x4 = transform4.localToWorldMatrix * sharedMesh2.bindposes[boneWeight.boneIndex3];
					Vector3 vector3 = matrix4x.MultiplyPoint(v) * boneWeight.weight0 + matrix4x2.MultiplyPoint(v) * boneWeight.weight1 + matrix4x3.MultiplyPoint(v) * boneWeight.weight2 + matrix4x4.MultiplyPoint(v) * boneWeight.weight3;
					array3[m] = vector3;
					if (vector2 == Vector3.zero && vector == Vector3.zero)
					{
						vector2 = array3[m];
						vector = array3[m];
						continue;
					}
					if (array3[m].x < vector2.x)
					{
						vector2.x = array3[m].x;
					}
					else if (array3[m].x > vector.x)
					{
						vector.x = array3[m].x;
					}
					if (array3[m].y < vector2.y)
					{
						vector2.y = array3[m].y;
					}
					else if (array3[m].y > vector.y)
					{
						vector.y = array3[m].y;
					}
					if (array3[m].z < vector2.z)
					{
						vector2.z = array3[m].z;
					}
					else if (array3[m].z > vector.z)
					{
						vector.z = array3[m].z;
					}
				}
				MeshPoints meshPoints = new MeshPoints();
				meshPoints.m_vPoints = array3;
				MeshInfo meshInfo2 = animationInfo.m_mapMesh[sharedMesh2.name];
				meshInfo2.m_frames.Add(meshPoints);
			}
			Vector3 item = vector2 + (vector - vector2) / 2f;
			list.Add(item);
		}
		string path = GetDocumentPath() + "/" + aniName + ".bytes";
		FileStream fileStream = File.Open(path, FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Write(animationInfo.m_iFrameCount);
		binaryWriter.Write(animationInfo.m_iFrameRate);
		binaryWriter.Write(animationInfo.m_mapMesh.Count);
		foreach (KeyValuePair<string, MeshInfo> item2 in animationInfo.m_mapMesh)
		{
			string key = item2.Key;
			MeshInfo value = item2.Value;
			binaryWriter.Write(key);
			binaryWriter.Write(value.m_triangles.Length);
			for (int n = 0; n < value.m_triangles.Length; n++)
			{
				binaryWriter.Write(value.m_triangles[n]);
			}
			binaryWriter.Write(value.m_uv.Length);
			for (int num2 = 0; num2 < value.m_uv.Length; num2++)
			{
				binaryWriter.Write(value.m_uv[num2].x);
				binaryWriter.Write(value.m_uv[num2].y);
			}
			foreach (MeshPoints frame in value.m_frames)
			{
				for (int num3 = 0; num3 < frame.m_vPoints.Length; num3++)
				{
					Vector3 vector4 = frame.m_vPoints[num3];
					binaryWriter.Write(vector4.x);
					binaryWriter.Write(vector4.y);
					binaryWriter.Write(vector4.z);
				}
			}
		}
		foreach (Vector3 item3 in list)
		{
			binaryWriter.Write(item3.x);
			binaryWriter.Write(item3.y);
			binaryWriter.Write(item3.z);
		}
		binaryWriter.Close();
		fileStream.Close();
	}

	private string GetDocumentPath()
	{
		if (m_strDocumentDir.Length > 0)
		{
			return m_strDocumentDir;
		}
		string dataPath = Application.dataPath;
		dataPath += "/../Documents/Animation";
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		m_strDocumentDir = dataPath;
		return dataPath;
	}
}
