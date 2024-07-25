// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using FuryLion.UI;

public sealed class LoadingPage : Page, ILoadingPage
{
    protected override void OnCreate()
    {
        PageManager.Open<MainPage>();
    }
}
