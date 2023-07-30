using System.Collections.Generic;

namespace SandKing.Simulation
{
    internal class ExecutionTree : HashSet<ExecutionTree>
    {
        public ExecutionTree((int x, int y)? source, (int x, int y)? destination)
        {
            Cell = source;
            Add(new ExecutionTree(destination, null));
        }

        public (int x, int y)? Cell { get; set; }
        
        public void Insert(ExecutionTree node)
        {
            if (node == null) return;
            if (Cell == node.Cell)
            {

            }
        }
    }
}
