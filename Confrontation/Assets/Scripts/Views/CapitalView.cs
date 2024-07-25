using Data;
using Entities;
using Interfaces;
using UnityEngine;

namespace Views
{
    public class CapitalView : SettlementView
    {
        public new void OnTriggerEnter2D(Collider2D other)
        {
            (BuildingEntity as ICapital)?.OnCrash(other);
        }
    }
}