namespace Observability.Shared;

public sealed record ValidationError : Error
{
    public ValidationError(Error[] errors) : base("Validation.General",
        "One or more validation errors occured.",
        ErrorType.Validation)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }
}