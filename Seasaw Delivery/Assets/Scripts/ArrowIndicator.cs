using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowIndicator : MonoBehaviour
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
        transform.DOLocalMoveY(2f, 2);
    }

    public void DownBehave()
    {
        upBehave = false;
        transform.DOLocalMoveY(1f, 2);
        Invoke("BoolReturner", 2f);
    }

    public void BoolReturner()
    {
        upBehave = true;
    }
    #endregion
}
