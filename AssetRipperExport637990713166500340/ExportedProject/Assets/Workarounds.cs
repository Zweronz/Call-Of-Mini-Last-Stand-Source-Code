using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workarounds : MonoBehaviour {
public static bool isCheats = true;
void Start()
{
	DontDestroyOnLoad(base.gameObject);
}
public void DoTheCoroutine(IEnumerator method)
{
	StartCoroutine(method);
}
}
