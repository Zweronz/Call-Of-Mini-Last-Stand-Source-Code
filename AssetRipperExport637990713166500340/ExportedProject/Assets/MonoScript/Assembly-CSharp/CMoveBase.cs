using UnityEngine;

public class CMoveBase : MonoBehaviour
{
	public enum MoveType
	{
		Stand = 0,
		Move = 1,
		Rotate = 2
	}

	public MoveType m_State;
}
