using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;

public class WorkBenchController :  Controller
{
	public const string NAME = "WorkBenchController";

	public void SaveCapture(WorkBenchModel.WorkBenchStatus _status, int _textureWidth, int _textureHeight, float _screenWidth, float _screenHeight, Rect _rect)
	{
		if(string.IsNullOrEmpty(StaticData.folder))
            return;

        if(!Directory.Exists(StaticData.folder))
            Directory.CreateDirectory(StaticData.folder);

        string outdir = Path.Combine(StaticData.folder, "thumb");
        if(!Directory.Exists(outdir))
            Directory.CreateDirectory(outdir); 

        FolderModel.FolderStatus statusFolder = _status.Access(FolderModel.FolderStatus.NAME) as FolderModel.FolderStatus;
        if(null == statusFolder.currentAsset)
            return;

        string file = Path.Combine(outdir, statusFolder.currentAsset.code+".jpg");
        Sprite sprite = captureRect(_textureWidth, _textureHeight, (int)_screenWidth, (int)_screenHeight, _rect);
        byte[] bytes = sprite.texture.EncodeToJPG(100);
        File.WriteAllBytes(file ,bytes);
	}

	
    private Sprite captureRect(int _textureWidth, int _textureHeight, int _screenWidth, int _screenHeight, Rect _rect)
    {
        Camera camera = Camera.main;
        RenderTexture rt = new RenderTexture(_screenWidth, _screenHeight, 16);
        camera.targetTexture = rt;
        camera.Render();
        RenderTexture.active = rt;

        Texture2D screenShot = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGB24, false);
        screenShot.ReadPixels(_rect, 0, 0, false);
        screenShot.Apply();

        RenderTexture.active = null;
        camera.targetTexture = null;
        GameObject.Destroy(rt);

        Sprite sprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

}
