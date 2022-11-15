using Fastenshtein;
using Phonix;

namespace ConsoleApp1;

public class Search<T>
{
    private readonly SearchOptions _options;
    private readonly List<IndexItem<T>> _index;

    public Search(SearchOptions options, IEnumerable<T> items, Func<T, string> accessor)
    {
        this._options = options;

        var metaphone = new DoubleMetaphone();
        _index = items.Select(x => new IndexItem<T>()
        {
            Item = x,
            MetaphoneValue = metaphone.BuildKey(accessor(x)),
            Value = accessor(x)
        }).ToList();
    }

    public List<T> GetMatches(string query)
    {
        var unsorted = _index.Select(x => new { Item = x, Score = WeightedSimilarity(x, query) });

        if (_options.UseMinimum)
            unsorted = unsorted.Where(x => x.Score >= _options.Minimum);

        var sorted = unsorted.OrderByDescending(x => x.Score).AsEnumerable();

        if (_options.LimitResults)
            sorted = sorted.Take(_options.ResultLimit).AsEnumerable();

        return sorted.Select(x => x.Item.Item).ToList();
    }

    private float WeightedSimilarity(IndexItem<T> item, string query)
    {
        var prefixSimilarity = GetPrefixSimilarity(item.Value, query);
        var metaphoneSimilarity = GetDoubleMetaphoneSimilarity(item.MetaphoneValue, query);
        var levenshteinSimilarity = GetSimilarity(item.Value, query);
        return prefixSimilarity + metaphoneSimilarity + levenshteinSimilarity;
    }

    private float GetPrefixSimilarity(string indexed, string query)
    {
        if (_options.UsePrefix)
            return indexed.StartsWith(query) ? 1 * _options.PrefixWeight : 0;
        else
            return 0;
    }

    private float GetDoubleMetaphoneSimilarity(string indexed, string query)
    {
        if (!_options.UseMetaphone)
            return 0;

        var metaphone = new DoubleMetaphone();
        var encodedVal = metaphone.BuildKey(query);
        var similarity = GetSimilarity(indexed, encodedVal, false);
        return similarity * _options.MetaphoneWeight * similarity;
    }

    private float GetSimilarity(string val1, string val2, bool weighted = true)
    {
        if (!_options.UseLevenshtein)
            return 0;

        var dist = Levenshtein.Distance(val1, val2);
        var similarity = ((float)val1.Length / (val1.Length + dist));

        if (weighted)
            return similarity * _options.LevenshteinWeight * similarity;
        else
            return similarity;
    }

    private class IndexItem<T>
    {
        public T Item { get; set; }

        public string Value { get; set; }

        public string MetaphoneValue { get; set; }
    }
}