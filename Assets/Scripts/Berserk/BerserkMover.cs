using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BerserkMover : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed = 5;

    public void Move(Vector3 inputVector)
    {
        Vector3 moveDirection = Vector3.zero;

        if (inputVector.sqrMagnitude > 0.1f)
        {
            moveDirection = Camera.main.transform.TransformDirection(inputVector);
            RotateToPoint(moveDirection);
        }

        _controller.Move(_speed * moveDirection * Time.deltaTime);
    }

    public void RotateToPoint(Vector3 point)
    {
        point.y = 0;
        point.Normalize();
        transform.forward = point;
    }

    public void RotateToClick()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(ray, out float hit))
        {
            Vector3 targetPoint = ray.GetPoint(hit);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = targetRotation;
        }
    }
}
