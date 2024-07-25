// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using FuryLion;
using FuryLion.UI;
using UnityEngine;

public sealed class PausePage : Page
{
    [SerializeField] private BaseButton _continueButton;
    [SerializeField] private BaseButton _restartButton;
    [SerializeField] private BaseButton _menuButton;
    [SerializeField] private BaseButton _infoButton;
    
    [SerializeField] private SoundController _soundController;

    private void OnContinueButton()
    {
        PageManager.Open<GamePage>();
    }

    private void OnRestartButton()
    {
        var lvl = LevelManager.ResetLevel();
        Gameplay.Deactivate();
        Gameplay.Init(lvl);
        PageManager.Open<GamePage>();
    }
    
    private void OnMenuButton()
    {
        LevelManager.SaveWorlds();
        PageManager.Open<MainPage>();
    }
    
    private void OnInfoButton()
    {
        PopupManager.Open<LibraryPopUp>();
    }

    protected override void OnCreate()
    {
        _soundController.Init();
        _continueButton.Click += OnContinueButton;
        _restartButton.Click += OnRestartButton;
        _menuButton.Click += OnMenuButton;
        _infoButton.Click += OnInfoButton;
    }

    protected override void OnOpenStart(ViewParam viewParam)
    {
        _soundController.Init();
        Gameplay.SetPause(true);
        LevelManager.SaveWorlds();
    }
}
