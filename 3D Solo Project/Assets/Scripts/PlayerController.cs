using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour, IIdle, IMove, ICheckIsGround, IFallen, ILanding, IJump, IUpDownStair, IAttack
{
    private PlayerData playerData;//ÇÃ·¹ÀÌ¾î µ¥ÀÌÅÍ
    private Rigidbody playerRb;//¸®Áöµå ¹Ùµğ
    private Camera cam;//¸ŞÀÎ Ä«¸Ş¶ó
    private IPlayerState currentState;//ÇÃ·¹ÀÌ¾î »óÅÂ
    private AnimationController anime;
    private RaycastHit sloopHit;
    private Vector3 moveDir;//¿òÁ÷ÀÌ´Â ¹æÇâ
    private Vector3 featPos;//¹ß À§Ä¡
    private Vector2 inputMoveDir;//ÀÎÇ² º¯¼ö ¹Ş¾Æ¿È

    private void Awake()
    {
        playerData = GetComponent<PlayerData>();
        playerRb = GetComponent<Rigidbody>();
        currentState = new PlayerIdleState();
        currentState.Enter(this);
        cam = Camera.main;
<<<<<<< HEAD
<<<<<<< HEAD
        anime = GetComponent<AnimationController>();
        moveDir = Vector3.zero;
=======
>>>>>>> parent of 39136e69 (feat: ìƒíƒœíŒ¨í„´ ë¦¬íŒ©í† ë§)
=======
>>>>>>> parent of 39136e69 (feat: ìƒíƒœíŒ¨í„´ ë¦¬íŒ©í† ë§)
        playerData.UpperRay.position = new Vector3(playerData.UpperRay.position.x, playerData.StepHight, playerData.UpperRay.position.y);
    }

    public PlayerData PlayerData { get => playerData; set => playerData = value; }
<<<<<<< HEAD
<<<<<<< HEAD
    public Rigidbody PlayerRb { get => playerRb; set => playerRb = value; }
    public Camera Cam { get => cam; set => cam = value; }
    public IPlayerState CurrentState { get => currentState; set => currentState = value; }
    public AnimationController Anime { get => anime; set => anime = value; }
    public RaycastHit SloopHit { get => sloopHit; set => sloopHit = value; }
    public Vector3 MoveDir { get => moveDir; set => moveDir = value; }
    public Vector3 InputMoveDir { get => inputMoveDir; set => inputMoveDir = value; }

    public void FixedUpdated()
    {
        if (currentState != null)
        {
            currentState.Move();
            currentState.Rotation();
        }
    }

=======
    public Vector3 InputMoveDir { get => inputMoveDir; set => inputMoveDir = value; }

>>>>>>> parent of 39136e69 (feat: ìƒíƒœíŒ¨í„´ ë¦¬íŒ©í† ë§)
=======
    public Vector3 InputMoveDir { get => inputMoveDir; set => inputMoveDir = value; }

>>>>>>> parent of 39136e69 (feat: ìƒíƒœíŒ¨í„´ ë¦¬íŒ©í† ë§)
    public void Updated()
    {
        if(currentState != null)
        {
            currentState.Update();
            currentState.Attack();
        }
    }

    //ÇÃ·¹ÀÌ¾î »óÅÂ º¯È­ ½ÃÄÑÁÖ´Â ÄÚµå
    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(this);
    }

    //¸í·É ³»¸®±â
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
    }

    //°¡¸¸È÷ ÀÖ´Â »óÅÂ
    public void Idle()
    {
        OnSloop();
        playerRb.velocity = Vector3.zero;
        playerData.Magnitude = playerRb.velocity.magnitude;
    }

    //¿òÁ÷ÀÓ
    public void Move()
    {
        moveDir = cam.transform.forward * inputMoveDir.y + cam.transform.right * inputMoveDir.x;
        moveDir.y = 0;
        moveDir.Normalize();
        OnSloop();
        if (playerData.IsSprint)
        {
            playerRb.velocity = moveDir * playerData.PlayerSprintSpeed;
        }
        else
        {
            playerRb.velocity = moveDir * playerData.PlayerMoveSpeed;
        }
        playerData.Magnitude = playerRb.velocity.magnitude;
    }

    //ÇÃ·¹ÀÌ¾î È¸Àü
    public void Rotation()
    {
        Vector3 targetdir = (cam.transform.forward * inputMoveDir.y) + (cam.transform.right * inputMoveDir.x);
        targetdir.Normalize();
        targetdir.y = 0;
        if(targetdir == Vector3.zero)
        {
            targetdir = transform.forward;
        }
        Quaternion roDir = Quaternion.LookRotation(targetdir);
        Quaternion playerRo = Quaternion.Lerp(transform.rotation, roDir, playerData.PlayerRotationSpeed * Time.deltaTime);
        transform.rotation = playerRo;
    }

    //ÇÃ·¹ÀÌ¾î°¡ ¶Ù³ª ¾È¶Ù³ª ¿©ºÎ ÆÇ´Ü
    public void SetSprint(bool TorF)
    {
        playerData.IsSprint = TorF;
    }

    //¶Ù´ÂÁö ¿©ºÎ Àü´Ş
    public bool GetSprint()
    {
        return playerData.IsSprint;
    }

    //ÇÃ·¹ÀÌ¾î°¡ ¶¥¿¡ ÀÖ´ÂÁö ¿©ºÎ ÆÇ´Ü
    public bool CheckIsGround()
    {
        Vector3 originalPos = transform.position;
        originalPos.y += playerData.RayCastHightOffset;

        var coll = Physics.OverlapSphere(originalPos, playerData.FallenSphereRadius, playerData.GroundLayerMask);
        bool isGround = coll.Length > 0;
<<<<<<< HEAD
<<<<<<< HEAD

=======
=======
>>>>>>> parent of 39136e69 (feat: ìƒíƒœíŒ¨í„´ ë¦¬íŒ©í† ë§)
        //bool isGround = Physics.SphereCast(originalPos, playerData.FallenSphereRadius, -Vector3.up, out hit, playerData.GroundLayerMask);
        
>>>>>>> parent of 39136e69 (feat: ìƒíƒœíŒ¨í„´ ë¦¬íŒ©í† ë§)
        return isGround;
    }

    private void OnDrawGizmos()
    {
        Vector3 originalPos = transform.position;
        originalPos.y += playerData.RayCastHightOffset;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(originalPos, playerData.FallenSphereRadius);
    }

    //ÇÃ·¹ÀÌ¾î Ãß¶ô ¹× ·£µù
    public void PlayerFallenAndLanding()
    {
        Vector3 originalPos = transform.position;
        originalPos.y += playerData.RayCastHightOffset;
        RaycastHit hit;
        
        if(!playerData.IsGround)
        {
            playerData.InAirTime += Time.deltaTime;
            playerRb.AddForce(-Vector3.up * playerData.PlayerFallenSpeed * playerData.InAirTime);
        }

        if (Physics.SphereCast(originalPos, playerData.FallenSphereRadius, -Vector3.up, out hit, playerData.GroundLayerMask))
        {
            playerData.IsGround = true;
            playerData.InAirTime = 0;
        }
        else
        {
            playerData.IsGround = false;
        }
    }

    //ÇÃ·¹ÀÌ¾î Ãß¶ô
    public void Fallen()
    {
        playerData.InAirTime += Time.deltaTime;
        playerRb.AddForce(-Vector3.up * playerData.PlayerFallenSpeed * playerData.InAirTime);
    }

    //ÇÃ·¹ÀÌ¾î ÂøÁö
    public void Landing()
    {
        playerData.IsGround = true;
        playerData.InAirTime = 0;
    }

    //ÇÃ·¹ÀÌ¾î Á¡ÇÁ
    public void Jump()
    {
        playerRb.useGravity = true;
        float playerHight = Mathf.Sqrt(-2 * playerData.GravityForce * playerData.JumpPower);
        Vector3 player = playerRb.velocity;
        player.y = playerHight;
        playerRb.velocity = player;
        Debug.Log("Á¡ÇÁÇÔ");
    }

    //Á¡ÇÁ Âü°ÅÁş
    public void SetJump(bool TorF)
    {
        playerData.IsJump = TorF;
    }

    //Á¡ÇÁ Âü°ÅÁş ¹İÈ¯
    public bool GetJump()
    {
        return playerData.IsJump;
    }

    //°è´Ü ÀÎÁö ¾Æ´ÑÁö
    public bool CheckUpDownStair()
    {
        RaycastHit rawHit;
        if (Physics.Raycast(playerData.RawerRay.position, transform.TransformDirection(Vector3.forward), out rawHit, 0.1f) && IsToHigh())
        {
            RaycastHit upperHit;
            if (!Physics.Raycast(playerData.UpperRay.position, transform.TransformDirection(Vector3.forward), out upperHit, 0.2f))
            {
                return true;
            }
        }

        RaycastHit rawHit45;
        if (Physics.Raycast(playerData.RawerRay.position, transform.TransformDirection(1.5f, 0, 1), out rawHit45, 0.1f) && IsToHigh())
        {
            RaycastHit upperHit45;
            if (!Physics.Raycast(playerData.UpperRay.position, transform.TransformDirection(1.5f, 0, 1), out upperHit45, 0.2f))
            {
                return true;
            }
        }

        RaycastHit rawHitmin45;
        if (Physics.Raycast(playerData.RawerRay.position, transform.TransformDirection(-1.5f, 0, 1), out rawHitmin45, 0.1f) && IsToHigh())
        {
            RaycastHit upperHitmin45;
            if (!Physics.Raycast(playerData.UpperRay.position, transform.TransformDirection(-1.5f, 0, 1), out upperHitmin45, 0.2f))
            {
                return true;
            }
        }

        return false;
    }

    //°è´Ü ¿À¸£³»¸®±â
    public void UpDownStair()
    {
        playerRb.position -= new Vector3(0, -playerData.StepSmooth, 0);
    }

    //°æ»ç·ÎÀÎÁö ±¸ºĞ¹ı
    public bool IsOnSloop()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out sloopHit, playerData.MaxDistance, playerData.GroundLayerMask))
        {
            playerData.AngleResult = Vector3.Angle(Vector3.up, sloopHit.normal);
            return playerData.IsSloop = playerData.AngleResult != 0 && IsToHigh();
        }
        return playerData.IsSloop = false;
    }

    //°æ»ç·Î°¡ ³Ê¹« ³ôÀºÁö ÆÇº°
    public bool IsToHigh()
    {
        if (playerData.AngleResult < playerData.MaxAngle)
        {
            return true;
        }
        return false;
    }

    //°æ»ç·Î ÀÏ¶§ Çàµ¿
    public void OnSloop()
    {
        IsOnSloop();

        if (playerData.IsSloop)
        {
            moveDir = Vector3.ProjectOnPlane(moveDir, sloopHit.normal);
            playerRb.useGravity = false;
            Debug.Log(playerData.IsSloop + " °æ»ç¸é");
        }
        else if (!NextFrameIsSloop())
        {
            moveDir = Vector3.zero;
            playerRb.velocity = Vector3.zero;
        }
        else
        {
            playerRb.useGravity = true;
        }
    }

    //´ÙÀ½ Ä³¸¯ÅÍÀÇ °¢µµ °è»ê
    public bool NextFrameIsSloop()
    {
        RaycastHit hit;
        var nextPlayerPos = transform.position + (moveDir * playerData.PlayerMoveSpeed * Time.fixedDeltaTime);
        if (Physics.Raycast(nextPlayerPos, Vector3.down, out hit, playerData.MaxDistance, playerData.GroundLayerMask))
        {
            float nextAngle = Vector3.Angle(Vector3.up, hit.normal);
            return nextAngle < playerData.MaxAngle;
        }
        return false;
    }

    //°ø°İ Âü °ÅÁş
    public void SetAttack(bool TorF)
    {
        playerData.IsAttack = TorF;
    }

    public IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(2.02f);
        playerData.IsAttack = false;
        anime.ResetAttackAnime();
    }

    public void A()
    {

    }
}
