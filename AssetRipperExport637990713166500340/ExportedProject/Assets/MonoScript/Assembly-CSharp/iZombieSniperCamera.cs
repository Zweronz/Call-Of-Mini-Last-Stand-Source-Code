using UnityEngine;

public class iZombieSniperCamera : MonoBehaviour
{
	public enum CameraState
	{
		kNormal = 0,
		kDynmaic = 1,
		kAiming = 2,
		kHeadShot = 3,
		kAimLook = 4,
		kLock = 5,
		kMovie = 6
	}

	public Camera m_Camera;

	public Transform m_CameraTransform;

	public iZombieSniperGameSceneBase m_GameScene;

	public iZombieSniperGameState m_GameState;

	public CameraState m_CameraState;

	private Vector2 m_v2MoveSpeed;

	private Vector3 m_v3StartGravity;

	private float m_fAimFov;

	private float m_fAimDuringTime;

	private Vector3 m_v3SrcForward;

	private Vector3 m_v3DstForward;

	private float m_fAimLookRate;

	private float m_fAimLookRateSpeed;

	private bool m_bShake;

	private float m_fShakeTimeCount;

	private float m_fShakeTime;

	private float m_fShakeFactor;

	private Vector3 m_v3ShakePos;

	private bool m_bShakeGround;

	private float m_fHeadShotTime;

	private float m_fHeadShotStartFov;

	private bool m_bSteadyBreath;

	private float m_fBreathCur;

	private float m_fBreathMax;

	private float m_fBreathSpeed;

	private int m_nBreathDir;

	private Vector3 m_v3CameraPosition;

	private float m_fPitchMax;

	private float m_fPitchMin;

	private float m_fYawMax;

	private float m_fYawMin;

	public void Initialize()
	{
		m_Camera = Camera.main;
		m_Camera.nearClipPlane = 0.1f;
		m_Camera.farClipPlane = 1000f;
		m_CameraTransform = m_Camera.transform;
		m_GameScene = iZombieSniperGameApp.GetInstance().m_GameScene;
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_CameraState = CameraState.kNormal;
		m_v3StartGravity = Vector3.zero;
		m_bShake = false;
		m_fShakeTimeCount = 0f;
		m_fShakeTime = 0f;
		m_fShakeFactor = 0f;
		m_v3ShakePos = Vector3.zero;
		m_bShakeGround = false;
		m_fHeadShotTime = 0f;
		m_fHeadShotStartFov = 0f;
		m_bSteadyBreath = false;
		m_fBreathCur = 0f;
		m_fBreathMax = 0f;
		m_fBreathSpeed = 0f;
		m_nBreathDir = 1;
		m_fPitchMax = 0f;
		m_fPitchMin = 0f;
		m_fYawMax = 0f;
		m_fYawMin = 0f;
		m_v3CameraPosition = m_GameScene.m_v3SniperPosition;
		Restore();
	}

	public void Restore()
	{
		m_Camera.transform.position = m_v3CameraPosition;
		m_Camera.fieldOfView = m_GameScene.m_fSniperFov;
		m_CameraState = CameraState.kNormal;
	}

