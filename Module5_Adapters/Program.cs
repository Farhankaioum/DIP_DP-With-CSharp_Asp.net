using Autofac;
using System;
using System.Collections.Generic;
using Autofac.Features.Metadata;

namespace Module5_Adapters
{
    public interface ICommand
    {
        void Command();
    }

    public class SaveCommand : ICommand
    {
        public void Command()
        {
            Console.WriteLine("Saving a file");
        }
    }
    public class OpenCommand : ICommand
    {
        public void Command()
        {
            Console.WriteLine("Opening a file");
        }
    }

    public class Button
    {
        private ICommand command;
        private string name;

        public Button(ICommand command, string name)
        {
            if(command == null)
                throw new ArgumentNullException(paramName: nameof(command));

            this.command = command;
            this.name = name;
        }

        public void Click()
        {
            command.Command();
        }

        public void PrintMe()
        {
            Console.WriteLine($"You are clicked button {name}");
        }
    }

    public class Editor
    {
        private readonly IEnumerable<Button> buttons;

        public IEnumerable<Button> Buttons
        {
            get { return buttons; }
        }

        public Editor(IEnumerable<Button> buttons)
        {
            if (buttons == null)
                throw new ArgumentNullException(paramName: nameof(buttons));

            this.buttons = buttons;
        }

        public void ClickAll()
        {
            foreach (var btn in buttons)
                btn.Click();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SaveCommand>().As<ICommand>()
                .WithMetadata("Name", "Save");
            builder.RegisterType<OpenCommand>().As<ICommand>()
                .WithMetadata("Name", "Open");
            //builder.RegisterType<Button>(); // registration traditionally way
            // builder.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd)); // registration type with adapter
            builder.RegisterAdapter<Meta<ICommand>, Button>(cmd =>
                new Button(cmd.Value, (string)cmd.Metadata["Name"]) 
            ); // registration type with adapter with metadata
            builder.RegisterType<Editor>();

            using (var c = builder.Build())
            {
                var editor = c.Resolve<Editor>();
                // editor.ClickAll();
                foreach (var btn in editor.Buttons)
                {
                    btn.PrintMe();
                }
            }
            
        }
    }
}
