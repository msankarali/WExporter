using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace Core.Interceptors
{
    public class InterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptorAttribute>(true).ToList();
            var methodAttributes = type.GetMethod(method.Name)!.GetCustomAttributes<MethodInterceptorAttribute>(true);
            classAttributes.AddRange(methodAttributes);

            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}