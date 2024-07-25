// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using System;
using FuryLion.UI;
using UnityEngine;
using Views;

public sealed class GamePage : Page
{
    [SerializeField] private CustomerView _customerView;
    [SerializeField] private BaseButton _pauseButton;
    [SerializeField] private BaseButton _boostButton;
    [SerializeField] private BaseButton _decreaseButton;
    [SerializeField] private Text _boost;
    
    private static GamePage _instance;

    private static CustomerController _customer;

    public static event Func<float, float> UpdatedBoost;

    public static void OnUpdateMoney(CustomerController customer)
    {
        if (_instance != null)
            _instance._customerView.SetMoneyText(customer.Money);
    }

    public static void OnUpdateMana(CustomerController customer)
    {
        if (_instance != null)
            _instance._customerView.SetManaText(customer.Mana);
    }

    public static void SetCustomer(CustomerController customerController) => _customer = customerController;

    private void UpdateBoostView(float boost)
    {
        _instance._boost.Value = boost.ToString("F1");
    }

    private void OnBoostButton()
    {
        var boost = UpdatedBoost?.Invoke(0.2f);
        if (boost != null)
            UpdateBoostView((float) boost);
    }

    private void OnDecreaseButton()
    {
        var boost = UpdatedBoost?.Invoke(-0.2f);
        if (boost != null)
            UpdateBoostView((float) boost);
    }

    private void OnPauseButton()
    {
        PageManager.Open<PausePage>();
    }

    protected override void OnCreate()
    {
        _instance = this;
        _boostButton.Click += OnBoostButton;
        _decreaseButton.Click += OnDecreaseButton;
        _pauseButton.Click += OnPauseButton;
        OnUpdateMana(_customer);
        OnUpdateMoney(_customer);
    }
    
    protected override void OnOpenComplete()
    {
        UpdateBoostView(1);
        Gameplay.SetPause(false);
    }
}
