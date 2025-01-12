using Microsoft.Xrm.Sdk;

namespace devusD365
{
    public interface IExecutionContextProvider
    {
        IExecutionContext ExecutionContext { get; }
    }
}