using System.Collections.Generic;
using UnityEngine;

public class AmazonIAPListener : MonoBehaviour
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
		AmazonIAPManager.itemDataRequestFailedEvent += itemDataRequestFailedEvent;
		AmazonIAPManager.itemDataRequestFinishedEvent += itemDataRequestFinishedEvent;
		AmazonIAPManager.purchaseFailedEvent += purchaseFailedEvent;
		AmazonIAPManager.purchaseSuccessfulEvent += purchaseSuccessfulEvent;
		AmazonIAPManager.purchaseUpdatesRequestFailedEvent += purchaseUpdatesRequestFailedEvent;
		AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent += purchaseUpdatesRequestSuccessfulEvent;
		AmazonIAPManager.onSdkAvailableEvent += onSdkAvailableEvent;
		AmazonIAPManager.onGetUserIdResponseEvent += onGetUserIdResponseEvent;
	}

	private void OnDisable()
	{
		AmazonIAPManager.itemDataRequestFailedEvent -= itemDataRequestFailedEvent;
		AmazonIAPManager.itemDataRequestFinishedEvent -= itemDataRequestFinishedEvent;
		AmazonIAPManager.purchaseFailedEvent -= purchaseFailedEvent;
		AmazonIAPManager.purchaseSuccessfulEvent -= purchaseSuccessfulEvent;
		AmazonIAPManager.purchaseUpdatesRequestFailedEvent -= purchaseUpdatesRequestFailedEvent;
		AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent -= purchaseUpdatesRequestSuccessfulEvent;
		AmazonIAPManager.onSdkAvailableEvent -= onSdkAvailableEvent;
		AmazonIAPManager.onGetUserIdResponseEvent -= onGetUserIdResponseEvent;
	}

	private void itemDataRequestFailedEvent()
	{
		Debug.Log("itemDataRequestFailedEvent");
		iZombieSniperGameApp.GetInstance().m_isSupported = false;
		if (m_GameIAP == null)
		{
			m_GameIAP = GameObject.Find("Main Camera").GetComponent<iZombieSniperIAP>();
		}
		if (m_GameIAP != null)
		{
			m_GameIAP.BuyCancel();
		}
	}

	private void itemDataRequestFinishedEvent(List<string> unavailableSkus, List<AmazonItem> availableItems)
	{
		Debug.Log("itemDataRequestFinishedEvent. unavailable skus: " + unavailableSkus.Count + ", avaiable items: " + availableItems.Count);
		iZombieSniperGameApp.GetInstance().m_isSupported = true;
	}

	private void purchaseFailedEvent(string reason)
	{
		Debug.Log("m_GameIAP is" + m_GameIAP);
		Debug.Log("purchaseFailedEvent: " + reason);
		if (m_GameIAP == null)
		{
			m_GameIAP = GameObject.Find("Main Camera").GetComponent<iZombieSniperIAP>();
		}
		if (m_GameIAP != null)
		{
			m_GameIAP.BuyCancel();
		}
	}

	private void purchaseSuccessfulEvent(AmazonReceipt receipt)
	{
		Debug.Log("purchaseSuccessfulEvent: " + receipt);
		iZombieSniperGameApp.GetInstance().OnPurchaseSuccess(receipt.sku);
		if (m_GameIAP == null)
		{
			m_GameIAP = GameObject.Find("Main Camera").GetComponent<iZombieSniperIAP>();
		}
		if (m_GameIAP != null)
		{
			m_GameIAP.OnPurchaseSuccess(receipt.sku);
		}
	}

	private void purchaseUpdatesRequestFailedEvent()
	{
		Debug.Log("purchaseUpdatesRequestFailedEvent");
		if (m_GameIAP == null)
		{
			m_GameIAP = GameObject.Find("Main Camera").GetComponent<iZombieSniperIAP>();
		}
		if (m_GameIAP != null)
		{
			m_GameIAP.OnPurchaseTimeout();
		}
	}

	private void purchaseUpdatesRequestSuccessfulEvent(List<string> revokedSkus, List<AmazonReceipt> receipts)
	{
		Debug.Log("purchaseUpdatesRequestSuccessfulEvent. revoked skus: " + revokedSkus.Count);
		foreach (AmazonReceipt receipt in receipts)
		{
			Debug.Log(receipt);
		}
	}

	private void onSdkAvailableEvent(bool isTestMode)
	{
		Debug.Log("onSdkAvailableEvent. isTestMode: " + isTestMode);
	}

	private void onGetUserIdResponseEvent(string userId)
	{
		Debug.Log("onGetUserIdResponseEvent: " + userId);
	}
}
