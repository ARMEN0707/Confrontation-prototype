using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Interfaces;

public static class ShopManager
{
    private static readonly Dictionary<BuildingType, int> ShopItems = new Dictionary<BuildingType, int>();
    
    public static readonly List<CustomerController> Customers = new List<CustomerController>();

    public static void InitItems(BuildingsConfig buildingsConfig)
    {
        foreach (var b in buildingsConfig.BuildingConfigs) 
            if (!ShopItems.Keys.Contains(b.BuildingType))
                ShopItems.Add(b.BuildingType, b.Cost);
    }

    public static void AddCustomer(CustomerController customer)
    {
        if (Customers.Contains(customer))
            return;
        
        Customers.Add(customer);
    }

    public static void Clear()
    {
        ShopItems.Clear();
        Customers.Clear();
    }

    public static bool Buy(ICell cellEntity, BuildingType type)
    {
        if (!IsAvailable(type))
            return false;

        foreach (var m in Customers)
        {
            if (m.TeamID != cellEntity.TeamID) 
                continue;

            if (m.Money < ShopItems[type]) 
                continue;
            
            m.Money -= ShopItems[type];
            cellEntity.CreateBuilding(type);
            return true;
        }

        return false;
    }

    public static bool Upgrade(IBuilding building)
    {
        if (!ActOnBuilding(building, IsAvailable))
            return false;
        
        foreach (var m in Customers)
        {
            if (building.TeamID != m.TeamID) 
                continue;

            var cost = ActOnBuilding(building, type => GetCost(type, building.Level));

            if (m.Money < cost) 
                continue;
            
            m.Money -= cost;
            building.Level++;
            return true;
        }

        return false;
    }

    private static bool IsAvailable(BuildingType type) => LevelManager.PlayerData.AvailableBuildings.Contains(type);

    public static int GetCost(BuildingType type, int rang = 1)
    {
        return ShopItems[type] * 2 - (int) Math.Round((double) ShopItems[type] / rang);
    }

    public static T ActOnBuilding<T>(IBuilding building, Func<BuildingType, T> func)
    {
        return building switch
        {
            BarracksEntity barracksEntity => func(BuildingType.Barracks),
            FarmEntity farmEntity => func(BuildingType.Farm),
            ForgeEntity forgeEntity => func(BuildingType.Forge),
            MineEntity mineEntity => func(BuildingType.Mine),
            StableEntity stableEntity => func(BuildingType.Stable),
            WizardTowerEntity wizardTowerEntity => func(BuildingType.WizardTower),
            QuarryEntity quarryEntity => func(BuildingType.Quarry),
            WorkshopEntity workshopEntity => func(BuildingType.Workshop),
            FortEntity fortEntity => func(BuildingType.Fort),
            SettlementEntity settlementEntity => func(BuildingType.Settlement),
            CapitalEntity capitalEntity => func(BuildingType.Capital),
            _ => default(T)
        };
    }
}
