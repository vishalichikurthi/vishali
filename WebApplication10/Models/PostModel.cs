using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication10.Models
{
    public class PostModel
    {
        public int LoadId { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class ResponseModel
    {
        public int RobotId { get; set; }
        public decimal DistanceToGoal { get; set; }
        public int batteryLevel { get; set; }
    }

    public class RestAPI
    {
        public int RobotId { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int batteryLevel { get; set; }

    }
}
