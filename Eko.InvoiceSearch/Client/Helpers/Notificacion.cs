using Microsoft.AspNetCore.Components;
using Radzen;
using System.Net.Http.Headers;

namespace Eko.InvoiceSearch.Client.Helpers
{
	public class Notificacion
	{
		public NotificationMessage MensajeOk(string mensaje = "") 
		{
			string mDefault = "Proceso satisfactorio";
			NotificationMessage notification = new NotificationMessage();
			try
			{
				notification.Severity = NotificationSeverity.Success;
				notification.Summary = "Ok";
				notification.Detail = mensaje == "" ? mDefault : mensaje;
				notification.Duration = 4000;
				notification.CloseOnClick = true;
			}
			catch (Exception ex)
			{
			}
			return notification;
		}

		public NotificationMessage MensajeError(string mensaje = "") 
		{
			string mDefault = "Proceso no finalizado";
			NotificationMessage notification = new NotificationMessage();
			try
			{
				notification.Severity = NotificationSeverity.Error;
				notification.Summary = "Error";
				notification.Detail = mensaje == "" ? mDefault : mensaje;
				notification.Duration = 4000;
				notification.CloseOnClick = true;
			}
			catch (Exception ex)
			{
			}
			return notification;
		}
		public NotificationMessage MensajeAlerta(string mensaje = "" )
		{
			string mDefault = "Validar proceso";
			NotificationMessage notification = new NotificationMessage();
			try
			{
				notification.Severity = NotificationSeverity.Warning;
				notification.Summary = "Alerta";
				notification.Detail = mensaje == "" ? mDefault : mensaje;
				notification.Duration = 4000;
				notification.CloseOnClick = true;
			}
			catch (Exception ex)
			{
			}
			return notification;
		}
		public NotificationMessage MensajeInfo(string mensaje = "")
		{
			string mDefault = "Validar información";
			NotificationMessage notification = new NotificationMessage();
			try
			{
				notification.Severity = NotificationSeverity.Warning;
				notification.Summary = "Información";
				notification.Detail = mensaje == "" ? mDefault : mensaje;
				notification.Duration = 4000;
				notification.CloseOnClick = true;
			}
			catch (Exception ex)
			{
			}
			return notification;	
		}
	}
}
