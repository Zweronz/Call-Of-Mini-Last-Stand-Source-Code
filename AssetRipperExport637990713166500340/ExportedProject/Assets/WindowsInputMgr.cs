using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsInputMgr : MonoBehaviour
{
	public static UITouchInner[] MockTouches()
	{
		UITouchInner[] touches = new UITouchInner[1];
		foreach(Touch touchThe in InputHelper.GetTouches())
		{
			touches[0].deltaPosition = touchThe.deltaPosition;
			touches[0].deltaTime = touchThe.deltaTime;
			touches[0].fingerId = touchThe.fingerId;
			touches[0].phase = touchThe.phase;
			touches[0].position = touchThe.position;
			touches[0].tapCount = 1;
			return touches;
		}
		return touches;
	}
}