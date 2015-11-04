using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace mobilehome.insure.Models.JQDataTable
{
    /// <summary>
    /// Contains sorting helpers for In Memory collections.
    /// </summary>
    public static class CollectionHelper
    {
        public static IOrderedEnumerable<TSource> Sort<TSource, TKey>(this IEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.OrderBy(keySelector);
            }

            return items.OrderByDescending(keySelector);
        }

        public static IOrderedEnumerable<TSource> Sort<TSource, TKey>(this IOrderedEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.ThenBy(keySelector);
            }

            return items.ThenByDescending(keySelector);
        }
    }

    public static class GenericFilterHelper<T>
        where T : class
    {
        private static T entity;

        public static IList<T> GetFilteredRecords(
            Func<List<T>> runTimeMethod,
            int startIndex, int pageSize,
            ReadOnlyCollection<SortedColumn> sortedColumns,
            out int totalRecordCount,
            out int searchRecordCount,
            string searchString,
            ReadOnlyCollection<string> searchColumnValues,
            List<string> properties)
        {
            var types = runTimeMethod();
            //if (typeof(T).FullName.Equals("MobileHome.Insure.Model.Customer", StringComparison.OrdinalIgnoreCase))
            //{

            //}
            totalRecordCount = types.Count;


            var isFilterValue = searchColumnValues.Any(e => !string.IsNullOrWhiteSpace(e));
            if ((properties != null && properties.Count > 0) && properties.Count == searchColumnValues.Count - 1 && isFilterValue) // minus -1 means, skipping action column from search list
            {
                var filterValueProp = new Dictionary<string, string>();
                for (int idx = 0; idx < searchColumnValues.Count; idx++)
                {
                    if (!string.IsNullOrWhiteSpace(searchColumnValues[idx]))
                    {
                        filterValueProp.Add(properties[idx], searchColumnValues[idx]);
                    }
                }

                IEnumerable<T> result = null;
                foreach (var item in filterValueProp)
                {
                    //check property in type, whether its exist or not
                    var checkProp = typeof(T).GetProperties().FirstOrDefault(p => p.Name.Equals(item.Key, StringComparison.OrdinalIgnoreCase));
                    if (checkProp == null)
                        continue;

                    result = WhereQuery<T>((result == null ? types : result), item.Key, item.Value);
                    if (result != null && result.Count() > 0)
                        continue;
                }

                if (result != null && result.Count() > 0)
                {
                    types = result.ToList();
                }
            }

            searchRecordCount = types.Count;
            IEnumerable<T> sortedEntityTypes = null;
            foreach (var sortedColumn in sortedColumns)
            {
                if (sortedColumn.Direction == SortingDirection.Ascending)
                {
                    sortedEntityTypes = types.OrderBy(sortedColumn.PropertyName);
                }
                else
                {
                    sortedEntityTypes = types.OrderByDescending(entity => entity.GetType().GetProperty(sortedColumn.PropertyName).GetValue(entity, null));
                }
            }

            return sortedEntityTypes.Skip(startIndex).Take((pageSize > 0 ? pageSize : totalRecordCount)).ToList();
            //return types.Skip(startIndex).Take(pageSize).ToList();
        }

        public static IEnumerable<T> WhereQuery<T>(IEnumerable<T> source, string columnName, string propertyValue)
        {
            var filtered = source.Where(m =>
                                            {
                                                var prop = m.GetType().GetProperty(columnName);
                                                if (prop != null && prop.GetValue(m, null) != null && prop.GetValue(m, null).ToString().StartsWith(propertyValue))
                                                {
                                                    return true;
                                                }
                                                else
                                                    return false;
                                            });
            return (filtered != null && filtered.Count() > 0) ? filtered : null;
        }
    }


    //public static class InMemoryCustomersRepository
    //{
    //    private static IList<Customer> GetAllCustomers()
    //    {
    //        var customers = new List<Customer>();
    //        customers.Add(new Customer(firstName: "Justin", lastName: "Michaels", age: 27, phoneNumber: "(123) 555-5555", birthday: Convert.ToDateTime("12/03/1984")));
    //        customers.Add(new Customer(firstName: "James", lastName: "Halk", age: 21, phoneNumber: "(123) 555-5554", birthday: Convert.ToDateTime("12/01/1990")));
    //        customers.Add(new Customer(firstName: "Lauren", lastName: "Waddams", age: 22, phoneNumber: "(123) 555-5553", birthday: Convert.ToDateTime("05/09/1990")));
    //        customers.Add(new Customer(firstName: "Dan", lastName: "Callahan", age: 35, phoneNumber: "(123) 555-5552", birthday: Convert.ToDateTime("12/09/76")));
    //        customers.Add(new Customer(firstName: "Kevin", lastName: "Kentucky", age: 40, phoneNumber: "(123) 555-5551", birthday: Convert.ToDateTime("12/08/1972")));
    //        customers.Add(new Customer(firstName: "Mike", lastName: "Peterson", age: 24, phoneNumber: "(123) 555-5550", birthday: Convert.ToDateTime("1/03/1988")));
    //        customers.Add(new Customer(firstName: "Tom", lastName: "Gun", age: 59, phoneNumber: "(123) 555-5559", birthday: Convert.ToDateTime("1/23/1953")));
    //        customers.Add(new Customer(firstName: "Erich", lastName: "Milton", age: 54, phoneNumber: "(123) 555-5558", birthday: Convert.ToDateTime("4/03/1958")));
    //        customers.Add(new Customer(firstName: "Jason", lastName: "Ralph", age: 27, phoneNumber: "(123) 555-5557", birthday: Convert.ToDateTime("11/03/1984")));
    //        customers.Add(new Customer(firstName: "Jarold", lastName: "Interface", age: 39, phoneNumber: "(123) 555-5556", birthday: Convert.ToDateTime("10/03/1972")));
    //        customers.Add(new Customer(firstName: "John", lastName: "Thompson21", age: 27, phoneNumber: "(123) 555-5545", birthday: Convert.ToDateTime("12/31/1984")));
    //        return customers;
    //    }

    //    public static IList<Customer> GetCustomers(int startIndex,
    //        int pageSize,
    //        ReadOnlyCollection<SortedColumn> sortedColumns,
    //        out int totalRecordCount,
    //        out int searchRecordCount,
    //        string searchString)
    //    {
    //        var customers = GetAllCustomers();

    //        totalRecordCount = customers.Count;

    //        if (!string.IsNullOrWhiteSpace(searchString))
    //        {
    //            customers = customers.Where(c => c.FirstName.ToLower().Contains(searchString.ToLower())
    //                || c.LastName.ToLower().Contains(searchString.ToLower())).ToList();
    //        }

    //        searchRecordCount = customers.Count;

    //        IOrderedEnumerable<Customer> sortedCustomers = null;
    //        foreach (var sortedColumn in sortedColumns)
    //        {
    //            switch (sortedColumn.PropertyName)
    //            {
    //                case "FirstName":
    //                    sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.FirstName)
    //                        : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.FirstName);
    //                    break;
    //                case "LastName":
    //                    sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.LastName)
    //                        : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.LastName);
    //                    break;
    //                case "Age":
    //                    sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.Age)
    //                        : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.Age);
    //                    break;
    //                case "PhoneNumber":
    //                    sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.PhoneNumber)
    //                        : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.PhoneNumber);
    //                    break;
    //                case "Birthday":
    //                    sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.Birthday)
    //                        : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.Birthday);
    //                    break;
    //            }
    //        }

    //        return sortedCustomers.Skip(startIndex).Take(pageSize).ToList();
    //    }
    //}
}