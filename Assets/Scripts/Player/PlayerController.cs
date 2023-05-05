using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float PlayerSpeed = 7;
    public float Gravity = 6;
    private CharacterController _characterController;
    private float _verticalSpeed = 6;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        DoMovement();
    }

    private void DoMovement()
    {
        var horizontalMovement = transform.right * Input.GetAxis("Horizontal");
        var verticalMovement = transform.forward * Input.GetAxis("Vertical");

        if (_characterController.isGrounded)
        {
            _verticalSpeed = 0;
        }
        else
        {
            _verticalSpeed -= Gravity * Time.deltaTime;
        }

        var playerMovement = verticalMovement + horizontalMovement;
        var movement = PlayerSpeed * Time.deltaTime * playerMovement + new Vector3(0, _verticalSpeed, 0) * Time.deltaTime;
        _characterController.Move(movement);
    }
}
