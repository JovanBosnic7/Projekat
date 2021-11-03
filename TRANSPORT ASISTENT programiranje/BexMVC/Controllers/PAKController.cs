using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;



using PagedList;

using Bex.Common;
using Bex.MVC.Exceptions;
using Bex.DAL.EF.UOW;
using BexMVC.ViewModels;
using Bex.Models;
using Bex.ViewModels;
using BexMVC.Helpers;
using System.Data.Entity.Core;
using BexMVC.Filters;

namespace EFproject.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "Pak", Operation = "Read, All")]
    public class PAKController : Controller
    {
        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }

        public PAKController() : this(new BexUow(), new ExceptionSolver())
        { }
        public PAKController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }

        public ActionResult Index()
        {
                     
            TempData.ToList().ForEach(keyValue =>
               ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();

            return View();
   
        }

        [HttpGet]
        public ActionResult GetAdrese(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var skip = (pageNumber - 1) * pageSize;
            var total = 0;

            if (!String.IsNullOrEmpty(searchTerms))
            {
                total = BexUow.Addresses.GetSearchAddressData(searchTerms).Count();

                var addressData = BexUow.Addresses.GetSearchAddressData(searchTerms).Select(x =>
                                                         new AddressIndexData
                                                         {
                                                             Pak = x.Id,
                                                             NazivUlice = x.Street?.StreetName,
                                                             OznReona = x.Reon?.NazivReona,
                                                             NazivMesta = x.Place?.PlaceName,
                                                             NazivReoncica = x.Reoncic?.NazivReoncica,
                                                             NazivSkraceniRegiona = x.Region?.NazivSkraceni,
                                                             OdBroja = x.OdBroja,
                                                             DoBroja = x.DoBroja
                                                         });
                if (sortOrder.Equals("desc"))
                {
                    addressData = addressData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "Pak" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }
                else
                {
                    addressData = addressData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Pak" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }

                var jsonData = new TableJsonIndexData<AddressIndexData>()
                {
                    total = total,
                    rows = addressData
                };

                var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
           

           
            return null;

        }


        [ClaimsAuthentication(Resource = "Pak", Operation = "Create, All")]
        public ActionResult Create(string id)
        {
            ViewBag.UlicaId = new SelectList(BexUow.Street.AllAsNoTracking, "Id", "StreetName");
            ViewBag.MestoId = new SelectList(BexUow.Place.AllAsNoTracking, "Id", "PlaceName");
            ViewBag.OpstinaId = new SelectList(BexUow.Opstina.AllAsNoTracking, "Id", "OpstinaNaziv");
            ViewBag.ReoncicId = new SelectList(BexUow.Reoncic.AllAsNoTracking, "Id", "NazivReoncica");
            ViewBag.ReonId = new SelectList(BexUow.Reon.AllAsNoTracking, "Id", "OznReona");
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PAK pak)
        {
            if (ModelState.IsValid)
            {
                BexUow.Addresses.Add(pak);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("../Pak"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            ViewBag.UlicaId = new SelectList(BexUow.Street.AllAsNoTracking, "Id", "StreetName");
            ViewBag.MestoId = new SelectList(BexUow.Place.AllAsNoTracking, "Id", "PlaceName");
            ViewBag.OpstinaId = new SelectList(BexUow.Opstina.AllAsNoTracking, "Id", "OpstinaNaziv");
            ViewBag.ReoncicId = new SelectList(BexUow.Reoncic.AllAsNoTracking, "Id", "NazivReoncica");
            ViewBag.ReonId = new SelectList(BexUow.Reon.AllAsNoTracking, "Id", "OznReona");
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
            return View(pak);

        }

        [ClaimsAuthentication(Resource = "Pak", Operation = "Edit, All")]
        public ActionResult Edit(string id)
        {
            ViewBag.PakId = id;
            return View();
            // ovo bi trebalo da se uvrsti u sve metode jer je Guid jedna vrsta zastite

            //if (!String.IsNullOrWhiteSpace(id))
            //{
            //    bool isGuid = Guid.TryParse(id, out Guid customerId);
            //    if (isGuid && customerId != Guid.Empty)
            //    {
            //        return View();
            //    }
            //}
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult GetUlica(string query, string mestoId)
        {
            return Json(_GetUlica(query, mestoId), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetUlica(string query, string mestoId)
        {
            List<Autocomplete> ulicaList = new List<Autocomplete>();
            try
            {
                int mestoIdInt = System.Convert.ToInt32(mestoId);
                var results = BexUow.Street.GetAll(true).Where(m => m.StreetName.Contains(query) && m.PlaceId == mestoIdInt).Take(10).ToList();

                foreach (var r in results)
                {
                    Autocomplete ulica = new Autocomplete();
                    ulica.Name = r.StreetName;
                    ulica.Id = r.Id;
                    ulicaList.Add(ulica);
                }

            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }
            catch
            {
                throw;
            }
            return ulicaList;
        }

        public ActionResult GetMestaZaDodavanje(string query, string opstinaId)
        {

            return Json(_GetMestaZaDodavanje(query, opstinaId), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetMestaZaDodavanje(string query, string opstinaId)
        {
            List<Autocomplete> mestaList = new List<Autocomplete>();
            try
            {
                int opstinaIdInt = System.Convert.ToInt32(opstinaId);
                    var results = BexUow.Place.GetAll(true).Where(m => m.PlaceName.Contains(query) && m.OpstinaId == opstinaIdInt).Take(10).ToList();
                    foreach (var r in results)
                    {
                        Autocomplete mesta = new Autocomplete();
                        mesta.Name = r.PlaceName;
                        mesta.Id = r.Id;
                        mestaList.Add(mesta);
                    }
               


                

            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }
            catch
            {
                throw;
            }
            return mestaList;
        }


       

        public ActionResult GetOpstina(string query)
        {
            return Json(_GetOpstina(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetOpstina(string query)
        {
            List<Autocomplete> opstinaList = new List<Autocomplete>();
            try
            {
                var results = BexUow.Opstina.GetAll(true).Where(m => m.OpstinaNaziv.Contains(query)).Take(10).ToList();

                foreach (var r in results)
                {
                    Autocomplete opstina = new Autocomplete();
                    opstina.Name = r.OpstinaNaziv;
                    opstina.Id = r.Id;
                    opstinaList.Add(opstina);
                }

            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }
            catch
            {
                throw;
            }
            return opstinaList;
        }

        public ActionResult GetReoncic(string query)
        {
            return Json(_GetReoncic(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetReoncic(string query)
        {
            List<Autocomplete> reoncicList = new List<Autocomplete>();
            try
            {
                var results = BexUow.Reoncic.GetAll(true).Where(m => m.NazivReoncica.Contains(query)).Take(10).ToList();

                foreach (var r in results)
                {
                    Autocomplete reoncic = new Autocomplete();
                    reoncic.Name = r.NazivReoncica;
                    reoncic.Id = r.Id;
                    reoncicList.Add(reoncic);
                }

            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }
            catch
            {
                throw;
            }
            return reoncicList;
        }

        public ActionResult GetReon(string query)
        {
            return Json(_GetReon(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetReon(string query)
        {
            List<Autocomplete> reonList = new List<Autocomplete>();
            try
            {
                var results = BexUow.Reon.GetAll(true).Where(m => m.NazivReona.Contains(query)).Take(10).ToList();

                foreach (var r in results)
                {
                    Autocomplete reon = new Autocomplete();
                    reon.Name = r.NazivReona;
                    reon.Id = r.Id;
                    reonList.Add(reon);
                }

            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }
            catch
            {
                throw;
            }
            return reonList;
        }

        public ActionResult GetRegion(string query)
        {
            return Json(_GetRegion(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetRegion(string query)
        {
            List<Autocomplete> regionList = new List<Autocomplete>();
            try
            {
                var results = BexUow.Region.GetAll(true).Where(m => m.NazivSkraceni.Contains(query)).Take(10).ToList();

                foreach (var r in results)
                {
                    Autocomplete region = new Autocomplete();
                    region.Name = r.NazivSkraceni;
                    region.Id = r.Id;
                    regionList.Add(region);
                }

            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }
            catch
            {
                throw;
            }
            return regionList;
        }

        [ChildActionOnly]
        public ActionResult EditAddressPartial(string pakId)
        {

            PAK pak = BexUow.Addresses.GetEditAddress(System.Int32.Parse(pakId));
            var addressEditViewModel = new AddressEditViewModel()
            {
                PakBroj = pak.Id.ToString(),
                SelectedMesto = pak.Place.PlaceName,
                SelectedUlica = pak.Street.StreetName
            };


            addressEditViewModel.Mesta = GetMesta(pak.MestoId); 
            addressEditViewModel.Ulice = GetUliceData(pak.MestoId);


            return View(addressEditViewModel);

          
        }

        public IEnumerable<SelectListItem> GetMesta(int? mestoId)
        {
            List<SelectListItem> mesta = BexUow.Place.GetAll(true)
                                                         .OrderBy(n => n.PlaceName)
                                                         .Select(n =>
                                                            new SelectListItem
                                                            {
                                                                Value = n.Id.ToString(),
                                                                Text =  n.PlaceName
                                                            }).ToList();
            var mestaFirstRow = new SelectListItem()
            {
                Value = null,
                Text = "--- izaberite mesto ---"
            };

           
            //mesta.Insert(0, mestaFirstRow);
            return new SelectList(mesta, "Value", "Text", mestoId.ToString());
            
        }

        //public IEnumerable<SelectListItem> GetUlice()
        //{
        //    List<SelectListItem> ulice = new List<SelectListItem>()
        //    {
        //        new SelectListItem
        //        {
        //            Value = null,
        //            Text = " "
        //        }
        //    };
        //    return ulice;
        //}

        /* zahtev za ulice za punjenje cmb-a */
        [HttpGet]
        public ActionResult GetUlice(int? mestoId)
        {
            IEnumerable<SelectListItem> ulice = GetUliceData(mestoId);
            return Json(ulice, JsonRequestBehavior.AllowGet);
        }


        /* punjenje cmb-a ulicama na osnovu selektovanog mesta */
        public IEnumerable<SelectListItem> GetUliceData(int? mestoId)
        {
            
            IEnumerable<SelectListItem> ulice = BexUow.Street.GetAll(true)
                                                      .OrderBy(n => n.StreetName)
                                                      .Where(n => n.PlaceId == mestoId)
                                                      .Select(n =>
                                                           new SelectListItem
                                                           {
                                                               Value = n.Id.ToString(),
                                                               Text = n.StreetName
                                                           }).ToList();
            return new SelectList(ulice, "Value", "Text");
                
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAddressPartial(AddressEditViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                bool saved = SavePAK(model);
                if (saved)
                {
                   var modelUpdate = BexUow.Addresses.GetEditAddress(272432);
                   return PartialView(modelUpdate);
                    
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public bool SavePAK(AddressEditViewModel pakedit)
        {
            if (pakedit != null)
            {
               
                        var pak = new PAK()
                        {
                            //CustomerID = newGuid,
                            //CustomerName = customeredit.CustomerName,
                            //CountryIso3 = customeredit.SelectedCountryIso3,
                            //RegionCode = customeredit.SelectedRegionCode
                        };
                        //customer.Country = context.Countries.Find(customeredit.SelectedCountryIso3);
                        //customer.Region = context.Regions.Find(customeredit.SelectedRegionCode);

                BexUow.Addresses.Add(pak);

                var commandResult = BexUow.SubmitChanges();

                //if (commandResult.IsSuccessful)
                //{ return RedirectToAction("Details", new { id = pak.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);

                //context.Customers.Add(customer);
                //context.SaveChanges();
                return true;
                    
            }
            // Return false if customeredit == null or CustomerID is not a guid
            return false;
        }


        [ChildActionOnly]
        public ActionResult AddressTypePartial(string id)
        {
            var model = new AddressTypeViewModel()
            {
                pakId = id,
                AddressTypes = GetMesta(4240)
            };
            return PartialView("AddressTypePartial", model);
            //if (!String.IsNullOrWhiteSpace(id))
            //{
            //    bool isGuid = Guid.TryParse(id, out Guid customerId);
            //    if (isGuid && customerId != Guid.Empty)
            //    {
            //        //var repo = new MetadataRepository();
            //        var model = new AddressTypeViewModel()
            //        {
            //            pakId = id,
            //            AddressTypes = GetMesta(22)
            //        };
            //        return PartialView("AddressTypePartial", model);
            //    }
            //}
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddressTypePartial(AddressTypeViewModel model)
        {
            var postalAddressModel = new StreetEditViewModel()
            {
                PakId = model.pakId
            };
            //var countriesRepo = new CountriesRepository();
            postalAddressModel.Countries = GetMesta(22);
            //var regionsRepo = new RegionsRepository();
            postalAddressModel.Regions = GetUliceData(2);
            return PartialView("CreateStreetPartial", postalAddressModel); //CreatePostalAddressPartial
            //if (ModelState.IsValid && !String.IsNullOrWhiteSpace(model.pakId))
            //{
            //switch (model.SelectedAddressType)
            //{
            //    //case "Email":
            //    //    var emailAddressModel = new EmailAddressViewModel()
            //    //    {
            //    //        CustomerID = model.CustomerID
            //    //    };
            //    //    return PartialView("CreateEmailAddressPartial", emailAddressModel);
            //    case "Postal":
            //        var postalAddressModel = new StreetEditViewModel()
            //        {
            //            PakId = model.pakId
            //        };
            //        //var countriesRepo = new CountriesRepository();
            //        postalAddressModel.Countries = GetMesta(22);
            //        //var regionsRepo = new RegionsRepository();
            //        postalAddressModel.Regions = GetUliceData(2);
            //        return PartialView("CreatePostalAddressPartial", postalAddressModel);
            //    default:
            //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //}
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateStreetPartial(StreetEditViewModel model)
        {

            return RedirectToAction("Edit", new { id = model.PakId });

            //if (ModelState.IsValid)
            //{
            //    var repo = new CustomersRepository();
            //    var updatedModel = repo.SavePostalAddress(model);
            //    if (updatedModel != null)
            //    {
            //        return RedirectToAction("Edit", new { id = model.CustomerID });
            //    }
            //}
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        // GET: /PAK/Details/5

        public ViewResult Details(int id)
        {
            PAK pak = null;
            //addressRepository.GetPakByID(id);
            return View(pak);
        }

        //
        //// GET: /PAK/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /PAK/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,ReoncicId,ReonId,RegionId,OpstinaId,MestoId,UlicaId,OdBroja,DoBroja,Parnost,PttId,PakZaStampu,Napomena,UserUnosId,UserIzmeneId,DI")] PAK pAK)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //addressRepository.InsertPAk(pAK);
        //            //addressRepository.Save();
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (DataException /* dex */)
        //    {
        //        //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
        //        ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
        //    }
        //    return View(pAK);
        //}

        //
        // POST: /PAK/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,ReoncicId,ReonId,RegionId,OpstinaId,MestoId,UlicaId,OdBroja,DoBroja,Parnost,PttId,PakZaStampu,Napomena,UserUnosId,UserIzmeneId,DI")] PAK pAK)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //addressRepository.UpdatePak(pAK);
        //            //addressRepository.Save();
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (DataException /* dex */)
        //    {
        //        //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
        //        ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
        //    }
        //    return View(pAK);
        //}



        //
        // GET: /PAK/Delete/5

        [ClaimsAuthentication(Resource = "Pak", Operation = "Delete, All")]
        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            PAK pak = null;
                //addressRepository.GetPakByID(id);
            return View(pak);
        }

        //
        // POST: /Student/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                PAK pak = null;
                    //addressRepository.GetPakByID(id);
                //addressRepository.DeletePAK(id);
                //addressRepository.Save();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //addressRepository.Dispose();
            base.Dispose(disposing);
        }


        /*private BexMainDbContext db = new BexMainDbContext();

        // GET: PAK
        public ActionResult Index()
        {
            var pAKs = db.PAKs.Include(p => p.Reon).Include(p => p.Reoncic).Include(p => p.Street).Take(100);
            return View(pAKs.ToList());
        }

        // GET: PAK/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PAK pAK = db.PAKs.Find(id);
            if (pAK == null)
            {
                return HttpNotFound();
            }
            return View(pAK);
        }

        // GET: PAK/Create
        public ActionResult Create()
        {
            ViewBag.ReonId = new SelectList(db.Reons, "Id", "OznReona");
            ViewBag.ReoncicId = new SelectList(db.Reoncics, "Id", "NazivReoncica");
            ViewBag.UlicaId = new SelectList(db.Streets, "Id", "StreetName");
            return View();
        }

        // POST: PAK/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ReoncicId,ReonId,RegionId,OpstinaId,MestoId,UlicaId,OdBroja,DoBroja,Parnost,PttId,PakZaStampu,Napomena,UserUnosId,UserIzmeneId,DI")] PAK pAK)
        {
            if (ModelState.IsValid)
            {
                db.PAKs.Add(pAK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReonId = new SelectList(db.Reons, "Id", "OznReona", pAK.ReonId);
            ViewBag.ReoncicId = new SelectList(db.Reoncics, "Id", "NazivReoncica", pAK.ReoncicId);
            ViewBag.UlicaId = new SelectList(db.Streets, "Id", "StreetName", pAK.UlicaId);
            return View(pAK);
        }

        // GET: PAK/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PAK pAK = db.PAKs.Find(id);
            if (pAK == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReonId = new SelectList(db.Reons, "Id", "OznReona", pAK.ReonId);
            ViewBag.ReoncicId = new SelectList(db.Reoncics, "Id", "NazivReoncica", pAK.ReoncicId);
            ViewBag.UlicaId = new SelectList(db.Streets, "Id", "StreetName", pAK.UlicaId);
            return View(pAK);
        }

        // POST: PAK/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReoncicId,ReonId,RegionId,OpstinaId,MestoId,UlicaId,OdBroja,DoBroja,Parnost,PttId,PakZaStampu,Napomena,UserUnosId,UserIzmeneId,DI")] PAK pAK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pAK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReonId = new SelectList(db.Reons, "Id", "OznReona", pAK.ReonId);
            ViewBag.ReoncicId = new SelectList(db.Reoncics, "Id", "NazivReoncica", pAK.ReoncicId);
            ViewBag.UlicaId = new SelectList(db.Streets, "Id", "StreetName", pAK.UlicaId);
            return View(pAK);
        }

        // GET: PAK/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PAK pAK = db.PAKs.Find(id);
            if (pAK == null)
            {
                return HttpNotFound();
            }
            return View(pAK);
        }

        // POST: PAK/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PAK pAK = db.PAKs.Find(id);
            db.PAKs.Remove(pAK);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
