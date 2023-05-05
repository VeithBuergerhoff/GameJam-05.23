using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;
    public float Speed = 6;

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
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        var movement = transform.forward * verticalMove + transform.right * horizontalMove;
        _characterController.Move(movement);
    }
}
