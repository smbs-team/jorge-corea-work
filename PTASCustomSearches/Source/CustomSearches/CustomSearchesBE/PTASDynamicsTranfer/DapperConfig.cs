using System;

namespace PTASDynamicsTranfer
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Dapper;

    static class DapperConfig
    {
        public static void ConfigureMapper(params Type[] types)
        {
            types.ToList().ForEach(tableType =>
            {
                SqlMapper.SetTypeMap(
                    tableType,
                    new CustomPropertyTypeMap(
                        tableType,
                        (type, columnName)
                            => type.GetProperties()
                                .FirstOrDefault(prop
                                => prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));
            });
        }
    }
}
