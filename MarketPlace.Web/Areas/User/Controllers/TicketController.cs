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

        [HttpGet("tickets")]
        public async Task<IActionResult> Index(FilterTicketDTO filter)
        {
            filter.UserId = User.GetUserId();
            filter.FilterTicketState = FilterTicketState.NotDeleted;
            filter.OrderBy = FilterTicketOrder.CreateDate_DC;
            return View(await _contactService.FilterTickets(filter));
        }

        #endregion

        #region add ticket

        [HttpGet("add-Ticket")]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }

        [HttpPost("add-Ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketDTO ticket)
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

        #region show Ticket detalis

        [HttpGet("tickets/{ticketId}")]
        public async Task<IActionResult> TicketDetail(long ticketId)
        {
            var ticket = await _contactService.GetTicketForShow(ticketId, User.GetUserId());
            if (ticket == null) return NotFound();
            return View(ticket);
        }

        #endregion

        #region answer ticket

        [HttpPost("answer-ticket"),ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerTicket(AnswerTicketDTO answer)
        {
            if (string.IsNullOrEmpty(answer.Text))
            {
                TempData[ErrorMessage] = "لطفا متن را پر کنید";
            }

            if (ModelState.IsValid)
            {
                var res = await _contactService.AnswerTicket(answer, User.GetUserId());
                switch (res)
                {
                    case AnswerTicketResult.NotFound:
                        TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.NotFourUser:
                        TempData[ErrorMessage] = "عدم دسترسی";
                        TempData[InfoMessage] = "در صورت تکرار دسترسی شما قطع خواهد شد";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = "عملبات موفق بود";
                        break;
                }
            }

            return RedirectToAction("TicketDetail", "Ticket", new { area = "User", ticketId = answer.Id });
        }
        #endregion
    }
}
