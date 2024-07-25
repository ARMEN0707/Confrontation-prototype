using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class CustomerController
{
    private int _money;
    private int _mana;

    public readonly List<ICell> AvailableCells = new List<ICell>();

    public int TeamID { get; }

    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            UpdatedMoney?.Invoke(this);
        }
    }

    public int Mana
    {
        get => _mana;
        set
        {
            _mana = value;
            UpdatedMana?.Invoke(this);
        }
    }

    public event Action<CustomerController> UpdatedMoney;
    public event Action<CustomerController> UpdatedMana;

    public CustomerController(int teamID, int money, int mana)
    {
        TeamID = teamID;
        _money = money;
        _mana = mana;
    }
}
