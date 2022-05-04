

using FluentAssertions;
using RentSystem.Model;
using RentSystem.Repository;
using Xunit;

namespace UnitTest.Repository
{

    public class AccountRepositoryTest
    {
        private AccountRepository testSub = new AccountRepository();
        
        
        [Fact]
        public void PostAccount_ShouldReturnTrue_WhenInsertDBSuccess()
        {
            var account = new Account() { AccountID = 3, ID = 3, Money = 300, Name = "Account1" };
            var result = testSub.PostAccount(account);

            Assert.True(result);
        }

        /// <summary>
        ///
        /// </summary>
        [Fact]
        public void PostAccount_ShouldReturnTrue_WhenUpdateMoney()
        {
            var account = new Account() { AccountID = 3, ID = 3, Money = 600, Name = "Account1" };
            testSub.PostAccount(account);
            account.Money = 300;

            var result = testSub.PostAccount(account);

            var actualDBAccount = testSub.GetAccountById(account.ID);
            Assert.True(result);
            Assert.Equal(actualDBAccount.Money, account.Money);

        }

    }
}
