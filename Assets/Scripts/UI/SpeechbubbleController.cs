using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechbubbleController : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject followedGameObject;
    public Vector3 positionOffset = new Vector3(1, 2, 1);

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.SetPositionAndRotation(followedGameObject.transform.position + positionOffset, mainCamera.transform.rotation);
        
    }
}
