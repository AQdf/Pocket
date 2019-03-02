using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Currencies.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Assets
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public AssetService(
            IAssetRepository assetRepository,
            ICurrencyRepository currencyRepository,
            IMapper mapper)
        {
            _assetRepository = assetRepository;
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public IEnumerable<AssetViewModel> GetAll()
        {
            List<Asset> assets = _assetRepository.GetAll();

            List<AssetViewModel> result = _mapper.Map<List<AssetViewModel>>(assets);

            return result;
        }

        public void Add(AssetViewModel assetModel)
        {
            Asset asset = _mapper.Map<Asset>(assetModel);

            _assetRepository.Add(asset);
        }

        public void Update(AssetViewModel assetModel)
        {
            Asset asset = _mapper.Map<Asset>(assetModel);

            _assetRepository.Update(asset);
        }

        public void Delete(Guid Id)
        {
            _assetRepository.Remove(Id);
        }

        public List<CurrencyViewModel> GetCurrencies()
        {
            List<Currency> currencies = _currencyRepository.GetAll();

            List<CurrencyViewModel> result = _mapper.Map<List<CurrencyViewModel>>(currencies);

            return result;
        }
    }
}
