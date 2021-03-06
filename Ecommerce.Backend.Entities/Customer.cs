using System;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Backend.Entities
{
  public class CustomerAuth
  {
    [BsonRequired]
    [BsonElement("password")]
    public string Password { get; set; } // hashed password

    [BsonRequired]
    [BsonElement("salt")]
    public string Salt { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("passwordResetToken")]
    public string PasswordResetToken { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("passwordResetExpires")]
    public DateTime? PasswordResetExpires { get; set; }
  }

  public class Customer : BaseEntityWithStatus
  {
    [BsonElement("phoneNo")]
    [BsonRequired]
    public string PhoneNo { get; set; }

    [BsonElement("fullName")]
    [BsonRequired]
    public string FullName { get; set; }

    [JsonIgnore]
    [BsonElement("auth")]
    [BsonRequired]
    public CustomerAuth Auth { get; set; }

    [BsonElement("email")]
    [BsonIgnoreIfNull]
    public string Email { get; set; }

    [BsonElement("billingAddress")]
    [BsonIgnoreIfNull]
    public BillingAddress BillingAddress { get; set; }

    [BsonElement("shippingAddress")]
    [BsonIgnoreIfNull]
    public ShippingAddress ShippingAddress { get; set; }

    [BsonElement("avatarUrl")]
    [BsonIgnoreIfNull]
    public string AvatarUrl { get; set; }

    /// <summary>
    /// Indicates profile completeness in % (percentage)
    /// </summary>
    [BsonElement("profileCompleteness")]
    [BsonIgnoreIfNull]
    public int ProfileCompleteness { get; set; }
  }

  public class BillingAddress
  {
    [BsonElement("phoneNo")]
    [BsonRequired]
    public string PhoneNo { get; set; }

    [BsonElement("email")]
    [BsonIgnoreIfNull]
    public string Email { get; set; }

    [BsonElement("fullName")]
    [BsonRequired]
    public string FullName { get; set; }

    [BsonElement("country")]
    [BsonRequired]
    public string Country { get; set; }

    [BsonElement("state")]
    [BsonRequired]
    public string State { get; set; }

    [BsonElement("address")]
    [BsonRequired]
    public string Address { get; set; }

    [BsonElement("city")]
    [BsonRequired]
    public string City { get; set; }

    [BsonElement("postalCode")]
    [BsonRequired]
    public string PostalCode { get; set; }
  }

  public class ShippingAddress : BillingAddress
  {
    [BsonElement("sameToBillingAddress")]
    [BsonRequired]
    public bool SameToBillingAddress { get; set; }
  }
}