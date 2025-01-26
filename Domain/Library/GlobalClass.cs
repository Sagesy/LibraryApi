using Microsoft.AspNetCore.Hosting;

namespace LibraryApi.Domain.Library
{
    public class GlobalClass
    {

        private static IWebHostEnvironment _HostingEnvironment;

        public static void Configure(IWebHostEnvironment HostingEnvironment)
        {
            _HostingEnvironment = HostingEnvironment;
        }
        public static string GetRootPath => _HostingEnvironment.ContentRootPath;

    }
}