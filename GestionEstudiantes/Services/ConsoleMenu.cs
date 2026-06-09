using System;
using System.Collections.Generic;
using System.Linq;
using GestionEstudiantes.Entities;
using GestionEstudiantes.Entities.Base;
using GestionEstudiantes.Enums;

namespace GestionEstudiantes.Services
{
    public static class ConsoleMenu
    {
        public static void Run()
        {
            bool exit = false;
            while (!exit)
            {
                PrintHeader();
                Console.WriteLine("Docente Activo: Dr. José Pérez (Estructura de Datos, Programación I, Base de Datos)");
                Console.WriteLine();
                Console.WriteLine("1. Ver Asignaturas y Grupos");
                Console.WriteLine("2. Agregar Estudiante a un Grupo");
                Console.WriteLine("3. Registrar o Actualizar Calificaciones");
                Console.WriteLine("4. Generar Reporte de Notas por Grupo");
                Console.WriteLine("5. Salir");
                Console.WriteLine();
                Console.Write("Seleccione una opción: ");

                string choice = Console.ReadLine() ?? "";
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        ShowSubjectsAndGroups();
                        break;
                    case "2":
                        AddNewStudent();
                        break;
                    case "3":
                        RegisterStudentGrades();
                        break;
                    case "4":
                        ShowGroupReport();
                        break;
                    case "5":
                        exit = true;
                        PrintGoodbye();
                        break;
                    default:
                        PrintError("Opción no válida. Intente de nuevo.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine("=================================================");
            Console.WriteLine("   SISTEMA DE CONTROL ACADÉMICO - UNIVERSIDAD");
            Console.WriteLine("=================================================");
        }

        private static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ERROR] {message}");
            Console.ResetColor();
        }

        private static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[ÉXITO] {message}");
            Console.ResetColor();
        }

        private static void ShowSubjectsAndGroups()
        {
            Console.WriteLine("=== ASIGNATURAS Y GRUPOS DISPONIBLES ===");
            Console.WriteLine();

            var subjects = UniversityService.Subjects;
            if (subjects.Count == 0)
            {
                Console.WriteLine("No hay asignaturas registradas.");
                return;
            }

            foreach (var subject in subjects)
            {
                Console.WriteLine($"* Asignatura: {subject.SubjectName}");
                var groups = UniversityService.Groups.Where(g => g.SubjectId == subject.SubjectId).ToList();
                if (groups.Count == 0)
                {
                    Console.WriteLine("  - (Sin grupos registrados)");
                }
                else
                {
                    foreach (var group in groups)
                    {
                        int studentCount = group.GetStudents().Count;
                        Console.WriteLine($"  - {group.GroupName} ({studentCount} estudiantes)");
                    }
                }
                Console.WriteLine();
            }
        }

        private static Group? SelectGroupPrompt(string actionDescription)
        {
            var groups = UniversityService.Groups;
            if (groups.Count == 0)
            {
                PrintError("No hay grupos registrados en el sistema.");
                return null;
            }

            Console.WriteLine($"=== SELECCIONAR GRUPO PARA {actionDescription.ToUpper()} ===");
            Console.WriteLine();

            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                var subject = UniversityService.Subjects.FirstOrDefault(s => s.SubjectId == group.SubjectId);
                string subjectName = subject?.SubjectName ?? "Desconocida";
                Console.WriteLine($"[{i + 1}] {subjectName} - {group.GroupName}");
            }
            Console.WriteLine();

