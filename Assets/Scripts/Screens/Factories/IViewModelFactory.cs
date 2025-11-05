namespace Screens.Factories
{
    public interface IViewModelFactory<out TViewModel, in TParam>
    {
        TViewModel Create(TParam param);
    }
}