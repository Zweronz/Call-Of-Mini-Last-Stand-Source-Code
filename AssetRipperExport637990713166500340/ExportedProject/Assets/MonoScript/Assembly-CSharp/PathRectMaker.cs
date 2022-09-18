using UnityEngine;

[ExecuteInEditMode]
public class PathRectMaker : MonoBehaviour
{
	private CRectPara m_RectPara;

	private void Awake()
	{
		m_RectPara = base.gameObject.GetComponent<CRectPara>();
	}

	public void AddRect()
	{
		GameObject gameObject = new GameObject();
		gameObject.AddComponent<CRect>();
		m_RectPara.AddRect(gameObject);
		m_RectPara.RefreshRectSequence();
	}
}
