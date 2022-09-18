using UnityEngine;

[ExecuteInEditMode]
public class PathSingleMaker : MonoBehaviour
{
	private CSinglePara m_SinglePara;

	private void Awake()
	{
		m_SinglePara = base.gameObject.GetComponent<CSinglePara>();
	}

	public void AddPoint()
	{
		GameObject point = new GameObject();
		m_SinglePara.AddPoint(point);
		m_SinglePara.RefreshPointSequence();
	}
}
