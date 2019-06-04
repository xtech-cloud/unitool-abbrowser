using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using XTC.MVCS;
using XTC.Logger;

public class SettingsView : View
{

    public const string NAME = "SettingsView";


    private SettingsFacade settingsFacade
    {
        get
        {
            return UIFacade.Find(SettingsFacade.NAME) as SettingsFacade;
        }
    }

    protected override void setup()
    {
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        RenderSettings.ambientIntensity = 1.0f;
        Camera.main.backgroundColor = new Color(49/255.0f, 77/255.0f, 121/255.0f, 0f);
    }

    protected override void bindEvents()
    {
        settingsFacade.sdAmbient.onValueChanged.AddListener(onAmbientValueChanged);
        settingsFacade.btnAmbientDefault.onClick.AddListener(onAmbientDefaultClickChanged);
        settingsFacade.tgLightOn.onValueChanged.AddListener(onLightOnChanged);
        settingsFacade.tgLightOff.onValueChanged.AddListener(onLightOffChanged);
        settingsFacade.tgSkybox.onValueChanged.AddListener(onSkyboxChanged);
        settingsFacade.tgColorBlue.onValueChanged.AddListener(onColorBlueChanged);
        settingsFacade.sdPosY.onValueChanged.AddListener(onPoxYChanged);
        settingsFacade.inputWidth.onEndEdit.AddListener(onWidthChanged);
        settingsFacade.inputHeight.onEndEdit.AddListener(onHeightChanged);
    }

    protected override void unbindEvents()
    {
        settingsFacade.sdAmbient.onValueChanged.RemoveListener(onAmbientValueChanged);
        settingsFacade.btnAmbientDefault.onClick.RemoveListener(onAmbientDefaultClickChanged);
        settingsFacade.tgLightOn.onValueChanged.RemoveListener(onLightOnChanged);
        settingsFacade.tgLightOff.onValueChanged.RemoveListener(onLightOffChanged);
        settingsFacade.tgSkybox.onValueChanged.RemoveListener(onSkyboxChanged);
        settingsFacade.tgColorBlue.onValueChanged.RemoveListener(onColorBlueChanged);
        settingsFacade.sdPosY.onValueChanged.RemoveListener(onPoxYChanged);
        settingsFacade.inputWidth.onEndEdit.RemoveListener(onWidthChanged);
        settingsFacade.inputHeight.onEndEdit.RemoveListener(onHeightChanged);
    }

    private void onAmbientValueChanged(float _value)
    {
        RenderSettings.ambientIntensity = _value;
    }

    private void onAmbientDefaultClickChanged()
    {
        settingsFacade.sdAmbient.value = 1.0f;
    }

    private void onLightOnChanged(bool _toggled)
    {
        if (!_toggled)
            return;
        settingsFacade.goLight.SetActive(true);
    }

    private void onLightOffChanged(bool _toggled)
    {
        if (!_toggled)
            return;
        settingsFacade.goLight.SetActive(false);
    }

    private void onSkyboxChanged(bool _toggled)
    {
        if (!_toggled)
            return;
        Camera.main.clearFlags = CameraClearFlags.Skybox;
    }

    private void onColorBlueChanged(bool _toggled)
    {
        if (!_toggled)
            return;
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = new Color(49/255.0f, 77/255.0f, 121/255.0f, 0f);
    }

    private void onPoxYChanged(float _value)
    {
        Vector2 pos = settingsFacade.captureBox.anchoredPosition;
        pos.y = Screen.height / 2 * _value;
        settingsFacade.captureBox.anchoredPosition = pos;
    }

    private void onWidthChanged(string _value)
    {
        float value = float.Parse(_value);
        settingsFacade.captureBox.sizeDelta = new Vector2(value, settingsFacade.captureBox.sizeDelta.y);
    }

    private void onHeightChanged(string _value)
    {
        float value = float.Parse(_value);
        settingsFacade.captureBox.sizeDelta = new Vector2(settingsFacade.captureBox.sizeDelta.x, value);
    }

}
