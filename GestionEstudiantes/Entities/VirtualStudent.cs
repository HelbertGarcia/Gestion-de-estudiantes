using GestionEstudiantes.Entities.Base;
using GestionEstudiantes.Enums;

namespace GestionEstudiantes.Entities
{
    public class VirtualStudent : Student
    {
        public VirtualStudent(string fullName, StudentType studentType = StudentType.Virtual) : base(fullName)
        {
            StudentType = StudentType.Virtual;
        }

        public override double CalculateFinalGrade(Grade grade)
        {
            if (grade == null) return 0;
            // Virtual students do not have theorical points; their final grade is based only on Exams (max 60) and Practical work (max 40)
            return grade.ExamsPoint + grade.PracticalPoints;
        }

        public override string ToString()
        {
            return $"[A distancia] {FullName} (ID: {Id.ToString().Substring(0, 8)})";
        }
    }
}
