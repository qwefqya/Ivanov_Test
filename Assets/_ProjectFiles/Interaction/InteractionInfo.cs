public struct InteractionInfo
{
    public bool IsAvailable;
    public string PromptText;
    public InteractionType InteractionType;

    public InteractionInfo(bool isAvailable, string promptText, InteractionType interactionType)
    {
        IsAvailable = isAvailable;
        PromptText = promptText;
        InteractionType = interactionType;
    }
}