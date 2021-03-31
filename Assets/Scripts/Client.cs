using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private Transform serverPosition;

    private LineRenderer lineRenderer;

    private void Start()
    {
        // Setting line renderer properties
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.04f;
        lineRenderer.endWidth = 0.04f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.useWorldSpace = true;

        // Setting line renderer position at start of the game
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, serverPosition.position);

        AddCollider(lineRenderer, transform.position, serverPosition.position);
    }

    private void AddCollider(LineRenderer lineRenderer, Vector3 startPoint, Vector3 endPoint)
    {
        // Add collider to the line renderer
        BoxCollider collider = new GameObject(gameObject.name + " Client").AddComponent<BoxCollider>();
        collider.transform.parent = lineRenderer.transform;

        // Set collider size and position
        float lineWidth = lineRenderer.endWidth;
        float lineLength = Vector3.Distance(startPoint, endPoint);
        collider.size = new Vector3(lineLength, lineWidth, 0.1f);

        Vector3 midPoint = (startPoint + endPoint) / 2;
        collider.transform.position = midPoint;

        // Set collider rotation
        float angle = Mathf.Atan2((endPoint.z - startPoint.z), (endPoint.x - startPoint.x));
        angle *= Mathf.Rad2Deg;
        angle *= -1;

        collider.transform.Rotate(0, angle, 0);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Disabling the line on click/touch
                LineRenderer renderer = hit.transform.GetComponentInParent<LineRenderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }

    }
}
