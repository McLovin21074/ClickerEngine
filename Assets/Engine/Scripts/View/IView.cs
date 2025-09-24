using ClickerEngine.Services;

namespace ClickerEngine
{
    public interface IView
    {
        void Bind(IService service);
    }
}
