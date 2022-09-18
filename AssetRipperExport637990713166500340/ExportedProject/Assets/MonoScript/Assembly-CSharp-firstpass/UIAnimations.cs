using System.Collections;
using UnityEngine;

public class UIAnimations
{
	public class ControlData
	{
		public string control_name;

		public Vector2 pos;

		public float angle;

		public float alpha;

		public float scale;

		public Vector2 clip;

		public ControlData()
		{
			control_name = string.Empty;
			pos = new Vector2(0f, 0f);
			angle = 0f;
			alpha = 0f;
			scale = 1f;
			clip = Vector2.zero;
		}
	}

	public enum state
	{
		none = 0,
		doing = 1,
		wait_end = 2,
		end = 3
	}

	public string animation_name;

	public ArrayList control_data;

	private state translate_state;

	private float translate_duringtime;

	private Vector2 translate_current;

	private bool translate_exchange;

	public bool translate_have;

	public Vector2 translate_start;

	public float translate_time;

	public Vector2 translate_offset;

	public bool translate_restore;

	public bool translate_loop;

	public bool translate_reverse;

	public state scale_state;

	public float scale_duringtime;

	public float scale_current;

	public bool scale_exchange;

	public bool scale_have;

	public float scale_start;

	public float scale_time;

	public float scale_range;

	public bool scale_restore;

	public bool scale_loop;

	public bool scale_reverse;

	private state rotate_state;

	private float rotate_duringtime;

	private float rotate_current;

	private bool rotate_exchange;

	public bool rotate_have;

	public float rotate_start;

	public float rotate_time;

	public float rotate_angle;

	public bool rotate_restore;

	public bool rotate_loop;

	public bool rotate_reverse;

	public state alpha_state;

	public float alpha_duringtime;

	public float alpha_current;

	public bool alpha_exchange;

	public bool alpha_have;

	public float alpha_start;

	public float alpha_time;

	public float alpha_range;

	public bool alpha_restore;

	public bool alpha_loop;

	public bool alpha_reverse;

	public state clip_state;

	public float clip_duringtime;

	public Vector2 clip_current;

	public bool clip_exchange;

	public bool clip_have;

	public Vector2 clip_start;

	public float clip_time;

	public Vector2 clip_offset;

	public bool clip_restore;

	public bool clip_loop;

	public bool clip_reverse;

	public UIAnimations()
	{
		animation_name = string.Empty;
		control_data = new ArrayList();
		translate_have = false;
		translate_start = new Vector2(0f, 0f);
		translate_time = 0f;
		translate_offset = new Vector2(0f, 0f);
		translate_restore = false;
		translate_loop = false;
		translate_reverse = false;
		scale_have = false;
		scale_start = 0f;
		scale_time = 0f;
		scale_range = 0f;
		scale_restore = false;
		scale_loop = false;
		scale_reverse = false;
		rotate_have = false;
		rotate_start = 0f;
		rotate_time = 0f;
		rotate_angle = 0f;
		rotate_restore = false;
		rotate_loop = false;
		rotate_reverse = false;
		alpha_have = false;
		alpha_start = 0f;
		alpha_time = 0f;
		alpha_range = 0f;
		alpha_restore = false;
		alpha_loop = false;
		alpha_reverse = false;
		clip_have = false;
		clip_start = Vector2.zero;
		clip_time = 0f;
		clip_offset = Vector2.zero;
		clip_restore = false;
		clip_loop = false;
		clip_reverse = false;
	}

	public void Reset()
	{
		translate_state = state.none;
		translate_duringtime = 0f;
		translate_current = new Vector2(0f, 0f);
		translate_exchange = false;
		scale_state = state.none;
		scale_duringtime = 0f;
		scale_current = 0f;
		scale_exchange = false;
		rotate_state = state.none;
		rotate_duringtime = 0f;
		rotate_current = 0f;
		rotate_exchange = false;
		alpha_state = state.none;
		alpha_duringtime = 0f;
		alpha_current = 0f;
		alpha_exchange = false;
		clip_state = state.none;
		clip_duringtime = 0f;
		clip_current = Vector2.zero;
		clip_exchange = false;
	}

	public bool IsRuning()
	{
		return translate_state != 0 || scale_state != 0 || rotate_state != 0 || alpha_state != 0 || state.none != clip_state;
	}

	public void Start()
	{
		if (translate_have)
		{
			translate_state = state.doing;
		}
		if (scale_have)
		{
			scale_state = state.doing;
		}
		if (rotate_have)
		{
			rotate_state = state.doing;
		}
		if (alpha_have)
		{
			alpha_state = state.doing;
		}
		if (clip_have)
		{
			clip_state = state.doing;
		}
	}

	public void Stop()
	{
		translate_state = state.none;
		scale_state = state.none;
		rotate_state = state.none;
		alpha_state = state.none;
		clip_state = state.none;
	}

	public bool IsFinish()
	{
		return !IsRuning();
	}

	public bool IsTranslating()
	{
		return state.none != translate_state;
	}

	public Vector2 GetTranslate()
	{
		switch (translate_state)
		{
		case state.wait_end:
			translate_state = state.end;
			break;
		case state.end:
			if (translate_loop)
			{
				translate_state = state.doing;
				if (translate_reverse)
				{
					translate_offset = -translate_offset;
					translate_start = translate_current;
				}
			}
			else if (translate_restore)
			{
				if (translate_reverse)
				{
					translate_offset = -translate_offset;
					translate_start = translate_current;
					if (!translate_exchange)
					{
						translate_exchange = true;
						translate_state = state.doing;
					}
					else
					{
						translate_state = state.none;
					}
				}
				else
				{
					translate_state = state.none;
					translate_current = translate_start;
				}
			}
			else
			{
				translate_state = state.none;
			}
			translate_duringtime = 0f;
			break;
		}
		return translate_current;
	}

