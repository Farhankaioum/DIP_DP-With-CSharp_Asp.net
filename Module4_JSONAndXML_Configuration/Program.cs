using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;

namespace Module4_JSONAndXML_Configuration
{
    interface IOperation
    {
        float Calculate(float a, float b);
    }

    public class Addition : IOperation
    {
        public float Calculate(float a, float b)
        {
            return a + b;
        }
    }

    public class Multiplication : IOperation
    {
        public float Calculate(float a, float b)
        {
            return a * b;
        }
    }

    public class CalculationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Addition>().As<IOperation>();
            builder.RegisterType<Multiplication>().As<IOperation>();

            base.Load(builder);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(@"H:\M\DP_Advanced\di-ioc-dotnet-practice\DIP_DP\Module4_JSONAndXML_Configuration\config.json");
            var configuration = configBuilder.Build();

            var containerBuilder = new ContainerBuilder();
            var configModule = new ConfigurationModule(configuration);
            containerBuilder.RegisterModule(configModule);

            using (var container = containerBuilder.Build())
            {
                float a = 3, b = 4;

                foreach (IOperation op in container.Resolve<IList<IOperation>>())
                {
                    Console.WriteLine($"{op.GetType().Name} of {a} and {b} = {op.Calculate(a, b)}");
                }
            }
        }
    }
}
