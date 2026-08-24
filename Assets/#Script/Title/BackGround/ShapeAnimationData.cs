using UnityEngine;
using UnityEngine.UI;

public readonly struct ShapeAnimationData
{
    public readonly Image Shape;
    public readonly float RotationSpeed;
    public readonly float MoveSpeed;

    public ShapeAnimationData(Image shape, float rotationSpeed,float moveSpeed)
    {
        Shape = shape;
        RotationSpeed = rotationSpeed;
        MoveSpeed = moveSpeed;
    }
}
