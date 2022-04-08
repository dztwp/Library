using Epam.Library.NinjectBinds;
using Ninject;
using Ninject.Modules;

namespace Epam.Library.Dependencies
{
    public static class DependencyResolver
    {
        static DependencyResolver()
        {
            NinjectKernel = new StandardKernel(_ninjectRegistrations);
        }
        public static IKernel NinjectKernel { get; private set; }
        private static NinjectModule _ninjectRegistrations
        {
            get
            {
                return new NinjectRegistrations();
            }
        }
    }
}
