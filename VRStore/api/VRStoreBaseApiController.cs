using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using VRStore.Models;

namespace VRStore.api
{
    public class VRStoreBaseApiController : ApiController
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        protected Guid UserID;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
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