using System;
using UnityEngine;

public class TUIButtonSelectGroupEx : TUIButtonSelectGroup
{
	[Serializable]
	public class TabInfo
	{
		public TUIButtonSelect control;

		public GameObject panel;

		public Vector3 showPosition;

		public Vector3 hidePosition;
	}

	public TabInfo[] tabInfo;

	public int current = -1;

	public new void Start()
	{
		if (tabInfo == null)
		{
			return;
		}
		for (int i = 0; i < tabInfo.Length; i++)
		{
			if (i == current)
			{
				tabInfo[i].panel.transform.localPosition = tabInfo[i].showPosition;
			}
			else
			{
				tabInfo[i].panel.transform.localPosition = tabInfo[i].hidePosition;
			}
		}
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (tabInfo != null)
		{
			for (int i = 0; i < tabInfo.Length; i++)
			{
				if (tabInfo[i].control == control && eventType == 1)
				{
					current = i;
					break;
				}
			}
			for (int j = 0; j < tabInfo.Length; j++)
			{
				if (j == current)
				{
					tabInfo[j].panel.transform.localPosition = tabInfo[j].showPosition;
				}
				else
				{
					tabInfo[j].panel.transform.localPosition = tabInfo[j].hidePosition;
				}
			}
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}
}
