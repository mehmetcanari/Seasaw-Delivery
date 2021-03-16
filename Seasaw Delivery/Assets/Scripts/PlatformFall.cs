using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = transform.parent.gameObject.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.tag == "Destroy")
        {
            Destroy(transform.parent.gameObject);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
