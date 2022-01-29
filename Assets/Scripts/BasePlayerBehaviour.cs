using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BasePlayerBehaviour : MonoBehaviour
{

    [SerializeField] private CoinSide heads, tails;
    
    private CoinSide side;

    private Rigidbody playerRB;
    private Camera camera;
    private bool grounded = true;

    // Start is called before the first frame update
    void Start()
    {
        if (side == null) side = heads;
        playerRB = GetComponent<Rigidbody>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = camera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            //transform.LookAt(lookAt);
        
            if (Input.GetMouseButtonUp(0) && grounded)
            {
                Jump(hit.point);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            Flip();
        }
    }

    void Flip()
    {
        if (grounded)
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Jump(hit.point);
            }
        }
        
        side = side.Side == Side.HEAD ? tails : heads;
    }

    void Jump (Vector3 targetPoint)
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        playerRB.AddForce(new Vector3(direction.x * side.Velocity, side.JumpForce,
                direction.z * side.Velocity),
            ForceMode.Force);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        grounded = true;
    }

    private void OnCollisionStay(Collision other)
    {
        grounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        grounded = false;
    }
}