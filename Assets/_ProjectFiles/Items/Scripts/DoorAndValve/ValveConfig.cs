using UnityEngine;

[CreateAssetMenu(menuName = "Game Configs/Valve Config")]
public class ValveConfig : ScriptableObject
{
    [Header("Valve")]
    public float maxRotationAngle = 180f;
    public float rotateSpeed = 120f;
    public float returnSpeed = 90f;

    [Header("Door")]
    public float openHeight = 3f;
}
