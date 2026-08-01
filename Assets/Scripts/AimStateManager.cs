using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SocialPlatforms;
using TreeEditor;
public class AimStateManager : MonoBehaviour
{
    [SerializeField] Transform camFollowPos;
    [SerializeField] float xAxis, yAxis; //cinemachine x and y-axis inputs
    [SerializeField] float mouseSensitivity = 1f;

    #region AimStates
    AimBaseState currentState;
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

    #endregion
    void Start()
    {
        //Debug.Log("adsFov = " + adsFov);

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
        currentState.UpdateState(this);

        //Screen centre Calculating
        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);
        //raycast
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
        {
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothPos * Time.deltaTime);
        }

        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);
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
}