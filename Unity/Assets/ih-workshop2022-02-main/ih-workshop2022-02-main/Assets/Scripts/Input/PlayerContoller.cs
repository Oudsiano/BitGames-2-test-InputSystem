using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private Rigidbody playerRigidbody;
    private Vector3 startPosition;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turboSpeed = 1200f;//значение турборежима
    [SerializeField] private float relaxSpeed = 600f;//значение релакс скорости



    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerRigidbody = GetComponent<Rigidbody>();
        startPosition = transform.position;

        playerInputActions.Kart.ResetPosition.performed += context => ResetPositionPlayer();
        
        playerInputActions.Kart.TurboMove.started += context => Turbo();//при нажатии(переключение),а при зажатии вклюючение
        
        playerInputActions.Kart.TurboMove.canceled += context => CanselTurbo();//при отпускании кнопки



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
    /// движение и поворот
    /// </summary>
    /// <param name="direction"></param>
    private void Move(Vector2 direction)
    {
        playerRigidbody.velocity = new Vector3(direction.x*moveSpeed*Time.fixedDeltaTime,0f,direction.y*moveSpeed*Time.fixedDeltaTime);
        Vector3 directionRotation = playerRigidbody.velocity;
        directionRotation.y = 0;
        if (playerInputActions.Kart.Move.ReadValue<Vector2>().magnitude > 0.1f)//если мы что-то нажимаем
        {
            this.playerRigidbody.rotation = Quaternion.LookRotation(directionRotation,Vector3.up);//смотреть в направление взгляда и вращаться вокруг вектора/вверх
        }
        else
        {
            playerRigidbody.angularVelocity = Vector3.zero;//выключить угловую скорость.
        }
        
    }
    /// <summary>
    /// Метод увеличения скорости до значения TurboSpeed которое задается в начале скрипта или в инспекторе
    /// </summary>
    private void Turbo()
    {
        moveSpeed = turboSpeed;
    }
    /// <summary>
    /// Метод уменьшения скорости до значения relaxSpeed которое задается в начале скрипта или в инспекторе
    /// </summary>
    private void CanselTurbo()
    {
        moveSpeed = relaxSpeed;
    }
    /// <summary>
    /// сброс позиции
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
