using Microsoft.Data.SqlClient;

namespace CultureAPI.Domain.DataLayerAbstractions
{
    public interface IDatabaseContext
    {
        SqlConnection SqlConnection { get; set; }
    }
}
