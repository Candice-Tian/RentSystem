using RentSystem.DI;
using RentSystem.Model;

namespace RentSystem.Repository
{
    public interface IAccountRepository : IDependency
    {
        public Account GetAccountById(int accountID);
        public bool PostAccount(Account account);
    }
    public class AccountRepository:IAccountRepository
    {
        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public Account GetAccountById(int accountID)
        {
            string sql = "Select * From Account Where AccountId =" + accountID;
            var result = DBContext.QueryFirstOrDefault<Account>(sql);
            return result;
        }

        /// <summary>
        /// 插入账户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool PostAccount(Account account)
        {
            string sql = string.Empty;
            int count = 0;
            if (GetAccountById(account.AccountID) != null)
            {
                sql = "update Account set Name=@Name,AccountId=@AccountId,Money=@Money,State=@State Where AccountId=@AccountId";
                count = DBContext.Execute(sql, account);
            }
            else
            {
                 sql = "insert into Account(Id,Name,AccountId,Money,State) Values(@Id,@Name, @AccountId,@Money,@State)";
                 count = DBContext.Execute(sql, account);
            }
            return count > 0 ? true : false;
        }



    }
}
