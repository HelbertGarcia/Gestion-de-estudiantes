using System;
using System.Collections.Generic;
using System.Linq;
using GestionEstudiantes.Entities;
using GestionEstudiantes.Entities.Base;
using GestionEstudiantes.Enums;
using GestionEstudiantes.Services;

namespace GestionEstudiantes.Data
{
    public static class AddingEntities
    {
        public static void AddEntities()
        {
            // Clear lists to prevent duplicate seeding
            UniversityService.Teachers.Clear();
            UniversityService.Subjects.Clear();
            UniversityService.Groups.Clear();
            UniversityService.Grades.Clear();

            // 1. Create Subjects
            var prog1 = new Subject("Programación I");
            var estDatos = new Subject("Estructura de Datos");
            var bd = new Subject("Base de Datos");

            UniversityService.Subjects.Add(prog1);
            UniversityService.Subjects.Add(estDatos);
            UniversityService.Subjects.Add(bd);

            // 2. Create Teacher
            var teacher = new Teacher("Dr. José Pérez", new List<Subject> { prog1, estDatos, bd });
            UniversityService.Teachers.Add(teacher);

            // 3. Create Groups for the subjects
            var groupProgA = new Group(teacher.Id, prog1.SubjectId, "Grupo A");
            var groupEstA = new Group(teacher.Id, estDatos.SubjectId, "Grupo A");
            var groupEstB = new Group(teacher.Id, estDatos.SubjectId, "Grupo B");
            var groupBdA = new Group(teacher.Id, bd.SubjectId, "Grupo A");

            UniversityService.Groups.Add(groupProgA);
            UniversityService.Groups.Add(groupEstA);
            UniversityService.Groups.Add(groupEstB);
            UniversityService.Groups.Add(groupBdA);

            // Link groups to teacher
            teacher.groups.Add(groupProgA);
            teacher.groups.Add(groupEstA);
            teacher.groups.Add(groupEstB);
            teacher.groups.Add(groupBdA);

            // 4. Seed students into "Estructura de Datos - Grupo A"
            UniversityService.AddStudentToGroup(groupEstA.GroupId, "Carlos Mendoza", StudentType.Presential);
            UniversityService.AddStudentToGroup(groupEstA.GroupId, "Ana Sofia Gómez", StudentType.Virtual);
            UniversityService.AddStudentToGroup(groupEstA.GroupId, "Luisa Fernández", StudentType.Presential);
            UniversityService.AddStudentToGroup(groupEstA.GroupId, "David Ortega", StudentType.Virtual);

            // 5. Register grades for these students
            var students = groupEstA.GetStudents();
            var carlos = students.FirstOrDefault(s => s.FullName == "Carlos Mendoza");
            var ana = students.FirstOrDefault(s => s.FullName == "Ana Sofia Gómez");
            var luisa = students.FirstOrDefault(s => s.FullName == "Luisa Fernández");
            var david = students.FirstOrDefault(s => s.FullName == "David Ortega");

            if (carlos != null)
                UniversityService.RegisterGrade(groupEstA.GroupId, carlos.Id, 65, 8, 18);

            if (ana != null)
                UniversityService.RegisterGrade(groupEstA.GroupId, ana.Id, 52, 0, 35);

            if (luisa != null)
                UniversityService.RegisterGrade(groupEstA.GroupId, luisa.Id, 40, 5, 10);

            if (david != null)
                UniversityService.RegisterGrade(groupEstA.GroupId, david.Id, 30, 0, 20);
            
            // Seed a student into Programacion I - Grupo A
            UniversityService.AddStudentToGroup(groupProgA.GroupId, "María Becerra", StudentType.Presential);
            var maria = groupProgA.GetStudents().FirstOrDefault(s => s.FullName == "María Becerra");
            if (maria != null)
                UniversityService.RegisterGrade(groupProgA.GroupId, maria.Id, 70, 10, 20);
        }
    }
}
