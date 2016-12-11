using System;

namespace Zadatak_1
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }

        public Guid UserId { get; set; }
        public ToDoItem(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            IsCompleted = false;
            DateCreated = DateTime.Now;
            UserId = userId;
        }

        public ToDoItem()
        {
        }

        public bool MarkAsDone()
        {
            if (IsCompleted)
                return false;
            IsCompleted = true;
            DateCompleted = DateTime.Now;
            return true;
        }

        public override string ToString()
        {
            return $"Item: {Text}, Created on {DateCreated}";
        }
    }
}
