using Core.Extensions;
using Core.Utilities;

using ProjectTest.Constants;
using ProjectTest.Models;

namespace ProjectTest.Tests.DataProviders
{
    public class AccountDataProvider
    {
        private static Dictionary<string, AccountDataModel> AccountData = null!;

        public static Dictionary<string, AccountDataModel> LoadAccountDataFile()
        {
            if (AccountData == null || AccountData.Count == 0)
            {
                AccountData = JsonUtils.ReadAndParse<Dictionary<string, AccountDataModel>>(FileConstant.GetAccountDataFilePath.GetAbsolutePath());
            }

            return AccountData;
        }

        public static IEnumerable<AccountDataModel> GetValidAccount()
        {
            yield return LoadAccountDataFile()["user_1"];
        }
    }
}