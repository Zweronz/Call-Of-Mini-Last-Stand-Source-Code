using UnityEngine;

[ExecuteInEditMode]
public class CRect : MonoBehaviour
{
	public float WidthX
	{
		get
		{
			return 2f * base.transform.lossyScale.x;
		}
	}

	public float WidthZ
	{
		get
		{
			return 2f * base.transform.lossyScale.z;
		}
	}

	private void OnDrawGizmos()
	{
		base.transform.position = new Vector3(base.transform.position.x, 0.1f, base.transform.position.z);
		base.transform.eulerAngles = Vector3.zero;
	}
}
