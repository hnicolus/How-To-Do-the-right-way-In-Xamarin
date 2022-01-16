using Autofac;
using System.Linq;
using System.Reflection;
using ToDo.Repositories;
using ToDo.ViewModels;
using Xamarin.Forms;

namespace ToDo
{
    public abstract class Bootstrapper
    {
        protected ContainerBuilder ContainerBuilder { get; private set; }
        public Bootstrapper()
        {
            Initialize();
            FinishInitialization();
        }
        private void Initialize()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            ContainerBuilder = new ContainerBuilder();

            foreach (var type in currentAssembly.DefinedTypes.Where(e => e.IsSubclassOf(typeof(Page)) ||
            e.IsSubclassOf(typeof(BaseViewModel))))
            {
                ContainerBuilder.RegisterType(type.AsType());
            }

            ContainerBuilder.RegisterType<TodoItemRepository>().SingleInstance();
          
        }
        protected virtual void FinishInitialization()
        {
            var container = ContainerBuilder.Build();
            Resolver.Initialize(container);
        }
   
    }
}
