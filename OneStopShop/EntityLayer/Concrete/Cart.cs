using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Cart
    {


        [Key]
        public int CartId { get; set; }
        public int Quantity { get; set; }
     
        //User
       // public ApplicationUser User{ get; set; }


        //Product
        public int ProductId { get; set; }
        public Product Product{ get; set; }

        //Basket
        public int? BasketId { get; set; }
        public Basket Basket { get; set; }

        [Required]
        public virtual int CargoTypeId
        {
            get
            {
                return (int)this.CargoType;
            }
            set
            {
                CargoType = (CargoTypes)value;
            }
        }

        [EnumDataType(typeof(CargoTypes))]
        public CargoTypes CargoType { get; set; }

        public enum CargoTypes
        {
            Getting_ready = 0,
            Send_by_cargo = 1,
            Set_out = 2,
            Distribution = 3,
            Delivered = 4
        }
    }
}
