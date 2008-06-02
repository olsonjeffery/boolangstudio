using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using System.IO;
using Xunit;
using Spec = Xunit.FactAttribute;
using Rhino.Mocks;
using Rhino.Testing.AutoMocking;

namespace Boo.BooLangStudioSpecs
{
    public abstract class AutoMockingTestFixture
    {
        private MockRepository _mocks;
        private AutoMockingContainer _container;

        protected MockRepository Mocks
        {
            get { return _mocks; }
        }

        protected AutoMockingContainer Container
        {
            get { return _container; }
        }

        public AutoMockingTestFixture()
        {
            _mocks = new MockRepository();
            _container = new AutoMockingContainer(_mocks);
            _container.Initialize();
        }

        public T Create<T>()
        {
            return _container.Create<T>();
        }

        public T Mock<T>() where T : class
        {
            return _container.Get<T>();
        }

        public void Provide<TService, TImplementation>()
        {
            _container.AddComponent(typeof(TImplementation).FullName, typeof(TService), typeof(TImplementation));
        }

        public void Provide<TService>(object instance)
        {
            _container.Kernel.AddComponentInstance(instance.GetType().FullName, typeof(TService), instance);
        }
    }

}
