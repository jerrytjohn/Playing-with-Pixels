using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad_Canvas : MonoBehaviour 
{ 
	[HideInInspector] public Texture2D texture;
	private Renderer quad_renderer;
	private int pixel_width, pixel_height;

	//public Line line;
	[HideInInspector] public float angular_tolerance = 15f;

	public List <Nail> nails;

	private bool draw_toggle;

	// Properties
	public int Pixel_Width
	{
		get
		{
			return pixel_width;
		}
	}

	public int Pixel_Height
	{
		get
		{
			return pixel_height;
		}
	}
	
	void Start()
	{
		draw_toggle= false;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.L))
		{
			draw_toggle = !draw_toggle;
			
		}

		if (draw_toggle)
		{
			Draw_Line(line.nail_A ,line.nail_B, width, Color.red);
		}
		
	}

	public void Initialize(Texture2D new_Texture, float _width, float tolerance)
	{
		quad_renderer = GetComponent<Renderer>();

		Set_Texture(new_Texture);

		Scale_Canvas(_width);

		angular_tolerance = tolerance;

		Display_Texture();
	}

	public Line Find_Darkest_Line()
	{
		for(int i=0; i< nails.Count; i++)
		{
			for(int j = i+1; j< nails.Count; i++)
			{

			}
		}
	}
	public void Set_Texture(Texture2D _texture)
	{
		texture = _texture;

		pixel_width = texture.width;
		pixel_height = texture.height;
	}

	public void Display_Texture()
	{
		texture.Apply();
		quad_renderer.material.mainTexture = texture;
	}

	void Scale_Canvas(float frame_Width = 1f)
	{
		float scaling_ratio = (float) pixel_height/pixel_width;
		transform.localScale = new Vector3(frame_Width, frame_Width*scaling_ratio, 1f);
	}

	public Color Get_Pixel(int _x=0, int _y=0)
	{
		Color color = texture.GetPixel(_x,_y);
		return color;
	}

	public void Set_Pixel(int _x, int _y, Color _color)
	{
		Clamp_XY(ref _x, ref _y);
		texture.SetPixel(_x, _y, _color);
	}

	void Clamp_XY(ref int X, ref int Y)
	{
		if(X < 0)
		{
			X=0;
		}
		if(X >= texture.width)
		{
			X = texture.width -1;
		}

		if(Y < 0)
		{
			Y=0;
		}
		if(Y >= texture.height)
		{
			Y = texture.height -1;
		}
	}

	public void Update_Texture()
	{
		texture.Apply();
	}
	public void Draw_Line(Nail nail_A, Nail nail_B, float width, Color color)
	{
		Vector2 direction_AB = nail_B.pixel_pos - nail_A.pixel_pos;

		if(direction_AB.x >= direction_AB.y)	// Run >= rise
		{
			// Iterate in x and compute y

			Vector2 start_pos, end_pos;

			if(direction_AB.x > 0)
			{
				start_pos = nail_A.pixel_pos;
				end_pos = nail_B.pixel_pos;
			}
			else
			{
				// Reverse the vector
				start_pos = nail_B.pixel_pos;
				end_pos = nail_A.pixel_pos;

				direction_AB *= -1f;
			}

			float slope = direction_AB.y/direction_AB.x;
			float theta = Mathf.Atan2(direction_AB.y, direction_AB.x);

			for(float x = start_pos.x; x <= end_pos.x; x += 1f)
			{
				float dx = x - start_pos.x;
				float y_central = start_pos.y + slope*dx;

				float y_thickness = width/Mathf.Cos(theta);			// Because Trigonometry

				float y_low = y_central - (y_thickness/2f);
				float y_high = y_central + (y_thickness/2f);

				for(float y = y_low; y <= y_high; y += 1f)
				{
					texture.SetPixel((int) x,(int) y, color);
				}
			}
		}

		else
		{
			// Iterate in y and compute x

			Vector2 start_pos, end_pos;

			if(direction_AB.y > 0)
			{
				start_pos = nail_A.pixel_pos;
				end_pos = nail_B.pixel_pos;
			}
			else
			{
				// Reverse the vector
				start_pos = nail_B.pixel_pos;
				end_pos = nail_A.pixel_pos;

				direction_AB *= -1f;
			}

			float slope = direction_AB.x/direction_AB.y;
			float theta = Mathf.Atan2(direction_AB.x, direction_AB.y);

			for(float y= start_pos.y; y<= end_pos.y; y+=1f)
			{
				float dy = y-start_pos.y;
				float x_central = start_pos.x + slope*dy;

				float x_thickness = width/Mathf.Cos(theta);			// Because Trigonometry

				float x_low = x_central - (x_thickness/2f);
				float x_high = x_central + (x_thickness/2f);

				for(float x = x_low; x <= x_high; x += 1f)
				{
					texture.SetPixel((int) x,(int) y, color);
				}
			}
		}

		Update_Texture();
	}
	public void Analyze_Line(ref Line line)
	{

		Vector2 direction_AB = line.nail_A.pixel_pos - line.nail_A.pixel_pos;

		Color current_pixel_colour;
		float current_pixel_blackness;

		if(direction_AB.x >= direction_AB.y)	// Run >= rise
		{
			// Iterate in x and compute y

			Vector2 start_pos, end_pos;

			if(direction_AB.x > 0)
			{
				start_pos = line.nail_A.pixel_pos;
				end_pos = line.nail_B.pixel_pos;
			}
			else
			{
				// Reverse the vector
				start_pos = line.nail_B.pixel_pos;
				end_pos = line.nail_A.pixel_pos;

				direction_AB *= -1f;
			}

			float slope = direction_AB.y/direction_AB.x;
			float theta = Mathf.Atan2(direction_AB.y, direction_AB.x);

			for(float x = start_pos.x; x <= end_pos.x; x += 1f)
			{
				float dx = x - start_pos.x;
				float y_central = start_pos.y + slope*dx;

				float y_thickness = line.width_px/Mathf.Cos(theta);			// Because Trigonometry

				float y_low = y_central - (y_thickness/2f);
				float y_high = y_central + (y_thickness/2f);

				for(float y = y_low; y <= y_high; y += 1f)
				{
					current_pixel_colour = texture.GetPixel((int) x,(int) y);
					current_pixel_blackness = 1f-current_pixel_colour.r;
					line.total_blackness += current_pixel_blackness;
					line.pixel_Count++;
				}
			}
		}

		else
		{
			// Iterate in y and compute x

			Vector2 start_pos, end_pos;

			if(direction_AB.y > 0)
			{
				start_pos = line.nail_A.pixel_pos;
				end_pos = line.nail_B.pixel_pos;
			}
			else
			{
				// Reverse the vector
				start_pos = line.nail_B.pixel_pos;
				end_pos = line.nail_A.pixel_pos;

				direction_AB *= -1f;
			}

			float slope = direction_AB.x/direction_AB.y;
			float theta = Mathf.Atan2(direction_AB.x, direction_AB.y);

			for(float y= start_pos.y; y<= end_pos.y; y+=1f)
			{
				float dy = y-start_pos.y;
				float x_central = start_pos.x + slope*dy;

				float x_thickness = line.width_px/Mathf.Cos(theta);			// Because Trigonometry

				float x_low = x_central - (x_thickness/2f);
				float x_high = x_central + (x_thickness/2f);

				for(float x = x_low; x <= x_high; x += 1f)
				{
					current_pixel_colour = texture.GetPixel((int) x,(int) y);
					current_pixel_blackness = 1f-current_pixel_colour.r;
					line.total_blackness += current_pixel_blackness;
					line.pixel_Count++;
				}
			}
		}

		line.average_blackness = line.total_blackness/line.pixel_Count;
	}
}

[System.Serializable]
public class Line
{
	public Nail nail_A;
	public Nail nail_B;
	public float length_m;

	public float width_px;
	public int pixel_Count;
	public float total_blackness;
	public float average_blackness;

	public Line(Nail a, Nail b, float pixel_width=1f)
	{
		nail_A = a;
		nail_B = b;

		length_m = (b.world_pos - a.world_pos).magnitude;
		width_px = pixel_width;
		pixel_Count = 0;
		total_blackness = 0f;
		average_blackness = 0f;
	}
}
