using System;
using FuryLion.UI;
using UnityEngine;

namespace Views
{
    public class CustomerView : BaseView
    {
        [SerializeField] private Text _money;
        [SerializeField] private Text _mana;

        public void SetMoneyText(int money)
        {
            _money.Value = money.ToString();
        }

        public void SetManaText(int mana)
        {
            _mana.Value = mana.ToString();
        }
    }
}