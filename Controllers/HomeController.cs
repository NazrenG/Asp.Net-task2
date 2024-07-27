using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication2.Entities;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
	public class HomeController : Controller
	{
		public static List<Product> Products { get; set; } = new List<Product>
		{
			new Product()
			{
				Id= 1,
				Name="Gaming Mouse",
				Description="Precision gaming mouse with customizable DPI settings, RGB lighting, and programmable buttons.",
				Price=49.99,
				ImagePath=$"/images/mouse.jpg",
				Discount=20,
			},

			new Product()
			{
				Id= 2,
				Name="Laptop Stand",
				Description="Ergonomic laptop stand with adjustable height and angle, made from lightweight aluminum.",
				Price=399.99,
				ImagePath=$"/images/laptopStand.jpg",
				Discount=5,
			},
			new Product()
			{
				Id= 3,
				Name="Smartwatch",
				Description="A versatile smartwatch with fitness tracking, heart rate monitoring, and customizable watch faces.",
				Price=132.99,
				ImagePath=$"/images/watch.jpg",
				Discount=10,
			}


		};

		[HttpGet]
		public IActionResult Index()
		{
			var vm = new ProductViewModel
			{
				Products = Products
			};


			return View(vm);
		}
		[HttpGet]
		public IActionResult Add()
		{
			var newProduct = new ProductAddViewModel
			{
				Product = new Product()
			};


			return View(newProduct);
		}
		[HttpGet]
		public IActionResult Delete(int id)
		{
			var item = Products.SingleOrDefault(i => i.Id == id);
			Products.Remove(item);
			return RedirectToAction("Index");
		}

		private readonly IHttpClientFactory _clientFactory;
		private readonly IWebHostEnvironment _env;

		public HomeController(IHttpClientFactory clientFactory, IWebHostEnvironment env)
		{
			_clientFactory = clientFactory;
			_env = env;
		}

		[HttpPost]
		public async Task<IActionResult> Add(ProductAddViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (!string.IsNullOrEmpty(model.ImageUrl))
				{
					var client = _clientFactory.CreateClient();
					var response = await client.GetAsync(model.ImageUrl);

					if (response.IsSuccessStatusCode)
					{
						var imageBytes = await response.Content.ReadAsByteArrayAsync();
						var imageName = Path.GetFileName(new Uri(model.ImageUrl).AbsolutePath);
						var imagePath = Path.Combine(_env.WebRootPath, "images", imageName);

						await System.IO.File.WriteAllBytesAsync(imagePath, imageBytes);

						model.Product.ImagePath = $"/images/{imageName}";
						model.Product.Id = new Random().Next(100, 1000);
						Products.Add(model.Product);
					}
				}



				return RedirectToAction("Index");
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult Update(int id)
		{
			var item = Products.SingleOrDefault(i => i.Id == id); 
			if (item == null)
			{

				return NotFound();
			}

			var vm = new ProductUpdateViewModel
			{
				Product = item
			};


			return View(vm);
		}
		[HttpPost]
		 
		public IActionResult Update(ProductUpdateViewModel vm,int id)
		{  
			var item = Products.SingleOrDefault(i => i.Id == id);
			if (ModelState.IsValid)
			{
				item.Name= vm.Product.Name;
				item.Description= vm.Product.Description;
				item.Price= vm.Product.Price;
				item.Discount= vm.Product.Discount;
				item.ImagePath= vm.Product.ImagePath;
				return RedirectToAction("Index");
			}
			return View(vm);
		}

	}
}
