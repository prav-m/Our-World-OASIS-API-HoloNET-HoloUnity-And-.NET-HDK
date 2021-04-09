﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EOSNewYork.EOSCore.ActionArgs;
using EOSNewYork.EOSCore.Response.API;
using EOSNewYork.EOSCore.Utilities;
using NextGenSoftware.OASIS.API.Core;
using NextGenSoftware.OASIS.API.Core.Managers;
using NextGenSoftware.OASIS.API.Core.Interfaces;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Providers.SEEDSOASIS.ParamObjects;
using NextGenSoftware.OASIS.API.Core.Helpers;

namespace NextGenSoftware.OASIS.API.Providers.SEEDSOASIS
{
    public class SEEDSOASIS : OASISProvider, IOASISApplicationProvider
    {
        public const string ENDPOINT_TEST = "https://test.hypha.earth";
        public const string ENDPOINT_LIVE = "https://node.hypha.earth";
        public const string SEEDS_EOSIO_ACCOUNT_TEST = "seeds";
        public const string SEEDS_EOSIO_ACCOUNT_LIVE = "seed.seeds";
        public const string CHAINID_TEST = "1eaa0824707c8c16bd25145493bf062aecddfeb56c736f6ba6397f3195f33c9f";
        public const string CHAINID_LIVE = "4667b205c6838ef70ff7988f6e8257e8be0e1284a2f59699054a018f743b1d11";
        public const string PUBLICKEY_TEST = "EOS8MHrY9xo9HZP4LvZcWEpzMVv1cqSLxiN2QMVNy8naSi1xWZH29";
        public const string PUBLICKEY_TEST2 = "EOS8C9tXuPMkmB6EA7vDgGtzA99k1BN6UxjkGisC1QKpQ6YV7MFqm";
        public const string PUBLICKEY_LIVE = "EOS6kp3dm9Ug5D3LddB8kCMqmHg2gxKpmRvTNJ6bDFPiop93sGyLR";
        public const string PUBLICKEY_LIVE2 = "EOS6kp3dm9Ug5D3LddB8kCMqmHg2gxKpmRvTNJ6bDFPiop93sGyLR";
        public const string APIKEY_TEST = "EOS7YXUpe1EyMAqmuFWUheuMaJoVuY3qTD33WN4TrXbEt8xSKrdH9";
        public const string APIKEY_LIVE = "EOS7YXUpe1EyMAqmuFWUheuMaJoVuY3qTD33WN4TrXbEt8xSKrdH9";

        // Lookup Cache. TODO: Move to generic CacheManager in OASIS.API.Core, maybe also in ProviderManager so other providers can also share the cache.
        //private static Dictionary<Guid, string> _avatarIdToTelosAccountNameLookup = new Dictionary<Guid, string>();
        //private static Dictionary<Guid, Account> _avatarIdToTelosAccountLookup = new Dictionary<Guid, Account>();
        //private static Dictionary<string, Guid> _telosAccountNameToAvatarIdLookup = new Dictionary<string, Guid>();
        //private static Dictionary<string, IAvatar> _telosAccountNameToAvatarLookup = new Dictionary<string, IAvatar>();

        private static Random _random = new Random();
        private AvatarManager _avatarManager = null;
      //  private EOSIOOASIS.EOSIOOASIS EOSIOOASIS = null;

        public EOSIOOASIS.EOSIOOASIS EOSIOOASIS { get; }


        //TODO: Not sure if this should share the EOSOASIS AvatarManagerInstance? May be better to have seperate?
        private AvatarManager AvatarManagerInstance
        {
            get
            {
                if (_avatarManager == null)
                {
                    if (EOSIOOASIS != null)
                        _avatarManager = new AvatarManager(ProviderManager.GetStorageProvider(Core.Enums.ProviderType.MongoDBOASIS));
                        //_avatarManager = new AvatarManager(this); // TODO: URGENT: PUT THIS BACK IN ASAP! TEMP USING MONGO UNTIL EOSIO METHODS IMPLEMENTED...

                    else
                    {
                        if (!ProviderManager.IsProviderRegistered(Core.Enums.ProviderType.EOSIOOASIS))
                            throw new Exception("EOSIOOASIS Provider Not Registered. Please register and try again.");
                        else
                            throw new Exception("EOSIOOASIS Provider Is Registered But Was Not Injected Into SEEDSOASIS Provider.");
                    }
                }

                return _avatarManager;
            }
        }

