using GestionEstudiantes.Entities.Base;
using GestionEstudiantes.Enums;

namespace GestionEstudiantes.Entities
{
    public class PresentialStudent : Student
    {
        public PresentialStudent(string fullName, StudentType studentType = StudentType.Presential) : base(fullName)
        {
            StudentType = StudentType.Presential;
        }

        public override double CalculateFinalGrade(Grade grade)
        {
            if (grade == null) return 0;
            return grade.ExamsPoint + grade.TeoricalPoints + grade.PracticalPoints;
        }

        public override string ToString()
        {
            return $"[Presencial] {FullName} (ID: {Id.ToString().Substring(0, 8)})";
        }
    }
}
