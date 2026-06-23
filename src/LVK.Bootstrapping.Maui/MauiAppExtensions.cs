namespace LVK.Bootstrapping.Maui;

public static class MauiAppExtensions
{
    extension(MauiApp app)
    {
        public async Task InitializeAsync()
        {
            var moduleInitializers = app.Services.GetServices<IModuleInitializer>().ToList();
            foreach (IModuleInitializer initializer in moduleInitializers)
            {
                await initializer.InitializeAsync(CancellationToken.None);
            }

            var initializers = app.Services.GetServices<IMauiAppInitializer>().ToList();
            foreach (IMauiAppInitializer initializer in initializers)
            {
                await initializer.InitializeAsync(app);
            }
        }
    }
}