namespace Core.Entities;

public class FilterCondition
{
    public string Property { get; set; }

    public object Value { get; set; }

    public ComparisonCondition Condition { get; set; }

    public FilterCondition()
    {
        
    }

    public FilterCondition(string property, object value, ComparisonCondition condition)
    {
        Property = property;
        Value = value;
        Condition = condition;
    }
}
