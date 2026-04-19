using System.Collections.Generic;
using System.Text;

public static class HackCodeUtility
{
    public static List<HackSignal> ParseSequence(string encoded)
    {
        List<HackSignal> result = new List<HackSignal>();

        if (string.IsNullOrEmpty(encoded))
            return result;

        foreach (char c in encoded)
        {
            if (c == '0')
                result.Add(HackSignal.Short);
            else if (c == '1')
                result.Add(HackSignal.Long);
        }

        return result;
    }

    public static string ToSymbolString(string encoded, bool spaced = true)
    {
        if (string.IsNullOrEmpty(encoded))
            return "";

        StringBuilder sb = new StringBuilder();

        foreach (char c in encoded)
        {
            if (c == '0')
                sb.Append("•");
            else if (c == '1')
                sb.Append("—");
            else
                continue;

            if (spaced)
                sb.Append(" ");
        }

        return sb.ToString().TrimEnd();
    }

    public static string ProgressString(IReadOnlyList<HackSignal> sequence, int completedSteps)
    {
        if (sequence == null || sequence.Count == 0)
            return "";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < sequence.Count; i++)
        {
            if (i < completedSteps)
                sb.Append(sequence[i] == HackSignal.Long ? "— " : "• ");
            else
                sb.Append("_ ");
        }

        return sb.ToString().TrimEnd();
    }
}
