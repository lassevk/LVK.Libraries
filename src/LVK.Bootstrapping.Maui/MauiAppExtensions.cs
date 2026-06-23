namespace LVK.Bootstrapping.Maui;

public static class MauiAppExtensions
{
    extension(MauiApp app)
    {
        public void Initialize()
        {
            var moduleInitializers = app.Services.GetServices<IModuleInitializer>().ToList();
            if (moduleInitializers.Any())
            {
                throw new InvalidOperationException("Async module initializers are not supported in Maui.");
            }

            var initializers = app.Services.GetServices<IMauiAppInitializer>().ToList();
            foreach (IMauiAppInitializer initializer in initializers)
            {
                initializer.Initialize(app);
            }
        }
    }
}