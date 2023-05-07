using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private GameObject _mainCamera;

    [SerializeField]
    private Vector3 _offset = new(0, 10, 10);

    [SerializeField]
    private bool _useLerping = true;
    
    [SerializeField]
    [Range(min: 0, 1)]
    private float _freeWindow = 0.3f;

    [SerializeField]
    [Range(0, 10)]
    private float _lerpValue = 1f;

    [SerializeField]
    [Range(0, 10)]
    private float _maxDistance = 3;

    [SerializeField]
    [Range(0, 100)]
    private float _lerpBoost = 10;

    public static CameraController Instance { get; private set; }

    void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            throw new Exception($"{nameof(CameraController)} is a singleton! There can only ever be one instance");
        }

        Instance = this;
    }

    public GameObject getMainCamera()
    {
        return _mainCamera;
    }

    void Update()
    {
        if (_player is null)
        {
            return;
        }

        var desiredPosition = _player.position + _offset;
        if (!_useLerping)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 1);
        }

        var distance = Vector3.Distance(transform.position, desiredPosition);
        if (distance > _freeWindow)
        {
            var delta = distance * _lerpValue * Time.deltaTime;
            if (distance > _maxDistance)
            {
                delta *= _lerpBoost;
            }

            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, delta);
        }
    }
}
