namespace WFC.Interfaces
{
    public interface IInputReader<T>
    {
        IValue<T>[][] ReadInputToGrid();
    }
}
