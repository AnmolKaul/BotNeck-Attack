using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Client : MonoBehaviour
{
    private Transform serverPosition;

    private LineRenderer lineRenderer;
    private LineRenderer cashedLine;

    [SerializeField] private GameObject virusPacketPrefab;
    [SerializeField] private GameObject goodPacketPrefab;

    private float virusPacketSpawnTime;
    private float goodPacketSpawnTime;
    public float startvirusPacketSpawnTime;
    public float startGoodPacketSpawnTime;

    [SerializeField] private float virusPacketDecreaseTime;
    [SerializeField] private float virusPacketMintime;

    [HideInInspector] public bool canSpawnPackets = true;

    private void Start()
    {
        serverPosition = GameObject.Find("Server Pos").transform;
        // Setting line renderer properties
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
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

        // Add line collider script
        collider.gameObject.AddComponent<LineCollider>();

        // Set collider size and position
        float lineWidth = lineRenderer.endWidth;
        float lineLength = Vector3.Distance(startPoint, endPoint);
        collider.size = new Vector3(lineLength, lineWidth, 0.2f);

        Vector3 midPoint = (startPoint + endPoint) / 2;
        collider.transform.position = midPoint;

        // Set collider rotation
        float angle = Mathf.Atan2((endPoint.z - startPoint.z), (endPoint.x - startPoint.x));
        angle *= Mathf.Rad2Deg;
        angle *= -1;

        collider.transform.Rotate(0, angle, 0);
    }

    private IEnumerator DisableLine(LineRenderer line)
    {
        line.enabled = false;

        // Get the script from parent and stop packets from spawning
        Client client = line.gameObject.GetComponent<Client>();
        client.canSpawnPackets = false;

        yield return new WaitForSeconds(3);
        line.enabled = true;
        client.canSpawnPackets = true;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Get the line renderer of the tapped line
                cashedLine = hit.transform.GetComponentInParent<LineRenderer>();

                // Disable the line for some time
                if (cashedLine != null)
                {
                    StartCoroutine(DisableLine(cashedLine));
                }

                // Destroy all packets which are still active on the tapped line
                LineCollider lineCollider = hit.transform.gameObject.GetComponent<LineCollider>();

                foreach (var collider in lineCollider.GetColliders())
                {
                    Destroy(collider.gameObject);
                }

                // Increment virus terminated count 
                Server.instance.ModifyVirusPackets(lineCollider.GetDestoyedColliderCount());
                lineCollider.ClearColliderSet();

            }
        }

        #region  Packet Spawning

        // Virus Packet spawning
        if (virusPacketSpawnTime <= 0 && canSpawnPackets)
        {
            GeneratePacket(virusPacketPrefab);
            virusPacketSpawnTime = startvirusPacketSpawnTime;
            if (startvirusPacketSpawnTime > virusPacketMintime)
            {
                startvirusPacketSpawnTime -= virusPacketDecreaseTime;
            }
        }
        else
        {
            virusPacketSpawnTime -= Time.deltaTime;
        }

        // Good packet spawning
        if (goodPacketSpawnTime <= 0 && canSpawnPackets)
        {
            GeneratePacket(goodPacketPrefab);
            goodPacketSpawnTime = startGoodPacketSpawnTime;
        }
        else
        {
            goodPacketSpawnTime -= Time.deltaTime;
        }
        #endregion
    }

    private void GeneratePacket(GameObject packet)
    {
        Instantiate(packet, transform.position, Quaternion.identity);
    }
}