        public SEEDSOASIS(EOSIOOASIS.EOSIOOASIS eosioOaisis)
        {
            this.ProviderName = "SEEDSOASIS";
            this.ProviderDescription = "SEEDS Provider";
            this.ProviderType = new EnumValue<ProviderType>(Core.Enums.ProviderType.SEEDSOASIS);
            this.ProviderCategory = new EnumValue<ProviderCategory>(Core.Enums.ProviderCategory.Application);
            EOSIOOASIS = eosioOaisis;
        }

        //public async Task<Account> GetTelosAccountAsync(string telosAccountName)
        //{
        //    var account = await EOSIOOASIS.ChainAPI.GetAccountAsync(telosAccountName);
        //    return account;
        //}

        //public Account GetTelosAccount(string telosAccountName)
        //{
        //    var account = EOSIOOASIS.ChainAPI.GetAccount(telosAccountName);
        //    return account;
        //}

        public async Task<string> GetBalanceAsync(string telosAccountName)
        {
            //var currencyBalance = await EOSIOOASIS.ChainAPI.GetCurrencyBalanceAsync(telosAccountName, "seeds.seeds", "SEEDS");
            //var currencyBalance = await EOSIOOASIS.ChainAPI.GetCurrencyBalanceAsync(telosAccountName, "token.seeds", "SEEDS");
            //return currencyBalance.balances[0];

            return await EOSIOOASIS.GetBalanceAsync(telosAccountName, "token.seeds", "SEEDS");
        }

        public string GetBalanceForTelosAccount(string telosAccountName)
        {
            //https://github.com/JoinSEEDS/seeds-smart-contracts/blob/master/scripts/balancecheck.js
            //eos.getCurrencyBalance("token.seeds", account, 'SEEDS')

            // var currencyBalance = EOSIOOASIS.ChainAPI.GetCurrencyBalance(telosAccountName, "token.seeds", "SEEDS");
            //return currencyBalance.balances[0];

            return EOSIOOASIS.GetBalanceForEOSIOAccount(telosAccountName, "token.seeds", "SEEDS");
        }

        public string GetBalanceForAvatar(Guid avatarId)
        {
            //return GetBalanceForTelosAccount(GetTelosAccountNameForAvatar(avatarId));
            return EOSIOOASIS.GetBalanceForAvatar(avatarId, "token.seeds", "SEEDS");
        }


        // TODO: URGENT - NEED TO MOVE THESE TO THE TELOSOASIS PROVIDER (EOSOASIS is a different chain so the account names will be different so cant use the EOS methods as SEEDSOASIS currently does).
        // Need to make these cache lookups generic for all OASIS Providers, sort this ASAP! ;-)

        //public string GetTelosAccountNameForAvatar(Guid avatarId)
        //{
        //    if (!_avatarIdToTelosAccountNameLookup.ContainsKey(avatarId))
        //        _avatarIdToTelosAccountNameLookup[avatarId] = AvatarManagerInstance.LoadAvatar(avatarId).ProviderKey[Core.Enums.ProviderType.TelosOASIS];

        //    return _avatarIdToTelosAccountNameLookup[avatarId];
        //}

        //public Account GetTelosAccountForAvatar(Guid avatarId)
        //{
        //    if (!_avatarIdToTelosAccountLookup.ContainsKey(avatarId))
        //        _avatarIdToTelosAccountLookup[avatarId] = GetTelosAccount(GetTelosAccountNameForAvatar(avatarId));

        //    return _avatarIdToTelosAccountLookup[avatarId];
        //}

