using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace VoteApp.SharedCommon
{
    public static class CommonExtensions
    {
        public static void AddScopedAllByTypeRule(this IServiceCollection services, Assembly targetAssembly, Func<Type, bool> typeRule, Func<Type, bool>? interfaceTypeRule = null)
        {
            interfaceTypeRule ??= new Func<Type, bool>(i => true);

            targetAssembly
                .GetTypes()
                .Where(typeRule)
                .Select(t => new { implementType = t, interfaceType = t.GetInterfaces().First(interfaceTypeRule) })
                .ToList()
                .ForEach(item =>
                {
                    services.AddScoped(item.interfaceType, item.implementType);
                });
        }

        public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> method)
        {
            return await Task.WhenAll(source.Select(async s => await method(s)));
        }

        public static string ToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}
