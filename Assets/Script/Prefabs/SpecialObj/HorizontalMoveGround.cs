using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HorizontalMoveGround : MonoBehaviour {
    void Start()
    {
        transform.DOMoveX(-40f, 8f).SetRelative().SetLoops(-1, LoopType.Yoyo);
    }
}
