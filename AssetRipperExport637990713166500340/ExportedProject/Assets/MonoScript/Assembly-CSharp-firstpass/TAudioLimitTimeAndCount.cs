using System.Collections.Generic;
using UnityEngine;

public class TAudioLimitTimeAndCount : MonoBehaviour, ITAudioLimit
{
	public int maxCount;

	public float deltaTime = 0.2f;

	private static Dictionary<string, int> s_records = new Dictionary<string, int>();

	private float m_triggerTime;

	private bool m_isTimeout = true;

	private Queue<float> m_timeBound = new Queue<float>();

	public bool isCanPlay
	{
		get
		{
			return !Limit();
		}
	}

	private void Update()
	{
		if (!m_isTimeout)
		{
			m_triggerTime += Time.deltaTime;
			if (m_triggerTime > deltaTime)
			{
				m_isTimeout = true;
				string text = "TimeAndCountLimit_" + base.name;
				if (s_records.ContainsKey(text))
				{
					Dictionary<string, int> dictionary;
					Dictionary<string, int> dictionary2 = (dictionary = s_records);
					string key;
					string key2 = (key = text);
					int num = dictionary[key];
					dictionary2[key2] = num - 1;
				}
			}
		}
		while (m_timeBound.Count > 0 && m_timeBound.Peek() < Time.time)
		{
			m_timeBound.Dequeue();
		}
	}

	private void OnAudioTrigger()
	{
		if (m_timeBound.Count < maxCount)
		{
			m_timeBound.Enqueue(Time.time + deltaTime);
		}
		if (m_isTimeout)
		{
			string text = "TimeAndCountLimit_" + base.name;
			if (s_records.ContainsKey(text))
			{
				Dictionary<string, int> dictionary;
				Dictionary<string, int> dictionary2 = (dictionary = s_records);
				string key;
				string key2 = (key = text);
				int num = dictionary[key];
				dictionary2[key2] = num + 1;
			}
			else
			{
				s_records.Add(text, 1);
			}
			m_triggerTime = 0f;
			m_isTimeout = false;
		}
	}

	private bool Limit()
	{
		if (m_timeBound.Count >= maxCount)
		{
			return true;
		}
		string key = "TimeAndCountLimit_" + base.name;
		if (s_records.ContainsKey(key) && s_records[key] >= maxCount)
		{
			return true;
		}
		return false;
	}
}
