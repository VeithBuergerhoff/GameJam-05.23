using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechbubbleController : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject followedGameObject;
    public Vector3 positionOffset = new(1, 2, 1);

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.SetPositionAndRotation(followedGameObject.transform.position + positionOffset, mainCamera.transform.rotation);
    }
}
