using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet = null; /*怪物的子弹*/

    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>(); /*获取怪物脚本*/
    }

    public void Attack()
    {
        if (enemy.Health > 0)
        {
            Instantiate(bullet, transform.position, Quaternion.identity); /*当怪物的生命值大于 0 时（即未死亡时），并且怪物调动攻击方法时，生成预置体子弹在怪物的位置上*/
        }
        else
            return;
    }
}
