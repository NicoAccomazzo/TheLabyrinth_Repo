using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotateSpeed = 150f;
    public float jumpVelocity = 3f;
    public float distanceToGround = 0.01f;
    public LayerMask groundLayer;
    public float miniMoveSpeed = 0.1f;
    public float miniJumpVelocity = 1f;


    public GameObject bullet;
    public float bulletSpeed = 100f;

    private bool doJump;
    private float vInput;
    private float hInput;
    private Rigidbody _rb;
    private CapsuleCollider _col;
    public Vector3 playerShrink = new Vector3(0.01f, 0.01f, 0.01f);

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.localScale == playerShrink)
        {
            moveSpeed = miniMoveSpeed;
            jumpVelocity = miniJumpVelocity;
        } else {
            moveSpeed = 3f;
            jumpVelocity = 3f;
        }

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            doJump = true;
        }


        vInput = Input.GetAxis("Vertical") * moveSpeed;
        hInput = Input.GetAxis("Horizontal") * rotateSpeed;
    }

    // 
    void FixedUpdate()
    {
        // Jumping when flag is triggered
        if (doJump)
        {
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            doJump = false;
        }
        
        // Bullet when left mouse is triggered
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate (bullet, this.transform.position + new Vector3(1, 0, 0), this.transform.rotation) as GameObject;
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.velocity = this.transform.forward * bulletSpeed;
        }

        Vector3 rotation = Vector3.up * hInput;
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);
        _rb.MovePosition(this.transform.position + this.transform.forward * vInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot);
    }

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3 (_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
        
        return grounded;
    }

    /*
        Speed Boost
    */
    private float speedMultiplier;

    public void BoostSpeed(float multiplier, float seconds)
    {
        speedMultiplier = multiplier;
        moveSpeed *= multiplier;
        Invoke("EndSpeedBoost", seconds);
    }

    private void EndSpeedBoost()
    {
        Debug.Log("Speed Boost Ended");
        moveSpeed /= speedMultiplier;
    }
}
