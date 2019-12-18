using System;
using System.Security.Cryptography;

namespace Numaka.Common
{
    internal enum GuidTypes
    {
        Unknown = 0,
        SqlServer = 1,
        Oracle = 2,
        MySql = 3,
        Postgres = 4
    }

    public static class GuidExtensions
    {
        public static Guid NewSqlServerGuid(this Guid guid) => NewGuid(GuidTypes.SqlServer);

        public static Guid NewOracleGuid(this Guid guid) => NewGuid(GuidTypes.Oracle);

        public static Guid NewMySqlGuid(this Guid guid) => NewGuid(GuidTypes.MySql);

        public static Guid NewPostgresGuid(this Guid guid) => NewGuid(GuidTypes.Postgres);

        private static Guid NewGuid(GuidTypes guidType)
        {
            try
            {
                using (var random = RandomNumberGenerator.Create())
                {
                    var randomBytes = new byte[10];

                    random.GetBytes(randomBytes);

                    var timestamp = DateTime.UtcNow.Ticks / 10000L;
                    var timestampBytes = BitConverter.GetBytes(timestamp);

                    if(BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(timestampBytes);
                    }

                    var bytes = new byte[16];

                    switch(guidType)
                    {
                        case GuidTypes.Oracle:
                            Buffer.BlockCopy(timestampBytes, 2, bytes, 0, 6);
                            Buffer.BlockCopy(randomBytes, 0, bytes, 6, 10);
                            break;

                        case GuidTypes.Postgres:
                        case GuidTypes.MySql:
                            Buffer.BlockCopy(timestampBytes, 2, bytes, 0, 6);
                            Buffer.BlockCopy(randomBytes, 0, bytes, 6, 10);

                            if(BitConverter.IsLittleEndian)
                            {
                                Array.Reverse(bytes, 0, 4);
                                Array.Reverse(bytes, 4, 2);
                            }
                            break;

                        case GuidTypes.SqlServer:
                            Buffer.BlockCopy(randomBytes, 0, bytes, 0, 10);
                            Buffer.BlockCopy(timestampBytes, 2, bytes, 10, 6);
                            break;

                        default:
                            return Guid.Empty;
                    }

                    return new Guid(bytes);
                }
            }
            catch
            {
                return Guid.Empty;
            }
        }
    }
}