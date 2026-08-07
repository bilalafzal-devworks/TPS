using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SocialPlatforms;
using TreeEditor;
using UnityEngine.UIElements;
public class AimStateManager : MonoBehaviour
{
    [SerializeField] Transform camFollowPos;
    [SerializeField] float xAxis, yAxis; //cinemachine x and y-axis inputs
    [SerializeField] float mouseSensitivity = 1f;

    #region AimStates
    [HideInInspector] public AimBaseState currentState;
    [HideInInspector] public Hip hipFireState = new Hip();
    [HideInInspector] public ADS adsState = new ADS();

    #endregion
    [HideInInspector] public Animator anim;
    [Header("CineMachine")]
    #region Cinemachine

    [HideInInspector] CinemachineVirtualCamera vCam;
    public float adsFov = 50f;
    public float hipFov;
    public float currentFov;
    public float fovSmoothSpeed = 10f;
    #endregion

    #region WeaponAiming
    [SerializeField] float aimSmoothPos = 5f;
    public Transform aimPos;
    [SerializeField] LayerMask aimMask;

    [SerializeField] Transform muzzlepos;

    #endregion


    #region ShoulderSwap
    [Header("Shoulder Swap")]
    float xCamPos, yCamPos, ogYCamPos;
    [SerializeField] float yCamHeight = 0f;
    float returnCamSpeed = 10f;
    MovementStateManager movement;
    #endregion

    void Start()
    {
        //Debug.Log("adsFov = " + adsFov);
        movement = GetComponent<MovementStateManager>();
        xCamPos = camFollowPos.localPosition.x;
        ogYCamPos = camFollowPos.localPosition.y; //taking backup of current yPos
        yCamPos = ogYCamPos;

        vCam = FindAnyObjectByType<Cinemachine.CinemachineVirtualCamera>();
        hipFov = vCam.m_Lens.FieldOfView;
        anim = GetComponent<Animator>();

        if (!anim)
            Debug.Log("Animator not assigned");
        if (!vCam)
            Debug.Log("vCam not assigned");

        SwitchState(hipFireState);

    }

    void Update()
    {
        if (!anim)
            Debug.Log("Animator not assigned");
        if (!vCam)
            Debug.Log("vCam not assigned");

        xAxis += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        yAxis = Mathf.Clamp(yAxis, -80, 80);


        //Screen centre Calculating
        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);
        //raycast
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothPos * Time.deltaTime);

        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);
        ShoulderSwap();
        currentState.UpdateState(this);
    }

    void LateUpdate()
    {
        //camera position changes with input of mouse
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        //change player rotation
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Physics.Raycast(muzzlepos.position,muzzlepos.forward,out RaycastHit hit,Mathf.Infinity);
        if (Physics.Raycast(muzzlepos.position, muzzlepos.forward, out RaycastHit hit, Mathf.Infinity))
        {
            Gizmos.DrawLine(muzzlepos.position, hit.point);
            //Gizmos.DrawWireSphere(hit.point, 0.1f);
        }
    }
    void ShoulderSwap()
    {
        if (Input.GetKeyDown(KeyCode.X))
            xCamPos = -xCamPos;
        if (movement.currentState == movement.crouchState)
            yCamPos = yCamHeight;//will change the height of camera if we are currently in crouch state
        else
            yCamPos = ogYCamPos; //return to original camera yFollow

        Vector3 newCamPos = new Vector3(xCamPos, yCamPos, camFollowPos.localPosition.z);

        camFollowPos.localPosition = Vector3.Lerp(camFollowPos.localPosition, newCamPos, returnCamSpeed * Time.deltaTime);
    }
}