        //public Guid GetAvatarIdForTelosAccountName(string telosAccountName)
        //{
        //    if (!_telosAccountNameToAvatarIdLookup.ContainsKey(telosAccountName))
        //        _telosAccountNameToAvatarIdLookup[telosAccountName] = AvatarManagerInstance.LoadAllAvatars().FirstOrDefault(x => x.ProviderKey[Core.Enums.ProviderType.TelosOASIS] == telosAccountName).Id;

        //    return _telosAccountNameToAvatarIdLookup[telosAccountName];
        //}

        //public IAvatar GetAvatarForTelosAccountName(string telosAccountName)
        //{
        //    if (!_telosAccountNameToAvatarLookup.ContainsKey(telosAccountName))
        //        _telosAccountNameToAvatarLookup[telosAccountName] = AvatarManagerInstance.LoadAllAvatars().FirstOrDefault(x => x.ProviderKey[Core.Enums.ProviderType.TelosOASIS] == telosAccountName);

        //    return _telosAccountNameToAvatarLookup[telosAccountName];
        //}


        public async Task<TableRows> GetAllOrganisationsAsync()
        {
            TableRows rows = await EOSIOOASIS.ChainAPI.GetTableRowsAsync("orgs.seeds", "orgs.seeds", "organization", "true", 0, -1, 99999);
            return rows;
        }


        public TableRows GetAllOrganisations()
        {
            TableRows rows = EOSIOOASIS.ChainAPI.GetTableRows("orgs.seeds", "orgs.seeds", "organization", "true", 0, -1, 99999);
            return rows;
        }

        public async Task<string> GetAllOrganisationsAsJSONAsync()
        {
            TableRows rows = await EOSIOOASIS.ChainAPI.GetTableRowsAsync("orgs.seeds", "orgs.seeds", "organization", "true", 0, -1, 99999);
            return JsonConvert.SerializeObject(rows);
        }

        public string GetAllOrganisationsAsJSON()
        {
            TableRows rows = EOSIOOASIS.ChainAPI.GetTableRows("orgs.seeds", "orgs.seeds", "organization", "true", 0, -1, 99999);
            return JsonConvert.SerializeObject(rows);
        }

        public OASISResult<string> PayWithSeedsUsingTelosAccount(string fromTelosAccountName, string toTelosAccountName, float quanitity, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null, string memo = null)
        {
            return PayWithSeeds(fromTelosAccountName, toTelosAccountName, quanitity, KarmaTypePositive.PayWithSeeds, KarmaTypePositive.BeAHero, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink, memo);
        }

        public OASISResult<string> PayWithSeedsUsingAvatar(Guid fromAvatarId, Guid toAvatarId, float quanitity, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null, string memo = null)
        {
            return PayWithSeedsUsingTelosAccount(EOSIOOASIS.GetEOSIOAccountNameForAvatar(fromAvatarId), EOSIOOASIS.GetEOSIOAccountNameForAvatar(toAvatarId), quanitity, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink, memo);
        }

        public OASISResult<string> DonateWithSeedsUsingTelosAccount(string fromTelosAccountName, string toTelosAccountName, float quanitity, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null, string memo = null)
        {
            return PayWithSeeds(fromTelosAccountName, toTelosAccountName, quanitity, KarmaTypePositive.DonateWithSeeds, KarmaTypePositive.BeASuperHero, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink, memo);
        }

        public OASISResult<string> DonateWithSeedsUsingAvatar(Guid fromAvatarId, Guid toAvatarId, float quanitity, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null, string memo = null)
        {
            return DonateWithSeedsUsingTelosAccount(EOSIOOASIS.GetEOSIOAccountNameForAvatar(fromAvatarId), EOSIOOASIS.GetEOSIOAccountNameForAvatar(toAvatarId), quanitity, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink, memo);
        }

        public OASISResult<string> RewardWithSeedsUsingTelosAccount(string fromTelosAccountName, string toTelosAccountName, float quanitity, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null, string memo = null)
        {
            return PayWithSeeds(fromTelosAccountName, toTelosAccountName, quanitity, KarmaTypePositive.RewardWithSeeds, KarmaTypePositive.BeASuperHero, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink, memo);
        }

