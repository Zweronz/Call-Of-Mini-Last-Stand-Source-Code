public class IAPPlugin
{
	public enum Status
	{
		kUserCancel = -2,
		kError = -1,
		kBuying = 0,
		kSuccess = 1
	}

	public static void NowPurchaseProduct(string productId, string productCount)
	{
	}

	public static Status GetPurchaseStatus()
	{
		return Status.kSuccess;
	}
}
