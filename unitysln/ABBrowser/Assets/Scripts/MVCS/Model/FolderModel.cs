﻿using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using SimpleJSON;

public class FolderModel : Model
{
    public const string NAME = "FolderModel";

    public class Asset
    {
        public string pack = "";
        public string path = "";
        public string name = "";
        public string alias = "";
        public string code = "";
        public string thumb = "";
        public string android = "";
        public string webgl = "";
        public string win32 = "";
    }

    public class FolderStatus : Model.Status
    {
        public const string NAME = "FolderStatus";
        public Asset currentAsset = null;
        public Dictionary<string, Asset> assets = new Dictionary<string, Asset>();
    }


    private FolderController folderController
    {
        get
        {
            return controllerCenter_.FindController(FolderController.NAME) as FolderController;
        }
    }

    private FolderStatus status
    {
        get
        {
            return status_ as FolderStatus;
        }
    }

    protected override void setup()
    {
        status_ = new FolderStatus();
    }

    public void RefreshFolder(string _path)
    {
        StaticData.folder = _path;
        status.assets.Clear();

        if (string.IsNullOrEmpty(_path))
        {
            folderController.RefreshFolder(status);
            return;
        }

        if (!Directory.Exists(_path))
        {
            folderController.RefreshFolder(status);
            return;
        }

        string androidDir = Path.Combine(_path, "android");
        string metaDir = Path.Combine(_path, "meta");
        string thumbDir = Path.Combine(_path, "thumb");
        string webglDir = Path.Combine(_path, "webgl");
        string win32Dir = Path.Combine(_path, "win32");

        if (Directory.Exists(metaDir))
        {
            foreach (string file in Directory.GetFiles(metaDir))
            {
                string json = File.ReadAllText(file);
                try
                {
                    JSONClass root = JSON.Parse(json).AsObject;
                    string pack = root["uuid"].Value;
                    string path = root["path"].Value;
                    foreach (JSONNode nFile in root["files"].AsArray)
                    {
                        JSONClass cFile = nFile.AsObject;
                        string name = cFile["name"].Value;
                        string alias = cFile["alias"].Value;
                        string code = cFile["code"].Value;
                        if (!status.assets.ContainsKey(code))
                            status.assets.Add(code, new Asset());
                        status.assets[code].pack = pack;
                        status.assets[code].path = path;
                        status.assets[code].name = name;
                        status.assets[code].alias = alias;
                        status.assets[code].code = code;
                        status.assets[code].android = Path.Combine(androidDir, pack);
                        status.assets[code].win32 = Path.Combine(win32Dir, pack);
                        status.assets[code].webgl = Path.Combine(webglDir, pack);
                        status.assets[code].thumb = Path.Combine(thumbDir, code + ".jpg");
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        Debug.Log(status.assets.Count);
        folderController.RefreshFolder(status);
    }

    public void RefreshAsset(string _uuid)
    {
        status.currentAsset = null;
        if (!status.assets.ContainsKey(_uuid))
            return;

        status.currentAsset = status.assets[_uuid];
        folderController.RefreshAsset(status);
    }

    public void RefreshThumb()
    {
        folderController.RefreshThumb(status);
    }

    public void Export(string _format)
    {
        StringBuilder sb = new StringBuilder();
        string line = "";
        if(_format.Equals(".md"))
        {
            sb.AppendLine("|index|alias|name|pack|path|");
            sb.AppendLine("|:--|:--|:--|:--|:--|");
        }
        else
        {
            sb.AppendLine("index,alias,name,pack,path");
        }
        int index = 0;
        foreach (Asset asset in status.assets.Values)
        {
            if (asset.name.StartsWith("_"))
                continue;
            index += 1;
            if(_format.Equals(".md"))
                line = string.Format("|{0}|{1}|{2}|{3}|{4}|", index, asset.alias, asset.name, asset.pack, asset.path);
            else
                line = string.Format("{0},{1},{2},{3},{4}", index, asset.alias, asset.name, asset.pack, asset.path);
            sb.AppendLine(line);
        }

        string dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
        File.WriteAllText(Path.Combine(dir, "matloud" + _format), sb.ToString());
    }
}
