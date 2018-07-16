using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BulletBase {

    private bool m_FaceTo = true;

    void Start () {
        myTag = "Player";
        m_FaceTo = GameManager.Instance.m_Player.FaceTo;
	}

    protected override void Move()
    {
        Vector3 vec3 = new Vector3();
        if (m_FaceTo)
        {
            vec3 = Vector2.right * speed * Time.deltaTime;
            transform.Rotate(Vector3.forward *speed);
        }
        else
        {
            vec3 = Vector2.left * speed * Time.deltaTime;
            transform.Rotate(Vector3.back*speed) ;
        }
        transform.position += vec3;
    }
}
