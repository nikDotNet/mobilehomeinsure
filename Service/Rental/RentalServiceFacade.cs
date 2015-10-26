﻿using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Service.Rental
{
    public class RentalServiceFacade : IRentalServiceFacade
    {
        private readonly mhRentalContext _context;


        public RentalServiceFacade()
        {
            _context = new mhRentalContext();
        }

        public int saveCustomerInformation(string FirstName, string LastName, string FirstName2, string LastName2, string Email, string Password, string Address, int StateId, string City, string Zip, string Phone, int parkId)
        {
            var user = new User
            {
                EmailId = Email,
                Password = Password,
                CreatedDate = DateTime.Now,
                CreatedBy = "admin", //it should be according to logged in user
                IsActive = true
            };

            Customer customerObj = new Customer
            {
                FirstName = FirstName,
                LastName = LastName,
                FirstName2 = FirstName2,
                LastName2 = LastName2,
                Email = Email,
                Address = Address,
                StateId = StateId,
                City = City,
                Zip = Zip,
                Phone = Phone,
                ParkId = parkId,
                CreationDate = DateTime.Now,
                User = user,
                IsActive = true
            };

            _context.Users.Add(user);
            _context.Customers.Add(customerObj);
            _context.SaveChanges();

            return customerObj.Id;
        }

        public decimal generateQuote(DateTime EffectiveDate, decimal PersonalProperty, decimal Deductible, decimal Liability, int CustomerId, int NoOfInstallments, bool SendLandlord, ref int quoteId)
        {
            //Generating proposal number
            string ProposalNo = "ABCD1234";

            decimal Premium = 0;
            Quote quoteObj = null;

            if (quoteId == 0)
            {
                //saving quote generated
                quoteObj = new Quote
               {
                   EffectiveDate = EffectiveDate,
                   PersonalProperty = PersonalProperty,
                   Deductible = Deductible,
                   ProposalNumber = ProposalNo,
                   Liability = Liability,
                   Premium = Premium,
                   CustomerId = CustomerId,
                   NoOfInstallments = NoOfInstallments,
                   CreationDate = DateTime.Now,
                   SendLandLord = SendLandlord,
                   IsActive = true
               };
                _context.Quotes.Add(quoteObj);
            }
            else
            {
                quoteObj = _context.Quotes.Find(quoteId);
                quoteObj.EffectiveDate = EffectiveDate;
                quoteObj.PersonalProperty = PersonalProperty;
                quoteObj.Deductible = Deductible;
                quoteObj.ProposalNumber = ProposalNo;
                quoteObj.Liability = Liability;
                quoteObj.Premium = Premium;
                quoteObj.CustomerId = CustomerId;
                quoteObj.NoOfInstallments = NoOfInstallments;
                quoteObj.CreationDate = DateTime.Now;
                quoteObj.SendLandLord = SendLandlord;
                quoteObj.IsActive = (quoteObj.IsActive ? quoteObj.IsActive : !quoteObj.IsActive);

                _context.Entry(quoteObj).State = System.Data.Entity.EntityState.Modified;
            }

            //Get Premium calculation service result
            var result = new MobileHoome.Insure.ExtService.CalculateHomePremiumService();
            quoteObj.Premium = Premium = result.GetPremiumDetail(quoteObj);
            _context.SaveChanges();


            quoteId = quoteObj.Id;
            return Premium;
        }

        public bool SendLandlord(int quoteId, bool SendLandlord)
        {
            Quote quoteObj = GetQuoteById(quoteId);

            if (quoteObj != null)
            {
                quoteObj.SendLandLord = SendLandlord;
                _context.Entry(quoteObj).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            return false;
        }


        public int generateInvoice(decimal amount, int customerId, int quoteId)
        {
            MobileHome.Insure.Model.Payment paymentObj = new Model.Payment();
            paymentObj.Amount = amount;
            paymentObj.CustomerId = customerId;
            paymentObj.RentalQuoteId = quoteId;

            _context.Payments.Add(paymentObj);
            _context.SaveChanges();

            return paymentObj.Id;
        }

        public bool saveInvoice(int PaymentId, string ResponseCode, string TransactionId, string ApprovalCode, string approvalMessage, string ErrorMessage, DateTime creationDate)
        {
            MobileHome.Insure.Model.Payment paymentObj = _context.Payments.Where(x => x.Id == PaymentId).SingleOrDefault();
            paymentObj.ResponseCode = ResponseCode;
            paymentObj.TransactionId = TransactionId;
            paymentObj.ApprovalCode = ApprovalCode;
            paymentObj.ApprovalMessage = "";
            paymentObj.ErrorMessage = ErrorMessage;
            paymentObj.IsActive = (paymentObj.IsActive ? paymentObj.IsActive : !paymentObj.IsActive);
            paymentObj.CreationDate = DateTime.Now;
            _context.Entry(paymentObj).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return true;

        }


        public List<Customer> GetCustomers()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Customers.AsNoTracking().Where(c => c.IsActive == true).ToList();
        }

        public Customer GetCustomerById(int Id)
        {
            //  _context.Configuration.ProxyCreationEnabled = false;
            return _context.Customers.AsNoTracking().Where(c => c.IsActive == true && c.Id == Id).SingleOrDefault();
        }

        public List<Quote> GetQuotes()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Quotes.AsNoTracking().Where(c => c.IsActive == true).ToList();
        }

        public Quote GetQuoteById(int Id)
        {
            // _context.Configuration.ProxyCreationEnabled = false;
            return _context.Quotes.AsNoTracking().Where(q => q.IsActive == true && q.Id == Id).SingleOrDefault();
        }


        public List<Model.Payment> GetPayments()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Payments.AsNoTracking().Where(c => c.IsActive == true).ToList();
        }

        public bool SaveCustomerInfo(int id, string delType)
        {
            bool result = false;
            switch (delType)
            {
                case "customer":
                    var entityCust = _context.Customers.Where(x => x.Id == id).SingleOrDefault();
                    entityCust.IsActive = false;
                    _context.Entry(entityCust).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    result = true;
                    break;
                case "quote":
                    var entityQuot = _context.Quotes.Where(x => x.Id == id).SingleOrDefault();
                    entityQuot.IsActive = false;
                    _context.Entry(entityQuot).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    result = true;
                    break;
                case "payment":
                    var entityPaym = _context.Payments.Where(x => x.Id == id).SingleOrDefault();
                    entityPaym.IsActive = false;
                    _context.Entry(entityPaym).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    result = true;
                    break;
            }

            return result;
        }

        public bool SaveParkNotify(ParkNotify notify)
        {
            notify.IsNotified = false;
            notify.CreatedOn = DateTime.Now;

            _context.ParkNotifies.Add(notify);
            _context.SaveChanges();
            return true;
        }

        public List<Customer> OwnRentalCustomerByParkId(int parkId)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Customers.AsNoTracking().Where(c => (c.ParkId != null && c.ParkId == parkId)).ToList();
        }
    }
}
