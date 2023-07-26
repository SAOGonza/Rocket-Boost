using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandleDoor : MonoBehaviour
{
    // Variables
    [HideInInspector] public enum lightState { Open, Close };
    [HideInInspector] public lightState lsStatus = lightState.Close;

    // Position & Lerp variables
    [SerializeField] private Vector3 _goalPosition;
    [SerializeField] private float speed = 0.02f;
    private float _current, _target;

    // Class variables
    [SerializeField] private GameObject lightSource;
    private CollisionHandler collisionHandler;

    // Start is called before the first frame update
    void Start()
    {
        collisionHandler = GameObject.Find("Rocket").GetComponent<CollisionHandler>();
        _target = 1;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDoorStatus();
    }

    private void FixedUpdate()
    {
        if (lsStatus == lightState.Open)
        {
            lightSource.GetComponent<Light>().color = Color.green;
        }
        
        else if (lsStatus == lightState.Close)
        {
            lightSource.GetComponent <Light>().color = Color.red;
        }
    }

    void CheckDoorStatus()
    {
        if (collisionHandler.doorIsOpen)
        {
            lsStatus = lightState.Open;
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        _current = Mathf.MoveTowards(_current, _target, speed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, _goalPosition, _current);
    }
}
