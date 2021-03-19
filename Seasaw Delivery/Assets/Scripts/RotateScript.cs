using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RotateScript : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 2, 0 * Time.deltaTime);
    }
}
