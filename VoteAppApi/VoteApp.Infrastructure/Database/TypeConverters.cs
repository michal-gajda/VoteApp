using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VoteApp.Infrastructure.Database
{
    internal static class TypeConverters
    {
        public static PropertyBuilder<bool> HasConversionToInt(this PropertyBuilder<bool> prop)
        {
            return prop.HasConversion(boolValue => boolValue ? 1 : 0, intValue => intValue == 1);
        }

        public static PropertyBuilder<bool?> HasConversionToInt(this PropertyBuilder<bool?> prop)
        {
#pragma warning disable S3358 // Ternary operators should not be nested: the expression tree requires lambda
            return prop.HasConversion(boolValue => boolValue == false ? 0 : (boolValue == true ? 1 : default(int?))
             , intValue => intValue == 1 ? true : (intValue == 0 ? false : default(bool?)));
#pragma warning restore S3358 // Ternary operators should not be nested
        }
    }
}
