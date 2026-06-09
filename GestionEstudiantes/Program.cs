using System;
using GestionEstudiantes.Data;
using GestionEstudiantes.Services;

namespace GestionEstudiantes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Set console encoding for clean drawing characters
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            // Seed default university data
            AddingEntities.AddEntities();

            // Launch the simplified menu system
            ConsoleMenu.Run();
        }
    }
}
