using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvementTest : MonoBehaviour
{
    public PlayerControllerAttack PlayerControllerAttack;
    public CharacterController controller;
    public Animator animator;
    public PhotonView m_MyphotonView;

    public float speed;
    public float turnSmoothTime = 0.1f;

    public UltimateJoystick joystick;


    float horizontaleMove = 0f;
    float VerticakMove = 0f;
    float turnSmoothVelocity;
    Vector3 direction;
    float targetAngle;
    float angle;
    public bool m_isDead=false;
    private void Start()
    {
    }
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (joystick == null) return;

        horizontaleMove = joystick.GetHorizontalAxis() * speed;
        VerticakMove = joystick.GetVerticalAxis() * speed;

       direction = new Vector3(horizontaleMove, 0, VerticakMove).normalized;

        if (direction.magnitude >= 0.5f)
        {
            PlayerControllerAttack.attackInputs.Clear();
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(direction * speed * Time.deltaTime);

        }

        animator.SetFloat("Move", Mathf.Abs(horizontaleMove + VerticakMove));
    }
    
}
