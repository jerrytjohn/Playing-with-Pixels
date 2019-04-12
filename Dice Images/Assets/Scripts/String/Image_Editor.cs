using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image_Editor : MonoBehaviour 
{
	public float frame_Width = 1f;
	public Texture2D target_texture;


	[HideInInspector] 
	public int pixel_width;

	[HideInInspector] 
	public int pixel_height; 

	private Renderer renderer;

	void Start()
	{
		Initialize();
	}

	void Initialize()
	{
		pixel_width = target_texture.width;
		pixel_height = target_texture.height;

		renderer = GetComponent<Renderer>();

		Scale_Canvas();
		Set_Texture(target_texture);
	}

	void Scale_Canvas()
	{
		float scaling_ratio = (float) pixel_height/pixel_width;
		transform.localScale = new Vector3(frame_Width, frame_Width*scaling_ratio, 1f);
	}

	void Set_Texture(Texture2D texture)
	{
		renderer.material.mainTexture = texture;
	}
}
