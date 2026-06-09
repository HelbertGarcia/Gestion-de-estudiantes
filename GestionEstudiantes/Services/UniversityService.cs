using System;
using System.Collections.Generic;
using System.Linq;
using GestionEstudiantes.Entities;
using GestionEstudiantes.Entities.Base;
using GestionEstudiantes.Enums;

namespace GestionEstudiantes.Services
{
    public static class UniversityService
    {
        // Centralized in-memory database
        public static List<Teacher> Teachers { get; } = new List<Teacher>();
        public static List<Subject> Subjects { get; } = new List<Subject>();
        public static List<Group> Groups { get; } = new List<Group>();
        public static List<Grade> Grades { get; } = new List<Grade>();

        /// <summary>
        /// Adds a student to a specific group in a subject.
        /// </summary>
        public static OperationResult AddStudentToGroup(Guid groupId, string fullName, StudentType type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fullName))
                {
                    return OperationResult.FailureResult("El nombre completo del estudiante no puede estar vacío.");
                }

                var group = Groups.FirstOrDefault(g => g.GroupId == groupId);
                if (group == null)
                {
                    return OperationResult.FailureResult("El grupo especificado no existe.");
                }

                // Check if student with same name already exists in this group to avoid duplicates
                if (group.GetStudents().Any(s => s.FullName != null && s.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase)))
                {
                    return OperationResult.FailureResult($"El estudiante '{fullName}' ya se encuentra inscrito en este grupo.");
                }

                Student student;
                if (type == StudentType.Presential)
                {
                    student = new PresentialStudent(fullName);
                }
                else if (type == StudentType.Virtual)
                {
                    student = new VirtualStudent(fullName);
                }
                else
                {
                    return OperationResult.FailureResult("Tipo de estudiante no soportado.");
                }

                group.AddStudent(student);
                return OperationResult.SuccessResult("Estudiante agregado exitosamente al grupo.", student);
            }
            catch (Exception ex)
            {
                return OperationResult.FailureResult($"Error al agregar estudiante: {ex.Message}");
            }
        }

        /// <summary>
        /// Registers or updates a student's grade in a group.
        /// </summary>
        public static OperationResult RegisterGrade(Guid groupId, Guid studentId, int examsPoint, int teoricalPoints, int practicalPoints)
        {
            try
            {
                var group = Groups.FirstOrDefault(g => g.GroupId == groupId);
                if (group == null)
                {
                    return OperationResult.FailureResult("El grupo especificado no existe.");
                }

                var student = group.GetStudents().FirstOrDefault(s => s.Id == studentId);
                if (student == null)
                {
                    return OperationResult.FailureResult("El estudiante no pertenece a este grupo.");
                }

                // Validation rules based on student type
                if (student is PresentialStudent)
                {
                    if (examsPoint < 0 || examsPoint > 70)
                    {
                        return OperationResult.FailureResult("Para estudiantes presenciales, la nota de examen debe estar entre 0 y 70.");
                    }
                    if (teoricalPoints < 0 || teoricalPoints > 10)
                    {
                        return OperationResult.FailureResult("Para estudiantes presenciales, la nota teórica debe estar entre 0 y 10.");
                    }
                    if (practicalPoints < 0 || practicalPoints > 20)
                    {
                        return OperationResult.FailureResult("Para estudiantes presenciales, la nota práctica debe estar entre 0 y 20.");
                    }
                }
                else if (student is VirtualStudent)
                {
                    if (examsPoint < 0 || examsPoint > 60)
                    {
                        return OperationResult.FailureResult("Para estudiantes a distancia (virtuales), la nota de examen debe estar entre 0 y 60.");
                    }
                    if (practicalPoints < 0 || practicalPoints > 40)
                    {
                        return OperationResult.FailureResult("Para estudiantes a distancia (virtuales), la nota práctica debe estar entre 0 y 40.");
                    }
                    if (teoricalPoints != 0)
                    {
                        // Set theoretical points to 0 since virtual students do not participate in person
                        teoricalPoints = 0;
                    }
                }

                // Look for existing grade for this student in this subject
                var existingGrade = Grades.FirstOrDefault(g => g.StudentId == studentId && g.SubjectId == group.SubjectId);
                if (existingGrade != null)
                {
                    existingGrade.ExamsPoint = examsPoint;
                    existingGrade.TeoricalPoints = teoricalPoints;
                    existingGrade.PracticalPoints = practicalPoints;
                    return OperationResult.SuccessResult("Calificación actualizada exitosamente.", existingGrade);
                }
                else
                {
                    var newGrade = new Grade(group.SubjectId, studentId, examsPoint, teoricalPoints, practicalPoints);
                    Grades.Add(newGrade);
                    return OperationResult.SuccessResult("Calificación registrada exitosamente.", newGrade);
                }
            }
            catch (Exception ex)
            {
                return OperationResult.FailureResult($"Error al registrar calificación: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates a list of students with grades and final calculations for a group.
        /// </summary>
        public static OperationResult GetGroupReport(Guid groupId)
        {
            try
            {
                var group = Groups.FirstOrDefault(g => g.GroupId == groupId);
                if (group == null)
                {
                    return OperationResult.FailureResult("El grupo especificado no existe.");
                }

                var students = group.GetStudents();
                var reportList = new List<StudentReportItem>();

                foreach (var student in students)
                {
                    var grade = Grades.FirstOrDefault(g => g.StudentId == student.Id && g.SubjectId == group.SubjectId);
                    
                    // If no grade is registered, we create a temporary zero grade
                    var activeGrade = grade ?? new Grade(group.SubjectId, student.Id, 0, 0, 0);
                    double finalGrade = student.CalculateFinalGrade(activeGrade);
                    bool isApproved = finalGrade >= 70;

                    reportList.Add(new StudentReportItem
                    {
                        StudentId = student.Id,
                        FullName = student.FullName ?? string.Empty,
                        StudentType = student.StudentType,
                        ExamsPoint = activeGrade.ExamsPoint,
                        TeoricalPoints = activeGrade.TeoricalPoints,
                        PracticalPoints = activeGrade.PracticalPoints,
                        FinalGrade = finalGrade,
                        IsApproved = isApproved
                    });
                }

                return OperationResult.SuccessResult("Reporte generado exitosamente.", reportList);
            }
            catch (Exception ex)
            {
                return OperationResult.FailureResult($"Error al generar reporte: {ex.Message}");
            }
        }

        /// <summary>
        /// Calculates metrics for a specific group (approved percentage).
        /// </summary>
        public static OperationResult CalculateGroupMetrics(Guid groupId)
        {
            try
            {
                var reportResult = GetGroupReport(groupId);
                if (!reportResult.success)
                {
                    return reportResult;
                }

                var reportItems = reportResult.data as List<StudentReportItem>;
                if (reportItems == null)
                {
                    return OperationResult.FailureResult("El formato del reporte no es válido.");
                }
                int totalStudents = reportItems.Count;
                if (totalStudents == 0)
                {
                    var emptyMetrics = new GroupMetrics
                    {
                        TotalStudents = 0,
                        ApprovedStudents = 0,
                        ReprovedStudents = 0,
                        ApprovedPercentage = 0
                    };
                    return OperationResult.SuccessResult("Métricas calculadas para un grupo vacío.", emptyMetrics);
                }

                int approved = reportItems.Count(item => item.IsApproved);
                int reproved = totalStudents - approved;
                double percentage = ((double)approved / totalStudents) * 100.0;

                var metrics = new GroupMetrics
                {
                    TotalStudents = totalStudents,
                    ApprovedStudents = approved,
                    ReprovedStudents = reproved,
                    ApprovedPercentage = Math.Round(percentage, 2)
                };

                return OperationResult.SuccessResult("Métricas del grupo calculadas exitosamente.", metrics);
            }
            catch (Exception ex)
            {
                return OperationResult.FailureResult($"Error al calcular métricas: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Helper class to represent a student's report record.
    /// </summary>
    public class StudentReportItem
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public StudentType StudentType { get; set; }
        public int ExamsPoint { get; set; }
        public int TeoricalPoints { get; set; }
        public int PracticalPoints { get; set; }
        public double FinalGrade { get; set; }
        public bool IsApproved { get; set; }
    }

    /// <summary>
    /// Helper class to represent group statistics.
    /// </summary>
    public class GroupMetrics
    {
        public int TotalStudents { get; set; }
        public int ApprovedStudents { get; set; }
        public int ReprovedStudents { get; set; }
        public double ApprovedPercentage { get; set; }
    }
}
