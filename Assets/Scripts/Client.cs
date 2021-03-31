using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private Transform serverPosition;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, serverPosition.position);

        Mesh lineMesh = new Mesh();
        lineRenderer.BakeMesh(lineMesh, Camera.main, true);

        MeshCollider collider = transform.gameObject.AddComponent<MeshCollider>();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.name);
        }

    }
}
