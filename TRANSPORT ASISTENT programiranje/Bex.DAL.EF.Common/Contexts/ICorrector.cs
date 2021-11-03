namespace Bex.DAL.EF.Common
{
    public interface ICorrector
    {
        bool IsCorrected(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry);
    }
}