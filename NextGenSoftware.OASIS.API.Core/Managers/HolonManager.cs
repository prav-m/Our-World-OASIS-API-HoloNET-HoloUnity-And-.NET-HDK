﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Events;
using NextGenSoftware.OASIS.API.Core.Helpers;
using NextGenSoftware.OASIS.API.Core.Interfaces;

namespace NextGenSoftware.OASIS.API.Core.Managers
{
    public class HolonManager : OASISManager
    {
        public delegate void StorageProviderError(object sender, AvatarManagerErrorEventArgs e);

        //TODO: In future more than one storage provider can be active at a time where each call can specify which provider to use.
        public HolonManager(IOASISStorage OASISStorageProvider) : base(OASISStorageProvider)
        {

        }

        public OASISResult<IHolon> LoadHolon(Guid id, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IHolon> result = new OASISResult<IHolon>();

            result = LoadHolonForProviderType(id, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    {
                        result = LoadHolonForProviderType(id, type.Value, result);

                        if (result.Result != null)
                            break;
                    }
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to load the holon with id ", id, ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);

            // Store the original holon for change tracking in STAR/COSMIC.
            result.Result.Original = result.Result;
            return result;
        }
     
        public async Task<OASISResult<IHolon>> LoadHolonAsync(Guid id, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IHolon> result = new OASISResult<IHolon>();

            result = await LoadHolonForProviderTypeAsync(id, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    {
                        result = await LoadHolonForProviderTypeAsync(id, type.Value, result);

                        if (result.Result != null)
                            break;
                    }
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to load the holon with id ", id, ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);

            // Store the original holon for change tracking in STAR/COSMIC.
            result.Result.Original = result.Result;
            return result;
        }

        public OASISResult<IHolon> LoadHolon(string providerKey, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IHolon> result = new OASISResult<IHolon>();

            result = LoadHolonForProviderType(providerKey, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    {
                        result = LoadHolonForProviderType(providerKey, type.Value, result);

                        if (result.Result != null)
                            break;
                    }
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to load the holon with providerKey ", providerKey, ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);

            // Store the original holon for change tracking in STAR/COSMIC.
            result.Result.Original = result.Result;
            return result;
        }

        public async Task<OASISResult<IHolon>> LoadHolonAsync(string providerKey, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IHolon> result = new OASISResult<IHolon>();

            result = await LoadHolonForProviderTypeAsync(providerKey, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    {
                        result = await LoadHolonForProviderTypeAsync(providerKey, type.Value, result);

                        if (result.Result != null)
                            break;
                    }
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to load the holon with providerKey ", providerKey, ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);

            // Store the original holon for change tracking in STAR/COSMIC.
            result.Result.Original = result.Result;
            return result;
        }

        public OASISResult<IEnumerable<IHolon>> LoadHolonsForParent(Guid id, HolonType holonType = HolonType.All, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IEnumerable<IHolon>> result = new OASISResult<IEnumerable<IHolon>>();

            result = LoadHolonsForParentForProviderType(id, holonType, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    {
                        result = LoadHolonsForParentForProviderType(id, holonType, type.Value, result);

                        if (result.Result != null)
                            break;
                    }
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to load holons for parent with id ", id, ", and holonType ", Enum.GetName(typeof(HolonType), holonType), ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);

            // Store the original holon for change tracking in STAR/COSMIC.
            foreach (IHolon holon in result.Result)
                holon.Original = holon;

            return result;
        }

        public async Task<OASISResult<IEnumerable<IHolon>>> LoadHolonsForParentAsync(Guid id, HolonType holonType = HolonType.All, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IEnumerable<IHolon>> result = new OASISResult<IEnumerable<IHolon>>();

            result = await LoadHolonsForParentForProviderTypeAsync(id, holonType, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    {
                        result = await LoadHolonsForParentForProviderTypeAsync(id, holonType, type.Value, result);

                        if (result.Result != null)
                            break;
                    }
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to load holons for parent with id ", id, ", and holonType ", Enum.GetName(typeof(HolonType), holonType), ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);

            // Store the original holon for change tracking in STAR/COSMIC.
            foreach (IHolon holon in result.Result)
                holon.Original = holon;

            return result;
        }

        public OASISResult<IEnumerable<IHolon>> LoadHolonsForParent(string providerKey, HolonType holonType = HolonType.All, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IEnumerable<IHolon>> result = new OASISResult<IEnumerable<IHolon>>();

            result = LoadHolonsForParentForProviderType(providerKey, holonType, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    {
                        result = LoadHolonsForParentForProviderType(providerKey, holonType, type.Value, result);

                        if (result.Result != null)
                            break;
                    }
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to load holons for parent with providerKey ", providerKey, ", and holonType ", Enum.GetName(typeof(HolonType), holonType), ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);

            // Store the original holon for change tracking in STAR/COSMIC.
            foreach (IHolon holon in result.Result)
                holon.Original = holon;

            return result;
        }

        //TODO: Need to implement this proper way of calling an OASIS method across the entire OASIS...
        public async Task<OASISResult<IEnumerable<IHolon>>> LoadHolonsForParentAsync(string providerKey, HolonType holonType = HolonType.All, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IEnumerable<IHolon>> result = new OASISResult<IEnumerable<IHolon>>();

            result = await LoadHolonsForParentForProviderTypeAsync(providerKey, holonType, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    {
                        result = await LoadHolonsForParentForProviderTypeAsync(providerKey, holonType, type.Value, result);

                        if (result.Result != null)
                            break;
                    }
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to load holons for parent with providerKey ", providerKey, ", and holonType ", Enum.GetName(typeof(HolonType), holonType), ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);

            // Store the original holon for change tracking in STAR/COSMIC.
            foreach (IHolon holon in result.Result)
                holon.Original = holon;

            return result;
        }

        public OASISResult<IEnumerable<IHolon>> LoadAllHolons(HolonType holonType = HolonType.All, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IEnumerable<IHolon>> result = new OASISResult<IEnumerable<IHolon>>();

            result = LoadAllHolonsForProviderType(holonType, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    {
                        result = LoadAllHolonsForProviderType(holonType, type.Value, result);

                        if (result.Result != null)
                            break;
                    }
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to load all holons for holonType ", Enum.GetName(typeof(HolonType), holonType), ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);

            // Store the original holon for change tracking in STAR/COSMIC.
            foreach (IHolon holon in result.Result)
                holon.Original = holon;

            return result;
        }

        public async Task<OASISResult<IEnumerable<IHolon>>> LoadAllHolonsAsync(HolonType holonType = HolonType.All, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IEnumerable<IHolon>> result = new OASISResult<IEnumerable<IHolon>>();

            result = await LoadAllHolonsForProviderTypeAsync(holonType, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    {
                        result = await LoadAllHolonsForProviderTypeAsync(holonType, type.Value, result);

                        if (result.Result != null)
                            break;
                    }
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to load all holons for holonType ", Enum.GetName(typeof(HolonType), holonType), ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);

            // Store the original holon for change tracking in STAR/COSMIC.
            foreach (IHolon holon in result.Result)
                holon.Original = holon;

            return result;
        }

        public OASISResult<IHolon> SaveHolon(IHolon holon, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IHolon> result = new OASISResult<IHolon>();

            result = SaveHolonForProviderType(holon, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    {
                        result = SaveHolonForProviderType(holon, type.Value, result);

                        if (result.Result != null)
                            break;
                    }
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to save ", LoggingHelper.GetHolonInfoForLogging(holon), ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            foreach (EnumValue<ProviderType> type in ProviderManager.GetProvidersThatAreAutoReplicating())
            {
                if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    SaveHolonForProviderType(holon, type.Value);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);
            result.IsSaved = result.Result != null && result.Result.Id != Guid.Empty;
            result.Result.IsChanged = !result.IsSaved;

            return result;
        }

        //TODO: Need to implement this format to ALL other Holon/Avatar Manager methods with OASISResult, etc.
        public async Task<OASISResult<IHolon>> SaveHolonAsync(IHolon holon, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IHolon> result = new OASISResult<IHolon>();

            result = await SaveHolonForProviderTypeAsync(holon, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                        result = await SaveHolonForProviderTypeAsync(holon, type.Value, result);
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                string errorMessage = string.Concat("All registered OASIS Providers in the AutoFailOverList failed to save ", LoggingHelper.GetHolonInfoForLogging(holon), ". Please view the logs for more information. Providers in the list are: ", ProviderManager.GetProviderAutoFailOverListAsString());
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
            }

            foreach (EnumValue<ProviderType> type in ProviderManager.GetProvidersThatAreAutoReplicating())
            {
                if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    await SaveHolonForProviderTypeAsync(holon, type.Value);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);
            result.IsSaved = result.Result != null && result.Result.Id != Guid.Empty;
            result.Result.IsChanged = !result.IsSaved;

            return result;
        }

        public OASISResult<IEnumerable<IHolon>> SaveHolons(IEnumerable<IHolon> holons, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IEnumerable<IHolon>> result = new OASISResult<IEnumerable<IHolon>>();

            result = SaveHolonsForProviderType(holons, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                        result = SaveHolonsForProviderType(holons, type.Value, result);
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                result.Message = string.Concat("All Registered OASIS Providers In The AutoFailOverList Failed To Save The Holons. Providers in list are ", ProviderManager.GetProviderAutoFailOverListAsString());
            }
            else
            {
                //Should already be false but just in case...
                foreach (IHolon holon in result.Result)
                    holon.IsChanged = false;
            }

            foreach (EnumValue<ProviderType> type in ProviderManager.GetProvidersThatAreAutoReplicating())
            {
                if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    SaveHolonsForProviderType(holons, type.Value);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);
            return result;
        }

        //TODO: Need to implement this format to ALL other Holon/Avatar Manager methods with OASISResult, etc.
        public async Task<OASISResult<IEnumerable<IHolon>>> SaveHolonsAsync(IEnumerable<IHolon> holons, ProviderType providerType = ProviderType.Default)
        {
            ProviderType currentProviderType = ProviderManager.CurrentStorageProviderType.Value;
            OASISResult<IEnumerable<IHolon>> result = new OASISResult<IEnumerable<IHolon>>();

            result = await SaveHolonsForProviderTypeAsync(holons, providerType, result);

            if (result.Result == null)
            {
                foreach (EnumValue<ProviderType> type in ProviderManager.GetProviderAutoFailOverList())
                {
                    if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                        result = await SaveHolonsForProviderTypeAsync(holons, type.Value, result);
                }
            }

            if (result.Result == null)
            {
                result.IsError = true;
                result.Message = string.Concat("All Registered OASIS Providers In The AutoFailOverList Failed To Save The Holons. Providers in list are ", ProviderManager.GetProviderAutoFailOverListAsString());
            }
            else
            {
                //Should already be false but just in case...
                foreach (IHolon holon in result.Result)
                    holon.IsChanged = false;
            }

            foreach (EnumValue<ProviderType> type in ProviderManager.GetProvidersThatAreAutoReplicating())
            {
                if (type.Value != providerType && type.Value != ProviderManager.CurrentStorageProviderType.Value)
                    await SaveHolonsForProviderTypeAsync(holons, type.Value);
            }

            SwitchBackToCurrentProvider(currentProviderType, ref result);
            return result;
        }

        public OASISResult<bool> DeleteHolon(Guid id, bool softDelete = true, ProviderType provider = ProviderType.Default)
        {
            OASISResult<bool> result = new OASISResult<bool>();

            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(provider);

                if (providerResult.IsError)
                {
                    result.IsError = true;
                    result.Message = providerResult.Message;
                }
                else
                    result.Result = providerResult.Result.DeleteHolon(id, softDelete);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Concat("An unknown error occured in DeleteHolon method. id: ", id, ", softDelete = ", softDelete, ", providerType = ", Enum.GetName(typeof(ProviderType), provider), " Error details: ", ex.ToString());
                result.IsError = true;
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
                result.Exception = ex;
            }

            return result;
        }

        public async Task<OASISResult<bool>> DeleteHolonAsync(Guid id, bool softDelete = true, ProviderType provider = ProviderType.Default)
        {
            OASISResult<bool> result = new OASISResult<bool>();

            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(provider);

                if (providerResult.IsError)
                {
                    result.IsError = true;
                    result.Message = providerResult.Message;
                }
                else
                    result.Result = await providerResult.Result.DeleteHolonAsync(id, softDelete);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Concat("An unknown error occured in DeleteHolonAsync method. id: ", id, ", softDelete = ", softDelete, ", providerType = ", Enum.GetName(typeof(ProviderType), provider), " Error details: ", ex.ToString());
                result.IsError = true;
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
                result.Exception = ex;
            }

            return result;
        }

        public OASISResult<bool> DeleteHolon(string providerKey, bool softDelete = true, ProviderType provider = ProviderType.Default)
        {
            OASISResult<bool> result = new OASISResult<bool>();

            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(provider);

                if (providerResult.IsError)
                {
                    result.IsError = true;
                    result.Message = providerResult.Message;
                }
                else
                    result.Result = providerResult.Result.DeleteHolon(providerKey, softDelete);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Concat("An unknown error occured in DeleteHolonAsync method. providerKey: ", providerKey, ", softDelete = ", softDelete, ", providerType = ", Enum.GetName(typeof(ProviderType), provider), " Error details: ", ex.ToString());
                result.IsError = true;
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
                result.Exception = ex;
            }

            return result;
        }

        public async Task<OASISResult<bool>> DeleteHolonAsync(string providerKey, bool softDelete = true, ProviderType provider = ProviderType.Default)
        {
            OASISResult<bool> result = new OASISResult<bool>();

            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(provider);

                if (providerResult.IsError)
                {
                    result.IsError = true;
                    result.Message = providerResult.Message;
                }
                else
                    result.Result = await providerResult.Result.DeleteHolonAsync(providerKey, softDelete);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Concat("An unknown error occured in DeleteHolonAsync method. providerKey: ", providerKey, ", softDelete = ", softDelete, ", providerType = ", Enum.GetName(typeof(ProviderType), provider), " Error details: ", ex.ToString());
                result.IsError = true;
                result.Message = errorMessage;
                LoggingManager.Log(errorMessage, LogType.Error);
                result.Exception = ex;
            }

            return result;
        }

        private IHolon PrepareHolonForSaving(IHolon holon)
        {
            // TODO: I think it's best to include audit stuff here so the providers do not need to worry about it?
            // Providers could always override this behaviour if they choose...
            
            //if (holon.Id != Guid.Empty)
            if (!holon.IsNewHolon)
            {
                holon.ModifiedDate = DateTime.Now;

                if (AvatarManager.LoggedInAvatar != null)
                    holon.ModifiedByAvatarId = AvatarManager.LoggedInAvatar.Id;
            }
            else
            {
                holon.IsActive = true;
                holon.CreatedDate = DateTime.Now;

                if (AvatarManager.LoggedInAvatar != null)
                    holon.CreatedByAvatarId = AvatarManager.LoggedInAvatar.Id;
            }

            return holon;
        }

        private IEnumerable<IHolon> PrepareHolonsForSaving(IEnumerable<IHolon> holons)
        {
            List<IHolon> holonsToReturn = new List<IHolon>();

            foreach (IHolon holon in holons)
                holonsToReturn.Add(PrepareHolonForSaving(holon));

            return holonsToReturn;
        }

        private void LogError(IHolon holon, ProviderType providerType, string errorMessage)
        {
            LoggingManager.Log(string.Concat("An error occured attempting to save the ", LoggingHelper.GetHolonInfoForLogging(holon), " using the ", Enum.GetName(providerType), " provider. Error Details: ", errorMessage), LogType.Error);
        }

        private OASISResult<IHolon> LoadHolonForProviderType(Guid id, ProviderType providerType, OASISResult<IHolon> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }
                }
                else if (result != null)
                {
                    result.Result = providerResult.Result.LoadHolon(id);
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to load the holon for id ", id, " using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private async Task<OASISResult<IHolon>> LoadHolonForProviderTypeAsync(Guid id, ProviderType providerType, OASISResult<IHolon> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }
                }
                else if (result != null)
                {
                    result.Result = await providerResult.Result.LoadHolonAsync(id);
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to load the holon for id ", id, " using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private OASISResult<IHolon> LoadHolonForProviderType(string providerKey, ProviderType providerType, OASISResult<IHolon> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }
                }
                else if (result != null)
                {
                    result.Result = providerResult.Result.LoadHolon(providerKey);
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to load the holon for providerKey ", providerKey, " using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private async Task<OASISResult<IHolon>> LoadHolonForProviderTypeAsync(string providerKey, ProviderType providerType, OASISResult<IHolon> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }
                }
                else if (result != null)
                {
                    result.Result = await providerResult.Result.LoadHolonAsync(providerKey);
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to load the holon for providerKey ", providerKey, " using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private OASISResult<IEnumerable<IHolon>> LoadHolonsForParentForProviderType(Guid id, HolonType holonType, ProviderType providerType, OASISResult<IEnumerable<IHolon>> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }
                }
                else if (result != null)
                {
                    result.Result = providerResult.Result.LoadHolonsForParent(id, holonType);
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to load the holons for parent with id ", id, " and holonType ", Enum.GetName(typeof(HolonType), holonType), " using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private async Task<OASISResult<IEnumerable<IHolon>>> LoadHolonsForParentForProviderTypeAsync(Guid id, HolonType holonType, ProviderType providerType, OASISResult<IEnumerable<IHolon>> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }
                }
                else if (result != null)
                {
                    result.Result = await providerResult.Result.LoadHolonsForParentAsync(id, holonType);
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to load the holons for parent with id ", id, " and holonType ", Enum.GetName(typeof(HolonType), holonType), " using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private OASISResult<IEnumerable<IHolon>> LoadHolonsForParentForProviderType(string providerKey, HolonType holonType, ProviderType providerType, OASISResult<IEnumerable<IHolon>> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }
                }
                else if (result != null)
                {
                    result.Result = providerResult.Result.LoadHolonsForParent(providerKey, holonType);
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to load the holons for parent with providerKey ", providerKey, " and holonType ", Enum.GetName(typeof(HolonType), holonType), " using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private async Task<OASISResult<IEnumerable<IHolon>>> LoadHolonsForParentForProviderTypeAsync(string providerKey, HolonType holonType, ProviderType providerType, OASISResult<IEnumerable<IHolon>> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }
                }
                else if (result != null)
                {
                    result = await providerResult.Result.LoadHolonsForParentAsync(providerKey, holonType);
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to load the holons for parent with providerKey ", providerKey, " and holonType ", Enum.GetName(typeof(HolonType), holonType), " using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private OASISResult<IEnumerable<IHolon>> LoadAllHolonsForProviderType(HolonType holonType, ProviderType providerType, OASISResult<IEnumerable<IHolon>> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }
                }
                else if (result != null)
                {
                    result.Result = providerResult.Result.LoadAllHolons(holonType);
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to load all holons for holonType ", Enum.GetName(typeof(HolonType), holonType), " using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private async Task<OASISResult<IEnumerable<IHolon>>> LoadAllHolonsForProviderTypeAsync(HolonType holonType, ProviderType providerType, OASISResult<IEnumerable<IHolon>> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }
                }
                else if (result != null)
                {
                    result.Result = await providerResult.Result.LoadAllHolonsAsync(holonType);
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to load all holons for holonType ", Enum.GetName(typeof(HolonType), holonType), " using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private OASISResult<IHolon> SaveHolonForProviderType(IHolon holon, ProviderType providerType, OASISResult<IHolon> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }

                    //TODO: In future will return these extra error messages in the OASISResult.
                }
                else if (result != null)
                {
                    result.Result = providerResult.Result.SaveHolon(PrepareHolonForSaving(holon));
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LogError(holon, providerType, ex.ToString());
            }

            return result;
        }

        private async Task<OASISResult<IHolon>> SaveHolonForProviderTypeAsync(IHolon holon, ProviderType providerType, OASISResult<IHolon> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }

                    //TODO: In future will return these extra error messages in the OASISResult.
                }
                else if (result != null)
                {
                    result.Result = await providerResult.Result.SaveHolonAsync(PrepareHolonForSaving(holon));
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LogError(holon, providerType, ex.ToString());
            }

            return result;
        }

        private OASISResult<IEnumerable<IHolon>> SaveHolonsForProviderType(IEnumerable<IHolon> holons, ProviderType providerType, OASISResult<IEnumerable<IHolon>> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }

                    //TODO: In future will return these extra error messages in the OASISResult.
                }
                else if (result != null)
                {
                    result.Result = providerResult.Result.SaveHolons(PrepareHolonsForSaving(holons));
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to save the holons using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private async Task<OASISResult<IEnumerable<IHolon>>> SaveHolonsForProviderTypeAsync(IEnumerable<IHolon> holons, ProviderType providerType, OASISResult<IEnumerable<IHolon>> result = null)
        {
            try
            {
                OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(providerType);

                if (providerResult.IsError)
                {
                    LoggingManager.Log(providerResult.Message, LogType.Error);

                    if (result != null)
                    {
                        result.IsError = true;
                        result.Message = providerResult.Message;
                    }

                    //TODO: In future will return these extra error messages in the OASISResult.
                }
                else if (result != null)
                {
                    result.Result = await providerResult.Result.SaveHolonsAsync(PrepareHolonsForSaving(holons));
                    result.IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                if (result != null)
                    result.Result = null;

                LoggingManager.Log(string.Concat("An error occured attempting to save the holons using the ", Enum.GetName(providerType), " provider. Error Details: ", ex.ToString()), LogType.Error);
            }

            return result;
        }

        private void SwitchBackToCurrentProvider<T>(ProviderType currentProviderType, ref OASISResult<T> result)
        {
            OASISResult<IOASISStorage> providerResult = ProviderManager.SetAndActivateCurrentStorageProvider(currentProviderType);

            if (providerResult.IsError)
            {
                result.IsWarning = true; //TODO: Not sure if this should be an error or a warning? Because there was no error saving the holons but an error switching back to the current provider.                
                //result.InnerMessages.Add(string.Concat("The holons saved but there was an error switching the default provider back to ", Enum.GetName(typeof(ProviderType), currentProviderType), " provider. Error Details: ", providerResult.Message));
                result.Message = string.Concat(result.Message, ". The holons saved but there was an error switching the default provider back to ", Enum.GetName(typeof(ProviderType), currentProviderType), " provider. Error Details: ", providerResult.Message);
            }
        }
    }
}