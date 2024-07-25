// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace FuryLion.UI
{
    public abstract partial class Popup : BasePopup
    {
        protected override void PlayAnimationOpen(View previousView)
        {
            transform.position = new Vector3(0.0f, 0.0f, transform.position.z);
            CompleteAnimationOpen();
        }

        protected override void PlayAnimationClose(View nextView)
        {
            transform.position = new Vector3(1000.0f, 1000.0f, transform.position.z);
            CompleteAnimationClose();
        }
    }
}
