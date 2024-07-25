// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using FuryLion.UI;
using UnityEngine;
using Data;

public sealed class EndGamePage : Page
{
    [SerializeField] private Text _result;
    [SerializeField] private Text _reward;
    [SerializeField] private BaseButton _restart;
    [SerializeField] private BaseButton _next;
    [SerializeField] private BaseButton _menu;

    private static EndGamePage _instance;
    public static bool IsWin = false;
    
    private void OnRestartButton()
    {
        Gameplay.Deactivate();
        Gameplay.Init(LevelManager.ResetLevel());
        PageManager.Open<GamePage>();
    }
    
    private void OnMenuButton()
    {
        Gameplay.Deactivate();
        LevelManager.SaveWorlds();
        PageManager.Open<MainPage>();
    }

    private void OnNextLvlButton()
    {
        Gameplay.Deactivate();
        Gameplay.Init(LevelManager.CurrentLevel + 1);
        PageManager.Open<GamePage>();
    }

    private void SetResult()
    {
        int reward = 1;
        if (IsWin)
            reward = LevelManager.CurrentLevel <= PlayerData.LevelCompleted - 1
                ? LevelManager.RewardCurrentLevel / 3 : LevelManager.RewardCurrentLevel;

        if (IsWin && PlayerData.LevelCompleted < LevelManager.CurrentLevel + 1)
            PlayerData.LevelCompleted = LevelManager.CurrentLevel + 1;

        _reward.Value = "+ " + reward.ToString();
        PlayerData.GameCurrency += reward;
        _result.Value = IsWin ? "You win!" : "You lost!";
        _next.gameObject.SetActive(IsWin);
        _next.gameObject.SetActive(LevelManager.LevelsInfo.Levels.Count - 1 > LevelManager.CurrentLevel);
    }

    protected override void OnCreate()
    {
        _restart.Click += OnRestartButton;
        _menu.Click += OnMenuButton;
        _next.Click += OnNextLvlButton;
    }

    protected override void OnOpenComplete()
    {
        SetResult();
    }
}
