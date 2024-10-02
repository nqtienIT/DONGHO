using System.ComponentModel.DataAnnotations;

namespace CuaHangDongHo.Models
{
    public class VMContact
    {
        public string ResponsibleShop { get; set; }

        public string MapShop { get; set; }

        public string PhoneShop { get; set; }

        public string EmailShop { get; set; }

        public string AddressShop { get; set; }

        public string FaxShop { get; set; }

        public string FacebookShop { get; set; }

        public string TwitterShop { get; set; }

        public string ZaloShop { get; set; }

        [Required]
        [StringLength(255)]
        public string FullNameContact { get; set; }

        [Required]
        [StringLength(255)]
        public string EmailContact { get; set; }

        [Required]
        [StringLength(15)]
        public string PhoneContact { get; set; }

        [Required]
        [StringLength(255)]
        public string TitleContact { get; set; }

        [Required]
        public string DetailContact { get; set; }
    }
}