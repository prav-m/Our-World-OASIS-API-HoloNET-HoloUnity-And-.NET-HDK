﻿using NextGenSoftware.OASIS.API.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.Contracts;
//using Nethereum.Model;
using Nethereum.Web3.Accounts;
using Nethereum.Web3;
using NextGenSoftware.OASIS.API.Core.Interfaces;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Helpers;

namespace NextGenSoftware.OASIS.API.Providers.EthereumOASIS
{
    public class EthereumOASIS : OASISStorageBase, IOASISStorage, IOASISNET
    {
        public Web3 Web3 { get; set; }
        public string HostUri { get; set; }

        private class account
        {
            public string userid;
            public string name;
            public string providerkey;
            public string password;
            public string email;
            public string title;
            public string firstname;
            public string lastname;
            public string dob;
            public string playeraddr;
            public int karma;
        };


        public EthereumOASIS(string hostUri)
        {
            this.ProviderName = "EthereumOASIS";
            this.ProviderDescription = "Ethereum Provider";
            this.ProviderType = new Core.Helpers.EnumValue<ProviderType>(Core.Enums.ProviderType.EthereumOASIS);
            this.ProviderCategory = new Core.Helpers.EnumValue<ProviderCategory>(Core.Enums.ProviderCategory.StorageAndNetwork);
            this.HostUri = hostUri;

           // oasisTest();
        }

