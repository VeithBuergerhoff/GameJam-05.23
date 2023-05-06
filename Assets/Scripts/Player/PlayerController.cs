using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float PlayerSpeed = 7;
    public float Gravity = 6;
    [Range(0, 1)]
    public float RotationSpeed = 0.15f;
    public Transform RotatingTransform;

    private CharacterController _characterController;
    private float _verticalSpeed;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();

        if (RotatingTransform is null)
        {
            Debug.LogError($"{nameof(RotatingTransform)} must be set");
        }
    }

    void Update()
    {
        DoMovement();
    }

    private void DoMovement()
    {
        var horizontalMovement = transform.right * Input.GetAxis("Horizontal");
        var verticalMovement = transform.forward * Input.GetAxis("Vertical");
        var playerMovement = verticalMovement + horizontalMovement;

        if (playerMovement != Vector3.zero)
        {
            RotatingTransform.rotation = Quaternion.Slerp(RotatingTransform.rotation, Quaternion.LookRotation(playerMovement), RotationSpeed);
        }

        if (_characterController.isGrounded)
        {
            _verticalSpeed = 0;
        }
        else
        {
            _verticalSpeed -= Gravity * Time.deltaTime;
        }


        var finalMovement = PlayerSpeed * Time.deltaTime * playerMovement
            + new Vector3(0, _verticalSpeed, 0) * Time.deltaTime;
        _characterController.Move(finalMovement);
    }
}
