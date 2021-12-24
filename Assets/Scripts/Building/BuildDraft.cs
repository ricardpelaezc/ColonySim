using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDraft : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private Material _builtMaterial;
    private Color _finalColor;

    public Vector3[] PointsAround { get; private set; }

    public bool IsBuilt;
    public bool Assigned;

    //private bool _isBuilding = true;
    private MeshRenderer _meshRenderer;
    private Color _color;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _finalColor = _builtMaterial.color;
        _color = _meshRenderer.material.color;

        PointsAround = new Vector3[4];
        PointsAround[0] = transform.position + new Vector3(_radius, 0, 0);
        PointsAround[1] = transform.position + new Vector3(-_radius, 0, 0);
        PointsAround[2] = transform.position + new Vector3(0, 0, _radius);
        PointsAround[3] = transform.position + new Vector3(0, 0, -_radius);
    }
    private void Update()
    {
        //if (_isBuilding)
        //{
        //    ColorFade();
        //    if (IsBuilt)
        //    {
        //        _surfaceGenerator.AddSurface(transform);
        //        _isBuilding = false;
        //    }
        //}
    }
    public void ColorFade(float buildingRatio)
    {
        _color = Color.Lerp(_color, _finalColor, buildingRatio * Time.deltaTime);
        if (_color.a >= _finalColor.a - 0.001f && _color.a <= _finalColor.a + 0.001f)
        {
            _color = _finalColor;
            _meshRenderer.material = _builtMaterial;
            IsBuilt = true;
        }
        _meshRenderer.material.color = _color;
    }
}
