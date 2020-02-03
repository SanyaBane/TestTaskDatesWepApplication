using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTaskDatesCommon.Models;
using TestTaskDatesCommon.Payloads;
using TestTaskDatesWepApplication.Database;
using static TestTaskDatesWepApplication.Controllers.AccountController;

namespace TestTaskDatesWepApplication.Controllers
{
    [Authorize(Roles = "User")]
    public class DateController : ControllerBase
    {
        private ApplicationDBContext context;

        public DateController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [AllowAnonymous]
        [Route("api/[controller]/get/all")]
        [HttpGet]
        public ObjectResult GetDatesAll()
        {
            var ret = context.DateRanges;

            var result = new ObjectResult(ret);
            return result;
        }

        [Route("api/[controller]/insert_date_range")]
        [HttpPut]
        public ObjectResult InsertDateRange(DateRange dateRange)
        {
            context.DateRanges.Add(dateRange);
            context.SaveChanges();

            var response = new GeneralResponsePayload()
            {
                isSuccess = true,
            };

            var result = new ObjectResult(response);
            return result;
        }

        [Route("api/[controller]/get_date_range_intersect")]
        [HttpPost]
        public ObjectResult GetDateRangeIntersect(DateRange dateRange)
        {
            var ret = context.DateRanges.Where(x => (x.Start >= dateRange.Start && x.Start <= dateRange.End) || (x.End >= dateRange.Start && x.End <= dateRange.End));

            var result = new ObjectResult(ret);
            return result;
        }
    }
}