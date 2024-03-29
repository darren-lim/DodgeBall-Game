using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmMove : MonoBehaviour
{
    // https://www.youtube.com/watch?v=zYN1LTMdFYg

    [SerializeField]
    private GameObject arm;
    private Vector2 worldPosition;
    private Vector2 direction;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleArmRotation();
    }

    private void HandleArmRotation()
    {
        // rotate the arm towards the mouse
        worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosition - (Vector2)transform.position).normalized;

        // flip the arm when reaches 90 degrees
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector3 localScale = new Vector3(1f, 1f, 1f);
        if(angle > 90 || angle < -90)
        {
            localScale.y = -1f;
        } else
        {
            localScale.y = 1f;
        }
        transform.localScale = localScale;
        transform.right = direction;
    }
}
