// this one: http://ayende.com/blog/2886/building-an-ioc-container-in-15-lines-of-code
// another: http://timross.wordpress.com/2010/01/21/creating-a-simple-ioc-container/
// and another: http://ruijarimba.wordpress.com/2013/10/28/implementing-a-basic-ioc-container-using-csharp/
// http://www.remondo.net/building-simple-ioc-container-csharp/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ica.aps.container
{
	public class IocContainer : IIocContainer
	{
		private readonly IDictionary<string, object> configuration = new Dictionary<string, object>();
		private readonly IDictionary<Type, Creator> typeToCreator = new Dictionary<Type, Creator>();

		public IDictionary<string, object> Configuration
		{
			get { return configuration; }
		}

        public void Register<T>(Creator creator)
        {
            typeToCreator.Add(typeof(T), creator);
        }

		public void Register<T>(Type creator)
		{
			typeToCreator.Add(typeof(T),delegate(IIocContainer container, object[] args) {
                return createByType<T>(creator, args);
            });
		}
		
		public T Create<T>(params object[] args)
		{
			return (T) typeToCreator[typeof (T)](this, args);
		}

		public T GetConfiguration<T>(string name)
		{
			return (T) configuration[name];
		}
		
		private T createByType<T>(Type type, params object[] args)
		{						
            // Try to construct the object
            // Step-1: find the constructor (ideally first constructor if multiple constructos present for the type)
            ConstructorInfo ctorInfo = type.GetConstructors().First();

            // Step-2: find the parameters for the constructor and try to resolve those
            List<ParameterInfo> paramsInfo = ctorInfo.GetParameters().ToList();
            List<object> resolvedParams = new List<object>();
            resolvedParams.Add(this);
            foreach (ParameterInfo param in paramsInfo)
            {
                Type t = param.ParameterType;
				if (args != null && args.Length > 0)
				{
	                object arg = findByType(args, t);
					if (arg != null)
						resolvedParams.Add(arg);
				}
            }

            // Step-3: using reflection invoke constructor to create the object
            object retObject = ctorInfo.Invoke(resolvedParams.ToArray());
            return (T) retObject;
		}
		
		private object findByType(object[] args, Type t)
		{
			foreach (object arg in args)
			{
				if (arg.GetType() == t)
					return arg;
			}
			return null;
		}
		
	}
}

