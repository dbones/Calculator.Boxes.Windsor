using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspects
{
    using Castle.DynamicProxy;
    using Identifiers;

    public class LoggingInterceptor : IInterceptor, ITransientDependency
    {
        public void Intercept(IInvocation invocation)
        {
            //over simplified...
            try
            {
                invocation.Proceed();
            }
            catch (Exception)
            {
                Console.WriteLine("had issues running: {0}", invocation.TargetType.Name);
                invocation.ReturnValue = "";
            }
        }
    }
}
