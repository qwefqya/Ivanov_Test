using UnityEngine;

[CreateAssetMenu(menuName = "Game Configs/Player Config")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Look")]
    public float sensitivity = 0.1f;
    public float maxAngle = 75f;

    [Header("Interaction")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
}