	public bool IsScaling()
	{
		return state.none != scale_state;
	}

	public float GetScale()
	{
		switch (scale_state)
		{
		case state.wait_end:
			scale_state = state.end;
			break;
		case state.end:
			if (scale_loop)
			{
				scale_state = state.doing;
				if (scale_reverse)
				{
					scale_range = 0f - scale_range;
					scale_start = scale_current;
				}
			}
			else if (scale_restore)
			{
				if (scale_reverse)
				{
					scale_range = 0f - scale_range;
					scale_start = scale_current;
					if (!scale_exchange)
					{
						scale_exchange = true;
						scale_state = state.doing;
					}
					else
					{
						scale_state = state.none;
					}
				}
				else
				{
					scale_state = state.none;
					scale_current = scale_start;
				}
			}
			else
			{
				scale_state = state.none;
			}
			scale_duringtime = 0f;
			break;
		}
		return scale_current;
	}

	public bool IsRotating()
	{
		return state.none != rotate_state;
	}

	public float GetRotate()
	{
		switch (rotate_state)
		{
		case state.wait_end:
			rotate_state = state.end;
			break;
		case state.end:
			if (rotate_loop)
			{
				rotate_state = state.doing;
				if (rotate_reverse)
				{
					rotate_angle = 0f - rotate_angle;
					rotate_start = rotate_current;
				}
			}
			else if (rotate_restore)
			{
				if (rotate_reverse)
				{
					rotate_angle = 0f - rotate_angle;
					rotate_start = rotate_current;
					if (!rotate_exchange)
					{
						rotate_exchange = true;
						rotate_state = state.doing;
					}
					else
					{
						rotate_state = state.none;
					}
				}
				else
				{
					rotate_state = state.none;
					rotate_current = rotate_start;
				}
			}
			else
			{
				rotate_state = state.none;
			}
			rotate_duringtime = 0f;
			break;
		}
		return rotate_current;
	}

	public float GetAlpha()
	{
		switch (alpha_state)
		{
		case state.wait_end:
			alpha_state = state.end;
			break;
		case state.end:
			if (alpha_loop)
			{
				alpha_state = state.doing;
				if (alpha_reverse)
				{
					alpha_range = 0f - alpha_range;
					alpha_start = alpha_current;
				}
			}
			else if (alpha_restore)
			{
				if (alpha_reverse)
				{
					alpha_range = 0f - alpha_range;
					alpha_start = alpha_current;
					if (!alpha_exchange)
					{
						alpha_exchange = true;
						alpha_state = state.doing;
					}
					else
					{
						alpha_state = state.none;
					}
				}
				else
				{
					alpha_state = state.none;
					alpha_current = alpha_start;
				}
			}
			else
			{
				alpha_state = state.none;
			}
			alpha_duringtime = 0f;
			break;
		}
		return alpha_current;
	}

	public bool IsAlphaing()
	{
		return state.none != alpha_state;
	}

	public Vector2 GetCliping()
	{
		switch (clip_state)
		{
		case state.wait_end:
			clip_state = state.end;
			break;
		case state.end:
			if (clip_loop)
			{
				clip_state = state.doing;
				if (clip_reverse)
				{
					clip_offset = -clip_offset;
					clip_start = clip_current;
				}
			}
			else if (clip_restore)
			{
				if (clip_reverse)
				{
					clip_offset = -clip_offset;
					clip_start = clip_current;
					if (!clip_exchange)
					{
						clip_exchange = true;
						clip_state = state.doing;
					}
					else
					{
						clip_state = state.none;
					}
				}
				else
				{
					clip_state = state.none;
					clip_current = clip_start;
				}
			}
			else
			{
				clip_state = state.none;
			}
			clip_duringtime = 0f;
			break;
		}
		return clip_current;
	}

	public bool IsCliping()
	{
		return state.none != clip_state;
	}

	public void Update(float delta_time)
	{
		if (translate_state == state.doing)
		{
			translate_duringtime += delta_time;
			if (translate_duringtime > translate_time)
			{
				translate_duringtime = translate_time;
				translate_state = state.wait_end;
			}
			translate_current = translate_duringtime / translate_time * translate_offset + translate_start;
		}
		if (scale_state == state.doing)
		{
			scale_duringtime += delta_time;
			if (scale_duringtime > scale_time)
			{
				scale_duringtime = scale_time;
				scale_state = state.wait_end;
			}
			scale_current = scale_duringtime / scale_time * scale_range + scale_start;
		}
		if (rotate_state == state.doing)
		{
			rotate_duringtime += delta_time;
			if (rotate_duringtime > rotate_time)
			{
				rotate_duringtime = rotate_time;
				rotate_state = state.wait_end;
			}
			rotate_current = rotate_duringtime / rotate_time * rotate_angle + rotate_start;
		}
		if (alpha_state == state.doing)
		{
			alpha_duringtime += delta_time;
			if (alpha_duringtime > alpha_time)
			{
				alpha_duringtime = alpha_time;
				alpha_state = state.wait_end;
			}
			alpha_current = alpha_duringtime / alpha_time * alpha_range + alpha_start;
		}
		if (clip_state == state.doing)
		{
			clip_duringtime += delta_time;
			if (clip_duringtime > clip_time)
			{
				clip_duringtime = clip_time;
				clip_state = state.wait_end;
			}
			clip_current = clip_duringtime / clip_time * clip_offset + clip_start;
		}
	}
}
