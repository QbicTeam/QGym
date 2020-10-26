using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Payment
{
    // Esta es la clase que se debe encriptar
    public class PaymentBankRequestDto
    {
        /// <summary>
        /// Email donde se mandara la notificacion.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Titular de la tarjeta... Osea... Nombre completo.
        /// </summary>
        public string Cardholder { get; set; }
        /// <summary>
        /// Numero de Tarjeta.
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// Solo Importa Año y Mes.
        /// </summary>
        public DateTime ExpirationDate { get; set; }
        /// <summary>
        /// Codigo de Verificacion.
        /// </summary>
        public int CCV { get; set; }
        /// <summary>
        /// Monto de la Compra.
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// Moneda de la Transaccion... Desconozco como se manda.
        /// </summary>
        public string Currency { get; set; }

	}
}
