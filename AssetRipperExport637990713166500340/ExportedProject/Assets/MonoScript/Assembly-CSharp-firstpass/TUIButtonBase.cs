using UnityEngine;

public class TUIButtonBase : TUIControlImpl
{
	public GameObject frameNormal;

	public GameObject framePressed;

	public GameObject frameDisabled;

	public bool disabled;

	public bool pressed;

	public bool hided;

	protected int fingerId = -1;

	public new void Awake()
	{
		base.Awake();
		UpdateFrame();
	}

	public new void Start()
	{
		base.Start();
	}

	public void OnEnable()
	{
		UpdateFrame();
		hided = false;
	}

	public void OnDisable()
	{
		HideFrame();
		hided = true;
	}

	public void SetDisabled(bool disabled)
	{
		this.disabled = disabled;
		UpdateFrame();
	}

	protected void UpdateFrame()
	{
		HideFrame();
		ShowFrame();
	}

	private void HideFrame()
	{
		if ((bool)frameNormal)
		{
			frameNormal.active = false;
		}
		if ((bool)framePressed)
		{
			framePressed.active = false;
		}
		if ((bool)frameDisabled)
		{
			frameDisabled.active = false;
		}
	}

	private void ShowFrame()
	{
		if (disabled)
		{
			if ((bool)frameDisabled)
			{
				frameDisabled.active = true;
			}
		}
		else if (pressed)
		{
			if ((bool)framePressed)
			{
				framePressed.active = true;
			}
		}
		else if ((bool)frameNormal)
		{
			frameNormal.active = true;
		}
	}
}
