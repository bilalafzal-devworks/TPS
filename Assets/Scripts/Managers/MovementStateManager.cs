using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    [Header("Grounded-Msg")]
    [SerializeField] bool groundMsg;

    [Header("Input")]
    public float hzInput, vInput;
    #region Speed
    [Header("MovementSpeed")]
    public float currentSpeed;
    public float walkSpeed = 5f, walkBackSpeed = 2f;
    public float runSpeed = 7f, runBackSpeed = 5f;
    public float crouchSpeed = 2f, crouchBackSpeed = 1f;
    #endregion
    [SerializeField] CharacterController characterController;
    [HideInInspector] public Vector3 dir = Vector3.zero; //we will make its directional vetor for player movement


    [Header("GravitationalForce")]
    [SerializeField] float gravity = -9.81f;

    [Header("GroundMask")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundYOffset = 1f;
    [SerializeField] Vector3 spherePos;

    Vector3 velocity = Vector3.zero;
    [HideInInspector] public Animator anim;

    #region MovementStates
    [HideInInspector] public MovementBaseState currentState;
    [HideInInspector] public MovementBaseState previousState;
    [HideInInspector] public IdleState idleState = new IdleState();
    [HideInInspector] public WalkingState walkingState = new WalkingState();
    [HideInInspector] public RunningState runningState = new RunningState();
    [HideInInspector] public CrouchState crouchState = new CrouchState();
    [HideInInspector] public JumpState jumpState = new JumpState();
    #endregion

    #region JumpState Parameters
    [HideInInspector] public bool isJumped;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float airSpeed = 1.5f;
    Vector3 airDir = Vector3.zero;

    #endregion

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController.skinWidth = 0.0001f;
        SwitchState(idleState);

    }
    void Update()
    {
        // if (!characterController || !anim)
        // {
        //     Debug.Log("character controller or Animator is not asssigned!");
        //     return;
        // }
        GetDirectionAndMovement();
        // anim.SetFloat("hzInput", hzInput);
        // anim.SetFloat("vInput", vInput);
        ApplyGravity();
        Falling();
        currentState.UpdateState(this);
    }

    void GetDirectionAndMovement()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        anim.SetFloat("hzInput", hzInput);
        anim.SetFloat("vInput", vInput);
        Vector3 airDir = Vector3.zero;
        if (!isGrounded())
            airDir = transform.forward * vInput + transform.right * hzInput;
        else
            dir = transform.forward * vInput + transform.right * hzInput;
        // "Vector-Normalization" cause without out it, diagonal movement speed increase then actual speed 
        characterController.Move((dir.normalized * currentSpeed + airDir.normalized * airSpeed) * Time.deltaTime);
    }

    public bool isGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, characterController.radius - 0.05f, groundMask))
        {
            if (groundMsg)
            {
                Debug.Log("Player is grounded :)");
            }
            return true;
        }
        else
            return false;
    }
    void OnDrawGizmos()
    {
        //characterController = GetComponent<CharacterController>();
        //if (!characterController) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spherePos, characterController.radius - 0.05f);
    }

    void ApplyGravity()
    {
        if (isGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        //means grounded nhi hai
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    public void JumpForce() => velocity.y += jumpForce;
    public void Jumped() => isJumped = true;

    void Falling() => anim.SetBool("Falling", !isGrounded());
}
