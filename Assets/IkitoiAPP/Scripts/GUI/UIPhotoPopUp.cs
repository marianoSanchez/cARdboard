using UnityEngine;
using System.Collections;
using System.IO;
using Vuforia;

public class UIPhotoPopUp : UIPopUp
{
	#region Variables (private)

    private byte[] texture;
    private int width;
    private int height;

	#endregion

	#region Propiedades (public)

    [Space(10)]
    public UITexture photoTexture;

	public string path = "/sdcard/Pictures/";

	#endregion

	#region Funciones de evento de Unity

    protected override void Awake()
    {
        EventDelegate.Add(onAccept, SavetoPNG);
        base.Awake();
    }

    protected override void OnEnable()
    {
        CameraDevice.Instance.Stop();
        base.OnEnable();
    }

    protected void OnDisable()
    {
        CameraDevice.Instance.Start();
    }
        
	#endregion

	#region Metodos

    //public override void Close()
    //{
    //    CameraDevice.Instance.Start();
    //    base.Close();
    //}

    private void SavetoPNG()
    {        
		StartCoroutine(SaveScreenshotRoutine(path));
    }

    IEnumerator SaveScreenshotRoutine(string finalPath)
    {
        // Gets the texture
        var texture = (Texture2D)photoTexture.mainTexture;

        // Store dimensions
        width = texture.width;
        height = texture.height;

        // Set Filename and temporary save location
        var currFilename = ScreenShotName(width, height);
		var dstFilepath = finalPath + currFilename;

        var snapShot = GameObject.FindObjectOfType<CaptureAndSave>();
        snapShot.SaveTextureToGallery(texture);
        yield return null;

        Debug.Log("Captura de pantalla guardada en galeria.");

//#if UNITY_IOS
//        var snapShot = GameObject.FindObjectOfType<CaptureAndSave>();
//        snapShot.SaveTextureToGallery(texture);
//        yield return null;

//        Debug.Log("Captura de pantalla guardada en galeria.");
//#elif UNITY_ANDROID
//        var bytes = texture.EncodeToPNG();
//        var origFilepath = System.IO.Path.Combine(Application.persistentDataPath, currFilename);
//        if (!System.IO.File.Exists(origFilepath))
//        {
//            File.WriteAllBytes(origFilepath, bytes);

//            // Wait until it's created (or timeOut)
//            var timeOut = 4f;
//            while (!System.IO.File.Exists(origFilepath))
//            {
//                yield return new WaitForSeconds(.001f);

//                // Timer
//                timeOut -= .001f;
//                if (timeOut < 0)
//                    break;
//            }
//        }

//        // Move to its final destination
//        System.IO.File.Move(origFilepath, dstFilepath);

//        Debug.Log("Captura de pantalla guardada en: '" + dstFilepath + "'.");
//#endif

        // Destroy Texture
        Destroy(texture);
    }

    private string ScreenShotName(int width, int height)
    {
        return "ikitoiScreenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
    }
    
    public void SetPhotoTexture(Texture2D texture)
    {
        if (photoTexture == null) return;

        photoTexture.mainTexture = texture;
    }
    
	#endregion
}

