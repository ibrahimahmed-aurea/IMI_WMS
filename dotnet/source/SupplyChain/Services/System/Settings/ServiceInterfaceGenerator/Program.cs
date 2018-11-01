using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.ServiceLayerGenerator;

namespace ServiceInterfaceGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            GeneratorEngine.Generate();
            Console.ReadLine();
        }
    }
}
