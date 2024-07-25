using FuryLion.UI;
using UnityEngine;

public class TalentButton : BaseButton
{
    [SerializeField] private TalentConfig _talentConfig;
    [SerializeField] private Text _costText;
    [SerializeField] private GameObject _blockPanel;

    public TalentConfig TalentConfig => _talentConfig;

    public void SetBlockPanelActive(bool isActive)
    {
        _blockPanel.SetActive(isActive);
    }

    public void SetCostTextActive(bool isActive)
    {
        _costText.gameObject.SetActive(isActive);
    }

    private void Awake()
    {
        _costText.Value = _talentConfig.Cost.ToString();
    }
}