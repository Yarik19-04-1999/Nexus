namespace Nexus.Core.Tests.Constants;

public static class TestData
{
    public static readonly char[] Alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

    public const int IntValue = 19;
    public const int NonExistentIntValue = int.MinValue;

    public const string StringValue = "test string data";
    public const string ChangedStringValue = "changed string data";

    
    public static string RandomString(int length)
        => new string(Random.Shared.GetItems(Alphabet, length));
}
