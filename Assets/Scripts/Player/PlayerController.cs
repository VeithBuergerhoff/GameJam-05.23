using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed = 7;

    [SerializeField]
    private float _gravity = 6;

    [Range(0, 1)]
    [SerializeField]
    private float _rotationSpeed = 0.15f;

    [SerializeField]
    private Transform _rotatingTransform;

    private CharacterController _characterController;
    private float _verticalSpeed;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        if (_rotatingTransform is null)
        {
            Debug.LogError($"{nameof(_rotatingTransform)} must be set");
        }
    }

    void Update()
    {
        DoMovement();
    }

    public void DoMovement()
    {
        var horizontalMovement = transform.right * Input.GetAxis("Horizontal");
        var verticalMovement = transform.forward * Input.GetAxis("Vertical");
        var playerMovement = verticalMovement + horizontalMovement;

        if (playerMovement != Vector3.zero)
        {
            _rotatingTransform.rotation = Quaternion.Slerp(_rotatingTransform.rotation, Quaternion.LookRotation(playerMovement), _rotationSpeed);
        }

        if (_characterController.isGrounded)
        {
            _verticalSpeed = 0;
        }
        else
        {
            _verticalSpeed -= _gravity * Time.deltaTime;
        }


        var finalMovement = _playerSpeed * Time.deltaTime * playerMovement
            + new Vector3(0, _verticalSpeed, 0) * Time.deltaTime;
        _characterController.Move(finalMovement);
    }
}