            int selection = ReadInteger("Seleccione el número de grupo: ", 1, groups.Count);
            return groups[selection - 1];
        }

        private static void AddNewStudent()
        {
            var selectedGroup = SelectGroupPrompt("Agregar Estudiante");
            if (selectedGroup == null) return;

            Console.Clear();
            PrintHeader();
            
            var subject = UniversityService.Subjects.FirstOrDefault(s => s.SubjectId == selectedGroup.SubjectId);
            Console.WriteLine($"Agregando estudiante a: {subject?.SubjectName} - {selectedGroup.GroupName}");
            Console.WriteLine();

            Console.Write("Ingrese el nombre completo del estudiante: ");
            string name = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                PrintError("El nombre no puede estar vacío.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Seleccione el tipo de estudiante:");
            Console.WriteLine("1. Presencial (Evaluación: Examen 70%, Teoría 10%, Práctica 20%)");
            Console.WriteLine("2. A Distancia/Virtual (Evaluación: Examen 60%, Práctica 40%)");
            Console.WriteLine();
            
            int typeChoice = ReadInteger("Selección (1 o 2): ", 1, 2);
            StudentType studentType = typeChoice == 1 ? StudentType.Presential : StudentType.Virtual;

            OperationResult result = UniversityService.AddStudentToGroup(selectedGroup.GroupId, name, studentType);

            if (result.success)
            {
                PrintSuccess(result.message);
                if (result.data is Student student)
                {
                    Console.WriteLine($"Detalles registrados: {student}");
                }
            }
            else
            {
                PrintError(result.message);
            }
        }

        private static void RegisterStudentGrades()
        {
            var selectedGroup = SelectGroupPrompt("Registrar Notas");
            if (selectedGroup == null) return;

            var students = selectedGroup.GetStudents();
            if (students.Count == 0)
            {
                Console.Clear();
                PrintError($"No hay estudiantes inscritos en {selectedGroup.GroupName}.");
                return;
            }

            Console.Clear();
            PrintHeader();
            var subject = UniversityService.Subjects.FirstOrDefault(s => s.SubjectId == selectedGroup.SubjectId);
            Console.WriteLine($"Registrando calificaciones para: {subject?.SubjectName} - {selectedGroup.GroupName}");
            Console.WriteLine();
            
            Console.WriteLine("Seleccione el estudiante:");
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {students[i]}");
            }
            Console.WriteLine();

            int studentIndex = ReadInteger("Seleccione el estudiante: ", 1, students.Count);
            var selectedStudent = students[studentIndex - 1];

            int exams = 0;
            int theoretical = 0;
            int practical = 0;

            Console.Clear();
            PrintHeader();
            Console.WriteLine($"Ingresando notas para: {selectedStudent.FullName}");
            Console.WriteLine($"Tipo: {(selectedStudent is PresentialStudent ? "Presencial" : "A Distancia (Virtual)")}");
            Console.WriteLine();

            if (selectedStudent is PresentialStudent)
            {
                Console.WriteLine("Límites: Examen (0-70), Teoría (0-10), Práctica (0-20)");
                exams = ReadInteger("Nota de Examen (0-70): ", 0, 70);
                theoretical = ReadInteger("Nota Teórica / Participación (0-10): ", 0, 10);
                practical = ReadInteger("Nota Práctica (0-20): ", 0, 20);
            }
            else if (selectedStudent is VirtualStudent)
            {
                Console.WriteLine("Límites: Examen Virtual (0-60), Trabajo Práctico (0-40)");
                exams = ReadInteger("Nota de Examen Virtual (0-60): ", 0, 60);
                practical = ReadInteger("Nota Trabajo Práctico (0-40): ", 0, 40);
                theoretical = 0;
            }

            OperationResult result = UniversityService.RegisterGrade(selectedGroup.GroupId, selectedStudent.Id, exams, theoretical, practical);

            if (result.success)
            {
                PrintSuccess(result.message);
                var activeGrade = result.data as Grade;
                if (activeGrade != null)
                {
                    double finalGrade = selectedStudent.CalculateFinalGrade(activeGrade);
                    string status = finalGrade >= 70 ? "APROBADO" : "REPROBADO";
                    
                    Console.WriteLine();
                    Console.WriteLine("Calificación Guardada:");
                    Console.WriteLine($"  - Examen:     {activeGrade.ExamsPoint}");
                    Console.WriteLine($"  - Teoría:     {(selectedStudent is PresentialStudent ? activeGrade.TeoricalPoints.ToString() : "N/A")}");
                    Console.WriteLine($"  - Práctica:   {activeGrade.PracticalPoints}");
                    
                    Console.ForegroundColor = finalGrade >= 70 ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine($"  - Nota Final: {finalGrade} / 100 ({status})");
                    Console.ResetColor();
                }
            }
            else
            {
                PrintError(result.message);
            }
        }

        private static void ShowGroupReport()
        {
            var selectedGroup = SelectGroupPrompt("Generar Reporte");
            if (selectedGroup == null) return;

            Console.Clear();
            PrintHeader();

            var subject = UniversityService.Subjects.FirstOrDefault(s => s.SubjectId == selectedGroup.SubjectId);
            Console.WriteLine($"=== REPORTE DE NOTAS: {subject?.SubjectName} - {selectedGroup.GroupName} ===");
            Console.WriteLine();

            var reportResult = UniversityService.GetGroupReport(selectedGroup.GroupId);
            var metricsResult = UniversityService.CalculateGroupMetrics(selectedGroup.GroupId);

            if (!reportResult.success || !metricsResult.success)
            {
                PrintError($"No se pudo obtener el reporte: {reportResult.message}");
                return;
            }

            var reportItems = reportResult.data as List<StudentReportItem>;
            var metrics = metricsResult.data as GroupMetrics;

            if (reportItems == null || metrics == null)
            {
                PrintError("El formato del reporte o las métricas es inválido.");
                return;
            }

            if (reportItems.Count == 0)
            {
                Console.WriteLine("El grupo no tiene estudiantes inscritos.");
                return;
            }

            // Print Simple Table
            Console.WriteLine("Estudiante           | Tipo        | Examen | Teoria | Practica | Nota Final | Estado");
            Console.WriteLine("-----------------------------------------------------------------------------------------");

            foreach (var item in reportItems)
            {
                string namePart = item.FullName.Length > 20 ? item.FullName.Substring(0, 17) + "..." : item.FullName.PadRight(20);
                string typePart = item.StudentType == StudentType.Presential ? "Presencial".PadRight(11) : "A Distancia".PadRight(11);
                string examPart = item.ExamsPoint.ToString().PadLeft(6);
                string theoPart = item.StudentType == StudentType.Presential ? item.TeoricalPoints.ToString().PadLeft(6) : "   N/A";
                string pracPart = item.PracticalPoints.ToString().PadLeft(8);
                string finalPart = item.FinalGrade.ToString("F1").PadLeft(10);
                string statusPart = item.IsApproved ? "Aprobado" : "Reprobado";

                Console.Write($"{namePart} | {typePart} | {examPart} | {theoPart} | {pracPart} | ");
                if (item.IsApproved)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(finalPart);
                    Console.ResetColor();
                    Console.Write(" | ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(statusPart);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(finalPart);
                    Console.Write(" | ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(statusPart);
                    Console.ResetColor();
                }
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine();

            // Print simple metrics panel
            Console.WriteLine("ESTADÍSTICAS DEL GRUPO:");
            Console.WriteLine($"- Total Estudiantes:      {metrics.TotalStudents}");
            Console.WriteLine($"- Estudiantes Aprobados:  {metrics.ApprovedStudents}");
            Console.WriteLine($"- Estudiantes Reprobados: {metrics.ReprovedStudents}");
            Console.Write("- Porcentaje Aprobados:   ");
            
            Console.ForegroundColor = metrics.ApprovedPercentage >= 70 ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{metrics.ApprovedPercentage:F2}%");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static int ReadInteger(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";
                if (int.TryParse(input, out int result) && result >= min && result <= max)
                {
                    return result;
                }
                PrintError($"Entrada inválida. Debe ser un número entero entre {min} y {max}.");
            }
        }

        private static void PrintGoodbye()
        {
            Console.WriteLine();
            Console.WriteLine("¡Gracias por utilizar el Sistema de Control Académico!");
            Console.WriteLine("Cerrando la aplicación...");
        }
    }
}
