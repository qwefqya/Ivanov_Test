using UnityEngine;

[CreateAssetMenu(menuName = "Game Configs/Hack Config")]
public class HackConfig : ScriptableObject
{
    [Header("Code")]
    public float longPressThreshold = 0.5f;
    public int maxMistakes = 3;

    [Header("Door")]
    public float openHeight = 3f;
    public float moveSpeed = 2f;
    public string encodedSequence = "1001";
}
