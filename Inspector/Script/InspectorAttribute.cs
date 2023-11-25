using UnityEngine;

namespace Ayla.Inspector
{
    public class InspectorAttribute : PropertyAttribute
    {
        public int GroupId { get; set; }

        public int LogicalOrder { get; set; }
    }
}