        public OASISResult<string> RewardWithSeedsUsingAvatar(Guid fromAvatarId, Guid toAvatarId, float quanitity, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null, string memo = null)
        {
            return RewardWithSeedsUsingTelosAccount(EOSIOOASIS.GetEOSIOAccountNameForAvatar(fromAvatarId), EOSIOOASIS.GetEOSIOAccountNameForAvatar(toAvatarId), quanitity, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink, memo);
        }

        public OASISResult<SendInviteResult> SendInviteToJoinSeedsUsingTelosAccount(string sponsorTelosAccountName, string referrerTelosAccountName, float transferQuantitiy, float sowQuantitiy, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null)
        {
            OASISResult<SendInviteResult> result = new OASISResult<SendInviteResult>();

            try
            {
                result.Result = SendInviteToJoinSeeds(sponsorTelosAccountName, referrerTelosAccountName, transferQuantitiy, sowQuantitiy);
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.ErrorMessage = string.Concat("Error occured pushing the transaction onto the EOSIO chain. Error Message: ", ex.ToString());
            }

            // If there was no error then now add the karma.
            if (!result.IsError && !string.IsNullOrEmpty(result.Result.TransactionId))
                AddKarmaForSeeds(EOSIOOASIS.GetAvatarIdForEOSIOAccountName(sponsorTelosAccountName), KarmaTypePositive.SendInviteToJoinSeeds, KarmaTypePositive.BeAHero, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink);
            else
            {
                if (!result.IsError)
                {
                    result.IsError = true;
                    result.ErrorMessage = "Unknown error occured pushing the transaction onto the EOSIO chain.";
                }
            }

            return result;
        }

        public OASISResult<SendInviteResult> SendInviteToJoinSeedsUsingAvatar(Guid sponsorAvatarId, Guid referrerAvatarId, float transferQuantitiy, float sowQuantitiy, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null)
        {
            return SendInviteToJoinSeedsUsingTelosAccount(EOSIOOASIS.GetEOSIOAccountNameForAvatar(sponsorAvatarId), EOSIOOASIS.GetEOSIOAccountNameForAvatar(referrerAvatarId), transferQuantitiy, sowQuantitiy, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink);
        }

        public OASISResult<string> AcceptInviteToJoinSeedsUsingTelosAccount(string telosAccountName, string inviteSecret, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null)
        {
            OASISResult<string> result = new OASISResult<string>();

            try
            {
                result.Result = AcceptInviteToJoinSeeds(telosAccountName, inviteSecret);
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.ErrorMessage = string.Concat("Error occured pushing the transaction onto the EOSIO chain. Error Message: ", ex.ToString());
            }

            // If there was no error then now add the karma.
            if (!result.IsError && !string.IsNullOrEmpty(result.Result))
                AddKarmaForSeeds(EOSIOOASIS.GetAvatarIdForEOSIOAccountName(telosAccountName), KarmaTypePositive.AcceptInviteToJoinSeeds, KarmaTypePositive.BeAHero, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink);
            else
            {
                if (!result.IsError)
                {
                    result.IsError = true;
                    result.ErrorMessage = "Unknown error occured pushing the transaction onto the EOSIO chain.";
                }
            }

            return result;
        }

        public OASISResult<string> AcceptInviteToJoinSeedsUsingAvatar(Guid avatarId, string inviteSecret, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null)
        {
            return AcceptInviteToJoinSeedsUsingTelosAccount(EOSIOOASIS.GetEOSIOAccountNameForAvatar(avatarId), inviteSecret, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink);
        }

        public string GenerateSignInQRCode(string telosAccountName)
        {
            //https://github.com/JoinSEEDS/encode-transaction-service/blob/master/buildTransaction.js
            return "";
        }

        public string GenerateSignInQRCodeForAvatar(Guid avatarId)
        {
            return GenerateSignInQRCode(EOSIOOASIS.GetEOSIOAccountNameForAvatar(avatarId));
        }

