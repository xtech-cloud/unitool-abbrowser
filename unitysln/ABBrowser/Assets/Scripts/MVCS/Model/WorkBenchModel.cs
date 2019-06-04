using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using SimpleJSON;

public class WorkBenchModel : Model
{
    public const string NAME = "WorkBenchModel";


    public class WorkBenchStatus : Model.Status
    {
    }


    private WorkBenchController controller
    {
        get
        {
            return controllerCenter_.FindController(WorkBenchController.NAME) as WorkBenchController;
        }
    }

    private WorkBenchStatus status
    {
        get
        {
            return status_ as WorkBenchStatus;
        }
    }

    protected override void setup()
    {
        status_ = new WorkBenchStatus();
    }

    public void SaveCapture(int _textureWidth, int _textureHeight, float _screenWidth, float _screenHeight, Rect _rect)
    {
        controller.SaveCapture(status, _textureWidth, _textureHeight, _screenWidth, _screenHeight, _rect);
    }
}
