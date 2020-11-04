using System;
using System.Collections.Generic;
using Autofac;

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

        public void Ahead(int power)
        {
            log.Write($"Engine [{id}] ahead {power}");
        }
    }

    public class Car
    {
        private Engine engine;
        private ILog log;

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

    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EmailLog>()
              .As<ILog>();

            builder.RegisterType<ConsoleLog>()
                   .As<ILog>()
                   .As<IConsole>()
                   .PreserveExistingDefaults();// for preventing user last registration type, if they use two/more type reg into one super type
                   //.UsingConstructor(typeof(ILog)); // for using constructor

            var log = new ConsoleLog();
            builder.RegisterInstance(log).As<ILog>(); // if want to register a instance

            builder.Register((IComponentContext c) => new Engine(c.Resolve<ILog>())); // using lamda to register a type, if want to resovle type inside
                                                                                      // new Object(c.Resolve<type>()) use (IComponentContext c) => new Obj()

            builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>)); // registering generic class/type
           

            builder.RegisterType<Engine>();

            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            var myList = container.Resolve<IList<int>>(); // find generic type object


            var car = container.Resolve<Car>();
            car.Go();
        }
    }
}