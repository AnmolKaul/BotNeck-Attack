using UnityEngine;

public class PacketMover : MonoBehaviour
{
    [SerializeField] private float speed;

    private Transform server;

    private float damage = 0.1f;
    private float shieldAmount = 0.1f;

    private void Start()
    {
        server = GameObject.FindGameObjectWithTag("Server").transform;
    }

    private void Update()
    {
        // Get direction to the server and move
        Vector3 direction = (server.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.z > -2.2f)
        {
            gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Server" && gameObject.tag == "Virus")
        {
            // Destroy packet when hit with server
            gameObject.SetActive(false);

            Server serverObj = other.GetComponent<Server>();
            if (serverObj != null)
            {
                serverObj.TakeDamage(damage);
            }
            else
                return;

        }

        if (other.gameObject.name == "Server" && gameObject.tag == "Good")
        {
            // Destroy packet when hit with server
            gameObject.SetActive(false);

            // Add value to server's shield
            Server serverObj = other.GetComponent<Server>();

            // Increment good packets collected
            serverObj.GetComponent<Server>().ModifyGoodPackets(1);

            if (serverObj != null)
            {
                serverObj.Antivirus(shieldAmount);
            }
            else
                return;
        }
    }
}
