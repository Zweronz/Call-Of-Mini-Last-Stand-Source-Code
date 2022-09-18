using System.Collections.Generic;
using UnityEngine;

public class TrinitiModelAnimation : MonoBehaviour
{
	protected class AnimationInfo
	{
		public int iFrameCount;

		public int iFrameRate;

		public List<AnimationEvent> listEvent;

		public AnimationInfo()
		{
			iFrameCount = 0;
			iFrameRate = 0;
			listEvent = new List<AnimationEvent>();
		}
	}

	public string[] m_strAnimations;

	public string m_strResPath;

	public Vector3 m_v3Center;

	public string m_sCurAnimName;

	protected TrinitiMeshAnimation[] m_MeshAnimations;

	private int m_iEventPostIndex;

	private float m_fCurPlayTime;

	private WrapMode m_WarpMode;

	private float m_fSpeed;

	private AnimationInfo m_AnimationInfo;

	private List<AnimationEvent> m_listEvent = new List<AnimationEvent>();

	public void Awake()
	{
		m_MeshAnimations = base.gameObject.GetComponentsInChildren<TrinitiMeshAnimation>();
	}

	public void Update()
	{
		if (m_AnimationInfo == null || m_AnimationInfo.listEvent.Count == 0)
		{
			return;
		}
		float num = Time.deltaTime * m_fSpeed;
		float num2 = (float)m_AnimationInfo.iFrameCount * (1f / (float)m_AnimationInfo.iFrameRate);
		m_fCurPlayTime += num;
		if (m_WarpMode == WrapMode.Loop)
		{
			while (m_listEvent.Count != 0)
			{
				AnimationEvent animationEvent = m_listEvent[0];
				if (animationEvent.time > m_fCurPlayTime)
				{
					break;
				}
				if (animationEvent.stringParameter.Length > 0)
				{
					SendMessage(animationEvent.functionName, animationEvent.stringParameter, animationEvent.messageOptions);
				}
				else
				{
					SendMessage(animationEvent.functionName, animationEvent.messageOptions);
				}
				m_listEvent.RemoveAt(0);
			}
			if (m_fCurPlayTime >= num2)
			{
				m_fCurPlayTime = 0f;
				for (int i = 0; i < m_AnimationInfo.listEvent.Count; i++)
				{
					AnimationEvent item = m_AnimationInfo.listEvent[i];
					m_listEvent.Add(item);
				}
			}
		}
		else
		{
			if (m_WarpMode == WrapMode.PingPong || (m_WarpMode != WrapMode.ClampForever && m_WarpMode != WrapMode.Once && m_WarpMode != WrapMode.Once))
			{
				return;
			}
			while (m_listEvent.Count != 0)
			{
				AnimationEvent animationEvent2 = m_listEvent[0];
				if (animationEvent2.time > m_fCurPlayTime)
				{
					break;
				}
				if (animationEvent2.stringParameter.Length > 0)
				{
					SendMessage(animationEvent2.functionName, animationEvent2.stringParameter, animationEvent2.messageOptions);
				}
				else
				{
					SendMessage(animationEvent2.functionName, animationEvent2.messageOptions);
				}
				m_listEvent.RemoveAt(0);
			}
		}
	}

	public void Play(string name, WrapMode mode, float fSpeed = 1f, bool bRandomFrameIndex = false)
	{
		m_sCurAnimName = name;
		m_AnimationInfo = GetAnimationInfo(name);
		m_WarpMode = mode;
		m_fSpeed = fSpeed;
		m_iEventPostIndex = -1;
		int num = 0;
		if (bRandomFrameIndex)
		{
			num = UnityEngine.Random.Range(0, m_AnimationInfo.iFrameCount);
		}
		m_fCurPlayTime = (float)num * (1f / (float)m_AnimationInfo.iFrameRate);
		for (int i = 0; i < m_AnimationInfo.listEvent.Count; i++)
		{
			AnimationEvent animationEvent = m_AnimationInfo.listEvent[i];
			if (animationEvent.time >= m_fCurPlayTime)
			{
				m_listEvent.Add(animationEvent);
			}
		}
		for (int j = 0; j < m_MeshAnimations.Length; j++)
		{
			m_MeshAnimations[j].PlayAnimation(name, m_AnimationInfo.iFrameRate, mode, fSpeed, num);
		}
	}

	protected virtual AnimationInfo GetAnimationInfo(string aniName)
	{
		return null;
	}

	protected GameObject CreateParts(string partName, Material material)
	{
		Transform transform = base.transform.Find(partName);
		if (null == transform)
		{
			GameObject gameObject = new GameObject(partName);
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.AddComponent<TrinitiMeshAnimation>();
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = material;
			return gameObject;
		}
		return transform.gameObject;
	}

	protected TrinitiMeshClip CreatePartAnimation(GameObject partObj, string aniName)
	{
		TrinitiMeshClip trinitiMeshClip = null;
		Transform transform = partObj.transform.Find(aniName);
		if (null == transform)
		{
			GameObject gameObject = new GameObject(aniName);
			gameObject.transform.parent = partObj.transform;
			gameObject.transform.localPosition = Vector3.zero;
			return gameObject.AddComponent<TrinitiMeshClip>();
		}
		return transform.gameObject.GetComponent<TrinitiMeshClip>();
	}
}
