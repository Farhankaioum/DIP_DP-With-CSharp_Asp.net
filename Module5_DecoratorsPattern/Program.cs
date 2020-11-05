using System;
using Autofac;

namespace AutofacSamples
{
    public interface IReportingService
    {
        void Report();

    }

    public class ReportingService : IReportingService
    {
        public void Report()
        {
            Console.WriteLine("Here is your report");
        }
    }

    public class ReportingServiceWithLogging : IReportingService
    {
        private IReportingService decorated;

        public ReportingServiceWithLogging(IReportingService decorated)
        {
            this.decorated = decorated;
        }

        public void Report()
        {
            Console.WriteLine("Commencing log...");
            decorated.Report();
            Console.WriteLine("Ending log...");
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var b = new ContainerBuilder();
            b.RegisterType<ReportingService>().Named<IReportingService>("reporting");
            b.RegisterDecorator<IReportingService>(
              (context, service) => new ReportingServiceWithLogging(service), "reporting"
            ); // if you want to inherit a interface and also inject this interface, this interface use another class
                            // you want invoke inject type members then for registration using decorator pattern

            using (var c = b.Build())
            {
                var r = c.Resolve<IReportingService>();
                r.Report();
            }
        }
    }
}