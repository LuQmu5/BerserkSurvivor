using System;
using System.Collections;
using UnityEngine;

public class CharacterMover
{
    private CharacterController _controller;
    private float _movementSpeed = 5;
    private float _rotationSpeed = 10;
    private Transform _transform;
    private bool _moveIsFreezed;
    private ICoroutineRunner _coroutineRunner;

    public bool MoveIsFreezed => _moveIsFreezed;

    public CharacterMover(CharacterController controller, float movementSpeed, float rotationSpeed, ICoroutineRunner coroutineRunner)
    {
        _controller = controller;
        _movementSpeed = movementSpeed;
        _rotationSpeed = rotationSpeed;
        _transform = controller.transform;
        _coroutineRunner = coroutineRunner;
    }

    public bool TryMove(Vector3 inputVector, bool attackInProgress = false)
    {
        if (_moveIsFreezed)
            return false;

        float speedCoeff = attackInProgress ? 0.4f : 1; // #test_values
        float deadZone = 0.1f;

        if (inputVector.sqrMagnitude < deadZone)
            return false;

        _controller.Move(new Vector3(inputVector.x, 0, inputVector.y) * _movementSpeed * speedCoeff * Time.deltaTime);

        return true;
    }

    public void RotateToPoint(Vector3 point)
    {
        if (point == Vector3.zero)
            return;

        point.y = 0;
        point.Normalize();
        _transform.forward = point;
    }

    public void RotateToMouse()
    {
        Plane playerPlane = new Plane(Vector3.up, _transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(ray, out float hit))
        {
            Vector3 targetPoint = ray.GetPoint(hit);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - _transform.position);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
    }

    public void FreezeMovementFor(float seconds)
    {
        _moveIsFreezed = true;

        _coroutineRunner.StartCoroutine(FreezingMovement(seconds));
    }

    private IEnumerator FreezingMovement(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        _moveIsFreezed = false;
    }
}
