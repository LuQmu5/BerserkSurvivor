using System;
using System.Collections;
using UnityEngine;

public class CharacterMover
{
    private CharacterController _controller;
    private Transform _transform;
    private ICharacterStats _stats;
    private bool _moveIsFreezed;
    private float _baseRotationSpeed = 10;

    public bool MoveIsFreezed => _moveIsFreezed;

    public CharacterMover(CharacterController controller, ICharacterStats stats)
    {
        _controller = controller;
        _transform = controller.transform;
        _stats = stats;
    }

    public bool TryMove(Vector3 inputVector, bool attackInProgress = false)
    {
        if (_moveIsFreezed)
            return false;

        float speedCoeff = attackInProgress ? 0.1f : 1; // #test_values
        float deadZone = 0.1f;

        if (inputVector.sqrMagnitude < deadZone)
            return false;

        _stats.TryGetCurrentValueOfStat(StatNames.MovementSpeed, out float movementSpeed);
        _controller.Move(new Vector3(inputVector.x, 0, inputVector.y) * movementSpeed * speedCoeff * Time.deltaTime);

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

    public void RotateToMouse(bool isInstantly = false)
    {
        Plane playerPlane = new Plane(Vector3.up, _transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(ray, out float hit))
        {
            Vector3 targetPoint = ray.GetPoint(hit);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - _transform.position);
            float multiplier = isInstantly ? 100 : 1;
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * _baseRotationSpeed * multiplier);
        }
    }

    public void Rotate(Vector3 inputVector, bool toMouse)
    {
        if (toMouse)
        {
            RotateToMouse();
        }
        else
        {
            RotateToPoint(inputVector);
        }
    }

    public void InstaRotateToMouse()
    {
        RotateToMouse(isInstantly: true);
    }
}
