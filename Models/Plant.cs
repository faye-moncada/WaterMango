using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterMango.Models
{
	public class Plant
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Status { 
			get {
				if (DateTime.Now.AddHours(-6) > LastWatered)
					return "icon-red";
				else if (DateTime.Now.AddSeconds(-10) < LastWatered)
					return "icon-darkblue";
				else if (DateTime.Now.AddSeconds(-30) < LastWatered)
					return "icon-gray";
				else
					return "icon-blue";
			} 
		}
		public DateTime LastWatered { get; set; }
		public string Image { get; set; }
	}
}