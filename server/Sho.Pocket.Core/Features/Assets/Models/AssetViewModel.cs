using System;
using System.Collections.Generic;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Assets.Models
{
    public class AssetViewModel : IEquatable<AssetViewModel>
    {
        public AssetViewModel()
        {
        }

        public AssetViewModel(Asset asset)
        {
            Id = asset.Id;
            Name = asset.Name;
            Currency = asset.Balance.Currency;
            IsActive = asset.IsActive;
            Value = asset.Balance.Value;
            UpdatedOn = asset.UpdatedOn;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public bool IsActive { get; set; }

        public decimal Value { get; set; }

        public DateTime UpdatedOn { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as AssetViewModel);
        }

        public bool Equals(AssetViewModel other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   Name.Equals(other.Name, StringComparison.Ordinal) &&
                   Currency.Equals(other.Currency, StringComparison.Ordinal) &&
                   IsActive == other.IsActive &&
                   Value == other.Value &&
                   UpdatedOn == other.UpdatedOn;
        }

        public override int GetHashCode()
        {
            int hashCode = 438930606;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Currency);
            hashCode = hashCode * -1521134295 + IsActive.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + UpdatedOn.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(AssetViewModel left, AssetViewModel right)
        {
            return EqualityComparer<AssetViewModel>.Default.Equals(left, right);
        }

        public static bool operator !=(AssetViewModel left, AssetViewModel right)
        {
            return !(left == right);
        }
    }
}