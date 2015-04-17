using ACE.Demo.Model.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ACE.Demo.API.Controllers
{
    public class ProjectController : ApiController
    {
        private IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [HttpPost]
        public Project Index(int id)
        {
            return _projectService.Get(id);
        }
    }
}
