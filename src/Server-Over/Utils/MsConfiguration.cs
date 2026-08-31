namespace Common;

public static class ConfigurationExtension
{
    public static T BindObj<T>(this IConfiguration configuration, string key, T instance, Action<IConfiguration, T>? fix) where T : class
    {
        var st = configuration.GetSection(key);
        st.Bind(instance);
        fix?.Invoke(st, instance);
        return instance;
    }

    public static T BindObj<T>(this IConfiguration configuration, T instance, Action<IConfiguration, T>? fix) where T : class
    {
        configuration.Bind(instance);
        fix?.Invoke(configuration, instance);
        return instance;
    }
}
