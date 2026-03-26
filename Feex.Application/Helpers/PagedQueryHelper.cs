namespace Feex.Application.Helpers
{
    public static class PagedQueryHelper
    {
        public interface IPagedResult
        {
            int Offset { get; set; }
            int Limit { get; set; }
        }
    }
}