        private OASISResult<string> PayWithSeeds(string fromTelosAccountName, string toTelosAccountName, float quanitity, KarmaTypePositive seedsKarmaType, KarmaTypePositive seedsKarmaHeroType, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null, string memo = null)
        {
            // TODO: Make generic and apply to all other calls...
            OASISResult<string> result = new OASISResult<string>();

            try
            {
                result.Result = PayWithSeeds(fromTelosAccountName, toTelosAccountName, quanitity, memo);
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.ErrorMessage = string.Concat("Error occured pushing the transaction onto the EOSIO chain. Error Message: ", ex.ToString());
            }

            // If there was no error then now add the karma.
            if (!result.IsError && !string.IsNullOrEmpty(result.Result))
                AddKarmaForSeeds(EOSIOOASIS.GetAvatarIdForEOSIOAccountName(fromTelosAccountName), seedsKarmaType, seedsKarmaHeroType, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink);
            else
            {
                if (!result.IsError)
                {
                    result.IsError = true;
                    result.ErrorMessage = "Unknown error occured pushing the transaction onto the EOSIO chain.";
                }
            }

            return result;
        }

        private string PayWithSeeds(string fromTelosAccountName, string toTelosAccountName, float quanitity, string memo)
        {
            return PayWithSeeds(fromTelosAccountName, toTelosAccountName, string.Concat(quanitity, " SEEDS"), memo);
        }

        private string PayWithSeeds(string fromTelosAccountName, string toTelosAccountName, string quanitity, string memo)
        {
            //Use standard TELOS/EOS Token API.Use Transfer action.
            //https://developers.eos.io/manuals/eosjs/latest/basic-usage/browser

            //string _code = "eosio.token", _action = "transfer", _memo = "";
            //TransferArgs _args = new TransferArgs() { from = "yatendra1", to = "yatendra1", quantity = "1.0000 EOS", memo = _memo };
            //var abiJsonToBin = chainAPI.GetAbiJsonToBin(_code, _action, _args);
            //logger.Info("For code {0}, action {1}, args {2} and memo {3} recieved bin {4}", _code, _action, _args, _memo, abiJsonToBin.binargs);

            //var abiBinToJson = chainAPI.GetAbiBinToJson(_code, _action, abiJsonToBin.binargs);
            //logger.Info("Received args json {0}", JsonConvert.SerializeObject(abiBinToJson.args));


            //TransferArgs args = new TransferArgs() { from = fromTelosAccountName, to = toTelosAccountName, quantity = "1.0000 EOS", memo = memo };
            TransferArgs args = new TransferArgs() { from = fromTelosAccountName, to = toTelosAccountName, quantity = quanitity, memo = memo };
            // var abiJsonToBin = EOSIOOASIS.ChainAPI.GetAbiJsonToBin("eosio.token", "transfer", args);

            //prepare action object
            //EOSNewYork.EOSCore.Params.Action action = new ActionUtility(ENDPOINT_TEST).GetActionObject("transfer", fromTelosAccountName, "active", "eosio.token", args);
            //EOSNewYork.EOSCore.Params.Action action = new ActionUtility(ENDPOINT_TEST).GetActionObject("transfer", fromTelosAccountName, "active", "seed.seeds", args);
            EOSNewYork.EOSCore.Params.Action action = new ActionUtility(ENDPOINT_TEST).GetActionObject("transfer", fromTelosAccountName, "active", "token.seeds", args);

            var keypair = KeyManager.GenerateKeyPair();
            //List<string> privateKeysInWIF = new List<string> { keypair.PrivateKey }; //TODO: Set Private Key
            List<string> privateKeysInWIF = new List<string> { "5KW2jynm7kHw9XfAJ6WKPFk9xfP6rrDB6ggpuk48i3sTZCwVnz4" }; //TODO: Set Private Key

            //push transaction
            var transactionResult = EOSIOOASIS.ChainAPI.PushTransaction(new[] { action }, privateKeysInWIF);


            // logger.Info(transactionResult.transaction_id);

            //transactionResult.processed
            return transactionResult.transaction_id;


            // string accountName = "eosio";
            //var abi = EOSIOOASIS.ChainAPI.GetAbi(accountName);

            //abi.abi.actions[0].
            //abi.abi.tables

            //logger.Info("For account {0} recieved abi {1}", accountName, JsonConvert.SerializeObject(abi));
        }

