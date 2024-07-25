using System.Linq;
using UnityEngine;

public class RegionLevelMap : MonoBehaviour
{
    [SerializeField] private Tower[] _towers;
    [SerializeField] private RegionLevelMap _nextRegion;
    [SerializeField] private GameObject _backGround;

    private void Start()
    {
        if (CheckRegion())
            OpenNextRegion();
    }

    private void Open()
    {
        _backGround.SetActive(false);
    }

    private void OpenNextRegion()
    {
        _nextRegion.Open();
    }

    private bool CheckRegion()
    {
        return _nextRegion != null && _towers.All(tower => tower.State == StateLevel.Ruined);
    }
}
