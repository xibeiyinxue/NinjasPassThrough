using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase {

    private Animator m_Anim;
    private BoxCollider2D m_Coll2D = null;
    private SpriteRenderer m_Rend = null;
    
    private bool m_Determination; /*判定*/

    //private EnemyFactory enemyCount;

    private void Awake()
    {
        m_Bullet = GetComponentInChildren<EnemyAttack>();
        m_Anim = GetComponentInChildren<Animator>();
        m_Coll2D = GetComponentInChildren<BoxCollider2D>();
        m_Rend = GetComponentInChildren<SpriteRenderer>();
        //enemyCount = GetComponentInParent<EnemyFactory>();
    }
	
	void Update () {
        if (playerTrans == null) return;

        if (!m_Die)
        {
            distance = Vector3.Distance(playerTrans.position, transform.position);
            m_CD += Time.deltaTime;

            BoolTrueOrFalse();
            LookFor();

            if (!m_Sober)
            {
                transform.position = new Vector3(transform.position.x,
                    Mathf.Clamp(transform.position.y, transform.position.y - 1, transform.position.y + 1),
                    transform.position.z);
            }

            if (m_StepOn && playerTrans)
            {
                Damage(20, this.gameObject);
            }
            if (Health <= 0)
            {
                m_Die = true;
            }
            SetAnimPlay();
        }
        else if (m_Die && m_Coll2D.enabled)
        {
            StartCoroutine(DestorySelf());
        }
	}

    //怪物的布尔值（开关）检测
    private void BoolTrueOrFalse()
    {
        if (distance <= m_DetectionRange)/*当范围的值小于检测范围时*/
        {
            m_Sober = true; /*怪物的清醒开关开启*/
            if (distance <= m_AttackRange)/*当范围值小于攻击范围时*/
                m_AttackBool = true;/*怪物的攻击开关开启*/
            else
                m_AttackBool = false;/*怪物的攻击开关关闭*/
        }
        else
            m_Sober = false;
    }

    //看向玩家
    private void LookFor()
    {
        if (playerTrans.position.x < transform.position.x)
        {
            m_Rend.flipX = true;
        }
        else
        {
            m_Rend.flipX = false;
        }
    }

    //怪物移动
    private void Move()
    {
        if (m_AttackBool)
        {
            return;     /*————————————这里有点不一样*/
        }
        else
        {
            Vector3 dir = playerTrans.position - transform.position;
            transform.position = dir.normalized * 8f * Time.deltaTime + transform.position;
        }
    }

    private void SetAnimPlay()
    {
        if (!m_Die)
        {
            if (m_Sober)
            {
                //怪物苏醒
                m_Anim.SetBool("sober", m_Sober);
                Move();
                if (m_AttackBool && m_CD >= 4f)
                {
                    m_CD = 0f;

                    StartCoroutine(Attack());

                    m_Anim.SetTrigger("coolDown");
                }
            }
            if (!m_Sober)
            {
                m_Anim.SetBool("sober", m_Sober);
            }
        }
    }

    //探测范围、攻击范围以及头顶判定射线
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_DetectionRange);/*怪物的检测范围为黄色的圆*/
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_AttackRange);/*怪物的攻击范围为红色的圆*/
    }

    private IEnumerator Attack() {
        m_Anim.SetTrigger("attack");
        yield return new WaitForSeconds(m_Anim.GetCurrentAnimatorStateInfo(0).length);
        if(playerTrans != null)
        {
            m_Bullet.Attack();
        }
    }

    private IEnumerator DestorySelf()
    {
        m_Anim.SetTrigger("die");
        yield return StartCoroutine(Fade());
        this.gameObject.SetActive(false);
        //enemyCount.EnemyAmount--;
    }

    private IEnumerator Fade()
    {
        float alpha = 1f;
        yield return new WaitForSeconds(m_Anim.GetCurrentAnimatorClipInfo(0).Length);
        m_Coll2D.enabled = false;
        while (alpha > 0)
        {
            m_Rend.color = new Color(m_Rend.color.r, m_Rend.color.g, m_Rend.color.b, alpha -= Time.deltaTime);
            yield return null;
        }
    }
}
