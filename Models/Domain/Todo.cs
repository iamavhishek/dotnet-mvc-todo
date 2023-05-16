namespace FinalTodoApp.Models.Domain
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Due { get; set; }
        public bool isCompleted { get; set; }
    }
}
