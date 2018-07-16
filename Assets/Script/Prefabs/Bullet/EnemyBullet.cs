using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BulletBase {

    protected Transform _player;

    void Start () {
        myTag = "Enemy";
	}

    protected override void Move()
    {
        if (GameManager.Instance.m_Player != null)
        {
            _player = GameManager.Instance.m_Player.transform;
        }
        if (_player == null)
        {
            return;
        }
        else
        {
            Vector3 dir = _player.position - transform.position;
            transform.position += dir.normalized * speed * Time.deltaTime;
        }
    }

}
