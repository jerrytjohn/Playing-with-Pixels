using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(Camera))]
public class SnapshotCamera : MonoBehaviour 
{
    Camera snapCam;
    public string folderName = "Desktop folder name";
    public string fileName = "Enter filename here";

    [HideInInspector]
    public int resWidth = 300;

    [HideInInspector]
    public int resHeight = 300;

	// Use this for initialization
	void Awake () 
    {
        snapCam = transform.GetComponent<Camera>();	

        if(snapCam.targetTexture == null)
        {
            snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24); 
        }
        else
        {
            resWidth = snapCam.targetTexture.width;
            resHeight = snapCam.targetTexture.height;
        }

        // Don't render unless you need to
        snapCam.gameObject.SetActive(false);

	}

    public void TakeSnapshot()
    {
        snapCam.gameObject.SetActive(true);
    }

	
	// Update is called once per frame
	void LateUpdate () 
    {
        if(snapCam.gameObject.activeInHierarchy)
        {
            Texture2D myNewSnapShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);

            // the snap cam was just turned on this frame. It hasn't rendered anything yet
            snapCam.Render();

            // The currently active render texture is the main camera's. For this frame, that needs to be explicitly changed to the render texture of the snapshot camera
            RenderTexture.active = snapCam.targetTexture;

            myNewSnapShot.ReadPixels(new Rect(0,0,resWidth,resHeight), 0, 0);
            byte[] bytes = myNewSnapShot.EncodeToPNG();

            //string filename = "[HideInInspector]";
            System.IO.File.WriteAllBytes(File_Path(), bytes);

            snapCam.gameObject.SetActive(false);
        }
		
	}

    string File_Path()
    {
        //{0} The datapath
        string datapath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        datapath += "/Desktop/";

        if (!Directory.Exists(datapath))
        {
            Directory.CreateDirectory(datapath);
        }

        string finalPath = string.Format("{0}/{1}/{2}.png", datapath, folderName, fileName);

        return finalPath;
        // datapath/Desktop/folderName/fileName.png
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            TakeSnapshot();
        }
    }
}
