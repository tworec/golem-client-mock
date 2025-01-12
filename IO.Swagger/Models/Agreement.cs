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

namespace IO.Swagger.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Agreement : IEquatable<Agreement>
    { 
        /// <summary>
        /// Gets or Sets ProposalId
        /// </summary>
        [Required]
        [DataMember(Name="proposalId")]
        public string ProposalId { get; set; }

        /// <summary>
        /// Gets or Sets ExpirationDate
        /// </summary>
        [Required]
        [DataMember(Name="expirationDate")]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Agreement {\n");
            sb.Append("  ProposalId: ").Append(ProposalId).Append("\n");
            sb.Append("  ExpirationDate: ").Append(ExpirationDate).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
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
            return obj.GetType() == GetType() && Equals((Agreement)obj);
        }

        /// <summary>
        /// Returns true if Agreement instances are equal
        /// </summary>
        /// <param name="other">Instance of Agreement to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Agreement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    ProposalId == other.ProposalId ||
                    ProposalId != null &&
                    ProposalId.Equals(other.ProposalId)
                ) && 
                (
                    ExpirationDate == other.ExpirationDate ||
                    ExpirationDate != null &&
                    ExpirationDate.Equals(other.ExpirationDate)
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
                    if (ProposalId != null)
                    hashCode = hashCode * 59 + ProposalId.GetHashCode();
                    if (ExpirationDate != null)
                    hashCode = hashCode * 59 + ExpirationDate.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Agreement left, Agreement right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Agreement left, Agreement right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
