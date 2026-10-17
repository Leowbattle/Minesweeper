using Microsoft.JSInterop;
using Microsoft.Xna.Framework;

namespace MinesweeperWeb.Pages
{
    public partial class Index
    {
        Game _game;

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                JsRuntime.InvokeAsync<object>("initRenderJS", DotNetObjectReference.Create(this));
            }
        }

        [JSInvokable]
        public void TickDotNet()
        {
            if (_game == null)
            {
                _game = new Minesweeper.Minesweeper();
                _game.Run();
            }

            _game.Tick();
        }
    }
}
