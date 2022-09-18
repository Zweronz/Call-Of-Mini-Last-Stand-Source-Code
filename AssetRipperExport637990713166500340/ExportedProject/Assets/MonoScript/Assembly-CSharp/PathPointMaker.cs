using System;
using UnityEngine;

[ExecuteInEditMode]
public class PathPointMaker : MonoBehaviour
{
	[NonSerialized]
	public string[] m_sOptions = new string[3] { "Go", "Rotate", "Stand" };

	private CPathPara m_PathPara;

	private void Awake()
	{
		m_PathPara = base.gameObject.GetComponent<CPathPara>();
	}

	public void AddPoint(string sOptions, int nIndex = -1)
	{
		GameObject gameObject = new GameObject();
		switch (sOptions)
		{
		case "Stand":
		{
			CMoveStand cMoveStand = gameObject.AddComponent<CMoveStand>();
			cMoveStand.m_State = CMoveBase.MoveType.Stand;
			cMoveStand.m_fTime = 1f;
			break;
		}
		case "Go":
		{
			CMoveGo cMoveGo = gameObject.AddComponent<CMoveGo>();
			cMoveGo.m_State = CMoveBase.MoveType.Move;
			cMoveGo.m_fSpeed = 1f;
			break;
		}
		case "Rotate":
		{
			CMoveRotate cMoveRotate = gameObject.AddComponent<CMoveRotate>();
			cMoveRotate.m_State = CMoveBase.MoveType.Rotate;
			cMoveRotate.m_fSpeed = 1f;
			break;
		}
		}
		if (gameObject != null)
		{
			m_PathPara.AddPoint(gameObject, nIndex);
			m_PathPara.RefreshPointSequence();
		}
	}

	public void SavePath()
	{
	}
}
