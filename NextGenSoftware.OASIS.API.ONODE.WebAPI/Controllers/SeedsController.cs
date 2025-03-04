﻿using Microsoft.AspNetCore.Mvc;
using System;
using EOSNewYork.EOSCore.Response.API;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.DNA.Manager;
using NextGenSoftware.OASIS.API.Providers.SEEDSOASIS;
using NextGenSoftware.OASIS.API.Providers.TelosOASIS;
using NextGenSoftware.OASIS.API.ONODE.WebAPI.Models;

namespace NextGenSoftware.OASIS.API.ONODE.WebAPI.Controllers
{
    [ApiController]
    [Route("api/seeds")]
    public class SeedsController : OASISControllerBase
    {
        SEEDSOASIS _SEEDSOASIS = null;

        SEEDSOASIS SEEDSOASIS
        {
            get
            {
                // TODO: Currently SEEDSOASIS is injected in with a TelosOASIS provider with a SEEDS connectionstring and will unregister the TelosOASIS Provider first if it is already registered so it uses the SEEDS connection string instead.
                // Not sure if we want SEEDSOASIS to use its own seperate private instance of the TelosOASIS provider using the SEEDS connection string allowing others to use the existing TelosOASIS Provider on the default Telos connectionstring?
                // If that is the case then uncomment the bottom line and comment the top line.
                if (_SEEDSOASIS == null)
                    _SEEDSOASIS = new SEEDSOASIS((TelosOASIS)OASISDNAManager.GetAndActivateProvider(ProviderType.TelosOASIS, OASISDNAManager.OASISDNA.OASIS.StorageProviders.SEEDSOASIS.ConnectionString, true));
                    //_SEEDSOASIS = new SEEDSOASIS(new TelosOASIS(OASISDNAManager.OASISDNA.OASIS.StorageProviders.SEEDSOASIS.ConnectionString));

                return _SEEDSOASIS;
            }
        }

        public SeedsController()
        {

        }

        /// <summary>
        /// Get's all of the SEEDS Organisations.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAllOrganisations")]
        public ActionResult<string> GetAllOrganisations()
        {
            return Ok(SEEDSOASIS.GetAllOrganisationsAsJSON());
        }

        /// <summary>
        /// Pay someone with seeds using their telos account and receive karma and the PayWithSeeds &amp; Hero gifts
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("PayWithSeedsUsingTelosAccount")]
        public ActionResult<string> PayWithSeedsUsingTelosAccount(PayWithSeedsUsingTelosAccountRequest request)
        {
            return Ok(SEEDSOASIS.PayWithSeedsUsingTelosAccount(request.FromTelosAccountName, request.FromTelosAccountPrivateKey, request.ToTelosAccountName, request.Quanitity, request.ReceivingKarmaFor, request.AppWebsiteServiceName, request.AppWebsiteServiceDesc, request.AppWebsiteServiceWebLink, request.Memo));
        }

        /// <summary>
        /// Pay someone with seeds using their avatar and receive karma and the PayWithSeeds &amp; Hero gifts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("PayWithSeedsUsingAvatar")]
        public ActionResult<string> PayWithSeedsUsingAvatar(PayWithSeedsUsingAvatarRequest request)
        {
            return Ok(SEEDSOASIS.PayWithSeedsUsingAvatar(request.FromAvatarId, request.ToAvatarId, request.Quanitity, request.ReceivingKarmaFor, request.AppWebsiteServiceName, request.AppWebsiteServiceDesc, request.AppWebsiteServiceWebLink, request.Memo));
        }

        /// <summary>
        /// Reward someone with seeds using their telos account and receive karma and the RewardWithSeeds &amp; Hero gifts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("RewardWithSeedsUsingTelosAccount")]
        public ActionResult<string> RewardWithSeedsUsingTelosAccount(PayWithSeedsUsingTelosAccountRequest request)
        {
            return Ok(SEEDSOASIS.RewardWithSeedsUsingTelosAccount(request.FromTelosAccountName, request.FromTelosAccountPrivateKey, request.ToTelosAccountName, request.Quanitity, request.ReceivingKarmaFor, request.AppWebsiteServiceName, request.AppWebsiteServiceDesc, request.AppWebsiteServiceWebLink, request.Memo));
        }