	private void Update()
	{
		if (m_GameScene == null || m_GameState == null)
		{
			return;
		}
		switch (m_CameraState)
		{
		case CameraState.kAimLook:
		{
			m_fAimLookRate += m_fAimLookRateSpeed * Time.deltaTime;
			if (m_fAimLookRate >= 1f)
			{
				m_fAimLookRate = 1f;
				m_CameraState = CameraState.kNormal;
			}
			m_CameraTransform.forward = Vector3.Lerp(m_v3SrcForward, m_v3DstForward, m_fAimLookRate);
			Vector3 localEulerAngles = m_CameraTransform.localEulerAngles;
			localEulerAngles.z = 0f;
			LimitEulerAngle(ref localEulerAngles.x, m_fPitchMin, m_fPitchMax);
			LimitEulerAngle(ref localEulerAngles.y, m_fYawMin, m_fYawMax);
			m_CameraTransform.localEulerAngles = localEulerAngles;
			break;
		}
		case CameraState.kDynmaic:
			m_fAimDuringTime += Time.deltaTime;
			if (m_fAimDuringTime >= 0.2f)
			{
				m_fAimDuringTime = 0.2f;
				m_CameraState = CameraState.kAiming;
			}
			m_Camera.fieldOfView = m_GameScene.m_fSniperFov + m_fAimDuringTime / 0.2f * (m_fAimFov - m_GameScene.m_fSniperFov);
			break;
		case CameraState.kAiming:
		{
			if (m_GameState.m_bIsTiltControl)
			{
				ComputeMoveSpeed();
			}
			if (m_v2MoveSpeed.x == 0f && m_v2MoveSpeed.y == 0f)
			{
				break;
			}
			Vector2 zero = Vector2.zero;
			if (m_bSteadyBreath)
			{
				m_fBreathCur += Time.deltaTime * (float)m_nBreathDir * m_fBreathSpeed;
				if (m_fBreathCur >= m_fBreathMax)
				{
					m_fBreathCur = m_fBreathMax;
					m_nBreathDir *= -1;
				}
				else if (m_fBreathCur <= 0f)
				{
					m_fBreathCur = 0f;
					m_bSteadyBreath = false;
				}
				zero.y += m_fBreathCur * (float)m_nBreathDir;
			}
			zero.x += m_v2MoveSpeed.x;
			zero.y += m_v2MoveSpeed.y;
			ResetMoveSpeed();
			YawCamera(zero.x);
			PitchCamera(zero.y);
			break;
		}
		case CameraState.kHeadShot:
			m_fHeadShotTime += Time.deltaTime;
			if (m_fHeadShotTime <= 0.4f)
			{
				float t = m_fHeadShotTime / 0.4f;
				m_Camera.fieldOfView = Mathf.Lerp(m_fHeadShotStartFov, 5f, t);
			}
			else if (m_fHeadShotTime >= 0.9f)
			{
				m_CameraState = CameraState.kNormal;
				Time.timeScale = 1f;
			}
			break;
		}
		if (m_bShake)
		{
			m_fShakeTimeCount += Time.deltaTime;
			Vector3 vector = Random.onUnitSphere * 0.5f * m_fShakeFactor;
			if (m_bShakeGround && vector.y < 0.1f)
			{
				vector.y = 0.1f;
			}
			if (m_CameraTransform.parent != null && m_CameraState != CameraState.kMovie)
			{
				m_Camera.transform.localPosition = vector;
			}
			else
			{
				m_Camera.transform.position = m_v3CameraPosition + vector;
			}
			if (m_fShakeTimeCount > m_fShakeTime)
			{
				if (m_CameraTransform.parent != null && m_CameraState != CameraState.kMovie)
				{
					m_Camera.transform.localPosition = Vector3.zero;
				}
				else
				{
					m_Camera.transform.position = m_v3CameraPosition;
				}
				m_v3ShakePos = Vector3.zero;
				m_bShake = false;
				m_bShakeGround = false;
			}
		}
		else
		{
			m_v3CameraPosition = m_CameraTransform.position;
		}
	}

	public bool Aim(Vector2 point)
	{
		if (m_CameraState != 0 && m_CameraState != CameraState.kAimLook)
		{
			return false;
		}
		m_CameraState = CameraState.kDynmaic;
		m_v3StartGravity = Input.acceleration;
		iWeapon userWeapon = m_GameState.GetUserWeapon();
		m_fAimFov = userWeapon.m_fSZ;
		m_fAimDuringTime = 0f;
		return true;
	}

	public bool CloseAim()
	{
		if (m_CameraState != CameraState.kAiming)
		{
			return false;
		}
		Restore();
		return true;
	}

	private void ComputeMoveSpeed()
	{
		m_v2MoveSpeed.x = Input.GetAxis("Horizontal");
		m_v2MoveSpeed.y = Input.GetAxis("Vertical");
	}

	public void AimMove(float x, float y)
	{
		m_v2MoveSpeed.x = x / (float)Screen.width * 0.7f;
		m_v2MoveSpeed.y = y / (float)Screen.height * 0.7f;
	}

	public void ResetMoveSpeed()
	{
		m_v2MoveSpeed.x = 0f;
		m_v2MoveSpeed.y = 0f;
	}

	public float CaculateMoveSpeed(float x)
	{
		Debug.Log(x);
		return (!(x > 5f) && !(x < -5f)) ? (x / (float)Screen.width * 0.5f) : (x / (float)Screen.width * 2.5f);
	}

	public void AdjustFov()
	{
		iWeapon userWeapon = m_GameState.GetUserWeapon();
		m_fAimFov = userWeapon.m_fSZ;
		if (m_CameraState == CameraState.kAiming)
		{
			m_Camera.fieldOfView = m_fAimFov;
		}
	}

