using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            //Debug.Log("Kutu yere düştü");
            transform.parent = collision.gameObject.transform;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            return;
        }
    }
}
