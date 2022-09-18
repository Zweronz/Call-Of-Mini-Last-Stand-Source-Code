using System.Collections.Generic;
using UnityEngine;

namespace Prime31
{
	public class MonoBehaviourGUI : MonoBehaviour
	{
		protected float _width;

		protected float _buttonHeight;

		protected Dictionary<string, bool> _toggleButtons = new Dictionary<string, bool>();

		private bool isRetinaOrLargeScreen()
		{
			return Screen.width >= 960 || Screen.height >= 960;
		}

		protected void beginColumn()
		{
			_width = Screen.width / 2 - 15;
			_buttonHeight = ((!isRetinaOrLargeScreen()) ? 30 : 70);
			GUI.skin.button.margin = new RectOffset(0, 0, 10, 0);
			GUI.skin.button.stretchWidth = true;
			GUI.skin.button.fixedHeight = _buttonHeight;
			GUI.skin.button.wordWrap = false;
			GUILayout.BeginArea(new Rect(10f, 10f, _width, Screen.height));
			GUILayout.BeginVertical();
		}

		protected void endColumn()
		{
			endColumn(false);
		}

		protected void endColumn(bool hasSecondColumn)
		{
			GUILayout.EndVertical();
			GUILayout.EndArea();
			if (hasSecondColumn)
			{
				beginRightColumn();
			}
		}

		private void beginRightColumn()
		{
			GUILayout.BeginArea(new Rect((float)Screen.width - _width - 10f, 10f, _width, Screen.height));
			GUILayout.BeginVertical();
		}

		protected bool bottomRightButton(string text, float width = 150f)
		{
			return GUI.Button(new Rect((float)Screen.width - width - 10f, (float)Screen.height - _buttonHeight - 10f, width, _buttonHeight), text);
		}

		protected bool bottomLeftButton(string text, float width = 150f)
		{
			return GUI.Button(new Rect(10f, (float)Screen.height - _buttonHeight - 10f, width, _buttonHeight), text);
		}

		protected bool bottomCenterButton(string text, float width = 150f)
		{
			float left = (float)(Screen.width / 2) - width / 2f;
			return GUI.Button(new Rect(left, (float)Screen.height - _buttonHeight - 10f, width, _buttonHeight), text);
		}

		protected bool toggleButton(string defaultText, string selectedText)
		{
			if (!_toggleButtons.ContainsKey(defaultText))
			{
				_toggleButtons[defaultText] = true;
			}
			string text = ((!_toggleButtons[defaultText]) ? selectedText : defaultText);
			if (!_toggleButtons[defaultText])
			{
				GUI.skin.button.fontStyle = FontStyle.BoldAndItalic;
				GUI.contentColor = Color.yellow;
			}
			else
			{
				GUI.skin.button.fontStyle = FontStyle.Bold;
				GUI.contentColor = Color.red;
			}
			if (GUILayout.Button(text))
			{
				_toggleButtons[defaultText] = text != defaultText;
			}
			GUI.skin.button.fontStyle = FontStyle.Normal;
			GUI.contentColor = Color.white;
			return _toggleButtons[defaultText];
		}

		protected bool toggleButtonState(string defaultText)
		{
			if (!_toggleButtons.ContainsKey(defaultText))
			{
				_toggleButtons[defaultText] = true;
			}
			return _toggleButtons[defaultText];
		}
	}
}
