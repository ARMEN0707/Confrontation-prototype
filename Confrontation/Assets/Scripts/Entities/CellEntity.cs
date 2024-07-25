using System;
using System.Collections.Generic;
using Core;
using Data;
using FuryLion.UI;
using Interfaces;
using UnityEngine;
using Views;

namespace Entities
{
    public class CellEntity : BaseEntity<CellData>, ICell
    {
        public CellView CellView { get; }

        public int TeamID
        {
            get => Data.TeamID;
            set => Data.TeamID = value;
        }

        public IBuilding Building { get; set; }

        public CellEntity(CellData data) : base(data)
        {
            CellView = Recycler.Get<CellView>();
            CellView.transform.position = Data.Position;
            CellView.SetSprite(LevelManager.LevelsInfo.TeamSprites[Data.TeamID]);
            CellView.CellEntity = this;
        }

        public List<ICell> FindNeighbours()
        {
            return CellView.FindNeighbours();
        }

        public void DrawLine(Vector3 p1, Vector3 p2)
        {
            CellView.DrawLine(p1, p2);
        }

        public void SetBuilding(IBuilding building) => Building = building;
        
        public void CreateBuilding(BuildingType type)
        {
            Data.Building = type switch
            {
                BuildingType.Barracks => new BarracksData(),
                BuildingType.Farm => new FarmData(),
                BuildingType.Forge => new ForgeData(),
                BuildingType.Mine => new MineData(),
                BuildingType.Stable => new StableData(),
                BuildingType.WizardTower => new WizardTowerData(),
                _ => Data.Building
            };

            if (Data.Building == null)
                return;

            Data.Building.TeamID = TeamID;
            Data.Building.Position = Data.Position - new Vector3(0, 0, 2);
            Building = Gameplay.CreateNewObject(Data.Building) as IBuilding;
        }

        public void ChangeTeamID(int newTeamID)
        {
            Data.TeamID = newTeamID;
            if (Data.Building != null)
                Data.Building.TeamID = newTeamID;
            
            CellView.SetSprite(LevelManager.LevelsInfo.TeamSprites[newTeamID]);
        }

        public void SetFog(bool active) => CellView.SetFog(active);
    }
}
