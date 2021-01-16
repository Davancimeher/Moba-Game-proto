using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public PhotonView m_MyphotonView;

    public float speed;
    public float turnSmoothTime = 0.1f;

    private UltimateJoystick joystick;

    float horizontaleMove = 0f;
    float VerticakMove = 0f;
    float turnSmoothVelocity;
    Vector3 direction;
    float targetAngle;
    float angle;
    public bool m_isDead=false;

    // Update is called once per frame

    private void Start()
    {
        m_MyphotonView = GetComponent<PhotonView>();

        if (m_MyphotonView.IsMine)
        {
            joystick = InGameManager.IGM.m_joystick;
        }

       // Application.targetFrameRate = 120;
    }
    void Update()
    {
        if (m_MyphotonView.IsMine && !m_isDead)
        {
            Move();
        }
    }

    private void Move()
    {
        if (joystick == null) return;

        horizontaleMove = joystick.GetHorizontalAxis() * speed;
        VerticakMove = joystick.GetVerticalAxis() * speed;

       direction = new Vector3(horizontaleMove, 0, VerticakMove).normalized;

        if (direction.magnitude >= 0.5f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(direction * speed * Time.deltaTime);

        }

        animator.SetFloat("Move", Mathf.Abs(horizontaleMove + VerticakMove));
    }
}
