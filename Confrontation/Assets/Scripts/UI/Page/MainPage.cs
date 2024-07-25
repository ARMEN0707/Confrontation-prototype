// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using Data;
using UnityEngine;
using FuryLion;
using FuryLion.UI;

public sealed class MainPage : Page, IMainPage
{
    [SerializeField] private SoundController _soundController;
    [SerializeField] private Text _currency;
    [SerializeField] private BaseButton _infoButton;
    [SerializeField] private BaseButton _academyButton;
    
    protected override void OnCreate()
    {
        _infoButton.Click += OnInfoButton;
        _academyButton.Click += () => PageManager.Open<AcademyPage>();
        _soundController.Init();
        SoundManager.PlaySound(Sounds.Music.MainMenu, true);
    }
    
    protected override void OnOpenStart(ViewParam viewParam)
    {
        _soundController.Init();
        _currency.Value = PlayerData.GameCurrency.ToString();
    }

    private void OnInfoButton()
    {
        PopupManager.Open<LibraryPopUp>();
    }

    private void OnDisable()
    {
        Storage.Save(LevelManager.PlayerDataFilePath, LevelManager.PlayerData);
    }
}
