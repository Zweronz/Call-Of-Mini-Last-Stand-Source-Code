using System;
using System.Collections;
using System.Xml;
using UnityEngine;

public class UIHelper : MonoBehaviour, UIHandler
{
	public string m_ui_cfgxml;

	public string m_ui_material_path;

	public string m_font_path;

	public UIManager m_UIManagerRef;

	public Hashtable m_control_table;

	public int cur_control_id;

	public Hashtable m_animations;

	public int GetControlId(string name)
	{
		return ((UIControl)m_control_table[name]).Id;
	}

	public void OnGUI()
	{
	}

	public void Start()
	{
		GameObject gameObject = new GameObject("UIManager");
		m_UIManagerRef = gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManagerRef.SetParameter(8, 1, false);
		m_UIManagerRef.SetUIHandler(this);
		m_control_table = new Hashtable();
		m_animations = new Hashtable();
		int num = 1;
		if (Utils.IsRetina())
		{
			num = 2;
		}
		cur_control_id = 0;
		XmlElement xmlElement = null;
		string empty = string.Empty;
		TextAsset textAsset = Resources.Load(m_ui_cfgxml) as TextAsset;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("UIElem" == childNode.Name)
			{
				foreach (XmlNode childNode2 in childNode.ChildNodes)
				{
					xmlElement = (XmlElement)childNode2;
					if ("UIButton" == childNode2.Name)
					{
						UIButtonBase uIButtonBase = null;
						Vector2 size = Vector2.zero;
						Rect rect = new Rect(0f, 0f, 0f, 0f);
						string[] array;
						if (Utils.IsRetina())
						{
							empty = xmlElement.GetAttribute("adjustrect").Trim();
							if (empty.Length > 0)
							{
								array = empty.Split(',');
								rect = new Rect(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()), int.Parse(array[3].Trim()));
							}
						}
						empty = xmlElement.GetAttribute("rect").Trim();
						array = empty.Split(',');
						empty = xmlElement.GetAttribute("type").Trim();
						if ("click" == empty)
						{
							uIButtonBase = new UIClickButton();
							((UIClickButton)uIButtonBase).SetRect(new Rect((float)(int.Parse(array[0].Trim()) * num) + rect.x, (float)(int.Parse(array[1].Trim()) * num) + rect.y, (float)(int.Parse(array[2].Trim()) * num) + rect.width, (float)(int.Parse(array[3].Trim()) * num) + rect.height));
						}
						else if ("push" == empty)
						{
							uIButtonBase = new UIPushButton();
							((UIPushButton)uIButtonBase).Rect = new Rect((float)(int.Parse(array[0].Trim()) * num) + rect.x, (float)(int.Parse(array[1].Trim()) * num) + rect.y, (float)(int.Parse(array[2].Trim()) * num) + rect.width, (float)(int.Parse(array[3].Trim()) * num) + rect.height);
						}
						else if ("select" == empty)
						{
							uIButtonBase = new UISelectButton();
							((UISelectButton)uIButtonBase).Rect = new Rect((float)(int.Parse(array[0].Trim()) * num) + rect.x, (float)(int.Parse(array[1].Trim()) * num) + rect.y, (float)(int.Parse(array[2].Trim()) * num) + rect.width, (float)(int.Parse(array[3].Trim()) * num) + rect.height);
						}
						else if ("wheel" == empty)
						{
							uIButtonBase = new UIWheelButton();
							((UIWheelButton)uIButtonBase).Rect = new Rect((float)(int.Parse(array[0].Trim()) * num) + rect.x, (float)(int.Parse(array[1].Trim()) * num) + rect.y, (float)(int.Parse(array[2].Trim()) * num) + rect.width, (float)(int.Parse(array[3].Trim()) * num) + rect.height);
						}
						else if ("joystick" == empty)
						{
							uIButtonBase = new UIJoystickButton();
							((UIJoystickButton)uIButtonBase).Rect = new Rect((float)(int.Parse(array[0].Trim()) * num) + rect.x, (float)(int.Parse(array[1].Trim()) * num) + rect.y, (float)(int.Parse(array[2].Trim()) * num) + rect.width, (float)(int.Parse(array[3].Trim()) * num) + rect.height);
						}
						if (uIButtonBase == null)
						{
							continue;
						}
						size = new Vector2(uIButtonBase.Rect.width, uIButtonBase.Rect.height);
						empty = xmlElement.GetAttribute("name").Trim();
						m_control_table.Add(empty, uIButtonBase);
						uIButtonBase.Id = cur_control_id;
						empty = xmlElement.GetAttribute("enable").Trim();
						if (empty.Length > 1)
						{
							uIButtonBase.Enable = "true" == empty;
						}
						empty = xmlElement.GetAttribute("visible").Trim();
						if (empty.Length > 1)
						{
							uIButtonBase.Visible = "true" == empty;
						}
						empty = xmlElement.GetAttribute("alpha").Trim();
						if (empty.Length > 1)
						{
							uIButtonBase.SetAlpha(float.Parse(empty));
						}
						empty = xmlElement.GetAttribute("layer").Trim();
						if (empty.Length > 0)
						{
							uIButtonBase.Layer = int.Parse(empty);
						}
						xmlElement = (XmlElement)childNode2.SelectSingleNode("Normal");
						if (xmlElement != null)
						{
							empty = xmlElement.GetAttribute("rect").Trim();
							array = empty.Split(',');
							empty = xmlElement.GetAttribute("material").Trim();
							if (num == 2)
							{
								empty += "_hd";
							}
							uIButtonBase.SetTexture(UIButtonBase.State.Normal, LoadUIMaterial(empty), new Rect(int.Parse(array[0].Trim()) * num, int.Parse(array[1].Trim()) * num, int.Parse(array[2].Trim()) * num, int.Parse(array[3].Trim()) * num), size);
						}
						xmlElement = (XmlElement)childNode2.SelectSingleNode("Press");
						if (xmlElement != null)
						{
							empty = xmlElement.GetAttribute("rect").Trim();
							array = empty.Split(',');
							empty = xmlElement.GetAttribute("material").Trim();
							if (num == 2)
							{
								empty += "_hd";
							}
							uIButtonBase.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterial(empty), new Rect(int.Parse(array[0].Trim()) * num, int.Parse(array[1].Trim()) * num, int.Parse(array[2].Trim()) * num, int.Parse(array[3].Trim()) * num), size);
						}
						xmlElement = (XmlElement)childNode2.SelectSingleNode("Disable");
						if (xmlElement != null)
						{
							empty = xmlElement.GetAttribute("rect").Trim();
							array = empty.Split(',');
							empty = xmlElement.GetAttribute("material").Trim();
							if (num == 2)
							{
								empty += "_hd";
							}
							uIButtonBase.SetTexture(UIButtonBase.State.Disabled, LoadUIMaterial(empty), new Rect(int.Parse(array[0].Trim()) * num, int.Parse(array[1].Trim()) * num, int.Parse(array[2].Trim()) * num, int.Parse(array[3].Trim()) * num), size);
						}
						xmlElement = (XmlElement)childNode2.SelectSingleNode("Hover");
						if (xmlElement != null)
						{
							empty = xmlElement.GetAttribute("rect").Trim();
							array = empty.Split(',');
							empty = xmlElement.GetAttribute("material").Trim();
							if (num == 2)
							{
								empty += "_hd";
							}
							uIButtonBase.SetHoverSprite(LoadUIMaterial(empty), new Rect(int.Parse(array[0].Trim()) * num, int.Parse(array[1].Trim()) * num, int.Parse(array[2].Trim()) * num, int.Parse(array[3].Trim()) * num));
						}
						m_UIManagerRef.Add(uIButtonBase);
					}
					else if ("UIImage" == childNode2.Name)
					{
						UIImage uIImage = new UIImage();
						empty = xmlElement.GetAttribute("name").Trim();
						m_control_table.Add(empty, uIImage);
						uIImage.Id = cur_control_id;
						Rect rect2 = new Rect(0f, 0f, 0f, 0f);
						string[] array2;
						if (Utils.IsRetina())
						{
							empty = xmlElement.GetAttribute("adjustrect").Trim();
							if (empty.Length > 0)
							{
								array2 = empty.Split(',');
								rect2 = new Rect(int.Parse(array2[0].Trim()), int.Parse(array2[1].Trim()), int.Parse(array2[2].Trim()), int.Parse(array2[3].Trim()));
							}
						}
						empty = xmlElement.GetAttribute("rect").Trim();
						array2 = empty.Split(',');
						uIImage.SetRect(new Rect((float)(int.Parse(array2[0].Trim()) * num) + rect2.x, (float)(int.Parse(array2[1].Trim()) * num) + rect2.y, (float)(int.Parse(array2[2].Trim()) * num) + rect2.width, (float)(int.Parse(array2[3].Trim()) * num) + rect2.height));
						empty = xmlElement.GetAttribute("enable").Trim();
						if (empty.Length > 0)
						{
							uIImage.Enable = "true" == empty;
						}
						empty = xmlElement.GetAttribute("visible").Trim();
						if (empty.Length > 0)
						{
							uIImage.Visible = "true" == empty;
						}
						empty = xmlElement.GetAttribute("alpha").Trim();
						if (empty.Length > 0)
						{
							uIImage.SetAlpha(float.Parse(empty));
						}
						empty = xmlElement.GetAttribute("rotate").Trim();
						if (empty.Length > 0)
						{
							uIImage.SetRotation(float.Parse(empty) * ((float)Math.PI / 180f));
						}
						empty = xmlElement.GetAttribute("clip").Trim();
						if (empty.Length > 0)
						{
							array2 = empty.Split(',');
							uIImage.SetClip(new Rect((float)(int.Parse(array2[0].Trim()) * num) + rect2.x, (float)(int.Parse(array2[1].Trim()) * num) + rect2.y, (float)(int.Parse(array2[2].Trim()) * num) + rect2.width, (float)(int.Parse(array2[3].Trim()) * num) + rect2.height));
						}
						empty = xmlElement.GetAttribute("layer").Trim();
						if (empty.Length > 0)
						{
							uIImage.Layer = int.Parse(empty);
						}
						xmlElement = (XmlElement)childNode2.SelectSingleNode("Texture");
						if (xmlElement != null)
						{
							empty = xmlElement.GetAttribute("rect").Trim();
							array2 = empty.Split(',');
							empty = xmlElement.GetAttribute("material").Trim();
							if (num == 2)
							{
								empty += "_hd";
							}
							Rect texture_rect = new Rect(int.Parse(array2[0].Trim()) * num, int.Parse(array2[1].Trim()) * num, int.Parse(array2[2].Trim()) * num, int.Parse(array2[3].Trim()) * num);
							Vector2 size2 = new Vector2(uIImage.Rect.width, uIImage.Rect.height);
							uIImage.SetTexture(LoadUIMaterial(empty), texture_rect, size2);
						}
						m_UIManagerRef.Add(uIImage);
					}
					else if ("UIText" == childNode2.Name)
					{
						UIText uIText = new UIText();
						empty = xmlElement.GetAttribute("name").Trim();
						m_control_table.Add(empty, uIText);
						uIText.Id = cur_control_id;
						Rect rect3 = new Rect(0f, 0f, 0f, 0f);
						string[] array3;
						if (Utils.IsRetina())
						{
							empty = xmlElement.GetAttribute("adjustrect").Trim();
							if (empty.Length > 0)
							{
								array3 = empty.Split(',');
								rect3 = new Rect(int.Parse(array3[0].Trim()), int.Parse(array3[1].Trim()), int.Parse(array3[2].Trim()), int.Parse(array3[3].Trim()));
							}
						}
						empty = xmlElement.GetAttribute("rect").Trim();
						array3 = empty.Split(',');
						uIText.SetRect(new Rect((float)(int.Parse(array3[0].Trim()) * num) + rect3.x, (float)(int.Parse(array3[1].Trim()) * num) + rect3.y, (float)(int.Parse(array3[2].Trim()) * num) + rect3.width, (float)(int.Parse(array3[3].Trim()) * num) + rect3.height));
						empty = xmlElement.GetAttribute("chargap").Trim();
						if (empty.Length > 0)
						{
							uIText.CharacterSpacing = int.Parse(empty) * num;
						}
						empty = xmlElement.GetAttribute("linegap").Trim();
						if (empty.Length > 0)
						{
							uIText.LineSpacing = int.Parse(empty) * num;
						}
						empty = xmlElement.GetAttribute("autoline").Trim();
						if (empty.Length > 0)
						{
							uIText.AutoLine = "true" == empty;
						}
						empty = xmlElement.GetAttribute("align").Trim();
						if (empty.Length > 0)
						{
							uIText.AlignStyle = (UIText.enAlignStyle)(int)Enum.Parse(typeof(UIText.enAlignStyle), empty);
						}
						empty = xmlElement.GetAttribute("enable").Trim();
						if (empty.Length > 0)
						{
							uIText.Enable = "true" == empty;
						}
						empty = xmlElement.GetAttribute("visible").Trim();
						if (empty.Length > 0)
						{
							uIText.Visible = "true" == empty;
						}
						uIText.SetFont(string.Concat(str1: (num != 2) ? xmlElement.GetAttribute("font").Trim() : xmlElement.GetAttribute("fontHD").Trim(), str0: m_font_path));
						empty = xmlElement.GetAttribute("color").Trim();
						if (empty.Length > 0)
						{
							array3 = empty.Split(',');
							uIText.SetColor(new Color((float)int.Parse(array3[0].Trim()) / 255f, (float)int.Parse(array3[1].Trim()) / 255f, (float)int.Parse(array3[2].Trim()) / 255f, (float)int.Parse(array3[3].Trim()) / 255f));
						}
						uIText.SetText(childNode2.InnerText.Trim(' ', '\t', '\r', '\n'));
						empty = xmlElement.GetAttribute("text");
						if (empty.Length > 0)
						{
							uIText.SetText(empty);
						}
						empty = xmlElement.GetAttribute("layer").Trim();
						if (empty.Length > 0)
						{
							uIText.Layer = int.Parse(empty);
						}
						m_UIManagerRef.Add(uIText);
					}
					else if ("UIBlock" == childNode2.Name)
					{
						UIBlock uIBlock = new UIBlock();
						empty = xmlElement.GetAttribute("name").Trim();
						m_control_table.Add(empty, uIBlock);
						uIBlock.Id = cur_control_id;
						Rect rect4 = new Rect(0f, 0f, 0f, 0f);
						string[] array4;
						if (Utils.IsRetina())
						{
							empty = xmlElement.GetAttribute("adjustrect").Trim();
							if (empty.Length > 0)
							{
								array4 = empty.Split(',');
								rect4 = new Rect(int.Parse(array4[0].Trim()), int.Parse(array4[1].Trim()), int.Parse(array4[2].Trim()), int.Parse(array4[3].Trim()));
							}
						}
						empty = xmlElement.GetAttribute("rect").Trim();
						array4 = empty.Split(',');
						uIBlock.Rect = new Rect((float)(int.Parse(array4[0].Trim()) * num) + rect4.x, (float)(int.Parse(array4[1].Trim()) * num) + rect4.y, (float)(int.Parse(array4[2].Trim()) * num) + rect4.width, (float)(int.Parse(array4[3].Trim()) * num) + rect4.height);
						empty = xmlElement.GetAttribute("enable").Trim();
						if (empty.Length > 1)
						{
							uIBlock.Enable = "true" == empty;
						}
						empty = xmlElement.GetAttribute("layer").Trim();
						if (empty.Length > 0)
						{
							uIBlock.Layer = int.Parse(empty);
						}
						m_UIManagerRef.Add(uIBlock);
					}
					else if ("UIMove" == childNode2.Name)
					{
						UIMove uIMove = new UIMove();
						empty = xmlElement.GetAttribute("name").Trim();
						m_control_table.Add(empty, uIMove);
						uIMove.Id = cur_control_id;
						Rect rect5 = new Rect(0f, 0f, 0f, 0f);
						string[] array5;
						if (Utils.IsRetina())
						{
							empty = xmlElement.GetAttribute("adjustrect").Trim();
							if (empty.Length > 0)
							{
								array5 = empty.Split(',');
								rect5 = new Rect(int.Parse(array5[0].Trim()), int.Parse(array5[1].Trim()), int.Parse(array5[2].Trim()), int.Parse(array5[3].Trim()));
							}
						}
						empty = xmlElement.GetAttribute("rect").Trim();
						array5 = empty.Split(',');
						uIMove.Rect = new Rect((float)(int.Parse(array5[0].Trim()) * num) + rect5.x, (float)(int.Parse(array5[1].Trim()) * num) + rect5.y, (float)(int.Parse(array5[2].Trim()) * num) + rect5.width, (float)(int.Parse(array5[3].Trim()) * num) + rect5.height);
						empty = xmlElement.GetAttribute("enable").Trim();
						if (empty.Length > 1)
						{
							uIMove.Enable = "true" == empty;
						}
						empty = xmlElement.GetAttribute("layer").Trim();
						if (empty.Length > 0)
						{
							uIMove.Layer = int.Parse(empty);
						}
						m_UIManagerRef.Add(uIMove);
					}
					cur_control_id++;
				}
			}
			else
			{
				if (!("UIAnimation" == childNode.Name))
				{
					continue;
				}
				foreach (XmlNode childNode3 in childNode.ChildNodes)
				{
					xmlElement = (XmlElement)childNode3;
					if ("Animation" != childNode3.Name)
					{
						continue;
					}
					UIAnimations uIAnimations = new UIAnimations();
					empty = xmlElement.GetAttribute("name").Trim();
					uIAnimations.animation_name = empty;
					empty = xmlElement.GetAttribute("control_id").Trim();
					string[] array6 = empty.Split(',');
					for (int i = 0; i < array6.Length; i++)
					{
						UIAnimations.ControlData controlData = new UIAnimations.ControlData();
						controlData.control_name = array6[i].Trim();
						uIAnimations.control_data.Add(controlData);
					}
					xmlElement = (XmlElement)childNode3.SelectSingleNode("Translate");
					if (xmlElement != null)
					{
						uIAnimations.translate_have = true;
						empty = xmlElement.GetAttribute("time").Trim();
						uIAnimations.translate_time = float.Parse(empty);
						empty = xmlElement.GetAttribute("offset").Trim();
						if (empty.Length > 0)
						{
							array6 = empty.Split(',');
							uIAnimations.translate_offset.x = int.Parse(array6[0].Trim()) * num;
							uIAnimations.translate_offset.y = int.Parse(array6[1].Trim()) * num;
						}
						empty = xmlElement.GetAttribute("restore").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.translate_restore = "true" == empty;
						}
						empty = xmlElement.GetAttribute("loop").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.translate_loop = "true" == empty;
						}
						empty = xmlElement.GetAttribute("reverse").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.translate_reverse = "true" == empty;
						}
					}
					xmlElement = (XmlElement)childNode3.SelectSingleNode("Scale");
					if (xmlElement != null)
					{
						uIAnimations.scale_have = true;
						empty = xmlElement.GetAttribute("time").Trim();
						uIAnimations.scale_time = float.Parse(empty);
						empty = xmlElement.GetAttribute("range").Trim();
						uIAnimations.scale_range = float.Parse(empty);
						empty = xmlElement.GetAttribute("restore").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.scale_restore = "true" == empty;
						}
						empty = xmlElement.GetAttribute("loop").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.scale_loop = "true" == empty;
						}
						empty = xmlElement.GetAttribute("reverse").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.scale_reverse = "true" == empty;
						}
					}
					xmlElement = (XmlElement)childNode3.SelectSingleNode("Rotate");
					if (xmlElement != null)
					{
						uIAnimations.rotate_have = true;
						empty = xmlElement.GetAttribute("time").Trim();
						uIAnimations.rotate_time = float.Parse(empty);
						empty = xmlElement.GetAttribute("angle").Trim();
						uIAnimations.rotate_angle = (float)Math.PI / 180f * float.Parse(empty);
						empty = xmlElement.GetAttribute("restore").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.rotate_restore = "true" == empty;
						}
						empty = xmlElement.GetAttribute("loop").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.rotate_loop = "true" == empty;
						}
						empty = xmlElement.GetAttribute("reverse").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.rotate_reverse = "true" == empty;
						}
					}
					xmlElement = (XmlElement)childNode3.SelectSingleNode("Alpha");
					if (xmlElement != null)
					{
						uIAnimations.alpha_have = true;
						empty = xmlElement.GetAttribute("time").Trim();
						uIAnimations.alpha_time = float.Parse(empty);
						empty = xmlElement.GetAttribute("range").Trim();
						uIAnimations.alpha_range = float.Parse(empty);
						empty = xmlElement.GetAttribute("restore").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.alpha_restore = "true" == empty;
						}
						empty = xmlElement.GetAttribute("loop").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.alpha_loop = "true" == empty;
						}
						empty = xmlElement.GetAttribute("reverse").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.alpha_reverse = "true" == empty;
						}
					}
					xmlElement = (XmlElement)childNode3.SelectSingleNode("Cliping");
					if (xmlElement != null)
					{
						uIAnimations.clip_have = true;
						empty = xmlElement.GetAttribute("time").Trim();
						uIAnimations.clip_time = float.Parse(empty);
						empty = xmlElement.GetAttribute("offset").Trim();
						if (empty.Length > 0)
						{
							array6 = empty.Split(',');
							uIAnimations.clip_offset.x = int.Parse(array6[0].Trim()) * num;
							uIAnimations.clip_offset.y = int.Parse(array6[1].Trim()) * num;
						}
						empty = xmlElement.GetAttribute("restore").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.clip_restore = "true" == empty;
						}
						empty = xmlElement.GetAttribute("loop").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.clip_loop = "true" == empty;
						}
						empty = xmlElement.GetAttribute("reverse").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.clip_reverse = "true" == empty;
						}
					}
					m_animations.Add(uIAnimations.animation_name, uIAnimations);
				}
			}
		}
	}

	public void Update()
	{
		foreach (UIAnimations value in m_animations.Values)
		{
			if (!value.IsRuning())
			{
				continue;
			}
			value.Update(Time.deltaTime);
			bool flag = false;
			Vector2 vector = new Vector2(0f, 0f);
			if (value.IsTranslating())
			{
				vector = value.GetTranslate();
				flag = true;
			}
			bool flag2 = false;
			float num = 0f;
			if (value.IsScaling())
			{
				num = value.GetScale();
				flag2 = true;
			}
			bool flag3 = false;
			float num2 = 0f;
			if (value.IsRotating())
			{
				num2 = value.GetRotate();
				flag3 = true;
			}
			bool flag4 = false;
			float num3 = 0f;
			if (value.IsAlphaing())
			{
				num3 = value.GetAlpha();
				flag4 = true;
			}
			bool flag5 = false;
			Vector2 vector2 = Vector2.zero;
			if (value.IsCliping())
			{
				vector2 = value.GetCliping();
				flag5 = true;
			}
			for (int i = 0; i < value.control_data.Count; i++)
			{
				UIAnimations.ControlData controlData = (UIAnimations.ControlData)value.control_data[i];
				string control_name = controlData.control_name;
				string text = m_control_table[control_name].GetType().ToString();
				if ("UIClickButton" == text)
				{
					UIClickButton uIClickButton = (UIClickButton)m_control_table[control_name];
					if (flag)
					{
						uIClickButton.Rect = new Rect(vector.x + controlData.pos.x, vector.y + controlData.pos.y, uIClickButton.Rect.width, uIClickButton.Rect.height);
					}
					if (flag3)
					{
						uIClickButton.SetRotate(num2);
					}
					if (flag4)
					{
						uIClickButton.SetAlpha(controlData.alpha + num3);
					}
				}
				else if ("UIPushButton" == text)
				{
					UIPushButton uIPushButton = (UIPushButton)m_control_table[control_name];
					if (flag)
					{
						uIPushButton.Rect = new Rect(vector.x + controlData.pos.x, vector.y + controlData.pos.y, uIPushButton.Rect.width, uIPushButton.Rect.height);
					}
					if (flag3)
					{
						uIPushButton.SetRotate(num2);
					}
					if (flag4)
					{
						uIPushButton.SetAlpha(controlData.alpha + num3);
					}
				}
				else if ("UISelectButton" == text)
				{
					UISelectButton uISelectButton = (UISelectButton)m_control_table[control_name];
					if (flag)
					{
						uISelectButton.Rect = new Rect(vector.x + controlData.pos.x, vector.y + controlData.pos.y, uISelectButton.Rect.width, uISelectButton.Rect.height);
					}
					if (flag3)
					{
						uISelectButton.SetRotate(num2);
					}
					if (flag4)
					{
						uISelectButton.SetAlpha(controlData.alpha + num3);
					}
				}
				else if ("UIWheelButton" == text)
				{
					UIWheelButton uIWheelButton = (UIWheelButton)m_control_table[control_name];
					if (flag)
					{
						uIWheelButton.Rect = new Rect(vector.x + controlData.pos.x, vector.y + controlData.pos.y, uIWheelButton.Rect.width, uIWheelButton.Rect.height);
					}
					if (flag4)
					{
						uIWheelButton.SetAlpha(controlData.alpha + num3);
					}
				}
				else if ("UIJoystickButton" == text)
				{
					UIJoystickButton uIJoystickButton = (UIJoystickButton)m_control_table[control_name];
					if (flag)
					{
						uIJoystickButton.Rect = new Rect(vector.x + controlData.pos.x, vector.y + controlData.pos.y, uIJoystickButton.Rect.width, uIJoystickButton.Rect.height);
					}
					if (flag4)
					{
						uIJoystickButton.SetAlpha(controlData.alpha + num3);
					}
				}
				else if ("UIImage" == text)
				{
					UIImage uIImage = (UIImage)m_control_table[control_name];
					if (flag)
					{
						uIImage.SetPosition(controlData.pos + vector);
					}
					if (flag2)
					{
						uIImage.SetScale(controlData.scale + num);
					}
					if (flag3)
					{
						uIImage.SetRotation(num2);
					}
					if (flag4)
					{
						uIImage.SetAlpha(controlData.alpha + num3);
					}
					if (flag5)
					{
						uIImage.SetClip(new Rect(uIImage.Rect.x, uIImage.Rect.y, vector2.x + controlData.clip.x, vector2.y + controlData.clip.y));
					}
				}
				else if ("UIText" == text)
				{
					UIText uIText = (UIText)m_control_table[control_name];
					if (flag)
					{
						uIText.Rect = new Rect(vector.x + controlData.pos.x, vector.y + controlData.pos.y, uIText.Rect.width, uIText.Rect.height);
					}
					if (flag4)
					{
						uIText.SetAlpha(controlData.alpha + num3);
					}
				}
			}
			if (value.IsFinish())
			{
				OnAnimationFinished(value.animation_name);
			}
		}
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		OnHandleEvent(control, command, wparam, lparam);
	}

	public virtual void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
	}

	public virtual void OnAnimationFinished(string name)
	{
	}

	public void StartAnimation(string name)
	{
		UIAnimations uIAnimations = (UIAnimations)m_animations[name];
		for (int i = 0; i < uIAnimations.control_data.Count; i++)
		{
			UIAnimations.ControlData controlData = (UIAnimations.ControlData)uIAnimations.control_data[i];
			string control_name = controlData.control_name;
			string text = m_control_table[control_name].GetType().ToString();
			if ("UIClickButton" == text)
			{
				UIClickButton uIClickButton = (UIClickButton)m_control_table[control_name];
				controlData.pos.x = uIClickButton.Rect.x;
				controlData.pos.y = uIClickButton.Rect.y;
				controlData.angle = uIClickButton.GetRotate();
				controlData.alpha = uIClickButton.GetAlpha();
				controlData.clip.x = uIClickButton.Clip.width;
				controlData.clip.y = uIClickButton.Clip.height;
			}
			else if ("UIPushButton" == text)
			{
				UIPushButton uIPushButton = (UIPushButton)m_control_table[control_name];
				controlData.pos.x = uIPushButton.Rect.x;
				controlData.pos.y = uIPushButton.Rect.y;
				controlData.angle = uIPushButton.GetRotate();
				controlData.alpha = uIPushButton.GetAlpha();
				controlData.clip.x = uIPushButton.Clip.width;
				controlData.clip.y = uIPushButton.Clip.height;
			}
			else if ("UISelectButton" == text)
			{
				UISelectButton uISelectButton = (UISelectButton)m_control_table[control_name];
				controlData.pos.x = uISelectButton.Rect.x;
				controlData.pos.y = uISelectButton.Rect.y;
				controlData.angle = uISelectButton.GetRotate();
				controlData.alpha = uISelectButton.GetAlpha();
				controlData.clip.x = uISelectButton.Clip.width;
				controlData.clip.y = uISelectButton.Clip.height;
			}
			else if ("UIWheelButton" == text)
			{
				UIWheelButton uIWheelButton = (UIWheelButton)m_control_table[control_name];
				controlData.pos.x = uIWheelButton.Rect.x;
				controlData.pos.y = uIWheelButton.Rect.y;
				controlData.alpha = uIWheelButton.GetAlpha();
				controlData.clip.x = uIWheelButton.Clip.width;
				controlData.clip.y = uIWheelButton.Clip.height;
			}
			else if ("UIJoystickButton" == text)
			{
				UIJoystickButton uIJoystickButton = (UIJoystickButton)m_control_table[control_name];
				controlData.pos.x = uIJoystickButton.Rect.x;
				controlData.pos.y = uIJoystickButton.Rect.y;
				controlData.alpha = uIJoystickButton.GetAlpha();
				controlData.clip.x = uIJoystickButton.Clip.width;
				controlData.clip.y = uIJoystickButton.Clip.height;
			}
			else if ("UIImage" == text)
			{
				UIImage uIImage = (UIImage)m_control_table[control_name];
				controlData.pos = uIImage.GetPosition();
				controlData.angle = uIImage.GetRotation();
				controlData.alpha = uIImage.GetAlpha();
				controlData.scale = uIImage.GetScale(0);
				controlData.clip.x = uIImage.Clip.width;
				controlData.clip.y = uIImage.Clip.height;
			}
			else if ("UIText" == text)
			{
				UIText uIText = (UIText)m_control_table[control_name];
				controlData.pos.x = uIText.Rect.x;
				controlData.pos.y = uIText.Rect.y;
				controlData.alpha = uIText.GetAlpha();
				controlData.clip.x = uIText.Clip.width;
				controlData.clip.y = uIText.Clip.height;
			}
		}
		uIAnimations.Reset();
		uIAnimations.Start();
	}

	public void StopAnimation(string name)
	{
		if (name != null && m_animations.ContainsKey(name))
		{
			UIAnimations uIAnimations = (UIAnimations)m_animations[name];
			uIAnimations.Stop();
		}
	}

	public Material LoadUIMaterial(string name)
	{
		string text = m_ui_material_path + name + "_M";
		Material material = Resources.Load(text) as Material;
		if (material == null)
		{
			Debug.Log("load material error: " + text);
		}
		return material;
	}

	public Material LoadMaterial(string name)
	{
		Material material = Resources.Load(name) as Material;
		if (material == null)
		{
			Debug.Log("load material error: " + name);
		}
		return material;
	}
}
