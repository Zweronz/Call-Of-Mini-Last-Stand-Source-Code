using UnityEngine;

[RequireComponent(typeof(GUIText))]
public class FrameCounter : MonoBehaviour
{
	public float updateInterval = 0.5f;

	private float accum;

	private int frames;

	private float timeLeft;

	private void Start()
	{
		timeLeft = updateInterval;
	}

	private void Update()
	{
		timeLeft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;
		if (timeLeft <= 0f)
		{
			float num = accum / (float)frames;
			float num2 = 1000f / num;
			base.GetComponent<GUIText>().text = "timePerFrame: " + num2.ToString("f2") + "ms\n";
			GUIText gUIText = base.GetComponent<GUIText>();
			gUIText.text = gUIText.text + "framePerSecond: " + num.ToString("f2");
			timeLeft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}
}
