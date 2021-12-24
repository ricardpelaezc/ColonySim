using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Colonist _colonistSelected;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit))
            {
                if (hit.transform.tag == "Colonist")
                {
                    _colonistSelected = hit.transform.GetComponent<Colonist>();
                }
                else
                {
                    if (_colonistSelected != null)
                    {
                        _colonistSelected.Move(hit.point);
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Surface")
                {
                    hit.transform.gameObject.SetActive(false);
                    SurfaceGenerator.NMSurface.BuildNavMesh();
                }
            }
        }
    }
}
