/*
 * Golem Market API
 *
 * Market API
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GolemMarketMockAPI.MarketAPI.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class NewAgreementEvent : ProviderEvent, IEquatable<NewAgreementEvent>
    { 
        /// <summary>
        /// Gets or Sets AgreementId
        /// </summary>
        [DataMember(Name="agreementId")]
        public string AgreementId { get; set; }

        /// <summary>
        /// Gets or Sets RequestorId
        /// </summary>
        [DataMember(Name = "requestorId")]
        public string RequestorId { get; set; }

        /// <summary>
        /// Gets or Sets Demand
        /// </summary>
        [DataMember(Name="demand")]
        public Demand Demand { get; set; }

        /// <summary>
        /// Gets or Sets ProviderId
        /// </summary>
        [DataMember(Name="providerId")]
        public string ProviderId { get; set; }

        /// <summary>
        /// Gets or Sets Offer
        /// </summary>
        [DataMember(Name="offer")]
        public Offer Offer { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class NewAgreementEvent {\n");
            sb.Append("  AgreementId: ").Append(AgreementId).Append("\n");
            sb.Append("  RequestorId: ").Append(RequestorId).Append("\n");
            sb.Append("  Demand: ").Append(Demand).Append("\n");
            sb.Append("  ProviderId: ").Append(ProviderId).Append("\n");
            sb.Append("  Offer: ").Append(Offer).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public  new string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((NewAgreementEvent)obj);
        }

        /// <summary>
        /// Returns true if NewAgreementEvent instances are equal
        /// </summary>
        /// <param name="other">Instance of NewAgreementEvent to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(NewAgreementEvent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    AgreementId == other.AgreementId ||
                    AgreementId != null &&
                    AgreementId.Equals(other.AgreementId)
                ) &&
                (
                    RequestorId == other.RequestorId ||
                    RequestorId != null &&
                    RequestorId.Equals(other.RequestorId)
                ) && 
                (
                    Demand == other.Demand ||
                    Demand != null &&
                    Demand.Equals(other.Demand)
                ) && 
                (
                    ProviderId == other.ProviderId ||
                    ProviderId != null &&
                    ProviderId.Equals(other.ProviderId)
                ) && 
                (
                    Offer == other.Offer ||
                    Offer != null &&
                    Offer.Equals(other.Offer)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (AgreementId != null)
                    hashCode = hashCode * 59 + AgreementId.GetHashCode();
                if (RequestorId != null)
                    hashCode = hashCode * 59 + RequestorId.GetHashCode();
                    if (Demand != null)
                    hashCode = hashCode * 59 + Demand.GetHashCode();
                    if (ProviderId != null)
                    hashCode = hashCode * 59 + ProviderId.GetHashCode();
                    if (Offer != null)
                    hashCode = hashCode * 59 + Offer.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(NewAgreementEvent left, NewAgreementEvent right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NewAgreementEvent left, NewAgreementEvent right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
