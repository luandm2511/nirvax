using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class GuestConsultationDTO
    {
        public int GuestId { get; set; }

        public string Fullname { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Content { get; set; } = null!;

        public int StatusGuestId { get; set; }

        public int AdId { get; set; }

        public int OwnerId { get; set; }
    }

    public class GuestConsultationCreateDTO
    {
       

        public string Fullname { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Content { get; set; } = null!;

        public int StatusGuestId { get; set; }

        public int AdId { get; set; }

        public int OwnerId { get; set; }
    }
}
