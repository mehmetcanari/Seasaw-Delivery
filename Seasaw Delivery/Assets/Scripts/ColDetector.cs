using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColDetector : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "box")
        {
            Debug.Log("Kutular birbirine çarptı");
            rb.AddForce(2, 2, 0, ForceMode.Impulse);
        }

        if (collision.gameObject.tag == "ground")
        {
            //Atılan kutular yere değmeden, ikinci kutuyu atmaya izin vermemesi lazım

            Debug.Log("Kutu yere düştü");
            transform.parent = collision.gameObject.transform;
            //rb.constraints = RigidbodyConstraints.FreezePositionX;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            //gameObject.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            Debug.Log("Kutuya çarpmadı");
            return;
        }
    }
}
