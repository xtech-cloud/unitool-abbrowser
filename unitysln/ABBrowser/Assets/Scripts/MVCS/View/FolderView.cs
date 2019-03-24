using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XTC.MVCS;
using XTC.Logger;

public class FolderView : View
{
    public delegate void OnSpriteLoadFinish(Sprite _sprite);

    public const string NAME = "FolderView";

    private GameObject currentAsset;
    private Material currentSkybox;

    private FolderFacade folderFacade
    {
        get
        {
            return UIFacade.Find("Folder") as FolderFacade;
        }
    }

    private FolderModel folderModel
    {
        get
        {
            return modelCenter_.FindModel(FolderModel.NAME) as FolderModel;
        }
    }

    protected override void setup()
    {
        folderFacade.inputPath.text =  PlayerPrefs.GetString("path", "");
    }

    protected override void bindEvents()
    {
        folderFacade.btnRefresh.onClick.AddListener(onRefreshButtonClick);
    }

    protected override void unbindEvents()
    {
        folderFacade.btnRefresh.onClick.RemoveListener(onRefreshButtonClick);
    }

    public void RefreshFolder(FolderModel.FolderStatus _status)
    {
        List<string> needMove = new List<string>();
        Transform container = folderFacade.templateAsset.transform.parent;
        foreach (Transform child in container)
        {
            if (!child.gameObject.activeSelf)
                continue;
            needMove.Add(child.name);
        }

        foreach (FolderModel.Asset asset in _status.assets.Values)
        {
            if (needMove.Contains(asset.code))
                needMove.Remove(asset.code);

            Transform tsAsset = container.Find(asset.code);
            if (null == tsAsset)
            {
                GameObject assetClone = GameObject.Instantiate(folderFacade.templateAsset);
                assetClone.name = asset.code;
                assetClone.transform.SetParent(container);
                assetClone.transform.localScale = Vector3.one;
                assetClone.SetActive(true);

                assetClone.transform.Find("Toggle/uuid").GetComponent<Text>().text = asset.code;
                assetClone.transform.Find("Toggle/name").GetComponent<Text>().text = asset.name;
                assetClone.transform.Find("Toggle/path").GetComponent<Text>().text = asset.path;

                assetClone.transform.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener((_toggled) =>
                {
                    if (!_toggled)
                        return;

                    if (null != currentAsset)
                    {
                        GameObject.Destroy(currentAsset);
                        currentAsset = null;
                    }
                    RenderSettings.skybox = (RootMVCS.instance as RootMVCS).skybox;

                    folderModel.RefreshAsset(assetClone.name);
                });

                Image img = assetClone.transform.Find("Toggle/thumb").GetComponent<Image>();
                RootMono.instance.StartCoroutine(loadSprite(asset.thumb, img));
            }
        }

        foreach (string uuid in needMove)
        {
            Transform child = container.Find(uuid);
            if (null == child)
                continue;
            GameObject.DestroyObject(child.gameObject);
        }
    }

    public void RefreshAsset(FolderModel.FolderStatus _status)
    {
        FolderModel.Asset asset = _status.currentAsset;
        RootMono.instance.StartCoroutine(loadAsset(asset.win32, asset.name, asset.path));
    }


    private void onRefreshButtonClick()
    {
        string path = folderFacade.inputPath.text;
        folderModel.RefreshFolder(path);
        PlayerPrefs.SetString("path", path);
        PlayerPrefs.Save();
    }

    private IEnumerator loadAsset(string _url, string _name, string _path)
    {
        if (!_path.StartsWith("environment") && !_path.StartsWith("model") && !_path.StartsWith("skybox"))
            yield break;

        WWW www = new WWW("file://" + _url);
        yield return www;

        if (null != www.error)
        {
            this.LogError(www.error);
            yield break;
        }

        AssetBundle ab = www.assetBundle;
        if (null == ab)
        {
            this.LogError("assetBundle is null");
            yield break;
        }

        if (_path.StartsWith("model"))
        {
            GameObject go = ab.LoadAsset(_name) as GameObject;
            if (null == go)
            {
                this.LogError("file is null");
                yield break;
            }
            currentAsset = GameObject.Instantiate(go);
            currentAsset.transform.position = Vector3.zero;
        }
        else if (_path.StartsWith("skybox"))
        {
            Material skybox = ab.LoadAsset(_name) as Material;
            RenderSettings.skybox = skybox;
        }

        ab.Unload(false);
        Resources.UnloadUnusedAssets();
    }

    private IEnumerator loadSprite(string _url, Image _image)
    {
        WWW www = new WWW("file://" + _url);
        yield return www;

        if (null != www.error)
        {
            this.LogError(www.error);
            yield break;
        }

        Texture2D texture = www.texture;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        yield return new WaitForEndOfFrame();
        if (null == _image)
            yield break;
        _image.sprite = sprite;
    }
}
