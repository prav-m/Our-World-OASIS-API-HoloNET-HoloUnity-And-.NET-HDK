﻿
using System;
using System.Collections.Generic;
using System.Linq;

namespace NextGenSoftware.OASIS.API.Core
{
    public class ProviderManager 
    {
        private static List<IOASISProvider> _registeredProviders = new List<IOASISProvider>();

        public static ProviderType CurrentStorageProviderType { get; private set; } = ProviderType.Default;

        public static string[] DefaultProviderTypes { get;  set; }

        public static IOASISStorage DefaultGlobalStorageProvider { get; set; }

        public static IOASISStorage CurrentStorageProvider { get; private set; } //TODO: Need to work this out because in future there can be more than one provider active at a time.

        //public static bool IgnoreDefaultProviderTypes { get; set; } = false;
        public static bool OverrideProviderType { get; set; } = false;


        //TODO: In future the registered providers will be dynamically loaded from MEF by watching a hot folder for compiled provider dlls (and other ways in future...)
        public static void RegisterProvider(IOASISProvider provider)
        {
            if (!_registeredProviders.Any(x => x.ProviderName == provider.ProviderName))
                _registeredProviders.Add(provider);
        }

        public static void RegisterProviders(List<IOASISProvider> providers)
        {
            foreach (IOASISProvider provider in providers)
                RegisterProvider(provider);
        }

        public static void UnRegisterProvider(IOASISProvider provider)
        {
            _registeredProviders.Remove(provider);
        }

        public static void UnRegisterProviders(List<IOASISProvider> providers)
        {
            foreach (IOASISProvider provider in providers)
                _registeredProviders.Remove(provider);
        }

        public static List<IOASISProvider> GetAllProviders()
        {
            return _registeredProviders;
        }

        public static List<IOASISProvider> GetProvidersOfCategory(ProviderCategory category)
        {
            return _registeredProviders.Where(x => x.ProviderCategory == category).ToList();
        }

        public static List<IOASISStorage> GetStorageProviders()
        {
            List<IOASISStorage> storageProviders = new List<IOASISStorage>();

            foreach (IOASISProvider provider in _registeredProviders.Where(x => x.ProviderCategory == ProviderCategory.Storage || x.ProviderCategory == ProviderCategory.StorageAndNetwork).ToList())
                storageProviders.Add((IOASISStorage)provider);  

            return storageProviders;
        }

        public static List<IOASISNET> GetNetworkProviders()
        {
            List<IOASISNET> networkProviders = new List<IOASISNET>();

            foreach (IOASISProvider provider in _registeredProviders.Where(x => x.ProviderCategory == ProviderCategory.Network || x.ProviderCategory == ProviderCategory.StorageAndNetwork).ToList())
                networkProviders.Add((IOASISNET)provider);

            return networkProviders;
        }

        public static List<IOASISRenderer> GetRendererProviders()
        {
            List<IOASISRenderer> rendererProviders = new List<IOASISRenderer>();

            foreach (IOASISProvider provider in _registeredProviders.Where(x => x.ProviderCategory == ProviderCategory.Renderer).ToList())
                rendererProviders.Add((IOASISRenderer)provider);

            return rendererProviders;
        }

        public static IOASISProvider GetProvider(ProviderType type)
        {
            return _registeredProviders.FirstOrDefault(x => x.ProviderType == type);
        }

        public static IOASISStorage GetStorageProvider(ProviderType type)
        {
            return (IOASISStorage)_registeredProviders.FirstOrDefault(x => x.ProviderType == type && x.ProviderCategory == ProviderCategory.Storage);
        }

        public static IOASISNET GetNetworkProvider(ProviderType type)
        {
            return (IOASISNET)_registeredProviders.FirstOrDefault(x => x.ProviderType == type && x.ProviderCategory == ProviderCategory.Network);
        }

        public static IOASISRenderer GetRendererProvider(ProviderType type)
        {
            return (IOASISRenderer)_registeredProviders.FirstOrDefault(x => x.ProviderType == type && x.ProviderCategory == ProviderCategory.Renderer);
        }

        public static bool IsProviderRegistered(IOASISProvider provider)
        {
            return _registeredProviders.Any(x => x.ProviderName == provider.ProviderName);
        }

        public static bool IsProviderRegistered(ProviderType providerType)
        {
            return _registeredProviders.Any(x => x.ProviderType == providerType);
        }

