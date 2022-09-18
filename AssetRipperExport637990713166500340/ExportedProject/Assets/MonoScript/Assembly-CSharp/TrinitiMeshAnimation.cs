using UnityEngine;

public class TrinitiMeshAnimation : MonoBehaviour
{
	private TrinitiMeshClip m_AnimaitonClip;

	private WrapMode m_WarpMode;

	private float m_fSpeed;

	private float m_fStartTime;

	private float m_fNextFrameTime;

	private int m_iCurrentFrame;

	private bool m_bIncrease;

	private int m_iFrameRate;

	private MeshFilter m_MeshFilter;

	private MeshCollider m_MeshCollider;

	public void PlayAnimation(string name, int rate, WrapMode mode, float fSpeed, int iFrameIndex)
	{
		GameObject gameObject = base.transform.Find(name).gameObject;
		TrinitiMeshClip component = gameObject.GetComponent<TrinitiMeshClip>();
		PlayAnimation(component, rate, mode, fSpeed, iFrameIndex);
	}

	public bool IsPlaying()
	{
		return m_AnimaitonClip != null;
	}

	private void PlayAnimation(TrinitiMeshClip clip, int frameRate, WrapMode mode, float speed, int iFrameIndex)
	{
		if (null == m_MeshFilter)
		{
			m_MeshFilter = base.gameObject.GetComponent<MeshFilter>();
		}
		if (null == m_MeshCollider)
		{
			m_MeshCollider = base.gameObject.GetComponent<MeshCollider>();
		}
		m_AnimaitonClip = clip;
		m_WarpMode = mode;
		m_iFrameRate = frameRate;
		m_fSpeed = speed;
		m_fStartTime = 0f;
		m_fNextFrameTime = 0f;
		m_bIncrease = true;
		m_iCurrentFrame = iFrameIndex;
		m_MeshFilter.mesh = m_AnimaitonClip.m_MeshFrames[m_iCurrentFrame];
	}

	private void Update()
	{
		if (null == m_AnimaitonClip)
		{
			return;
		}
		float num = Time.deltaTime * m_fSpeed;
		if (m_WarpMode == WrapMode.Loop)
		{
			m_fStartTime += num;
			m_fNextFrameTime += num;
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
			m_fStartTime += num;
			m_fNextFrameTime += num;
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
			m_fStartTime += num;
			if (m_iCurrentFrame < m_AnimaitonClip.GetFrameCount() - 1)
			{
				m_fNextFrameTime += num;
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
			m_fStartTime += num;
			if (m_iCurrentFrame < m_AnimaitonClip.GetFrameCount() - 1)
			{
				m_fNextFrameTime += num;
				if (!(m_fNextFrameTime < 1f / (float)m_iFrameRate))
				{
					m_fNextFrameTime = 0f;
					m_iCurrentFrame++;
					SetCurrentFrame();
				}
			}
			else
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
			m_fStartTime += num;
			m_fNextFrameTime += num;
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
	}

	public int GetCurrFrame()
	{
		return m_iCurrentFrame;
	}
}
