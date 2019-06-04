using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XTC.MVCS;

public class SettingsFacade : UIFacade
{
	public const string NAME = "SettingsFacade";
	public Slider sdAmbient;
	public Button btnAmbientDefault;

	public Toggle tgLightOn;
	public Toggle tgLightOff;
	public GameObject goLight;

	public Toggle tgSkybox;
	public Toggle tgColorBlue;

	public Slider sdPosY;
	public InputField inputWidth;
	public InputField inputHeight;
	public RectTransform captureBox;

}
