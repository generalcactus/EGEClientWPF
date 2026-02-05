using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgeClient.Classes
{
    public class Answer
    {
        public string TaskNumber { get; set; }
        public string Response { get; set; }

        public Answer(string taskNumber, string response)
        {
            TaskNumber = taskNumber;
            Response = response;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Answer answer)
                if (answer.TaskNumber.Equals(TaskNumber))
                    if (Response.Equals(answer.Response))
                        return true;

            return false;
        }
    }
}
