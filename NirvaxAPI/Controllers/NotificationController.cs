﻿using AutoMapper;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpGet("user/{id}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> GetNotificationByUser(int id)
        {
            try
            {
                var noti = await _notificationRepository.GetNotificationsByUserAsync(id);
                return Ok(noti);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpGet("owner/{id}")]
        //[Authorize(Roles = "Owner,Staff")]
        public async Task<IActionResult> GetNotificationByOwner(int id)
        {
            try
            {
                var noti = await _notificationRepository.GetNotificationsByOwnerAsync(id);
                return Ok(noti);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "User,Owner,Staff")]
        public async Task<IActionResult> GetNotificationByid(int id)
        {
            try
            {
                var noti = await _notificationRepository.GetNotificationByidAsync(id);
                var url = await _notificationRepository.UpdateStatusNotificationAsync(noti);
                return Ok(url);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

    }
}
