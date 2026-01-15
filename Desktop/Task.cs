using System;

namespace Desktop
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int UserId { get; set; }

        public DateTime? CompletedDate { get; set; }
    }
}