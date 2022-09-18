using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class MiscPlugin
{
	public static void TakePhoto(string photo_key)
	{
		try
		{
			Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			texture2D.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
			texture2D.Apply();
			byte[] buffer = texture2D.EncodeToPNG();
			string path = Utils.SavePath() + "/Avatar/" + photo_key + "_photo.png";
			FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write(buffer);
			binaryWriter.Close();
			fileStream.Close();
			Object.Destroy(texture2D);
		}
		catch
		{
			Debug.Log("AvataTakePhotoWin32 error");
		}
	}

	public static bool CheckOSIsIAPCrack()
	{
		return false;
	}

	public static bool CheckOSIsJailbreak()
	{
		return false;
	}

	public static void ToSendMail(string address, string subject, string content)
	{
	}

	public static int ShowMessageBox1(string title, string message, string button)
	{
		return 0;
	}

	public static int ShowMessageBox2(string title, string message, string button1, string button2)
	{
		return 0;
	}

	[DllImport("__Internal")]
	protected static extern void ShowIndicator(int style, bool iPad, float r, float g, float b, float a);

	public static void ShowIndicatorSystem(int style, bool iPad, float r, float g, float b, float a)
	{
		ShowIndicator(style, iPad, r, g, b, a);
	}

	[DllImport("__Internal")]
	protected static extern void HideIndicator();

	public static void HideIndicatorSystem()
	{
		HideIndicator();
	}

	public static string GetMacAddr()
	{
		return "000000000000";
	}
}