	public void Shake(float fFactor, float fShakeTime, bool bGround = false, bool bForce = false)
	{
		if (m_CameraState != CameraState.kDynmaic && m_CameraState != CameraState.kHeadShot && (!m_bShake || bForce))
		{
			m_bShake = true;
			m_fShakeTimeCount = 0f;
			m_fShakeFactor = fFactor;
			m_fShakeTime = fShakeTime;
			if (m_v3ShakePos == Vector3.zero)
			{
				m_v3ShakePos = base.transform.position;
			}
			m_bShakeGround = true;
		}
	}

	public void ShakeEnd()
	{
		if (m_bShake)
		{
			m_bShake = false;
			m_bShakeGround = false;
			if (m_v3ShakePos != Vector3.zero)
			{
				base.transform.position = m_v3ShakePos;
			}
			m_v3ShakePos = Vector3.zero;
		}
	}

	public void HeadShotCamera()
	{
		m_CameraState = CameraState.kHeadShot;
		if (m_bShake)
		{
			m_Camera.transform.position = m_v3CameraPosition;
			m_bShake = false;
		}
		m_fHeadShotTime = 0f;
		m_fHeadShotStartFov = m_Camera.fieldOfView;
		Time.timeScale = 0.2f;
	}

	public void BreathCamera(float factor)
	{
		m_bSteadyBreath = true;
		m_fBreathCur = 0f;
		m_fBreathMax = 5f * factor * (float)m_GameState.m_nHDFactor;
		m_fBreathSpeed = m_fBreathMax * 2f / 1f;
		m_nBreathDir = 1;
	}

	public void SetRotateLimit(float pitchmin, float pitchmax, float yawmin, float yawmax)
	{
		m_fPitchMin = pitchmin;
		m_fPitchMax = pitchmax;
		m_fYawMin = yawmin;
		m_fYawMax = yawmax;
	}

	public void YawCamera(float fAngle)
	{
		if (m_CameraState == CameraState.kNormal || m_CameraState == CameraState.kAiming || m_CameraState == CameraState.kAimLook)
		{
			m_CameraTransform.RotateAround(Vector3.up, fAngle);
			Vector3 localEulerAngles = m_CameraTransform.localEulerAngles;
			localEulerAngles.z = 0f;
			LimitEulerAngle(ref localEulerAngles.y, m_fYawMin, m_fYawMax);
			m_CameraTransform.localEulerAngles = localEulerAngles;
		}
	}

	public void PitchCamera(float fAngle)
	{
		if (m_CameraState == CameraState.kNormal || m_CameraState == CameraState.kAiming || m_CameraState == CameraState.kAimLook)
		{
			m_CameraTransform.RotateAround(m_CameraTransform.right, 0f - fAngle);
			Vector3 localEulerAngles = m_CameraTransform.localEulerAngles;
			localEulerAngles.z = 0f;
			LimitEulerAngle(ref localEulerAngles.x, m_fPitchMin, m_fPitchMax);
			m_CameraTransform.localEulerAngles = localEulerAngles;
		}
	}

	private void LimitEulerAngle(ref float value, float min, float max)
	{
		if (min == 0f && max == 0f)
		{
			return;
		}
		if (min < 0f)
		{
			min += 360f;
			float num = (min + max) / 2f;
			if (value < min && value > num)
			{
				value = min;
			}
			if (value > max && value < num)
			{
				value = max;
			}
		}
		else
		{
			float num2 = (360f + min + max) / 2f;
			if (value < min || value > num2)
			{
				value = min;
			}
			if (value > max && value < num2)
			{
				value = max;
			}
		}
	}

	public void AimLook(Vector2 point, float fRateSpeed)
	{
		m_CameraState = CameraState.kAimLook;
		m_v3SrcForward = m_Camera.transform.forward;
		m_v3DstForward = Vector3.zero;
		m_fAimLookRate = 0f;
		m_fAimLookRateSpeed = fRateSpeed;
		m_v3DstForward = m_Camera.ScreenPointToRay(new Vector3(point.x, point.y, 1000f)).direction.normalized;
	}

	public void SkipAimProcess()
	{
		if (m_CameraState == CameraState.kDynmaic)
		{
			m_CameraState = CameraState.kAiming;
			m_Camera.fieldOfView = m_fAimFov;
		}
	}

	public void SetPosition(Vector3 v3)
	{
		m_v3CameraPosition = v3;
		m_CameraTransform.position = v3;
	}

	public Vector3 GetPosition()
	{
		return m_v3CameraPosition;
	}
}
