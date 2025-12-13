using System;
using System.Collections.Generic;
using System.Text;

namespace EgeClient.Classes
{
    public class TaskBase
    {
        public int task_number {  get; set; }
        public string? question { get; set; }
        public string? student_answer { get; set; }
        public string? task_type { get; set; }
        public string? image {  get; set; }
        public string? file {  get; set; }

    }
}
