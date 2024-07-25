using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Views;

public class BaseButton : BaseView
{
    public event Action Click;

    public void OnClick()
    {
        Click?.Invoke();
    }
}