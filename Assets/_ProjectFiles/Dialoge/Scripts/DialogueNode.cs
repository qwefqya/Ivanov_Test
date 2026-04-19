using System;
using System.Collections.Generic;

[Serializable]
public class DialogueNode
{
    public string speakerName;
    public string text;
    public List<DialogueChoice> choices = new List<DialogueChoice>();

    public bool isEnding = false;
    public bool givesQuest = false;
}
