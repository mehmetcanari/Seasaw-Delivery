using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemUIScript : MonoBehaviour
{
    public GameObject canvas;
    //public GameObject counter;
    void Start()
    {
        Debug.LogWarning("Var");
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        transform.position = new Vector3(transform.position.x, transform.position.y, canvas.transform.position.z);
        transform.rotation = canvas.transform.rotation;
        transform.parent = canvas.transform;
    }

    void Update()
    {
        //transform.Translate(counter.transform.position);
    }
}
