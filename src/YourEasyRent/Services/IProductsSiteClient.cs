using YourEasyRent.Entities;
using System.Collections.Generic;
using Amazon.Auth.AccessControlPolicy;

namespace YourEasyRent.Services
{
    public interface IProductsSiteClient
    {
        /// <summary>
        /// Source
        /// </summary>
        Site Site { get; }

        /// <summary>
        /// Does not throw on nonexistant category
        /// </summary>
        /// <param name="section"></param>`1
        /// <param name="pageNumber"></param>
        /// <returns>List of products of a page, empty list of there is no products</returns>
        /// 
        Task<IEnumerable<Product>> FetchFromSectionAndPage(Section section, int pageNumber);
    }


    


}
