using UnityEngine;

public class TUIScript : MonoBehaviour, TUIHandler
{
	public TUI m_TUI;

	private void Awake()
	{
		m_TUI = TUI.Instance("TUI");
	}

	private void Start()
	{
		m_TUI.SetHandler(this);
	}

	private void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		TUIInput[] array = input;
		foreach (TUIInput input2 in array)
		{
			m_TUI.HandleInput(input2);
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
	}
}
