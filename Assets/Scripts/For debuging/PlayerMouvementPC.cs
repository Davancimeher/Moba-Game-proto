using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvementPC : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;

    public float speed;
    public float turnSmoothTime = 0.1f;
    private float horizontaleMove = 0f;
    private float VerticakMove = 0f;
    private float turnSmoothVelocity;
    private Vector3 direction;
    private float targetAngle;
    private float angle;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        horizontaleMove = Input.GetAxis("Horizontal") * speed;
        VerticakMove = Input.GetAxis("Vertical") * speed;
        direction = new Vector3(horizontaleMove, 0, VerticakMove).normalized;

        if (direction.magnitude >= 0.5f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(direction * speed * Time.deltaTime);
        }
            
            animator.SetFloat("Move", Mathf.Abs(horizontaleMove) + Mathf.Abs(VerticakMove));
    }
}
