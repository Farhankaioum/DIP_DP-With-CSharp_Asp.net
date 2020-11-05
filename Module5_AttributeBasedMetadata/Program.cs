using Autofac;
using Autofac.Extras.AttributeMetadata;
using Autofac.Features.AttributeFilters;
using System;
using System.ComponentModel.Composition;

namespace Module5_AttributeBasedMetadata
{
    [MetadataAttribute]
    public class AgeMetadataAttribute : Attribute
    {
        public int Age { get; set; }
        public AgeMetadataAttribute(int age)
        {
            Age = age;
        }
    }

    public interface IArtWork
    {
        void Display();
    }

    [AgeMetadata(100)]
    public class CenturyArtwork : IArtWork
    {
        public void Display()
        {
            Console.WriteLine("Displaying a century-old piece");
        }
    }

    [AgeMetadata(1000)]
    public class MillenialArtwork : IArtWork
    {
        public void Display()
        {
            Console.WriteLine("Displaying a Really-old piece of art");
        }
    }

    public class ArtDisplay
    {
        private readonly IArtWork artWork;

        public ArtDisplay([MetadataFilter("Age", 1000)] IArtWork artWork)
        {
            this.artWork = artWork;
        }

        public void Display()
        {
            artWork.Display();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AttributedMetadataModule>(); // registering autofac AttributedMetadataModule for working Attribute
            builder.RegisterType<CenturyArtwork>().As<IArtWork>();
            builder.RegisterType<MillenialArtwork>().As<IArtWork>();
            using (var container = builder.Build())
            {
               var artDisplay = container.Resolve<ArtDisplay>();
                artDisplay.Display();
            }


        }
    }
}
