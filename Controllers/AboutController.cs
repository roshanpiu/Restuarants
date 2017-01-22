using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{

    /************Attribute bases routing demo***********************/

    //[Route("[controller]")] //uses the conteoller name to route
    //[Route("about")] //we have to use about to get to this controller
    [Route("company/[controller]/[action]")]
    public class AboutController
    {

        // [Route("")]
        public string Phone()
        {
            return "0712527388";
        }

        // [Route("country")]
        // [Route("[action]")]
        public string Country()
        {
            return "Sri Lanka";
        }
    }
}