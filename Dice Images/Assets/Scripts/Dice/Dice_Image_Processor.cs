using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Image_Processor : MonoBehaviour 
{
    public Texture2D target_Image;
    public Transform anchor;

    public Sprite[] Dice_Faces;
    public Sprite BlankFace;

    public GameObject Dice_Prefab;

    public float dice_offset = 1f;

    void Start()
    {
        //Clear_Dice_Grid();
    }

    void Clear_Dice_Grid()
    {
        for(int i=0; i<anchor.childCount; i++)
        {
            Destroy(anchor.GetChild(i).gameObject);
        }
    }

    void Populate_Dice_Grid()
    {
        int width = target_Image.width;
        int height = target_Image.height;

        for(int y=0; y<height; y++)
        {
            for(int x=0; x<width; x++)
            {
                Color color = target_Image.GetPixel(x,y);
                float blackness = 1f -color.r;                  // color.g or color.b would work just as well, assuming you're working with a black and whitre image

                
                int dice_index = Mathf.FloorToInt(Dice_Faces.Length * blackness);      // this will spit out an integer in the range 0 to (N-1), where N is the number of dice face images you've entered into the Dice faces array. Please, enter the faces in increasing order of value

                // This is to handle the edge case where blackness is 1f
                if(dice_index == Dice_Faces.Length)
                {
                    dice_index = Dice_Faces.Length-1;
                }

                string name = Dice_Faces[dice_index].name;

                GameObject newDice = Instantiate(Dice_Prefab);
                newDice.transform.SetParent(anchor);
                newDice.transform.localPosition = new Vector3(dice_offset*x, dice_offset*y, 0f);
                newDice.GetComponent<SpriteRenderer>().sprite = Dice_Faces[dice_index];

                // This is to handle the edge case where blackness is 0f
                if(blackness == 0f)
                {
                    newDice.GetComponent<SpriteRenderer>().sprite = BlankFace;
                    name = BlankFace.name;
                }

                newDice.name = name;
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            Clear_Dice_Grid();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Clear_Dice_Grid();
            Populate_Dice_Grid();
        }
    }
}
