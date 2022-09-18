using UnityEngine;

public class iZombieSniperIAP : MonoBehaviour
{
	public iZombieSniperIAPUI m_GameIAPUI;

	public iZombieSniperGameState m_GameState;

	public Vector2 m_v2TouchPos = Vector2.zero;

	public bool m_bDragMouse;

	public bool m_bNowBuying;

	public string m_sIapBuyingID;

	public float m_fDelayTime;

	public bool m_isMask;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("Main Camera");
		m_GameIAPUI = gameObject.GetComponent<iZombieSniperIAPUI>();
		m_GameState = iZombieSniperGameApp.GetInstance().m_GameState;
		m_bDragMouse = false;
		m_bNowBuying = false;
		m_sIapBuyingID = string.Empty;
		m_isMask = false;
	}

	private void Update()
	{
		if (m_bNowBuying)
		{
			UpdateBuying(Time.deltaTime);
		}
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		for (int i = 0; i < array.Length; i++)
		{
			UITouchInner touch = array[i];
			if (m_GameIAPUI != null)
			{
				m_GameIAPUI.m_UIManagerRef.HandleInput(touch);
			}
			if (m_isMask)
			{
				continue;
			}
			if (touch.phase == TouchPhase.Began)
			{
				m_v2TouchPos = touch.position;
				m_bDragMouse = false;
				if (!m_bNowBuying)
				{
					if (!Utils.PtInRect(m_v2TouchPos, Utils.CalcScaleRect(new Rect(0f, 75f, 480f, 165f))))
					{
						break;
					}
					if (m_GameIAPUI.m_IAPCell.m_bAnim)
					{
						m_GameIAPUI.m_IAPCell.m_bAnim = false;
					}
				}
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				if (!m_bDragMouse && Vector2.Distance(m_v2TouchPos, touch.position) > 12f)
				{
					m_bDragMouse = true;
				}
				if (!m_bNowBuying && m_bDragMouse)
				{
					if (!Utils.PtInRect(m_v2TouchPos, Utils.CalcScaleRect(new Rect(0f, 75f, 480f, 165f))))
					{
						break;
					}
					float num = Mathf.Abs(touch.deltaPosition.x);
					m_GameIAPUI.m_IAPCell.Move(num * 5f, num * 5f, (touch.deltaPosition.x > 0f) ? 1 : (-1));
				}
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				m_bDragMouse = false;
			}
		}
	}

	public void Buy(string iapid)
	{
		if (!m_bNowBuying)
		{
			if (!iZombieSniperGameApp.GetInstance().Purchase(iapid))
			{
				m_GameIAPUI.ShowDialog(true, "Connect Failed");
				return;
			}
			m_bNowBuying = true;
			m_sIapBuyingID = iapid;
			m_fDelayTime = 30f;
			m_GameIAPUI.ShowDialog(true, "Connecting...", false);
		}
	}

	public void BuyCancel()
	{
		m_bNowBuying = false;
		m_sIapBuyingID = string.Empty;
		m_GameIAPUI.ShowDialog(false, string.Empty);
	}

	public bool IsBuying()
	{
		return m_bNowBuying;
	}

	public void UpdateBuying(float deltaTime)
	{
		if (m_bNowBuying)
		{
			m_fDelayTime -= deltaTime;
			if (m_fDelayTime <= 0f)
			{
				BuyCancel();
				m_GameIAPUI.ShowDialog(true, "Connection timed out.");
			}
		}
	}

	public void OnPurchaseSuccess(string sID)
	{
		m_GameIAPUI.SetPlayerGodGold(m_GameState.m_nPlayerTotalGod);
		m_GameIAPUI.SetPlayerGold(m_GameState.m_nPlayerTotalCash);
		BuyCancel();
	}

	public void OnPurchaseFailed(string sID)
	{
		if (!(m_sIapBuyingID != sID))
		{
			BuyCancel();
		}
	}

	public void OnPurchaseCancelled(string sID)
	{
		if (!(m_sIapBuyingID != sID))
		{
			BuyCancel();
		}
	}

	public void OnPurchaseRefunded(string sID)
	{
	}

	public void OnPurchaseTimeout()
	{
		BuyCancel();
		m_GameIAPUI.ShowDialog(true, "Connection timed out.");
	}
}
