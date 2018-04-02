using System.IO;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ExtractRenderTexture : MonoBehaviour {

    [SerializeField]
    Camera renderTextureCam;

    [SerializeField]
    TextureFormat textureFormat = TextureFormat.ARGB32;
    [Tooltip("The format to use, available are: png , jpg , xpr")]
    [SerializeField]
    string format = "jpg";
    [Range(0f,100f)]
    [SerializeField]
    int quality = 75;
    [Tooltip("Path inside the projects' assets, leave empty to store at the root")]
    [SerializeField]
    string path = "";
    [SerializeField]
    string fileName = "ExtractedRenderTexture";
    [SerializeField]
    bool appendIfExists = true;

	// Use this for initialization
	void Start () {
        renderTextureCam = GetComponent<Camera>();
	}
	
	

    [ExecuteInEditMode]
    public void ExtractTexture()
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
        renderTextureCam = GetComponent<Camera>();
        if (renderTextureCam.targetTexture == null)
        {
            Debug.Log("No Render Texture, Creating a new One");
            renderTextureCam .targetTexture = RenderTexture.GetTemporary(Screen.width,
                Screen.height);
            

        }
        RenderTexture renderTexture = renderTextureCam.targetTexture;

        Texture2D texture2D = 
            new Texture2D(renderTexture.width, renderTexture.height, textureFormat
            , false);
        renderTextureCam.Render();
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0, true);
        texture2D.Apply();
        
        //While reading the pixels, the active render texture could change which results in an empty picture
        RenderTexture.active = null; // "just in case" 
        

        string filePath = Path.Combine(Application.dataPath, path);
        //Make sure the format correct

        if (format.StartsWith("."))
            format.Remove(0);
        if (format != "jpg" && format != "png" && format != "exr")
            format = "jpg";
        //Combine the path
        filePath = filePath+"/"+fileName+"."+format;

        //Debug.Log(filePath);

        byte[] array = format.Equals("jpg") ? texture2D.EncodeToJPG(quality) :
            format.Equals("png") ? texture2D.EncodeToPNG() :
            format.Equals("exr") ? texture2D.EncodeToEXR() :
            texture2D.EncodeToJPG(quality);
        SaveByteArrayAsFile(filePath, array, appendIfExists);
    }

    public void SaveByteArrayAsFile(string path,byte[] bytes,bool appendIfExitst)
    {
        FileStream f = File.Open(path, appendIfExitst ? FileMode.Append : FileMode.Create);
        BinaryWriter w = new BinaryWriter(f);
        w.Write(bytes);
          
        
        f.Close();
    }

}
