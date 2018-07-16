using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VerticalMoveGround : MonoBehaviour {

    [SerializeField]
    private bool Yoyo = false;
    [SerializeField]
    private float high;
    [SerializeField]
    private float speed;

    void Start () {
        if (Yoyo)
        {
            transform.DOMoveY(high, speed).SetRelative().SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            transform.DOMoveY(high, speed).SetRelative().SetLoops(-1, LoopType.Restart);
        }
    }
}
