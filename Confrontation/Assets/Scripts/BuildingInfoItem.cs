using FuryLion.UI;
using UnityEngine;

public class BuildingInfoItem : ListElement
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _type;
    [SerializeField] private Text _description;

    public Image Icon => _image;

    public void SetData(BuildingInfo buildingInfo)
    {
        _image.Sprite = buildingInfo.Spr;
        _type.Value = buildingInfo.Name;
        _description.Value = buildingInfo.Description;
    }
}