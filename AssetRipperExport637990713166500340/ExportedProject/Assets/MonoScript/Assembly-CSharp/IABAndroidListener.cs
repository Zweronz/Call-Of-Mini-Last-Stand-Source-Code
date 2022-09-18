using UnityEngine;

public class IABAndroidListener : MonoBehaviour
{
	public iZombieSniperIAP m_GameIAP;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		Debug.Log("IABAndroidEventListener" + base.gameObject.name);
		Object.DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		IABAndroidManager.billingSupportedEvent += billingSupportedEvent;
		IABAndroidManager.purchaseSignatureVerifiedEvent += purchaseSignatureVerifiedEvent;
		IABAndroidManager.purchaseSucceededEvent += purchaseSucceededEvent;
		IABAndroidManager.purchaseCancelledEvent += purchaseCancelledEvent;
		IABAndroidManager.purchaseRefundedEvent += purchaseRefundedEvent;
		IABAndroidManager.purchaseFailedEvent += purchaseFailedEvent;
		IABAndroidManager.transactionsRestoredEvent += transactionsRestoredEvent;
		IABAndroidManager.transactionRestoreFailedEvent += transactionRestoreFailedEvent;
	}

	private void OnDisable()
	{
		IABAndroidManager.billingSupportedEvent -= billingSupportedEvent;
		IABAndroidManager.purchaseSignatureVerifiedEvent -= purchaseSignatureVerifiedEvent;
		IABAndroidManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		IABAndroidManager.purchaseCancelledEvent -= purchaseCancelledEvent;
		IABAndroidManager.purchaseRefundedEvent -= purchaseRefundedEvent;
		IABAndroidManager.purchaseFailedEvent -= purchaseFailedEvent;
		IABAndroidManager.transactionsRestoredEvent -= transactionsRestoredEvent;
		IABAndroidManager.transactionRestoreFailedEvent -= transactionRestoreFailedEvent;
	}

	private void billingSupportedEvent(bool isSupported)
	{
		Debug.Log("billingSupportedEvent: " + isSupported);
		IABAndroid.restoreTransactions();
		iZombieSniperGameApp.GetInstance().m_isSupported = isSupported;
	}

	private void purchaseSignatureVerifiedEvent(string signedData, string signature)
	{
		Debug.Log("purchaseSignatureVerifiedEvent. signedData: " + signedData + ", signature: " + signature);
	}

	private void purchaseSucceededEvent(string productId)
	{
		Debug.Log("purchaseSucceededEvent: " + productId);
		iZombieSniperGameApp.GetInstance().OnPurchaseSuccess(productId);
		if (m_GameIAP == null)
		{
			m_GameIAP = GameObject.Find("Main Camera").GetComponent<iZombieSniperIAP>();
		}
		if (m_GameIAP != null)
		{
			m_GameIAP.OnPurchaseSuccess(productId);
		}
	}

	private void purchaseCancelledEvent(string productId)
	{
		Debug.Log("purchaseCancelledEvent: " + productId);
		if (m_GameIAP == null)
		{
			m_GameIAP = GameObject.Find("Main Camera").GetComponent<iZombieSniperIAP>();
		}
		if (m_GameIAP != null)
		{
			m_GameIAP.OnPurchaseCancelled(productId);
		}
	}

	private void purchaseRefundedEvent(string productId)
	{
		Debug.Log("purchaseRefundedEvent: " + productId);
		if (m_GameIAP == null)
		{
			m_GameIAP = GameObject.Find("Main Camera").GetComponent<iZombieSniperIAP>();
		}
		if (m_GameIAP != null)
		{
			m_GameIAP.OnPurchaseRefunded(productId);
		}
	}

	private void purchaseFailedEvent(string productId)
	{
		Debug.Log("purchaseFailedEvent: " + productId);
		if (m_GameIAP == null)
		{
			m_GameIAP = GameObject.Find("Main Camera").GetComponent<iZombieSniperIAP>();
		}
		if (m_GameIAP != null)
		{
			m_GameIAP.OnPurchaseFailed(productId);
		}
	}

	private void transactionsRestoredEvent()
	{
		Debug.Log("transactionsRestoredEvent");
	}

	private void transactionRestoreFailedEvent(string error)
	{
		Debug.Log("transactionRestoreFailedEvent: " + error);
	}
}
