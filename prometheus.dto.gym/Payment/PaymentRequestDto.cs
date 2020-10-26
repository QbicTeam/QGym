using prometheus.dto.gym.Membership;
using prometheus.model.gym;
using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Payment
{
    public class PaymentRequestDto
    {
		/// <summary>
		/// Usuario que esta realizando la compra.
		/// </summary>
        public int UserId { get; set; }
		/// <summary>
		/// Fecha / Hora de la Operacion (solicitud).
		/// </summary>
        public DateTime Date { get; set; }
		/// <summary>
		/// Ultimos 4 dijitos.
		/// </summary>
		public string CardNumber { get; set; }
		/// <summary>
		/// Monto de la Compra.
		/// </summary>
		public decimal Amount { get; set; }
		/// <summary>
		/// Paquete que se esta Pagando.
		/// </summary>
        public int MemberschipTypeId { get; set; }
		/// <summary>
		/// Informacion para el banco (encriptado).
		/// </summary>
        public string ToBank { get; set; }
    }
}
