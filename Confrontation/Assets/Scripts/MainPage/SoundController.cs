using System;
using UnityEngine;
using FuryLion;
using FuryLion.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private Image _imageButton;
    [SerializeField] private Sprite _onSoundsSprite;
    [SerializeField] private Sprite _offSoundsSprite;
    [SerializeField] private BaseButton _button;

    private const string Key = "Sound";
    
    private bool _mute;

    public bool Mute
    {
        get => _mute;
        set
        {
            SoundManager.SetSoundVolume(value ? 0 : 1);
            _mute = value;
            SetSprite();
        }
    }

    private void Awake()
    {
        _button.Click += ChangeState;
    }

    public void Init()
    {
        Mute = PlayerPrefs.GetInt(Key, 1) == 0;
    }

    private void ChangeState()
    {
        PlayerPrefs.SetInt(Key, Convert.ToInt32(Mute));
        Mute = !Mute;
    }

    private void SetSprite() => _imageButton.Sprite = Mute ? _offSoundsSprite : _onSoundsSprite;
}
