using Epam.Library.BLL.Interfaces;
using Epam.Library.Dependencies;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.Library.ConsolePL
{
    class Program
    {
        static void Main(string[] args)
        {
            ILibraryLogic lb =  DependencyResolver.NinjectKernel.Get<ILibraryLogic>();
            lb.GetAll();
        }
    }
}
