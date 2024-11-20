using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkMover : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed = 5;

    public void Move(Vector3 inputVector, bool attackInProgress)
    {
        Vector3 movementVector = Vector3.zero;

        if (inputVector.sqrMagnitude > 0.1f)
        {
            movementVector = Camera.main.transform.TransformDirection(inputVector);
            movementVector.y = 0;
            movementVector.Normalize();

            if (attackInProgress == false)
                transform.forward = movementVector;
        }

        _controller.Move(_speed * movementVector * Time.deltaTime);
    }
}
