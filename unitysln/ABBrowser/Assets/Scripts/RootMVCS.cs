using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using XTC.Logger;

public class RootMVCS : RootMono 
{
	public Material skybox;

	private FolderModel folderModel;
	private FolderView folderView;
	private FolderController folderController;
	
	void Awake()
	{
		this.initialize();

		folderModel = new FolderModel();
		this.framework.modelCenter.Register(FolderModel.NAME, folderModel);

		folderView = new FolderView();
		this.framework.viewCenter.Register(FolderView.NAME, folderView);

		folderController = new FolderController();
		this.framework.controllerCenter.Register(FolderController.NAME, folderController);
	}

	void Start()
	{
	}

	void OnEnable()
	{
		this.setup();
	}

	void OnDisable()
	{
		this.dismantle();
	}

	void OnDestroy()
	{
		this.release();
	}
}
