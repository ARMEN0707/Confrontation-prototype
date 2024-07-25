using System;
using Entities;
using FuryLion.UI;
using Interfaces;
using UnityEngine;

namespace Views
{
    public class SettlementView : BuildingView
    {
        [SerializeField] private Text _unitCountText;
        [SerializeField] private Text _militaryCountText;

        public void SetArmyCount(int unitCount)
        {
            _unitCountText.Value = unitCount.ToString();
        }

        public void SetMilitaryCount(int militaryCount)
        {
            _militaryCountText.Value = militaryCount.ToString();
        }
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            (BuildingEntity as ISettlement)?.OnCrash(other);
        }
    }
}