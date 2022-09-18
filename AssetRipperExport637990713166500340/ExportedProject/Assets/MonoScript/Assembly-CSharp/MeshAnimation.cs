using UnityEngine;

public class MeshAnimation : MonoBehaviour
{
	private MeshAnimationClip m_AnimaitonClip;

	private WrapMode m_WarpMode;

	private float m_fStartTime;

	private float m_fNextFrameTime;

	private int m_iCurrentFrame;

	private bool m_bIncrease;

	private int m_iFrameRate;

	private MeshFilter m_MeshFilter;

	private MeshCollider m_MeshCollider;

	private BoxCollider m_BoxCollider;

	public int AnimationFrameCounts(string name)
	{
		GameObject gameObject = base.gameObject.transform.Find("Animation/" + name).gameObject;
		MeshAnimationClip component = gameObject.GetComponent<MeshAnimationClip>();
		return component.GetFrameCount();
	}

	public void PlayAnimation(string name, int rate, WrapMode mode, int iFrameIndex)
	{
		Transform transform = base.gameObject.transform.Find("Animation/" + name);
		if (base.transform == null)
		{
			Debug.Log(name);
			return;
		}
		GameObject gameObject = transform.gameObject;
		MeshAnimationClip component = gameObject.GetComponent<MeshAnimationClip>();
		PlayAnimation(component, rate, mode, iFrameIndex);
	}

	public bool IsPlaying()
	{
		return m_AnimaitonClip != null;
	}

	private void PlayAnimation(MeshAnimationClip clip, int frameRate, WrapMode mode, int iFrameIndex)
	{
		m_AnimaitonClip = clip;
		m_WarpMode = mode;
		m_iFrameRate = frameRate;
		m_fStartTime = 0f;
		m_fNextFrameTime = 0f;
		m_bIncrease = true;
		m_iCurrentFrame = iFrameIndex;
		m_MeshFilter.mesh = m_AnimaitonClip.m_MeshFrames[m_iCurrentFrame];
	}

	private void Awake()
	{
		m_MeshFilter = base.gameObject.GetComponent<MeshFilter>();
		m_MeshCollider = base.gameObject.GetComponent<MeshCollider>();
		m_BoxCollider = base.gameObject.GetComponent<BoxCollider>();
		if ((bool)m_MeshCollider)
		{
			m_MeshCollider.isTrigger = true;
		}
		if ((bool)m_BoxCollider)
		{
			m_BoxCollider.isTrigger = true;
		}
	}

	private void Update()
	{
		if (null == m_AnimaitonClip)
		{
			return;
		}
		if (m_WarpMode == WrapMode.Loop)
		{
			m_fStartTime += Time.deltaTime;
			m_fNextFrameTime += Time.deltaTime;
			if (!(m_fNextFrameTime < 1f / (float)m_iFrameRate))
			{
				m_fNextFrameTime = 0f;
				m_iCurrentFrame++;
				if (m_iCurrentFrame >= m_AnimaitonClip.GetFrameCount())
				{
					m_iCurrentFrame = 0;
				}
				SetCurrentFrame();
			}
		}
		else if (m_WarpMode == WrapMode.PingPong)
		{
			m_fStartTime += Time.deltaTime;
			m_fNextFrameTime += Time.deltaTime;
			if (m_fNextFrameTime < 1f / (float)m_iFrameRate)
			{
				return;
			}
			m_fNextFrameTime = 0f;
			if (m_bIncrease)
			{
				m_iCurrentFrame++;
				if (m_iCurrentFrame == m_AnimaitonClip.GetFrameCount() - 1)
				{
					m_bIncrease = false;
				}
			}
			else
			{
				m_iCurrentFrame--;
				if (m_iCurrentFrame == 0)
				{
					m_bIncrease = true;
				}
			}
			if (m_iCurrentFrame >= m_AnimaitonClip.GetFrameCount())
			{
				m_iCurrentFrame = 0;
			}
			SetCurrentFrame();
		}
		else if (m_WarpMode == WrapMode.ClampForever)
		{
			m_fStartTime += Time.deltaTime;
			if (m_iCurrentFrame < m_AnimaitonClip.GetFrameCount() - 1)
			{
				m_fNextFrameTime += Time.deltaTime;
				if (!(m_fNextFrameTime < 1f / (float)m_iFrameRate))
				{
					m_fNextFrameTime = 0f;
					m_iCurrentFrame++;
					SetCurrentFrame();
				}
			}
		}
		else if (m_WarpMode == WrapMode.Once)
		{
			m_fStartTime += Time.deltaTime;
			if (m_iCurrentFrame < m_AnimaitonClip.GetFrameCount() - 1)
			{
				m_fNextFrameTime += Time.deltaTime;
				if (!(m_fNextFrameTime < 1f / (float)m_iFrameRate))
				{
					m_fNextFrameTime = 0f;
					m_iCurrentFrame++;
					SetCurrentFrame();
				}
			}
			else if (m_iCurrentFrame == 0)
			{
				m_AnimaitonClip = null;
			}
		}
		else
		{
			if (m_WarpMode != WrapMode.Once)
			{
				return;
			}
			m_fStartTime += Time.deltaTime;
			m_fNextFrameTime += Time.deltaTime;
			if (!(m_fNextFrameTime < 1f / (float)m_iFrameRate))
			{
				m_fNextFrameTime = 0f;
				m_iCurrentFrame++;
				if (m_iCurrentFrame >= m_AnimaitonClip.GetFrameCount())
				{
					m_iCurrentFrame = 0;
				}
				SetCurrentFrame();
				if (m_iCurrentFrame == 0)
				{
					m_AnimaitonClip = null;
				}
			}
		}
	}

	private void SetCurrentFrame()
	{
		m_MeshFilter.mesh = m_AnimaitonClip.m_MeshFrames[m_iCurrentFrame];
		if (null != m_MeshCollider)
		{
			m_MeshCollider.sharedMesh = m_AnimaitonClip.m_MeshFrames[m_iCurrentFrame];
		}
		if (null != m_BoxCollider)
		{
			m_BoxCollider.center = m_AnimaitonClip.m_MeshBoxCenter[m_iCurrentFrame];
			m_BoxCollider.size = m_AnimaitonClip.m_MeshBoxSize[m_iCurrentFrame];
		}
	}
}
