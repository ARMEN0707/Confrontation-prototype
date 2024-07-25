// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using FuryLion.UI;
using UnityEngine;

public sealed class LevelMessageBox : MessageBox
{
    [SerializeField] private BaseButton _okButton;

    protected override void OnCreate()
    {
        _okButton.Click += CloseLast;
    }
}
