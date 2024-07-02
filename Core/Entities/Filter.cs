namespace Core.Entities;

public class Filter
{
    public List<FilterCondition> Conditions { get; set; } = new List<FilterCondition>();

    public Filter Equals(string proertyName, object value)
    {
        Conditions.Add(new FilterCondition(proertyName, value, ComparisonCondition.Equals));
        return this;
    }

    public Filter NotEquals(string proertyName, object value)
    {
        Conditions.Add(new FilterCondition(proertyName, value, ComparisonCondition.NotEquals));
        return this;
    }

    public Filter Contains(string proertyName, object value)
    {
        Conditions.Add(new FilterCondition(proertyName, value, ComparisonCondition.Contains));
        return this;
    }

    public Filter GreaterThanOrEqual(string proertyName, object value)
    {
        Conditions.Add(new FilterCondition(proertyName, value, ComparisonCondition.GreaterThanOrEqual));
        return this;
    }

    public Filter GreaterThan(string proertyName, object value)
    {
        Conditions.Add(new FilterCondition(proertyName, value, ComparisonCondition.GreaterThan));
        return this;
    }

    public Filter LessThanOrEqual(string proertyName, object value)
    {
        Conditions.Add(new FilterCondition(proertyName, value, ComparisonCondition.LessThanOrEqual));
        return this;
    }

    public Filter LessThan(string proertyName, object value)
    {
        Conditions.Add(new FilterCondition(proertyName, value, ComparisonCondition.LessThan));
        return this;
    }
}

