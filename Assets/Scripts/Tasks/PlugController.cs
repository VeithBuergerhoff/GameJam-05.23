using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _plug;

    [SerializeField]
    private AudioSource _gameMusic;

    public IEnumerator Pull()
    {
        _plug.isKinematic = false;
        _plug.useGravity = true;

        _plug.AddForce(Vector3.back);
        _gameMusic.enabled = false;
        yield return new WaitForSeconds(3);
        
        StartCoroutine(GameManager.Instance.WinGame());
    }

    void Update()
    {
        if (Input.GetKey("l"))
        {
            StartCoroutine(Pull());
        }
    }
}
