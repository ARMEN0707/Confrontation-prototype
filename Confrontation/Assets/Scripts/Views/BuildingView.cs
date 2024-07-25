using Data;
using Entities;
using FuryLion.UI;
using Interfaces;
using UnityEngine;

namespace Views
{
    public abstract class BuildingView : BaseView
    {
        [SerializeField] private Text _levelText;

        [SerializeField] private LineRenderer _lineRenderer;

        public IBuilding BuildingEntity;

        public void SetLevel(int lvl)
        {
            _levelText.Value = lvl.ToString();
        }
        
        public void SetLineEndPos(Vector3 pos) => _lineRenderer.SetPosition(1, pos);

        public void SetActiveLine(bool check)
        {
            _lineRenderer.enabled = check;
            var position = transform.position;
            _lineRenderer.SetPosition(0, position);
            _lineRenderer.SetPosition(1, position);
        }

        public void SetLineRendererSettings()
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
        }
    }
}