        private SendInviteResult SendInviteToJoinSeeds(string sponsorTelosAccountName, string referrerTelosAccountName, float transferQuantitiy, float sowQuantitiy)
        {
            //https://joinseeds.github.io/seeds-smart-contracts/onboarding.html
            //https://github.com/JoinSEEDS/seeds-smart-contracts/blob/master/scripts/onboarding-helper.js

            string randomHex = GetRandomHexNumber(64); //16
            string inviteHash = GetSHA256Hash(randomHex);
            var keypair = KeyManager.GenerateKeyPair();
            List<string> privateKeysInWIF = new List<string> { keypair.PrivateKey };

            EOSNewYork.EOSCore.Params.Action action = new ActionUtility(ENDPOINT_TEST).GetActionObject("invitefor", sponsorTelosAccountName, "active", "join.seeds", new Invite() { sponsor = sponsorTelosAccountName, referrer = referrerTelosAccountName, invite_hash = inviteHash, transfer_quantity = transferQuantitiy, sow_quantity = sowQuantitiy });
            var transactionResult = EOSIOOASIS.ChainAPI.PushTransaction(new[] { action }, privateKeysInWIF);

            return new SendInviteResult() { TransactionId = transactionResult.transaction_id, InviteSecret = inviteHash };
        }

        private string AcceptInviteToJoinSeeds(string telosAccountName, string inviteSecret)
        {
            //https://joinseeds.github.io/seeds-smart-contracts/onboarding.html
            //inviteSecret = inviteHash

            var keypair = KeyManager.GenerateKeyPair();
            List<string> privateKeysInWIF = new List<string> { keypair.PrivateKey };

            EOSNewYork.EOSCore.Params.Action action = new ActionUtility(ENDPOINT_TEST).GetActionObject("accept", telosAccountName, "active", "join.seeds", new Accept() { account = telosAccountName, invite_secret = inviteSecret, publicKey = keypair.PublicKey });
            var transactionResult = EOSIOOASIS.ChainAPI.PushTransaction(new[] { action }, privateKeysInWIF);

            return transactionResult.transaction_id;
        }

        private bool AddKarmaForSeeds(Guid avatarId, KarmaTypePositive seedsKarmaType, KarmaTypePositive seedsKarmaHeroType, KarmaSourceType receivingKarmaFor, string appWebsiteServiceName, string appWebsiteServiceDesc, string appWebsiteServiceLink = null)
        {
            //TODO: Add new karma methods OASIS.API.CORE that allow bulk/batch karma to be added in one call (maybe use params?)
            bool karmaHeroResult = !AvatarManagerInstance.AddKarmaToAvatar(avatarId, seedsKarmaHeroType, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink, Core.Enums.ProviderType.SEEDSOASIS).IsError;
            bool karmaSeedsResult = AvatarManagerInstance.AddKarmaToAvatar(avatarId, seedsKarmaType, receivingKarmaFor, appWebsiteServiceName, appWebsiteServiceDesc, appWebsiteServiceLink, Core.Enums.ProviderType.SEEDSOASIS).IsError;
            return karmaHeroResult && karmaSeedsResult;
        }

        private static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            _random.NextBytes(buffer);

            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());

            if (digits % 2 == 0)
                return result;

            return result + _random.Next(16).ToString("X");
        }

        private static string GetSHA256Hash(string value)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string hash = GetHash(sha256Hash, value);

                /*
                Console.WriteLine($"The SHA256 hash of {value} is: {hash}.");
                Console.WriteLine("Verifying the hash...");

                if (VerifyHash(sha256Hash, value, hash))
                    Console.WriteLine("The hashes are the same.");
                else
                    Console.WriteLine("The hashes are not same.");
                */

                return hash;
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            // Hash the input.
            var hashOfInput = GetHash(hashAlgorithm, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
