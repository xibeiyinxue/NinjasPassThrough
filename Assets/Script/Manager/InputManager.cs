using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : PersistentSingleton<InputManager>
{
    [SerializeField]
    private InputKeyCodeSetting t_KeyCodeData;

    private KeyCode _left;
    private KeyCode _right;
    private KeyCode _jump;
    private KeyCode _attack;

    protected override void Awake()
    {
        _left = t_KeyCodeData._left;
        _right = t_KeyCodeData._right;
        _jump = t_KeyCodeData._jump;
        _attack = t_KeyCodeData._attack;
    }

    void Update()
    {
        if (GameManager.Instance.m_Player != null && GameManager.Instance.TimeScale>0)
        {
            PlayerMove();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.GameOver || GameManager.Instance.GameWin)
            {
                return;
            }
            else
            {
                Esc();
            }
        }
    }

    private void PlayerMove()
    {
        if (Input.GetKey(_left))
        {
            GameManager.Instance.m_Player.Move(0);
        }
        if (Input.GetKey(_right))
        {
            GameManager.Instance.m_Player.Move(1);
        }
        if (Input.GetKeyDown(_jump))
        {
            GameManager.Instance.m_Player.Jump();
        }
        if (Input.GetKeyDown(_attack))
        {
            GameManager.Instance.m_Player.FireOnce();
        }
        if (Input.GetKey(_attack))
        {
            GameManager.Instance.m_Player.FireStart();
        }
    }

    public virtual void Esc()
    {
        if (UIManager.Instance.PauseMenu)
        {
            UIManager.Instance.PauseMenu.Back();
        }
    }

    public void PlayerInputInit()
    {
        _left = KeyCode.LeftArrow; /*Buttons[0]*/
        _right = KeyCode.RightArrow; /*Buttons[1]*/
        _jump = KeyCode.Space; /*Buttons[2]*/
        _attack = KeyCode.Z; /*Buttons[3]*/
    }

    public KeyCode Left { get { return _left; }
        set
        {
            t_KeyCodeData._left = value;
            _left = value;
        }
    }
    public KeyCode Right
    {
        get { return _right; }
        set
        {
            t_KeyCodeData._right = value;
            _right = value;
        }
    }
    public KeyCode Jump
    {
        get { return _jump; }
        set
        {
            t_KeyCodeData._jump = value;
            _jump = value;
        }
    }
    public KeyCode Attack
    {
        get { return _attack; }
        set
        {
            t_KeyCodeData._attack = value;
            _attack = value;
        }
    }
}
