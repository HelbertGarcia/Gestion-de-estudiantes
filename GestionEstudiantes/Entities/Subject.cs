namespace GestionEstudiantes.Entities
{
    public class Subject
    {
        public Subject(string subjectName)
        {
            SubjectId = Guid.NewGuid();
            SubjectName = subjectName;
        }
        public Guid SubjectId { get; set; }
        public string? SubjectName { get; set; }
    }
}
