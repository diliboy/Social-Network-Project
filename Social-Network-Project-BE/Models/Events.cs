﻿namespace Social_Network_Project_BE.Models
{
    public class Events
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Email { get; set; }
        public int IsActive { get; set; }
        public string CreatedOn { get; set; }
    }
}
