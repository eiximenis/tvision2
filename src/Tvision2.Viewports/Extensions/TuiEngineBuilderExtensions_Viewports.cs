using Tvision2.Viewports;

namespace Tvision2.Core.Engine
{
    public static class TuiEngineBuilderExtensions_Viewports
    {

        public static TuiEngineBuilder UseViewportManager(this TuiEngineBuilder builder)
        {
            builder.SetCustomItem<IViewportManager>(new ViewportManager());

            builder.AfterCreateInvoke(engine =>
            {
                var viewportManager = engine.GetCustomItem<IViewportManager>() as ViewportManager;
                viewportManager.AttachTo(engine.UI);
            });
            return builder;
        }
    }
}
