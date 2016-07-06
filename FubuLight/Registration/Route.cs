using System;
using System.Collections.Generic;
using System.Reflection;

namespace FubuLight.Registration
{
    using ConditionFunc = Func<IDictionary<string, object>, bool>;
    public class Route
    {
        private readonly Type _handlerType;
        private readonly MethodInfo _method;
        private readonly ConditionFunc _condition;

        public Route(Type handlerType, MethodInfo method, ConditionFunc condition)
        {
            _handlerType = handlerType;
            _method = method;
            _condition = condition;
        }

        public ConditionFunc Condition => _condition; 

        public Type HandlerType => _handlerType;

        public MethodInfo Method => _method;
    }
}