        /*
        //TODO: Try and fix this later...
        public static IOASISStorage SetAndActivateCurrentStorageProvider(ProviderType providerType, bool setGlobally = false)
        {
            if (providerType == ProviderType.Default)
            {
                // If a global provider has been set and the REST API call has not overiden the provider (OverrideProviderType) then set to global provider.
                if (DefaultGlobalStorageProvider != null && DefaultGlobalStorageProvider != CurrentStorageProvider && !OverrideProviderType)
                    return SetAndActivateCurrentStorageProvider(DefaultGlobalStorageProvider);

                // Otherwise set to default provider (configured in appSettings.json) if the provider has not been overiden in the REST call.
                else if (!OverrideProviderType && DefaultProviderTypes != null && CurrentStorageProviderType != (ProviderType)Enum.Parse(typeof(ProviderType), DefaultProviderTypes[0]))
                    return SetAndActivateCurrentStorageProviderInternal(ProviderType.Default, setGlobally);
            }

            if (CurrentStorageProvider == null || (CurrentStorageProviderType != providerType))
                return SetAndActivateCurrentStorageProviderInternal(providerType, setGlobally);

            return CurrentStorageProvider;
        }*/


        public static IOASISStorage SetAndActivateCurrentStorageProvider(ProviderType providerType)
        {
            if (providerType == ProviderType.Default)
                return SetAndActivateCurrentStorageProvider();
            else
                return SetAndActivateCurrentStorageProvider(providerType, false);
        }

        //THIS ONE WORKS.
        public static IOASISStorage SetAndActivateCurrentStorageProvider()
        {
            // If a global provider has been set and the REST API call has not overiden the provider (OverrideProviderType) then set to global provider.
            if (DefaultGlobalStorageProvider != null && DefaultGlobalStorageProvider != CurrentStorageProvider && !OverrideProviderType)
                return SetAndActivateCurrentStorageProvider(DefaultGlobalStorageProvider);

            // Otherwise set to default provider (configured in appSettings.json) if the provider has not been overiden in the REST call.
            else if (!OverrideProviderType && DefaultProviderTypes != null && CurrentStorageProviderType != (ProviderType)Enum.Parse(typeof(ProviderType), DefaultProviderTypes[0]))
                return SetAndActivateCurrentStorageProvider(ProviderType.Default, false);
            //return SetAndActivateCurrentStorageProviderInternal(ProviderType.Default);

            return CurrentStorageProvider;
        }


        public static IOASISStorage SetAndActivateCurrentStorageProvider(IOASISProvider OASISProvider)
        {
            if (OASISProvider != CurrentStorageProvider)
            {
                if (OASISProvider != null)
                {
                    if (!IsProviderRegistered(OASISProvider))
                        RegisterProvider(OASISProvider);

                    return SetAndActivateCurrentStorageProvider(OASISProvider.ProviderType);
                }
            }

            return CurrentStorageProvider;
        }

        //TODO: In future more than one StorageProvider will be active at a time so we need to work out how to handle this...
        //private static IOASISStorage SetAndActivateCurrentStorageProviderInternal(ProviderType providerType, bool setGlobally = false)
        public static IOASISStorage SetAndActivateCurrentStorageProvider(ProviderType providerType, bool setGlobally = false)
        {
            //TODO: Need to get this to use the next provider in the list if there is an issue with the first/current provider...
            if (providerType == ProviderType.Default && !OverrideProviderType)
                providerType = (ProviderType)Enum.Parse(typeof(ProviderType), DefaultProviderTypes[0]);

            if (providerType != CurrentStorageProviderType)
            {
                IOASISProvider provider = _registeredProviders.FirstOrDefault(x => x.ProviderType == providerType);

                if (provider == null)
                    throw new InvalidOperationException(string.Concat(Enum.GetName(typeof(ProviderType), providerType), " ProviderType is not registered. Please call RegisterProvider() method to register the provider before calling this method."));

                if (provider != null && (provider.ProviderCategory == ProviderCategory.Storage || provider.ProviderCategory == ProviderCategory.StorageAndNetwork))
                {
                    CurrentStorageProviderType = providerType;
                    CurrentStorageProvider = (IOASISStorage)provider;
                    CurrentStorageProvider.ActivateProvider();

                    if (setGlobally)
                        DefaultGlobalStorageProvider = CurrentStorageProvider;
                }
            }

            return CurrentStorageProvider;
        }

        public static void ActivateProvider(ProviderType type)
        {
            IOASISProvider provider = _registeredProviders.FirstOrDefault(x => x.ProviderType == type);

            if (provider != null)
                provider.ActivateProvider();
        }

        public static void DeaAtivateProvider(ProviderType type)
        {
            IOASISProvider provider = _registeredProviders.FirstOrDefault(x => x.ProviderType == type);

            if (provider != null)
                provider.DeActivateProvider();
        }
    }
}
