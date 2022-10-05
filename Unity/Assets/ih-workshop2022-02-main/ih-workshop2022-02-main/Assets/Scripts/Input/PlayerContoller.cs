using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private Rigidbody playerRigidbody;
    private Vector3 startPosition;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turboSpeed = 1200f;//�������� �����������
    [SerializeField] private float relaxSpeed = 600f;//�������� ������ ��������



    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerRigidbody = GetComponent<Rigidbody>();
        startPosition = transform.position;

        playerInputActions.Kart.ResetPosition.performed += context => ResetPositionPlayer();
        
        playerInputActions.Kart.TurboMove.started += context => Turbo();//��� �������(������������),� ��� ������� ����������
        
        playerInputActions.Kart.TurboMove.canceled += context => CanselTurbo();//��� ���������� ������



    }
    private void OnEnable()
    {
        playerInputActions.Enable();
    }
    private void OnDisable()
    {
        playerInputActions.Disable();
    }
    
    /// <summary>
    /// �������� � �������
    /// </summary>
    /// <param name="direction"></param>
    private void Move(Vector2 direction)
    {
        playerRigidbody.velocity = new Vector3(direction.x*moveSpeed*Time.fixedDeltaTime,0f,direction.y*moveSpeed*Time.fixedDeltaTime);
        Vector3 directionRotation = playerRigidbody.velocity;
        directionRotation.y = 0;
        if (playerInputActions.Kart.Move.ReadValue<Vector2>().magnitude > 0.1f)//���� �� ���-�� ��������
        {
            this.playerRigidbody.rotation = Quaternion.LookRotation(directionRotation,Vector3.up);//�������� � ����������� ������� � ��������� ������ �������/�����
        }
        else
        {
            playerRigidbody.angularVelocity = Vector3.zero;//��������� ������� ��������.
        }
        
    }
    /// <summary>
    /// ����� ���������� �������� �� �������� TurboSpeed ������� �������� � ������ ������� ��� � ����������
    /// </summary>
    private void Turbo()
    {
        moveSpeed = turboSpeed;
    }
    /// <summary>
    /// ����� ���������� �������� �� �������� relaxSpeed ������� �������� � ������ ������� ��� � ����������
    /// </summary>
    private void CanselTurbo()
    {
        moveSpeed = relaxSpeed;
    }
    /// <summary>
    /// ����� �������
    /// </summary>
    private void ResetPositionPlayer()
    {
        playerRigidbody.MovePosition(startPosition);
        playerRigidbody.MoveRotation(Quaternion.identity);
    }
    private void FixedUpdate()
    {
        Vector2 moveDirection = playerInputActions.Kart.Move.ReadValue<Vector2>();
        Move(moveDirection);
    }
}
