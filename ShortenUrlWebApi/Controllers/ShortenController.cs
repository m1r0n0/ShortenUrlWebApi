using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Data;
using BusinessLayer.Enums;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Exceptions;

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
        public ActionResult<UrlListDTO> GetAllLinks()
        {
            UrlListDTO linkDTO = _shortenService.GetURLs();
            return linkDTO;
        }

        [HttpGet]
        public ActionResult<UrlListDTO> GetLinksForCurrentUser()
        {
            LinkDTO linkDTO = _shortenService.GetURLsForCurrentUser(GetUserIdFromClaims());          
            UrlListDTO urlListDTO = _mapper.Map<UrlListDTO>(linkDTO);
            return urlListDTO;
        }

        [HttpPut]
        public async Task<ActionResult<Url>> CreateLink(Url url)
        {
            LinkDTO linkDTO = _mapper.Map<LinkDTO>(url);
            linkDTO = await _shortenService.CreateShortLinkFromFullUrl(linkDTO, GetUserIdFromClaims());
            return Ok(linkDTO);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ChangeLinkPrivacy(int id, bool state)
        {
            try
            {
                _shortenService.ChangePrivacy(id, state, GetUserIdFromClaims());
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
    }
}
