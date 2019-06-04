using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using XTC.Logger;

public class RootMVCS : RootMono 
{
	public Material skybox;

	public FolderFacade folderFacade;
	public SettingsFacade settingsFacade;
	public WorkBenchFacade workBenchFacade;

	private FolderModel folderModel;
	private FolderView folderView;
	private FolderController folderController; 

	private WorkBenchModel workBenchModel;
	private MouseController mouseController;
	
	void Awake()
	{
		folderFacade.Register();
		settingsFacade.Register();
		workBenchFacade.Register();

		this.initialize();

		folderModel = new FolderModel();
		this.framework.modelCenter.Register(FolderModel.NAME, folderModel);

		folderView = new FolderView();
		this.framework.viewCenter.Register(FolderView.NAME, folderView);

		folderController = new FolderController();
		this.framework.controllerCenter.Register(FolderController.NAME, folderController);

		mouseController = new MouseController();
		this.framework.controllerCenter.Register(MouseController.NAME, mouseController);

		SettingsView viewSettings = new SettingsView();
		this.framework.viewCenter.Register(SettingsView.NAME, viewSettings);

		WorkBenchView viewWorkBench = new WorkBenchView();
		this.framework.viewCenter.Register(WorkBenchView.NAME, viewWorkBench);
		WorkBenchController controllerWorkBench = new WorkBenchController();
		this.framework.controllerCenter.Register(WorkBenchController.NAME, controllerWorkBench);
		workBenchModel = new WorkBenchModel();
		this.framework.modelCenter.Register(WorkBenchModel.NAME, workBenchModel);
	}

	void Start()
	{
		workBenchModel.LinkStatus(FolderModel.FolderStatus.NAME, folderModel);
	}

	void OnEnable()
	{
		this.setup();
	}

	void LateUpdate()
	{
		mouseController.LateUpdate();
	}

	void OnDisable()
	{
		this.dismantle();
	}

	void OnDestroy()
	{
		this.release();
		folderFacade.Cancel();
		settingsFacade.Cancel();
	}
}
