using Autofac;
using Instructor.Core;
using Instructor.Core.Common.Seeds;

namespace ExceptionFailures.Application.Configuration;

public class ApplicationAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
               .AsClosedTypesOf(typeof(IInstructionHandler<,>));

        builder.Register(c =>
        {
            var context = c.Resolve<IComponentContext>();
            return new InstructionDispatcher(type => context.Resolve(type));

        }).As<IInstructionDispatcher>().InstancePerLifetimeScope();

        base.Load(builder);
    }
}
