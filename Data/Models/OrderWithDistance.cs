using System.Runtime.Serialization;

namespace HandleHjelp.Data.Models
{
    public class OrderWithDistance : Order
    {
        public OrderWithDistance(Order order)
        {
            OrderId = order.OrderId;
            OrderDate = order.OrderDate;
            CustomerId = order.CustomerId;
            Latitude = order.Latitude;
            Longitude = order.Longitude;
            Image = order.Image;
            City = order.City;
            CountryName = order.CountryName;
            CountryCode = order.CountryCode;
            TotalAmount = order.TotalAmount;
            CreatedDate = order.CreatedDate;
            CreatedBy = order.CreatedBy;
            UpdatedBy = order.UpdatedBy;
            UpdatedDate = order.UpdatedDate;
            Note = order.Note;
            PostalCode = order.PostalCode;
            PhoneNumber = order.PhoneNumber;
            ReceivedOn = order.ReceivedOn;
            TotalAmount = order.TotalAmount;
        }

        public double DistanceInKm { get; set; }
    }
}
