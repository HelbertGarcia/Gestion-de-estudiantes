namespace GestionEstudiantes.Entities
{
    public class Grade
    {
        public Guid GradeId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid StudentId { get; set; }
        public int ExamsPoint { get; set; }
        public int TeoricalPoints { get; set; }
        public int PracticalPoints { get; set; }

        public Grade(Guid subjectId, Guid studentId, int examsPoint, int teoricalPoints, int practicalPoints)
        {
            GradeId = Guid.NewGuid();
            SubjectId = subjectId;
            StudentId = studentId;
            ExamsPoint = examsPoint;
            TeoricalPoints = teoricalPoints;
            PracticalPoints = practicalPoints;
        }

        public int GetGrade(Guid subjectId, Guid studentId)
        {
            return ExamsPoint + TeoricalPoints + PracticalPoints;
        }

        

    }
}
