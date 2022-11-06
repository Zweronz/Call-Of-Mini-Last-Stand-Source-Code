using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOnlyIfDebugBuild : MonoBehaviour {
void Start()
{
	if(!Debug.isDebugBuild)
	{
		Destroy(base.gameObject);
	}
}
}
