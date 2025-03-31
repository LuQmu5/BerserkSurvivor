using UnityEngine;

public class CharacterMover
{
    private CharacterController _controller;
    private float _movementSpeed = 5;
    private float _rotationSpeed = 10;
    private Transform _transform;

    public CharacterMover(CharacterController controller, float movementSpeed, float rotationSpeed)
    {
        _controller = controller;
        _movementSpeed = movementSpeed;
        _rotationSpeed = rotationSpeed;
        _transform = controller.transform;
    }

    public void Move(Vector3 inputVector, bool attackInProgress = false)
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
}
