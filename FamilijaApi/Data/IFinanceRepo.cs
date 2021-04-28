using FamilijaApi.Models;
using System.Threading.Tasks;

namespace FamilijaApi.Data
{
    public interface IFinanceRepo
    {
        Task<Finance> GetFinance(int id);
        void CreateFinance(Finance finance);
        Task<bool> SaveChanges();
        void UpdateFinance(Finance updateModelFinance);
        void DeleteUser(Finance deleteModelFinance);
    }
}
