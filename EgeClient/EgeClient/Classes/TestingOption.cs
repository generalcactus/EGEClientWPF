using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EgeClient.Classes
{
    [Serializable]
    public class TestingOption
    {
        [JsonInclude]
        public string OptionID { get; private set; }
        public List<TaskData> TaskList { get; set; } = new();

        [JsonIgnore]
        public List<string> ResponsesList { get; private set; } = new();

        public TestingOption() { }

        public TestingOption(string id)
        {
            OptionID = id;
        }

        public void AddTask(TaskData data)
        {
            TaskList.Add(data);
        }
    }

}
