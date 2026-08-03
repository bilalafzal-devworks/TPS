using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{
    #region ActionStates
    [HideInInspector] public ActionBaseState currentState;
    [HideInInspector] public DefaultState defaultState = new DefaultState();

    [HideInInspector] public ReloadState reloadState = new ReloadState();
    #endregion

    [SerializeField] GameObject currentWeapon;
    [HideInInspector] public AmmoManager ammo;
    [HideInInspector] public Animator anim;

    [Header("Rigging")]
    public MultiAimConstraint rHandAim;
    public TwoBoneIKConstraint lHandIK;

    [Header("Audio-Source")]
    AudioSource audioSource;


    void Start()
    {
        ammo = currentWeapon.GetComponent<AmmoManager>();
        audioSource = currentWeapon.GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        SwitchState(defaultState);

    }
    void Update()
    {
        //check at every frame does we need to switch state state
        currentState.UpdateState(this);
    }

    public void SwitchState(ActionBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    //Animation Trigger Events
    public void Reload()
    {
        ammo.Reload();
        SwitchState(defaultState);
    }
    public void MagIn()
    {
        audioSource.PlayOneShot(currentWeapon.GetComponent<WeaponManager>().magIn);
    }

    public void MagOut()
    {
        audioSource.PlayOneShot(currentWeapon.GetComponent<WeaponManager>().magOut);

    }
    public void ReleaseSlide()
    {
        audioSource.PlayOneShot(currentWeapon.GetComponent<WeaponManager>().releaseSlide);
    }
}