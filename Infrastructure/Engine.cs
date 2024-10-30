using Infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;

namespace Infrastructure
{
    public static class Engine
    {
        private static IServiceProvider _serviceProvider;
        private static IServiceScope _currentScope;

        /// <summary>
        /// Configura o Engine com o ServiceProvider fornecido.
        /// </summary>
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Cria um novo escopo para resolver serviços scoped.
        /// </summary>
        public static void CreateScope() => _currentScope = _serviceProvider?.CreateScope();

        /// <summary>
        /// Destrói o escopo atual, liberando recursos.
        /// </summary>
        public static void DisposeScope()
        {
            _currentScope?.Dispose();
            _currentScope = null;
        }

        /// <summary>
        /// Resolve uma entidade do banco de dados.
        /// </summary>
        public static T Table<T>() where T : class
        {
            return GetRequiredService<T>();
        }

        /// <summary>
        /// Resolve um serviço genérico com escopo.
        /// </summary>
        public static T Service<T>() where T : class
        {
            return GetScopedService<T>();
        }

        /// <summary>
        /// Resolve um recurso de tradução ou configuração com uma chave específica.
        /// </summary>
        public static string Translate(string key)
        {
            return ResourceHelper.GetStringAsync(key).Result;
        }

        private static T GetRequiredService<T>() where T : class
        {
            return _currentScope?.ServiceProvider.GetRequiredService<T>() ?? _serviceProvider.GetRequiredService<T>();
        }

        private static T GetScopedService<T>() where T : class
        {
            return _currentScope?.ServiceProvider.GetService<T>();
        }

        /// <summary>
        /// Reseta o Engine e o ServiceProvider.
        /// </summary>
        public static void Reset()
        {
            _serviceProvider = null;
            DisposeScope();
        }
    }
}
