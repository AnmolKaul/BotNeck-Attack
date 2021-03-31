using UnityEngine;

public class PacketMover : MonoBehaviour
{
    [SerializeField] private float speed;

    private Transform server;

    private void Start()
    {
        server = GameObject.FindGameObjectWithTag("Server").transform;
    }

    private void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, server.position, speed);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Quaternion lookAt = Quaternion.LookRotation(transform.forward, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAt, speed);
    }
}
