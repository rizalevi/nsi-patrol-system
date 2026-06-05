using Microsoft.AspNetCore.Mvc;

namespace NSI.Application.Filters
{
    public class AuthorizeMenuAttribute : TypeFilterAttribute
    {
        public AuthorizeMenuAttribute() : base(typeof(AuthorizeMenuFilter))
        {
        }
    }
}