        /// <summary>
        /// Reward someone with seeds using their avatar and receive karma and the RewardWithSeeds &amp; Hero gifts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("RewardWithSeedsUsingAvatar")]
        public ActionResult<string> RewardWithSeedsUsingAvatar(PayWithSeedsUsingAvatarRequest request)
        {
            return Ok(SEEDSOASIS.PayWithSeedsUsingAvatar(request.FromAvatarId, request.ToAvatarId, request.Quanitity, request.ReceivingKarmaFor, request.AppWebsiteServiceName, request.AppWebsiteServiceDesc, request.AppWebsiteServiceWebLink, request.Memo));
        }

        /// <summary>
        /// Donate someone with seeds using their telos account and receive karma and the DonateWithSeeds &amp; SuperHero gifts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("DonateWithSeedsUsingTelosAccount")]
        public ActionResult<string> DonateWithSeedsUsingTelosAccount(PayWithSeedsUsingTelosAccountRequest request)
        {
            return Ok(SEEDSOASIS.RewardWithSeedsUsingTelosAccount(request.FromTelosAccountName, request.FromTelosAccountPrivateKey, request.ToTelosAccountName, request.Quanitity, request.ReceivingKarmaFor, request.AppWebsiteServiceName, request.AppWebsiteServiceDesc, request.AppWebsiteServiceWebLink, request.Memo));
        }

        /// <summary>
        /// Donate someone with seeds using their avatar and receive karma and the DonateWithSeeds &amp; SuperHero gifts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("DonateWithSeedssUsingAvatar")]
        public ActionResult<string> DonateWithSeedssUsingAvatar(PayWithSeedsUsingAvatarRequest request)
        {
            return Ok(SEEDSOASIS.PayWithSeedsUsingAvatar(request.FromAvatarId, request.ToAvatarId, request.Quanitity, request.ReceivingKarmaFor, request.AppWebsiteServiceName, request.AppWebsiteServiceDesc, request.AppWebsiteServiceWebLink, request.Memo));
        }

        /// <summary>
        /// Send an invite to someone to join Seeds using their telos account and then receive karma and the RewardWithSeeds &amp; Hero gifts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("SendInviteToJoinSeedsUsingTelosAccount")]
        public ActionResult<string> SendInviteToJoinSeedsUsingTelosAccount(SendInviteToJoinSeedsUsingTelosAccountRequest request)
        {
            return Ok(SEEDSOASIS.SendInviteToJoinSeedsUsingTelosAccount(request.SponsorTelosAccountName, request.SponsorTelosAccountPrivateKey, request.RefererTelosAccountName, request.TransferQuanitity, request.SowQuanitity, request.ReceivingKarmaFor, request.AppWebsiteServiceName, request.AppWebsiteServiceDesc, request.AppWebsiteServiceWebLink));
        }

        /// <summary>
        /// Send an invite to someone to join Seeds using avatar and then receive karma and the SendInviteToJoinSeeds &amp; Hero gifts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("SendInviteToJoinSeedsUsingAvatar")]
        public ActionResult<string> SendInviteToJoinSeedsUsingAvatar(SendInviteToJoinSeedsUsingAvatarRequest request)
        {
            return Ok(SEEDSOASIS.SendInviteToJoinSeedsUsingAvatar(request.SponsorAvatarId, request.RefererAvatarId, request.TransferQuanitity, request.SowQuanitity, request.ReceivingKarmaFor, request.AppWebsiteServiceName, request.AppWebsiteServiceDesc, request.AppWebsiteServiceWebLink));
        }

        /// <summary>
        /// Accept an invite to join Seeds using their telos account and then receive karma and the AcceptInviteToJoinSeeds &amp; Hero gifts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AcceptInviteToJoinSeedsUsingTelosAccount")]
        public ActionResult<string> AcceptInviteToJoinSeedsUsingTelosAccount(AcceptInviteToJoinSeedsUsingTelosAccountRequest request)
        {
            return Ok(SEEDSOASIS.AcceptInviteToJoinSeedsUsingTelosAccount(request.TelosAccountName, request.InviteSecret, request.ReceivingKarmaFor, request.AppWebsiteServiceName, request.AppWebsiteServiceDesc, request.AppWebsiteServiceWebLink));
        }

        /// <summary>
        /// Accept an invite to join Seeds using their avatar and then receive karma and the AcceptInviteToJoinSeeds &amp; Hero gifts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AcceptInviteToJoinSeedsUsingAvatar")]
        public ActionResult<string> AcceptInviteToJoinSeedsUsingAvatar(AcceptInviteToJoinSeedsUsingAvatarRequest request)
        {
            return Ok(SEEDSOASIS.AcceptInviteToJoinSeedsUsingAvatar(request.AvatarId, request.InviteSecret, request.ReceivingKarmaFor, request.AppWebsiteServiceName, request.AppWebsiteServiceDesc, request.AppWebsiteServiceWebLink));
        }

