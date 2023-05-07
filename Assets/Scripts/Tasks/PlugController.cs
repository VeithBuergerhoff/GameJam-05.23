using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _plug;

    public void Pull()
    {
        _plug.isKinematic = false;
        _plug.useGravity = true;

        _plug.AddForce(Vector3.back);

        StartCoroutine(GameManager.Instance.WinGame());
    }

    void Update()
    {
        if (Input.GetKey("l"))
        {
            Pull();
        }
    }
}
