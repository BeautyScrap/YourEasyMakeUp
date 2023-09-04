using YourEasyRent.Entities;
using System.Collections.Generic;
using Amazon.Auth.AccessControlPolicy;

namespace YourEasyRent.Services
{
    public interface ISephoraProductSiteClient
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
        Task<IEnumerable<Product>> FetchFromSection(Section section, int pageNumber);
    }


    //Section Section { get; }


    //Task<IEnumerable<Product>> FetchFromSection(Section section, int pageNumber);


}
