using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    [SerializeField]
    public GameObject mainCamera;

    [SerializeField]
    private Vector3 _offset = new(0, 10, 10);

    [SerializeField]
    private bool _useDynamicLerping = true;
    
    [SerializeField]
    [Range(min: 0, 1)]
    private float _freeWindow = 0.3f;

    [SerializeField]
    [Range(0, 1)]
    private float _lerpValue = 0.5f;

    [SerializeField]
    [Range(0, 10)]
    private float _maxDistance = 3;

    [SerializeField]
    [Range(0, 10)]
    private float _lerpBoost = 10;

    void Update()
    {
        if (_player is null)
        {
            return;
        }

        var desiredPosition = _player.position + _offset;
        if (!_useDynamicLerping)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 1);
        }

        var distance = Vector3.Distance(transform.position, desiredPosition);
        if (distance > _freeWindow)
        {
            var delta = distance * _lerpValue;
            if (distance > _maxDistance)
            {
                delta *= _lerpBoost;
            }

            transform.position = Vector3.Lerp(transform.position, desiredPosition, delta);
        }
    }
}
