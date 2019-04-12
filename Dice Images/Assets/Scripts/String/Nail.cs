
using UnityEngine;

public class Nail : MonoBehaviour 
{
	public Quad_Canvas target_Quad;
	//[HideInInspector] 
	public int index;
	//[HideInInspector] 
	public Vector2 pixel_pos;
	[HideInInspector] public Vector2 world_pos;

	public void Refesh_Position_Info()
	{
		pixel_pos = Get_Pixel_Pos();
		world_pos = Get_World_Pos();
	}

	public Vector2 Get_Pixel_Pos()
	{
		Vector2 origin = new Vector2(target_Quad.Pixel_Width/2f, target_Quad.Pixel_Height/2f);
		Vector2 offset = new Vector2(transform.localPosition.x * target_Quad.Pixel_Width, transform.localPosition.y * target_Quad.Pixel_Height);
		Vector2 pos = origin + offset;
		
		// Rounding
		pos = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

		// Clamping
		pos = Pixel_Clamp(pos);

		return pos;
	} 

	public Vector2 Get_World_Pos()
	{
		Vector2 pos = new Vector2(transform.position.x, transform.position.y);
		return pos; 
	}

	Vector2 Pixel_Clamp (Vector2 pix_pos)
	{
		float _x = pix_pos.x;
		float _y = pix_pos.y;

		// x-value
		if(pix_pos.x < 0)
		{
			_x=0;
		}
		else if(pix_pos.x >= target_Quad.Pixel_Width)
		{
			_x = target_Quad.Pixel_Width-1;
		}

		// y-value
		if(pix_pos.y < 0)
		{
			_y=0;
		}
		else if(pix_pos.y >= target_Quad.Pixel_Height)
		{
			_y = target_Quad.Pixel_Height-1;
		}

		Vector2 clamped_pos = new Vector2(_x,_y);

		return clamped_pos;
	}

	void Paint()
	{
		target_Quad.Set_Pixel((int)pixel_pos.x, (int)pixel_pos.y, Color.red);
	}

	Color Pixel_Colour()
	{
		Color colour = target_Quad.Get_Pixel( (int)pixel_pos.x, (int)pixel_pos.y );
		return colour;
	}

	float Blackness()
	{
		float white = Pixel_Colour().r;
		float black = 1f-white;
		return black;
	}

}
