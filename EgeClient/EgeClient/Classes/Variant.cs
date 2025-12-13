using System;
using System.Collections.Generic;
using System.Text;

namespace EgeClient.Classes
{
    public class Variant
    {
        public ExamInfo ExamInfo { get; set; }
        public List<TaskBase> Tasks{ get; set; }
        public Student Student { get; set; }
    }
}
