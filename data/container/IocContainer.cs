using System;
using System.Collections.Generic;

namespace ica.aps.container
{
	public class IocContainer
	{
		public delegate object Creator(IocContainer container, params object[] args = null);

		private readonly Dictionary<string, object> configuration = new Dictionary<string, object>();
		private readonly Dictionary<Type, Creator> typeToCreator = new Dictionary<Type, Creator>();

		public Dictionary<string, object> Configuration
		{
			get { return configuration; }
		}

		public void Register<T>(Creator creator)
		{
			typeToCreator.Add(typeof(T),creator);
		}

		public T Create<T>(params object[] args = null)
		{
			return (T) typeToCreator[typeof (T)](this, args);
		}

		public T GetConfiguration<T>(string name)
		{
			return (T) configuration[name];
		}
	}
}

