using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EgeClient.Classes
{
    [Serializable]
    public class TaskData
    {
        public string TaskNumber { get; set; }
        public byte[] Image { get; set; }
        public int TaskWeight { get; set; } = 1;

        public List<FileData> Files { get; set; }

        [JsonIgnore]
        public List<byte[]> Answers { get; set; }

        public TaskData() { }

    }

}
