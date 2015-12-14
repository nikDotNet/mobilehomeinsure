using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.DTO;
using MobileHome.Insure.Model.Rental;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
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
              

        #region Invoice
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
            paymentObj.ApprovalMessage = ErrorMessage == string.Empty ? "Approved" : "";
            paymentObj.ErrorMessage = ErrorMessage;
            paymentObj.IsActive = (paymentObj.IsActive ? paymentObj.IsActive : !paymentObj.IsActive);
            paymentObj.CreationDate = DateTime.Now;
            _context.Entry(paymentObj).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return true;

        }
        #endregion

        #region Customer
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

        public List<Customer> GetCustomers()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Customers.AsNoTracking().Where(c => c.IsActive == true).ToList();
        }

        private Customer GetCustomerObject(SearchParameter searchParam)
        {
            Customer CustomerObj = new Customer();
            if (searchParam != null)
            {
                var isFilterValue = searchParam.SearchColumnValue.Any(e => !string.IsNullOrWhiteSpace(e));

                searchParam.IsFilterValue = isFilterValue;

                if ((searchParam.SearchColumn != null && searchParam.SearchColumn.Count > 0) &&
                    searchParam.SearchColumn.Count == searchParam.SearchColumnValue.Count - 1 && isFilterValue) // minus -1 means, skipping action column from search list
                {
                    var filterValueProp = new Dictionary<string, string>();
                    for (int idx = 0; idx < searchParam.SearchColumnValue.Count; idx++)
                    {
                        if (!string.IsNullOrWhiteSpace(searchParam.SearchColumnValue[idx]))
                        {
                            if (searchParam.SearchColumn[idx] == "Id") CustomerObj.Id = Convert.ToInt32(searchParam.SearchColumnValue[idx]);
                            else if (searchParam.SearchColumn[idx] == "FirstName") CustomerObj.FirstName = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "LastName") CustomerObj.LastName = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "FirstName2") CustomerObj.FirstName2 = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "LastName2") CustomerObj.LastName2 = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "Phone") CustomerObj.Phone = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "Email") CustomerObj.Email = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "Address") CustomerObj.Address = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "Zip") CustomerObj.Zip = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "City") CustomerObj.City = searchParam.SearchColumnValue[idx];
                        }
                    }
                }

            }
            return CustomerObj;
        }

        public List<Customer> GetCustomers(SearchParameter searchParam)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            
            List<Customer> result = null;
            
            if (searchParam != null)
            {
                Customer Customer = GetCustomerObject(searchParam);
                if (!searchParam.IsFilterValue)
                {
                    searchParam.TotalRecordCount = _context.Customers.Count();
                    result = _context.Customers.Where(c=>c.IsActive == true).OrderBy(x => x.Id)
                    .Skip(searchParam.StartIndex).Take((searchParam.PageSize > 0 ? searchParam.PageSize : searchParam.TotalRecordCount)).
                    ToList();
                }
                else
                {
                    result = _context.Customers.Where(c =>
                    c.IsActive == true &&
                    (Customer.Id == 0 ? 1 == 1 : c.Id == Customer.Id) &&
                    (string.IsNullOrEmpty(Customer.FirstName) ? 1 == 1 : c.FirstName.ToUpper().StartsWith(Customer.FirstName.ToUpper())) &&
                    (string.IsNullOrEmpty(Customer.LastName) ? 1 == 1 : c.LastName.ToUpper().StartsWith(Customer.LastName.ToUpper())) &&
                    (string.IsNullOrEmpty(Customer.FirstName2) ? 1 == 1 : c.FirstName2.ToUpper().StartsWith(Customer.FirstName2.ToUpper())) &&
                    (string.IsNullOrEmpty(Customer.LastName2) ? 1 == 1 : c.LastName2.ToUpper().StartsWith(Customer.LastName2.ToUpper())) &&
                    (string.IsNullOrEmpty(Customer.Address) ? 1 == 1 : c.Address.ToUpper().StartsWith(Customer.Address.ToUpper())) &&
                    (string.IsNullOrEmpty(Customer.Phone) ? 1 == 1 : c.Phone.ToUpper().StartsWith(Customer.Phone.ToUpper())) &&
                    (string.IsNullOrEmpty(Customer.Email) ? 1 == 1 : c.Email.ToUpper().StartsWith(Customer.Email.ToUpper())) &&
                    (string.IsNullOrEmpty(Customer.Zip) ? 1 == 1 : c.Zip.ToUpper().StartsWith(Customer.Zip.ToUpper())) &&
                    (string.IsNullOrEmpty(Customer.City) ? 1 == 1 : c.Zip.ToUpper().StartsWith(Customer.City.ToUpper()))
                    ).ToList();

                    searchParam.TotalRecordCount = result.Count();
                }
            }

            searchParam.SearchedCount = (!searchParam.IsFilterValue ? searchParam.TotalRecordCount : result.Count);

            return result;
        }

        public Customer GetCustomerById(int Id)
        {
            //  _context.Configuration.ProxyCreationEnabled = false;
            return _context.Customers.AsNoTracking().Where(c => c.IsActive == true && c.Id == Id).SingleOrDefault();
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
        #endregion

        #region Quote
        
        public decimal generateQuote(DateTime EffectiveDate, decimal PersonalProperty, decimal Deductible,
                                    decimal Liability, int CustomerId, int NoOfInstallments, 
                                    bool SendLandlord, ref int quoteId, out string ProposalNo)
        {
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
                    Liability = Liability,
                    Premium = Premium,
                    CustomerId = CustomerId,
                    NoOfInstallments = NoOfInstallments,
                    CreationDate = DateTime.Now,
                    SendLandLord = SendLandlord,
                    CompanyId = 1,
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
            ProposalNo = quoteObj.ProposalNumber;
            _context.SaveChanges();


            quoteId = quoteObj.Id;
            return Premium;
        }

        public List<QuoteDto> GetQuotes()
        {
            _context.Configuration.ProxyCreationEnabled = true;
            _context.Configuration.LazyLoadingEnabled = true;
            var quoteList = _context.Quotes.Where(c => c.IsActive == true && (c.Payments.Where(x => x.TransactionId == null).Any())).ToList();
            return quoteList.Select(x => new QuoteDto
            {
                Id = x.Id,
                ProposalNumber = x.ProposalNumber,
                PersonalProperty = x.PersonalProperty,
                Liability = x.Liability,
                Premium = x.Premium,
                EffectiveDate = x.EffectiveDate,
                NoOfInstallments = x.NoOfInstallments,
                SendLandLord = x.SendLandLord
            }).ToList();
        }
        public List<QuoteDto> GetQuotes(SearchParameter searchParam)
        {
            List<QuoteDto> result = null;
            List<Quote> items = null;
            if (searchParam != null)
            {
                Quote Quote = GetPolicyObject(searchParam);
                Decimal PersonalProperty = Convert.ToDecimal(Quote.PersonalProperty);
                Decimal Liability = Convert.ToDecimal(Quote.Liability);
                Decimal Premium = Convert.ToDecimal(Quote.Premium);
                 Int32 day = Convert.ToDateTime(Quote.EffectiveDate).Date.Day;
                Int32 month = Convert.ToDateTime(Quote.EffectiveDate).Date.Month;
                Int32 year = Convert.ToDateTime(Quote.EffectiveDate).Date.Year;

                if (!searchParam.IsFilterValue)
                {
                    searchParam.TotalRecordCount = _context.Quotes.Where(c => c.IsActive == true &&
                                                 ((c.Payments.Where(x => x.TransactionId != null).Any())
                                                 )).Count();

                    items = _context.Quotes.Where(c => c.IsActive == true &&
                                                 ((c.Payments.Where(x => x.TransactionId != null).Any())
                                                 )).OrderBy(x => x.Id)
                                                .Skip(searchParam.StartIndex).Take((searchParam.PageSize > 0 ?
                                                searchParam.PageSize : searchParam.TotalRecordCount)).
                                                ToList();

                }
                else
                {
                    items = _context.Quotes.Where(c => c.IsActive == true &&
                                                 ((c.Payments.Where(x => x.TransactionId != null).Any())
                                                 ) &&

                                                 (Quote.Id == 0 ? 1 == 1 : c.Id == Quote.Id) &&
                                                 (string.IsNullOrEmpty(Quote.ProposalNumber) ? 1 == 1 : c.ProposalNumber.ToUpper().StartsWith(Quote.ProposalNumber.ToUpper())) &&
                                                 (PersonalProperty == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)c.PersonalProperty).ToUpper().StartsWith(SqlFunctions.StringConvert((double)PersonalProperty).ToUpper())) &&
                                                 (Liability == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)c.Liability).ToUpper().StartsWith(SqlFunctions.StringConvert((double)Liability).ToUpper())) &&
                                                 (Premium == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)c.Premium).ToUpper().StartsWith(SqlFunctions.StringConvert((double)Premium).ToUpper())) &&
                                                 ((day == 1 && month == 1 && year == 1 ? 1 == 1 :
                                                 SqlFunctions.DatePart("dd",c.EffectiveDate) ==day &&
                                                 SqlFunctions.DatePart("mm", c.EffectiveDate) == month &&
                                                 SqlFunctions.DatePart("yyyy", c.EffectiveDate) == year))
                                                 ).ToList();
                    
                    searchParam.TotalRecordCount = items.Count();
                }
                result = items.Select(x => new QuoteDto
                {
                    Id = x.Id,
                    ProposalNumber = x.ProposalNumber,
                    PersonalProperty = x.PersonalProperty,
                    Liability = x.Liability,
                    Premium = x.Premium,
                    EffectiveDate = x.EffectiveDate,
                    NoOfInstallments = x.NoOfInstallments,
                    SendLandLord = x.SendLandLord
                }).ToList();
            }
            searchParam.SearchedCount = (!searchParam.IsFilterValue ? searchParam.TotalRecordCount : result.Count);
            return result;
        }
        public Quote GetQuoteById(int Id)
        {
            // _context.Configuration.ProxyCreationEnabled = false;
            return _context.Quotes.AsNoTracking().Where(q => q.IsActive == true && q.Id == Id).SingleOrDefault();
        }

        public void saveQuote(Quote quoteObj)
        {
            if (quoteObj != null && quoteObj.Id != 0)
            {
                _context.Entry(quoteObj).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }
        #endregion

        #region Policy
       
        private Quote GetPolicyObject(SearchParameter searchParam)
        {
            Quote Quote = new Quote();
            bool isOtherAnyParam = false;
            if (searchParam != null)
            {
                var isFilterValue = searchParam.SearchColumnValue.Any(e => !string.IsNullOrWhiteSpace(e));

                if ((searchParam.SearchColumn != null && searchParam.SearchColumn.Count > 0) &&
                    searchParam.SearchColumn.Count == searchParam.SearchColumnValue.Count - 1 && isFilterValue) // minus -1 means, skipping action column from search list
                {
                    var filterValueProp = new Dictionary<string, string>();
                    for (int idx = 0; idx < searchParam.SearchColumnValue.Count; idx++)
                    {
                        if (!string.IsNullOrWhiteSpace(searchParam.SearchColumnValue[idx]))
                            {
                                if (searchParam.SearchColumn[idx] == "Id") { Quote.Id = Convert.ToInt32(searchParam.SearchColumnValue[idx]); isOtherAnyParam = true; }
                                else if (searchParam.SearchColumn[idx] == "ProposalNumber") { Quote.ProposalNumber = searchParam.SearchColumnValue[idx]; isOtherAnyParam = true; }
                                else if (searchParam.SearchColumn[idx] == "PersonalProperty") { Quote.PersonalProperty = Convert.ToDecimal(searchParam.SearchColumnValue[idx]); isOtherAnyParam = true; }
                                else if (searchParam.SearchColumn[idx] == "Liability") { Quote.Liability = Convert.ToDecimal(searchParam.SearchColumnValue[idx]); isOtherAnyParam = true; }
                                else if (searchParam.SearchColumn[idx] == "Premium") { Quote.Premium = Convert.ToDecimal(searchParam.SearchColumnValue[idx]); isOtherAnyParam = true; }
                                else if (searchParam.SearchColumn[idx] == "EffectiveDate")
                                {
                                    DateTime tempDate;
                                    bool result = DateTime.TryParse(searchParam.SearchColumnValue[idx], out tempDate);
                                    if (result)
                                    {
                                        Quote.EffectiveDate = Convert.ToDateTime(searchParam.SearchColumnValue[idx]);
                                        isOtherAnyParam = true;
                                    }
                                }
                            //else if (searchParam.SearchColumn[idx] == "InsuredName") { Quote.ProposalNumber = searchParam.SearchColumnValue[idx]; isOtherAnyParam = true; }
                            //else if (searchParam.SearchColumn[idx] == "InsuredAddress") { Quote.ProposalNumber = searchParam.SearchColumnValue[idx]; isOtherAnyParam = true; }
                            //else if (searchParam.SearchColumn[idx] == "InsuredPhone") { Quote.ProposalNumber = searchParam.SearchColumnValue[idx]; isOtherAnyParam = true; }
                            //else if (searchParam.SearchColumn[idx] == "InsuredEmail") { Quote.ProposalNumber = searchParam.SearchColumnValue[idx]; isOtherAnyParam = true; }
                            //else if (searchParam.SearchColumn[idx] == "PolicyNumber") { Quote.ProposalNumber = searchParam.SearchColumnValue[idx]; isOtherAnyParam = true; }
                        }
                    }
                }
                searchParam.IsFilterValue = isFilterValue;
            }
            return Quote;
        }
        public List<QuoteDto> GetPolicies()
        {
            var policyList = _context.Quotes.Where(c => c.IsActive == true && 
            ((c.Payments.Where(x => x.TransactionId != null).Any()) || c.IsParkSitePolicy == true)).ToList();
            return policyList.Select(x => new QuoteDto
            {
                Id = x.Id,
                ProposalNumber = x.ProposalNumber,
                PersonalProperty = x.PersonalProperty,
                Liability = x.Liability,
                Premium = x.Premium,
                EffectiveDate = x.EffectiveDate,
                NoOfInstallments = x.NoOfInstallments,
                SendLandLord = x.SendLandLord
            }).ToList();

        }
        public List<QuoteDto> GetPolicies(SearchParameter searchParam)
        {
            List<QuoteDto> result = null;
            List<Quote> items = null;
            if (searchParam != null)
            {
                Quote Quote = GetPolicyObject(searchParam);
                Decimal PersonalProperty = Convert.ToDecimal(Quote.PersonalProperty);
                Decimal Liability = Convert.ToDecimal(Quote.Liability);
                Decimal Premium = Convert.ToDecimal(Quote.Premium);
                Int32 day = Convert.ToDateTime(Quote.EffectiveDate).Date.Day;
                Int32 month = Convert.ToDateTime(Quote.EffectiveDate).Date.Month;
                Int32 year = Convert.ToDateTime(Quote.EffectiveDate).Date.Year;

                string InsuredName = searchParam.SearchColumnValue[searchParam.SearchColumn.IndexOf("InsuredName")];
                string InsuredAddress = searchParam.SearchColumnValue[searchParam.SearchColumn.IndexOf("InsuredAddress")];
                string InsuredPhone = searchParam.SearchColumnValue[searchParam.SearchColumn.IndexOf("InsuredPhone")];
                string InsuredEmail = searchParam.SearchColumnValue[searchParam.SearchColumn.IndexOf("InsuredEmail")];
                //string PolicyNumber = searchParam.SearchColumnValue[searchParam.SearchColumn.IndexOf("PolicyNumber")];

                if (!searchParam.IsFilterValue)
                {
                    searchParam.TotalRecordCount = _context.Quotes.Where(c => c.IsActive == true &&
                                                 ((c.Payments.Where(x => x.TransactionId != null).Any())
                                                 || c.IsParkSitePolicy == true)).Count();

                    items = _context.Quotes.Include("Customer").Where(c => c.IsActive == true &&
                                                 ((c.Payments.Where(x => x.TransactionId != null).Any()) 
                                                 || c.IsParkSitePolicy == true)).OrderBy(x => x.Id)
                                                .Skip(searchParam.StartIndex).Take((searchParam.PageSize > 0 ? 
                                                searchParam.PageSize : searchParam.TotalRecordCount)).
                                                ToList();

                }
                else
                {
                    items = _context.Quotes.Include("Customer")                        
                        .Where(c => c.IsActive == true &&
                                                 ((c.Payments.Where(x => x.TransactionId != null).Any())
                                                 || c.IsParkSitePolicy == true) &&

                                                 (Quote.Id == 0 ? 1 == 1 : c.Id == Quote.Id) &&
                                                 (string.IsNullOrEmpty(Quote.ProposalNumber) ? 1 == 1 : c.ProposalNumber.ToUpper().StartsWith(Quote.ProposalNumber.ToUpper())) &&
                                                 (string.IsNullOrEmpty(InsuredName) ? 1==1 : c.Customer.FirstName.ToUpper().StartsWith(InsuredName.ToUpper()))&&
                                                 (string.IsNullOrEmpty(InsuredAddress) ? 1 == 1 : c.Customer.Address.ToUpper().StartsWith(InsuredAddress.ToUpper())) &&
                                                 (string.IsNullOrEmpty(InsuredPhone) ? 1 == 1 : c.Customer.Phone.ToUpper().StartsWith(InsuredPhone.ToUpper())) &&
                                                 (string.IsNullOrEmpty(InsuredEmail) ? 1 == 1 : c.Customer.Email.ToUpper().StartsWith(InsuredEmail.ToUpper())) &&                                                 
                                                 (PersonalProperty == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)c.PersonalProperty).ToUpper().StartsWith(SqlFunctions.StringConvert((double)PersonalProperty).ToUpper())) &&
                                                 (Liability == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)c.Liability).ToUpper().StartsWith(SqlFunctions.StringConvert((double)Liability).ToUpper())) &&
                                                 (Premium == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)c.Premium).ToUpper().StartsWith(SqlFunctions.StringConvert((double)Premium).ToUpper())) &&
                                                 ( (day==1 && month==1 && year==1 ? 1==1 :                                                 
                                                 SqlFunctions.DatePart("dd", c.EffectiveDate) == day &&
                                                 SqlFunctions.DatePart("mm", c.EffectiveDate) == month &&
                                                 SqlFunctions.DatePart("yyyy", c.EffectiveDate) == year))


                                                 ).ToList();

                    searchParam.TotalRecordCount = items.Count();
                }
                result = items.Select(x => new QuoteDto
                {
                    Id = x.Id,
                    ProposalNumber = x.ProposalNumber,
                    PersonalProperty = x.PersonalProperty,
                    Liability = x.Liability,
                    Premium = x.Premium,
                    EffectiveDate = x.EffectiveDate,
                    NoOfInstallments = x.NoOfInstallments,
                    SendLandLord = x.SendLandLord,
                    InsuredName = (x.Customer != null ? x.Customer.FirstName + " " + x.Customer.FirstName : string.Empty),
                    InsuredAddress = (x.Customer != null ? x.Customer.Address  : string.Empty),
                    InsuredEmail = (x.Customer != null ? x.Customer.Email  : string.Empty),
                    InsuredPhone = (x.Customer != null ? x.Customer.Phone  : string.Empty),


                }).ToList();
            }
            searchParam.SearchedCount = (!searchParam.IsFilterValue ? searchParam.TotalRecordCount : result.Count);
            return result;
        }
        public Model.Payment GetPolicyReceiptById(int quoteId)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Payments.AsNoTracking().FirstOrDefault(p => p.IsActive == true && p.RentalQuoteId == quoteId);
        }
        #endregion

        #region Payment
        public List<Model.Payment> GetPayments()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Payments.AsNoTracking().Where(c => c.IsActive == true).ToList();
        }
        private Model.Payment GetPaymentObject(SearchParameter searchParam)
        {
            Model.Payment payment = new Model.Payment();
            if (searchParam != null)
            {
                var isFilterValue = searchParam.SearchColumnValue.Any(e => !string.IsNullOrWhiteSpace(e));

                searchParam.IsFilterValue = isFilterValue;

                if ((searchParam.SearchColumn != null && searchParam.SearchColumn.Count > 0) &&
                    searchParam.SearchColumn.Count == searchParam.SearchColumnValue.Count - 1 && isFilterValue) // minus -1 means, skipping action column from search list
                {
                    var filterValueProp = new Dictionary<string, string>();
                    for (int idx = 0; idx < searchParam.SearchColumnValue.Count; idx++)
                    {
                        if (!string.IsNullOrWhiteSpace(searchParam.SearchColumnValue[idx]))
                        {
                            if (searchParam.SearchColumn[idx] == "Id") payment.Id = Convert.ToInt32(searchParam.SearchColumnValue[idx]);
                            else if (searchParam.SearchColumn[idx] == "TransactionId") payment.TransactionId = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "ResponseCode") payment.ResponseCode = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "ApprovalCode") payment.ApprovalCode = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "Amount") payment.Amount = Convert.ToDecimal(searchParam.SearchColumnValue[idx]);
                            else if (searchParam.SearchColumn[idx] == "CreationDate")
                            {
                                DateTime tempDate;
                                bool result = DateTime.TryParse(searchParam.SearchColumnValue[idx], out tempDate);
                                if (result)
                                {
                                    payment.CreationDate = Convert.ToDateTime(searchParam.SearchColumnValue[idx]);
                                }
                            }
                        }
                    }
                }

            }
            return payment;
        }
        public List<Model.Payment> GetPayments(SearchParameter searchParam)
        {
            List<Model.Payment> result = null;
            _context.Configuration.ProxyCreationEnabled = false;
            _context.Configuration.LazyLoadingEnabled = false;
            if (searchParam != null)
            {
                Model.Payment payment = GetPaymentObject(searchParam);
                
                Decimal Amount = Convert.ToDecimal(payment.Amount);
                Int32 day = Convert.ToDateTime(payment.CreationDate).Date.Day;
                Int32 month = Convert.ToDateTime(payment.CreationDate).Date.Month;
                Int32 year = Convert.ToDateTime(payment.CreationDate).Date.Year;

                if (!searchParam.IsFilterValue)
                {
                    searchParam.TotalRecordCount = _context.Payments.Where(c => c.IsActive == true).Count();

                    result = _context.Payments.Where(c => c.IsActive == true
                                                ).OrderBy(x => x.Id)
                                                .Skip(searchParam.StartIndex).Take((searchParam.PageSize > 0 ?
                                                searchParam.PageSize : searchParam.TotalRecordCount)).
                                                ToList();

                }
                else
                {
                    result = _context.Payments.Where(c => c.IsActive == true &&
                                                 (payment.Id == 0 ? 1 == 1 : c.Id == payment.Id) &&
                                                 (string.IsNullOrEmpty(payment.TransactionId) ? 1 == 1 : c.TransactionId.ToUpper().StartsWith(payment.TransactionId.ToUpper())) &&
                                                 (string.IsNullOrEmpty(payment.ResponseCode) ? 1 == 1 : c.ResponseCode.ToUpper().StartsWith(payment.ResponseCode.ToUpper())) &&
                                                 (string.IsNullOrEmpty(payment.ApprovalCode) ? 1 == 1 : c.ApprovalCode.ToUpper().StartsWith(payment.ApprovalCode.ToUpper())) &&
                                                 (Amount == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)c.Amount).ToUpper().StartsWith(SqlFunctions.StringConvert((double)Amount).ToUpper())) &&
                                                 ((day == 1 && month == 1 && year == 1 ? 1 == 1 :
                                                 SqlFunctions.DatePart("dd", c.CreationDate) == day &&
                                                 SqlFunctions.DatePart("mm", c.CreationDate) == month &&
                                                 SqlFunctions.DatePart("yyyy", c.CreationDate) == year))
                                                 ).ToList();

                    searchParam.TotalRecordCount = result.Count();
                }                
            }
            searchParam.SearchedCount = (!searchParam.IsFilterValue ? searchParam.TotalRecordCount : result.Count);
            return result;
        }
        #endregion

        #region Others
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
        #endregion

        #region Reporting

        public List<OrderDto> GetListOrder(string startDate, string endDate)
        {
            List<Model.Payment> items = null;

            _context.Configuration.ProxyCreationEnabled = false;
            if (string.IsNullOrWhiteSpace(startDate) && string.IsNullOrWhiteSpace(endDate))
                items = _context.Payments.Include("Customer").
                    Include("Quote").
                    Include("Quote.Company").
                    Where(x => x.TransactionId != null).ToList();
            else if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                var startDt = Master.MasterServiceFacade.GetStringAsDateFormat(startDate);
                var endDt = Master.MasterServiceFacade.GetStringAsDateFormat(endDate).AddDays(1);
                items = _context.Payments.Include("Customer").
                    Include("Quote").
                    Include("Quote.Company").
                    Where(x => x.TransactionId != null && (x.CreationDate >= startDt && x.CreationDate <= endDt)).ToList();
            }


            var rtnItems = items.Select(x => new OrderDto()
            {
                OrderId = x.Id,

                ApprovalCode = x.ApprovalCode,
                ApprovalMessage = x.ApprovalMessage,
                CreatedBy = x.CreatedBy,
                //CreationDate = x.CreationDate.HasValue ? x.CreationDate.Value.Date : DateTime.MinValue,
                CreationDateStr = x.CreationDate.HasValue ? Master.MasterServiceFacade.GetDateFormatAsString(x.CreationDate.Value) : string.Empty,
                ErrorMessage = (x.ErrorMessage != null ? x.ErrorMessage : string.Empty),
                ResponseCode = x.ResponseCode,
                TransactionId = x.TransactionId,

                RenterId = x.RentalQuoteId.Value,
                CompanyId = x.Quote != null && x.Quote.Company != null ? x.Quote.CompanyId.Value : 0,
                CompanyName = x.Quote != null && x.Quote.Company != null ? x.Quote.Company.Name : string.Empty,
                ProposalNumber = x.Quote != null ? x.Quote.ProposalNumber : string.Empty,

                CustomerId = x.CustomerId.Value,
                CustomerName = x.Customer != null ? x.Customer.FirstName + " " + x.Customer.LastName : string.Empty

            }).ToList();
            return rtnItems;
        }

        public List<OrderDto> GetListPremiums(int stateId, string zipCode, string startDate, string endDate)
        {
            List<Model.Payment> items = null;

            _context.Configuration.ProxyCreationEnabled = false;
            if (stateId == 0 & string.IsNullOrWhiteSpace(zipCode) && string.IsNullOrWhiteSpace(startDate) && string.IsNullOrWhiteSpace(endDate))
                items = _context.Payments.Where(x => x.TransactionId != null).ToList();

            if (stateId > 0 & !string.IsNullOrWhiteSpace(zipCode) && !string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                var startDt = Master.MasterServiceFacade.GetStringAsDateFormat(startDate);
                var endDt = Master.MasterServiceFacade.GetStringAsDateFormat(endDate).AddDays(1);

                items = _context.Payments
                                      .Where(x => x.TransactionId != null &&
                                            (x.Customer != null && x.Customer.StateId != null && x.Customer.StateId == stateId) &&
                                            (x.Customer != null && x.Customer.Zip == zipCode) &&
                                            (x.CreationDate != null && x.CreationDate.Value >= startDt && x.CreationDate.Value <= endDt)).ToList();

            }
            else if (stateId > 0 || !string.IsNullOrWhiteSpace(zipCode) || (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate)))
            {
                DateTime startDt = DateTime.MinValue, endDt = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
                {
                    startDt = Master.MasterServiceFacade.GetStringAsDateFormat(startDate);
                    endDt = Master.MasterServiceFacade.GetStringAsDateFormat(endDate).AddDays(1);
                }
                else
                    startDate = endDate = null;

                items = _context.Payments
                                      .Where(x => x.TransactionId != null &&
                                            (stateId == 0 || (x.Customer != null && x.Customer.StateId != null && x.Customer.State.Id == stateId)) &&
                                            ((zipCode == null || zipCode.Trim() == string.Empty) || (x.Customer != null && x.Customer.Zip == zipCode)) &&
                                            ((startDate == null || (x.CreationDate.Value >= startDt)) &&
                                            (endDate == null || (x.CreationDate.Value <= endDt)))).ToList();
            }

            List<OrderDto> rtnItems = null;
            if (items != null && items.Count > 0)
            {
                rtnItems = new List<OrderDto>();
                items.ForEach((rec) =>
                {
                    var totPremium = _context.Payments.Where(p => rec.CustomerId != null && p.CustomerId == rec.CustomerId).Sum(tp => tp.Amount);
                    var customer = _context.Customers.SingleOrDefault(c => c.Id == rec.CustomerId);
                    var quote = _context.Quotes.SingleOrDefault(q => q.Id == rec.RentalQuoteId.Value);
                    rtnItems.Add(new OrderDto()
                    {
                        OrderId = rec.Id,

                        TotalPremium = totPremium,
                        ApprovalCode = rec.ApprovalCode,
                        ApprovalMessage = rec.ApprovalMessage,
                        CreatedBy = rec.CreatedBy,
                        //CreationDate = rec.CreationDate != null ? rec.CreationDate.Value.Date : DateTime.MinValue,
                        CreationDateStr = rec.CreationDate != null ? Master.MasterServiceFacade.GetDateFormatAsString(rec.CreationDate.Value) : string.Empty,
                        ErrorMessage = rec.ErrorMessage,
                        ResponseCode = rec.ResponseCode,
                        TransactionId = rec.TransactionId,

                        RenterId = rec.RentalQuoteId.Value,
                        CompanyId = quote != null && quote.Company != null ? quote.CompanyId.Value : 0,
                        CompanyName = quote != null && quote.Company != null ? quote.Company.Name : "",
                        ProposalNumber = quote != null ? quote.ProposalNumber : "",

                        CustomerId = rec.CustomerId.Value,
                        CustomerName = customer != null ? customer.FirstName + " " + customer.LastName : "",
                        ZipCode = customer != null ? customer.Zip : "",
                        Phone = customer != null ? customer.Phone : "",
                        Email = customer != null ? customer.Email : "",

                    });
                });
            }

            return rtnItems;
        }

        #endregion
    }
}
