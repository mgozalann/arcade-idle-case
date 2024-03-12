using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private int _rotationSpeed;
    [SerializeField] private int _movementSpeed;

    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private CharacterController _controller;

    [SerializeField] private Animator _animator;
    private static readonly int Velocity = Animator.StringToHash("Velocity");

    private void FixedUpdate()
    {
        if (_joystick.Direction.magnitude == 0)
        {
            _animator.SetFloat(Velocity,0,.1f,Time.deltaTime);
            return;
        }
        
        Move();
        Turn();
    }


    private void Move()
    {
        var direction = new Vector3(_joystick.Horizontal * _movementSpeed, 0, _joystick.Vertical * _movementSpeed);

        _controller.Move(direction * Time.deltaTime);
        
        _animator.SetFloat(Velocity,1,0.1f,Time.deltaTime);
    }


    private void Turn()
    {
        Vector3 direction = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical);
        
       if(direction.magnitude <= 0.01f ) return;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * _rotationSpeed);
    }
}