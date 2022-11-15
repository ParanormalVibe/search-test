namespace ConsoleApp1;

public class SearchOptions
{
    public bool UseLevenshtein { get; set; }

    public float LevenshteinWeight { get; set; }

    public bool UseMetaphone { get; set; }

    public float MetaphoneWeight { get; set; }

    public bool UsePrefix { get; set; }

    public float PrefixWeight { get; set; }

    public bool LimitResults { get; set; }

    public int ResultLimit { get; set; }

    public bool UseMinimum { get; set; }

    public float Minimum { get; set; }
}
