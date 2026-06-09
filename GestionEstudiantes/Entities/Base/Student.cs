using GestionEstudiantes.Enums;
using GestionEstudiantes.Entities;

namespace GestionEstudiantes.Entities.Base
{
    public abstract class Student : Person
    {
        List<Subject> _subjects;
        public StudentType StudentType { get; protected set; }
        public List<Subject> Subjects => _subjects;

        protected Student(string fullName) : base(fullName)
        {
            _subjects = new List<Subject>();
        }

        public abstract double CalculateFinalGrade(Grade grade);
    }
}
