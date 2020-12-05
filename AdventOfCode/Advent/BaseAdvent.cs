namespace AdventOfCode.Advent
{
    public abstract class BaseAdvent
    {
        protected string filePath;
        
        public BaseAdvent(string filePath)
        {
            this.filePath = filePath;
        }

        public abstract void Execute();
    }
}