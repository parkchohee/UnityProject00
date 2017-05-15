using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public enum PlayerAnimationType
    {
        NORMAL_IDLE = 0,
        NORMAL_WALK = 1,
        NORMAL_JUMP = 2,
        NORMAL_DIE = 3,
        NORMAL_RESPAWN = 4,

        SPARKCHER_BASIC_ATTACK = 10,
        SPARKCHER_SKILL_ATTACK_1 = 11,
        SPARKCHER_SKILL_ATTACK_2 = 12,
        SPARKCHER_SKILL_ATTACK_3 = 13,

        WORRIOR_BASIC_ATTACK = 20,
        WORRIOR_SKILL_ATTACK_1 = 21,
        WORRIOR_SKILL_ATTACK_2 = 22,
        WORRIOR_SKILL_ATTACK_3 = 23

    }

    public bool isLocalPlayer = true;   // TODO : 네트워크에서 변경할것..

    public float runSpeed = 1.5f;
    public float rotationSpeed = 360.0f;
    float jumpPower = 0.0f;
    float jumpSpeed = 13.0f;

    public int AttackPower = 0;

    protected CharacterController pcController;
    protected Animator animator;
    protected PlayerAnimationType animationType;

    public virtual void Start()
    {
        pcController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        animationType = PlayerAnimationType.NORMAL_IDLE;
    }

    void Update()
    {
        // >> : 로컬 플레이어가 아니면 입력 받지 않는다..
        if (!isLocalPlayer)
            return;
        // << :

        // >> : 플레이어가 공격모션일때 키입력 막는다..
        if ((animationType != PlayerAnimationType.NORMAL_IDLE) &&
        (animationType != PlayerAnimationType.NORMAL_JUMP) &&
        (animationType != PlayerAnimationType.NORMAL_WALK))
            return;
        // << :

        CharacterControl_Slerp();
        CharacterAttackControl();

        // >> : animator의 Index 업데이트..
        animator.SetInteger("AnimationIndex", animationType.GetHashCode());

    }

    void CharacterControl_Slerp()
    {
        Vector3 direction = new Vector3(0, 0, 0);

        animationType = PlayerAnimationType.NORMAL_IDLE;

        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow))
        {
            direction.x = Input.GetAxis("Horizontal");
            direction.z = Input.GetAxis("Vertical");

            Vector3 forward = Vector3.Slerp(transform.forward,
                                       direction,
                                       rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction));
            transform.LookAt(transform.position + forward);

            animationType = PlayerAnimationType.NORMAL_WALK;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            jumpPower = jumpSpeed;
        }

        if (jumpPower > 0)
        {
            jumpPower -= Time.deltaTime * 10;

            animationType = PlayerAnimationType.NORMAL_JUMP;
        }
        else
        {
            jumpPower = 0.0f;
        }

        Vector3 jump = new Vector3(0, jumpPower, 0);
        pcController.Move((direction * runSpeed + Physics.gravity + jump) * Time.deltaTime);
    }

    void CharacterAttackControl()
    {
        if (Input.GetKeyDown(KeyCode.X))
            BasicAttack();
       
    }

    public virtual void BasicAttack()
    {
    }

    public virtual void Skill_1()
    {
    }

    public virtual void Skill_2()
    {
    }

    public virtual void Skill_3()
    {
    }

    public void SetPlayerAnimationType(PlayerAnimationType _animationType)
    {
        animationType = _animationType;
        animator.SetInteger("AnimationIndex", animationType.GetHashCode());
    }

    public PlayerAnimationType GetAnimationType()
    {
        return animationType;
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    //if (hit.gameObject.CompareTag("Enemy"))
    //    //    Debug.Log("")
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    GameObject hitObject = other.gameObject;
    //       print("Rigidbody충돌 " + hitObject.name + "충돌");
    //}
}
