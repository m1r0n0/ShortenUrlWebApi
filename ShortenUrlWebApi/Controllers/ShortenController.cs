﻿using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ShortenUrlWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShortenController : AppController
    {
        private readonly ApplicationContext _context;
        private readonly IShortenService _shortenService;
        private readonly IMapper _mapper;

        public ShortenController(IHttpContextAccessor httpContextAccessor,
            ApplicationContext context,
            IShortenService shortenService,
            IMapper mapper)
            : base(httpContextAccessor)
        {
            _context = context;
            _shortenService = shortenService;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<UrlListDTO> GetLinksForCurrentUser(string userID)
        {
            UrlListDTO linkDTO = await _shortenService.GetURLsForCurrentUser(userID);
            return linkDTO;
        }

        [HttpPut]
        public async Task<ActionResult<Url>> CreateLink(Url url)
        {
            var linkDTO = _mapper.Map<LinkDTO>(url);
            linkDTO = await _shortenService.CreateShortLinkFromFullUrl(linkDTO);
            return Ok(linkDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangeLinkPrivacy(string shortUrl, bool state, string userID)
        {
            try
            {
                await _shortenService.ChangePrivacy(shortUrl, state, userID);
                return Ok();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLink(Url url)
        {
            try
            {
                Url link = await _shortenService.DeleteLink(url);
                return Ok(link);
            }
            catch (NotFoundException)
            {
                return NotFound(url);
            }
            catch (UnauthorizedException)
            {
                return Unauthorized(url);
            }
        }
    }
}