        private void oasisTest()
        {
            string address = "0x5AD3aeBBB1E99c851D68aeFaF07FC98c2e706Fd5";

            //The ABI for the contract.
            string ABI = @"[{'inputs':[],'stateMutability':'nonpayable','type':'constructor'},{'inputs':[{'internalType':'string','name':'first','type':'string'},{'internalType':'string','name':'second','type':'string'}],'name':'compareString','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'pure','type':'function'},{'inputs':[{'internalType':'string','name':'userid','type':'string'},{'internalType':'string','name':'name','type':'string'},{'internalType':'string','name':'providerkey','type':'string'},{'internalType':'string','name':'password','type':'string'},{'internalType':'string','name':'email','type':'string'},{'internalType':'string','name':'title','type':'string'},{'internalType':'string','name':'firstname','type':'string'},{'internalType':'string','name':'lastname','type':'string'},{'internalType':'string','name':'dob','type':'string'},{'internalType':'string','name':'playeraddr','type':'string'},{'internalType':'uint32','name':'karma','type':'uint32'}],'name':'createAccount','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'string','name':'userid','type':'string'}],'name':'deleteAccount','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'getAccountCount','outputs':[{'internalType':'uint256','name':'count','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'string','name':'userid','type':'string'}],'name':'getAccountParameter','outputs':[{'components':[{'internalType':'string','name':'userid','type':'string'},{'internalType':'string','name':'name','type':'string'},{'internalType':'string','name':'providerkey','type':'string'},{'internalType':'string','name':'password','type':'string'},{'internalType':'string','name':'email','type':'string'},{'internalType':'string','name':'title','type':'string'},{'internalType':'string','name':'firstname','type':'string'},{'internalType':'string','name':'lastname','type':'string'},{'internalType':'string','name':'dob','type':'string'},{'internalType':'string','name':'playeraddr','type':'string'},{'internalType':'uint32','name':'karma','type':'uint32'}],'internalType':'structOASIS.account','name':'','type':'tuple'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'totalAccounts','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'string','name':'userid','type':'string'},{'internalType':'string','name':'name','type':'string'},{'internalType':'string','name':'providerkey','type':'string'},{'internalType':'string','name':'password','type':'string'},{'internalType':'string','name':'email','type':'string'},{'internalType':'string','name':'title','type':'string'},{'internalType':'string','name':'firstname','type':'string'},{'internalType':'string','name':'lastname','type':'string'},{'internalType':'string','name':'dob','type':'string'},{'internalType':'string','name':'playeraddr','type':'string'},{'internalType':'uint32','name':'karma','type':'uint32'}],'name':'updateAccount','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'nonpayable','type':'function'}]";

            var account = new Account("0x96a2cdfa6e4198b683188e0265d548da23a829f00fdf4858f9b7899e5d55fd4c");
            Web3 = new Web3(account, HostUri);
            Contract oasisContract = Web3.Eth.GetContract(ABI, address);

            account newAccount = new account();
            
            //TODO: Take out these Console.Write and put this into its own TestHarness like the other providers! ;-) lol
            Console.Write("User Id: ");
            newAccount.userid = Console.ReadLine();
            Console.Write("Name: ");
            newAccount.name = Console.ReadLine();
            Console.Write("Provider Key: ");
            newAccount.providerkey = Console.ReadLine();
            Console.Write("Password: ");
            newAccount.password = Console.ReadLine();
            Console.Write("Email: ");
            newAccount.email = Console.ReadLine();
            Console.Write("Title: ");
            newAccount.title = Console.ReadLine();
            Console.Write("First Name: ");
            newAccount.firstname = Console.ReadLine();
            Console.Write("Last Name: ");
            newAccount.lastname = Console.ReadLine();
            Console.Write("Date of Birth: ");
            newAccount.dob = Console.ReadLine();
            Console.Write("Player Address: ");
            newAccount.playeraddr = Console.ReadLine();
            Console.Write("Karma: ");
            newAccount.karma = Convert.ToInt32(Console.ReadLine());

            var newAccountFunction = oasisContract.GetFunction("createAccount");

            object[] accParams = new object[]{
                newAccount.userid,
                newAccount.name,
                newAccount.providerkey,
                newAccount.password,
                newAccount.email,
                newAccount.title,
                newAccount.firstname,
                newAccount.lastname,
                newAccount.dob,
                newAccount.playeraddr,
                newAccount.karma
            };

            // await newAccountFunction.CallAsync<int>(accParams);

            var getAccountCountFunction = oasisContract.GetFunction("getAccountCount");
            var totalAccount = getAccountCountFunction.CallAsync<int>();

            Console.Write("Account Creation Succeed, Count: ", totalAccount);
        }
        public IEnumerable<IHolon> GetHolonsNearMe(HolonType Type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPlayer> GetPlayersNearMe()
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<IAvatar>> LoadAllAvatarsAsync()
        {
            throw new NotImplementedException();
        }

        public override IAvatar LoadAvatar(string username, string password)
        {
            throw new NotImplementedException();
        }

        public override Task<IAvatar> LoadAvatarAsync(string providerKey)
        {
            throw new NotImplementedException();
        }

        public override Task<IAvatar> LoadAvatarAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public override Task<IAvatar> LoadAvatarAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public override Task<IAvatar> SaveAvatarAsync(IAvatar Avatar)
        {
            throw new NotImplementedException();
        }

        public override Task<ISearchResults> SearchAsync(ISearchParams searchTerm)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IAvatar> LoadAllAvatars()
        {
            throw new NotImplementedException();
        }

        public override IAvatar LoadAvatar(Guid Id)
        {
            throw new NotImplementedException();
        }

        public override IAvatar SaveAvatar(IAvatar Avatar)
        {
            throw new NotImplementedException();
        }

        public override IAvatar LoadAvatar(string username)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAvatar(Guid id, bool softDelete = true)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> DeleteAvatarAsync(Guid id, bool softDelete = true)
        {
            throw new NotImplementedException();
        }

        public override IHolon LoadHolon(Guid id)
        {
            throw new NotImplementedException();
        }

        public override IHolon LoadHolon(string providerKey)
        {
            throw new NotImplementedException();
        }

        public override Task<IHolon> LoadHolonAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<IHolon> LoadHolonAsync(string providerKey)
        {
            throw new NotImplementedException();
        }



        public override IHolon SaveHolon(IHolon holon)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IHolon> SaveHolons(IEnumerable<IHolon> holons)
        {
            throw new NotImplementedException();
        }

        public override Task<IHolon> SaveHolonAsync(IHolon holon)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<IHolon>> SaveHolonsAsync(IEnumerable<IHolon> holons)
        {
            throw new NotImplementedException();
        }

        public override Task<IAvatar> LoadAvatarForProviderKeyAsync(string providerKey)
        {
            throw new NotImplementedException();
        }

        public override IAvatar LoadAvatarForProviderKey(string providerKey)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAvatar(string providerKey, bool softDelete = true)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> DeleteAvatarAsync(string providerKey, bool softDelete = true)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteHolon(Guid id, bool softDelete = true)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> DeleteHolonAsync(Guid id, bool softDelete = true)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteHolon(string providerKey, bool softDelete = true)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> DeleteHolonAsync(string providerKey, bool softDelete = true)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IHolon> LoadHolonsForParent(Guid id, HolonType type = HolonType.All)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<IHolon>> LoadHolonsForParentAsync(Guid id, HolonType type = HolonType.All)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IHolon> LoadHolonsForParent(string providerKey, HolonType type = HolonType.All)
        {
            throw new NotImplementedException();
        }

        public override Task<OASISResult<IEnumerable<IHolon>>> LoadHolonsForParentAsync(string providerKey, HolonType type = HolonType.All)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IHolon> LoadAllHolons(HolonType type = HolonType.Holon)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<IHolon>> LoadAllHolonsAsync(HolonType type = HolonType.Holon)
        {
            throw new NotImplementedException();
        }
    }
}