        /// <summary>
        /// Get's the Telos account name for the given Avatar.
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetTelosAccountNameForAvatar")]
        public ActionResult<string> GetTelosAccountNameForAvatar(Guid avatarId)
        {
            return Ok(SEEDSOASIS.TelosOASIS.GetTelosAccountNameForAvatar(avatarId));
        }

        /// <summary>
        /// Get's the Telos account name for the given Avatar.
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetTelosAccountPrivateKeyForAvatar")]
        public ActionResult<string> GetTelosAccountPrivateKeyForAvatar(Guid avatarId)
        {
            return Ok(SEEDSOASIS.TelosOASIS.GetTelosAccountPrivateKeyForAvatar(avatarId));
        }

        /// <summary>
        /// Get's the Telos account.
        /// </summary>
        /// <param name="telosAccountName"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetTelosAccount")]
        public ActionResult<Account> GetTelosAccount(string telosAccountName)
        {
            return Ok(SEEDSOASIS.TelosOASIS.GetTelosAccount(telosAccountName));
        }

        /// <summary>
        /// Get's the Telos account for the given Avatar.
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetTelosAccountForAvatar")]
        public ActionResult<Account> GetTelosAccountForAvatar(Guid avatarId)
        {
            return Ok(SEEDSOASIS.TelosOASIS.GetTelosAccountForAvatar(avatarId));
        }

        /// <summary>
        /// Get's the Avatar id for the the given Telos account name.
        /// </summary>
        /// <param name="telosAccountName"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAvatarIdForTelosAccountName")]
        public ActionResult<string> GetAvatarIdForTelosAccountName(string telosAccountName)
        {
            return Ok(SEEDSOASIS.TelosOASIS.GetAvatarIdForTelosAccountName(telosAccountName));
        }

        /// <summary>
        /// Get's the Avatar for the the given Telos account name.
        /// </summary>
        /// <param name="telosAccountName"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAvatarForTelosAccountName")]
        public ActionResult<string> GetAvatarForTelosAccountName(string telosAccountName)
        {
            return Ok(SEEDSOASIS.TelosOASIS.GetAvatarForTelosAccountName(telosAccountName));
        }

        /// <summary>
        /// Get's the SEEDS balance for the given Telos account.
        /// </summary>
        /// <param name="telosAccountName"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetBalanceForTelosAccount")]
        public ActionResult<string> GetBalanceForTelosAccount(string telosAccountName)
        {
            return Ok(SEEDSOASIS.GetBalanceForTelosAccount(telosAccountName));
        }

        /// <summary>
        /// Get's the SEEDS balance for the given avatar.
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetBalanceForAvatar")]
        public ActionResult<string> GetBalanceForAvatar(Guid avatarId)
        {
            return Ok(SEEDSOASIS.GetBalanceForAvatar(avatarId));
        }

        /// <summary>
        /// Generates a QR Code for logging into your Seeds Passport.
        /// </summary>
        /// <param name="telosAccountName"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GenerateSeedsPassportSignInQRCode")]
        public ActionResult<string> GenerateSeedsPassportSignInQRCode(string telosAccountName)
        {
            return Ok(SEEDSOASIS.GenerateSignInQRCode(telosAccountName));
        }

        /// <summary>
        /// Generates a QR Code for logging into your Seeds Passport.
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GenerateSeedsPassportSignInQRCodeForAvatar")]
        public ActionResult<string> GenerateSeedsPassportSignInQRCodeForAvatar(Guid avatarId)
        {
            return Ok(SEEDSOASIS.GenerateSignInQRCodeForAvatar(avatarId));
        }

        ///// <summary>
        ///// Generates a QR Code for logging into your Seeds Passport.
        ///// </summary>
        ///// <param name="avatarId"></param>
        ///// <param name="seedsKarmaType"></param>
        ///// <param name="receivingKarmaFor"></param>
        ///// <param name="appWebsiteServiceName"></param>
        ///// <param name="appWebsiteServiceDesc"></param>
        ///// <returns></returns>
        //[Authorize]
        //[HttpPost("AddKarmaForSeeds")]
        //public ActionResult<bool> AddKarmaForSeeds(Guid avatarId, KarmaTypePositive seedsKarmaType, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc)
        //{
        //    return Ok(SEEDSOASIS.AddKarmaForSeeds(avatarId, seedsKarmaType, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc));
        //}
    }
}
