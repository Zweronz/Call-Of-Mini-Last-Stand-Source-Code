using UnityEngine;

public class LogicWaveFormula
{
	public int id;

	public float a;

	public float b;

	public float c;

	public float k;

	public float x1;

	public float x2;

	public float y1;

	public float y2;

	public void Initialize()
	{
		float num = y2 - c;
		if (num == 0f)
		{
			Debug.Log("y2 - c = 0 is not allowed");
			return;
		}
		num = Mathf.Pow((y1 - c) / (y2 - c), 1f / k) - 1f;
		if (num == 0f)
		{
			Debug.Log("Mathf.Pow((y1 - c) / (y2 - c), 1 / k) - 1 = 0 is not allowed");
			return;
		}
		b = (Mathf.Pow((y1 - c) / (y2 - c), 1f / k) * x2 - x1) / (Mathf.Pow((y1 - c) / (y2 - c), 1f / k) - 1f);
		num = Mathf.Pow(x2 - b, k);
		if (num == 0f)
		{
			Debug.Log("Mathf.Pow(x2 - b, k) = 0 is not allowed");
		}
		else
		{
			a = (y2 - c) / Mathf.Pow(x2 - b, k);
		}
	}
}
