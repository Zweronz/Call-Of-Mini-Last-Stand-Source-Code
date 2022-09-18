using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Fuild")]
public class ImageEffectTest : ImageEffectBase
{
	protected new void Start()
	{
		if (!SystemInfo.supportsRenderTextures)
		{
			base.enabled = false;
		}
		else
		{
			base.Start();
		}
	}

	protected new void OnDisable()
	{
		base.OnDisable();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, base.material);
	}
}
