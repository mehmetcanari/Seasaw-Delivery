using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RightLeft : MonoBehaviour
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
        transform.DOLocalMoveX(-4f, 4);
    }

    public void DownBehave()
    {
        upBehave = false;
        transform.DOLocalMoveX(3f, 4);
        Invoke("BoolReturner", 2f);
    }

    public void BoolReturner()
    {
        upBehave = true;
    }
    #endregion
}


