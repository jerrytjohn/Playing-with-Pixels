using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image_Manager : MonoBehaviour 
{
	public float frame_Width_m = 1f;

	public float string_thickness_mm = 0.5f;

	[HideInInspector] public float string_thickness_px = 1f;

	public float breathing_room_degrees = 15f;
	public int nail_count = 100;
	public Texture2D input_image;

	public Quad_Canvas input_quad, output_quad;

	public GameObject nail;


	[HideInInspector] 
	public int pixel_width;

	[HideInInspector] 
	public int pixel_height; 

	public float pixels_per_metre
	{
		get
		{
			return pixel_width/frame_Width_m;
		}
	}

	public float metres_per_pixel
	{
		get
		{
			return frame_Width_m/pixel_width;
		}
	}

	public float Millimeters_to_Pixels(float thing_in_mm = 1f)
	{
		float thing_in_m = thing_in_mm/1000f;				// 1000mm = 1m; so 1mm = 1/1000 m; so N mm = N/1000 m
		float thing_in_px = pixels_per_metre * thing_in_m;
		return thing_in_px;
	}

	void Start()
	{
		Initialize();
	}

	void Initialize()
	{
		pixel_width = input_image.width;
		pixel_height = input_image.height;

		string_thickness_px = Millimeters_to_Pixels(string_thickness_mm);

		Texture2D incoming_texture = new Texture2D(pixel_width,pixel_height);
		Texture2D outgoing_texture = new Texture2D(pixel_width,pixel_height);

		Color pixel_color = Color.white;

		for(int y=0; y<pixel_height; y++)
		{
			for(int x=0; x<pixel_width; x++)
			{
				pixel_color = input_image.GetPixel(x,y);
				incoming_texture.SetPixel(x,y, pixel_color);

				pixel_color = Color.white;
				outgoing_texture.SetPixel(x,y,pixel_color);
			}
		}

		incoming_texture.filterMode = FilterMode.Point;
		outgoing_texture.filterMode = FilterMode.Point;

		//incoming_texture.Apply();
		//outgoing_texture.Apply();

		input_quad.Initialize(incoming_texture, frame_Width_m);
		output_quad.Initialize(outgoing_texture, frame_Width_m);

		Mark_Nails();
		
	}

	void Mark_Nails()
	{
		GameObject temp_Nail;
		Nail nail_script;
		Transform input_frame = transform.GetChild(0);
		Transform output_frame = transform.GetChild(1);
		float d_Theta = 2f*Mathf.PI/nail_count;

		for(int i=0; i<nail_count; i++)
		{
			// Input frame nail
			temp_Nail = Instantiate(nail);
			temp_Nail.transform.SetParent(input_frame);
			temp_Nail.transform.localPosition = 0.5f * Unit_Circle_Pos( i * d_Theta );				// 0.5f is the radius, i.e half width of the frame.
			nail_script = temp_Nail.GetComponent<Nail>();
			nail_script.index = i+1;
			nail_script.target_Quad = input_quad;
			input_quad.nails.Add(nail_script);
			temp_Nail.name = "Nail_"+ (i+1).ToString();
			nail_script.Refesh_Position_Info();


			// Output frame nail
			temp_Nail = Instantiate(nail);
			temp_Nail.transform.SetParent(output_frame);
			temp_Nail.transform.localPosition = 0.5f * Unit_Circle_Pos( i * d_Theta );				// 0.5f is the radius, i.e half width of the frame.
			nail_script = temp_Nail.GetComponent<Nail>();
			nail_script.index = i+1;
			nail_script.target_Quad = output_quad;
			input_quad.nails.Add(nail_script);
			temp_Nail.name = "Nail_"+ (i+1).ToString();
			nail_script.Refesh_Position_Info();
		}
	}

	Vector2 Unit_Circle_Pos(float theta = 0f)
	{
		Vector2 pos = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
		return pos;
	}
}
