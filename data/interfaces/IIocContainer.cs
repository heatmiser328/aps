// this one: http://ayende.com/blog/2886/building-an-ioc-container-in-15-lines-of-code
// another: http://timross.wordpress.com/2010/01/21/creating-a-simple-ioc-container/
// and another: http://ruijarimba.wordpress.com/2013/10/28/implementing-a-basic-ioc-container-using-csharp/
// http://www.remondo.net/building-simple-ioc-container-csharp/
using System;
using System.Collections.Generic;

namespace ica.aps.container
{
	public delegate object Creator(IIocContainer container, params object[] args);

	public interface IIocContainer
	{
		IDictionary<string, object> Configuration {get;}

		void Register<T>(Creator creator);
        void Register<T>(Type creator);
		T Create<T>(params object[] args);
		T GetConfiguration<T>(string name);
	}
}

