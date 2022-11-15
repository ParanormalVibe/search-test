namespace ConsoleApp1;

public class SearchBuilder<T>
{
    private readonly SearchOptions _options;
    private readonly IEnumerable<T> _items;
    private readonly Func<T, string> _accessor;

    public SearchBuilder(IEnumerable<T> items, Func<T, string> accessor)
    {
        _options = new SearchOptions();
        _items = items;
        _accessor = accessor;
    }

    public SearchBuilder<T> WithLevenshtein(float weight = 0.45f)
    {
        _options.UseLevenshtein = true;
        _options.LevenshteinWeight = weight;
        return this;
    }

    public SearchBuilder<T> WithMetaphone(float weight = 0.05f)
    {
        _options.UseMetaphone = true;
        _options.MetaphoneWeight = weight;
        return this;
    }

    public SearchBuilder<T> WithPrefix(float weight = 0.5f)
    {
        _options.UsePrefix = true;
        _options.PrefixWeight = weight;
        return this;
    }

    public SearchBuilder<T> WithMinimum(float minimum = 0.25f)
    {
        _options.UseMinimum = true;
        _options.Minimum = minimum;
        return this;
    }

    public SearchBuilder<T> LimitResults(int limit = 10)
    {
        _options.LimitResults = true;
        _options.ResultLimit = limit;
        return this;
    }

    public Search<T> Build()
    {
        return new Search<T>(_options, _items, _accessor);
    }
}
