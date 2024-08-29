namespace AnimalAllies.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("Invalid.input", $"{label} is invalid");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $"for Id:{id}";
            return Error.NotFound("Record.not.found", $"record not found {forId}");
        }

        public static Error Null(string? name = null)
        {
            var label = name ?? "value";
            return Error.Null("Null.entity", $"{label} is null");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? "" : " " + name + " ";
            return Error.Validation("Invalid.length",$"invalid{label}length");
        }
    }

    public static class Volunteer
    {
        public static Error AlreadyExist()
        {
            return Error.Validation("Record.already.exist", $"Volunteer already exist");
        }
    }
}