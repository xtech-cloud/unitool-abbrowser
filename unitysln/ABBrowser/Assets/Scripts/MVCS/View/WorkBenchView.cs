using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using XTC.MVCS;
using XTC.Logger;

public class WorkBenchView : View
{

    public const string NAME = "WorkBenchView";


    private WorkBenchFacade workBenchFacade
    {
        get
        {
            return UIFacade.Find(WorkBenchFacade.NAME) as WorkBenchFacade;
        }
    }

    private WorkBenchModel model
    {
        get
        {
            return modelCenter_.FindModel(WorkBenchModel.NAME) as WorkBenchModel;
        }
    }

    protected override void setup()
    {
    }

    protected override void bindEvents()
    {
        workBenchFacade.btnCapture.onClick.AddListener(onCaptureClick);
    }

    protected override void unbindEvents()
    {
        workBenchFacade.btnCapture.onClick.RemoveListener(onCaptureClick);
    }

    private void onCaptureClick()
    {
        int textureWidth = (int)workBenchFacade.captureBox.rect.width;
        int textureHeight = (int)workBenchFacade.captureBox.rect.height;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float centerX = screenWidth / 2;
        float centerY = screenHeight / 2;

        Rect rect = new Rect(centerX - textureWidth / 2, centerY - textureHeight / 2, textureWidth, textureHeight);
        rect.y -= workBenchFacade.captureBox.anchoredPosition.y;
        model.SaveCapture(textureWidth, textureHeight, screenWidth, screenHeight, rect);

        (modelCenter_.FindModel(FolderModel.NAME) as FolderModel).RefreshThumb();
    }

}
