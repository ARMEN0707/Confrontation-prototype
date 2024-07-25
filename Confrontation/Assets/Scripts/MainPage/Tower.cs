using System;
using Data;
using UnityEngine;
using FuryLion.UI;

public class Tower : MonoBehaviour
{
    [SerializeField] private int _number;
    [SerializeField] private GameObject _tower;
    [SerializeField] private GameObject _ruinedTower;
    [SerializeField] private GameObject _lock;
    
    private BaseButton _button;
    private StateLevel _state;
    
    public StateLevel State
    {
        get => _state;
        private set
        {
            switch (value)
            {
                case StateLevel.Closed:
                    _tower.SetActive(true);
                    _ruinedTower.SetActive(false);
                    _lock.SetActive(true);
                    break;
                case StateLevel.Open:
                    _tower.SetActive(true);
                    _ruinedTower.SetActive(false);
                    _lock.SetActive(false);
                    break;
                case StateLevel.Ruined:
                    _tower.SetActive(false);
                    _ruinedTower.SetActive(true);
                    _lock.SetActive(false);
                    break;
            }

            _state = value;
        }
    }

    public void Awake()
    {
        _button = GetComponent<BaseButton>();
        _button.Click += OpenLevel;
    }

    private void OnEnable()
    {
        SetStateTowers(PlayerData.LevelCompleted);
    }

    private void SetStateTowers(int levelCompleted)
    {
        if (_number <= levelCompleted)
            State = StateLevel.Ruined;
        else if (_number == levelCompleted + 1)
            State = StateLevel.Open;
        else if (_number > levelCompleted + 1)
            State = StateLevel.Closed;
    }

    private void OpenLevel()
    {
        if (_state != StateLevel.Closed && _number <= LevelManager.LevelsInfo.Levels.Count)
        {
            Gameplay.Deactivate();
            Gameplay.Init(_number - 1);
            PageManager.Open<GamePage>();
        }
        else
            MessageBoxManager.Open<LevelMessageBox>();
    }
}
