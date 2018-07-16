using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IHealth {
    #region Prefab
    private PlayerState m_State;

    [SerializeField]
    private GameObject m_Bullet;
    #endregion

    #region m_Component 组件
    private Rigidbody2D m_Rig = null;
    private SpriteRenderer m_Rend = null;
    private Animator m_Anim = null;
    private AudioSource m_Audio = null;
    #endregion

    #region BasicInformation 基础信息
    [SerializeField]
    private float m_JumpForce,m_MoveSpeed = 0;
    [SerializeField]
    private int m_GroundGrav,m_WallGrav = 0;
    [SerializeField]
    private Transform m_UpLeft, m_UpRight, m_DownLeft, m_DownRight, m_LeftUp,
                        m_LeftDown, m_RightUp, m_RightDown = null;
    [SerializeField]
    private LayerMask m_LayerMask;

    private float horizontal = 0;
    private Vector2 velocity;

    private float m_FireTimer = 0;
    private float m_FireRate = 0.3f;
    private bool _faceTo;
    public bool FaceTo { get { return _faceTo; } }

    private int _health = 100;
    public int Health { get { return _health; }private set { _health = value; } }
    private int lastHealth = 0;
    #endregion

    void Awake() {
        m_State = new PlayerState();
        m_Rig = GetComponent<Rigidbody2D>();
        m_Rend = GetComponentInChildren<SpriteRenderer>();
        m_Anim = GetComponentInChildren<Animator>();
        m_Audio = GetComponentInChildren<AudioSource>();
        lastHealth = Health;
    }

    void Update()
    {
        InWallSide(); 

        SetAnimPlay();
        SetAudioPlay();

        DetecteRaycasts();

        if (!m_Rend.flipX)
        {
            _faceTo = true;
        }
        else
        {
            _faceTo = false;
        }
    }

    //刚体移动
    void FixedUpdate() {
        if (!m_State.IsDead)
        {
            if (m_State.IsCollidingGround)
            {
                m_Rig.gravityScale = m_GroundGrav; /*地面重力*/
                m_State.IsWall = false;
                if (m_State.IsJump)
                {
                    m_Rig.AddForce(new Vector2(0, m_JumpForce));
                    m_State.IsJump = false;
                }
            }
            else if (m_State.IsLeftSide || m_State.IsRightSide)
            {
                if (!m_State.IsCollidingGround)
                {
                    m_Rig.gravityScale = m_WallGrav;
                    m_Rig.velocity = velocity; //【这句话写在这能使忍者在墙上缓缓滑动】
                    if (m_State.IsJump)
                    {
                        m_Rig.AddForce(new Vector2(0f, m_JumpForce));
                        m_Rig.velocity = new Vector2(velocity.x * m_MoveSpeed, m_Rig.velocity.y); //【这句话写在这能使忍者在墙上跳跃出来】
                        m_State.IsJump = false;
                        m_State.IsWall = false;
                    }
                }
            }
        }
        //当刚体的 X 轴大于 0 时，方向为 Right；
        if (m_Rig.velocity.x > 0)
        {
            m_Rend.flipX = false;
        }
        //当刚体的 X 轴小于 0 时，方向为 Lift；
        else if (m_Rig.velocity.x < 0)
        {
            m_Rend.flipX = true;
        }
    }
    public void Move(int keyId) /*实现左右移动*/
    {
        horizontal = Mathf.Clamp(0f, -1f, 1f);
        if (keyId == 0) /*按下左移动键*/
        {
            if (m_State.IsLeftSide)
            {
                horizontal = 0;
            }
            else
            {
                horizontal = -m_MoveSpeed * Time.deltaTime;
            }
            if (m_State.IsRightSide)
            {
                m_State.IsWall = true;
            }
            velocity = new Vector2(horizontal, 0);
        }
        else if (keyId == 1) /*按下右移动键*/
        {
            if (m_State.IsRightSide)
            {
                horizontal = 0;
            }
            else
            {
                horizontal = m_MoveSpeed * Time.deltaTime;
            }
            if (m_State.IsLeftSide)
            {
                m_State.IsWall = true;
            }
        }
        velocity = new Vector2(horizontal, 0);
        m_Rig.velocity = new Vector2(velocity.x * m_MoveSpeed, m_Rig.velocity.y); //【这句话写在这能进行滑动】
    }
    public void Jump() /*跳跃开关*/{
        if (m_State.IsCollidingGround)
        {
            m_State.IsJump = true;
        }
        else if (m_State.IsLeftSide && m_State.IsWall || m_State.IsRightSide && m_State.IsWall)
        {
            m_State.IsJump = true;
        }
        else
        {
            m_State.IsJump = false;
        }
    }

    #region Fire
    public void FireStart()
    {
        m_FireTimer += Time.deltaTime;
        if (m_FireTimer > m_FireRate)
        {
            CreateBullet();
            m_FireTimer = 0;
        }
    }
    public void FireOnce()
    {
        CreateBullet();
        m_FireTimer = 0;
    }

    private void CreateBullet()
    {
        Instantiate(m_Bullet, transform.position, Quaternion.identity);
    }
    #endregion

    //在墙边
    private void InWallSide()
    {
        if (m_State.IsCollidingFront)
        {
            m_Rend.flipX = true;
        }
        else if (m_State.IsCollidingBehind)
        {
            m_Rend.flipX = false;
        }
    }

    //动画控制
    private void SetAnimPlay() {
        Vector2 velocit = m_Rig.velocity;

        if (m_State.IsCollidingGround)
        {
            m_Anim.SetFloat("speedPercent", velocit.magnitude / m_MoveSpeed);
        }
        else if (!m_State.IsCollidingGround)
        {
            m_Anim.SetFloat("jump", velocit.y / 0.018f);
        }
        m_Anim.SetBool("isGround", m_State.IsCollidingGround);
        m_Anim.SetBool("isWall", m_State.IsLeftSide || m_State.IsRightSide);
    }
    //音效控制
    private void SetAudioPlay()
    {
        if (m_State.IsJump)
        {
            m_Audio.Play();
        }
    }

    //射线检测
    void DetecteRaycasts()
    {
        m_State.IsCollidingUp =
            Physics2D.Raycast(m_UpLeft.position, Vector2.up, 0.1f, m_LayerMask) || Physics2D.Raycast(m_UpRight.position, Vector2.up, 0.1f, m_LayerMask);
        m_State.IsCollidingGround =
            Physics2D.Raycast(m_DownLeft.position, Vector2.down, 0.1f, m_LayerMask) || Physics2D.Raycast(m_DownRight.position, Vector2.down, 0.1f, m_LayerMask);
        m_State.IsCollidingBehind =
            Physics2D.Raycast(m_LeftUp.position, Vector2.left, 0.1f, m_LayerMask) || Physics2D.Raycast(m_LeftDown.position, Vector2.left, 0.1f, m_LayerMask);
        m_State.IsCollidingFront =
            Physics2D.Raycast(m_RightUp.position, Vector2.right, 0.1f, m_LayerMask) || Physics2D.Raycast(m_RightDown.position, Vector2.right, 0.1f, m_LayerMask);

        m_State.IsLeftSide =
            Physics2D.Raycast(m_LeftUp.position, Vector2.left, 0.1f, m_LayerMask) && Physics2D.Raycast(m_LeftDown.position, Vector2.left, 0.1f, m_LayerMask);
        m_State.IsRightSide =
            Physics2D.Raycast(m_RightUp.position, Vector2.right, 0.1f, m_LayerMask) && Physics2D.Raycast(m_RightDown.position, Vector2.right, 0.1f, m_LayerMask);

        RaycastHit2D on = Physics2D.Raycast(transform.position, Vector2.down,2f, m_LayerMask);
        if (on && on.collider.gameObject.layer == 9)
        {
            transform.parent = on.transform;
        }
        else
        {
            transform.parent = null;
        }
    }
    //Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(m_UpLeft.position, m_UpLeft.position + Vector3.up * 0.1f);
        Gizmos.DrawLine(m_UpRight.position, m_UpRight.position + Vector3.up * 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_DownLeft.position, m_DownLeft.position + Vector3.down * 0.1f);
        Gizmos.DrawLine(m_DownRight.position, m_DownRight.position + Vector3.down * 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(m_LeftUp.position, m_LeftUp.position + Vector3.left * 0.1f);
        Gizmos.DrawLine(m_LeftDown.position, m_LeftDown.position + Vector3.left * 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(m_RightUp.position, m_RightUp.position + Vector3.right * 0.1f);
        Gizmos.DrawLine(m_RightDown.position, m_RightDown.position + Vector3.right * 0.1f);
    }

    public void Damage(int damage,GameObject initiator)
    {
        if (!m_State.IsHurt)
        {
            Health -= damage;
            if (Health != lastHealth && Health >= 0)
            {
                m_State.IsHurt = true;
                if (m_State.IsHurt)
                {
                    StartCoroutine(DelayColl());
                }
            }
        }
        if (Health <= 0)
        {
            DestroySelf();
        }
    }

    IEnumerator DelayColl() {
        lastHealth = Health; /*上一次生命等于现在生命减去受到伤害的值*/
        for (int i = 1; i < 5; i++) {
            if (i % 2 == 1) /*当 i 除 2 余 1 时，主角的 alpha = 1 ，并进行 While 循环让 alpha -= Time.deltaTime ,实现渐隐*/
            {
                float alpha = 1;
                while (alpha > 0)
                {
                    m_Rend.color = new Color(m_Rend.color.r, m_Rend.color.g, m_Rend.color.b, alpha -= Time.deltaTime);
                    yield return null;
                }
            }
            if (i % 2 == 0) /*当 i 除 2 余 0 时，主角的 alpha = 0 ，并进行 While 循环让 alpha += Time.deltaTime ,实现逐渐浮现*/
            {
                float alpha = 0;
                while (alpha < 1)
                {
                    m_Rend.color = new Color(m_Rend.color.r, m_Rend.color.g, m_Rend.color.b, alpha += Time.deltaTime);
                    yield return null;
                }
            }
        }
        yield return new WaitForSeconds(3f);
        m_State.IsHurt = false;
    }

    public void DestroySelf()
    {
        LevelDirector.Instance.OnPlayerDead();

        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0f);
        Destroy(this.gameObject);
    }
}
