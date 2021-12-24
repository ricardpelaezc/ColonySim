using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SurfaceGenerator : MonoBehaviour
{
    [SerializeField] GameObject _surfaceUnit;
    [SerializeField] private Vector3 _surfaceDimensions;
    public static NavMeshSurface NMSurface;

    private void Start()
    {
        NMSurface = GetComponent<NavMeshSurface>();
        GenerateSurface();
    }

    private void GenerateSurface()
    {
        Vector3 surfaceUnitDimensions = _surfaceUnit.transform.localScale;

        float surfaceExtensionsX = _surfaceDimensions.x * surfaceUnitDimensions.x;
        float surfaceExtensionsY = _surfaceDimensions.y * surfaceUnitDimensions.y;
        float surfaceExtensionsZ = _surfaceDimensions.z * surfaceUnitDimensions.z;

        for (int y = 0; y < _surfaceDimensions.y; y++)
        {
            for (int x = 0; x < _surfaceDimensions.x; x++)
            {
                for (int z = 0; z < _surfaceDimensions.z; z++)
                {
                    float currentX = x * surfaceUnitDimensions.x + surfaceUnitDimensions.x/2 - (surfaceExtensionsX / 2.0f);
                    float currentY = y * surfaceUnitDimensions.y + surfaceUnitDimensions.y / 2 - surfaceExtensionsY;
                    float currentZ = z * surfaceUnitDimensions.z + surfaceUnitDimensions.z / 2 - (surfaceExtensionsZ / 2.0f);

                    Instantiate(_surfaceUnit, new Vector3(currentX, currentY, currentZ), new Quaternion(), transform);
                }
            }
        }
        NMSurface.BuildNavMesh();
    }

    public void AddSurface(Transform t)
    {
        t.SetParent(transform);
        NMSurface.UpdateNavMesh(NMSurface.navMeshData);
    }
}
