namespace Pitstop.WorkshopManagementAPI.Domain.ValueObjects;

public class WorkshopPlanningId : ValueObject
{
    private const string DATE_FORMAT = "yyyy-MM-dd";
    public string Value { get; private set; }

    public static WorkshopPlanningId Create(DateTime date)
    {
        return new WorkshopPlanningId { Value = date.ToString(DATE_FORMAT) };
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator string(WorkshopPlanningId id) => id.Value;
    public static implicit operator DateTime(WorkshopPlanningId id) =>
        DateTime.ParseExact(id.Value, DATE_FORMAT, CultureInfo.InvariantCulture);
}