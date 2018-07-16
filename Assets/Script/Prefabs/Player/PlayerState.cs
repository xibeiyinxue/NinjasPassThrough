[System.Serializable]
public class PlayerState{
    public bool IsWall { get; set; }
    public bool IsJump { get; set; }
    public bool IsHurt { get; set; }
    public bool IsDead { get; set; }
    public bool IsGrabWall { get; set; }
    public bool IsLeftSide { get; set; }
    public bool IsRightSide { get; set; }
    public bool IsCollidingUp { get; set; }
    public bool IsCollidingGround { get; set; }
    public bool IsCollidingBehind { get; set; }
    public bool IsCollidingFront { get; set; }

    public void PlayerStateReset()
    {
            IsWall =
            IsJump =
            IsHurt =
            IsDead =
            IsGrabWall =
            IsLeftSide =
            IsRightSide =
            IsCollidingUp =
            IsCollidingGround =
            IsCollidingBehind=
            IsCollidingFront=
            false;
    }
}
