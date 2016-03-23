using System.Collections.Specialized;

namespace Industrial.Data.Domain
{
    public class Branch:BaseClass<int>
    {
        public string Name { get; set; }
        public int? BranchId { get; set; }
    }
}