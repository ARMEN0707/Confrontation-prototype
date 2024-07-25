using Entities;
using FuryLion.UI;
using UnityEngine;

namespace Views
{
    public class BarracksView : BuildingView
    {
        [SerializeField] private Text _unitCountText;

        public void SetUnitCount(int unitCount)
        {
            _unitCountText.Value = unitCount.ToString();
        }
    }
}