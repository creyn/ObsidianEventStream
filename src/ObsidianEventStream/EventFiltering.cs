namespace ObsidianEventStream;

public static class EventFiltering
{
    public static string ExtractTextBetweenBrackets(string input, int occurence, char leftBracket = '[', char rightBracket = ']')
    {
        int startIndex = -1;
        int currentOccurence = 0;

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == leftBracket)
            {
                currentOccurence++;
                if (currentOccurence == occurence)
                {
                    startIndex = i;
                }
            }
            else if (input[i] == rightBracket && startIndex != -1)
            {
                int endIndex = i;
                int depth = 1;

                for (int j = startIndex + 1; j <= endIndex; j++)
                {
                    if (input[j] == leftBracket) depth++;
                    if (input[j] == rightBracket) depth--;

                    if (depth == 0)
                    {
                        return input.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }
                }

                startIndex = -1;
            }
        }

        return null;
    }
}
