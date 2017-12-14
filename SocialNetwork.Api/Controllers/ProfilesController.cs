﻿using System.Threading.Tasks;
using SocialNetwork.Api.Helpers;
using SocialNetwork.Data.Models;
using SocialNetwork.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProfilesController : SocialNetworkApiController
    {
        private readonly IProfileRepository profileRepository;
        private readonly IUserRepository userRepository;

        public ProfilesController(IProfileRepository profileRepository, IUserRepository userRepository)
        {
            this.profileRepository = profileRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string username, string password)
        {
            var user = await userRepository.GetAsync(username, HashHelper.Sha512(password + username));

            if (user == null)
            {
                return NotFound();
            }

            var profile = await profileRepository.GetForAsync(user);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(string username, string password, [FromBody]Profile profile)
        {
            var user = await userRepository.GetAsync(username, HashHelper.Sha512(password + username));

            if (user == null)
            {
                return NotFound();
            }

            var toUpdate = await profileRepository.GetForAsync(user);

            if (toUpdate == null)
            {
                return NotFound();
            }

            toUpdate.Bio = profile.Bio;

            await profileRepository.UpdateAsync(toUpdate);

            return Ok(toUpdate);
        }
    }
}