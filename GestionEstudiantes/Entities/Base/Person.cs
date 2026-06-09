namespace GestionEstudiantes.Entities.Base
{
    public abstract class Person
    {
        protected Person(string fullName)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
        }
        public Guid Id { get; set; }
        public string? FullName { get; set; }
    }
}
