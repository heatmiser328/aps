using System;
using NUnit.Framework;

using ica.aps.container;

namespace IOC
{
	[TestFixture]
	public class IocContainerTest
	{
        [Test]
        public void create()
        {
			IIocContainer container = new IocContainer();
			container.Register<ObjectWithoutContructorParams>(ObjectWithoutContructorParams.Create);
            container.Register<ObjectWithContructorParams>(ObjectWithContructorParams.Create);
			
			ObjectWithoutContructorParams owo = container.Create<ObjectWithoutContructorParams>();
			Assert.IsNotNull(owo);
			Assert.AreEqual(owo.I, 1);
			Assert.AreEqual(owo.S, "one");
			
			ObjectWithContructorParams ow = container.Create<ObjectWithContructorParams>(88, "eighty-eight");
			Assert.IsNotNull(ow);
			Assert.AreEqual(ow.I, 88);
			Assert.AreEqual(ow.S, "eighty-eight");
        }
		
        [Test]
        public void create_constructor()
        {
			IIocContainer container = new IocContainer();
			container.Register<ObjectWithoutContructorParams>(typeof(ObjectWithoutContructorParams));
            container.Register<ObjectWithContructorParams>(typeof(ObjectWithContructorParams));
			
			ObjectWithoutContructorParams owo = container.Create<ObjectWithoutContructorParams>();
			Assert.IsNotNull(owo);
			Assert.AreEqual(owo.I, 1);
			Assert.AreEqual(owo.S, "one");
			
			ObjectWithContructorParams ow = container.Create<ObjectWithContructorParams>(88, "eighty-eight");
			Assert.IsNotNull(ow);
			Assert.AreEqual(ow.I, 88);
			Assert.AreEqual(ow.S, "eighty-eight");
        }
		
	}
	
	public class ObjectWithoutContructorParams
	{
        public ObjectWithoutContructorParams(IIocContainer container)
		{
			this.I = 1;
			this.S = "one";
		}
		
		public int I {get;set;}
		public string S {get;set;}

        internal static object Create(IIocContainer container, params object[] args)
        {
            return new ObjectWithoutContructorParams(container);
        }
	}
    public class ObjectWithContructorParams
	{
        public ObjectWithContructorParams(IIocContainer container, int i, string s)
		{
			this.I = i;
			this.S = s;
		}
		
		public int I {get;set;}
		public string S {get;set;}

        internal static object Create(IIocContainer container, params object[] args)
        {
            return new ObjectWithContructorParams(container, Convert.ToInt32(args[0]), Convert.ToString(args[1]));
        }
	}
	
}
