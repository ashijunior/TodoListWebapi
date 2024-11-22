using System.ComponentModel.DataAnnotations;

namespace TodoListWebapi.Model
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public string UserId { get; set; }

    }
}
