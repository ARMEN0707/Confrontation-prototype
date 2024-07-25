using FuryLion.UI;
using Interfaces;
using UnityEngine;

namespace Views
{
    public class BaseView : Element, IRecyclable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
    }
}