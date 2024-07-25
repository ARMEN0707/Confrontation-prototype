using System.Collections.Generic;
using Core;
using Entities;
using Interfaces;

namespace Systems
{
    public class BonusSystem : BaseSystem<IFarm, IForge, IStable, IUnitController, IQuarry, IWorkshop>
    {
        private readonly List<IBuilding> _buildings = new List<IBuilding>();
        private readonly List<IUnitController> _bonusDependents = new List<IUnitController>();

        protected override void AddActor(IUnitController building)
        {
            _bonusDependents.Add(building);
        }

        protected override void AddActor(IFarm warehouse)
        {
            UpdateBonus(warehouse, warehouse.TeamID);

            warehouse.ChangedLevel += UpdateBonus;
            warehouse.ChangedTeamID += OnChangedTeamID;
            
            _buildings.Add(warehouse);
        }

        protected override void AddActor(IStable warehouse)
        {
            UpdateBonus(warehouse, warehouse.TeamID);

            warehouse.ChangedLevel += UpdateBonus;
            warehouse.ChangedTeamID += OnChangedTeamID;
            
            _buildings.Add(warehouse);
        }

        protected override void AddActor(IForge warehouse)
        {
            UpdateBonus(warehouse, warehouse.TeamID);

            warehouse.ChangedLevel += UpdateBonus;
            warehouse.ChangedTeamID += OnChangedTeamID;
            
            _buildings.Add(warehouse);
        }

        protected override void AddActor(IQuarry warehouse)
        {
            UpdateBonus(warehouse, warehouse.TeamID);

            warehouse.ChangedLevel += UpdateBonus;
            warehouse.ChangedTeamID += OnChangedTeamID;

            _buildings.Add(warehouse);
        }

        protected override void AddActor(IWorkshop warehouse)
        {
            UpdateBonus(warehouse, warehouse.TeamID);

            warehouse.ChangedLevel += UpdateBonus;
            warehouse.ChangedTeamID += OnChangedTeamID;

            _buildings.Add(warehouse);
        }

        protected override void RemoveActor(IUnitController building)
        {
            _bonusDependents.Remove(building);
        }

        protected override void RemoveActor(IFarm actor)
        {
            _buildings.Remove(actor);
            UpdateBonus(actor, actor.TeamID);
        }

        protected override void RemoveActor(IStable actor)
        {
            _buildings.Remove(actor);
            UpdateBonus(actor, actor.TeamID);
        }
    
        protected override void RemoveActor(IForge actor)
        {
            _buildings.Remove(actor);
            UpdateBonus(actor, actor.TeamID);
        }
    
        protected override void RemoveActor(IQuarry actor)
        {
            _buildings.Remove(actor);
            UpdateBonus(actor, actor.TeamID);
        }

        protected override void RemoveActor(IWorkshop actor)
        {
            _buildings.Remove(actor);
            UpdateBonus(actor, actor.TeamID);
        }

        private void UpdateBonus(IBuilding building, int teamID)
        {
            switch (building)
            {
                case IFarm farm:
                    foreach (var b in _bonusDependents)
                    {
                        if (b.TeamID != teamID) 
                            continue;
                        
                        b.Dispose();
                        foreach (var bonusBuilding in _buildings)
                        {
                            if (!(bonusBuilding is IFarm bonusFarm))
                                continue;
                                
                            if (bonusFarm.TeamID != teamID)
                                continue;
                                
                            b.AddReproductionBonus(bonusFarm.GetReproductionBonus());
                        }
                    }
                    break;
                case IStable stable:
                    foreach (var b in _bonusDependents)
                    {
                        if (b.TeamID != teamID)
                            continue;
                        
                        b.Dispose();
                        foreach (var bonusBuilding in _buildings)
                        {
                            if (!(bonusBuilding is IStable bonusStable))
                                continue;
                                
                            if (bonusStable.TeamID != teamID)
                                continue;
                                
                            b.AddSpeedBonus(bonusStable.GetSpeedBonus());
                        }
                    }
                    break;
                case IForge forge:
                    foreach (var b in _bonusDependents)
                    {
                        if (b.TeamID != teamID)
                            continue;
                        
                        b.Dispose();
                        foreach (var bonusBuilding in _buildings)
                        {
                            if (!(bonusBuilding is IForge bonusForge))
                                continue;
                                
                            if (bonusForge.TeamID != teamID)
                                continue;
                                
                            b.AddForceBonus(bonusForge.GetForceBonus());
                        }
                    }
                    break;
                case IQuarry quarry:
                    foreach(var b in _bonusDependents)
                    {
                        if (b.TeamID != teamID)
                            continue;

                        b.Dispose();
                        foreach(var bonusBuilding in _buildings)
                        {
                            if (!(bonusBuilding is IQuarry bonusQuarry))
                                continue;

                            if (bonusQuarry.TeamID != teamID)
                                continue;

                            b.AddProtectionBonus(bonusQuarry.GetProtectionBonus());
                        }
                    }
                    break;
                case IWorkshop workshop:
                    foreach (var b in _bonusDependents)
                    {
                        if (b.TeamID != teamID)
                            continue;

                        b.Dispose();
                        foreach (var bonusBuilding in _buildings)
                        {
                            if (!(bonusBuilding is IWorkshop bonusWorkshop))
                                continue;

                            if (bonusWorkshop.TeamID != teamID)
                                continue;

                            b.AddProtectionBonus(bonusWorkshop.GetDebuffProtectionBonus());
                        }
                    }
                    break;
            }
        }

        private void OnChangedTeamID(IBuilding building, int id, int newID)
        {
            UpdateBonus(building, id);
            UpdateBonus(building, newID);
        }
    }
}