using UnityEngine;

public enum ItemKind
{
    Generic,
    Key,
    Note
}

public enum ItemState
{
    InSocket,
    InWorld,
    Inspecting,
    Held,
    Consumed

}

[CreateAssetMenu(menuName = "Game/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    public string itemId;
    public string displayName;

    [TextArea(3, 8)]
    public string description;

    public ItemKind itemKind;
}