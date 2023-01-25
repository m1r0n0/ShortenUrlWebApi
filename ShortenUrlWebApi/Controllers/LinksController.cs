using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShortenUrlWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LinksController : AppController
    {
        private readonly ApplicationContext _context;
        private readonly IShortenService _shortenService;
        private readonly IMapper _mapper;

        public LinksController(IHttpContextAccessor httpContextAccessor,
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

        //[Route("{id}/{state}")]
        [HttpPost]
        [Authorize]
        public IActionResult ChangeLinkPrivacy(int id, bool state)
        {
            string result = _shortenService.ChangePrivacy(id, state, GetUserIdFromClaims());
            switch (result)
            {
                case "Ok":
                    return Ok();
                case "Unauthorized":
                    return Unauthorized();
                case "NotFound":
                    return NotFound();//exeptions
            }
            return BadRequest();
        }
    }
}
