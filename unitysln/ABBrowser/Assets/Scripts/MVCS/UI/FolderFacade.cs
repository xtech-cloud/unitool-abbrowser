using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XTC.MVCS;

public class FolderFacade : UIFacade
{
	public Button btnRefresh;
	public Button btnExportCSV;
	public Button btnExportMarkdown;
	public InputField inputPath;
	public GameObject templateAsset;
}
