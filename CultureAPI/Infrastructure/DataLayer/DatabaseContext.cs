using CultureAPI.Domain.DataLayerAbstractions;
using Microsoft.Data.SqlClient;

namespace CultureAPI.Infrastructure.DataLayer
{
    public class DatabaseContext: IDatabaseContext
    {
        public SqlConnection SqlConnection { get; set; }
    }
}
