using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using System.Linq;
using System;
using VRStore.Models;

namespace VRStore.Controllers
{
    public class VRStoreBaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        protected Guid UserID;


        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (User != null)
            {
                if (User.Identity is ClaimsIdentity claimsIdentity)
                {
                    // the principal identity is a claims identity.
                    // now we need to find the NameIdentifier claim
                    var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                    if (userIdClaim != null)
                    {
                        var userIdValue = userIdClaim.Value;
                        UserID = new Guid(userIdValue);
                        
                    }
                }
            }
        }

    }
}