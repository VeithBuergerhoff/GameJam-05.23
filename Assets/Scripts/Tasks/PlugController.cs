using System.Collections;
using UnityEngine;

public class PlugController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _plug;

    [SerializeField]
    private AudioSource _gameMusic;

    [SerializeField]
    private SensorController _playerSensor;

    [SerializeField]
    private LabelController _label;

    public IEnumerator Pull()
    {
        _plug.isKinematic = false;
        _plug.useGravity = true;

        _plug.AddForce(Vector3.back);
        _gameMusic.enabled = false;
        yield return new WaitForSeconds(3);
        
        StartCoroutine(GameManager.Instance.WinGame());
    }

    private void PlayerEnteredArea(object sender, Collider playerCollider)
    {
        ShowLabel(true);
    }

    private void PlayerExitedArea(object sender, Collider playerCollider)
    {
        ShowLabel(false);
    }

    private void ShowLabel(bool show)
    {
        _label.gameObject.SetActive(show);
    }

    void Awake()
    {
        _label.SetText("Stecker ziehen", 'l');
    }

    void Update()
    {
        if (Input.GetKey("l") && _playerSensor.isEntryInZone)
        {
            StartCoroutine(Pull());
        }
    }

    void OnEnable()
    {
        _playerSensor.TagEntered += PlayerEnteredArea;
        _playerSensor.TagExited += PlayerExitedArea;
    }

    void OnDisable()
    {
        _playerSensor.TagEntered -= PlayerEnteredArea;
        _playerSensor.TagExited -= PlayerExitedArea;
    }
}
