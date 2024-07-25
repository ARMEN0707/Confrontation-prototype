// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using FuryLion.UI;
using UnityEngine;

public sealed class LibraryPopUp : Popup
{
    [SerializeField] private BaseButton _exitButton;
    [SerializeField] private BaseButton _buildingsInfoButton;
    [SerializeField] private BaseButton _magicInfoButton;

    [SerializeField] private VerticalListView _buildingsListView;

    protected override void OnCreate()
    {
        _exitButton.Click += OnExitButtonClick;
        _buildingsInfoButton.Click += OnBuildingInfoButtonClick;
        SetDataListView();
    }

    private void SetDataListView()
    {
        var buildingsInfo = LibraryManager.GetBuildingsInfo();
        foreach (var building in buildingsInfo)
        {
            var infoObj = Recycler.Get<BuildingInfoItem>();
            infoObj.SetData(building.Value);
            if(building.Key == BuildingType.Farm)
                ImageUtility.SetImageScale(infoObj.Icon,0.75f);
            
            _buildingsListView.Add(infoObj);
        }
    }

    private void OnExitButtonClick()
    {
        if(_buildingsListView.gameObject.activeSelf)
            HandleObjects(true);
        else
            CloseLast();
    }

    private void OnBuildingInfoButtonClick()
    {
        HandleObjects(false);
    }
    
    private void HandleObjects(bool check)
    {
        _buildingsListView.gameObject.SetActive(!check);
        _buildingsInfoButton.gameObject.SetActive(check);
        _magicInfoButton.gameObject.SetActive(check);
    }
}
