﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Contact;
using MarketPlace.Web.PresentationExtensions;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class TicketController : UserBaseController
    {
        #region constructor
        private readonly IContactService _contactService;
        public TicketController(IContactService contactService)
        {
            _contactService = contactService;
        }
        #endregion

        #region list
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region add ticket
        [HttpGet("add-Ticket")]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }

        [HttpPost("add-Ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketViewModel ticket)
        {
            if (ModelState.IsValid)
            {
                var res = await _contactService.AddUserTicket(ticket, User.GetUserId());
                switch (res)
                {
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "عملیات با ارور مواجه شد";
                        break;
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما ثبت شد";
                        TempData[InfoMessage] = "جواب شما رو  زود میدیم";
                        //return RedirectToAction("AddTicket");
                        break;
                }
            }
            return View(ticket);
        }

        #endregion
    }
}
