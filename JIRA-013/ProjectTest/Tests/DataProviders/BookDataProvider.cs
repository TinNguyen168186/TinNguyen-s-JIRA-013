using Core.Extensions;
using Core.Utilities;

using ProjectTest.Constants;
using ProjectTest.Models;

namespace ProjectTest.Tests.DataProviders
{
    public class BookDataProvider
    {
        private static Dictionary<string, BookDataModel> BookData = null!;

        public static Dictionary<string, BookDataModel> LoadBookDataFile()
        {
            if (BookData == null || BookData.Count == 0)
            {
                BookData = JsonUtils.ReadAndParse<Dictionary<string, BookDataModel>>(FileConstant.BookDataFilePath.GetAbsolutePath());
            }

            return BookData;
        }

        public static IEnumerable<BookDataModel> GetValidBookData()
        {
            yield return LoadBookDataFile()["BookValidData"];
        }

                public static IEnumerable<BookDataModel> GetValidBook2Data()
        {
            yield return LoadBookDataFile()["Book2ValidData"];
        }
    }
}