using System;

namespace Injector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectReferenceAttribute : Attribute
    {
        
    }
}