using GestionEstudiantes.Entities.Base;

namespace GestionEstudiantes.Entities
{
    public class Teacher : Person
    {
        public List<Subject> subjects = new List<Subject>();
        public List<Group> groups = new List<Group>();
        public Teacher(string fullName, List<Subject> subjects) : base(fullName)
        {
            this.subjects = subjects;
        }

        public void AddSubject(Subject subject)
        {
            subjects.Add(subject);
        }

        public List<Subject> GetSubjects()
        {
            return subjects;
        }

        public List<Group> GetGroups()
        {
            return groups;
        }
    }
}
