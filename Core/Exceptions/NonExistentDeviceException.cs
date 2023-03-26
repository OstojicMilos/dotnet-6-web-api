namespace CleanArchitecture.Core.Exceptions
{
    public sealed class NonExistentDeviceException : AppException
    {
        public NonExistentDeviceException() : base() { }

        public NonExistentDeviceException(string message) : base(message) { }

        public NonExistentDeviceException(string message, Exception inner) : base(message, inner) { }
    }
}
