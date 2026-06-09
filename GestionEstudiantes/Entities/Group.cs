using GestionEstudiantes.Entities.Base;

namespace GestionEstudiantes.Entities
{
    public class Group
    {
        public Guid GroupId { get; set; }
        public Guid TeacherId { get; set; }
        public Guid SubjectId { get; set; }
        public string GroupName { get; set; }
        private List<Student> _students;
        public Group(Guid teacherId, Guid subjectId, string groupName)
        {
            GroupId = Guid.NewGuid();
            TeacherId = teacherId;
            SubjectId = subjectId;
            GroupName = groupName;
            _students = new List<Student>();
        }

        public void AddStudent(Student student)
        {
            _students.Add(student);
        }

        public List<Student> GetStudents()
        {
            return _students;
        }

    }
}
