using System.Collections.Generic;

namespace DAL.Entities
{
    public class Union : BaseEntity
    {
        public int Partner1Id { get; set; }
        public int Partner2Id { get; set; }
        public List<int> ChildrenIds { get; set; } = new List<int>();
    }
}