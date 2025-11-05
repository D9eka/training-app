namespace Screens.Factories.Parameters
{
    public class BlockIdParameter : IScreenParameter
    {
        public string BlockId { get; private set; }
        
        public BlockIdParameter(string blockId)
        {
            BlockId = blockId;
        }
    }
}