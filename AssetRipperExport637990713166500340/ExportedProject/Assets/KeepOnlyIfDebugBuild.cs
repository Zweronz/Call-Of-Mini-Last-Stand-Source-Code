using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOnlyIfDebugBuild : MonoBehaviour {
void Start()
{
	if(!Debug.isDebugBuild || Application.platform == RuntimePlatform.WindowsEditor)
	{
		Destroy(base.gameObject);
	}
}
}
