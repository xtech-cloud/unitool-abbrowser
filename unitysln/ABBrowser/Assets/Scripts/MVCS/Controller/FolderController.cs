using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;

public class FolderController :  Controller
{
	public const string NAME = "FolderController";

	private FolderView folderView{
		get{
			return viewCenter_.FindView(FolderView.NAME) as FolderView;
		}
	}

	public void RefreshFolder(FolderModel.FolderStatus _status)
	{
		folderView.RefreshFolder(_status);
	}

	public void RefreshAsset(FolderModel.FolderStatus _status)
	{
		folderView.RefreshAsset(_status);
	}

}
