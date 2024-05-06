using System;

public static class StringCorrupter {
    public static string CorruptString(string sentence, double corruptionPercentage, string colorTag = "<color=#FF0000>")
    {
        // Ensure the corruption percentage is within [0, 1] range
        corruptionPercentage = Math.Max(0, Math.Min(1, corruptionPercentage));

        // Calculate number of characters to corrupt
        int numCharactersToCorrupt = (int)(sentence.Replace(" ", "").Length * corruptionPercentage);

        // Create a char array from the sentence
        char[] chars = sentence.ToCharArray();

        // Corrupt the specified number of characters
        for (int i = 0; i < numCharactersToCorrupt; i++)
        {
            // Generate a random index
            int index = UnityEngine.Random.Range(0, chars.Length);

            // Ignore spaces
            while (chars[index] == ' ')
            {
                index = UnityEngine.Random.Range(0, chars.Length);
            }

            // Generate a random character to replace the original one
            char randomChar = (char)UnityEngine.Random.Range(33, 127); // Excluding space, using ASCII printable characters range (33-126)

            // Replace the character
            chars[index] = randomChar;
        }

        // Convert char array back to string
        string corruptedString = new string(chars);

        // Apply color formatting to the corrupted characters
        string coloredCorruptedString = "";
        for (int i = 0; i < sentence.Length; i++)
        {
            if (corruptedString[i] != sentence[i])
            {
                coloredCorruptedString += colorTag + corruptedString[i] + "</color>";
            }
            else
            {
                coloredCorruptedString += sentence[i];
            }
        }

        // Return the colored corrupted string
        return coloredCorruptedString;
    }
}