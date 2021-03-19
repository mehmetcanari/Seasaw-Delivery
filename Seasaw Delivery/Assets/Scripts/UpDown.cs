using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UpDown : MonoBehaviour
{
    public Vector3 indicatorPos;
    private bool upBehave = true;

    #region Up-Down Behave
    void Update()
    {
        if (upBehave)
        {
            UpBehave();
            Invoke("DownBehave", 2);
        }
    }

    public void UpBehave()
    {
        transform.DOLocalMoveY(-4f, 5);
    }

    public void DownBehave()
    {
        upBehave = false;
        transform.DOLocalMoveY(-8f, 5);
        Invoke("BoolReturner", 2f);
    }

    public void BoolReturner()
    {
        upBehave = true;
    }
    #endregion
}
