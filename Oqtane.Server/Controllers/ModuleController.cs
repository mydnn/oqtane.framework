﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Repository;
using Oqtane.Models;

namespace Oqtane.Controllers
{
    [Route("{site}/api/[controller]")]
    public class ModuleController : Controller
    {
        private readonly IModuleRepository modules;
        private readonly IPageModuleRepository pagemodules;

        public ModuleController(IModuleRepository Modules, IPageModuleRepository PageModules)
        {
            modules = Modules;
            pagemodules = PageModules;
        }

        // GET: api/<controller>?pageid=x
        // GET: api/<controller>?siteid=x&moduledefinitionname=x
        [HttpGet]
        public IEnumerable<Module> Get(string pageid, string siteid, string moduledefinitionname)
        {
            if (!string.IsNullOrEmpty(pageid))
            {
                List<Module> modulelist = new List<Module>();
                foreach (PageModule pagemodule in pagemodules.GetPageModules(int.Parse(pageid)))
                {
                    Module module = pagemodule.Module;
                    module.PageModuleId = pagemodule.PageModuleId;
                    module.PageId = pagemodule.PageId;
                    module.Title = pagemodule.Title;
                    module.Pane = pagemodule.Pane;
                    module.Order = pagemodule.Order;
                    module.ContainerType = pagemodule.ContainerType;
                    modulelist.Add(module);
                }
                return modulelist;
            }
            else
            {
                return modules.GetModules(int.Parse(siteid), moduledefinitionname);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public Module Get(int id)
        {
            return modules.GetModule(id);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody] Module Module)
        {
            if (ModelState.IsValid)
                modules.AddModule(Module);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Module Module)
        {
            if (ModelState.IsValid)
                modules.UpdateModule(Module);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            modules.DeleteModule(id);
        }
    }
}
