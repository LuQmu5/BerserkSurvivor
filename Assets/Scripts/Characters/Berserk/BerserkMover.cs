using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BerserkMover : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _rotationSpeed = 10;

    public void Move(Vector3 inputVector, bool attackInProgress)
    {
        float speedCoeff = attackInProgress ? 0.4f : 1;
        float deadZone = 0.1f;

        if (inputVector.sqrMagnitude < deadZone)
            return;

        _controller.Move(new Vector3(inputVector.x, 0, inputVector.y) * _movementSpeed * speedCoeff * Time.deltaTime);
    }

    public void RotateToPoint(Vector3 point)
    {
        if (point == Vector3.zero)
            return;

        point.y = 0;
        point.Normalize();
        transform.forward = point;
    }

    public void RotateToMouse()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(ray, out float hit))
        {
            Vector3 targetPoint = ray.GetPoint(hit);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
    }
}
