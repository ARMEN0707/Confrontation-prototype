// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using Data;
using FuryLion.UI;
using UnityEngine;

public sealed class AcademyPage : Page
{
    [SerializeField] private Text _currency;
    [SerializeField] private BaseButton _backButton;
    [SerializeField] private List<TalentNode> _talentNodes = new List<TalentNode>();

    private readonly List<TalentButton> _availableTalentButtons = new List<TalentButton>();

    private Vector3 _mouseDownPos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseDownPos = MouseManager.GetMousePosition(0, CameraManager.AcademyCamera);
        }

        if (Input.GetMouseButton(0))
        {
            if (_mouseDownPos != MouseManager.GetMousePosition(0, CameraManager.AcademyCamera))
            {
                CameraManager.CameraMove(CameraManager.AcademyCamera, _mouseDownPos, Bounds.center, Bounds.size / 4);
            }
        }
    }

    private void OnBuyButtonClick(TalentButton button)
    {
        if (!_availableTalentButtons.Contains(button) || button.TalentConfig.Cost > PlayerData.GameCurrency) 
            return;
        
        LevelManager.PlayerData.BoughtTalents.Add(button.TalentConfig.Talent);
        PlayerData.GameCurrency -= button.TalentConfig.Cost;
        _currency.Value = PlayerData.GameCurrency.ToString();
        button.SetCostTextActive(false);
        SetTalent(button);
        _availableTalentButtons.Clear();
        foreach (var t in _talentNodes) 
            t.Traverse(UpdateNode);
    }

    private void OnTalentButtonClick(TalentButton button)
    {
        var position = button.Position + Vector3.down * 10;
        var isAvailable = _availableTalentButtons.Contains(button);
        TalentMessageBox.Init(position, button.TalentConfig.Info, () => OnBuyButtonClick(button), isAvailable);
        MessageBoxManager.Open<TalentMessageBox>();
    }

    private void SetTalent(TalentButton button)
    {
        switch (button.TalentConfig.Talent)
        {
            case Talent.UnitSpeed:
                LevelManager.PlayerData.BaseSpeed += 0.25f;
                break;
            case Talent.UnitForce:
                LevelManager.PlayerData.BaseForce += 1f;
                break;
            case Talent.MilitaryReproduction:
                LevelManager.PlayerData.BaseMilitaryReproduction -= 2f;
                break;
            case Talent.ArmyReproduction:
                LevelManager.PlayerData.BaseArmyReproduction -= 1f;
                break;
            case Talent.Mine:
                LevelManager.PlayerData.AvailableBuildings.Add(BuildingType.Mine);
                break;
            case Talent.Farm:
                LevelManager.PlayerData.AvailableBuildings.Add(BuildingType.Farm);
                break;
            case Talent.Forge:
                LevelManager.PlayerData.AvailableBuildings.Add(BuildingType.Forge);
                break;
            case Talent.Stable:
                LevelManager.PlayerData.AvailableBuildings.Add(BuildingType.Stable);
                break;
        }
    }

    protected override void OnCreate()
    {
        _backButton.Click += CloseLast;
        foreach (var t in _talentNodes) 
            t.Traverse(b => b.Click += () => OnTalentButtonClick(b));
    }

    protected override void OnOpenStart(ViewParam viewParam)
    {
        _currency.Value = PlayerData.GameCurrency.ToString();
        _availableTalentButtons.Clear();
        foreach (var t in _talentNodes) 
            t.Traverse(UpdateNode);
    }

    private void UpdateNode(TreeNode<TalentButton> node)
    {
        var isBought = LevelManager.PlayerData.BoughtTalents.Contains(node.Value.TalentConfig.Talent);
        if (node.Parent == null && !isBought || 
            node.Parent != null && !isBought &&
            LevelManager.PlayerData.BoughtTalents.Contains(node.Parent.Value.TalentConfig.Talent))
        {
            node.Value.SetBlockPanelActive(false);
            node.Value.SetCostTextActive(true);
            _availableTalentButtons.Add(node.Value);
        }
        else if (isBought)
        {
            node.Value.SetBlockPanelActive(false);
            node.Value.SetCostTextActive(false);
        }
    }
}
