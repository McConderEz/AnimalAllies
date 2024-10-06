namespace AnimalAllies.SharedKernel.Shared;

public class Result
{
    public Result(bool isSuccess, IEnumerable<Error> errors)
    {
        if (isSuccess && errors.Any(x => x != Error.None))
            throw new InvalidOperationException();
        
        if (!isSuccess && errors.Any(x => x == Error.None))
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Errors = errors.ToList();
    }
    
    public ErrorList Errors { get; set; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, [Error.None]);
    public static Result Failure(Error error) => new(false, [error]);
    public static implicit operator Result(Error error) => new( false, [error]);
    public static implicit operator Result(ErrorList errors) => new( false, errors);
}

public class Result<TValue> : Result
{
    public Result(TValue value,bool isSuccess, IEnumerable<Error> errors) 
        : base(isSuccess, errors)
    {
        _value = value;
    }
    
    private readonly TValue _value;

    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("The value of a failure result cannot be accessed");

    public static Result<TValue> Success(TValue value) => new(value, true, [Error.None]);
    public new static Result<TValue> Failure(Error error) => new(default!, false, [error]);

    public static implicit operator Result<TValue>(TValue value) => new(value, true, [Error.None]);
    public static implicit operator Result<TValue>(Error error) => new(default!, false, [error]);
    public static implicit operator Result<TValue>(ErrorList errors) => new(default!, false, errors);
}