using System.Collections.Generic;

namespace DAL.Entities
{
    public class Union
    {
        public int Partner1Id { get; set; }
        public int Partner2Id { get; set; }
        public int ChildId { get; set; }
    }
}
