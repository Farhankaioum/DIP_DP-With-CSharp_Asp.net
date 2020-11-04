using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;

namespace AutofacSamples
{
    public interface ILog
    {
        void Write(string message);
    }

    public interface IConsole
    {

    }

    public class ConsoleLog : ILog, IConsole
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class EmailLog : ILog
    {
        private const string adminEmail = "admin@foo.com";

        public void Write(string message)
        {
            Console.WriteLine($"Email sent to {adminEmail} : {message}");
        }
    }

    public class Engine
    {
        private ILog log;
        private int id;

        public Engine(ILog log)
        {
            this.log = log;
            id = new Random().Next();
        }

        public Engine(ILog log, int id)
        {
            this.log = log;
            this.id = id;
        }

        public void Ahead(int power)
        {
            log.Write($"Engine [{id}] ahead {power}");
        }
    }

    public class SMSLog : ILog
    {
        private string phoneNumber;

        public SMSLog(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public void Write(string message)
        {
            Console.WriteLine($"SMS to {phoneNumber} : {message}");
        }
    }

    public class Car
    {
        private Engine engine;
        private ILog log;

        public Car(Engine engine)
        {
            this.engine = engine;
            this.log = new EmailLog();
        }

        public Car(Engine engine, ILog log)
        {
            this.engine = engine;
            this.log = log;
        }

        public void Go()
        {
            engine.Ahead(100);
            log.Write("Car going forward...");
        }
    }

    public class Parent
    {
        public override string ToString()
        {
            return "I am your .....";
        }
    }

    public class Child
    {
        public string Name { get; set; }
        public Parent Parent { get; set; }

        public void SetParent(Parent parent)
        {
            Parent = parent;
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            // named parameter
            //      builder.RegisterType<SMSLog>()
            //        .As<ILog>()
            //        .WithParameter("phoneNumber", "+12345678");

            // typed parameter
            //      builder.RegisterType<SMSLog>()
            //        .As<ILog>()
            //        .WithParameter(new TypedParameter(typeof(string), "+12345678"));

            // resolved parameter
            //builder.RegisterType<SMSLog>()
            //     .As<ILog>()
            //     .WithParameter(new ResolvedParameter(
            //         // predicate
            //         (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "phoneNumber",
            //          // value
            //         (pi, ctx) => "+12345678"
            //         ));


            //Random random = new Random();
            //builder.Register((c, p) => new SMSLog(p.Named<string>("phoneNumber")))
            //  .As<ILog>();

            //builder.RegisterType<Parent>();

            // builder.RegisterType<Child>().PropertiesAutowired(); // if inject property type without constructor
            //builder.RegisterType<Child>()
            //    .WithProperty("Parent", new Parent()); // if inject property type without constructor

            // registration method
            //builder.Register(c =>
            //{
            //    var child = new Child();
            //    child.SetParent(c.Resolve<Parent>());
            //    return child;
            //});
            //builder.RegisterType<Child>()
            //    .OnActivated(e  =>
            //    {
            //        var p = e.Context.Resolve<Parent>();
            //        e.Instance.SetParent(p);
            //    });

            // registration all types using reflection
            var assembly = Assembly.GetExecutingAssembly();
            //builder.RegisterAssemblyTypes(assembly)
            //    .Where(t => t.Name.EndsWith("Log"))
            //    .Except<SMSLog>()
            //    .Except<ConsoleLog>(c => c.As<ILog>().SingleInstance())
            //    .AsSelf();

            //builder.RegisterAssemblyTypes(assembly)
            //    .Except<SMSLog>()
            //    .Where(t => t.Name.EndsWith("Log"))
            //    .As(t => t.GetInterfaces()[0])
            //    .AsSelf(); // if want registration type with those classes first interfaces

            //var ob = container.Resolve<SMSLog>(new NamedParameter("phoneNumber", ""));
            //Console.WriteLine(ob);
            //var parent = container.Resolve<Child>().Parent;
            //Console.WriteLine(parent);


            //var log = container.Resolve<ILog>(new NamedParameter("phoneNumber", random.Next().ToString()));
            //log.Write("Testing");

            // registration module
            builder.RegisterAssemblyModules(typeof(Program).Assembly); // all module
            builder.RegisterAssemblyModules<ParentChildModule>(typeof(Program).Assembly); // specific module

            Console.WriteLine("About to build container...");
            var container = builder.Build();
        }
    }

    public class ParentChildModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Parent>();
            builder.Register(c => new Child() { Parent = c.Resolve<Parent>()});

            base.Load(builder);
        }
    }
}