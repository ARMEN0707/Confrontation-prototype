using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using FuryLion.UI;
using Interfaces;
using UnityEngine;

namespace Views
{
    public class CellView : BaseView
    {
        [SerializeField] private SpriteRenderer _outlineRenderer;
        [SerializeField] private GameObject _lineRenderer;
        [SerializeField] private SpriteRenderer _fog;

        private Animation _animation;

        public CellEntity CellEntity;

        private void Awake()
        {
            _animation = GetComponent<Animation>();
        }

        public void SetActiveOutline(bool isActive) => _outlineRenderer.enabled = isActive;

        public List<ICell> FindNeighbours()
        {
            var cells = Physics2D.OverlapCircleAll(
                transform.position, 0.7f, LayerMask.GetMask("Cell"));

            var cellEntities = new List<ICell>();
            foreach (var c in cells)
                if (c.gameObject.TryGetComponent(out CellView cell) && cell != this)
                    cellEntities.Add(cell.CellEntity);

            return cellEntities;
        }

        public void DrawLine(Vector3 p1, Vector3 p2)
        {
            var obj = Instantiate(_lineRenderer, transform);

            var line = obj.GetComponentInChildren<LineRenderer>();
            SetLineRendererSettings(line);

            var points = new List<Vector3> {p1, p2};
            line.SetPositions(points.ToArray());
        }

        private void SetLineRendererSettings(LineRenderer lineRenderer)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.04f;
            lineRenderer.endWidth = 0.04f;
        }

        public void SetFog(bool active)
        {
            if (!active)
                _animation.Play("hidefog");
            else
            {
                _fog.enabled = true;
                _animation.Play("showfog");
            }
        }

        public void HideFog()
        {
            _fog.enabled = false;
        }
    }
}