namespace Service
{
    public class APIContant
    {
        public const string GenerateTokenEndPoint = "Account/v1/GenerateToken";
        public const string AddBookToCollectionEndPoint = "BookStore/v1/Books";
        public const string DeleteBookFromCollectionEndPoint = "BookStore/v1/Book";
        public const string GetDetialUserEndPoint = "Account/v1/User/{0}";
        public const string ReplaceBookInCollectionEndPoint = "BookStore/v1/Books/{0}";
    }
}