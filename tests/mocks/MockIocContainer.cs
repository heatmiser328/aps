using System;
using System.Collections.Generic;
using System.Text;

using ica.aps.container;
using ica.aps.data.interfaces;

namespace ica.aps.tests.mocks
{
    internal static class MockIocContainer
    {
		internal static IIocContainer Create()
		{
			IIocContainer container = new IocContainer();
			container.Register<IDBFactory>(delegate {
				return TestHelpers.TestData.Database;
			});
			
			return container;
		}
	}
}
